using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIExDoubleClickListener : UIBehaviour, IPointerClickHandler
{
    public static UIExDoubleClickListener Get(GameObject go)
    {
        UIExDoubleClickListener listener = go.GetComponent<UIExDoubleClickListener>();
        if (listener == null) listener = go.AddComponent<UIExDoubleClickListener>();
        return listener;
    }

    public Action<PointerEventData> onHandler;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && onHandler != null)
        {
            onHandler.Invoke(eventData);
        }
    }
}
