using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ShareOnTwitter : ES3Cloud
{
    #region Constructor
    public ShareOnTwitter(string url, string apiKey) : base(url, apiKey)
    {
    }
    #endregion

    [SerializeField] LeaderBoardHandler _leaderBoardHandler;
    [SerializeField] string[] _messages;



    private void Start()
    {
        _leaderBoardHandler = GetComponent<LeaderBoardHandler>();
        Invoke(nameof(GetMessagesFromServer), 2f);
    }

    public void ShareMessageOnX(int currentPos)
    {
        string message = _messages[currentPos - 1];


        string twitterUrl = "";

        twitterUrl = "https://twitter.com/intent/tweet?text=" +
                          UnityWebRequest.EscapeURL(message);

        Application.OpenURL(twitterUrl);
        Debug.LogError("Twitter shared");
        _leaderBoardHandler._twitterShareBox.SetActive(false);
    }

    public void GetMessagesFromServer()
    {
        GetJson.instance.GetJsonFromServer(APIStrings.getTwitterMessagesAPIURL, MessageJsonToCls);
    }

    void MessageJsonToCls(string json, bool isSuccess)
    {
        if (isSuccess)
        {
            TwitterMessagesRootCls[] twitterMessagesRootCls = JsonConvert.DeserializeObject<TwitterMessagesRootCls[]>(json);
            _messages = new string[twitterMessagesRootCls.Length];
            for (int i = 0; i < twitterMessagesRootCls.Length; i++)
            {
                _messages[i] = twitterMessagesRootCls[i].tweet_text;
            }
        }
        else
        {
            Invoke(nameof(GetMessagesFromServer), 2f);
        }
    }
}
#region Messages Josn to c#
public class TwitterMessagesRootCls
{
    public string rank { get; set; }
    public string tweet_text { get; set; }
}


#endregion
