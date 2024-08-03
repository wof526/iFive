using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXtriggerManager : MonoBehaviour
{
    public MapFXManager mapFXManager;
    public float areaSecBlue = 60.0f;
    public float areaSecRed = 60.0f;

    private bool nowEnter = false;
    private bool blueEnter = false;
    private bool redEnter = false;

    private bool isPaused = false;

    private List<string> carList = new List<string>();


    //안에 오브젝트가 계속 있을때 업데이트 식으로 불러 
    private void OnTriggerEnter(Collider collider)
    {
        string coltag = collider.gameObject.tag;
        Debug.Log("nowtag " + coltag);
        // ���

        if (nowEnter = false) //아무도 들어온 사람이 없을떄 
        {
            if (coltag == "Team Blue")//파란팀이면
            {
                mapFXManager.FXchangerBlue();
                blueEnter = true;
                nowEnter = true;
            }
            else if (coltag == "Team Red")//빨간팀이면 
            {
                mapFXManager.FXchangerRed();
                redEnter = true;
                nowEnter = true;
            }
        }
        else //들어온 사람이 있을때
        {
            if (blueEnter == true) //파란팀이 들어와 있을때 
            {
                if (coltag == "Team Red") //빨간팀이 들어오면 
                {
                    //파란팀 카운팅 멈춤
                    mapFXManager.FXchangerYellow(); //노란색으로 바꿈
                }
            }
            else if (redEnter == true) //레드팀이 들어와 있을때
            {
                if (coltag == "Team Blue")
                {
                    //레드팀 카운팅 멈
                    mapFXManager.FXchangerYellow();
                }
            }

        }
    }

    private void OnTriggerStay(Collider collider)
    {
        string coltag = collider.gameObject.tag;
    }

    public void CountSecBlue()
    {
        areaSecBlue -= Time.deltaTime;
    }

    public void CountSecRed()
    {
        areaSecRed -= Time.deltaTime;
    }


    /*
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

    else//clotag 3개 -> untagged
    {
        if (isPaused)
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
            //else if (��̿� ��Ұ� > 2) // when car >= 2
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



//닿았을 때.
private void OnTriggerEnter(Collider other)
{
    string coltag = other.gameObject.tag;


    if (coltag == "Team Blue")
    {

        // array�� �̸��߰�
        carList.Add("Team Blue");


    }

    else if (coltag == "Team Red")
    {
        carList.Add("Team Red");

    }

}

*/


    //--------------------------------------------------------


    /*

    private IEnumerator PauseCoroutineBlue(string coltag) // Coroutine : pause yellow
    {
        Debug.Log("blue - in coroutine");
        // ������ �ٲٰ� ����
        yield return new WaitUntil(() => coltag == "Team Blue");
    }

    private IEnumerator PauseCoroutineRed(string coltag)
    {
        Debug.Log("red - in coroutine");

        yield return new WaitUntil(() => coltag == "Team Red");
    }
    */

    /*
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if ((filterTags?.Count ?? 0) > 0 && !filterTags.Contains(obj.tag))
        {
            return;
        }

        if (host)
        {
            host.OnTriggerEnter(other);
        }

        if (detectCounter.TryGetValue(obj, out var cnt))
        {
            detectCounter[obj] = ++cnt;
        }
        else
        {
            detectCounter[obj] = 1;
            onEnter.Invoke(obj);
        }
    }
    */
    //--------------------------------------------------------

    /*switch (coltag)
    {
        case "Team Blue":
            isPaused = true;
            mapFXManager.FXchangerBlue();
            CountSecBlue();
            Debug.Log(coltag);
            if (coltag == "Untagged")
            {
                isPaused = false;
            }
            break;   


        case "Team Red":
            isPaused = true;

            mapFXManager.FXchangerRed();
            CountSecRed();
            break;


        case "Untagged": // �ǳ�?

            mapFXManager.FXchangerYellow();
            Debug.Log("case Yellow");
            break;

        default:

            if (isPaused) // pause yellow
            { /// if���ƴ϶� case�� �ؾ�? �ϴ� ����׷� ������ Ȯ��
                //Debug.Log(isPaused);
                //Debug.Log(collider.gameObject.tag);
                //Debug.Log(coltag);


                /*
                switch (collider.gameObject.tag)
                {
                    case "Team Blue":
                        Debug.Log("isPaused == true, case blue");

                        StartCoroutine(PauseCoroutineBlue(collider.gameObject.tag));
                        isPaused = false; // restart yellowFX

                        break;

                    case "Team Red":
                        Debug.Log("isPaused == true, case red");

                        StartCoroutine(PauseCoroutineRed(collider.gameObject.tag));
                        isPaused = false; // restart yellowFX

                        break;

                    default:
                        Debug.Log("Unhandled coltag value: " + collider.gameObject.tag);
                        break;
                        // blue && red �Ѵ� �����ϸ� ��������� ó���ϴ� ���̽� �߰�?
                        // �ٵ� case�� ���ǹ����� �۵��ϴ��� �𸣰ھ Ȯ���غ���
                }

            }

            else // �۵�����
            {
                mapFXManager.FXchangerYellow();
                Debug.Log("case Yellow");
            }
            break;  */


    /*
    private void OnTriggerExit(Collider other) // yellow ����
    {
        string coltag = other.gameObject.tag;
        Debug.Log(coltag);

        if (coltag == "Team Blue")
        {
            mapFXManager.FXchangerYellow();
            Debug.Log("case Yellow");
        }

        else
        {
            mapFXManager.FXchangerYellow();
            Debug.Log("case Yellow");
        }
    }*/




}
