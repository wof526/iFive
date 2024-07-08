using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Match : MonoBehaviourPunCallbacks{
    public static string username;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 연결되었을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinLobby();
    }

    // 로비에 참여했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    public void StartMatch(){
        if(PhotonNetwork.IsConnectedAndReady){
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }
    // 연결 실패 시 호출되는 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected: " + cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 참가에 실패하면 새 방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 룸에 성공적으로 참여했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        username = "jiyunkim";
        SceneManager.LoadScene("Driving");
    }
}
