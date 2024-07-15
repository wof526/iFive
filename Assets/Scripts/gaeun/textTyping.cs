using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class textTyping : MonoBehaviour
{
    public Text text;

    private void Start()
    {
        text.text = null;
        text.DOText(" . . . ", 3f).SetLoops(-1, LoopType.Incremental);
    }

}
