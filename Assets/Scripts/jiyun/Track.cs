using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class Track : MonoBehaviourPunCallbacks
{
    public Joystick jsInstance;
    public TextMeshProUGUI speed_textInstance;
    public Slider speedBar;

    public static Joystick js;
    public static TextMeshProUGUI speed_text;
    public static Slider speedbar;

    private bool hasSpawned = false;    // 스폰되었는가?
    public static string team;  // 팀 이름
    public static bool isfind = false;    // 오브젝트가 찾아졌나?

    void Start()
    {
        // Play BGM
        AudioManager.instance.PlayBgm(true);

        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedBar;

        team = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team") 
            ? (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"] 
            : "Unknown";
    }

    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.TagObject != null && isfind)
        {
            if (PhotonNetwork.LocalPlayer.TagObject is GameObject player)
            {
                if (team == "Our")
                {
                    photonView.RPC("ChangePlayerTag", RpcTarget.AllBuffered, player.GetPhotonView().ViewID, "Team Blue");
                }
                else if (team == "Enemy")
                {
                    photonView.RPC("ChangePlayerTag", RpcTarget.AllBuffered, player.GetPhotonView().ViewID, "Team Red");
                }
                isfind = false;
            }
        }
    }

    [PunRPC]
    void ChangePlayerTag(int viewID, string newTag)
    {
        GameObject player = PhotonView.Find(viewID).gameObject;
        player.tag = newTag;
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