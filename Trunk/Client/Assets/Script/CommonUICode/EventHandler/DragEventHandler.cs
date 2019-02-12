using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class DragEventHandler : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler
{


    public Action<PointerEventData> onDragHandler;

    public Action<PointerEventData> onBeginDragHandler;

    public Action<PointerEventData> onEndDragHandler;

    static public DragEventHandler Get(GameObject go)
    {
        DragEventHandler listener = go.GetComponent<DragEventHandler>();
        if (listener == null) listener = go.AddComponent<DragEventHandler>();
        return listener;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDragHandler != null) onBeginDragHandler(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDragHandler != null) onDragHandler(eventData);
    }

    

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDragHandler != null) onEndDragHandler(eventData);
    }

}
