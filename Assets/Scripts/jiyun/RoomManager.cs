using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName_input; // 방 이름 설정
    public Transform content;
    public GameObject roomListingPrefab; // 방 리스트 prefab

    void Start()
    {   
        // 서버에 연결
        PhotonNetwork.ConnectUsingSettings();

         // 자동 씬 동기화 활성화
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 참가");
    }

    public void SetRoom(){  // 버튼 클릭 시 방 생성
        string roomName = roomName_input.text;
        if(!string.IsNullOrEmpty(roomName)){
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 이름: " + PhotonNetwork.CurrentRoom.Name);
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel("Driving"); // 씬 전환
        }        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed: " + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject); // 기존 방 목록 삭제
        }

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                continue; // 제거된 방은 리스트에 표시하지 않음
            }

            // 방 새로 추가
            GameObject roomListing = Instantiate(roomListingPrefab, content);
            TextMeshProUGUI roomText = roomListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
               
            if(roomText != null){
                roomText.text = room.Name;
            }    
            else{
                Debug.Log("error!");
            }

            Button roomButton = roomListing.GetComponent<Button>();
            if(roomButton != null){ // 방 목록으로 뜨는 버튼
                roomButton.onClick.AddListener(() => OnClickJoinRoom(room.Name));
            }
            else{
                Debug.Log("no btn!");
            }
        }
    }

    public void OnClickJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed: " + message);
    }
}
