using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ImageEvent : MonoBehaviour {
    
    public event System.Action<UnityEngine.Collision2D> OnCollision2DEnterAct;
    public event System.Action<UnityEngine.Collider2D> OnTrigger2DEnterAct;

    public event System.Action<int,int,float,float> OnCollision2DEnterActPro;
    public event System.Action<int, int> OnTriggerEnterActPro;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (OnTrigger2DEnterAct != null)
            OnTrigger2DEnterAct(col);

        if (OnTriggerEnterActPro != null)
        {
            int layer = col.gameObject.layer;
            int insId = col.gameObject.GetInstanceID();
            OnTriggerEnterActPro(layer, insId);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (OnCollision2DEnterAct != null)
            OnCollision2DEnterAct(coll);
        if (OnCollision2DEnterActPro != null)
        {
            int layer = coll.gameObject.layer;
            int insId = coll.gameObject.GetInstanceID();
            Vector2 vec = coll.contacts[0].point;
            OnCollision2DEnterActPro(layer, insId, vec.x, vec.y);
        }
    }
}