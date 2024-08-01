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
}