using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mid_base_control_panel:IMiddleware
{
	public GameObject go;
	public ImageWidget control_img;

	public Mid_base_control_panel(GameObject go) 
	{
		this.go =  go;
		control_img =  go.transform.Find("control_img").GetComponent<ImageWidget>();
	}

	public void DelReference() 
	{
#if TOOL
#else
		if(go!=null) GameObject.Destroy(go);
#endif
	}

}
