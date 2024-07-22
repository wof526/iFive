using Firebase.Database;
using UnityEngine;

public class FirebaseManager1 : MonoBehaviour
{
    private DatabaseReference dbReference;

    public static object Instance { get; internal set; }

    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void UpdatePlayerHP(string playerId, int hp)
    {
        dbReference.Child("players").Child(playerId).Child("hp").SetValueAsync(hp);
    }

    public void GetPlayerHP(string playerId, System.Action<int> onHpReceived)
    {
        dbReference.Child("players").Child(playerId).Child("hp").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int hp = int.Parse(snapshot.Value.ToString());
                onHpReceived(hp);
            }
        });
    }
}