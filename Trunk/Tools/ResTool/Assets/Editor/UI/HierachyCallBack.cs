using System;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierachyCallBack
{
    // 层级窗口项回调
    private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

    private static Texture2D hierarchyIcon;
    private static Texture2D HierarchyIcon
    {
        get
        {
            if (HierachyCallBack.hierarchyIcon == null)
            {
                HierachyCallBack.hierarchyIcon = (Texture2D)Resources.Load("selected");
            }
            return HierachyCallBack.hierarchyIcon;
        }
    }

    private static Texture2D hierarchyEventIcon;
    private static Texture2D HierarchyEventIcon
    {
        get
        {
            if (HierachyCallBack.hierarchyEventIcon == null)
            {
                HierachyCallBack.hierarchyEventIcon = (Texture2D)Resources.Load("selected");
            }
            return HierachyCallBack.hierarchyEventIcon;
        }
    }

    /// <summary>
    /// 静态构造
    /// </summary>
    static HierachyCallBack()
    {
        HierachyCallBack.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(HierachyCallBack.DrawHierarchyIcon);
        EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
            EditorApplication.hierarchyWindowItemOnGUI,
            HierachyCallBack.hiearchyItemCallback);

        //EditorApplication.update += Update;
    }

    // 绘制icon方法
    private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if(gameObject && gameObject.GetComponent<UIBaseWidget>() && gameObject.GetComponent<UIBaseWidget>().exportSign)
        {
            // 设置icon的位置与尺寸（Hierarchy窗口的左上角是起点）
            Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
            // 画icon
            GUI.DrawTexture(rect, HierachyCallBack.HierarchyEventIcon);
        }
     
    }

    private static void Update()
    {
        Debug.Log("1");
    }
}