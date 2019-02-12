using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using System.Collections;

public abstract class BaseEditView
{

    protected enum LayoutGroup
    {
        None,
        Vertical,
        Horizontal,
        Grid,
    }

    public abstract void Render(EditorWindow window, UIBaseWidget widget);



    protected void DrawCommon(EditorWindow window, GameObject go, UIBaseWidget widget)
    {


        RectTransform rt = go.transform as RectTransform;

        GUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("组件名称:", new GUILayoutOption[] { GUILayout.Width(100f) });

        go.name = EditorGUILayout.TextField(go.name, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();


        GUILayout.Label("组件类型 :" + widget.GetWidgetType().ToString(), new GUILayoutOption[] { GUILayout.Width(250f) });

        EditorGUILayout.BeginHorizontal();
        widget.exportSign = EditorGUILayout.Toggle("导出：", widget.exportSign, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        widget.ignoreEventSign = EditorGUILayout.Toggle("是否忽略事件：", widget.ignoreEventSign, GUILayout.ExpandWidth(true));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        UIAlign align = UITools.GetWidgetAlign(rt);
        var pre = align;
        var cur = (UIAlign)EditorGUILayout.EnumPopup("对齐align", align, GUILayout.ExpandWidth(true));
        //align = (UIAlign)EditorGUILayout.EnumPopup("对齐align", align, GUILayout.ExpandWidth(true));
        if (pre != cur)
            UITools.SetWidgetAlign(rt, align);

        rt.anchoredPosition = EditorGUILayout.Vector2Field("位置：", rt.anchoredPosition, GUILayout.ExpandWidth(true));
        rt.sizeDelta = EditorGUILayout.Vector2Field("大小：", rt.sizeDelta, GUILayout.ExpandWidth(true));

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        bool layoutElementSign = CheckLayoutElement(widget);
        bool oldLayoutElementSign = layoutElementSign;

        layoutElementSign = EditorGUILayout.Toggle("是否布局单元 ：", layoutElementSign, GUILayout.ExpandWidth(true));
        if (layoutElementSign != oldLayoutElementSign)
        {
            DealLayoutElement(widget, layoutElementSign);
        }

        GUILayout.EndHorizontal();
    }

    protected void setImgSize(GameObject go, float x, float y)
    {
        RectTransform rt = go.transform as RectTransform;

        rt.sizeDelta = new Vector2(x, y);
    }


    protected void Del(GameObject go)
    {
        GameObject.Destroy(go);
    }


    bool CheckLayoutElement(UIBaseWidget widget)
    {
        return widget.gameObject.GetComponent<LayoutElement>() != null;
    }
    void DealLayoutElement(UIBaseWidget widget, bool sign)
    {
        if (sign)
        {
            widget.gameObject.AddComponent<LayoutElement>();
        }
        else
        {
            LayoutElement layoutElement = widget.gameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                GameObject.DestroyImmediate(layoutElement);
            }
        }
    }


    protected LayoutGroup CheckLayoutGroup(UIBaseWidget widget)
    {

        return CheckLayoutGroup(widget.gameObject);
    }
    protected LayoutGroup CheckLayoutGroup(GameObject go)
    {
        if (go.GetComponent<VerticalLayoutGroup>() != null)
        {
            return LayoutGroup.Vertical;
        }
        if (go.GetComponent<HorizontalLayoutGroup>() != null)
        {
            return LayoutGroup.Horizontal;
        }
        if (go.GetComponent<GridLayoutGroup>() != null)
        {
            return LayoutGroup.Grid;
        }
        return LayoutGroup.None;
    }
    protected void AddLayoutGroup(UIBaseWidget widget, LayoutGroup layoutGroup)
    {
        AddLayoutGroup(widget.gameObject, layoutGroup);

    }
    protected void AddLayoutGroup(GameObject go, LayoutGroup layoutGroup)
    {
        VerticalLayoutGroup verticalLayoutGroup = go.GetComponent<VerticalLayoutGroup>();
        HorizontalLayoutGroup horizontalLayoutGroup = go.GetComponent<HorizontalLayoutGroup>();
        GridLayoutGroup gridLayoutGroup = go.GetComponent<GridLayoutGroup>();

        if (verticalLayoutGroup != null)
        {
            GameObject.DestroyImmediate(verticalLayoutGroup);
        }
        if (horizontalLayoutGroup != null)
        {
            GameObject.DestroyImmediate(horizontalLayoutGroup);
        }
        if (gridLayoutGroup != null)
        {
            GameObject.DestroyImmediate(gridLayoutGroup);
        }
        switch (layoutGroup)
        {
            case LayoutGroup.Vertical:
                go.AddComponent<VerticalLayoutGroup>();
                break;
            case LayoutGroup.Horizontal:
                go.AddComponent<HorizontalLayoutGroup>();
                break;
            case LayoutGroup.Grid:
                go.AddComponent<GridLayoutGroup>();
                break;
        }

    }
}
