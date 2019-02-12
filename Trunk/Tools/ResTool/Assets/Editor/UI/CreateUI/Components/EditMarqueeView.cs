using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditMarqueeView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        MarqueeWidget marqueeWidget = widget as MarqueeWidget;
        DrawCommon(window, widget.gameObject, widget);

        Font oldFont = marqueeWidget.txtArr[0].font;
        marqueeWidget.txtArr[0].font = EditorGUILayout.ObjectField("字体 ：",
         marqueeWidget.txtArr[0].font, typeof(Font), false, GUILayout.ExpandWidth(true)
       ) as Font;
        if(oldFont!= marqueeWidget.txtArr[0].font)
        {
            foreach(Text txt in marqueeWidget.txtArr)
            {
                txt.font = marqueeWidget.txtArr[0].font;
            }
        }

        Color oldColor = marqueeWidget.txtArr[0].color;
        marqueeWidget.txtArr[0].color = EditorGUILayout.ColorField("字体色 : ", marqueeWidget.txtArr[0].color);
        if (oldColor != marqueeWidget.txtArr[0].color)
        {
            foreach (Text txt in marqueeWidget.txtArr)
            {
                txt.color = marqueeWidget.txtArr[0].color;
            }
        }

        int oldSize = marqueeWidget.txtArr[0].fontSize;
        marqueeWidget.txtArr[0].fontSize = EditorGUILayout.DelayedIntField("字体大小：", marqueeWidget.txtArr[0].fontSize);
        if (oldSize != marqueeWidget.txtArr[0].fontSize)
        {
            foreach (Text txt in marqueeWidget.txtArr)
            {
                txt.fontSize = marqueeWidget.txtArr[0].fontSize;
            }
        }

        marqueeWidget.curInsertStyle = (MarqueeWidget.InsertStyle)EditorGUILayout.EnumPopup("马灯插入方式：",marqueeWidget.curInsertStyle);

        marqueeWidget.Speed = EditorGUILayout.IntSlider("（速度）每秒多少像素", marqueeWidget.Speed,1,100);

        marqueeWidget.isLoop = EditorGUILayout.Toggle("是否循环：", marqueeWidget.isLoop);

        marqueeWidget.beginFreeze_MS = EditorGUILayout.IntSlider("开始间隔（毫秒）：", marqueeWidget.beginFreeze_MS, 0, 10000);
        marqueeWidget.endFreeze_MS = EditorGUILayout.IntSlider("结束间隔（毫秒）：", marqueeWidget.endFreeze_MS, 0, 10000);

        marqueeWidget.space = EditorGUILayout.Slider("（像素） 文本之间的间隔: ", marqueeWidget.space, 0f, 100f);

        Mask m = marqueeWidget.mask.GetComponent<Mask>();
        m.showMaskGraphic = EditorGUILayout.Toggle("是否显示mask图片：", m.showMaskGraphic);
        marqueeWidget.showMaskSign= m.showMaskGraphic;

        marqueeWidget.maskImg.sprite = EditorGUILayout.ObjectField("mask图片 ：",
         marqueeWidget.maskImg.sprite, typeof(Sprite), false, GUILayout.ExpandWidth(true)
       ) as Sprite;
    }

}
