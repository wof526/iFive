using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HPBarRotation : MonoBehaviour
{
    public string cameraName = "Camera"; // ã�ƾ� �� ī�޶��� �̸�

    private Camera targetCamera;
    private Image hpBarImage;

    private Car carScript; 


    void Start()
    {
        // ������ �̸��� ���� ī�޶� ã���ϴ�.
        GameObject cameraObject = GameObject.Find(cameraName);
        if (cameraObject != null)
        {
            targetCamera = cameraObject.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("ī�޶� ã�� �� �����ϴ�: " + cameraName);
        }


        // �ڽ� ������Ʈ �� "hpbar"��� �̸��� ���� ������Ʈ�� ã��
        Transform hpBarTransform = transform.Find("hpbar");
        if (hpBarTransform != null)
        {
            hpBarImage = hpBarTransform.GetComponent<Image>();

            if (hpBarImage != null)
            {
                Debug.Log("hpbar ������Ʈ�� Image ������Ʈ�� ã�ҽ��ϴ�.");
            }
            else
            {
                Debug.Log("hpbar ������Ʈ�� Image ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.Log("hpbar ������Ʈ�� ã�� ���߽��ϴ�.");
        }

        // �θ� �ڽ� ������Ʈ���� Car ��ũ��Ʈ�� ã���ϴ�.
        carScript = GetComponentInParent<Car>();
        if (carScript == null)
        {
            Debug.LogError("Car ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.Log("Car script assigned");
            UpdateHpBar(); // HP �� ������Ʈ
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            // �� ������Ʈ�� ������ ī�޶� �������� ȸ����ŵ�ϴ�.
            transform.LookAt(targetCamera.transform);
        }
        UpdateHpBar();
    }

    // HP �� ������Ʈ �޼���
    void UpdateHpBar()
    {
        if (hpBarImage != null && carScript != null)
        {
            float curHp = carScript.curHP;
            float maxHp = carScript.maxHP;
            Debug.Log("curHP: " + curHp);
            Debug.Log("maxHP: " + maxHp);
            hpBarImage.fillAmount = curHp / maxHp;
            Debug.Log("hpBarImage.filAmount: " + hpBarImage.fillAmount);
        }
    }
}
