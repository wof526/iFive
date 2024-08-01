using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXtriggerManager : MonoBehaviour
{
    public MapFXManager mapFXManager;


    private void OnTriggerStay(Collider collider)
    {

        switch (collider.gameObject.tag)
        {
            case "Team Blue":
                switch (collider.gameObject.tag)
                {
                    case "Team Red":
                        mapFXManager.FXchangerYellow();
                        Debug.Log("case Blue");
                        break;

                    default:
                        mapFXManager.FXchangerBlue();
                        break;
                }
                break;


            case "Team Red":
                switch (collider.gameObject.tag)
                {
                    case "Team Blue":
                        mapFXManager.FXchangerYellow();
                        Debug.Log("case Blue");
                        break;

                    default:
                        mapFXManager.FXchangerRed();
                        break;
                }
                break;

            default:
                //mapFXManager.FXchangerYellow();
                Debug.Log("case Yellow");
                break;       
        }
    }
}
