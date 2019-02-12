using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class RawImageWidget : UIBaseWidget
{
   
    public override WidgetType GetWidgetType()
    {
        return WidgetType.RawImage;
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
    public RawImage rawImage;


    public void SetTexture2D(Texture2D tex2D)
    {
        rawImage.rectTransform.sizeDelta = new Vector2(tex2D.width, tex2D.height);
        rawImage.texture = tex2D;
    }

}
