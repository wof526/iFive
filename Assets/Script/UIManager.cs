using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnMoveFoward()
    {
        transform.DOMoveX(630, 0.2f, false); //targetPosition
    }

    public void BtnMoveBack()
    {
        transform.DOMoveX(726, 0.2f, false); //targetPosition
    }


}
