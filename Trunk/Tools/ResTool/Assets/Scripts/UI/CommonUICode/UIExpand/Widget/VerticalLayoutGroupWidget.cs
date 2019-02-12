﻿using UnityEngine;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class VerticalLayoutGroupWidget : UIBaseWidget
{

    public override WidgetType GetWidgetType()
    {
        return WidgetType.VerticalLayout;
    }

    public VerticalLayoutGroup InnerVerticalGroup;

    public RectOffset GetGroupPadding()
    {
        return InnerVerticalGroup.padding;
    }

    public void SetGropPadding(RectOffset setData)
    {
        InnerVerticalGroup.padding = setData;
    }

    public override bool AddEventListener(UIEvent eventType, System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler)
    {
        // bool sign = true;
        return false;
    }
    public override bool RemoveEventListener(UIEvent eventType, System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler) {
        return false;
    }
}