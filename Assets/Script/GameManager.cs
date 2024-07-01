using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CarInfo CarInfo;
    public Car car;
    public Dash dash;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
