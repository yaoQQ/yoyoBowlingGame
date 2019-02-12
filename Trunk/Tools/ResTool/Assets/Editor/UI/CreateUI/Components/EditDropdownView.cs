using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditDropdownView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        DropdownWidget dropdownWidget = widget as DropdownWidget;
        DrawCommon(window, widget.gameObject, widget);

        dropdownWidget.Img.sprite = EditorGUILayout.ObjectField("背景图片 ：",
             dropdownWidget.Img.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        dropdownWidget.Arrow.sprite = EditorGUILayout.ObjectField("Arrow 图片 ：",
             dropdownWidget.Arrow.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        dropdownWidget.Template.sprite = EditorGUILayout.ObjectField("下拉列表背景图片 ：",
             dropdownWidget.Template.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        dropdownWidget.ItemBackground.sprite = EditorGUILayout.ObjectField("单元背景图片 ：",
             dropdownWidget.ItemBackground.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        dropdownWidget.ItemCheckmark.sprite = EditorGUILayout.ObjectField("单元 Checkmark 图片 ：",
             dropdownWidget.ItemCheckmark.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;


        //DropdownWidget dropdownWidget = widget as DropdownWidget;
        //DrawCommon(window, widget.gameObject, widget);
    }


}
