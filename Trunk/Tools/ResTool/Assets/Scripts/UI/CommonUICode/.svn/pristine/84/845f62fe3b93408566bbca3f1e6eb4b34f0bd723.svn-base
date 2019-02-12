using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if !TOOL
using XLua;
[LuaCallCSharp]
#endif
public class EmptyImageWidget : UIBaseWidget
{

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = onEventHandler;
                break;
            case UIEvent.PointerDown:
                PointerDownListener.Get(gameObject).onHandler = onEventHandler;
                break;
            case UIEvent.PointerUp:
                PointerUpListener.Get(gameObject).onHandler = onEventHandler;
                break;
            case UIEvent.Drag:
                DragEventHandler.Get(gameObject).onDragHandler = onEventHandler;
                break;
            case UIEvent.PointerLongClick:
                UIExLongClickListener.Get(gameObject).onHandler = onEventHandler;
                break;
            case UIEvent.DragEnd:
                DragEventHandler.Get(gameObject).onEndDragHandler = onEventHandler;
                break;
            case UIEvent.DragBegin:
                DragEventHandler.Get(gameObject).onBeginDragHandler = onEventHandler;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
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
    public override bool RemoveEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;
            case UIEvent.PointerDown:
                PointerDownListener.Get(gameObject).onHandler = null;
                break;
            case UIEvent.PointerUp:
                PointerUpListener.Get(gameObject).onHandler = null;
                break;
            case UIEvent.Drag:
                DragEventHandler.Get(gameObject).onDragHandler = null;
                break;
            case UIEvent.DragBegin:
                DragEventHandler.Get(gameObject).onBeginDragHandler = null;
                break;
            case UIEvent.DragEnd:
                DragEventHandler.Get(gameObject).onEndDragHandler = null;
                break;
            case UIEvent.PointerLongClick:
                UIExLongClickListener.Get(gameObject).onHandler = null;
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
        return WidgetType.EmptyImage;
    }
}

