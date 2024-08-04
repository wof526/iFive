using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class CircleSlider : MonoBehaviour
{
    public Image circleSlider;
    public float exValue = 60.0f; // 0 - 100, dont use int
    public TextMeshProUGUI exvalTxt;

    public TextMeshProUGUI winText;
    float circleTime = 1.0f;
    public float presentTime = 0.0f;


    void Start()
    {
        if (FXtriggerManger.bluewin)
        {
            winText.text = "Blue win";
        }
        else
        {
            winText.text = "Red win";
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);

        circleSlider.fillAmount = 0.0f;
        exvalTxt.enabled = false;

        Invoke("EnableEx", 2f);

    }

    private void Update()
    {
        if (presentTime > circleTime)
        {
            presentTime = 10.0f;
        }

        else
        {

            presentTime += Time.deltaTime;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Circle);

            circleSlider.fillAmount = (Mathf.Lerp(0, 100, presentTime / circleTime) * (exValue / 10000));


        }
    }

    public void EnableEx()
    {
        exvalTxt.enabled = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Experience);

    }
}
