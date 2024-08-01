using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarManager : MonoBehaviour
{
    public GameObject hpBarPrefab; // 체력바 프리팹
    private HPBarController hpBarController;
    private GameObject hpBarInstance;
    private Transform cameraTransform;

    private PhotonView photonView;
    private Car carScript;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        photonView = GetComponent<PhotonView>();
        carScript = GetComponent<Car>();

        // 체력바 프리팹을 인스턴스화하고 플레이어의 머리 위에 배치
        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity, transform);
            hpBarInstance.transform.localPosition = new Vector3(0, 2, 0); // 머리 위에 위치하도록 조정
            hpBarController = hpBarInstance.GetComponent<HPBarController>();

            // 초기 체력 설정
            if (carScript != null)
            {
                hpBarController.SetMaxHealth(carScript.maxHP);
                hpBarController.SetCurrentHealth(carScript.curHP);
            }
        }
        else
        {
            Debug.LogError("hpBarPrefab is not assigned.");
        }
    }

    private void Update()
    {
        // 체력바의 위치를 카메라에 맞게 조정
        if (hpBarInstance != null)
        {
            Vector3 hpBarPosition = transform.position + new Vector3(0, 2, 0);
            hpBarInstance.transform.position = Camera.main.WorldToScreenPoint(hpBarPosition);
        }
    }

    // 체력 업데이트 메서드
    public void UpdateHealth(float newHealth)
    {
        if (hpBarController != null)
        {
            hpBarController.SetCurrentHealth(newHealth);
        }
        else
        {
            Debug.LogError("hpBarController is null.");
        }
    }

    // 네트워크로부터 체력 업데이트를 받는 메서드
    [PunRPC]
    public void UpdateHealthFromRPC(float newHealth)
    {
        UpdateHealth(newHealth);
    }
}