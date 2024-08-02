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

    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;

        string team = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team") 
            ? (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] 
            : "Unknown";
        Debug.Log($"Local player team: {team}");

        Team_name.text = team;

        // 서버가 스폰 지점을 결정하도록 요청
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master");
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