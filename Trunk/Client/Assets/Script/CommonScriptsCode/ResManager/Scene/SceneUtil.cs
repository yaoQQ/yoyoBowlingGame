using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUtil
{

	public static void SetLayer(GameObject go, int layerValue)
	{
		go.layer = layerValue;
		SetLayerRecursion(go, layerValue);
	}
	static void SetLayerRecursion(GameObject go, int layerValue)
	{
		for (int i = 0; i < go.transform.childCount; i++)
		{
			GameObject child = go.transform.GetChild(i).gameObject;
			child.layer = layerValue;
			SetLayerRecursion(child, layerValue);
		}
	}
}
