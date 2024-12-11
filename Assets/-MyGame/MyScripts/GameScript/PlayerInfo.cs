using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static RoomNPlayerState;

public class PlayerInfo : MonoBehaviourPunCallbacks
{
    PhotonView _photonView;
    [SerializeField] Text _playerNameTxt;
    [SerializeField] PlayerState _playerState;
    void Start()
    {
        //GameManager.instance.SetPlayerPos(gameObject);
        _photonView = GetComponent<PhotonView>();
        _playerState = GetComponent<PlayerState>();
        _playerNameTxt.text = _photonView.Controller.NickName;
        GameManager.instance.AddPlayerToList(gameObject);

        GameManager.instance.SetMyPlayer(this);
        if (_photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            if (LocalSettings.GetCurrentRoom.GetRoomStateProperty(LocalSettings.ROOM_STATE) == ROOMSTATE.GameIsPlaying)
            {
                UpdatePlayerStateOnNetwork(PLAYERSTATE.OutOfGame);
                GamePlayHandler.instance.ResetValuesBeforeGameStart();
                GamePlayHandler.instance.startGameExecutionAction += GamePlayHandler.instance.StartPlayGame;
                BettingManager.instance.ResetThingsBettingManager();

                BettingManager.instance.placeBetBtn.interactable = false;
                BettingManager.instance.autoCashOutBtn.gameObject.SetActive(false);
                GameManager.instance.ShowWaitingOrMultiPlierBoxInGame(false);
            }
            else
            {
                UpdatePlayerStateOnNetwork(PLAYERSTATE.Waiting);
                GamePlayHandler.instance.ResetValuesBeforeGameStart();
                BettingManager.instance.ResetThingsBettingManager();
                GameManager.instance.ShowWaitingOrMultiPlierBoxInGame(true);
            }
        }



    }

    public PLAYERSTATE GetPlayerState()
    {
        return _playerState.GetPlayerState();
    }

    public void UpdatePlayerStateOnNetwork(PLAYERSTATE state)
    {
        _playerState.SetPlayerState(state);
    }

    public void ShowCashOutPointToOtherPlayers(float cashoutpoint)
    {
        GamePlayHandler.instance.RocketPosXY(out RectTransform rocketRectTransform);

        Vector3 position = rocketRectTransform.anchoredPosition3D;
        Vector2 size = rocketRectTransform.sizeDelta;
        Vector3 scale = rocketRectTransform.localScale;
        Quaternion rotation = rocketRectTransform.localRotation;

        //Debug.LogError($"Positions got: x: {x}, y: {y}");
        double betAmount = BettingManager.instance.getCurrentBetAmount;
        float multiplier = cashoutpoint;
        int winAmount = Convert.ToInt32(cashoutpoint * betAmount);
        _photonView.RPC(nameof(ShowCashOutPointToOtherPlayersRPC), RpcTarget.All, position, size, scale, rotation, LocalSettings.userName, winAmount.ToString(), LocalSettings.emailID);
    }
    [PunRPC]
    public void ShowCashOutPointToOtherPlayersRPC(Vector3 position, Vector2 size, Vector3 scale, Quaternion rotation, string userName, string winAmount, string emailID)
    {
        GameObject sign = GamePlayHandler.instance.GetCashOutPlayerSign();
        sign.transform.GetChild(1).GetComponent<Text>().text = _photonView.Controller.NickName;
        RectTransform signRect = sign.GetComponent<RectTransform>();

        signRect.anchoredPosition3D = position;
        signRect.sizeDelta = size;
        signRect.localScale = scale;
        signRect.rotation = rotation;
        // Create and Show leaderboard item in UI
        LeaderBoardHandler.instance.CreateUIOfLeaderboardPlayerRecord(userName, winAmount, emailID);
    }
}
