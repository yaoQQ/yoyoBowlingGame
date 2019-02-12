using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class PhysicGameManager : Singleton<PhysicGameManager>
{
    private Action updateFuns;
    private Action lateUpdateFuns;
    public void addPhysic(GameObject obj, OnCollisionCallBack enter, OnCollisionCallBack exit, OnCollisionCallBack stay) {
        if (obj == null) {
            Logger.PrintError("addPhysic（） obj == null");
            return;
        }
        PhysicCollider physic = obj.GetComponent<PhysicCollider>();
        if (physic == null) {
             physic = obj.AddComponent<PhysicCollider>();
        }
        physic.OnCollisionEnterCallBackFun = enter;
        physic.OnCollisionExitCallBackFun = exit;
        physic.OnCollisionStayCallBackFun = stay;
       // Loger.PrintColor("yellow", "physic.OnCollisionEnterCallBackFun=" + physic.OnCollisionEnterCallBackFun + "  physic.OnCollisionExitCallBackFun=" + physic.OnCollisionExitCallBackFun);
    }
    public void addPhysicTrigger(GameObject obj, OnTriggerCallBack triggerEnter, OnTriggerCallBack triggerStay, OnTriggerCallBack triggerExit) {
        if (obj == null) {
            Logger.PrintError("addPhysic（） obj == null");
            return;
        }
        PhysicCollider physic = obj.GetComponent<PhysicCollider>();
        if (physic == null) {
            physic = obj.AddComponent<PhysicCollider>();
        }
        physic.OnTriggerEnterCallBackFun = triggerEnter;
        physic.OnTriggerStayCallBackFun = triggerStay;
        physic.OnTriggerExitCallBackFun = triggerExit;

     //   Loger.PrintColor("yellow", "physic.OnTriggerEnterCallBackFun=" + physic.OnTriggerEnterCallBackFun + "  physic.OnTriggerStayCallBackFun=" + physic.OnTriggerStayCallBackFun);

    }
    public void addEnableFun(GameObject obj, Action OnEnableFun, Action OnDisableFun, OnCollisionCallBack stay) {
        if (obj == null) {
            Logger.PrintError("addEnableFun（） obj == null");
            return;
        }
        PhysicCollider physic = obj.GetComponent<PhysicCollider>();
        if (physic == null) {
            physic = obj.AddComponent<PhysicCollider>();
        }
        physic.OnDisableCallBackFun = OnDisableFun;
        physic.OnEnableCallBackFun = OnEnableFun;
      //  Loger.PrintColor("yellow", "physic.OnEnableCallBackFun=" + physic.OnEnableCallBackFun + "  physic.OnDisableCallBackFun=" + physic.OnDisableCallBackFun);
    }
    public void addUpdateFun(Action luaFun) {
        updateFuns += luaFun;
    }
    public void removeUpdateFun(Action luaFun) {
        updateFuns -= luaFun;
       //Application.Quit()

    }
    public void addLateUpdateFun(Action luaFun) {
        lateUpdateFuns += luaFun;
    }
    public void Update() {
        if (updateFuns != null) {
            updateFuns();
        }
    }
    public void LateUpdate() {
        if (lateUpdateFuns != null) {
            lateUpdateFuns();
        }
    }

    public void clear() {
        updateFuns = null;
        lateUpdateFuns = null;
    }
    public int getEnumNumber(Enum targetEnum) {
        if (targetEnum == null) {

            return 0;
        }
        return targetEnum.GetHashCode();
    }
}

