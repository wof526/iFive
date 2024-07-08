using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class CarSelector : MonoBehaviour
{
    [Header("Button")]
    public Button sephiaButton;
    public Button honeyBeeButton;
    public Button startButton;

    [Header("Prefab")]
    public GameObject sephiaPrefab;   // Sephia 프리팹
    public GameObject honeyBeePrefab; // HoneyBee 프리팹

    private CarInfo selectedCar;
    private string selectedCarName;

    private FirebaseAuth auth;         // Firebase 인증 관리
    private DatabaseReference databaseReference;  // Firebase 데이터베이스
    private FirebaseUser currentUser;  // 현재 로그인한 유저

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // 사용자 로그인 상태 확인
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);   // 초기 인증상태 확인

        sephiaButton.onClick.AddListener(() => SelectCar(sephiaPrefab, "Sephia"));
        honeyBeeButton.onClick.AddListener(() => SelectCar(honeyBeePrefab, "HoneyBee"));
        startButton.onClick.AddListener(SendSelectedCarDataToFirebase);
    }

    // Firebase 인증 상태 변경 확인
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        // 현재 유저 변경됐는지 확인
        if (auth.CurrentUser != currentUser)
        {
            bool signedIn = currentUser != auth.CurrentUser && auth.CurrentUser != null;
            // 로그아웃 했는지 확인
            if (!signedIn && currentUser != null)
            {
                Debug.Log("Signed out " + currentUser.UserId);
            }
            currentUser = auth.CurrentUser;  // 유저 업데이트
            // 새로 로그인한 유저 있는지 확인
            if (signedIn)
            {
                Debug.Log("Signed in " + currentUser.UserId);
            }
        }
    }

    // 차 선택
    void SelectCar(GameObject carPrefab, string carName)
    {
        if (carPrefab != null)
        {
            selectedCar = carPrefab.GetComponent<CarInfo>();
            selectedCarName = carName;
            if (selectedCar != null)
            {
                Debug.Log($"{selectedCarName} selected with Max HP: {selectedCar.maxHp}");
            }
            else
            {
                Debug.LogError($"{carPrefab.name} prefab does not have a CarInfo component");
            }
        }
        else
        {
            Debug.LogError("Selected car prefab not found");
        }
    }

    // 선택된 차 데이터를 Firebase에 전송
    void SendSelectedCarDataToFirebase()
    {
        if (selectedCar != null && !string.IsNullOrEmpty(selectedCarName) && currentUser != null)
        {
            CarData carData = new CarData
            {
                carName = selectedCarName,
                maxHp = selectedCar.maxHp,
                maxSpeed = selectedCar.maxSpeed,
                dashSpeed = selectedCar.dashSpeed,
                zeroBaek = selectedCar.zeroBaek
            };

            string json = JsonUtility.ToJson(carData);   // CarData를 JSON 형식으로 변환
            string userId = currentUser.UserId;          // 로그인한 사용자의 UID
            databaseReference.Child("users").Child(userId).Child("selectedCar").SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Data sent successfully");
                }
                else
                {
                    Debug.LogError("Error sending data: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("No car selected or user not logged in");
        }
    }
}

[System.Serializable]
public class CarData
{
    public string carName;
    public float maxHp;
    public float maxSpeed;
    public float dashSpeed;
    public float zeroBaek;
}
