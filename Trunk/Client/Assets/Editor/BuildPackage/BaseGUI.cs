using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BaseGUI
{
    private static GUIStyle mBoxDef;
    public static GUIStyle verticalLineBox;

    public static GUIStyle mLabelDef { get; private set; }
    public static GUIStyle mButtonDef { get; private set; }
    public static GUIStyle mBoldLabelDef { get; private set; }
    public static GUIStyle mBoldLabelDef2 { get; private set; }

    public static GUIStyle mYellowLabel { get; protected set; }
    public static GUIStyle mNormalLabel { get; protected set; }
    public static GUIStyle mItalicLabel { get; protected set; }

    public static GUIStyle mItalicTextField { get; protected set; }
    public static GUIStyle mNormalTextField { get; protected set; }

    public static Color labelTextColor { get; protected set; }
    public static readonly Color ErrorTextColor = new Color(1f, 0f, 0f);
    public static readonly Color WarnTextColor = new Color(1f, 1f, 0f);

    public static GUIStyle InfoBox { get; private set; }
    public static GUIStyle InfoBox2 { get; private set; }
    public static GUIStyle WarnBox { get; private set; }
    public static GUIStyle ErrorBox { get; private set; }

    public static GUIContent ErrorContent;

    private static bool m_Init;

    private static Font LoadFont(string name)
    {
        return new Font();
        //return AssetDatabase.LoadAssetAtPath(string.Format("Assets/{0}/ArtResources/Font/{1}", ResourceCommon.EDITOR_RES_DIR_NAME, name), typeof(Font)) as Font;
    }

    public static void Init()
    {
        if (m_Init)
            return;

        m_Init = true;

        ErrorContent = EditorGUIUtility.IconContent("console.erroricon.sml");

        mBoxDef = new GUIStyle(GUI.skin.box);
        mBoxDef.padding = new RectOffset(6, 6, 6, 6);

        verticalLineBox = new GUIStyle()
        {
            name = "PLYDivider",
            border = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
            margin = new RectOffset(0, 0, 0, 0),
            normal = { background = EditorGUIUtility.whiteTexture },
        };

        mLabelDef = new GUIStyle(GUI.skin.label);
        Font font = LoadFont("SIMHEI.TTF");
        if (font)
        {
            mLabelDef.font = font;
            mLabelDef.fontSize = 15;
        }

        mBoldLabelDef = new GUIStyle(GUI.skin.label);
        font = LoadFont("Fang_GBK.ttf");
        if (font)
        {
            mBoldLabelDef.font = font;
            mLabelDef.fontSize = 15;
        }

        mBoldLabelDef2 = new GUIStyle(mBoldLabelDef);
        mBoldLabelDef2.fontSize = 20;

        mButtonDef = new GUIStyle(GUI.skin.button);
        font = LoadFont("SIMHEI.TTF");
        if (font)
        {
            mButtonDef.font = font;
        }

        mYellowLabel = new GUIStyle(GUI.skin.label) { normal = { textColor = Color.yellow } };
        mItalicLabel = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic };

        mNormalLabel = new GUIStyle(GUI.skin.label);

        mNormalTextField = new GUIStyle(EditorStyles.textField);
        mItalicTextField = new GUIStyle(EditorStyles.textField) { fontStyle = FontStyle.Italic };

        labelTextColor = EditorStyles.label.normal.textColor;

        InfoBox = new GUIStyle
        {
            normal = { background = LoadResource("infoBox"), textColor = labelTextColor },
            border = new RectOffset(2, 2, 2, 2),
            padding = new RectOffset(5, 5, 3, 3),
            margin = new RectOffset(5, 5, 3, 3),
            alignment = TextAnchor.LowerLeft,
            wordWrap = true
        };

        InfoBox2 = new GUIStyle
        {
            normal = { background = LoadResource("infoBox"), textColor = labelTextColor },
            border = new RectOffset(2, 2, 2, 2),
            padding = new RectOffset(5, 5, 1, 1),
            margin = new RectOffset(5, 5, 1, 1),
            alignment = TextAnchor.UpperLeft,
            wordWrap = true
        };

        ErrorBox = new GUIStyle(InfoBox) { normal = { background = LoadResource("errorBox"), textColor = ErrorTextColor } };
        WarnBox = new GUIStyle(InfoBox) { normal = { background = LoadResource("warnBox"), textColor = WarnTextColor } };
    }

    public static Texture2D LoadResource(string resourceName)
    {
        return AssetDatabase.LoadMainAssetAtPath(string.Format("Assets/Editor/Res/{0}.png", resourceName)) as Texture2D;
    }

    public static void BeginVBox()
    {
        Color backgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.white;
        GUILayout.BeginVertical(mBoxDef);
        GUI.backgroundColor = backgroundColor;
    }

    public static void EndVBox()
    {
        GUILayout.EndVertical();
    }

    static public void BeginContents()
    {
        GUILayout.BeginVertical("AS TextArea");
        GUILayout.Space(2f);
    }

    static public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        GUILayout.Space(3f);
    }

    public static void DrawVerticalLine()
    {
        DrawVerticalLine(2f, new Color(0f, 0f, 0f, 0.25f), 0f, 10f);
    }

    public static void DrawVerticalLine(float thickness, Color color, float paddingLeft = 0f, float paddingRight = 0f, float height = 0f)
    {
        GUILayoutOption[] options = new GUILayoutOption[2]
        {
            GUILayout.ExpandWidth(false),
            height > 0.0f ? GUILayout.Height(height) : GUILayout.ExpandHeight(true)
        };
        Color prevColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUILayoutUtility.GetRect(thickness + paddingLeft + paddingRight, 0f, options);
        Rect r = GUILayoutUtility.GetLastRect();
        r.x += paddingLeft;
        r.width = thickness;
        GUI.Box(r, "", verticalLineBox);
        GUI.backgroundColor = prevColor;
    }

    public static void DrawHorizontalLine()
    {
        DrawHorizontalLine(2f, new Color(0f, 0f, 0f, 0.25f), 0f, 0f);
    }

    public static void DrawHorizontalLine(float thickness, Color color, float paddingTop = 0f, float paddingBottom = 0f, float width = 0f)
    {
        GUILayoutOption[] options = new GUILayoutOption[2]
        {
            width > 0.0f ? GUILayout.Width(width) : GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(false)
        };
        Color prevColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUILayoutUtility.GetRect(0f, thickness + paddingTop + paddingBottom, options);
        Rect r = GUILayoutUtility.GetLastRect();
        r.y += paddingTop;
        r.height = thickness;
        GUI.Box(r, "", verticalLineBox);
        GUI.backgroundColor = prevColor;
    }

    #region 枚举类型的选项

    private static readonly Dictionary<System.Type, KeyValuePair<string[], int[]>> m_EnumDesDict = new Dictionary<Type, KeyValuePair<string[], int[]>>();

    public static KeyValuePair<string[], int[]> GetEnumDes<T>()
    {
        return GetEnumDes(typeof(T));
    }

    public static KeyValuePair<string[], int[]> GetEnumDes(System.Type type)
    {
        KeyValuePair<string[], int[]> keyValuePair;
        if (m_EnumDesDict.TryGetValue(type, out keyValuePair))
            return keyValuePair;

        List<string> desList = new List<string>();
        List<int> valueList = new List<int>();

        foreach (var value in Enum.GetNames(type))
        {
            string des = value;
            object[] objs = type.GetField(value).GetCustomAttributes(false);
            foreach (object o in objs)
            {
                if (o is DescriptionAttribute)
                {
                    des = (o as DescriptionAttribute).Description;
                    break;
                }
            }
            desList.Add(des);
        }

        foreach (object value in Enum.GetValues(type))
        {
            valueList.Add((int)value);
        }

        keyValuePair = new KeyValuePair<string[], int[]>(desList.ToArray(), valueList.ToArray());
        m_EnumDesDict.Add(type, keyValuePair);
        return keyValuePair;
    }

    public static string EnumPop<T>(Rect rect, string value)
    {
        KeyValuePair<string[], int[]> keyValuePair = GetEnumDes<T>();
        int p;
        int.TryParse(value, out p);
        if (keyValuePair.Value.Length > 0 && !keyValuePair.Value.Contains(p))
            p = keyValuePair.Value[0];
        return EditorGUI.IntPopup(rect, p, keyValuePair.Key, keyValuePair.Value).ToString();
    }

    public static int EnumPop(Rect rect, Enum value)
    {
        return EnumPop(rect, null, value);
    }

    public static int EnumPop(Rect rect, string label, Enum value)
    {
        KeyValuePair<string[], int[]> keyValuePair = GetEnumDes(value.GetType());
        int p = System.Convert.ToInt32(value);
        if (keyValuePair.Value.Length > 0 && !keyValuePair.Value.Contains(p))
            p = keyValuePair.Value[0];
        if (!string.IsNullOrEmpty(label))
            return EditorGUI.IntPopup(rect, label, p, keyValuePair.Key, keyValuePair.Value);
        return EditorGUI.IntPopup(rect, p, keyValuePair.Key, keyValuePair.Value);
    }

    public static int EnumPop(string title, Enum value)
    {
        return EnumPop(GUILayoutUtility.GetRect(0, 15), title, value);
    }

    #endregion

    #region ReorderableList

    public static void DrawHeaderCallback(Rect rect, int[] widths, string[] titles)
    {
        if (widths.Length == 0)
        {
            return;
        }
        if (widths.Length != titles.Length)
        {
            Debug.LogError("DrawHeaderCallback width.Length != title.Length");
            return;
        }
        Rect preRect = new Rect(rect) { xMax = rect.xMin + 20 };
        for (int i = 0; i < widths.Length; i++)
        {
            float w = widths[i];
            string t = titles[i];
            if (i == widths.Length - 1)
            {
                w = rect.xMax - preRect.xMax;
            }
            Rect curRect = new Rect(preRect) { xMin = preRect.xMax, width = w };
            preRect = curRect;
            EditorGUI.LabelField(curRect, i == 0 ? t : "|" + t);
        }
    }

    #endregion

    private static float? m_LabelWidth;

    public static void LabelWidthBegin(float w = 250.0f)
    {
        m_LabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = w;
    }

    public static void LabelWidthEnd()
    {
        if (!m_LabelWidth.HasValue)
            return;
        EditorGUIUtility.labelWidth = m_LabelWidth.Value;
        m_LabelWidth = null;
    }

}