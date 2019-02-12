using UnityEngine;
using UnityEditor;

public class GUIStyleUtil
{

    static GUIStyle groupFoldoutGUIStyle;
    public static GUIStyle GetGroupFoldoutGUIStyle()
    {
        if (groupFoldoutGUIStyle == null)
        {
            InitGroupFoldoutGUIStyle();
        }
        return groupFoldoutGUIStyle;
    }
    static void InitGroupFoldoutGUIStyle()
    {
        groupFoldoutGUIStyle = new GUIStyle(EditorStyles.foldout);
        groupFoldoutGUIStyle.alignment = TextAnchor.MiddleLeft;
        groupFoldoutGUIStyle.font = DefaultGUIStyle.font;
        groupFoldoutGUIStyle.fontSize = 18;
        //groupFoldoutGUIStyle.fontStyle = FontStyle.Bold;
    }
    //======================================================

    static GUIStyle selectedToggleGUIStyle;
    public static GUIStyle GetSelectedToggleGUIStyle()
    {
        if (selectedToggleGUIStyle == null)
        {
            InitSelectedToggleGUIStyle();
        }
        return selectedToggleGUIStyle;
    }
    static void InitSelectedToggleGUIStyle()
    {
        selectedToggleGUIStyle = new GUIStyle(GUI.skin.toggle);
        selectedToggleGUIStyle.alignment = TextAnchor.UpperLeft;
        selectedToggleGUIStyle.fontSize = 15;
        selectedToggleGUIStyle.fontStyle = FontStyle.Bold;
        //selectedToggleGUIStyle.border= new RectOffset(0, 0, 0, 0);
        //selectedToggleGUIStyle.margin = new RectOffset(0, 0, 100, 2);
        //selectedToggleGUIStyle.normal.textColor = Color.white;
        //selectedToggleGUIStyle.active.textColor = Color.white;
        //selectedToggleGUIStyle.hover.textColor = Color.white;
        //selectedToggleGUIStyle.focused.textColor = Color.white;
        selectedToggleGUIStyle.padding = new RectOffset(20, 2, 20, 0);


    }
    //===============================================================

    static GUIStyle popupGUIStyle;
    public static GUIStyle GetPopupGUIStyle()
    {
        if (popupGUIStyle == null)
        {
            InitPopupGUIStyle();
        }
        return popupGUIStyle;
    }
    static void InitPopupGUIStyle()
    {
        popupGUIStyle = new GUIStyle();
        popupGUIStyle.font = DefaultGUIStyle.font;
    }



    //=====================================================================
    public static GUISkin DefaultGUIStyle;
   

}
