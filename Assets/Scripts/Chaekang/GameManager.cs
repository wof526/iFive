using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DatabaseManager databaseManager;
    public CarInfo CarInfo;
    public Car car;
    public Dash dash;
    public Drive drive;

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

        // 데이터베이스 초기화
        //databaseManager.ResetDatabase();

        // 팀 리스폰 지역 설정
        databaseManager.SaveTeamRespawnArea("teamA", new Vector3(0, 3, 0));
        databaseManager.SaveTeamRespawnArea("teamB", new Vector3(10, 3, 0));

        string userId = auth.CurrentUser?.UserId;

        // 유저 팀 배정 및 리스폰 지역 불러오기
        databaseManager.SaveUserData(userId, "teamA");
    }

    public void RespawnUser(string userId, Vector3 respawnArea)
    {
        GameManager.Instance.car.curSpeed = 0;
        // 유저 리스폰 로직 (예: 유저의 위치를 리스폰 지역으로 이동)
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

    GameObject FindLocalPlayerObject()
    {
        GameObject localPlayerObject = GameObject.FindGameObjectWithTag("Car");
        if (localPlayerObject != null)
        {
            Debug.Log("Local player object found: " + localPlayerObject);
            return localPlayerObject;
        }
        else
        {
            Debug.LogError("Local player object not found!");
            return null;
        }
    }
}
