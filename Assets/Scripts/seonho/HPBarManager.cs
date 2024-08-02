using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarManager : MonoBehaviour
{
    public GameObject hpBarPrefab; // ü�¹� ������
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

        // ü�¹� �������� �ν��Ͻ�ȭ�ϰ� �÷��̾��� �Ӹ� ���� ��ġ
        if (hpBarPrefab != null)
        {
            hpBarInstance = Instantiate(hpBarPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity, transform);
            hpBarInstance.transform.localPosition = new Vector3(0, 2, 0); // �Ӹ� ���� ��ġ�ϵ��� ����
            hpBarController = hpBarInstance.GetComponent<HPBarController>();

            // �ʱ� ü�� ����
            if (carScript != null)
            {
                //hpBarController.SetMaxHealth(carScript.maxHP);
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
        // ü�¹��� ��ġ�� ī�޶� �°� ����
        if (hpBarInstance != null)
        {
            Vector3 hpBarPosition = transform.position + new Vector3(0, 2, 0);
            hpBarInstance.transform.position = Camera.main.WorldToScreenPoint(hpBarPosition);
        }
    }

    // ü�� ������Ʈ �޼���
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

    // ��Ʈ��ũ�κ��� ü�� ������Ʈ�� �޴� �޼���
    [PunRPC]
    public void UpdateHealthFromRPC(float newHealth)
    {
        UpdateHealth(newHealth);
    }
}