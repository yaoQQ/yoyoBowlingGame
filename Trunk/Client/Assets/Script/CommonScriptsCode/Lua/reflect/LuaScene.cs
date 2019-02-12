using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface LuaScene
{

    string getSceneName();

    bool getIsInit();

    void onEnter();

    void onReset();

    void onLeave();

}
