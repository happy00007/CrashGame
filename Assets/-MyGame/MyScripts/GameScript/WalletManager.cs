using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletManager : MonoBehaviour
{
    #region Creating Instance
    private static WalletManager _instance;


    public static WalletManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindFirstObjectByType<WalletManager>();
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public double GetTotalAmount()
    {
        return LocalSettings.walletAmount;
    }

}
