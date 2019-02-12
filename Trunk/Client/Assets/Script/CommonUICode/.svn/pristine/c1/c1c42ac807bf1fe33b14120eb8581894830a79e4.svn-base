using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if !TOOL
using XLua;
[LuaCallCSharp]
#endif

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public partial class AnimatorWidget
{
    [SerializeField]
    private Animator animator;
    AnimatorOverrideController overrideController;
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogErrorFormat("错误,该物体不存在动画控制器");
            return;
        }
        overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        animator.runtimeAnimatorController = overrideController;
    }

    Queue<Action<string>> endEventQueue = new Queue<Action<string>>();
    /// <summary>
    /// 动态增加结束事件
    /// </summary>
    public void AddEndAnimationEvent(string clipName, Action<string> callback)
    {
        if (overrideController == null)
            return;
        AnimationClip runClip = overrideController[clipName];
        if (runClip == null)
        {
            Debug.LogErrorFormat("错误,不存在该动画, name = {0}", clipName);
            return;
        }
        endEventQueue.Enqueue(callback);
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = runClip.length;
        animEvent.functionName = "OnEndFunction";
        animEvent.stringParameter = clipName;
        runClip.AddEvent(animEvent);

    }
    /// <summary>
    /// 动画结束回调函数
    /// </summary>
    /// <param name="str"></param>
    void OnEndFunction(string str)
    {
        if (endEventQueue.Count > 0)
        {
            var endEvent = endEventQueue.Dequeue();
            if (endEvent != null)
                endEvent.Invoke(str);
        }
    }

    public void ResetEvents()
    {
        if (overrideController == null)
            return;
        foreach (var anim in overrideController.animationClips)
        {
            anim.events = null;
        }
        this.endEventQueue.Clear();
    }
    public void Play(string name)
    {
        if (animator == null)
            return;
        this.ResetPlay();
        this.animator.SetTrigger(name);
    }
    public void ResetPlay()
    {
        if (overrideController == null)
            return;

        foreach (var anim in overrideController.animationClips)
        {
            this.animator.ResetTrigger(anim.name);
        }
    }
    public AnimationClip GetAnimClip(string clipName)
    {
        return overrideController[clipName];
    }
    public void SetSpeed(float speed)
    {
        if (animator != null) animator.speed = speed;
    }
}

public partial class AnimatorWidget : UIBaseWidget
{

    public bool AddExEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;

        return sign;
    }

    public bool RemoveExEventListener(UIEvent eventType, Action<GameObject> onEventHandler)
    {
        bool sign = true;

        return sign;
    }

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        return sign;
    }
    public override bool RemoveEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;

        return sign;
    }
    public override WidgetType GetWidgetType()
    {
        return WidgetType.Animator;
    }

}