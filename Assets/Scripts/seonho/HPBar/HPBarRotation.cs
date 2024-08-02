using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarRotation : MonoBehaviour
{
    public string cameraName = "Camera"; // 찾아야 할 카메라의 이름

    private Camera targetCamera;

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
    }

    void Update()
    {
        if (targetCamera != null)
        {
            // 이 오브젝트를 지정한 카메라 방향으로 회전시킵니다.
            transform.LookAt(targetCamera.transform);
        }
    }
}