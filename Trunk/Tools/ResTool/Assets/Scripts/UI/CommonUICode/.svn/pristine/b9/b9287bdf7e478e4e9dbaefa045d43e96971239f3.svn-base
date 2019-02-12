using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class SliderWidget : UIBaseWidget
{

    Action<PointerEventData> pointerDownHandler;
    Action<PointerEventData> pointerUpHandler;
    Action<PointerEventData> pointerClickHandler;

    void Awake()
    {
        slider.maxValue = 1;
        slider.minValue = 0;
        InitEvent();
    }

    void InitEvent()
    {
        PointerDownListener.Get(gameObject).onHandler = OnPointerDown;
        PointerUpListener.Get(gameObject).onHandler = OnPointerUp;
        PointerClickListener.Get(gameObject).onHandler = OnPointerClick;
    }
    void OnPointerDown(PointerEventData eventData)
    {
        
        if (pointerDownHandler != null)
        {
            pointerDownHandler.Invoke(eventData);
        }
    }


    void OnPointerUp(PointerEventData eventData)
    {
        
        if (pointerUpHandler != null)
        {
            pointerUpHandler.Invoke(eventData);
        }
    }


    void OnPointerClick(PointerEventData eventData)
    {
        if (pointerClickHandler != null)
        {
            pointerClickHandler(eventData);
        }
    }

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerDown:
                pointerDownHandler = onEventHandler;
                break;
            case UIEvent.PointerUp:
                pointerUpHandler = onEventHandler;
                break;
            case UIEvent.PointerClick:
                pointerClickHandler = onEventHandler;
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
                pointerDownHandler = null;
                break;
            case UIEvent.PointerUp:
                pointerUpHandler = null;
                break;
            case UIEvent.PointerClick:
                pointerClickHandler = null;
                break;
            default:
                sign = false;
                break;
        }

        return sign;
    }

    public override WidgetType GetWidgetType()
    {
        return WidgetType.Slider;
    }

    public bool handleSign = true;

    public Slider slider;

    public Image bgImg;

    public Image fillImg;


    public Vector2 handleRange = new Vector2(0, 1);

    public float value
    {
        get
        {
            return slider.value;
        }

        set
        {
            slider.value = value;
            slider.handleRect.gameObject.SetActive(true);
            //slider.handleRect.gameObject.SetActive(CheckHandleActive(slider.value));

        }
    }
    bool CheckHandleActive(float v)
    {
        if (!handleSign) return false;
        if (v >= handleRange.x && v <= handleRange.y)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }




}
