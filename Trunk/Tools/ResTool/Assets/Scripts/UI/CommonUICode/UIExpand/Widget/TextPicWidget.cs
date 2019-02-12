using System;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class TextPicWidget : UIBaseWidget
{
    void Awake()
    {
        InitEvent();
    }

    void InitEvent()
    {
        textPic.onHrefClick.AddListener(OnHrefClick);

    }
    void OnHrefClick(string hrefValue)
    {
        if(textPicHrefClickHandler != null)
        {
            textPicHrefClickHandler.Invoke(hrefValue);
        }
    }

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerDown:
                PointerDownListener.Get(gameObject).onHandler = onEventHandler;
                break;
            case UIEvent.PointerUp:
                PointerUpListener.Get(gameObject).onHandler = onEventHandler;
                break;
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
            case UIEvent.PointerDown:
                PointerDownListener.Get(gameObject).onHandler = null;
                break;
            case UIEvent.PointerUp:
                PointerUpListener.Get(gameObject).onHandler = null;
                break;
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }

    Action<object> textPicHrefClickHandler;
    public override bool AddCustomEventListener(UICustomEvent eventType, Action<object> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UICustomEvent.HrefClick:

                textPicHrefClickHandler = onEventHandler;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }

    public override WidgetType GetWidgetType()
    {
        return WidgetType.TextPic;
    }

    public TextPic   textPic;

    public string text
    {
        get
        {
            return textPic.text;
        }

        set
        {
            textPic.text = value;
        }
    }

    public int fontSize
    {
        get
        {
            return textPic.fontSize;
        }

        set
        {
            textPic.fontSize = value;
        }

    }


    public void SetIconList(TextPic.IconName[] iconName)
    {
        if(textPic != null)
        {
            textPic.SetIconList(iconName);
        }
    }

}
