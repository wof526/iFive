using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public bool isDash = false;
    float maxSpeed;
    float curSpeed;
    float zeroBaek;
    GameObject fire;
    public Button dashButton; // Unity UI Button reference

    // Script
    Car car;
    FirestoreManager firestoreManager;

    private void Awake()
    {
        car = GameManager.Instance.GetCar();
        firestoreManager = GameManager.Instance.firestoreManager;
    }

    private void Start()
    {
        maxSpeed = firestoreManager.MaxSpeed;
        curSpeed = car.curSpeed;
        zeroBaek = firestoreManager.ZeroBaek;

        fire = car.fire;
    }

    void Update()
    {
        // Enable or disable the dash button based on the current speed
        if (curSpeed >= maxSpeed)
        {
            //dashButton.interactable = true;
        }
        else
        {
            //dashButton.interactable = false;
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
