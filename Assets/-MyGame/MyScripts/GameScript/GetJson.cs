using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class GetJson : ES3Cloud
{
    #region Creating Instance
    private static GetJson _instance;

    public GetJson(string url, string apiKey) : base(url, apiKey)
    {
    }

    public static GetJson instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GetJson>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;

    }
    #endregion


    #region get data from server
    public void GetJsonFromServer(string apiURL, Action<string, bool> methodCall)
    {
        StartCoroutine(GetJsonFromServerUsingAPI(apiURL, methodCall));
    }
    IEnumerator GetJsonFromServerUsingAPI(string apiURL, Action<string, bool> methodCall)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            webRequest.timeout = 15;
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Get the JSON response
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response: " + jsonResponse);
                methodCall?.Invoke(jsonResponse, true);

            }
            else
            {
                // Log the error
                Debug.LogError("Error fetching data: " + webRequest.error);
                methodCall?.Invoke(webRequest.downloadHandler.text, false);
            }
        }
    }

    #endregion


    #region Get player data from server



    public void PostDataAndGetResponseFromServer(string url, List<KeyValuePair<string, string>> formData, Action<string, bool> methodCall)
    {
        StartCoroutine(GetPlayerLoginAPIURL(url, formData, methodCall));
    }
    IEnumerator GetPlayerLoginAPIURL(string url, List<KeyValuePair<string, string>> formData, Action<string, bool> methodCall)
    {
        WWWForm form = new WWWForm();
        foreach (var field in formData)
        {
            form.AddField(field.Key, field.Value);
        }
        using (var webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.timeout = 15;
            yield return SendWebRequest(webRequest);
            HandleError(webRequest, true);
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                //Debug.LogError("Result: " + webRequest.result + "\nFail Json: " + webRequest.downloadHandler.text + "\n Error: " + webRequest.error + "\n Response Code: " + webRequest.responseCode + "\n Result: " + webRequest.result);
                methodCall?.Invoke(webRequest.downloadHandler.text, false);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                //Debug.LogError("Get Detail Json String: " + jsonResponse);
                methodCall?.Invoke(webRequest.downloadHandler.text, true);
            }
        }
    }


    #endregion
}
