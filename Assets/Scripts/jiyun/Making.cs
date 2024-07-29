using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Making : MonoBehaviourPunCallbacks
{
    public override void OnJoinRandomFailed(short returnCode, string message){
        // 랜덤 방 참가에 실패하면 새 방을 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        Debug.Log("create a new Room");
    }
    public override void OnDisconnected(DisconnectCause cause){
        Debug.LogError("Disconnected: " + cause);
        PhotonNetwork.ConnectUsingSettings(); // 접속 실패 시 재접속 시도
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.InRoom){
            Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
            Match.username = "jiyunkim";
            PhotonNetwork.AutomaticallySyncScene = true;

        }
        else{
            Debug.Log("Not in room..");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        if(PhotonNetwork.CurrentRoom.PlayerCount == 3){
            PhotonNetwork.LoadLevel("Driving"); //같은 씬을 자동 동기화 함.
        }
    }
}