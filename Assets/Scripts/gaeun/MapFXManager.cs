using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapFXManager : MonoBehaviour
{
    public GameObject mapFX;
    public float presentime;
    public GameObject quad;

    public Material areaYellow;
    public Material areaBlue;
    public Material areaRed;



    void Start()
    {
        mapFX.SetActive(false);
    }


    void Update()
    {
        presentime = Time.time;

        if(Time.time > 10.0f)
        {
            mapFX.SetActive(true);
        }

        
    }

    /*
    private void OnTriggerStay(Collider collider)
    {
        mapFX.GetComponent<CapsuleCollider>();


        switch (collider.gameObject.tag)
        {
            case "Team Blue":
                FXchangerBlue();
                Debug.Log("case Blue");
                break;

            case "Team Red":
                FXchangerRed();
                Debug.Log("case Red");
                break;

            default:
                FXchangerYellow();
                Debug.Log("case Yellow");
                break;
        }
    }*/

    public void FXchangerBlue()
    {
        quad.GetComponent<MeshRenderer>().material = areaBlue;
        Debug.Log("change blue");
    }
    public void FXchangerRed()
    {
        quad.GetComponent<MeshRenderer>().material = areaRed;
        Debug.Log("change red");

    }
    public void FXchangerYellow()
    {
        quad.GetComponent<MeshRenderer>().material = areaYellow;
        Debug.Log("change yellow");

    }


}
