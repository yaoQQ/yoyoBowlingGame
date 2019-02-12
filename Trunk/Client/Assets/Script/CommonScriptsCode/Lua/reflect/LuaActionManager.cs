using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;

[CSharpCallLua]
public interface  LuaActionManager {

    void execute(float deltaTime_ms);
}
