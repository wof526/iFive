using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    Vector2 targetPos;
    private RectTransform rectTransform;
    carData cardata;
    carDic cardic;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        rectTransform = GetComponent<RectTransform>();
        targetPos = rectTransform.anchoredPosition;

        AudioManager.instance.PlayBgm(true);

    }

    public void BtnMoveFoward()
    {
        rectTransform.DOAnchorPosX(targetPos.x - 70f, 0.2f, false); //targetPosition

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //AudioManager.instance.EffectBgm(true);

    }

    public void BtnMoveBack()
    {
        rectTransform.DOAnchorPosX(targetPos.x, 0.2f, false); //targetPosition
    }


    public void ChangeMatchScene()
    {
        SceneManager.LoadScene("CreateRoom");
        AudioManager.instance.EffectBgm(false);

    }

    public void AudioStartBtn()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StartBtn);

    }
}
