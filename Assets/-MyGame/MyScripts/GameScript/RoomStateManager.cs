using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using static RoomNPlayerState;

public class RoomStateManager : MonoBehaviourPunCallbacks
{
    #region Creating Instance
    private static RoomStateManager _instance;
    public static RoomStateManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<RoomStateManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;

    }
    #endregion


    ROOMSTATE _currentRoomState;

    void Start()
    {

    }

    public ROOMSTATE getCurrentRoomState => _currentRoomState;


    public void UpdateCurrentRoomState(ROOMSTATE state)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateRoomStateProperty(state);
            photonView.RPC(nameof(UpdateThisStateOnNetwork), RpcTarget.All, state, "");
        }
    }

    [PunRPC]
    public void UpdateThisStateOnNetwork(ROOMSTATE state, string info)
    {

        _currentRoomState = state;
        Debug.Log("Current State is Set to " + state);
        OnUpdateCurrentState(state, info);
    }
    void UpdateRoomStateProperty(ROOMSTATE state)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            LocalSettings.GetCurrentRoom.SetRoomStateProperty(LocalSettings.ROOM_STATE, state);
    }

    void OnUpdateCurrentState(ROOMSTATE state, string infoText)
    {
        switch (state)
        {
            case ROOMSTATE.Waiting:
                TriggerStateWaitingForPlayers();
                break;

            case ROOMSTATE.GameIsPlaying:
                TriggerStateGameIsPlaying();
                break;

        }
    }

    void TriggerStateWaitingForPlayers()
    {
        GamePlayHandler.instance.ResetValuesBeforeGameStart();
        GameManager.instance.ShowWaitingOrMultiPlierBoxInGame(true);
        //GameStartManager.instance.GetDelayTimeBetweenRounds();
        BettingManager.instance.ActivateBettingSection(true);
        BettingManager.instance.ResetThingsBettingManager();
    }

    void TriggerStateGameIsPlaying()
    {
        LeaderBoardHandler.instance.ClearLeaderBoardValues();
        GameManager.instance.ShowWaitingOrMultiPlierBoxInGame(false);
        BettingManager.instance.ActivateBettingSection(false);
        SendBetAmountAndCashOutPoint();
        Debug.LogError("Game is started___________________________");
        if (GameManager.instance.GetMyPlayer() != null)
            GameManager.instance.GetMyPlayer().UpdatePlayerStateOnNetwork(PLAYERSTATE.GameIsPlaying);
        else
            Debug.LogError("MyPlayer is null");
        GamePlayHandler.instance.startGameExecutionAction += GamePlayHandler.instance.StartPlayGame;
        //if (PhotonNetwork.IsMasterClient)
        {
            GamePlayHandler.instance.GetCrashPointFromServer();
        }
    }
    public void SetRoomStateFromNetworkCustomProperty()
    {
        UpdateCurrentRoomState(LocalSettings.GetCurrentRoom.GetRoomStateProperty(LocalSettings.ROOM_STATE));
    }
    public bool isGameStarted => _currentRoomState == ROOMSTATE.GameIsPlaying;

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
            SetRoomStateFromNetworkCustomProperty();
        else
        {
            UpdateCurrentRoomState(ROOMSTATE.Waiting);
        }
    }

    public void SendBetAmountAndCashOutPoint()
    {
        BettingManager.instance.SendBetAmountToServer();
    }
}
