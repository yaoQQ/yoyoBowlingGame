﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if !TOOL
using XLua;
[LuaCallCSharp]
#endif

[RequireComponent(typeof(Animation))]
[DisallowMultipleComponent]
public partial class AnimationWidget
{
    public Animation BaseAnimation;

    private Action _animationEndCallBack;

    private bool _mBPlaying = false;

    private float _duration = 0;

    private float _mFLastTime = 0;

    void Update()
    {
        if (BaseAnimation == null)
        {
            return;
        }

        if (_mBPlaying)
        {
            //动画结束
            if (_mFLastTime >= _duration)
            {
                _mBPlaying = false;
                _animationEndCallBack();
            }
            _mFLastTime += Time.deltaTime;
        }
    }


    public void Play()
    {
        Debug.Log("begain playy  BaseAnimation="+ BaseAnimation);
        if (BaseAnimation != null)
        {
            BaseAnimation.Play();
            Debug.Log("end playy");
        }
    }
    public void Play(string animationName, Action callback)
    {
        if (BaseAnimation != null)
        {
            BaseAnimation[animationName].speed = 1;
            BaseAnimation[animationName].time = 0;
            BaseAnimation.Play(animationName);
        }
        EndCallBack(animationName, callback);
    }
    public void PlayBack(string animationName, Action callback)
    {
        if (BaseAnimation != null)
        {
            BaseAnimation[animationName].speed = -1;
            BaseAnimation[animationName].time = BaseAnimation[animationName].length;
            BaseAnimation.Play(animationName);
        }

        EndCallBack(animationName, callback);
    }

    void EndCallBack(string animationName, Action callback)
    {
        if (callback != null)
        {
            _animationEndCallBack = callback;
            _mBPlaying = true;
            _mFLastTime = 0;
            if (BaseAnimation != null) _duration = BaseAnimation[animationName].length;
        }
    }

    public void Stop()
    {
        if (BaseAnimation != null)
        {
            BaseAnimation.Stop();
        }
        _mBPlaying = false;
    }
    public void Stop(string animationName)
    {
        if (BaseAnimation != null) BaseAnimation.Stop(animationName);
        _mBPlaying = false;
    }
}

public partial class AnimationWidget : UIBaseWidget
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
        return WidgetType.Animation;
    }

}