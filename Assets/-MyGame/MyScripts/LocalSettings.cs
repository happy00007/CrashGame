
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;
using Photon.Realtime;


public static class LocalSettings
{
    public const string ROOM_STATE = "ROOMSTATE";
    public const float GAME_RESET_DELAY_TIME = 3f;

    public const int GAME_DELAY_TIME_OVERRIDE = 35;

    public const float GRAPH_X_AXIS_LIMIT = 12f;
    public const float GRAPH_Y_AXIS_LIMIT = 3.8f;



    public static string TimeFormat(float timeLeft)
    {
        float timeLeftInSeconds = timeLeft;

        int hours = (int)(timeLeftInSeconds / 3600);
        int minutes = (int)(timeLeftInSeconds / 60);
        int seconds = (int)(timeLeftInSeconds % 60);

        string formattedTimeLeft;
        if (hours > 0)
            formattedTimeLeft = $"{hours}:{minutes:D2}:{seconds:D2}";
        else if (minutes > 0)
            formattedTimeLeft = $"{minutes:D2}:{seconds:D2}";
        else if (seconds >= 0)
            formattedTimeLeft = $"{seconds:D2}s";
        else
            formattedTimeLeft = $"0s";

        return formattedTimeLeft;
    }


    public static void SetPosAndRect(GameObject instantiatedObj, RectTransform aLReadyObjPos, Transform parentobj)
    {
        instantiatedObj.transform.parent = parentobj;
        RectTransform myPlayerRectTransform = instantiatedObj.GetComponent<RectTransform>();
        myPlayerRectTransform.localScale = aLReadyObjPos.localScale;
        myPlayerRectTransform.localPosition = aLReadyObjPos.localPosition;
        myPlayerRectTransform.anchorMin = aLReadyObjPos.anchorMin;
        myPlayerRectTransform.anchorMax = aLReadyObjPos.anchorMax;
        myPlayerRectTransform.anchoredPosition = aLReadyObjPos.anchoredPosition;
        myPlayerRectTransform.sizeDelta = aLReadyObjPos.sizeDelta;
        myPlayerRectTransform.localRotation = aLReadyObjPos.localRotation;

    }

    const string TOTAL_AMOUNT = "total_amount";
    const string DEFAULT_AMOUNT = "1000";
    public static double walletAmount
    {
        get => Convert.ToDouble(PlayerPrefs.GetString(TOTAL_AMOUNT));
        set => PlayerPrefs.SetString(TOTAL_AMOUNT, value.ToString());
    }
    public static Room GetCurrentRoom
    {
        get => PhotonNetwork.CurrentRoom;
    }
    const string EMAIL = "email";
    public static string emailID
    {
        get => PlayerPrefs.GetString(EMAIL);
        set => PlayerPrefs.SetString(EMAIL, value);
    }

    const string WALLETID = "walletID";
    public static string walletID
    {
        get => PlayerPrefs.GetString(WALLETID);
        set => PlayerPrefs.SetString(WALLETID, value);
    }
    const string ISBLOCKED = "is_blocked";
    public static bool isBlocked
    {
        get => PlayerPrefs.GetString(ISBLOCKED, "0") == "0" ? false : true;
        set => PlayerPrefs.GetString(ISBLOCKED, value == false ? "0" : "1");
    }
    const string USERNAME = "user_name";
    public static string userName
    {
        get => PlayerPrefs.GetString(USERNAME);
        set => PlayerPrefs.SetString(USERNAME, value);
    }
}
