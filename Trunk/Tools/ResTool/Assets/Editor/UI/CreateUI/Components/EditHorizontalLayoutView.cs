using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditHorizontalLayoutView : BaseEditView
{
    public override void Render(UnityEditor.EditorWindow window, UIBaseWidget widget)
    {
        HorizontalLayoutGroupWidget horizantalLayoutWidget = widget as HorizontalLayoutGroupWidget;
        DrawCommon(window, widget.gameObject, widget);      
        RectOffset _offdata = horizantalLayoutWidget.GetGroupPadding();
        horizantalLayoutWidget.InnerHorizontalGroup.padding.left = EditorGUILayout.IntField("偏移_左", _offdata.left, GUILayout.ExpandWidth(true));
        horizantalLayoutWidget.InnerHorizontalGroup.padding.right = EditorGUILayout.IntField("偏移_右", _offdata.right, GUILayout.ExpandWidth(true));
        horizantalLayoutWidget.InnerHorizontalGroup.padding.top = EditorGUILayout.IntField("偏移_上", _offdata.top, GUILayout.ExpandWidth(true));
        horizantalLayoutWidget.InnerHorizontalGroup.padding.bottom = EditorGUILayout.IntField("偏移_下", _offdata.bottom, GUILayout.ExpandWidth(true));

        horizantalLayoutWidget.InnerHorizontalGroup.spacing = EditorGUILayout.FloatField("间距", horizantalLayoutWidget.InnerHorizontalGroup.spacing, GUILayout.ExpandWidth(true));

        horizantalLayoutWidget.InnerHorizontalGroup.childAlignment = (TextAnchor)EditorGUILayout.EnumPopup("整个子物体Aligmment",horizantalLayoutWidget.InnerHorizontalGroup.childAlignment,GUILayout.ExpandWidth(true));

        horizantalLayoutWidget.InnerHorizontalGroup.childControlWidth = EditorGUILayout.Toggle("是否控制子物体的宽度", horizantalLayoutWidget.InnerHorizontalGroup.childControlWidth, GUILayout.ExpandWidth(true));
        horizantalLayoutWidget.InnerHorizontalGroup.childControlHeight = EditorGUILayout.Toggle("是否控制子物体的高度", horizantalLayoutWidget.InnerHorizontalGroup.childControlHeight, GUILayout.ExpandWidth(true));

        horizantalLayoutWidget.InnerHorizontalGroup.childForceExpandWidth = EditorGUILayout.Toggle("是否让子物体以宽对齐", horizantalLayoutWidget.InnerHorizontalGroup.childForceExpandWidth, GUILayout.ExpandWidth(true));
        horizantalLayoutWidget.InnerHorizontalGroup.childForceExpandHeight = EditorGUILayout.Toggle("是否让子物体以高对齐", horizantalLayoutWidget.InnerHorizontalGroup.childForceExpandHeight, GUILayout.ExpandWidth(true));
        

    }
	
}
