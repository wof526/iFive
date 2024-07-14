using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class yuraTrack : MonoBehaviour
{
    public TextMeshProUGUI roomNumText;

    
    public Transform spawnPoint;
    public string playerPrefabName = "Player";

    public Joystick jsInstance;
    public TextMeshProUGUI speed_textInstance;
    public Image speedbarInstance;

    public static Joystick js;
    public static TextMeshProUGUI speed_text;
    public static Image speedbar;

    // Start is called before the first frame update
    void Start()
    {
        js = jsInstance;
        speed_text = speed_textInstance;
        speedbar = speedbarInstance;

        roomNumText.text = PhotonNetwork.CurrentRoom.Name;

        GameObject player = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
