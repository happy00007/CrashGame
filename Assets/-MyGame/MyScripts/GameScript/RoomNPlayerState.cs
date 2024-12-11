using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNPlayerState : MonoBehaviour
{
    public enum ROOMSTATE
    {
        Waiting = 0,
        GameIsPlaying = 1,
    };

    public enum PLAYERSTATE
    {
        Waiting,
        WaitingInGame,
        GameIsPlaying,
        OutOfGame
    }
}
