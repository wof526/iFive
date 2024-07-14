using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class yuraDrive : MonoBehaviour
{
    
    public float rotationspeed = 180f;  // 회전 속도
    public float deadzone = 0.1f;   // 무감도 범위
    public static float speed;    // 속력
    public static bool isBreak = false; // 브레이크 버튼을 눌렀는가?
      // 속력바
      // 입장 안내

    void Start()
    {
        if (Match.username != null)
        {
            //startinfo.text = Match.username + " has entered";
        }
    }

    void Update()
    {
        if (Mathf.Abs(yuraTrack.js.Horizontal) < deadzone && Mathf.Abs(yuraTrack.js.Vertical) < deadzone)
        {   // 조이스틱을 어느정도 움직여야 이동
            return;
        }

        if (!isBreak)
        {
            // 조이스틱의 입력을 받아와서 방향 벡터 설정
            Vector3 direction = new Vector3(yuraTrack.js.Horizontal, 0, yuraTrack.js.Vertical).normalized;
            float magnitude = new Vector3(yuraTrack.js.Horizontal, 0, yuraTrack.js.Vertical).magnitude;
            speed = magnitude * 25; // 차종마다 magnitude에 곱해주는 값 다르게 하면 될 듯

            if (magnitude < 0.2)
            {    // 조이스틱을 놓을때에도 magnitude가 미미하게(?) 생기므로 무시하게 함
                speed = 0;
            }

            Debug.Log(speed);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;  // 조이스틱의 방향에 따라 회전

            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationspeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            transform.position += direction * speed * Time.deltaTime;
        }

        yuraTrack.speedbar.fillAmount = speed / 100;
        yuraTrack.speed_text.text = (int)speed + "km/h";
    }

    public void Break()
    {
        isBreak = true;
        transform.position = Vector3.zero;  // 정지!
        speed = 0;
    }
}
