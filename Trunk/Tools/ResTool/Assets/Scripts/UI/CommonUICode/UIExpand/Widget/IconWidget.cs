﻿using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class IconWidget : UIBaseWidget
{

    public enum IconType
    {
        Sprite = 0,
        MovicClip = 1
    }

    public IconType iconType = IconType.Sprite;

    public Image Img;

    public int initIndex;

    public Material grayMaterial;
    private bool m_activeGray = false;

    [SerializeField]
    public Sprite[] IconArr;

    public bool activeGray
    {
        get { return this.m_activeGray; }

        set
        {
            m_activeGray = value;
            if (m_activeGray)
            {
                if (this.Img != null && this.grayMaterial != null)
                {
                    this.Img.material = this.grayMaterial;
                }
            }
            else
            {
                if (this.Img != null)
                {
                    this.Img.material = null;
                }
            }

        }
    }


    public Sprite[] GetIconArr
    {
        get
        {
            return IconArr;
        }
    }

    [SerializeField]
    public MovieClip[] mcArr;

    public bool useImgSize = false;


    Action<object> mcPlayEndAction;

    public override WidgetType GetWidgetType()
    {
        return WidgetType.Icon;
    }

    public bool AddExEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerDoubleClick:
                UIExClickListener.Get(gameObject).onDoubleHandler = onEventHandler;
                break;
            case UIEvent.PointerShortClick:
                UIExClickListener.Get(gameObject).onSingleHandler = onEventHandler;
                break;

            default:
                break;
        }
        return sign;
    }
    public bool RemoveExEventListener(UIEvent eventType, Action<GameObject> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerDoubleClick:
                UIExClickListener.Get(gameObject).onDoubleHandler = null;
                break;
            case UIEvent.PointerShortClick:
                UIExClickListener.Get(gameObject).onSingleHandler = null;
                break;
            default:
                break;
        }
        return sign;
    }

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = onEventHandler;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }
    public override bool RemoveEventListener(UIEvent eventType, System.Action<PointerEventData> onEventHandler) {
        bool sign = true;
        switch (eventType) {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }

    public override bool AddCustomEventListener(UICustomEvent eventType, Action<object> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UICustomEvent.Complete:
                mcPlayEndAction = onEventHandler;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }
    public void SetFarAway(bool isFar)
    {
        BaseSetFarAway(isFar);
    }
    public bool IsFarAway
    {
        get { return IsBaeFarAway; }
    }
    void OnEnable()
    {
        //if (iconType == IconType.MovicClip)
        //{
        //    PlayMC();
        //}
    }
    void OnDisable()
    {
        if (iconType == IconType.MovicClip)
        {
            StopMC();
        }
    }
    void OnDestroy()
    {
        if (iconType == IconType.MovicClip)
        {
            StopMC();
        }
    }

    bool mcPlaying = false;
    public bool GetPlayState()
    {
        return mcPlaying;
    }

    public void ChangeIcon(int index=-1,bool playSign=true)
    {
        //Debug.LogError("index  "+ index);
        if (index < 0 || (iconType == IconType.Sprite && index >= IconArr.Length) || (iconType == IconType.MovicClip && index >= mcArr.Length))
        {
            Img.enabled = false;
            return;
        }
        Img.enabled = true;
        initIndex = index;
        if (iconType == IconType.Sprite)
        {
            
            Img.sprite = IconArr[index];
            if (useImgSize)
            {
                RectTransform rt = gameObject.transform as RectTransform;
                rt.sizeDelta = new Vector2(Img.sprite.rect.width, Img.sprite.rect.height);
            }
        }
        else if (iconType == IconType.MovicClip)
        {
            if(playSign)
            {
                PlayMC();
            }
            else
            {
                ResetMC();
            }
            
        }
    }

    public void setNativeSize() {
        if (Img == null)
            return;
        Img.SetNativeSize();
    }

    void PlayMC()
    {
        MovieClip mc = mcArr[initIndex];
        if(mc!=null)
        {
#if !TOOL
            GlobalTimeManager.Instance.timerController.RemoveTimerByKey(this);
#endif
            ResetMC();
#if !TOOL
            GlobalTimeManager.Instance.timerController.AddTimer(this, mc.spaceTime, mc.GetPlayNum(), frameAnimation);
#endif
            mcPlaying = true;
        }
    }
    void frameAnimation(int n)
    { 
        MovieClip mc = mcArr[initIndex];
        Img.sprite = mc.GetSprite(n);

        if (mc.loopNum != -1 && mc.loopNum == n)
        {
            //结束
            OnMCPlayEnd();
        }
    }
    void ResetMC()
    {
        MovieClip mc = mcArr[initIndex];
        Img.sprite = mc.GetSprite(0);
    }
    void StopMC()
    {
        mcPlaying = false;
#if !TOOL
        GlobalTimeManager.Instance.timerController.RemoveTimerByKey(this);
#endif
    }
    void OnMCPlayEnd()
    {
        if(mcPlayEndAction!=null)
        {
            mcPlayEndAction.Invoke(null);
        }
    }

    
}
