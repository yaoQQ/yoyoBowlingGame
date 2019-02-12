using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class HorizontalLayoutGroupWidget : UIBaseWidget
{

    public override WidgetType GetWidgetType()
    {
        return WidgetType.HorizontalLayout;
    }

    public HorizontalLayoutGroup InnerHorizontalGroup;


    public RectOffset GetGroupPadding() 
    {
        return InnerHorizontalGroup.padding;
    }

    public void SetGropPadding(RectOffset setData) 
    {
        InnerHorizontalGroup.padding = setData; 
    }

    public override bool AddEventListener(UIEvent eventType, System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.DragBegin:
                beginDragHandler = onEventHandler;
                break;
            case UIEvent.DragEnd:
                endDragHandler = onEventHandler;
                break;
            case UIEvent.Drag:
                dragingHandler = onEventHandler;
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
            case UIEvent.DragBegin:
                beginDragHandler = null;
                break;
            case UIEvent.DragEnd:
                endDragHandler = null;
                break;
            case UIEvent.Drag:
                dragingHandler = null;
                break;

            default:
                sign = false;
                break;
        }
        return sign;
    }
    Action<PointerEventData> beginDragHandler;
    protected virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (beginDragHandler != null)
        {
            beginDragHandler.Invoke(eventData);
        }
    }

      Action<PointerEventData> endDragHandler;
      protected virtual void OnEndDrag(PointerEventData eventData)
      {
          if (endDragHandler != null)
          {
              endDragHandler.Invoke(eventData);
          }
      }

      Action<PointerEventData> dragingHandler;
      protected virtual void OnScollerDrag(PointerEventData eventData)
      {
          if (endDragHandler != null)
          {
              endDragHandler.Invoke(eventData);
          }
      }
}
