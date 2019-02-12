using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(PointerEventData eventData);
    public delegate void BaseDelegate(BaseEventData eventData);
    public delegate void MoveDelegate(AxisEventData eventData);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public MoveDelegate onMove;

    public BaseDelegate onSelect;
    public BaseDelegate onUpdateSelect;

    public Action<object> onSubmit;

    public Action<object> onDeselect;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(eventData);
    }
    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(eventData);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(eventData);
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(eventData);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null) onDeselect(eventData);
    }
}