using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneCameraInfo : ASceneObject
{
	public string cameraName;
	public int cullingMask;
	public float fieldOfView;
	public float nearValue;
	public float farValue;

	public float cameraDepth;


	public int clearFlags;
	public bool orthographic;
	public float orthographicSize;

    public bool cameraEnable=true;

}
