using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Application.runInBackground = true;
        ShowLoadingScreen(true);
        ConnectToPhotonServer();
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
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected from Photon: {cause}. Attempting to reconnect...");
        ShowLoadingScreen(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ShowLoadingScreen(bool show)
    {
        GameManager.instance.ShowLoadingPanel(show);
    }
}
