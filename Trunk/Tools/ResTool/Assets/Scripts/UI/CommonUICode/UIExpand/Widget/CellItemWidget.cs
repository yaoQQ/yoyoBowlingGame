using System;
using UnityEngine;
using UnityEngine.EventSystems;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class CellItemWidget : UIBaseWidget
{
    private RectTransform m_rt = null;
    public RectTransform rt
    {
        get
        {
            if (m_rt == null)
                m_rt = GetComponent<RectTransform>();
            return m_rt;
        }
    }
    public int index;

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

    public void SetFarAway(bool isFar)
    {
        BaseSetFarAway(isFar);
    }

    public bool IsFarAway
    {
        get { return IsBaeFarAway; }
    }
    public override WidgetType GetWidgetType()
    {
        return WidgetType.CellItem;
    }

    public float Height
    {
        get { return rt.sizeDelta.y; }
        set
        {
            Vector2 sizeDelta = rt.sizeDelta;
            sizeDelta.y = value;
            rt.sizeDelta = sizeDelta;
        }
    }

    public Vector2 Top
    {
        get
        {
            return rt.anchoredPosition;
        }
        set
        {
            rt.anchoredPosition = value;
        }
    }

    public Vector2 Bottom
    {
        get
        {
            return rt.anchoredPosition - new Vector2(0f, Height);
        }
        set
        {
            rt.anchoredPosition = value + new Vector2(0f, Height);
        }
    }
}
