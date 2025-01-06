using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Web;

public class GetUserDetailWithToken : MonoBehaviour
{
    #region Creating Instance
    private static GetUserDetailWithToken _instance;

    public static GetUserDetailWithToken instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<GetUserDetailWithToken>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion
    [Header("Keep empty if getting token from link")]
    [SerializeField] string _customToken;
    private string _apiUrl = "http://52.74.162.233/user/profile";
    private string _jwtTokenExtracted;

    private string _url;
    //"https://muzammilshs.github.io/BuildCrashGame/?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjMxYjhmY2NiMmU1MjI1M2IxMzMxMzhhY2YwZTU2NjMyZjA5OTU3ZWUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDE4NzcyMzY2MjA5MTIwMjQ5MTAiLCJlbWFpbCI6InNhbmRlZXBzajI2ODk5QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJub25jZSI6IjE2MTk2NDkzMjA1NjYzNTk1ODc2MjQzNDQxNzM3MDE2MDQwMzkwODUyNjIzNjY4NTgwMTQ5NDQwNjk5NDY3NTc4Nzk4OTMxMzk4MzgzIiwibmJmIjoxNzM1MDIxMDA5LCJuYW1lIjoiU2FuZGVlcCBKYWlzd2FsIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0lBM1Y1YnJyb0JYN2czaVJMMnFDZHdYU2tmMjNaaVhkMUh6Ymt3czNYZm90OHhxb19Nc1E9czk2LWMiLCJnaXZlbl9uYW1lIjoiU2FuZGVlcCIsImZhbWlseV9uYW1lIjoiSmFpc3dhbCIsImlhdCI6MTczNTAyMTMwOSwiZXhwIjoxNzM1MDI0OTA5LCJqdGkiOiJjNTQ3Njc4MmY0YjVhNmVlYTVmNTUyYjEzNmIxYjI1NWE1NjhlNjRlIn0.U6LAGoTVTtg6fzH5WZqFInAsUP4ZjV9tcuhjfH8R_JI__1EDLllfT8K7HnI-sOsb6Dj8onh6F6UBLdQmPKS1pOc7zoLsDHStq6TA8LWVY4F8DKppWTVZVGiCYCRp9UmlbHog6QIMjmw5hw8E0xDfvN6nu8uDS-LPmhYOwveFcBQktBcxnjaoXK0r7ddWpU_6qLDbQxAx6ngsYeyzQ_FcvidJ9dl1vp6OvPYOYQENg_MUrWZF7td2HcJIiyLSVsLC7Szyk96M8NLeZLAQF5HV9cUoeWwSixjA6uBoYzm1A10DNo6M7WbBUi0eTIBFZOMTwlP0hWIHSbxSlLLGXswm9Q"
    void Start()
    {

        //Invoke(nameof(GetUserDetail), 3f);
    }

    public void GetUserDetail()
    {
        _url = Application.absoluteURL;
        //if (_customToken != "")
        //    _url = _customToken;
        _url = "https://muzammilshs.github.io/BuildCrashGame/?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjMxYjhmY2NiMmU1MjI1M2IxMzMxMzhhY2YwZTU2NjMyZjA5OTU3ZWUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDE4NzcyMzY2MjA5MTIwMjQ5MTAiLCJlbWFpbCI6InNhbmRlZXBzajI2ODk5QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJub25jZSI6IjE2MTk2NDkzMjA1NjYzNTk1ODc2MjQzNDQxNzM3MDE2MDQwMzkwODUyNjIzNjY4NTgwMTQ5NDQwNjk5NDY3NTc4Nzk4OTMxMzk4MzgzIiwibmJmIjoxNzM1MDIxMDA5LCJuYW1lIjoiU2FuZGVlcCBKYWlzd2FsIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0lBM1Y1YnJyb0JYN2czaVJMMnFDZHdYU2tmMjNaaVhkMUh6Ymt3czNYZm90OHhxb19Nc1E9czk2LWMiLCJnaXZlbl9uYW1lIjoiU2FuZGVlcCIsImZhbWlseV9uYW1lIjoiSmFpc3dhbCIsImlhdCI6MTczNTAyMTMwOSwiZXhwIjoxNzM1MDI0OTA5LCJqdGkiOiJjNTQ3Njc4MmY0YjVhNmVlYTVmNTUyYjEzNmIxYjI1NWE1NjhlNjRlIn0.U6LAGoTVTtg6fzH5WZqFInAsUP4ZjV9tcuhjfH8R_JI__1EDLllfT8K7HnI-sOsb6Dj8onh6F6UBLdQmPKS1pOc7zoLsDHStq6TA8LWVY4F8DKppWTVZVGiCYCRp9UmlbHog6QIMjmw5hw8E0xDfvN6nu8uDS-LPmhYOwveFcBQktBcxnjaoXK0r7ddWpU_6qLDbQxAx6ngsYeyzQ_FcvidJ9dl1vp6OvPYOYQENg_MUrWZF7td2HcJIiyLSVsLC7Szyk96M8NLeZLAQF5HV9cUoeWwSixjA6uBoYzm1A10DNo6M7WbBUi0eTIBFZOMTwlP0hWIHSbxSlLLGXswm9Q";
        _jwtTokenExtracted = ExtractToken(_url);
        StartCoroutine(MakeRequest());
    }

    IEnumerator MakeRequest()
    {
        // Set up the headers
        UnityWebRequest request = UnityWebRequest.Get(_apiUrl);
        request.SetRequestHeader("Authorization", "Bearer " + _jwtTokenExtracted);
        request.SetRequestHeader("Accept", "*/*");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            UserDetailJWTTokenRootCls uDJWTTC = JsonConvert.DeserializeObject<UserDetailJWTTokenRootCls>(json);
            string username = uDJWTTC.result.userDetails.name;
            string email = uDJWTTC.result.userDetails.email;
            string walledID = uDJWTTC.result.userDetails.walletAddress;
            PlayerLogin.instance.GetPlayerDataWithLogin(email, username, walledID);
        }
        else
        {
            PlayerLogin.instance.GetPlayerDataWithLogin("abdullah@gmail.com", "Abdullah", "sdfhnsdfkf41df5g4bd52g");
            //Debug.LogError($"Error:   {request.error} ");
            //GameManager.instance.ShowMessage($"Unable to connect. Please reload the page\n{request.error} \n Token not received, playing with fake id");
            GameManager.instance.ShowMessage($"Your session has timed out. Please close the page and reopen the game to continue.");

            //Invoke(nameof(GetUserDetail), 5f);
        }
    }
    private string ExtractToken(string url)
    {
        Uri uri = new Uri(url);

        // Extract the query string without the leading "?" character
        string query = uri.Query.TrimStart('?');

        // Split the query into key-value pairs
        string[] parameters = query.Split('&');
        foreach (string param in parameters)
        {
            // Split each key-value pair
            string[] keyValue = param.Split('=');
            if (keyValue.Length == 2 && keyValue[0] == "token")
            {
                return keyValue[1]; // Return the token value
            }
        }

        throw new Exception("Token not found in the URL.");
    }
}
public class ResultCls
{
    public UserDetailsCls userDetails { get; set; }
}

public class UserDetailJWTTokenRootCls
{
    public string message { get; set; }
    public bool error { get; set; }
    public int statusCode { get; set; }
    public ResultCls result { get; set; }
}

public class UserDetailsCls
{
    public string name { get; set; }
    public string email { get; set; }
    public string profileImage { get; set; }
    public string walletAddress { get; set; }
    public object phoneNumber { get; set; }
    public object age { get; set; }
    public object country { get; set; }
    public object sex { get; set; }
    public object telegram { get; set; }
    public object twitter { get; set; }
}
