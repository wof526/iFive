using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Track : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomNumText; // 룸 이름   
    public Joystick jsInstance;
    public TextMeshProUGUI speed_textInstance;
    public Image speedbarInstance;

    public static Joystick js;
    public static TextMeshProUGUI speed_text;
    public static Image speedbar;

    private bool hasSpawned = false;    // 스폰되었는가?

    private PhotonView photonView;

    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;

        /*string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        Debug.Log($"Local player team: {team}");*/

        string team = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team") 
            ? (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] 
            : "Unknown";
        Debug.Log($"Local player team: {team}");

        // 서버가 스폰 지점을 결정하도록 요청
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master");
            RequestSpawnPoints();
        }

        //UpdatePlayerTeam();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // 커스텀 속성이 업데이트된 플레이어가 로컬 플레이어인지 확인
        if (targetPlayer.IsLocal)
        {
            //UpdatePlayerTeam();
            if (!hasSpawned && AllPlayersHaveTeamProperty())
            {
                Debug.Log("All players have the Team property. Spawning player...");
                RequestSpawnPoints();
                hasSpawned = true; // 스폰 후 플래그를 true로 설정
            }
        }
    }

    /*private void UpdatePlayerTeam()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log($"Local player team: {team}");

            // 팀에 따라 다른 스폰 위치 설정
            if (team == "Our")
            {
                SetSpawnPoints("Our");
            }
            else if (team == "Enemy")
            {
                SetSpawnPoints("Enemy");
            }
        }
        else
        {
            Debug.Log("Team property not found.");
        }
    }*/

    /*private void SetSpawnPoints(string team)
    {
        List<Transform> spawnPoints = new List<Transform>();
        Transform[] teamPos = GameObject.Find(team).GetComponentsInChildren<Transform>();
        foreach (Transform t in teamPos)
        {
            if (t != teamPos[0])
            {
                spawnPoints.Add(t);
            }
        }

        int idx = Random.Range(0, spawnPoints.Count);
        GameObject player = PhotonNetwork.Instantiate("Sephia", spawnPoints[idx].position, spawnPoints[idx].rotation);
        spawnPoints.RemoveAt(idx);
    }*/

    private bool AllPlayersHaveTeamProperty()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("Team"))
            {
                return false;
            }
        }
        return true;
    }

    private void RequestSpawnPoints()
    {
        photonView = GetComponent<PhotonView>();

        if(photonView == null){
            Debug.LogError("No photonView!");
            return;
        }
        // 서버에서 스폰 지점을 결정하고 모든 클라이언트에게 전송
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("request");
            photonView.RPC("AssignSpawnPoints", RpcTarget.All);
        }
    }

    [PunRPC]
    public void AssignSpawnPoints()
    {
        Debug.Log("Rpc");
        string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        List<Transform> spawnPoints = new List<Transform>();

        // 각 팀의 스폰 지점 가져오기
        Transform[] teamPos = GameObject.Find(team).GetComponentsInChildren<Transform>();
        foreach (Transform t in teamPos)
        {
            if (t != teamPos[0])
            {
                spawnPoints.Add(t);
            }
        }

        // 스폰 지점 랜덤 선택
        if (spawnPoints.Count > 0)
        {
            int idx = Random.Range(0, spawnPoints.Count);
            Vector3 spawnPosition = spawnPoints[idx].position;
            Quaternion spawnRotation = spawnPoints[idx].rotation;

            // 선택한 스폰 지점 정보를 커스텀 속성에 저장
            ExitGames.Client.Photon.Hashtable spawnProps = new ExitGames.Client.Photon.Hashtable
            {
                ["SpawnPos"] = spawnPosition,
                ["SpawnRot"] = spawnRotation
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(spawnProps);

            Debug.Log("spawnpoint 성공!");
            // RPC를 통해 모든 클라이언트에게 스폰 지점 정보 전송
            photonView.RPC("ReceiveSpawnPoints", RpcTarget.All, spawnPosition, spawnRotation);
        }
    }

    [PunRPC]
    public void ReceiveSpawnPoints(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Debug.Log("receive 시작");
        if (!hasSpawned)
        {
            Debug.LogWarning("if문 안으로 들어옴");
            // 스폰 지점 정보를 받아서 플레이어를 스폰합니다.
            PhotonNetwork.Instantiate("Sephia", spawnPosition, spawnRotation);
            hasSpawned = true; // 스폰 후 플래그를 true로 설정
            Debug.Log("receive 성공!");
        }
    }
}
