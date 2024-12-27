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
    [SerializeField] string _customToken = "https://muzammilshs.github.io/BuildCrashGame/?token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjMxYjhmY2NiMmU1MjI1M2IxMzMxMzhhY2YwZTU2NjMyZjA5OTU3ZWUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJhdWQiOiI4MzgxMDE3MzE5NDUtMWQ3b3FtY2hpMWptYTlrc2s4Y2FxNWhyNGs3NmtiY2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJzdWIiOiIxMDE4NzcyMzY2MjA5MTIwMjQ5MTAiLCJlbWFpbCI6InNhbmRlZXBzajI2ODk5QGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJub25jZSI6IjE2MTk2NDkzMjA1NjYzNTk1ODc2MjQzNDQxNzM3MDE2MDQwMzkwODUyNjIzNjY4NTgwMTQ5NDQwNjk5NDY3NTc4Nzk4OTMxMzk4MzgzIiwibmJmIjoxNzM1MTAxNDk5LCJuYW1lIjoiU2FuZGVlcCBKYWlzd2FsIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FDZzhvY0lBM1Y1YnJyb0JYN2czaVJMMnFDZHdYU2tmMjNaaVhkMUh6Ymt3czNYZm90OHhxb19Nc1E9czk2LWMiLCJnaXZlbl9uYW1lIjoiU2FuZGVlcCIsImZhbWlseV9uYW1lIjoiSmFpc3dhbCIsImlhdCI6MTczNTEwMTc5OSwiZXhwIjoxNzM1MTA1Mzk5LCJqdGkiOiIyNTNjZDM0YzI1NzgzYmQ4OTMyNmIxYTk5NDAyZmJjMmEyNjk4YzMwIn0.o2lWqCebzd_kMpF14dcK8iMpPg3TnPsH7vRSt9N8IraYUJeZ2FRrDOrMw-WdWQI8AL2y2TneJBvNaZnmxSCGARniwcSDKXknYGZ4lUv08igS6c-SxLIbpv8pGS5wYECfFfm_qcMTDs3fEbvOus6zEXqWqKNjfUn8QaM2leQxWuyNCRScUR7Fol2Gm2-lqSV_kHIFQLkvDNdPGrP6Gcru4gtsZqe1goE6Yqov3N7X9ruyW0LANlf5N97_OKRwEL-YLpj6n0LxjmgpQLo4MQtTz-TyPp2xfnS-je9DXsl1UPnoB5FffuFUlDPsjY78AohrWNrojhrSL9Eei5x1DsQn3Q";
    private string _apiUrl = "http://52.74.162.233/user/profile";
    private string _jwtTokenExtracted;

    private string _url;
    //float _timeConsumed = 0;
    //bool _isStart = false;
    void Start()
    {

        //Invoke(nameof(GetUserDetail), 3f);
    }

    public void GetUserDetail()
    {
        _url = Application.absoluteURL;
        if (_customToken != "")
            _url = _customToken;

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

            PlayerLogin.instance.GetPlayerDataWithLogin(uDJWTTC.result.userDetails.email, uDJWTTC.result.userDetails.name, uDJWTTC.result.userDetails.walletAddress);
        }
        else
        {
            PlayerLogin.instance.GetPlayerDataWithLogin("abdullah@gmail.com", "Abdullah", "sdfhnsdfkf41df5g4bd52g");
            Debug.LogError("Error: " + request.error);
            GameManager.instance.ShowMessage($"Unable to connect. Please reload the page\n{request.error}");

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
