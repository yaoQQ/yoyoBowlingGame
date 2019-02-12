using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditCircleImageView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {

        CircleImageWidget imageWidget = widget as CircleImageWidget;
        DrawCommon(window, widget.gameObject, widget);

        imageWidget.Img.sprite=EditorGUILayout.ObjectField("图片 ：",
          imageWidget.Img.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
        ) as Sprite;

        imageWidget.Img.enabled = EditorGUILayout.Toggle("是否启用图片 ： ",imageWidget.Img.enabled, GUILayout.ExpandWidth(true));


        imageWidget.defaultModeSign = EditorGUILayout.Toggle("默认图片模式（物品显示之类） ： ", imageWidget.defaultModeSign, GUILayout.ExpandWidth(true));

        if (imageWidget.defaultModeSign)
        {
            imageWidget.defaultPng = imageWidget.Img.sprite;
        }
        else
        {
            imageWidget.defaultPng = null; 
        }

        if (GUILayout.Button("使用原图大小"))
        {          
            if (imageWidget.Img != null)
            {
                float x = imageWidget.Img.sprite.rect.width;
                float y = imageWidget.Img.sprite.rect.height;

                setImgSize(widget.gameObject, x, y);
            }           
        }
  
    }
}
