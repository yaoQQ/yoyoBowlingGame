using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
[CustomEditor(typeof(SceneContainer))]
public class SceneContainerInspector : Editor
{

    SceneContainer model;
    public override void OnInspectorGUI()
    {
        model = target as SceneContainer;
        if (model.gameObject.name != "Container_"+ model.containerName)
        {
            model.gameObject.name = "Container_" + model.containerName;
        }
        base.DrawDefaultInspector();
    }
}