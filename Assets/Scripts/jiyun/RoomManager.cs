using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName_input; // 방 이름 설정
    public Transform content;
    public GameObject roomListingPrefab; // 방 리스트 prefab
    public static string roomName;  // 방 이름

    void Start()
    {
        // 지역 설정 (예: 아시아 지역)
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";

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
        roomName = roomName_input.text;
        if(!string.IsNullOrEmpty(roomName)){
            CreateRoomListing(roomName);
        }
    }

    private void CreateRoomListing(string roomName) {   // 방 목록 생성
        GameObject roomListing = Instantiate(roomListingPrefab, content);
        TextMeshProUGUI roomText = roomListing.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (roomText != null)
        {
            roomText.text = roomName;
        }
        else
        {
            Debug.Log("error!");
        }

        Button roomButton = roomListing.GetComponent<Button>();
        if (roomButton != null)
        {
            roomButton.onClick.AddListener(() => OnClickJoinRoom(roomName));
        }
        else
        {
            Debug.Log("no btn!");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패: " + message);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 이름: " + PhotonNetwork.CurrentRoom.Name);   
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("방 목록 업데이트됨. 방 수: " + roomList.Count);

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

            CreateRoomListing(room.Name);
        }
    }

    public void OnClickJoinRoom(string roomName)
    {   // 방 누르면 들어가짐
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions{ MaxPlayers = 4, IsVisible = true, IsOpen = true}, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.LoadLevel("jyLoading"); // 씬 전환
        }     
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2){
            PhotonNetwork.LoadLevel("Driving"); //같은 씬을 자동 동기화 함.
        }
    }
}
