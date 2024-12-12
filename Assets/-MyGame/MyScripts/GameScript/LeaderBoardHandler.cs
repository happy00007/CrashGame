using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoardHandler : MonoBehaviour
{
    #region Creating Instance
    private static LeaderBoardHandler _instance;


    public static LeaderBoardHandler instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<LeaderBoardHandler>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion


    #region Variables
    public GameObject leaderboardDetailPrefab;
    public Color[] itemClr;
    public int[] positionScores;
    public List<GameObject> leaderboardItems;


    //public Texture2D[] rankImages;
    public string[] imageURLs;

    [SerializeField] Text _totalPlayersInGameTxt;
    [SerializeField] GameObject _twitterShareBox;
    [SerializeField] Text _winDescriptionTxt;

    #endregion

    #region Unity Event function
    void Start()
    {
        _twitterShareBox.SetActive(false);
        StartCoroutine(PlayerCountInGame());
    }

    #endregion

    #region Lederboard from server
    //public void GetLeaderboardUsingAPI()
    //{
    //    GetJson.instance.GetJsonFromServer(APIStrings.getLeaderBoardAPIURL, GetLeaderboardFromServer);
    //}

    //void GetLeaderboardFromServer(string jsonResponse, bool isSuccess)
    //{
    //    if (isSuccess)
    //    {
    //        List<LeaderBoardDetailRootCls> users = JsonConvert.DeserializeObject<List<LeaderBoardDetailRootCls>>(jsonResponse);
    //        CreateUIOfLeaderboard(users);
    //    }
    //}

    //void CreateUIOfLeaderboard(List<LeaderBoardDetailRootCls> users)
    //{
    //    int totalPlayers = 0;
    //    leaderboardItems = GamePlayHandler.instance.DestroyAndClearList(leaderboardItems);
    //    for (int i = 0; i < users.Count; i++)
    //    {
    //        GameObject item = Instantiate(leaderboardDetailPrefab);
    //        leaderboardItems.Add(item);
    //        LocalSettings.SetPosAndRect(item, leaderboardDetailPrefab.GetComponent<RectTransform>(), leaderboardDetailPrefab.transform.parent);
    //        item.SetActive(true);
    //        item.GetComponent<LeaderboardItemDetail>().FillFieldsLeaderBoard(users[i], i % 2 == 0 ? itemClr[0] : itemClr[1]);
    //        totalPlayers++;
    //    }
    //    _totalPlayersInGameTxt.text = totalPlayers.ToString();
    //}

    #endregion


    #region Local Leaderboard
    int _currentCounter;

    public void CreateUIOfLeaderboardPlayerRecord(string userName, string winAmount, string emailID)
    {
        //sdfsdf
        GameObject item = Instantiate(leaderboardDetailPrefab);
        leaderboardItems.Add(item);
        LocalSettings.SetPosAndRect(item, leaderboardDetailPrefab.GetComponent<RectTransform>(), leaderboardDetailPrefab.transform.parent);
        item.SetActive(true);
        LeaderBoardDetailRootCls lid = new LeaderBoardDetailRootCls {
            rank = 0,
            username = userName,
            wallet_balance = winAmount,
            email = emailID
        };
        item.GetComponent<LeaderboardItemDetail>().FillFieldsLeaderBoard(lid, _currentCounter % 2 == 0 ? itemClr[0] : itemClr[1]);
        _currentCounter++;

    }

    public void ClearLeaderBoardValues()
    {
        leaderboardItems = GamePlayHandler.instance.DestroyAndClearList(leaderboardItems);
    }

    IEnumerator PlayerCountInGame()
    {
        while (true)
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InRoom)
                {
                    _totalPlayersInGameTxt.text = PhotonNetwork.PlayerList.Length.ToString();
                }
            }

            yield return new WaitForSeconds(5);
        }

    }
    int _currentRank = 0;
    int _wonAmount = 0;
    public void SortLeaderboard()
    {

        if (leaderboardItems.Count < 1)
            return;

        leaderboardItems = leaderboardItems
            .OrderByDescending(item => item.GetComponent<LeaderboardItemDetail>().winAmount)
            .ToList();

        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            leaderboardItems[i].GetComponent<LeaderboardItemDetail>().UpdateRank(i + 1);
            leaderboardItems[i].transform.SetSiblingIndex(i);
        }

        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            if (LocalSettings.emailID == leaderboardItems[i].GetComponent<LeaderboardItemDetail>().emailID)
            {
                _currentRank = i + 1;
                _wonAmount = leaderboardItems[i].GetComponent<LeaderboardItemDetail>().winAmount;
                ShowTwitterShareBox();
                break;
            }
        }
    }

    void ShowTwitterShareBox()
    {
        string msg = "";
        switch (_currentRank)
        {
            case 1:
                msg = $"Congratulations! 🎉 You're ranked #{_currentRank} and just bagged ${_wonAmount}!";
                break;
            case 2:
                msg = $"Well done! 🥈 Rank: #{_currentRank}  | You've won ${_wonAmount}! Keep pushing for the top spot!";
                break;
            case 3:
                msg = $"Solid play! You're in 3rd place with ${_wonAmount}. Keep it up!";
                break;
            default:
                msg = $"Well done! 🥈 Rank: #{_currentRank}  | You've won ${_wonAmount}! Keep pushing for the top spot!";
                break;
        }
        _winDescriptionTxt.text = msg;
        _twitterShareBox.SetActive(true);

    }
    #endregion

    public void ResetThings()
    {
        _currentRank = 0;
        _wonAmount = 0;
        _twitterShareBox.SetActive(true);
    }

    #region Share achievement on X Twitter

    public void OnTwitterShareBtnClick()
    {
        ShareMessageOnX(_currentRank);
        //ShareMessageOnX(2);
    }

    //Texture2D _imageToShare;
    string _imageUrl;

    void ShareMessageOnX(int currentPos)
    {
        string msg = "";
        string url = "";
        switch (currentPos)
        {
            case 1:
                _imageUrl = imageURLs[0];
                msg = $"🏆 Just topped the leaderboard with a 10x multiplier on #KryzelCrashGame at @Kryzel_io! 🚀 Am I the king of predictions or what? Try to beat my record and claim the throne! 👑 Play now: {url}";
                break;
            case 2:
                _imageUrl = imageURLs[1];
                msg = $"🥈 Finished second with a 9x multiplier on #KryzelCrashGame at @Kryzel_io! Close to first, but there's room to improve! 💪 Ready to take the top spot next time! Who's challenging me? 🚀 Play here: {url} #PredictAndWin #CryptoGaming";
                break;
            case 3:
                _imageUrl = imageURLs[2];
                msg = $"🥉 Secured third place with an 8x multiplier on #KryzelCrashGame at @Kryzel_io! Climbing up the ranks and aiming for the top! 🎯 Join me in the challenge and let's see who dominates next! 🚀 Play now: {url} #PredictAndWin #CryptoGaming";
                break;
            default:
                msg = $"🚀 Just multiplied my wager by 5x on #KryzelCrashGame! 💸 Think you can do better? Check out @Kryzel_io for the ultimate prediction game experience. 🎮 Play and win big: {url} ";
                break;
        }
        string message = msg;


        //string filePath = Path.Combine(Application.temporaryCachePath, "sharedImage.png");
        //File.WriteAllBytes(filePath, _imageToShare.EncodeToPNG());

        string twitterUrl = "";
        //if (currentPos < 4)
        //{
        //    twitterUrl = "https://x.com/intent/tweet?text=" + UnityWebRequest.EscapeURL(message) + "&url=" + UnityWebRequest.EscapeURL(_imageUrl);
        //}
        //else
        {
            twitterUrl = "https://twitter.com/intent/tweet?text=" +
                              UnityWebRequest.EscapeURL(message);
        }
        Application.OpenURL(twitterUrl);
        Debug.LogError("Twitter shared");
        _twitterShareBox.SetActive(false);
    }
    #endregion
}
public class LeaderBoardDetailRootCls
{
    public int rank { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string wallet_number { get; set; }
    public string wallet_balance { get; set; }
}

