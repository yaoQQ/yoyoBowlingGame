using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEnterListener : MonoBehaviour {

    public Action<Collider2D> onHandler;



    static public TriggerEnterListener Get(GameObject go)
    {
        TriggerEnterListener listener = go.GetComponent<TriggerEnterListener>();
        if (listener == null) listener = go.AddComponent<TriggerEnterListener>();
        return listener;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter(collision);
    }

    public void OnEnter(Collider2D eventData)
    {

        if (onHandler != null) onHandler(eventData);

    }

}
