using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXtriggerManager : MonoBehaviour
{
    public MapFXManager mapFXManager;
    public float areaSecBlue = 60.0f;
    public float areaSecRed = 60.0f;

    private int RedCollisionCount = 0;
    private bool isPaused = false;


    private void OnTriggerStay(Collider collider)
    {
        string coltag = collider.gameObject.tag;


        switch (collider.gameObject.tag)
        {
            case "Team Blue":
                switch (collider.gameObject.tag)
                {
                    case "Team Red":
                        isPaused = true;

                        mapFXManager.FXchangerYellow();
                        Debug.Log("case Blue-red");
                        break;

                    default:
                        isPaused = true;

                        mapFXManager.FXchangerBlue();
                        CountSecBlue();
                        break;
                }
                break;


            case "Team Red":
                switch (collider.gameObject.tag)
                {
                    case "Team Blue":
                        //isPaused = true;

                        mapFXManager.FXchangerYellow();
                        Debug.Log("case Red-blue");
                        break;

                    default:
                        //isPaused = true;

                        mapFXManager.FXchangerRed();
                        CountSecRed();
                        break;
                }
                break;

            default:
                if (isPaused) // pause yellow
                {
                    StartCoroutine(PauseCoroutineBlue(coltag));
                    Debug.Log("pause yellow");
                    //isPaused = false; // restart yellowFX

                }

                else
                {
                    mapFXManager.FXchangerYellow();
                    Debug.Log("case Yellow");
                }
                break;       
        }
    }


    public void CountSecBlue()
    {
        areaSecBlue -= Time.deltaTime;
    }

    public void CountSecRed()
    {
        areaSecRed -= Time.deltaTime;
    }

    //--------------------------------------------------------

            

    private IEnumerator PauseCoroutineBlue(string coltag) // Coroutine : pause yellow
    {   Debug.Log("start coroutine");

        yield return new WaitUntil(() => coltag == "Team Blue"); // pause until all blue
    }

}
