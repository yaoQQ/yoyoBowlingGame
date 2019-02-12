using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimatorEvent
{
    public const string PLAY_END = "PLAY_END";
}


//在cell里面
public class SceneAnimator
{
    Animator animator;

    public SceneAnimator(Animator anim)
    {       
        animator = anim;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void PlayAnim(string animName)
    {
        var cur = this.animator.GetCurrentAnimatorStateInfo(0);
        if (cur.IsName(animName))
            return;
        animator.SetTrigger(animName);
    }

    public void Play(string stateName, int layer = 0)
    {
        animator.Play(stateName, layer);
    }

    Action<string> onPlayEndHandler;

    public void AddEventListener(string animatorEventType,Action<string> onEventHandler)
    {
        switch(animatorEventType)
        {
            case SceneAnimatorEvent.PLAY_END:
                onPlayEndHandler = onEventHandler;
                break;
            
        }
    }
    public void OnAnimationEvent(string animatorEventType,string parameter)
    {
        switch (animatorEventType)
        {
            case SceneAnimatorEvent.PLAY_END:
                if(onPlayEndHandler!=null)
                {
                    onPlayEndHandler.Invoke(parameter);
                }
                break;

        }
    }

    public void GoOnPlay()
    {
        animator.SetTrigger("endSign");
    }

    public void ReSetEndSign()
    {
        animator.ResetTrigger("endSign");
    }


}
