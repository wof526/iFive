using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class MapFXManager : MonoBehaviourPunCallbacks
{
    //public GameObject mapFX01;
    //public GameObject mapFX02;
    public GameObject[] areas;
    //public GameObject[] Minimap_areas;


    public float presentime;
    public GameObject[] quads;

    public Material areaYellow;
    public Material areaBlue;
    public Material areaRed;

    private float startTime;
    bool timeover = true;

    static public int randomNum;



    void Start()
    {
        startTime = Time.time;
        areas[0].SetActive(false);
        areas[1].SetActive(false);
        //Minimap_areas[0].SetActive(false);
        //Minimap_areas[1].SetActive(false);
    }


    void Update() //마스터 클라이언트가 대신 실행
    {

        checktime();

    }

    void checktime()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        presentime = Time.time - startTime;
        if (presentime > 10.0f && timeover)
        {
            timeover = false;
            RandomArea();
            //mapFX.SetActive(true);
        }
    }

    void RandomArea() //마스터 클라이언트면? -> 둘중 하나 실행 
    {
        randomNum = Random.Range(0, 2);
        Debug.Log(randomNum);

        //areas[randomNum].SetActive(true);
        //Minimap_areas[randomNum].SetActive(true);
        photonView.RPC("RPCUpdateArea", RpcTarget.All, randomNum);

    }

    [PunRPC]
    private void RPCUpdateArea(int randomNum)
    {
        Debug.Log("punRPC Activated");
        areas[randomNum].SetActive(true);
        //Minimap_areas[randomNum].SetActive(true);
    }


    public void FXchangerBlue()
    {
        quads[0].GetComponent<MeshRenderer>().material = areaBlue;
        Debug.Log("change blue");
    }
    public void FXchangerRed()
    {
        quads[0].GetComponent<MeshRenderer>().material = areaRed;
        Debug.Log("change red");

    }
    public void FXchangerYellow()
    {
        quads[0].GetComponent<MeshRenderer>().material = areaYellow;
        Debug.Log("change yellow");

    }


}