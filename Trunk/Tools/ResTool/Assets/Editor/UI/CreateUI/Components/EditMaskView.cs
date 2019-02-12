using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditMaskView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        MaskWidget maskWidget = widget as MaskWidget;
        DrawCommon(window, widget.gameObject, widget);
        bool curEffectMaskSign = maskWidget.maskImg.material.name != "Default UI Material";
        bool oldSign = curEffectMaskSign;
        curEffectMaskSign = EditorGUILayout.Toggle("特效遮罩 " , curEffectMaskSign, GUILayout.ExpandWidth(true));

        if(curEffectMaskSign!= oldSign)
        {
            if(curEffectMaskSign)
            {
                maskWidget.maskImg.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/EffectMaskMat.mat", typeof(Material));
                if (maskWidget.maskImg.material == null)
                {
                    Debug.LogError("EffectMaskMat 材质球不存在");
                }
            }
            else
            {
                maskWidget.maskImg.material = null;
            }
        }
        maskWidget.uiMask.enabled = EditorGUILayout.Toggle("UI 遮罩 ", maskWidget.uiMask.enabled, GUILayout.ExpandWidth(true));

        if(maskWidget.uiMask.enabled)
        {
            //maskWidget.uiMask.showMaskGraphic = false;
            maskWidget.maskImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            maskWidget.maskImg.color = new Color(1, 1, 1, 0);
        }
    }
}
