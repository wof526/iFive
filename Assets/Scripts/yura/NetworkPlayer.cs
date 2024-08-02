using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    public float rotationspeed = 180f;  // 회전 속도
    public float deadzone = 0.1f;   // 무감도 범위
    public static float speed;    // 속력
    public static bool isBreak = false; // 브레이크 버튼을 눌렀는가?
    private Rigidbody rb;
    private Vector3 lastDirection = Vector3.zero;   // 회전하고 마지막 방향 저장
    private float targetSpeed;  // 목표 속도
    public float speedSmoothTime = 1f;    // 속도 전환을 스무스하게 하기 위한 시간
    private float turnSmoothVelocity, speedSmoothVelocity;   // 회전과 속도변화를 스무스하게 하기 위한 속도


    private Vector3 networkPosition;
    private Quaternion networkRotation;

    public GameObject localCam;
    private PhotonView photonView;
   
    

// Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        photonView = GetComponent<PhotonView>();

        if (photonView == null)
        {
            Debug.LogError("PhotonView not found in networkPlayer!");
        }

        if (!photonView.IsMine)
        {
            localCam.SetActive(false);

            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

            for(int i = 0; i< scripts.Length; i++)
            {
                if (scripts[i] is NetworkPlayer) continue;
                else if (scripts[i] is PhotonView) continue;

                scripts[i].enabled = false;
            }
        }

        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    void Update()
    {
        
        if (photonView.IsMine)
        {
            float horizontal = Track.js.Horizontal;//조이스틱의 수평값을 가져온다
            float vertical = Track.js.Vertical; //조이스틱의 수직값을 가져온다

            if (Mathf.Abs(Track.js.Horizontal) < deadzone && Mathf.Abs(Track.js.Vertical) < deadzone)
            {   // 조이스틱을 어느정도 움직여야 이동
                return;
            }

            if (!isBreak)
            {
                // 조이스틱의 입력을 받아와서 방향 벡터 설정
                Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
                float magnitude = new Vector3(horizontal, 0, vertical).magnitude;
                targetSpeed = magnitude * 2; // 차종마다 magnitude에 곱해주는 값 다르게 하면 될 듯

                if (magnitude < 0.2f)
                {    // 조이스틱을 놓을때에도 magnitude가 미미하게(?) 생기므로 무시하게 함
                    targetSpeed = 0;
                }
                else
                {
                    if (vertical > 0)
                    {   // 차량의 전방을 기준으로 전진
                        lastDirection = transform.forward;
                    }
                    else if (vertical < 0)
                    {  // 현재 차량의 후방을 기준으로 후진
                        lastDirection = -transform.forward;
                    }
                }

                speed = Mathf.SmoothDamp(speed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

                if (magnitude > 0.2f && vertical > 0)
                {   // 전진 시에만 회전 및 lastDirection 갱신
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;  // 조이스틱의 방향에 따라 회전
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, speedSmoothTime);
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    lastDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                }

                // 마지막 방향을 따라 이동
                transform.position += lastDirection * speed * Time.deltaTime;
            }
            Track.speedbar.fillAmount = speed / 100;
            Track.speed_text.text = (int)speed + "km/h";
        }
        else
        {
            float lerpSpeed = 5f;

            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);


        }
    }


        /*
        if (photonView.IsMine)
        {
            if (Mathf.Abs(Track.js.Horizontal) < deadzone && Mathf.Abs(Track.js.Vertical) < deadzone)
            {   // 조이스틱을 어느정도 움직여야 이동
                return;
            }

            if (!isBreak)
            {
                // 조이스틱의 입력을 받아와서 방향 벡터 설정
                Vector3 direction = new Vector3(Track.js.Horizontal, 0, Track.js.Vertical).normalized;
                float magnitude = new Vector3(Track.js.Horizontal, 0, Track.js.Vertical).magnitude;
                speed = magnitude; // 차종마다 magnitude에 곱해주는 값 다르게 하면 될 듯

                if (magnitude < 0.2)
                {    // 조이스틱을 놓을때에도 magnitude가 미미하게(?) 생기므로 무시하게 함
                    speed = 0;
                }

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;  // 조이스틱의 방향에 따라 회전

                float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationspeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0, angle, 0);
                transform.position += direction * speed * Time.deltaTime;
                

                
            }
       
        }
        else
        {
            float lerpSpeed = 5f;

            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * lerpSpeed);


        }
    }
        */


    public void Break()
    {
        isBreak = true;
        transform.position = Vector3.zero;  // 정지!
        speed = 0;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            // 특정 거리 이상의 위치 차이는 직접 적용
            if (Vector3.Distance(transform.position, networkPosition) > 5f)
            {
                transform.position = networkPosition;
            }
            else
            {
                // 기존 위치 보간
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10);
            }

            if (Quaternion.Angle(transform.rotation, networkRotation) > 45f)
            {
                transform.rotation = networkRotation;
            }
            else
            {
                // 기존 회전 보간
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10);
            }
        }


    }
}
