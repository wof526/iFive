using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirestoreManager : MonoBehaviour
{
    // 파이어스토어 인스턴스
    FirebaseFirestore db;

    // 변수 선언
    public float Dash;
    public float Hp;
    public float MaxSpeed;
    public float ZeroBaek;

    public event Action OnDataReady; // 데이터가 준비되었음을 알리는 이벤트

    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.DefaultInstance;

            // Sepia 데이터 가져오기
            GetCarData("Sepia");
        });
    }

    void GetCarData(string carName)
    {
        DocumentReference docRef = db.Collection("carInfo").Document(carName);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log($"Document data for {carName} retrieved successfully!");

                    // 데이터를 float로 변환하여 할당
                    Dash = snapshot.GetValue<float>("Dash");
                    Hp = snapshot.GetValue<float>("Hp");
                    MaxSpeed = snapshot.GetValue<float>("MaxSpeed");
                    ZeroBaek = snapshot.GetValue<float>("ZeroBaek");

                    // 데이터 확인
                    Debug.Log($"Dash: {Dash}, Hp: {Hp}, MaxSpeed: {MaxSpeed}, ZeroBaek: {ZeroBaek}");

                    // 데이터가 준비되었음을 알림
                    OnDataReady?.Invoke();
                }
                else
                {
                    Debug.Log($"No such document in {carName}!");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve document: " + task.Exception);
            }
        });
    }
}
