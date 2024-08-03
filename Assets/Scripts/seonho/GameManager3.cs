using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager3 : MonoBehaviourPunCallbacks
{
    public static bool isBreak = false; // 버튼이 클릭되었는가

    void Start()
    {
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.SendRate = 30;
        
        SpawnPlayer();
    }

    public void BreakBtn(){ // 브레이크 버튼 클릭 시
        isBreak = true;
    }

    void SpawnPlayer()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber�� 1���� ����
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(spawnIndex);

        if (spawnPoint != null)
        {
            Debug.Log(carData.carString);
            GameObject player = PhotonNetwork.Instantiate(carData.carString, spawnPoint.position, spawnPoint.rotation, 0);
            PhotonNetwork.LocalPlayer.TagObject = player;   // 생성된 객체를 tagobject에 저장
            Track.isfind = true;    // 오브젝트 찾음
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateAllPlayersSpawn", RpcTarget.All);
        }
    }

    [PunRPC]
    void UpdateAllPlayersSpawn()
    {
        SpawnPlayer();
    }
}