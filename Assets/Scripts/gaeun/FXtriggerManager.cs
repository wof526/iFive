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
                mapFXManager.FXchangerBlue();
                Debug.Log("case Blue");
                break;

            case "Team Red":
                mapFXManager.FXchangerRed();
                Debug.Log("case Red");
                break;

            default:
                mapFXManager.FXchangerYellow();
                Debug.Log("case Yellow");
                break;
        }
    }
}
