using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private const int MAXPLAYERSINROOM = 20;
    #region Creating Instance
    private static RoomManager _instance;

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
        //ConnectToPhotonServer();
    }
    private void ConfigurePhotonSettings()
    {
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 2 * 60 * 1000;
        PhotonNetwork.KeepAliveInBackground = 120;
    }
    private void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server.");
        ShowLoadingScreen(false);
        Debug.LogError("Now creating the room");
        PhotonNetwork.NickName = "User: " + Random.RandomRange(10, 100);
        //JoinOrCreateRoom(); 
        //GameStartManager.instance.ResetRemainingWaitTimeFromServer();
        PlayerLogin.instance.GetPlayerDataWithLogin();
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Attempting to join an existing room...");
    }


    private void CreateNewRoom()
    {
        string newRoomName = "Room_" + Random.Range(1000, 9999); // Generate a unique room name
        RoomOptions roomOptions = new RoomOptions {
            MaxPlayers = MAXPLAYERSINROOM, // Set maximum players per room
            IsVisible = true, // Make the room visible
            IsOpen = true,     // Allow others to join
            PlayerTtl = 120000
        };

        PhotonNetwork.CreateRoom(newRoomName, roomOptions);
        Debug.Log($"Creating a new room: {newRoomName}");
    }

    #region Override methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Failed to join a random room: {message}. Creating a new room...");
        CreateNewRoom(); // No available room or all rooms are full
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
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to create a new room: {message}");
        ShowLoadingScreen(false); // Hide loading screen on failure
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        Debug.LogWarning($"Disconnected from Photon: {cause}. Attempting to reconnect...");

        if (cause == DisconnectCause.Exception || cause == DisconnectCause.ExceptionOnConnect || cause == DisconnectCause.ClientTimeout)
        {
            // Attempt to reconnect to the last room
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
