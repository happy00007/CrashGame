using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Creating Instance
    private static UIManager _instance;

    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<UIManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion

    [SerializeField] Text _walletAmountTxt;

    private void Start()
    {
        UpdateWalletAmountTxt();
    }

    public void UpdateWalletAmountTxt()
    {
        _walletAmountTxt.text = LocalSettings.walletAmount.ToString();
    }
}
