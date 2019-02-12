using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditScrollPanelView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {

        ScrollPanelWidget scrollPanelWidget = widget as ScrollPanelWidget;
        DrawCommon(window, widget.gameObject, widget);

        scrollPanelWidget.scrollRect.horizontal = !scrollPanelWidget.scrollRect.vertical;
        scrollPanelWidget.scrollRect.horizontal = EditorGUILayout.Toggle("水平滚动开启：", scrollPanelWidget.scrollRect.horizontal, GUILayout.ExpandWidth(true));
        scrollPanelWidget.scrollRect.vertical = !scrollPanelWidget.scrollRect.horizontal;
        scrollPanelWidget.scrollRect.vertical = EditorGUILayout.Toggle("垂直滚动开启：", scrollPanelWidget.scrollRect.vertical, GUILayout.ExpandWidth(true));

        scrollPanelWidget.scrollRect.content = EditorGUILayout.ObjectField("滚动容器    ：",
                scrollPanelWidget.scrollRect.content, typeof(RectTransform), false, GUILayout.ExpandWidth(true)
              ) as RectTransform;


        UIAlign align = UITools.GetWidgetAlign(scrollPanelWidget.scrollRect.content);
        align = (UIAlign)EditorGUILayout.EnumPopup("容器对齐方式 ", align, GUILayout.ExpandWidth(true));
        UITools.SetWidgetAlign(scrollPanelWidget.scrollRect.content, align);
    }


}
