using System;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirestoreManager : MonoBehaviour
{
    // Realtime Database 인스턴스
    DatabaseReference dbRef;

    // 변수 선언
    public float Dash;
    public float Hp;
    public float MaxSpeed;
    public float ZeroBaek;

    public event Action OnDataReady; // 데이터가 준비되었음을 알리는 이벤트

    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                // 차 데이터 가져오기
                GetCarData("Titan-3");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
            }
        });
    }

    void GetCarData(string carName)
    {
        DatabaseReference carRef = dbRef.Child("carInfo").Child(carName);
        carRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve document: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log($"Document data for {carName} retrieved successfully!");

                // 데이터를 float로 변환하여 할당
                Dash = snapshot.Child("Dash").Value != null ? Convert.ToSingle(snapshot.Child("Dash").Value) : 0;
                Hp = snapshot.Child("hp").Value != null ? Convert.ToSingle(snapshot.Child("hp").Value) : 0;
                MaxSpeed = snapshot.Child("MaxSpeed").Value != null ? Convert.ToSingle(snapshot.Child("MaxSpeed").Value) : 0;
                ZeroBaek = snapshot.Child("ZeroBaek").Value != null ? Convert.ToSingle(snapshot.Child("ZeroBaek").Value) : 0;

                // 데이터 확인
                Debug.Log($"Dash: {Dash}, Hp: {Hp}, MaxSpeed: {MaxSpeed}, ZeroBaek: {ZeroBaek}");

                // 데이터가 준비되었음을 알림
                OnDataReady?.Invoke();
            }
            else
            {
                Debug.Log($"No such document in {carName}!");
            }
        });
    }
}
