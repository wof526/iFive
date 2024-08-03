using System;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirestoreManager : MonoBehaviour
{
    // Realtime Database �ν��Ͻ�
    DatabaseReference dbRef;

    // ���� ����
    public float Dash;
    public float Hp;
    public float MaxSpeed;
    public float ZeroBaek;

    public event Action OnDataReady; // �����Ͱ� �غ�Ǿ����� �˸��� �̺�Ʈ

    void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                // �� ������ ��������
                GetCarData(carData.carString);
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
                // �����͸� float�� ��ȯ�Ͽ� �Ҵ�
                Dash = snapshot.Child("Dash").Value != null ? Convert.ToSingle(snapshot.Child("Dash").Value) : 0;
                Hp = snapshot.Child("hp").Value != null ? Convert.ToSingle(snapshot.Child("hp").Value) : 0;
                MaxSpeed = snapshot.Child("MaxSpeed").Value != null ? Convert.ToSingle(snapshot.Child("MaxSpeed").Value) : 0;
                ZeroBaek = snapshot.Child("ZeroBaek").Value != null ? Convert.ToSingle(snapshot.Child("ZeroBaek").Value) : 0;

                // ������ Ȯ��
                Debug.Log($"Dash: {Dash}, Hp: {Hp}, MaxSpeed: {MaxSpeed}, ZeroBaek: {ZeroBaek}");

                // �����Ͱ� �غ�Ǿ����� �˸�
                OnDataReady?.Invoke();
            }
            else
            {
                Debug.Log($"No such document in {carName}!");
            }
        });
    }
}
