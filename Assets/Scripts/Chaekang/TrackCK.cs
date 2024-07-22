using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class TrackCK : MonoBehaviourPun
{
    public TextMeshProUGUI roomNumText; // 룸 이름   
    //public Transform spawnPoint;    // 스폰 지점
    public string playerPrefabName = "Player CK";  // 자동차 프리팹

    public Joystick jsInstance;
    public TextMeshProUGUI speed_textInstance;
    public Image speedbarInstance;

    public static Joystick js;
    public static TextMeshProUGUI speed_text;
    public static Image speedbar;

    //public PhotonView photonView;

    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;

        List<Transform> spawnPoints = new List<Transform>();
        Transform[] teamPos = GameObject.Find("Our").GetComponentsInChildren<Transform>();
        foreach(Transform pos in teamPos){
            if(pos != teamPos[0]){  // 부모 오브젝트는 제외시키기
                spawnPoints.Add(pos);
            }
        }
        int idx = Random.Range(0, spawnPoints.Count);

        GameObject player = PhotonNetwork.Instantiate("Player CK", spawnPoints[idx].position, spawnPoints[idx].rotation);
        spawnPoints.RemoveAt(idx);
        /*string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        if(team == "Our"){
            Spawn("Our");
        }
        else if(team == "Enemy"){
            Spawn("Enemy");
        }*/

        /*if(photonView.IsMine){
            string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            if(team == "Our"){
                Spawn("Our");
            }
            else if(team == "Enemy"){
                Spawn("Enemy");
            }
        }*/
    }

    /*void Spawn(string teamname){
        List<Transform> spawnPoints = new List<Transform>();
        Transform[] teamPos = GameObject.Find(teamname).GetComponentsInChildren<Transform>();
        foreach(Transform pos in teamPos){
            if(pos != teamPos[0]){  // 부모 오브젝트는 제외시키기
                spawnPoints.Add(pos);
            }
        }

        int idx = Random.Range(0, spawnPoints.Count);
        GameObject player = PhotonNetwork.Instantiate("Player jy", spawnPoints[idx].position, spawnPoints[idx].rotation);
        spawnPoints.RemoveAt(idx);  // 이미 스폰된 위치를 제거하여 중복 방지
        photonView.RPC("RemovePoint", RpcTarget.AllBuffered, teamname, idx);
    }


    [PunRPC]
    void RemovePoint(string teamname, int idx){
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
    }*/
}
