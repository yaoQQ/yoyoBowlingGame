using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditToggleView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {



        ToggleWidget toggleWidget = widget as ToggleWidget;
        DrawCommon(window, widget.gameObject, widget);

        toggleWidget.CheackMaskImg.sprite= EditorGUILayout.ObjectField("Cheack 图片 ：",
             toggleWidget.CheackMaskImg.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;


        
        toggleWidget.BgImg.sprite = EditorGUILayout.ObjectField("背景 图片 ：",
            toggleWidget.BgImg.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
       ) as Sprite;


        toggleWidget.Txt.gameObject.SetActive (EditorGUILayout.Toggle("是否启用文本 ：", toggleWidget.Txt.gameObject.activeSelf, GUILayout.ExpandWidth(true)));


    }
}
