using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    carData cardata;

    public Slider sliderA;
    public Slider sliderB;
    public Slider sliderC;
    public Slider sliderD;

    int maxSpeedMin = 50;
    int maxSpeed = 60;
    int maxSpeedFull = 160;

    float atkMin = 3.0f;
    float atk = 4.5f;
    float atkFull = 7.0f;

    int hpMin = 150;
    int hp = 800;
    int hpFull = 1000;

    int dashSpeedMin = 25;
    int dashSpeed = 30;
    int dashSpeedFull = 120;

    private void Awake()
    {
        cardata = GetComponent<carData>();
        //maxSpeed = cardata.maxSpeed;

    }

    public void OnclickButtonA()
    {
        // Slider: right to left -> GageValue = full - (full - value)

        sliderA.minValue = maxSpeedMin;
        sliderA.maxValue = maxSpeedFull;
        sliderA.value = maxSpeedFull - (maxSpeed - maxSpeedMin);

        sliderB.minValue = atkMin;
        sliderB.maxValue = atkFull;
        sliderB.value = atkFull - (atk - atkMin);

        sliderC.minValue = hpMin;
        sliderC.maxValue = hpFull;
        sliderC.value = hpFull - (hp - hpMin);

        sliderD.minValue = dashSpeedMin;
        sliderD.maxValue = dashSpeedFull;
        sliderD.value = dashSpeedFull - (dashSpeed - dashSpeedMin);

    }
}
