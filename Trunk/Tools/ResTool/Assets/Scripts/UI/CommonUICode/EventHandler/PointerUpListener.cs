using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PointerUpListener : MonoBehaviour,IPointerUpHandler {


    public Action<PointerEventData> onHandler;

    static public PointerUpListener Get(GameObject go)
    {
        PointerUpListener listener = go.GetComponent<PointerUpListener>();
        if (listener == null) listener = go.AddComponent<PointerUpListener>();
        return listener;
    }



    public void OnPointerUp(PointerEventData eventData)
    {

        if (onHandler != null) onHandler(eventData);
    }
}
