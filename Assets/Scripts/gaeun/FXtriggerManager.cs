using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class CaptureZone : MonoBehaviourPunCallbacks
{
    public GameObject[] areas;

    public TextMeshProUGUI bluecount;
    public TextMeshProUGUI redcount;
    public Image teamCircle;

    public MapFXManager mapFXManager;
    public float areaSecBlue = 60.0f;
    public float areaSecRed = 60.0f;

    private string nowColor;

    private List<GameObject> redObjects = new List<GameObject>();
    private List<GameObject> blueObjects = new List<GameObject>();

    public Material areaYellow;
    public Material areaBlue;
    public Material areaRed;

    static public bool bluewin;

    void Update()
    {

        checkCountdown();
    }

    void checkCountdown()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (nowColor == "Red")
        {
            areaSecRed -= Time.deltaTime;
            photonView.RPC("UpdateRedCountdown", RpcTarget.All, areaSecRed);

            if (areaSecRed <= 0)
            {
                bluewin = false;
                photonView.RPC("LoadNextScene", RpcTarget.All); // "RedWinsScene"을 원하는 씬 이름으로 교체
            }

        }
        else if (nowColor == "Blue")
        {

            areaSecBlue -= Time.deltaTime;
            photonView.RPC("UpdateBlueCountdown", RpcTarget.All, areaSecBlue);
            if (areaSecBlue <= 0)
            {
                bluewin = true;
                photonView.RPC("LoadNextScene", RpcTarget.All); // "BlueWinsScene"을 원하는 씬 이름으로 교체
            }
        }

    }

    [PunRPC]
    private void LoadNextScene()
    {
        PhotonNetwork.LoadLevel("GameOver");
    }
    [PunRPC]
    void UpdateBlueCountdown(float areaSecBlue)
    {
        areaSecBlue = Mathf.Floor(areaSecBlue * 100f) / 100f;
        bluecount.text = areaSecBlue.ToString();

    }

    [PunRPC]
    void UpdateRedCountdown(float areaSecRed)
    {
        areaSecRed = Mathf.Floor(areaSecRed * 100f) / 100f;
        redcount.text = areaSecRed.ToString();

    }


    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.CompareTag("Team Red"))
        {
            redObjects.Add(other.gameObject);
        }
        else if (other.CompareTag("Team Blue"))
        {
            blueObjects.Add(other.gameObject);
        }

        photonView.RPC("UpdateZoneColor", RpcTarget.All, redObjects.Count, blueObjects.Count, MapFXManager.randomNum);
    }

    void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.CompareTag("Team Red"))
        {
            redObjects.Remove(other.gameObject);
        }
        else if (other.CompareTag("Team Blue"))
        {
            blueObjects.Remove(other.gameObject);
        }

        photonView.RPC("UpdateZoneColor", RpcTarget.All, redObjects.Count, blueObjects.Count, MapFXManager.randomNum);
    }

    [PunRPC]
    private void UpdateZoneColor(int redObjects, int blueObjects, int randomNum)
    {

        if (redObjects > 0 && blueObjects > 0)
        {
            areas[randomNum].GetComponent<MeshRenderer>().material = areaYellow; ;  // 두 색상이 모두 있을 때 노란색
            teamCircle.color = Color.yellow;
            nowColor = "Yellow";
        }
        else if (redObjects > 0)
        {
            areas[randomNum].GetComponent<MeshRenderer>().material = areaRed; ;  // 빨간색 오브젝트만 있을 때 빨간색
            teamCircle.color = Color.red;
            nowColor = "Red";
        }
        else if (blueObjects > 0)
        {
            areas[randomNum].GetComponent<MeshRenderer>().material = areaBlue;  // 파란색 오브젝트만 있을 때 파란색
            teamCircle.color = Color.blue;
            nowColor = "Blue";
        }
        else
        {
            areas[randomNum].GetComponent<MeshRenderer>().material = areaYellow;  // 아무도 없을 때 노란색
            teamCircle.color = Color.yellow;
            nowColor = "Yellow";
        }
    }

}

