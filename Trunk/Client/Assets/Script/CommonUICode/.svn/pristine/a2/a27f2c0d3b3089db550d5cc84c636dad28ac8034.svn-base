
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PointerExitListener : MonoBehaviour,IPointerExitHandler {

    public Action<PointerEventData> onHandler;

    static public PointerExitListener Get(GameObject go)
    {
        PointerExitListener listener = go.GetComponent<PointerExitListener>();
        if (listener == null) listener = go.AddComponent<PointerExitListener>();
        return listener;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onHandler != null) onHandler(eventData);
    }
}
