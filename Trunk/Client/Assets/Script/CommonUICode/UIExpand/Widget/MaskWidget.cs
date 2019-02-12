using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class MaskWidget : UIBaseWidget
{
    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        return false;
    }
    public override bool RemoveEventListener(UIEvent eventType, System.Action<PointerEventData> onEventHandler) {
        return false;
    }
    public override WidgetType GetWidgetType()
    {
        return WidgetType.Mask;
    }

    public Image maskImg;

    public RectMask2D uiMask;
}
