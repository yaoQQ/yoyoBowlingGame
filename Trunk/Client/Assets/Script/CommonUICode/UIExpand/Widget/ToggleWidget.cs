﻿using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class ToggleWidget : UIBaseWidget
{
    public Text Txt;
    public Image BgImg;
    public Image CheackMaskImg;
    public Toggle toggle;
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

    public override WidgetType GetWidgetType()
    {
        return WidgetType.Toggle;
    }

    public bool IsOn
    {
        get
        {
            return toggle.isOn;
        }
        set
        {
            toggle.isOn = value;
        }
    }
    public void OnValueChanged(Action<object> onEventHandler)
    {
        OnValueChangedRemoveAllListeners();
        toggle.onValueChanged.AddListener((bool value) =>
        {
            if (onEventHandler != null)
            {
                onEventHandler(value);
            }
        });
    }

    public void OnTargetChanged(Action<object> onEventHandler) {
        OnValueChangedRemoveAllListeners();
        toggle.onValueChanged.AddListener((bool value) => {
            if (onEventHandler != null) {
                onEventHandler(gameObject);
            }
        });
    }
    public void OnValueChangedRemoveAllListeners()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}