using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool isDash = false;
    float maxSpeed;
    float curSpeed;
    float zeroBaek;
    GameObject fire;
    public Button dashButton; // Unity UI Button reference

    private void Start()
    {
        maxSpeed = GameManager.Instance.CarInfo.maxSpeed;
        curSpeed = GameManager.Instance.car.curSpeed;
        zeroBaek = GameManager.Instance.CarInfo.zeroBaek;

        fire = GameManager.Instance.car.fire;
    }

    void Update()
    {
        // Enable or disable the dash button based on the current speed
        if (curSpeed >= maxSpeed)
        {
            dashButton.interactable = true;
        }
        else
        {
            dashButton.interactable = false;
        }
    }

    public void OnDashButtonClick()
    {
        isDash = true;
        StartCoroutine(ResetDashAfterDelay());
    }

    private IEnumerator ResetDashAfterDelay()
    {
        fire.SetActive(true);
        dashButton.interactable = false;
        yield return new WaitForSeconds(zeroBaek);
        dashButton.interactable = true;
        fire.SetActive(false);
        isDash = false;
    }
}
