using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MakingCK : MonoBehaviourPunCallbacks
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
            Match.username = "CKSong";
            PhotonNetwork.AutomaticallySyncScene = true;
            //MakeTeams();
        }
        else{
            Debug.Log("Not in room..");
        }
    }

    private void MakeTeams(){   // 팀 나누기
        Player[] players = PhotonNetwork.PlayerList;
        ExitGames.Client.Photon.Hashtable teamProperties = new ExitGames.Client.Photon.Hashtable();
        for(int i = 0; i < players.Length; i++){
            if(i % 2 == 0){ // 아군
                teamProperties["Team"] = "Our";
            }
            else{   // 적군
                teamProperties["Team"] = "Enemy";
            }
            players[i].SetCustomProperties(teamProperties);
        }
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer){
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2){
            PhotonNetwork.LoadLevel("DrivingCK"); //같은 씬을 자동 동기화 함.
        }
    }
}