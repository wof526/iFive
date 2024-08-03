using Photon.Pun;
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
    FirestoreManager firestoreManager;
    Car carScript; // Reference to the car script

    private void Start()
    {
        firestoreManager = GameManager.Instance.firestoreManager;

        maxSpeed = firestoreManager.MaxSpeed;
        curSpeed = NetworkPlayer.speed;
        zeroBaek = firestoreManager.ZeroBaek;

        // Start the coroutine to find the car script
        StartCoroutine(FindCarScript());
    }

    private IEnumerator FindCarScript()
    {
        // Keep trying to find the car script until it's found
        while (carScript == null)
        {
            carScript = FindObjectOfType<Car>();

            if (carScript != null)
            {
                fire = carScript.fire; // Access the fire GameObject from the car script
                Debug.Log("Car script found!");
            }
            else
            {
                Debug.LogWarning("Car script not found, retrying...");
            }

            // Wait for the next frame before trying again
            yield return null;
        }
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

        if (!isDash)
        {
            StopCoroutine(ResetDashAfterDelay());
        }
    }

    public void OnDashButtonClick()
    {
        isDash = true;
        Debug.Log("Dash Button Click");
        StartCoroutine(ResetDashAfterDelay());
    }

    private IEnumerator ResetDashAfterDelay()
    {
        Debug.Log("Start Dash Coroutine");
        fire.SetActive(true);
        dashButton.interactable = false;
        yield return new WaitForSeconds(5f);
        dashButton.interactable = true;
        fire.SetActive(false);
        isDash = false;
    }
}
