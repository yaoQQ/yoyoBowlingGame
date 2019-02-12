using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditVerticalLayoutGroupView : BaseEditView
{

    public override void Render(UnityEditor.EditorWindow window, UIBaseWidget widget)
    {
        VerticalLayoutGroupWidget verticalLayoutWidget =  widget as VerticalLayoutGroupWidget;
        DrawCommon(window, widget.gameObject, widget);
        RectOffset _offdata = verticalLayoutWidget.GetGroupPadding();
        verticalLayoutWidget.InnerVerticalGroup.padding.left = EditorGUILayout.IntField("偏移_左", _offdata.left, GUILayout.ExpandWidth(true));
        verticalLayoutWidget.InnerVerticalGroup.padding.right = EditorGUILayout.IntField("偏移_右", _offdata.right, GUILayout.ExpandWidth(true));
        verticalLayoutWidget.InnerVerticalGroup.padding.top = EditorGUILayout.IntField("偏移_上", _offdata.top, GUILayout.ExpandWidth(true));
        verticalLayoutWidget.InnerVerticalGroup.padding.bottom = EditorGUILayout.IntField("偏移_下", _offdata.bottom, GUILayout.ExpandWidth(true));

        verticalLayoutWidget.InnerVerticalGroup.spacing = EditorGUILayout.FloatField("间距", verticalLayoutWidget .InnerVerticalGroup.spacing, GUILayout.ExpandWidth(true));

        verticalLayoutWidget .InnerVerticalGroup.childAlignment = (TextAnchor)EditorGUILayout.EnumPopup("整个子物体Aligmment", verticalLayoutWidget .InnerVerticalGroup.childAlignment, GUILayout.ExpandWidth(true));

        verticalLayoutWidget.InnerVerticalGroup.childControlWidth = EditorGUILayout.Toggle("是否控制子物体的宽度", verticalLayoutWidget .InnerVerticalGroup.childControlWidth, GUILayout.ExpandWidth(true));
        verticalLayoutWidget.InnerVerticalGroup.childControlHeight = EditorGUILayout.Toggle("是否控制子物体的高度", verticalLayoutWidget .InnerVerticalGroup.childControlHeight, GUILayout.ExpandWidth(true));

        verticalLayoutWidget.InnerVerticalGroup.childForceExpandWidth = EditorGUILayout.Toggle("是否让子物体以宽对齐", verticalLayoutWidget .InnerVerticalGroup.childForceExpandWidth, GUILayout.ExpandWidth(true));
        verticalLayoutWidget.InnerVerticalGroup.childForceExpandHeight = EditorGUILayout.Toggle("是否让子物体以高对齐", verticalLayoutWidget .InnerVerticalGroup.childForceExpandHeight, GUILayout.ExpandWidth(true));
        

    }
}
