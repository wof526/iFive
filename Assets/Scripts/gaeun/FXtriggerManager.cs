using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class CaptureZone : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI bluecount;
    public TextMeshProUGUI redcount;
    public Image teamCircle;

    public MapFXManager mapFXManager;
    public float areaSecBlue = 60.0f;
    public float areaSecRed = 60.0f;

    private string nowColor;

    private List<GameObject> redObjects = new List<GameObject>();
    private List<GameObject> blueObjects = new List<GameObject>();

    void Update()
    {

        checkCountdown();
    }

    void checkCountdown(){

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (nowColor == "Red")
        {
            areaSecRed -= Time.deltaTime;
            photonView.RPC("UpdateRedCountdown", RpcTarget.All, areaSecRed);
            
        }
        else if(nowColor == "Blue")
        {
            
            areaSecBlue -= Time.deltaTime;
            photonView.RPC("UpdateBlueCountdown", RpcTarget.All, areaSecBlue);
        }
        
    }

    [PunRPC]
    void UpdateBlueCountdown()
    {
        areaSecBlue= Mathf.Floor(areaSecBlue * 100f) / 100f;
        bluecount.text = areaSecBlue.ToString();
        
    }

    [PunRPC]
    void UpdateRedCountdown()
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

        photonView.RPC("UpdateZoneColor",RpcTarget.All, redObjects,blueObjects);
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

        photonView.RPC("UpdateZoneColor", RpcTarget.All, redObjects, blueObjects);
    }

    [PunRPC]
    private void UpdateZoneColor(List<GameObject> redObjects, List<GameObject> blueObjects)
    {
        if (redObjects.Count > 0 && blueObjects.Count > 0)
        {
            mapFXManager.FXchangerYellow();  // 두 색상이 모두 있을 때 노란색
            teamCircle.color = Color.yellow;
            nowColor = "Yellow";
        }
        else if (redObjects.Count > 0)
        {
            mapFXManager.FXchangerRed();  // 빨간색 오브젝트만 있을 때 빨간색
            teamCircle.color = Color.red;
            nowColor = "Red";
        }
        else if (blueObjects.Count > 0)
        {
            mapFXManager.FXchangerBlue();  // 파란색 오브젝트만 있을 때 파란색
            teamCircle.color = Color.blue;
            nowColor = "Blue";
        }
        else
        {
            mapFXManager.FXchangerYellow();  // 아무도 없을 때 노란색
            teamCircle.color = Color.yellow;
            nowColor = "Yellow";
        }
    }
}

