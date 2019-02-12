using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PointerEnterListener : MonoBehaviour,IPointerEnterHandler {

    public Action<PointerEventData> onHandler;

    static public PointerEnterListener Get(GameObject go)
    {
        PointerEnterListener listener = go.GetComponent<PointerEnterListener>();
        if (listener == null) listener = go.AddComponent<PointerEnterListener>();
        return listener;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onHandler != null) onHandler(eventData);
    }
}
