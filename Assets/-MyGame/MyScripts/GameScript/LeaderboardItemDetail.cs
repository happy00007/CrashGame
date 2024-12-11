using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItemDetail : MonoBehaviour
{
    [SerializeField] Text _rankTxt;
    [SerializeField] Text _userNameTxt;
    [SerializeField] Text _walletAmountTxt;

    int _rank;
    string _userName;
    string _winAmount;
    [ShowOnly]
    public string emailID;
    [ShowOnly]
    public int winAmount = 0;
    public void FillFieldsLeaderBoard(LeaderBoardDetailRootCls ldrc, Color bgClr)
    {
        GetComponent<Image>().color = bgClr;
        _rank = ldrc.rank;
        _userName = ldrc.username;
        _winAmount = ldrc.wallet_balance;
        winAmount = int.Parse(_winAmount);
        emailID = ldrc.email;

        UpDateUIFields();
    }

    public void UpdateRank(int rnk)
    {
        _rank = rnk;
        UpDateUIFields();
    }
    void UpDateUIFields()
    {
        string rnk = _rank == 0 ? "" : _rank.ToString();
        _rankTxt.text = rnk;
        _userNameTxt.text = _userName.ToString();
        _walletAmountTxt.text = _winAmount;
    }

}
