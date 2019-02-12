using UnityEngine;
using System.Collections.Generic;
using ProtoBufSpace;
using XLua;

[LuaCallCSharp]
public class NetworkEventManager 
{

	protected static NetworkEventManager instance;
	
	public static NetworkEventManager Instance {
		get {
			if (instance == null) {
				instance = new NetworkEventManager ();
			}
			return instance;
		}
	}

	protected Dictionary<uint, MessageReceive> dicEventHandler = new Dictionary<uint, MessageReceive>();
	

	public void RegisterEventHandler(uint code, MessageReceive handler)
	{
        //Loger.PrintLog("注册 ===>>>  ",code.ToString());
		if (dicEventHandler.ContainsKey(code))
		{
			dicEventHandler[code] += handler;
		}
		else
		{
			dicEventHandler[code] = handler;
		}
	}

	public void RemoveEventHandler(uint code, MessageReceive handler)
	{
		if (dicEventHandler.ContainsKey(code))
		{
			dicEventHandler[code] -= handler;
		}
    }




    public void InvokeCallBack(uint protoID, byte[] ByteArray)
	{
        //if (returnCode!=(int)ReturnCode.Success)
        //{
        //    Loger.PrintError("错误码： ", returnCode.ToString());

        //    return;
        //}



		if (dicEventHandler.ContainsKey (protoID)) {
			if (dicEventHandler [protoID] != null) {
				dicEventHandler [protoID].Invoke (protoID, ByteArray);
			}
			else
			{
				Debug.Log(protoID+" 无监听");
			}
		} else {
			Debug.Log(protoID+" 无监听");
		}
	}

	public bool ContainsKey(uint code)
	{
		return dicEventHandler.ContainsKey(code);
	}
	
	public int Count { get { return this.dicEventHandler.Count; } }

	
	protected void reset()
	{
		this.dicEventHandler.Clear();
	}
}