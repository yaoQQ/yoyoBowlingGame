using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditTextView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        TextWidget buttonWidget = widget as TextWidget;
        DrawCommon(window, widget.gameObject, widget);


        buttonWidget.Txt.font= EditorGUILayout.ObjectField("字体 ：",
           buttonWidget.Txt.font, typeof(Font), false, GUILayout.ExpandWidth(true)
         ) as Font;
         


    }
}
