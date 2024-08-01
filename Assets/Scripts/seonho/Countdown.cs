using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Countdown : MonoBehaviour
{
    public GameObject joystick;
    public TMP_Text countdownText;
    public Image background;
    private GameObject joystickInstance;

    void Start()
    {
        joystickInstance = Instantiate(joystick);
        joystickInstance.SetActive(false);
        background.gameObject.SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();  // 카운트다운 텍스트 설정
            yield return new WaitForSeconds(1f);
        }

        // 카운트다운 끝난 후
        countdownText.text = "Go!";
        joystickInstance.SetActive(true);
        yield return new WaitForSeconds(1f);
        background.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 비활성화
    }
}