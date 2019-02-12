using System;
using System.Collections;
using XLua;

[LuaCallCSharp]
public class ObjectNotice : BaseNotice {

	private string currentType;
	private Object obj;
	public ObjectNotice(string type,Object obj)
	{
		currentType = type;
		this.obj = obj;
	}


	public override string GetNotificationType ()
	{
		return currentType;
	}

	public Object GetObj()
	{

		return obj;
	}
}
