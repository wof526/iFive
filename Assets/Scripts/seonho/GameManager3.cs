using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager3 : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.SendRate = 30;
        if (playerPrefab != null)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.LogError("Player Prefab is not assigned.");
        }
    }

    void SpawnPlayer()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber�� 1���� ����
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(spawnIndex);

        if (spawnPoint != null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
        }
        else
        {
            Debug.LogError("Spawn point not found for index: " + spawnIndex);
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