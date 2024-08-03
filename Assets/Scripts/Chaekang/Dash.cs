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
    Button dashButton; // Unity UI Button reference

    // Script
    FirestoreManager firestoreManager;
    Car carScript; // Reference to the car script

    private void Start()
    {
        firestoreManager = GameManager.Instance.firestoreManager;

        maxSpeed = firestoreManager.MaxSpeed;
        curSpeed = NetworkPlayer.speed;
        zeroBaek = firestoreManager.ZeroBaek;

        // Find the dash button by its name
        GameObject dashButtonObject = GameObject.Find("Dash");
        if (dashButtonObject != null)
        {
            dashButton = dashButtonObject.GetComponent<Button>();
        }
        else
        {
            Debug.LogError("Dash button not found!");
        }

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
        if (dashButton != null)
        {
            dashButton.interactable = curSpeed >= maxSpeed;
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
        if (fire != null)
        {
            fire.SetActive(true);
        }
        if (dashButton != null)
        {
            dashButton.interactable = false;
        }
        yield return new WaitForSeconds(5f);
        if (dashButton != null)
        {
            dashButton.interactable = true;
        }
        if (fire != null)
        {
            fire.SetActive(false);
        }
        isDash = false;
    }
}
