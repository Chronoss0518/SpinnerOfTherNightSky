using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPutScript : MonoBehaviour
{
    [SerializeField]
    public RawImage image = null;

    [SerializeField]
    public Animator animator = null;

    [SerializeField]
    CardScript cardObject = null;

    bool pushNow = false;

    int beforeTime = 0;

    bool openFlg = false;

    DateTime startTime;


    public void SetTexture(Texture2D _tex) { if (image != null) image.texture = _tex; }

    public Texture GetTexture() { return image.texture; }


    public void SetAnimation(bool _flg)
    {
        if (animator == null) return;

        animator.SetBool("SelectFlg", _flg);
    }

    public void SetAnimationVisible(bool _flg)
    {
        if (animator == null) return;

        animator.gameObject.SetActive(_flg);
    }

    public void PushStart()
    {
        beforeTime = 0;
        openFlg = false;
        startTime = DateTime.Now;
        pushNow = true;
    }
    public void PushEnd()
    {
        pushNow = false;
        if (!openFlg) return;
        if (cardObject == null) return;
        cardObject.SelectAction();
    }

    void Update()
    {
        if (!pushNow) return;
        if (openFlg) return;
        int now = DateTime.Now.Subtract(startTime).Seconds;
        Debug.Log($"Now Time{now}");
        if (beforeTime == now) return;
        openFlg = cardObject.OpenCardDescription(now);
        beforeTime = now;
    }

}
