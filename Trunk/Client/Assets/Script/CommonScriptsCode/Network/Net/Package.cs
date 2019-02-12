
using UnityEngine;
using System.Collections;
using ProtoBufSpace;

using XLua;

[LuaCallCSharp]
public class Package  {

	public uint ProtoID
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
    public byte[] ByteArray
	{
		get;
		set;
	}

	public Package(uint protocol, int returnCode, byte[] byteString)
	{
		this.ProtoID = protocol;
        this.returnCode = returnCode;
        this.ByteArray = byteString;
	}

   

  
   
  
}
