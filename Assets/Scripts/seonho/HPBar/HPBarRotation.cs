using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarRotation : MonoBehaviour
{
    public string cameraName = "Camera"; // 찾아야 할 카메라의 이름

    private Camera targetCamera;
    private Image hpBarImage;

    private Car carScript; // Car 스크립트를 저장할 변수

    void Start()
    {
        // 지정한 이름을 가진 카메라를 찾습니다.
        GameObject cameraObject = GameObject.Find(cameraName);
        if (cameraObject != null)
        {
            targetCamera = cameraObject.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("카메라를 찾을 수 없습니다: " + cameraName);
        }

        // 자식 오브젝트 중 "hpbar"라는 이름을 가진 오브젝트를 찾음
        Transform hpBarTransform = transform.Find("hpbar");
        if (hpBarTransform != null)
        {
            hpBarImage = hpBarTransform.GetComponent<Image>();

            if (hpBarImage != null)
            {
                Debug.Log("hpbar 오브젝트의 Image 컴포넌트를 찾았습니다.");
            }
            else
            {
                Debug.Log("hpbar 오브젝트에 Image 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.Log("hpbar 오브젝트를 찾지 못했습니다.");
        }

        // 부모나 자식 오브젝트에서 Car 스크립트를 찾습니다.
        carScript = GetComponentInParent<Car>();
        if (carScript == null)
        {
            Debug.LogError("Car 스크립트를 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("Car script assigned");
            UpdateHpBar(); // HP 바 업데이트
        }
    }

    void Update()
    {
        if (targetCamera != null)
        {
            // 이 오브젝트를 지정한 카메라 방향으로 회전시킵니다.
            transform.LookAt(targetCamera.transform);
        }

        UpdateHpBar();
    }

    // HP 바 업데이트 메서드
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
