using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif

public class CircleImageWidget : UIBaseWidget
{
    [SerializeField]
    public CircleImage Img;

    public bool defaultModeSign = false;

    public Sprite defaultPng;

    public Sprite[] DefaultPngArr;

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
    public override bool RemoveEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }

    public override WidgetType GetWidgetType()
    {
        return WidgetType.CircleImage;
    }

    public void Reset()
    {
        Img.sprite = null;
        Img.enabled = false;
        Color oldC = Img.color;
    }
    void Awake()
    {
        //if (defaultModeSign)
        //{
        //    Reset();
        //}
        //if (Img.sprite == null) Img.enabled = false;
    }
    public void SetFarAway(bool isFar)
    {
        BaseSetFarAway(isFar);
    }
    public bool IsFarAway
    {
        get { return IsBaeFarAway; }
    }
    public void SetPng(Sprite sprite, float aValue = 1f)
    {
        Img.sprite = sprite;
        Color oldC = Img.color;
        if (defaultModeSign && sprite == null) Img.sprite = defaultPng;
        Img.enabled = Img.sprite != null;
        Img.color = new Color(oldC.r, oldC.g, oldC.b, aValue);
    }

    public void SetPngEnabled(bool sign)
    {
        Img.enabled = sign;
    }

    public void ChangeDefaultPng(int index)
    {
        if (index < 0)
        {
            return;
        }

        defaultPng = DefaultPngArr[index];
    }
}
