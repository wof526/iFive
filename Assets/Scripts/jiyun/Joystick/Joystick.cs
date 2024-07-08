using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   // 조이스틱 조작 스크립트

    [Header("Settings")]
    [SerializeField, Range(1, 15)]private float Radio = 5;  // 조이스틱의 이동 범위
    [SerializeField, Range(0.01f, 1)]private float SmoothTime = 0.5f;   // 조이스틱이 중앙으로 돌아가는 속도
    public Color NormalColor = new Color(1, 1, 1, 1);   // 조이스틱의 기본 색상
    public Color PressColor = new Color(1, 1, 1, 1);    // 조이스틱을 눌렀을 때의 색상
    [SerializeField, Range(0.1f, 5)]private float Duration = 1; // 색상 변하는데 걸리는 시간

    [Header("Reference")]
    [SerializeField]private RectTransform StickRect;    // 조이스틱의 가운데 ui
    [SerializeField] private RectTransform CenterReference; // 조이스틱의 기본 위치

    private Vector3 DeathArea;  // 조이스틱의 중심 위치
    private Vector3 currentVelocity;    // 조이스틱의 현재 속도
    public static bool isFree = false;    // 조이스틱이 원래 위치로 돌아가야 하는가?
    private int lastId = -2;    // 마지막 터치 id
    private Image stickImage;   // 조이스틱 img
    private Image backImage;    // 배경 img
    private Canvas m_Canvas;    // 조이스틱이 있는 캔버스
    private float diff; // 중심 위치의 크기

    void Start()
    {
        if (transform.root.GetComponent<Canvas>() != null)
        {
            m_Canvas = transform.root.GetComponent<Canvas>();
        }
       
        DeathArea = CenterReference.position;
        diff = CenterReference.position.magnitude;
        if (GetComponent<Image>() != null)
        {
            backImage = GetComponent<Image>();
            stickImage = StickRect.GetComponent<Image>();
            backImage.CrossFadeColor(NormalColor, 0.1f, true, true);
            stickImage.CrossFadeColor(NormalColor, 0.1f, true, true);
        }
    }

    void Update()
    {
        DeathArea = CenterReference.position;

        if (!isFree)    // 조이스틱이 이동중이지 않으면 아무것도 안함
            return;

        // 조이스틱을 원하는 위치(DeathArea)로 이동
        StickRect.position = Vector3.SmoothDamp(StickRect.position, DeathArea, ref currentVelocity, smoothTime);
        
        if (Vector3.Distance(StickRect.position, DeathArea) < .1f)
        {   // 현재 조이스틱과 가려는 위치 사이의 거리를 계산하여
            // 가려는 위치와 가까우면 위치 저장하고 정지!
            isFree = false;
            StickRect.position = DeathArea;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        Drive.isBreak = false;  // 다시 조이스틱을 움직이기 시작할때
        if (lastId == -2)
        {
            lastId = data.pointerId;
            if (backImage != null)  // 조이스틱 색상 변경
            {
                backImage.CrossFadeColor(PressColor, Duration, true, true);
                stickImage.CrossFadeColor(PressColor, Duration, true, true);
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == lastId)
        {
            isFree = false;

            Vector3 position = JoystickUtils.TouchPosition(m_Canvas,GetTouchID);

            if (Vector2.Distance(DeathArea, position) < radio)
            {   // 터치 위치로 스틱 이동
                StickRect.position = position;
            }
            else
            {   // 스틱이 최대 반경을 넘지 않도록 함
                StickRect.position = DeathArea + (position - DeathArea).normalized * radio;
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {   // 조이스틱을 놓았을 때
        isFree = true;
        currentVelocity = Vector3.zero;

        if (data.pointerId == lastId)
        {
            lastId = -2;    // id 초기화!

            if (backImage != null)  // 조이스틱 색상 변경
            {
                backImage.CrossFadeColor(NormalColor, Duration, true, true);
                stickImage.CrossFadeColor(NormalColor, Duration, true, true);
            }
        }
    }
    
    public int GetTouchID
    {   // 현재 터치 id 반환
        get
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastId)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    private float radio {   // 조이스틱의 최대 반경
        get {
            return (Radio * 5 + Mathf.Abs((diff - CenterReference.position.magnitude))); 
        }
    }
    private float smoothTime {  // 조이스틱을 얼마나 부드럽게 이동시킬건가
        get { 
            return (1 - (SmoothTime));
        } 
    }

    public float Horizontal // 수평 입력 값 계산
    {
        get
        {
            return (StickRect.position.x - DeathArea.x) / Radio;
        }
    }

    public float Vertical   // 수직 입력 값 계산
    {
        get
        {
            return (StickRect.position.y - DeathArea.y) / Radio;
        }
    }
}