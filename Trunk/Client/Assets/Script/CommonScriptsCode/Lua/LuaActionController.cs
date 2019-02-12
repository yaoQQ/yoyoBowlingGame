using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaActionController :Singleton<LuaActionController> {

    LuaActionManager actionManager;
    public void RegistManager(LuaActionManager manager)
    {
        actionManager = manager;
    }

    public void Execute(float deltaTime_ms)
    {
        if (actionManager == null) return;
        actionManager.execute(deltaTime_ms);
    }

}
