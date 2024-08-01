using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    private LevelManager firebaseManager;

    void Start()
    {
        firebaseManager = FindObjectOfType<LevelManager>();

        // 예시: 플레이어 데이터 업데이트
        string playerId = "existing_player_id"; // 이미 연동된 플레이어 ID
        firebaseManager.UpdatePlayerData(playerId, 10, 2500);

        // 예시: 플레이어 데이터 읽기
        firebaseManager.GetPlayerData(playerId);
    }
}