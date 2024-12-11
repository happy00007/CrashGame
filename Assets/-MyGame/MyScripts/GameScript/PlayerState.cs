using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System;

public class PlayerState : MonoBehaviourPunCallbacks
{
    RoomNPlayerState.PLAYERSTATE _currentPlayerState;
    PhotonView _photonView;
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }
    public RoomNPlayerState.PLAYERSTATE GetPlayerState()
    {
        return _currentPlayerState;
    }

    public void SetPlayerState(RoomNPlayerState.PLAYERSTATE playerState)
    {
        if (_photonView == null)
            _photonView = GetComponent<PhotonView>();
        _photonView.RPC(nameof(SetUpdatePlayerStateOnNetwork), RpcTarget.All, playerState);
    }
    [PunRPC]
    public void SetUpdatePlayerStateOnNetwork(RoomNPlayerState.PLAYERSTATE playerState)
    {
        _currentPlayerState = playerState;
    }

    void OnUpdateCurrentState(RoomNPlayerState.PLAYERSTATE playerState)
    {
        switch (playerState)
        {
            case RoomNPlayerState.PLAYERSTATE.Waiting:

                break;

            case RoomNPlayerState.PLAYERSTATE.WaitingInGame:

                break;

            case RoomNPlayerState.PLAYERSTATE.GameIsPlaying:

                break;
            case RoomNPlayerState.PLAYERSTATE.OutOfGame:

                break;
        }
    }
}
