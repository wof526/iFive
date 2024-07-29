using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Track : MonoBehaviour
{
    public TextMeshProUGUI roomNumText; // 룸 이름   
    public Transform spawnPoint;    // 스폰 지점
    public string playerPrefabName = "Player jy";  // 자동차 프리팹

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

        roomNumText.text = PhotonNetwork.CurrentRoom.Name;

        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);
    }
}
