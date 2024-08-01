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
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber는 1부터 시작
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