using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    Vector2 targetPos;
    private RectTransform rectTransform;
    bool IsGageActive = false;


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        rectTransform = GetComponent<RectTransform>();
        targetPos = rectTransform.anchoredPosition;
        //Debug.Log(targetPos);
    }

    public void BtnMoveFoward()
    {
        rectTransform.DOAnchorPosX(targetPos.x - 70f, 0.2f, false); //targetPosition
    }

    public void BtnMoveBack()
    {
        rectTransform.DOAnchorPosX(targetPos.x, 0.2f, false); //targetPosition
    }

    public void Gagebar()
    {
        // if tag or objectname = gagebar -> setactivefalse;
        // 하나씩 setactive true
        // max값 자료에서 보기
    }
}
