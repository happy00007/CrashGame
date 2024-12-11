using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Creating Instance
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GameManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion


    [SerializeField] GameObject _loadingPanel;

    [SerializeField] GameObject _messagePanelPrefab;
    [SerializeField] Text _messageTxt;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] RectTransform _playerRectPos;
    [SerializeField] GameObject _waitingTimeBox;
    [SerializeField] GameObject _gamePlayBox;
    [SerializeField] GameObject _currentMultiplierBox;
    [SerializeField] GameObject _blastPrefabSprite;


    public List<GameObject> playersList;
    public List<GameObject> playingList;
    PlayerInfo _myPlayerInfo;
    public PlayerInfo GetMyPlayer()
    {
        return _myPlayerInfo;
    }

    public void SetMyPlayer(PlayerInfo playerInfo)
    {
        _myPlayerInfo = playerInfo;
    }

    public static bool isPlayerLogedIn;
    void Start()
    {

    }

    public void AddPlayerToList(GameObject player)
    {
        playersList.Add(player);
    }
    public void CreateThePlayer()
    {
        GameObject plyr = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity);
        plyr.SetActive(true);

    }

    public void SetPlayerPos(GameObject go)
    {
        LocalSettings.SetPosAndRect(go, _playerRectPos, _playerRectPos.transform.parent);
    }
    public void ShowLoadingPanel(bool isShow)
    {
        _loadingPanel.SetActive(isShow);
    }

    public void ShowMessage(string message)
    {
        _messageTxt.text = message;
        _messagePanelPrefab.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Successfully joined room: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log("<color=green>Room Joined successfully</color>");
        CreateThePlayer();
    }


    public void ShowWaitingOrMultiPlierBoxInGame(bool isWaiting)
    {
        _waitingTimeBox.SetActive(isWaiting);
        _currentMultiplierBox.SetActive(!isWaiting);
        _gamePlayBox.SetActive(!isWaiting);
    }

    public GameObject GetBlastPrefab()
    {
        return Instantiate(_blastPrefabSprite);
    }

    #region When master client goes to background

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Client went into the background.");

            // Optionally transfer Master Client role manually
            TransferMasterClient();
        }
    }

    void TransferMasterClient()
    {
        // Find a suitable new Master Client
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsMasterClient)
            {
                PhotonNetwork.SetMasterClient(player);
                Debug.Log($"Transferred Master Client role to: {player.NickName}");
                break;
            }
        }
    }


    #endregion
}
