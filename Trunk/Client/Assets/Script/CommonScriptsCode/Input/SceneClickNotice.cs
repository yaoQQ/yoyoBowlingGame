using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class SceneClickNotice :BaseNotice
{
    public override string GetNotificationType()
    {
        return NoticeType.Scene_Click_Event;
    }
    

    public string cameraName;

    public SceneCell sceneCell;

}
