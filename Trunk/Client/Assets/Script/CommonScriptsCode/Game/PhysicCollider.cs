using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XLua;

[CSharpCallLua]
public delegate void OnCollisionCallBack(Collision collision);
[CSharpCallLua]
public delegate void OnTriggerCallBack(Collider collision);


public class PhysicCollider : MonoBehaviour
{
    public OnCollisionCallBack OnCollisionEnterCallBackFun;
    public OnCollisionCallBack OnCollisionStayCallBackFun;
    public OnCollisionCallBack OnCollisionExitCallBackFun;

    public OnTriggerCallBack OnTriggerEnterCallBackFun;
    public OnTriggerCallBack OnTriggerStayCallBackFun;
    public OnTriggerCallBack OnTriggerExitCallBackFun;

    public Action OnDisableCallBackFun;
    public Action OnEnableCallBackFun;

    public void OnDisable() {
        if (OnDisableCallBackFun != null) {
            OnDisableCallBackFun();
        }
    }

    public void OnEnable() {
        if (OnEnableCallBackFun != null) {
            OnEnableCallBackFun();
        }
    }

    public void OnTriggerEnter(Collider collision) {
        if (OnTriggerEnterCallBackFun != null) {
            OnTriggerEnterCallBackFun(collision);
        }
    }

    public void OnTriggerStay(Collider collision) {
        if (OnTriggerStayCallBackFun != null) {
            OnTriggerStayCallBackFun(collision);
        }
    }

    public void OnTriggerExit(Collider collision) {
        if (OnTriggerExitCallBackFun != null) {
            OnTriggerExitCallBackFun(collision);
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (OnCollisionEnterCallBackFun != null) {
            OnCollisionEnterCallBackFun(collision);
        }
    }

    public void OnCollisionStay(Collision collision) {
        if (OnCollisionStayCallBackFun != null) {
            OnCollisionStayCallBackFun(collision);
        }
    }

    public void OnCollisionExit(Collision collision) {
        if (OnCollisionExitCallBackFun != null) {
            OnCollisionExitCallBackFun(collision);
        }
    }
}

