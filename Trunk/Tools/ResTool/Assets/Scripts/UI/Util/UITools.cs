using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class UITools
{
    public class CanvasInfo
    {
        public Canvas canvas;
        public CanvasScaler canvasScaler;
        public GraphicRaycaster graphicRaycaster;
    }
    public static CanvasInfo SetCanvasToUIGo(GameObject go)
    {
        CanvasInfo info = new CanvasInfo();
        go.layer = LayerMask.NameToLayer("UI");
        info.canvas = go.GetComponent<Canvas>();
        if (info.canvas == null)
        {
            info.canvas = go.AddComponent<Canvas>();
        }
        info.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        info.canvasScaler = go.AddComponent<CanvasScaler>();
        info.graphicRaycaster = go.AddComponent<GraphicRaycaster>();
        return info;
    }



    /// <summary>
    /// 设置父容器和对齐;
    /// </summary>
    public static void SetParentAndAlign(GameObject child, GameObject parent)
    {
        child.transform.SetParent(parent.transform);
        if (child.transform.parent as RectTransform)
        {
            RectTransform rect = child.transform as RectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
        }
    }
    /// <summary>
    /// 根据名字排序用;
    /// </summary>
    /// <param name="go"></param>
    public static void SortChildrenByName(GameObject go)
    {

        List<Transform> children = new List<Transform>();
        for (int i = go.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = go.transform.GetChild(i);
            children.Add(child);
            child.SetParent(null);
        }
        children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
        foreach (Transform child in children)
        {
            child.SetParent(go.transform);
        }
    }

    public static UIAlign GetWidgetAlign(RectTransform rt)
    {
        UIAlign align = UIAlign.Stretch_Stretch;
        if (rt.anchorMin == new Vector2(0f, 1f) && rt.anchorMax == new Vector2(0f, 1f))
            align = UIAlign.Top_Left;
        else if (rt.anchorMin == new Vector2(0.5f, 1f) && rt.anchorMax == new Vector2(0.5f, 1f))
            align = UIAlign.Top_Center;
        else if (rt.anchorMin == new Vector2(1f, 1f) && rt.anchorMax == new Vector2(1f, 1f))
            align = UIAlign.Top_Right;
        else if (rt.anchorMin == new Vector2(0f, 1f) && rt.anchorMax == new Vector2(1f, 1f))
            align = UIAlign.Top_Stretch;
        else if (rt.anchorMin == new Vector2(0f, 0.5f) && rt.anchorMax == new Vector2(0f, 0.5f))
            align = UIAlign.Middle_Left;
        else if (rt.anchorMin == new Vector2(0.5f, 0.5f) && rt.anchorMax == new Vector2(0.5f, 0.5f))
            align = UIAlign.Middle_Center;
        else if (rt.anchorMin == new Vector2(1f, 0.5f) && rt.anchorMax == new Vector2(1f, 0.5f))
            align = UIAlign.Middle_Right;
        else if (rt.anchorMin == new Vector2(0f, 0.5f) && rt.anchorMax == new Vector2(1f, 0.5f))
            align = UIAlign.Middle_Stretch;
        else if (rt.anchorMin == new Vector2(0f, 0f) && rt.anchorMax == new Vector2(0f, 0f))
            align = UIAlign.Bottom_Left;
        else if (rt.anchorMin == new Vector2(0.5f, 0f) && rt.anchorMax == new Vector2(0.5f, 0f))
            align = UIAlign.Bottom_Center;
        else if (rt.anchorMin == new Vector2(1f, 0f) && rt.anchorMax == new Vector2(1f, 0f))
            align = UIAlign.Bottom_Right;
        else if (rt.anchorMin == new Vector2(0f, 0f) && rt.anchorMax == new Vector2(1f, 0f))
            align = UIAlign.Bottom_Stretch;
        else if (rt.anchorMin == new Vector2(0f, 0f) && rt.anchorMax == new Vector2(0f, 1f))
            align = UIAlign.Stretch_Left;
        else if (rt.anchorMin == new Vector2(0.5f, 0f) && rt.anchorMax == new Vector2(0.5f, 1f))
            align = UIAlign.Stretch_Center;
        else if (rt.anchorMin == new Vector2(1f, 0f) && rt.anchorMax == new Vector2(1f, 1f))
            align = UIAlign.Stretch_Right;
        else if (rt.anchorMin == new Vector2(0f, 0f) && rt.anchorMax == new Vector2(1f, 1f))
            align = UIAlign.Stretch_Stretch;
        return align;
    }

    public static void SetWidgetAlign(RectTransform rf, UIAlign align)
    {
        switch (align)
        {
            case UIAlign.Top_Left:
                rf.anchorMin = new Vector2(0f, 1f);
                rf.anchorMax = new Vector2(0f, 1f);
                rf.pivot = new Vector2(0f, 1f);
                break;
            case UIAlign.Top_Center:
                rf.anchorMin = new Vector2(0.5f, 1f);
                rf.anchorMax = new Vector2(0.5f, 1f);
                rf.pivot = new Vector2(0.5f, 1f);
                break;
            case UIAlign.Top_Right:
                rf.anchorMin = new Vector2(1f, 1f);
                rf.anchorMax = new Vector2(1f, 1f);
                rf.pivot = new Vector2(1f, 1f);
                break;
            case UIAlign.Top_Stretch:
                rf.anchorMin = new Vector2(0f, 1f);
                rf.anchorMax = new Vector2(1f, 1f);
                rf.pivot = new Vector2(0.5f, 1f);
                break;
            case UIAlign.Middle_Left:
                rf.anchorMin = new Vector2(0f, 0.5f);
                rf.anchorMax = new Vector2(0f, 0.5f);
                rf.pivot = new Vector2(0f, 0.5f);
                break;
            case UIAlign.Middle_Center:
                rf.anchorMin = new Vector2(0.5f, 0.5f);
                rf.anchorMax = new Vector2(0.5f, 0.5f);
                rf.pivot = new Vector2(0.5f, 0.5f);
                break;
            case UIAlign.Middle_Right:
                rf.anchorMin = new Vector2(1f, 0.5f);
                rf.anchorMax = new Vector2(1f, 0.5f);
                rf.pivot = new Vector2(1f, 0.5f);
                break;
            case UIAlign.Middle_Stretch:
                rf.anchorMin = new Vector2(0f, 0.5f);
                rf.anchorMax = new Vector2(1f, 0.5f);
                rf.pivot = new Vector2(0.5f, 0.5f);
                break;
            case UIAlign.Bottom_Left:
                rf.anchorMin = new Vector2(0f, 0f);
                rf.anchorMax = new Vector2(0f, 0f);
                rf.pivot = new Vector2(0f, 0f);
                break;
            case UIAlign.Bottom_Center:
                rf.anchorMin = new Vector2(0.5f, 0f);
                rf.anchorMax = new Vector2(0.5f, 0f);
                rf.pivot = new Vector2(0.5f, 0f);
                break;
            case UIAlign.Bottom_Right:
                rf.anchorMin = new Vector2(1f, 0f);
                rf.anchorMax = new Vector2(1f, 0f);
                rf.pivot = new Vector2(1f, 0f);
                break;
            case UIAlign.Bottom_Stretch:
                rf.anchorMin = new Vector2(0f, 0f);
                rf.anchorMax = new Vector2(1f, 0f);
                rf.pivot = new Vector2(0.5f, 0f);
                break;
            case UIAlign.Stretch_Left:
                rf.anchorMin = new Vector2(0f, 0f);
                rf.anchorMax = new Vector2(0f, 1f);
                rf.pivot = new Vector2(0f, 0.5f);
                break;
            case UIAlign.Stretch_Center:
                rf.anchorMin = new Vector2(0.5f, 0f);
                rf.anchorMax = new Vector2(0.5f, 1f);
                rf.pivot = new Vector2(0.5f, 0.5f);
                break;
            case UIAlign.Stretch_Right:
                rf.anchorMin = new Vector2(1f, 0f);
                rf.anchorMax = new Vector2(1f, 1f);
                rf.pivot = new Vector2(1f, 0.5f);
                break;
            case UIAlign.Stretch_Stretch:
                rf.anchorMin = new Vector2(0f, 0f);
                rf.anchorMax = new Vector2(1f, 1f);
                rf.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
    }

}
