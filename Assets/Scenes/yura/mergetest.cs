using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
public MapFXManager mapFXManager;
public float areaSecBlue = 60.0f;
public float areaSecRed = 60.0f;

private bool isPaused = false;

private List<string> carList = new List<string>();

private void OnTriggerStay(Collider collider)
{
    string coltag = collider.gameObject.tag;
    Debug.Log("nowtag " + coltag);
    // 어레이
    Debug.Log(carList.Count);


    if (coltag == "Team Blue")
    {
        isPaused = true;
        mapFXManager.FXchangerBlue();
        CountSecBlue();
    }

    else if (coltag == "Team Red")
    {
        isPaused = true;
        mapFXManager.FXchangerRed();
        CountSecRed();
    }

    else
    {
        if (isPaused) //누가 있음
        {
            if (coltag == "Team Blue") // stop yellow
            {
                StartCoroutine(PauseCoroutineBlue(coltag));
                Debug.Log("pause yellow");
                isPaused = false; // restart yellowFX
            }

            else if (coltag == "Team Red") // stop yellow
            {
                StartCoroutine(PauseCoroutineRed(coltag));
                Debug.Log("pause yellow");
                isPaused = false; // restart yellowFX
            }
            //else if (어레이에 요소가 > 2) // when car >= 2
            else if (carList.Count > 2)
            {
                carList.RemoveAt(0);
                carList.RemoveAt(0);
                carList.RemoveAt(0);

                Debug.Log("in count 0");

                isPaused = false;
            }

        }

        else
        {
            mapFXManager.FXchangerYellow();
            Debug.Log("case Yellow");
        }
    }
}


private void OnTriggerEnter(Collider other)
{
    string coltag = other.gameObject.tag;


    if (coltag == "Team Blue")
    {

        // array에 이름추가
        carList.Add("Team Blue");


    }

    else if (coltag == "Team Red")
    {
        carList.Add("Team Red");

    }

}




//--------------------------------------------------------

public void CountSecBlue()
{
    areaSecBlue -= Time.deltaTime;
}

public void CountSecRed()
{
    areaSecRed -= Time.deltaTime;
}




private IEnumerator PauseCoroutineBlue(string coltag) // Coroutine : pause yellow, 
{
    Debug.Log("blue - in coroutine");
    // 조건을 바꾸고 싶음
    yield return new WaitUntil(() => coltag == "Team Blue");
}

private IEnumerator PauseCoroutineRed(string coltag)
{
    Debug.Log("red - in coroutine");

    yield return new WaitUntil(() => coltag == "Team Red");
}
*/