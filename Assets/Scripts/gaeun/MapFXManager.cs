using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapFXManager : MonoBehaviour
{
    //public GameObject mapFX01;
    //public GameObject mapFX02;
    public GameObject[] areas;
    public GameObject[] Minimap_areas;


    public float presentime;
    public GameObject[] quads;

    public Material areaYellow;
    public Material areaBlue;
    public Material areaRed;

    int randomNum;



    void Start()
    {        
        areas[0].SetActive(false);
        areas[1].SetActive(false);
        Minimap_areas[0].SetActive(false);
        Minimap_areas[1].SetActive(false);
        randomNum = Random.Range(0, 2);
        Debug.Log(randomNum);
    }


    void Update()
    {
        presentime = Time.time;

        if(Time.time > 10.0f)
        {
            RandomArea();
            //mapFX.SetActive(true);
        }

        
    }

    void RandomArea()
    {
        areas[0].SetActive(true);
        Minimap_areas[0].SetActive(true);
        //areas[randomNum].SetActive(true);
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
