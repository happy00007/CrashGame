using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class BettingManager : ES3Cloud
{
    #region Creating Instance
    private static BettingManager _instance;

    public static BettingManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<BettingManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    public BettingManager(string url, string apiKey) : base(url, apiKey)
    {
    }
    #endregion

    public InputField betAmountTxt;
    public InputField multiplierTxt;
    WalletManager _wm;
    double _currentBetAmount = 0;
    float _currentMultiplier = 0;


    public Button placeBetBtn;
    [SerializeField] Button _placeBetBtnPlus;
    [SerializeField] Button _placeBetBtnMinus;


    public Button autoCashOutBtn;
    [SerializeField] Button _autoCashOutBtnPlus;
    [SerializeField] Button _autoCashOutBtnMinus;

    bool _isPlayerPlacedBet;

    int _rank = 0;

    void Start()
    {
        _wm = WalletManager.instance;
        ResetThingsBettingManager();
    }

    #region Current bet setting
    public void SetBetAmount(bool isAdd)
    {
        if (isAdd)
        {
            double tempAmount = _currentBetAmount;
            tempAmount += 10;
            if (tempAmount > _wm.GetTotalAmount())
            {
                GameManager.instance.ShowMessage("Not Enough cash");
                return;
            }

            _currentBetAmount = tempAmount;
            betAmountTxt.text = _currentBetAmount.ToString();

        }
        else
        {
            double tempAmount = _currentBetAmount;
            tempAmount -= 10;
            if (tempAmount < 0)
                tempAmount = 0;
            _currentBetAmount = tempAmount;
            betAmountTxt.text = _currentBetAmount.ToString();
        }
        _isPlayerPlacedBet = false;
        placeBetBtn.interactable = true;
    }

    public void OnValueChangeBetAmount()
    {
        double amount = Convert.ToDouble(betAmountTxt.text);
        if (amount > _wm.GetTotalAmount())
        {
            amount = _wm.GetTotalAmount();
        }
        else if (amount < 0)
        {
            amount = 0;
        }
        _isPlayerPlacedBet = false;

        _currentBetAmount = amount;
        betAmountTxt.text = _currentBetAmount.ToString();
        placeBetBtn.interactable = true;

    }
    #endregion

    #region Current multiplier setting

    public void SetMultiplier(bool isAdd)
    {
        if (isAdd)
        {
            float tempMultiplier = _currentMultiplier;
            tempMultiplier += 0.1f;
            if (tempMultiplier > 100)
            {
                tempMultiplier = 100;
            }

            _currentMultiplier = tempMultiplier;
            multiplierTxt.text = _currentMultiplier.ToString();

        }
        else
        {
            float tempMultiplier = _currentMultiplier;
            tempMultiplier -= 0.1f;
            if (tempMultiplier < 1)
                tempMultiplier = 1;
            _currentMultiplier = tempMultiplier;
            multiplierTxt.text = _currentMultiplier.ToString();
        }
    }

    public void OnValueChangeMultiplier()
    {
        float multiplier = 0;
        float.TryParse(multiplierTxt.text, out multiplier);
        if (multiplier > 100)
        {
            multiplier = 100f;
        }
        else if (multiplier < 1)
        {
            multiplier = 1;
        }
        _currentMultiplier = multiplier;
        multiplierTxt.text = _currentMultiplier.ToString();
    }

    #endregion


    #region Total bet placed and auto cashout getter and 
    public void ActivateBettingSection(bool isShow)
    {
        betAmountTxt.interactable = isShow;
        multiplierTxt.interactable = isShow;

        placeBetBtn.interactable = isShow;
        _placeBetBtnPlus.interactable = isShow;
        _placeBetBtnMinus.interactable = isShow;

        autoCashOutBtn.interactable = !isShow;
        _autoCashOutBtnPlus.interactable = isShow;
        _autoCashOutBtnMinus.interactable = isShow;

        placeBetBtn.gameObject.SetActive(isShow);
        autoCashOutBtn.gameObject.SetActive(!isShow);


    }
    public double TotalBetPlaced()
    {
        return _currentBetAmount;
    }

    public float AutoCheckOutValue()
    {
        return _currentMultiplier;
    }

    public void DisableCashOutbtn(bool isEnable)
    {
        autoCashOutBtn.interactable = isEnable;
    }
    #endregion

    #region Send bets to server on game start
    const string EMAIL = "email";
    const string BETAMOUNT = "bet_amount";
    const string AUTOCASHOUTPOINT = "autocrash_point";
    public void SendBetAmountToServer()
    {
        if (!_isPlayerPlacedBet)
        {
            autoCashOutBtn.interactable = false;
            return;
        }
        if (_currentBetAmount <= 0)
        {
            autoCashOutBtn.interactable = false;
            return;
        }
        float autoCashoutMultiplier = 0;

        if (_currentMultiplier > 1)
        {
            // send cash out point to server
            autoCashoutMultiplier = _currentMultiplier;
        }

        // Send bet amount and auto cash point to server here
        double amount = LocalSettings.walletAmount - _currentBetAmount;
        LocalSettings.walletAmount = amount;
        UIManager.instance.UpdateWalletAmountTxt();

        formData = new List<KeyValuePair<string, string>>();
        AddPOSTField(EMAIL, LocalSettings.emailID);
        AddPOSTField(BETAMOUNT, _currentBetAmount.ToString());
        AddPOSTField(AUTOCASHOUTPOINT, autoCashoutMultiplier.ToString());
        GetJson.instance.PostDataAndGetResponseFromServer(APIStrings.sendBetToServerAPIURL, formData, OnBetPlacedResponseJson);

    }
    void OnBetPlacedResponseJson(string json, bool isSuccess)
    {
        Debug.LogError("111111 Bet placed json: " + json);
        CashOutRootCls cashOutRootCls = JsonConvert.DeserializeObject<CashOutRootCls>(json);
        LocalSettings.walletAmount = cashOutRootCls.new_balance;
    }


    #endregion


    public double getCurrentBetAmount => _currentBetAmount;


    #region Reset things

    public void ResetThingsBettingManager()
    {
        _currentBetAmount = 0;
        _currentMultiplier = 1;
        betAmountTxt.text = _currentBetAmount.ToString();
        multiplierTxt.text = _currentMultiplier.ToString();
        ActivateBettingSection(true);
        _isPlayerPlacedBet = false;
        _rank = 0;
    }

    #endregion

    #region Place bet Btn click  and cashout btn click

    public void OnPlaceBetBtnClick()
    {
        if (!GameManager.isPlayerLogedIn)
        {
            Debug.LogError("First login before placing bet");
            return;
        }
        if (_currentBetAmount <= 0)
        {
            Debug.LogError("Bet amount should be greater on 1 ");
            return;
        }
        placeBetBtn.interactable = false;

        _isPlayerPlacedBet = _currentBetAmount > 0 ? true : false;

    }
    const string MANUALCASHOUTPOINT = "manual_crash_point";
    public void OnCashOutBtnClick()
    {
        if (!_isPlayerPlacedBet)
        {
            return;
        }
        float cashOutMultiplierSendToServer = GamePlayHandler.instance.GetCurrentMultiplierPointOnCashOut();
        Debug.LogError("Cashing out at multiplier: " + cashOutMultiplierSendToServer + "        BetAmount: " + _currentBetAmount);
        GameManager.instance.GetMyPlayer().ShowCashOutPointToOtherPlayers(cashOutMultiplierSendToServer);
        autoCashOutBtn.interactable = false;
        formData = new List<KeyValuePair<string, string>>();
        AddPOSTField(EMAIL, LocalSettings.emailID);
        AddPOSTField(BETAMOUNT, _currentBetAmount.ToString());
        AddPOSTField(MANUALCASHOUTPOINT, cashOutMultiplierSendToServer.ToString());
        GetJson.instance.PostDataAndGetResponseFromServer(APIStrings.cashoutOnBtnAPIURL, formData, OnCashOutResponseJson);
    }
    public void OnAutoCashOutCall(float multiplierVal)
    {
        if (!_isPlayerPlacedBet || _currentMultiplier <= 1)
            return;
        if (multiplierVal < _currentMultiplier)
            return;
        _isPlayerPlacedBet = false;
        float cashOutMultiplierSendToServer = GamePlayHandler.instance.GetCurrentMultiplierPointOnCashOut();
        Debug.LogError("Auto cash out multiplier ");
        GameManager.instance.GetMyPlayer().ShowCashOutPointToOtherPlayers(cashOutMultiplierSendToServer);
        autoCashOutBtn.interactable = false;
        LocalSettings.walletAmount += (cashOutMultiplierSendToServer * _currentBetAmount);
        //GamePlayHandler.instance.CheckIfPlayerWon(true, _rank);
    }

    void OnCashOutResponseJson(string json, bool isSuccess)
    {
        Debug.LogError("Bet placed json: " + json);
        CashOutRootCls cashOutRootCls = JsonConvert.DeserializeObject<CashOutRootCls>(json);
        LocalSettings.walletAmount = cashOutRootCls.new_balance;
        UIManager.instance.UpdateWalletAmountTxt();
        // Show winning animations 
    }
    #endregion

}
public class CashOutRootCls
{
    public string status { get; set; }
    public double amount_won { get; set; }
    public double new_balance { get; set; }
}
