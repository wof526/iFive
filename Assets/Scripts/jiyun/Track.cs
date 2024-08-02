using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Track : MonoBehaviourPunCallbacks
{
    public Joystick jsInstance;
    public TextMeshProUGUI speed_textInstance, Team_name;
    public Image speedbarInstance;

    public static Joystick js;
    public static TextMeshProUGUI speed_text;
    public static Image speedbar;

    private bool hasSpawned = false;    // 스폰되었는가?
    public static string team;  // 팀 이름
    public static bool isfind = false;    // 오브젝트가 찾아졌나?

    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;

        team = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team") 
            ? (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] 
            : "Unknown";
        Debug.Log($"Local player team: {team}");

        // 서버가 스폰 지점을 결정하도록 요청
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master");
        }
    }

    private void Update() {
        if(PhotonNetwork.LocalPlayer.TagObject != null && isfind){
            if(PhotonNetwork.LocalPlayer.TagObject is GameObject player){
                if(team == "Our"){
                    player.tag = "Team Blue";
                }
                else if(team == "Enemy"){
                    player.tag = "Team Red";
                }
                Team_name.text = player.tag;
                isfind = false;
            }    
        }                
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // 커스텀 속성이 업데이트된 플레이어가 로컬 플레이어인지 확인
        if (targetPlayer.IsLocal)
        {
            if (!hasSpawned && AllPlayersHaveTeamProperty())
            {
                Debug.Log("All players have the Team property. Spawning player...");
            }
        }
    }

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