using UnityEngine;

public class Drive : MonoBehaviour
{
    public bl_Joystick js;
    public float speed;
    public float rotationspeed = 360f;  // 회전 속도
    public float deadzone = 0.1f;   // 무감도 범위

    void Update()
    {
        if(Mathf.Abs(js.Horizontal) < deadzone && Mathf.Abs(js.Vertical) < deadzone){   // 조이스틱을 어느정도 움직여야 이동
            return;
        }

        // 조이스틱의 입력을 받아와서 방향 벡터 설정
        Vector3 direction = new Vector3(js.Horizontal, 0, js.Vertical).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;  // 조이스틱의 방향에 따라 회전

        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationspeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        
        transform.position += direction * speed * Time.deltaTime;
    }
}
