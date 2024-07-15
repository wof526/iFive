using Firebase.Database;
using Firebase;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private DatabaseReference reference;

    void Start()
    {
        // Firebase ÃÊ±âÈ­
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void UpdatePlayerData(string userId, int level, int experience)
    {
        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates["level"] = level;
        updates["experience"] = experience;

        reference.Child("users").Child(userId).UpdateChildrenAsync(updates).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Player data updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to update player data: " + task.Exception);
            }
        });
    }

    public void GetPlayerData(string userId)
    {
        reference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log("Player Level: " + playerData.level);
                    Debug.Log("Player Experience: " + playerData.experience);
                }
                else
                {
                    Debug.LogWarning("No player data found for user: " + userId);
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve player data: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class PlayerData
{
    public int level;
    public int experience;

    public PlayerData(int level, int experience)
    {
        this.level = level;
        this.experience = experience;
    }
}