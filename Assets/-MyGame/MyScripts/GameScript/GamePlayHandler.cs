using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEditor;

public class GamePlayHandler : MonoBehaviour
{
    #region Creating Instance
    private static GamePlayHandler _instance;

    public static GamePlayHandler instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GamePlayHandler>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion

    [SerializeField] GameObject _rocketObj;
    [SerializeField] GameObject _rocketStartPos;
    [SerializeField] GameObject _cashOutSignPos;
    [SerializeField] GameObject _rocketEndPos;
    [SerializeField] GameObject _rocketEndMovePos;
    [SerializeField] Text _currentElapsedTimeTxt;
    [SerializeField] Text _currentMultiplierTxt;
    [SerializeField] GameObject _cashoutPlayerSign;
    [SerializeField] AnimationCurve _exponentialCurve;
    [SerializeField] GameObject _curveObject;

    [SerializeField] bool _iWonGame;

    public Action startGameExecutionAction;
    //[ShowOnly]
    [SerializeField] float _currentMultiplierPoint;
    [ShowOnly]
    [SerializeField] float _currentGameTime;
    [ShowOnly]
    [SerializeField] float _currentGameTimeToShow;
    [ShowOnly]
    [SerializeField] float _timeToStartCrashPoint;
    [ShowOnly]
    [SerializeField] float _finalCrashPoint;
    bool _isGameCrashed;
    PhotonView _photonView;

    float _gameResetDelayTime = 5f;

    [SerializeField] Text _debugFinalPtTxt;
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _finalCrashPoint = 0;
        _timeToStartCrashPoint = 2f;
        ResetValuesBeforeGameStart();


    }
    float _currentGameRestDelayTime = 0;
    private void Update()
    {
        if (_currentGameRestDelayTime > 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _currentGameRestDelayTime -= Time.deltaTime;
                _photonView.RPC(nameof(GameResetDelayTimeRPC), RpcTarget.All, _currentGameRestDelayTime);
                if (_currentGameRestDelayTime <= 0.2f)
                {
                    _currentGameRestDelayTime = -1f;
                    GameResetManager.instance.ResetWholeGame();
                }
            }
        }
        if (startGameExecutionAction == null)
            return;
        if (PhotonNetwork.IsMasterClient)
        {
            startGameExecutionAction?.Invoke();
        }
        BettingManager.instance.OnAutoCashOutCall(_currentMultiplierPoint);
    }
    float _crashpointFromServer = 0f;
    public void StartPlayGame()
    {
        _currentGameTime += Time.deltaTime;
        if (_crashpointFromServer >= 1)
        {
            _currentMultiplierPoint += (Time.deltaTime / 4.3f);
            _currentGameTimeToShow += Time.deltaTime;
            if (_currentMultiplierPoint >= _finalCrashPoint)
            {
                _isGameCrashed = true;
            }
            _photonView.RPC(nameof(UpdateValuesToAllPlayersOnNetwork), RpcTarget.All, _currentGameTime, _currentGameTimeToShow, _currentMultiplierPoint, _isGameCrashed, _finalCrashPoint);
        }
    }
    //void MoveRocketUpDown

    RectTransform _rocketRectTransform;
    float _rocketX, _rocketY;
    bool _isWaitingToStart;
    private Tween _currentTween;
    [PunRPC]
    public void UpdateValuesToAllPlayersOnNetwork(float currentGameTime, float currentGameTimeToShow, float currentCrashPoint, bool isGameCrashed, float cp)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _currentMultiplierPoint = currentCrashPoint;
            _currentGameTime = currentGameTime;
            _currentGameTimeToShow = currentGameTimeToShow;
            _finalCrashPoint = cp;
        }
        _debugFinalPtTxt.text = _finalCrashPoint.ToString();
        if (!_rocketObj.activeInHierarchy)
            _rocketObj.SetActive(true);
        _currentElapsedTimeTxt.text = _currentGameTimeToShow.ToString("F0");
        _currentMultiplierTxt.text = _currentMultiplierPoint.ToString("F2") + "x";
        _isGameCrashed = isGameCrashed;
        if (_rocketRectTransform == null)
            _rocketRectTransform = _rocketObj.GetComponent<RectTransform>();
        _rocketX = _currentGameTimeToShow * 50f;
        _rocketY = _currentMultiplierPoint * 60f;

        _rocketRectTransform = _rocketObj.GetComponent<RectTransform>();

        if (_currentMultiplierPoint > LocalSettings.GRAPH_Y_AXIS_LIMIT)
            ScaleBuilder.instance.DrawLinePointsY(_currentMultiplierPoint);


        if (_isWaitingToStart)
        {
            MoveRocketToTarget(_rocketObj, _rocketStartPos, _rocketEndPos, 12, _exponentialCurve);
            _isWaitingToStart = false;
        }
        if (isGameCrashed)
        {
            GameCrash();
        }
    }
    [PunRPC]
    public void GameResetDelayTimeRPC(float delayTime)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            _currentGameRestDelayTime = delayTime;
        }
    }

    public void RocketPosXY(out RectTransform rocketRecTransform)
    {
        rocketRecTransform = _rocketRectTransform;
    }
    public float GetCurrentMultiplierPointOnCashOut()
    {
        return _currentMultiplierPoint;
    }
    void GameCrash()
    {
        Debug.LogError(" 2 _______game crashed: ");

        startGameExecutionAction = null;
        _rocketObj.SetActive(false);
        _curveObject.SetActive(false);

        _currentGameRestDelayTime = LocalSettings.GAME_RESET_DELAY_TIME;
        GameResetManager.instance.GetValuesOnGameCrash();
        GameObject blast = GameManager.instance.GetBlastPrefab();
        LocalSettings.SetPosAndRect(blast, _rocketObj.GetComponent<RectTransform>(), _rocketObj.transform);
        blast.transform.SetParent(_rocketObj.transform.parent.parent);
        blast.transform.position = _rocketObj.transform.position;
        blast.SetActive(true);
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
            Debug.Log("Rocket movement stopped!");
        }
        if (_currentTweenUpDown != null && _currentTweenUpDown.IsActive())
        {
            _currentTweenUpDown.Kill();
        }
        LeaderBoardHandler.instance.SortLeaderboard();
    }
    public bool IsGameCrashed()
    {
        return _isGameCrashed;
    }

    public void ResetValuesBeforeGameStart()
    {
        //_rocketEndMovePos.transform.position = _rocketEndPos.transform.position;
        _iWonGame = false;
        _isWaitingToStart = true;
        _crashpointFromServer = 0;
        _currentMultiplierPoint = 1;
        _currentGameTime = 0;
        _currentGameTimeToShow = 0;
        _finalCrashPoint = 0;
        _debugFinalPtTxt.text = _finalCrashPoint.ToString();

        _isGameCrashed = false;
        _rocketObj.transform.position = _rocketStartPos.transform.position;
        LocalSettings.SetPosAndRect(_rocketObj, _rocketStartPos.GetComponent<RectTransform>(), _rocketStartPos.transform.parent);
        _rocketObj.SetActive(true);
        _curveObject.SetActive(true);
        _signsOfPlayers = DestroyAndClearList(_signsOfPlayers);
        _currentMultiplierTxt.text = "1.00x";

        //ScaleBuilder.instance.DrawLinePointsX(LocalSettings.GRAPH_X_AXIS_LIMIT);
        ScaleBuilder.instance.DrawLinePointsY(LocalSettings.GRAPH_Y_AXIS_LIMIT);
        //Debug.LogError("Game is resetting from game play handler");
    }

    List<GameObject> _signsOfPlayers = new List<GameObject>();
    public GameObject GetCashOutPlayerSign()
    {
        GameObject obj = Instantiate(_cashoutPlayerSign);
        obj.SetActive(true);
        LocalSettings.SetPosAndRect(obj, _cashOutSignPos.GetComponent<RectTransform>(), _cashOutSignPos.transform.parent);
        _signsOfPlayers.Add(obj);
        return obj;
    }
    public List<GameObject> DestroyAndClearList(List<GameObject> list)
    {
        if (list == null)
            return list = new List<GameObject>();
        if (list.Count == 0)
            return list;
        foreach (GameObject obj in list)
        {
            if (obj != null)
                Destroy(obj);
        }
        list.Clear();
        return list;
    }

    public void MoveRocketToTarget(GameObject rocket, GameObject startPoint, GameObject endPoint, float duration, AnimationCurve exponentialCurve)
    {
        rocket.transform.position = startPoint.transform.position;
        _currentTween = rocket.transform.DOMove(endPoint.transform.position, duration)
                                       .SetEase(exponentialCurve)
                                       .OnComplete(OnMovementComplete);
    }
    private Tween _currentTweenUpDown;

    void MoveRocketUp(bool isDown)
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            _currentTween = null;
        }
        if (isDown)
        {
            _rocketObj.transform.position = _rocketEndPos.transform.position;
            _currentTweenUpDown = _rocketObj.transform.DOMove(_rocketEndMovePos.transform.position, 0.4f)
                                                        .SetEase(Ease.InSine)
                                                        .OnComplete(() => MoveRocketUp(false));
        }
        else
        {
            _currentTweenUpDown = _rocketObj.transform.DOMove(_rocketEndPos.transform.position, 4f)
                                                        .SetEase(Ease.Linear)
                                                        .OnComplete(() => MoveRocketUp(true));
        }
    }

    private Vector2 WorldToUIPosition(Vector3 worldPosition, RectTransform canvasRect)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, Camera.main, out Vector2 localPoint);
        return localPoint;
    }
    private void OnMovementComplete()
    {
        Debug.Log("Rocket reached the target!");
        MoveRocketUp(true);
    }

    public void GetCrashPointFromServer()
    {
        if (PhotonNetwork.IsMasterClient)
            GetJson.instance.GetJsonFromServer(APIStrings.getCrashPointAPIURL, GetCrashpointFromServer);
    }

    void GetCrashpointFromServer(string jsonResponse, bool isSuccess)
    {
        if (isSuccess)
        {
            CrashPointGetCls crashPointGetCls = JsonConvert.DeserializeObject<CrashPointGetCls>(jsonResponse);
            _crashpointFromServer = crashPointGetCls.number;
            //_crashpointFromServer = UnityEngine.Random.Range(4, 7);
            _finalCrashPoint = _crashpointFromServer;
            _debugFinalPtTxt.text = _finalCrashPoint.ToString();

        }
        else
        {
            Debug.LogError("Fail to get Crash point: ");
            GetCrashPointFromServer();
        }
    }
}


public class CrashPointGetCls
{
    public float number { get; set; }
    public string message { get; set; }
}

