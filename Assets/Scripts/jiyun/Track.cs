using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

public class Track : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomNumText; // 룸 이름   
    public Transform spawnPoint;    // 스폰 지점
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

        GameObject player = PhotonNetwork.Instantiate("Player jy", spawnPoint.position, spawnPoint.rotation);
    }
}
