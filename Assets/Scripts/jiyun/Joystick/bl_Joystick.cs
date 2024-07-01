using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class bl_Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   // 조이스틱 조작 스크립트

    [Header("Settings")]
    [SerializeField, Range(1, 15)]private float Radio = 5;  // 레버의 범위 제한
    [SerializeField, Range(0.01f, 1)]private float SmoothTime = 0.5f;   //return to default position speed
    [SerializeField, Range(0.5f, 4)] private float OnPressScale = 1.5f; //return to default position speedㄴ
    public Color NormalColor = new Color(1, 1, 1, 1);
    public Color PressColor = new Color(1, 1, 1, 1);
    [SerializeField, Range(0.1f, 5)]private float Duration = 1;

    [Header("Reference")]
    [SerializeField]private RectTransform StickRect;//The middle joystick UI
    [SerializeField] private RectTransform CenterReference;

    private Vector3 DeathArea;
    private Vector3 currentVelocity;
    private bool isFree = false;
    private int lastId = -2;
    private Image stickImage;
    private Image backImage;
    private Canvas m_Canvas;
    private float diff;
    private Vector3 PressScaleVector;

    void Start()
    {
        if (StickRect == null)
        {
            Debug.LogError("Please add the stick for joystick work!.");
            this.enabled = false;
            return;
        }

        if (transform.root.GetComponent<Canvas>() != null)
        {
            m_Canvas = transform.root.GetComponent<Canvas>();
        }
        else if (transform.root.GetComponentInChildren<Canvas>() != null)
        {
            m_Canvas = transform.root.GetComponentInChildren<Canvas>();
        }
        else
        {
            Debug.LogError("Required at lest one canvas for joystick work.!");
            this.enabled = false;
            return;
        }
       
        DeathArea = CenterReference.position;
        diff = CenterReference.position.magnitude;
        PressScaleVector = new Vector3(OnPressScale, OnPressScale, OnPressScale);
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

        if (!isFree)
            return;

        StickRect.position = Vector3.SmoothDamp(StickRect.position, DeathArea, ref currentVelocity, smoothTime);
        
        if (Vector3.Distance(StickRect.position, DeathArea) < .1f)
        {
            isFree = false;
            StickRect.position = DeathArea;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (lastId == -2)
        {
            lastId = data.pointerId;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(true));
            // OnDrag(data);    ->  조이스틱을 무조건 가운데에서 드래그해서만 작동할 수 있도록 주석 처리함
            if (backImage != null)
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

            Vector3 position = bl_JoystickUtils.TouchPosition(m_Canvas,GetTouchID);

            if (Vector2.Distance(DeathArea, position) < radio)
            {
                StickRect.position = position;
            }
            else
            {
                StickRect.position = DeathArea + (position - DeathArea).normalized * radio;
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        isFree = true;
        currentVelocity = Vector3.zero;

        if (data.pointerId == lastId)
        {

            lastId = -2;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(false));
            if (backImage != null)
            {
                backImage.CrossFadeColor(NormalColor, Duration, true, true);
                stickImage.CrossFadeColor(NormalColor, Duration, true, true);
            }
        }
    }

    IEnumerator ScaleJoysctick(bool increase)
    {
        float _time = 0;

            while (_time < Duration)
            {
                Vector3 v = StickRect.localScale;
            if (increase)
            {
                v = Vector3.Lerp(StickRect.localScale, PressScaleVector, (_time / Duration));
            }
            else
            {
                v = Vector3.Lerp(StickRect.localScale, Vector3.one, (_time / Duration));
            }
            StickRect.localScale = v;
                _time += Time.deltaTime;
                yield return null;
            }
    }
    
    public int GetTouchID
    {
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

    private float radio { get { return (Radio * 5 + Mathf.Abs((diff - CenterReference.position.magnitude))); } }
    private float smoothTime { get { return (1 - (SmoothTime)); } }

    public float Horizontal
    {
        get
        {
            return (StickRect.position.x - DeathArea.x) / Radio;
        }
    }

    public float Vertical
    {
        get
        {
            return (StickRect.position.y - DeathArea.y) / Radio;
        }
    }
}