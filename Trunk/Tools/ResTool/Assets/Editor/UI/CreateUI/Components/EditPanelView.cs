using UnityEngine;
using UnityEditor;

using System.Collections;
using System;
using UnityEngine.UI;

public class EditPanelView : BaseEditView
{
   

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        PanelWidget panelWidget = widget as PanelWidget;
        DrawCommon(window, widget.gameObject, widget);

        panelWidget.Img.enabled = EditorGUILayout.Toggle("是否背景：", panelWidget.Img.enabled, GUILayout.ExpandWidth(true));
        if (panelWidget.Img.enabled)
        {
            panelWidget.Img.sprite = EditorGUILayout.ObjectField("背景图片   ：",
                  panelWidget.Img.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
                ) as Sprite;
            panelWidget.Img.color = EditorGUILayout.ColorField("背景颜色   ：", panelWidget.Img.color, GUILayout.ExpandWidth(true));
        }
        else
        {
            //panelWidget.Img.sprite = null;
        }
        LayoutGroup curLayoutGroup = CheckLayoutGroup(widget);
        LayoutGroup oldLayoutGroup = curLayoutGroup;
        curLayoutGroup = (LayoutGroup)EditorGUILayout.EnumPopup("布局组 :", curLayoutGroup, GUILayout.ExpandWidth(true));
        if (curLayoutGroup != oldLayoutGroup)
        {
            AddLayoutGroup(widget,curLayoutGroup);
        }
       

    }

   
}
