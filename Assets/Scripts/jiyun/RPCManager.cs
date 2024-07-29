using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class RPCManager : MonoBehaviourPun
{
    void RemovePoint(string teamname, int idx){
        Debug.Log("RPC");
        List<Transform> spawnPoints = new List<Transform>();
        Transform parent = GameObject.Find(teamname).transform; // 부모 오브젝트
        foreach(Transform pos in parent){
            if(pos != parent){
                spawnPoints.Add(pos);
            }
        }

        if(idx >= 0 && idx < spawnPoints.Count){
            spawnPoints.RemoveAt(idx);
        }
    }
}
