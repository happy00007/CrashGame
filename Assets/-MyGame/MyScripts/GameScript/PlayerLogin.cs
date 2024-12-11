using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;

public class PlayerLogin : ES3Cloud
{
    #region Creating Instance
    public PlayerLogin(string url, string apiKey) : base(url, apiKey)
    {
    }
    private static PlayerLogin _instance;

    public static PlayerLogin instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<PlayerLogin>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion

    const string USERNAME = "username";
    const string EMAIL = "email";
    const string WALLETNUMBER = "wallet_number";
    public void GetPlayerDataWithLogin()
    {
        formData = new List<KeyValuePair<string, string>>();

        if (LocalSettings.emailID == "")
        {
            LocalSettings.emailID = GenerateRandomEmail();
            LocalSettings.userName = GenerateRandomName();
            LocalSettings.walletID = UnityEngine.Random.Range(10000000, 100000000).ToString();
        }
        string playerName = LocalSettings.emailID;
        string email = LocalSettings.emailID;
        string walletNumber = LocalSettings.walletID;

        AddPOSTField(USERNAME, playerName);
        AddPOSTField(EMAIL, email);
        AddPOSTField(WALLETNUMBER, walletNumber);
        GetJson.instance.PostDataAndGetResponseFromServer(APIStrings.getPlayerDetailAPIURL, formData, PlayerLoginDetailParseJson);
    }


    public void PlayerLoginDetailParseJson(string json, bool isSuccess)
    {
        if (!isSuccess)
            return;

        PlayerLoginRootCls playerLoginRootCls = JsonConvert.DeserializeObject<PlayerLoginRootCls>(json);
        LocalSettings.userName = playerLoginRootCls.data.username;
        LocalSettings.emailID = playerLoginRootCls.data.email;
        LocalSettings.walletID = playerLoginRootCls.data.wallet_number;
        LocalSettings.walletAmount = Convert.ToDouble(playerLoginRootCls.data.wallet_balance);
        UIManager.instance.UpdateWalletAmountTxt();
        LocalSettings.isBlocked = playerLoginRootCls.data.status == "Active" ? false : true;
        if (!GameManager.isPlayerLogedIn && !LocalSettings.isBlocked)
            GameStartManager.instance.GetDelayTimeBetweenRounds();
        // Join Room 
        if (!LocalSettings.isBlocked)
        {
            if (!PhotonNetwork.InRoom)
                RoomManager.instance.JoinOrCreateRoom();
            GameManager.isPlayerLogedIn = true;
        }
        else
        {
            // If blocked then functionality here
            GameManager.isPlayerLogedIn = false;
        }
    }
    string GenerateRandomName()
    {
        // Define lists of first and last name parts
        string[] firstNames = { "Alex", "Jamie", "Taylor", "Jordan", "Casey", "Riley", "Morgan", "Avery" };
        string[] lastNames = { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "White", "Lee", "Walker" };

        // Pick a random first name and last name
        string firstName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
        string lastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];

        // Combine to create a full name
        return firstName + " " + lastName;
    }
    string GenerateRandomEmail()
    {
        // Define random elements
        string[] domains = { "gmail.com", "yahoo.com", "outlook.com", "example.com" };
        string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        // Generate random username
        int usernameLength = UnityEngine.Random.Range(5, 12);
        string username = "";
        for (int i = 0; i < usernameLength; i++)
        {
            username += chars[UnityEngine.Random.Range(0, chars.Length)];
        }

        // Pick a random domain
        string domain = domains[UnityEngine.Random.Range(0, domains.Length)];

        // Combine to create email
        return username + "@" + domain;
    }
}

public class UserDetailCls
{
    public string username { get; set; }
    public string email { get; set; }
    public string wallet_number { get; set; }
    public string wallet_balance { get; set; }
    public string status { get; set; }
}

public class PlayerLoginRootCls
{
    public bool success { get; set; }
    public string message { get; set; }
    public UserDetailCls data { get; set; }
}

