using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mid_login_panel:IMiddleware
{
	public GameObject go;
	public ButtonWidget closeBtn;
	public TextWidget showTxt;
	public InputFieldWidget InputField;
	public ButtonWidget okBtn;

	public Mid_login_panel(GameObject go) 
	{
		this.go =  go;
		closeBtn =  go.transform.Find("closeBtn").GetComponent<ButtonWidget>();
		showTxt =  go.transform.Find("showTxt").GetComponent<TextWidget>();
		InputField =  go.transform.Find("InputField").GetComponent<InputFieldWidget>();
		okBtn =  go.transform.Find("okBtn").GetComponent<ButtonWidget>();
	}

	public void DelReference() 
	{
#if TOOL
#else
		if(go!=null) GameObject.Destroy(go);
		//UILoadTool.Instance.DelUIReference("login_panel",1);
#endif
	}

}
