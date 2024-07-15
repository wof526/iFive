using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class yuraMatch : MonoBehaviourPunCallbacks //포톤의 event 감지 가능..
{
    public static string username;
    public Button joinButton; // 입장 버튼 
    public TextMeshProUGUI connectionInfoText; //버튼 내 텍스트 

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //설정 정보를 가지고 마스터 서버에 연결 시도

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting to Server...";
    }

    // 마스터 서버에 연결되었을 때 자동 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online!";

        Debug.Log("Connected to master server");
        PhotonNetwork.JoinLobby();
    }

    // 로비에 참여했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    public void StartMatch()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();

        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 참가에 실패하면 새 방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
        Debug.Log("create a new Room");
    }


    // 연결 실패 시 호출되는 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false; //접속버튼 비활성화
        Debug.LogError("Disconnected: " + cause);

        PhotonNetwork.ConnectUsingSettings(); // 접속 실패 시 재접속 시도
    }


    // 룸에 성공적으로 참여했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        username = "jiyunkim";
        //SceneManager.LoadScene("Driving"); 2명의 플레이어가 독자적으로 씬에 들어가게 됨.
        PhotonNetwork.LoadLevel("yuraDrivingScene"); //같은 씬을 자동 동기화 함.
    }
}