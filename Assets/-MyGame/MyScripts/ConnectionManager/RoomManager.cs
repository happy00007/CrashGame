using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const int MAXPLAYERSINROOM = 20;
    #region Creating Instance
    private static RoomManager _instance;
    const string _roomName = "CRASHGAMEROOM";
    public static RoomManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindFirstObjectByType<RoomManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    #endregion

    private void Start()
    {
        ConfigurePhotonSettings();
        ShowLoadingScreen(true);
    }

    private void ConfigurePhotonSettings()
    {
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 2 * 60 * 1000;
        PhotonNetwork.KeepAliveInBackground = 120;
    }

    public void JoinOrCreateRoom()
    {
        // Try to join the specific room
        PhotonNetwork.JoinRoom(_roomName);
    }

    private void CreateCrashGameRoom()
    {
        RoomOptions roomOptions = new RoomOptions {
            MaxPlayers = MAXPLAYERSINROOM,
            IsVisible = true,
            IsOpen = true,
            PlayerTtl = 120000
        };

        // Create the room with the specified name
        PhotonNetwork.CreateRoom(_roomName, roomOptions);
    }

    #region Override Methods

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        ShowLoadingScreen(false);
        PhotonNetwork.NickName = "User: " + Random.Range(10, 100);

        GetUserDetailWithToken.instance.GetUserDetail();
        //PlayerLogin.instance.GetPlayerDataWithLogin();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Failed to join room: {message}. Attempting to create the CRASHGAMEROOM...");

        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            CreateCrashGameRoom(); // Room doesn't exist, create it
        }
        else if (returnCode == ErrorCode.GameFull)
        {
            Debug.LogError("Room is full.");
            GameManager.instance.ShowMessage("The room is full. Please wait for a slot to become available.");
            Invoke(nameof(JoinOrCreateRoom), 5f);
        }
        else
        {
            Debug.LogError($"Unable to join the room due to error: {message}");
            Invoke(nameof(JoinOrCreateRoom), 1f);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to create room: {message}");
        GameManager.instance.ShowMessage("Failed to create the game room. Please try again later.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Successfully created a new room: {PhotonNetwork.CurrentRoom.Name}");
        ShowLoadingScreen(false); // Hide loading screen
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Successfully joined room: {PhotonNetwork.CurrentRoom.Name}");
        ShowLoadingScreen(false); // Hide loading screen
        //GameManager.instance.ShowMessage($"Joined room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected from Photon: {cause}. Attempting to reconnect...");

        if (cause == DisconnectCause.Exception || cause == DisconnectCause.ExceptionOnConnect || cause == DisconnectCause.ClientTimeout)
        {
            if (PhotonNetwork.ReconnectAndRejoin())
            {
                Debug.Log("Reconnecting and rejoining the room...");
            }
            else
            {
                Debug.LogError("Failed to reconnect and rejoin the room. Connecting to the server...");
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    #endregion

    private void ShowLoadingScreen(bool show)
    {
        GameManager.instance.ShowLoadingPanel(show);
    }
}
















//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

//public class RoomManager : MonoBehaviourPunCallbacks
//{
//    private const int MAXPLAYERSINROOM = 20;
//    #region Creating Instance
//    private static RoomManager _instance;

//    public static RoomManager instance
//    {
//        get
//        {
//            if (_instance == null)
//                _instance = FindFirstObjectByType<RoomManager>();
//            return _instance;
//        }
//    }
//    private void Awake()
//    {
//        if (_instance == null)
//            _instance = this;
//    }
//    #endregion

//    private void Start()
//    {
//        ConfigurePhotonSettings();
//        ShowLoadingScreen(true);
//    }
//    private void ConfigurePhotonSettings()
//    {
//        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 2 * 60 * 1000;
//        PhotonNetwork.KeepAliveInBackground = 120;
//    }
//    private void ConnectToPhotonServer()
//    {
//        if (!PhotonNetwork.IsConnected)
//        {
//            PhotonNetwork.AutomaticallySyncScene = true;
//            PhotonNetwork.ConnectUsingSettings();
//        }
//    }

//    public override void OnConnectedToMaster()
//    {
//        Debug.Log("Connected to Photon Master Server.");
//        ShowLoadingScreen(false);
//        Debug.LogError("Now creating the room");
//        PhotonNetwork.NickName = "User: " + Random.RandomRange(10, 100);

//        PlayerLogin.instance.GetPlayerDataWithLogin();
//    }

//    public void JoinOrCreateRoom()
//    {
//        PhotonNetwork.JoinRandomRoom();
//    }


//    private void CreateNewRoom()
//    {
//        string newRoomName = "Room_" + Random.Range(1000, 9999);
//        RoomOptions roomOptions = new RoomOptions {
//            MaxPlayers = MAXPLAYERSINROOM,
//            IsVisible = true,
//            IsOpen = true,
//            PlayerTtl = 120000
//        };

//        PhotonNetwork.CreateRoom(newRoomName, roomOptions);
//    }

//    #region Override methods

//    public override void OnJoinRandomFailed(short returnCode, string message)
//    {
//        Debug.LogWarning($"Failed to join a random room: {message}. Creating a new room...");
//        CreateNewRoom(); // No available room or all rooms are full
//    }
//    public override void OnCreatedRoom()
//    {
//        Debug.Log($"Successfully created a new room: {PhotonNetwork.CurrentRoom.Name}");
//        ShowLoadingScreen(false); // Hide loading screen
//    }

//    public override void OnJoinedRoom()
//    {
//        Debug.Log($"Successfully joined room: {PhotonNetwork.CurrentRoom.Name}");
//        ShowLoadingScreen(false); // Hide loading screen
//    }

//    public override void OnCreateRoomFailed(short returnCode, string message)
//    {
//        Debug.LogError($"Failed to create a new room: {message}");
//        ShowLoadingScreen(false); // Hide loading screen on failure
//    }

//    public override void OnDisconnected(DisconnectCause cause)
//    {

//        Debug.LogWarning($"Disconnected from Photon: {cause}. Attempting to reconnect...");

//        if (cause == DisconnectCause.Exception || cause == DisconnectCause.ExceptionOnConnect || cause == DisconnectCause.ClientTimeout)
//        {
//            // Attempt to reconnect to the last room
//            if (PhotonNetwork.ReconnectAndRejoin())
//            {
//                Debug.Log("Reconnecting and rejoining the room...");
//            }
//            else
//            {
//                Debug.LogError("Failed to reconnect and rejoin the room. Connecting to the server...");
//                PhotonNetwork.ConnectUsingSettings();
//            }
//        }
//    }

//    #endregion
//    private void ShowLoadingScreen(bool show)
//    {
//        GameManager.instance.ShowLoadingPanel(show);
//    }
//}
