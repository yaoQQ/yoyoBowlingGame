using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine.Unity;

public class EditSpineView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {

        SpineWidget spineWidget = widget as SpineWidget;
        DrawCommon(window, widget.gameObject, widget);

        var oldData=spineWidget.skeleton.skeletonDataAsset;
        spineWidget.skeleton.skeletonDataAsset = EditorGUILayout.ObjectField("SkeletonData ：",
          spineWidget.skeleton.skeletonDataAsset, typeof(SkeletonDataAsset), false, GUILayout.ExpandWidth(true)
        ) as SkeletonDataAsset;


        if(oldData!= spineWidget.skeleton.skeletonDataAsset)
        {
            if (spineWidget.skeleton.skeletonDataAsset == null)
            {
                //spineWidget.skeleton.Clear();
            }
            else
            {
                spineWidget.skeleton.Initialize(true);
            }

        }




    }
}
