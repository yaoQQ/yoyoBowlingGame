using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneContainerInfo : ASceneObject
{
    public string containerName;
    public int layerMaskValue;
    public SceneCellInfo[] cellInfoArr;
}
