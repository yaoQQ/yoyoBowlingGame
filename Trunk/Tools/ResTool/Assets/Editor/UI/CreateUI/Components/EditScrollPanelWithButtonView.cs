using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditScrollPanelWithButtonView : BaseEditView
{

    public override void Render(UnityEditor.EditorWindow window, UIBaseWidget widget)
    {
        ScrollPanelWithButtonWidget horizantalLayoutWidget = widget as ScrollPanelWithButtonWidget;
        DrawCommon(window, widget.gameObject, widget);

        horizantalLayoutWidget.BannerSample = EditorGUILayout.ObjectField("bannerSampleObj", horizantalLayoutWidget.BannerSample, typeof(GameObject), false, GUILayout.ExpandWidth(true)) as GameObject;
        
        horizantalLayoutWidget.contentRT = EditorGUILayout.ObjectField("bannersLayoutObj", horizantalLayoutWidget.contentRT,typeof(RectTransform), false, GUILayout.ExpandWidth(true)) as RectTransform;

        horizantalLayoutWidget.UseBottonPoint = EditorGUILayout.Toggle("是否启用按钮控制banner展示",horizantalLayoutWidget.UseBottonPoint,GUILayout.ExpandWidth(true));
        if (horizantalLayoutWidget.UseBottonPoint)
        {
            horizantalLayoutWidget.PointBtSample = EditorGUILayout.ObjectField("pointSampleObj", horizantalLayoutWidget.PointBtSample, typeof(GameObject), false, GUILayout.ExpandWidth(true)) as GameObject;
        
            horizantalLayoutWidget.pointRt = EditorGUILayout.ObjectField("pointBtnsLayoutObj", horizantalLayoutWidget.pointRt, typeof(RectTransform), false, GUILayout.ExpandWidth(true)) as RectTransform;

        }
    }
}
