using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditGridLayoutScrollView : BaseEditView
{
    public override void Render(UnityEditor.EditorWindow window, UIBaseWidget widget)
    {
        
        GridLayoutGroupWidget gridLayoutWidget = widget as GridLayoutGroupWidget;
        DrawCommon(window, widget.gameObject, widget);

        gridLayoutWidget.InnerGridGroup.cellSize = EditorGUILayout.Vector2Field("子物体大小", gridLayoutWidget.InnerGridGroup.cellSize, GUILayout.ExpandWidth(true));

        gridLayoutWidget.InnerGridGroup.spacing = EditorGUILayout.Vector2Field("间距", gridLayoutWidget.InnerGridGroup.spacing, GUILayout.ExpandWidth(true));

        gridLayoutWidget.InnerGridGroup.startCorner = (GridLayoutGroup.Corner)EditorGUILayout.EnumPopup("开始位置", gridLayoutWidget.InnerGridGroup.startCorner, GUILayout.ExpandWidth(true));

        gridLayoutWidget.InnerGridGroup.startAxis = (GridLayoutGroup.Axis)EditorGUILayout.EnumPopup("轴向", gridLayoutWidget.InnerGridGroup.startAxis, GUILayout.ExpandWidth(true));

        gridLayoutWidget.InnerGridGroup.childAlignment = (TextAnchor)EditorGUILayout.EnumPopup("整个子物体Aligmment", gridLayoutWidget.InnerGridGroup.childAlignment, GUILayout.ExpandWidth(true));

        gridLayoutWidget.InnerGridGroup.constraint = (GridLayoutGroup.Constraint)EditorGUILayout.EnumPopup("排列参照", gridLayoutWidget.InnerGridGroup.constraint, GUILayout.ExpandWidth(true));

    }


}
