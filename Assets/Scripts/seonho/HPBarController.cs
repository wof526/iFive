using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Slider hpBar;
    public TMP_Text hpText;

    public void SetMaxHealth(float maxHealth)
    {
        hpBar.maxValue = maxHealth;
        hpBar.value = maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        hpBar.value = currentHealth;
        hpText.text = $"{currentHealth}/{hpBar.maxValue}";
    }
}