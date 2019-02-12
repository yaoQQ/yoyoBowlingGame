using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class UIExEventTool
{
    /// <summary>
    /// 从屏幕坐标转成UI系统下的根局部坐标
    /// </summary>
    /// <param name="go"></param>
    /// <param name="screenPos"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Vector2 ScreenToUIRootLocal(GameObject go, Vector2 screenPos, Camera camera)
    {
        Vector2 target = new Vector2(99999, 99999);
        var graphic = go.GetComponent<Graphic>();
        if (graphic == null)
            return target;
        var canvasRTransform = graphic.canvas.transform as RectTransform;
        Vector2 local;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, screenPos, camera, out local))
        {
            target = local;
        }
        return target;
    }

    /// <summary>
    /// 相对指定物体的局部坐标
    /// </summary>
    /// <param name="go"></param>
    /// <param name="target"></param>
    /// <param name="screenPos"></param>
    /// <param name="camera"></param>
    /// <returns></returns>
    public static Vector2 ScreenToTargetLocal(GameObject go, GameObject target, Vector2 screenPos, Camera camera)
    {
        Vector2 temp = new Vector2(99999, 99999);
        var graphic = go.GetComponent<Graphic>();
        if (graphic == null)
            return temp;
        var rTransform = target.transform as RectTransform;
        if (rTransform == null)
            return temp;
        Vector2 local;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rTransform, screenPos, camera, out local))
        {
            temp = local;
        }
        return temp;
    }

    public static Vector2? ScreenToUIRootLocalNull(GameObject go, Vector2 screenPos, Camera camera)
    {
        var graphic = go.GetComponent<Graphic>();
        if (graphic == null)
            return null;
        var canvasRTransform = graphic.canvas.transform as RectTransform;
        Vector2 local;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, screenPos, camera, out local))
        {
            return local;
        }
        else
            return null;
    }

    public static Vector3 ScreenToWorld(GameObject go, Vector2 screenPos, Camera camera)
    {
        Vector3 target = new Vector3(99999, 99999, 99999);
        var graphic = go.GetComponent<Graphic>();
        if (graphic == null)
            return target;
        var canvasRTransform = graphic.canvas.transform as RectTransform;
        Vector3 world;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRTransform, screenPos, camera, out world))
        {
            target = world;
        }
        return target;
    }

    /// <summary>
    /// 计算出需要显示的42天的DateTime值
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public static List<DateTime> Days(DateTime month)
    {
        List<DateTime> days = new List<DateTime>();
        DateTime firstDay = new DateTime(month.Year, month.Month, 1);
        DayOfWeek week = firstDay.DayOfWeek;
        int lastMonthDays = (int)week;
        if (lastMonthDays.Equals(0))
            lastMonthDays = 7;
        for (int i = lastMonthDays; i > 0; i--)
            days.Add(firstDay.AddDays(-i));
        for (int i = 0; i < 42 - lastMonthDays; i++)
            days.Add(firstDay.AddDays(i));
        return days;
    }

    /// <summary>
    /// 计算出12个月的DateTime值
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static List<DateTime> Months(DateTime year)
    {
        List<DateTime> months = new List<DateTime>();
        DateTime firstMonth = new DateTime(year.Year, 1, 1);
        months.Add(firstMonth);
        for (int i = 1; i < 12; i++)
            months.Add(firstMonth.AddMonths(i));
        return months;
    }

    /// <summary>
    /// 计算出12个年份的DateTime值
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static List<DateTime> Years(DateTime year)
    {
        List<DateTime> years = new List<DateTime>();
        //前五年
        for (int i = 5; i > 0; i--)
            years.Add(year.AddYears(-i));
        //后六年
        for (int i = 0; i < 7; i++)
            years.Add(year.AddYears(i));
        return years;
    }

    public static Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
            return color;
        return Color.black;
    }

    private static readonly Vector3[] m_Corners = new Vector3[4];
    public static Bounds GetBounds(RectTransform m_Content, RectTransform realtive)
    {
        if (m_Content == null)
            return new Bounds();
        m_Content.GetWorldCorners(m_Corners);
        var viewWorldToLocalMatrix = realtive.worldToLocalMatrix;
        return InternalGetBounds(m_Corners, ref viewWorldToLocalMatrix);
    }

    internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
    {
        var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        for (int j = 0; j < 4; j++)
        {
            Vector3 v = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[j]);
            vMin = Vector3.Min(v, vMin);
            vMax = Vector3.Max(v, vMax);
        }

        var bounds = new Bounds(vMin, Vector3.zero);
        bounds.Encapsulate(vMax);
        return bounds;
    }

    public static void AdapterTextRectTransform(Text text, RectTransform rt)
    {
        if (text == null || rt == null)
            return;
        var y = text.preferredHeight + 2;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, y);
    }

    /// <summary>
    /// 检测a,b两个Rect的边界, 看a的下边界是否比b的下边界更低
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool IsDown(RectTransform a, RectTransform b)
    {
        if (a == null || b == null)
            return false;
        Bounds childBounds = GetBounds(a, b);
        Bounds parentBounds = GetBounds(b, b);
        return childBounds.min.y < parentBounds.min.y;
    }
}
