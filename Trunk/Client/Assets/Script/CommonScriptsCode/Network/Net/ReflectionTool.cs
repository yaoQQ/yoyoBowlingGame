
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ReflectionTool {

	static ReflectionTool _instance;
	private ReflectionTool reflectionTool;
	
	public static ReflectionTool Instance {
		get {
			if (_instance == null) {
				_instance = new ReflectionTool ();
			}
			return _instance;
		}
	}

	Dictionary <string,object>classInsDic;

	public void Init()
	{
		classInsDic = new Dictionary<string, object> ();
	}

	public bool ReflectMethod (string className, string methodName,System.Object[] param) {

		Type theType = Type.GetType (className);
		if (theType != null) {
			MethodInfo mi = theType.GetMethod(methodName);
			if(mi!=null)
			{
				object oj  = null;
				if (classInsDic.ContainsKey (className)) {
					oj = classInsDic[className];
				}
				else
				{
				oj = Activator.CreateInstance(theType);
					classInsDic[className] = oj;
				}
				bool returnValue =(bool) mi.Invoke(oj,param);
				return returnValue;
			}
		}
		return false;
	}
	
	public void Clear()
	{
		classInsDic = new Dictionary<string, object> ();
	}
}
