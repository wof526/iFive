using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

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

    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;    
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player jy", spawnPoint.position, spawnPoint.rotation);
    }
}
