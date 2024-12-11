using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    #region Creating Instance
    private static GameResetManager _instance;


    public static GameResetManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GameResetManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion


    public void ResetWholeGame()
    {
        //GamePlayHandler.instance.ResetValuesBeforeGameStart();
        RoomStateManager.instance.UpdateCurrentRoomState(RoomNPlayerState.ROOMSTATE.Waiting);
    }

    public void GetValuesOnGameCrash()
    {
        BettingManager.instance.DisableCashOutbtn(false);
        PlayerLogin.instance.GetPlayerDataWithLogin();
        GameStartManager.instance.GetDelayTimeBetweenRounds();
    }
}
