using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public FirestoreManager firestoreManager;
    public Dash dash;
    public Drive drive;

    private Car car;
    private FirebaseAuth auth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(FindAndAssignCar());

        string userId = auth.CurrentUser?.UserId;
    }

    IEnumerator FindAndAssignCar()
    {
        while (car == null)
        {
            car = FindLocalPlayerCar();
            if (car == null)
            {
                Debug.LogWarning("Car object not found. Retrying...");
                yield return new WaitForSeconds(0.1f); // 1초 후에 다시 시도
            }
            else
            {
                Debug.Log("Car object found and assigned.");
            }
        }
    }

    Car FindLocalPlayerCar()
    {
        GameObject localPlayerObject = FindLocalPlayerObject();
        if (localPlayerObject != null)
        {
            return localPlayerObject.GetComponent<Car>();
        }
        return null;
    }

    GameObject FindLocalPlayerObject()
    {
        foreach (GameObject carObject in GameObject.FindGameObjectsWithTag("Car"))
        {
            PhotonView photonView = carObject.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                Debug.Log("Local player object found: " + carObject);
                return carObject;
            }
        }
        return null;
    }

    public void RespawnUser(string userId, Vector3 respawnArea)
    {
        if (car != null)
        {
            car.curSpeed = 0;
        }

        Debug.Log($"Respawning user {userId} at {respawnArea}");

        // 실제 유저 오브젝트 이동 로직 구현
        GameObject userObject = FindLocalPlayerObject();
        if (userObject != null)
        {
            userObject.transform.position = respawnArea;
        }
        else
        {
            Debug.LogError("User object not found!");
        }
    }

    // Public 메서드를 통해 Car 객체를 가져오기
    public Car GetCar()
    {
        return car;
    }
}
