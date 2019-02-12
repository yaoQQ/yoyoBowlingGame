using UnityEngine;
using UnityEditor;

using System.Collections;
using System;
using UnityEngine.UI;

public class EditButtonView : BaseEditView
{



    public override void Render(EditorWindow window, UIBaseWidget widget)
    {



        ButtonWidget buttonWidget = widget as ButtonWidget;
        DrawCommon(window, widget.gameObject, widget);

        SpriteState st = buttonWidget.Btn.spriteState;





        buttonWidget.Btn.image.sprite = EditorGUILayout.ObjectField("一般状态  normal ：",
             buttonWidget.Btn.image.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        buttonWidget.normalStateParameter.texColor =buttonWidget.Txt.color= EditorGUILayout.ColorField("一般状态文本颜色：",buttonWidget.Txt.color, GUILayout.ExpandWidth(true));
        RectTransform rt = (RectTransform)buttonWidget.transform;
        if(!EditorApplication.isPlaying)
        {
            buttonWidget.normalStateParameter.size = rt.sizeDelta;
        }


        st.highlightedSprite = buttonWidget.Btn.image.sprite;

        if (st.pressedSprite==null) st.pressedSprite = buttonWidget.Btn.image.sprite;

        st.pressedSprite = EditorGUILayout.ObjectField("按下状态  press ：",
             st.pressedSprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
       ) as Sprite;

        buttonWidget.pressStateSign = EditorGUILayout.Toggle("启用按下状态", buttonWidget.pressStateSign, GUILayout.ExpandWidth(true));

        if (buttonWidget.pressStateSign)
        {
            buttonWidget.pressStateParameter.size = EditorGUILayout.Vector2Field("按下状态大小：", buttonWidget.pressStateParameter.size, GUILayout.ExpandWidth(true));
            buttonWidget.pressStateParameter.texColor = EditorGUILayout.ColorField("按下状态文本颜色：", buttonWidget.pressStateParameter.texColor, GUILayout.ExpandWidth(true));
        }

        EditorGUILayout.Space();

        if (st.disabledSprite == null) st.disabledSprite = buttonWidget.Btn.image.sprite;

        st.disabledSprite = EditorGUILayout.ObjectField("禁用状态  disabled ：",
           st.disabledSprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
         ) as Sprite;


        if (st.pressedSprite == null)
        {
            st.pressedSprite = buttonWidget.Btn.image.sprite;
        }

      

        buttonWidget.Btn.spriteState = st;



        buttonWidget.Txt.enabled = EditorGUILayout.Toggle("文本是否启用：",buttonWidget.Txt.enabled, GUILayout.ExpandWidth(true));
        if(buttonWidget.Txt.enabled)
        {
            buttonWidget.Txt.font= EditorGUILayout.ObjectField("字体  ：",
            buttonWidget.Txt.font, typeof(Font), false, GUILayout.ExpandWidth(true)
       ) as Font;
            buttonWidget.Txt.text = EditorGUILayout.TextField("文本内容：", buttonWidget.Txt.text, GUILayout.ExpandWidth(true));
            buttonWidget.Txt.fontSize = EditorGUILayout.IntField("文本字体大小：", buttonWidget.Txt.fontSize, GUILayout.ExpandWidth(true));
        }
       
        
    }
}
