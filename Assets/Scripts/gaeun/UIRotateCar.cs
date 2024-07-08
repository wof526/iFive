using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIRotateCar : MonoBehaviour
{

    public Vector3 targetRot;
    public float RotTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

    }

    // Update is called once per frame
    void Update()
    {
        CarRotate();
    }

    void CarRotate()
    {
        transform.DORotate(targetRot, RotTime, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
}
