using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimationEvent : MonoBehaviour
{

    public Animator animator = null;

    public UnityEngine.Events.UnityEvent act = new UnityEngine.Events.UnityEvent();

    [System.NonSerialized]
    private bool playFlg = false;

    public void ReSet()
    {
        playFlg = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (animator != null) return;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Start();

        if(animator == null) return;

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.normalizedTime < 1)
        {
            playFlg = false;
            return;
        }

        if (playFlg) return;

        playFlg = true;

        act.Invoke();

    }
}
