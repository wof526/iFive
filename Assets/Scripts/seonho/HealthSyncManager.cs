using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSyncManager : MonoBehaviourPun
{
    private Car carScript;
    private HPBarManager hpBarManager;

    private void Start()
    {
        carScript = GetComponent<Car>();
        hpBarManager = GetComponent<HPBarManager>();

        if (carScript != null && hpBarManager != null)
        {
            // 초기 체력 정보를 다른 클라이언트에게 전달
            photonView.RPC("UpdateHealthFromRPC", RpcTarget.All, carScript.curHP);
        }
    }

    private void Update()
    {
        // 체력 정보가 변경되었는지 확인
        if (carScript != null && hpBarManager != null)
        {
            // 체력 정보를 네트워크로 전송
            photonView.RPC("UpdateHealthFromRPC", RpcTarget.All, carScript.curHP);
        }
    }

    [PunRPC]
    public void UpdateHealthFromRPC(float newHealth)
    {
        if (hpBarManager != null)
        {
            hpBarManager.UpdateHealth(newHealth);
        }
    }
}