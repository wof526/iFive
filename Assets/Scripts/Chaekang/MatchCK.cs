using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MatchCK : MonoBehaviourPunCallbacks //포톤의 event 감지 가능..
{
    public static string username;
    public Button joinButton; // 입장 버튼 
    public TextMeshProUGUI connectionInfoText; //버튼 내 텍스트 
    public int Max = 2; // 4명으로 제한

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //설정 정보를 가지고 마스터 서버에 연결 시도

        joinButton.interactable = false;
    }
    // 마스터 서버에 연결되었을 때 자동 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online!";

        Debug.Log("Connected to master server");
    }
    public override void OnJoinedLobby(){   // 로딩씬으로 이동
        Debug.Log("Joined lobby");
        PhotonNetwork.LoadLevel("LoadingCK");
        CheckLobby();
    }
    public void CheckLobby(){
        Debug.Log("gma");
        if(PhotonNetwork.InLobby){
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("join room!");
        }
        else{
            Debug.Log("뭐징..");
        }
    }
    public void StartMatch(){   // 게임 스타트 버튼 클릭 시 로비로 들어감
        joinButton.interactable = false;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 방 참가에 실패하면 새 방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = Max });
        Debug.Log("create a new Room");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false; //접속버튼 비활성화
        Debug.LogError("Disconnected: " + cause);

        PhotonNetwork.ConnectUsingSettings(); // 접속 실패 시 재접속 시도
    }
}
