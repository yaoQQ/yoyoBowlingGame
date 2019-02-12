using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditSliderView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        SliderWidget sliderWidget = widget as SliderWidget;
        DrawCommon(window, widget.gameObject, widget);

        sliderWidget.handleSign = EditorGUILayout.Toggle("是否开启滑块", sliderWidget.handleSign, GUILayout.ExpandWidth(true));
        if (sliderWidget.handleSign)
        {
            sliderWidget.handleRange.x = EditorGUILayout.Slider(sliderWidget.handleRange.x, 0f, sliderWidget.handleRange.y, GUILayout.ExpandWidth(true));
            sliderWidget.handleRange.y = EditorGUILayout.Slider(sliderWidget.handleRange.y, 0f, 1f, GUILayout.ExpandWidth(true));
        }
       

        if (sliderWidget.slider.image.enabled)
        {
            sliderWidget.slider.image.sprite = EditorGUILayout.ObjectField("滑块图片 ：",
              sliderWidget.slider.image.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
           ) as Sprite;
        }
        else
        {
            sliderWidget.slider.image.sprite = null;
        }

        sliderWidget.bgImg.sprite = EditorGUILayout.ObjectField("背景图片 ：",
            sliderWidget.bgImg.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
         ) as Sprite;
        sliderWidget.fillImg.sprite = EditorGUILayout.ObjectField("填充图片 ：",
            sliderWidget.fillImg.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
         ) as Sprite;


    }
}
