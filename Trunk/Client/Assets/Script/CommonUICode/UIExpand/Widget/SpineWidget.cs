using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
#if !TOOL
using DG.Tweening;
using XLua;

[LuaCallCSharp]
#endif
public class SpineWidget : UIBaseWidget
{
    [SerializeField]
    public SkeletonGraphic skeleton;

    Action<object> spineCompleteHandler;
    Action<PointerEventData> pointerClickHandler;

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        { 
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
            case UIEvent.PointerClick:
                pointerClickHandler = null;
                break;
            default:
                sign = false;
                break;
        }

        return sign;
    }
    public override bool AddSpineCustomEventListener(UISpineEvent eventType, Action<object> onEventHandler)
    {
        bool sign = false;
        switch (eventType)
        {
            case UISpineEvent.Complete:
                spineCompleteHandler = onEventHandler;
                break;
        }
        return sign;
    }

    public override WidgetType GetWidgetType()
    {
        return WidgetType.Spine;
    }

    private void Awake()
    {
       
    }

    void InitEvent()
    {
        skeleton.AnimationState.Complete += onSpineCompleter;
        PointerClickListener.Get(gameObject).onHandler = OnPointerClick;
    }

    private void Start()
    {
        InitEvent();
    }

    void onSpineCompleter(TrackEntry entry)
    {
        if (spineCompleteHandler != null)
        {                    
            spineCompleteHandler(entry.animation.name);
        }
    }

    void OnPointerClick(PointerEventData eventData)
    {
        if (pointerClickHandler != null)
        {
            pointerClickHandler(eventData);
        }
    }

    private void OnDestroy()
    {
        skeleton.AnimationState.Complete -= onSpineCompleter;
        pointerClickHandler = null;
    }

    public void Fade(float targetAlpha, float duration)
    {
#if !TOOL
        skeleton.DOFade(targetAlpha, duration);
#endif
    }

}
