using UnityEngine;
using System.Collections;
using ProtoBufSpace;

using XLua;

[LuaCallCSharp]
public class JsonPackage
{

    public string ProtoID
    {
        get;
        set;
    }

    public int returnCode
    {
        get;
        set;
    }

    //消息
    public string jsonData
    {
        get;
        set;
    }

    public JsonPackage(string protocol, int returnCode, string json) {
        this.ProtoID = protocol;
        this.returnCode = returnCode;
        this.jsonData = json;
    }






}
