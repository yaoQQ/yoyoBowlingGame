using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEditor;

public class AssetBundlePrefsData
{
    public class PrefsBool
    {
        public bool mBool;

        public PrefsBool(bool b)
        {
            mBool = b;
        }

        public static implicit operator bool(PrefsBool prefsBool)
        {
            return prefsBool.mBool;
        }

        public static implicit operator PrefsBool(bool b)
        {
            return new PrefsBool(b);
        }
    }

    public class PrefsStr
    {

        public string mBool;

        public PrefsStr(string b)
        {
            mBool = b;
        }

        public static implicit operator string(PrefsStr prefsBool)
        {
            return prefsBool.mBool;
        }

        public static implicit operator PrefsStr(string b)
        {
            return new PrefsStr(b);
        }
    }

    [Description("清除上一个版本的资源")]
    public static PrefsBool PREFAB_BUILD_CLEAR_KEY;
    [Description("跳过更新")]
    public static PrefsBool PREFAB_BUILD_SKIP_UPDATE_KEY;
    [Description("连接Profiler")]
    public static PrefsBool PREFAB_BUILD_CONNECT_PROFILER_KEY;
    [Description("测试类型资源打包")]
    public static PrefsBool PREFAB_BUILD_PUBLISH_TEST_KEY;
    [Description("连接测试服务器")]
    public static PrefsBool PREFAB_BUILD_TEST_SERVER_KEY;
    [Description("连接测试CDN")]
    public static PrefsBool PREFAB_BUILD_BETA_CDN_KEY;
    [Description("游戏打入Base包")]
    public static PrefsBool PREFAB_BUILD_GAME_RES_KEY;
    [Description("打包资源")]
    public static PrefsBool PREFAB_BUILD_RES_KEY;
    [Description("拷贝到StreamingAssets")]
    public static PrefsBool PREFAB_BUILD_COPY_STREAMINGASSETS_KEY;
    [Description("编译运行程序")]
    public static PrefsBool PREFAB_BUILD_BUILD_PLAYER_KEY;

    public static PrefsStr PREFAB_ANDROID_KEYSTORE_NAME;
    public static PrefsStr PREFAB_ANDROID_KEYSTORE_PASS;
    public static PrefsStr PREFAB_ANDROID_KEYSTORE_ALIAS_NAME;
    public static PrefsStr PREFAB_ANDROID_KEYSTORE_ALIAS_PASS;

    private static readonly List<FieldInfo> m_FieldInfoList = new List<FieldInfo>();
    public static readonly List<string> mDescriptionList = new List<string>();

    private static string GetKey(string name)
    {
        return "__AssetBundlePrefsData__" + name;
    }

    public static void Init()
    {
        m_FieldInfoList.Clear();
        mDescriptionList.Clear();

        FieldInfo[] fieldInfos = typeof(AssetBundlePrefsData).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (FieldInfo fieldInfo in fieldInfos)
        {
            DescriptionAttribute descriptionAttribute = null;
            object[] objs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            for (int i = 0; i < objs.Length; i++)
            {
                descriptionAttribute = objs[i] as DescriptionAttribute;
                break;
            }
            m_FieldInfoList.Add(fieldInfo);
            mDescriptionList.Add(descriptionAttribute != null ? descriptionAttribute.Description : "None");
        }

        foreach (FieldInfo fieldInfo in m_FieldInfoList)
        {
            string key = GetKey(fieldInfo.Name);
            System.Type type = fieldInfo.FieldType;
            if (type == typeof(PrefsBool))
            {
                bool b = EditorPrefs.GetBool(key, false);
                fieldInfo.SetValue(null, new PrefsBool(b));
            }
            else if (type == typeof(PrefsStr))
            {
                string b = EditorPrefs.GetString(key, string.Empty);
                fieldInfo.SetValue(null, new PrefsStr(b));
            }
        }
    }

    public static void Save()
    {
        foreach (FieldInfo fieldInfo in m_FieldInfoList)
        {
            string key = GetKey(fieldInfo.Name);
            object o = fieldInfo.GetValue(null);
            System.Type type = fieldInfo.FieldType;
            if (type == typeof(PrefsBool))
            {
                bool b = o as PrefsBool;
                EditorPrefs.SetBool(key, b);
            }
            else if (type == typeof(PrefsStr))
            {
                string s = o as PrefsStr;
                EditorPrefs.SetString(key, s);
            }
        }
    }

    public static string GetDes(PrefsBool prefsBool)
    {
        for (int i = 0; i < m_FieldInfoList.Count; i++)
        {
            FieldInfo fieldInfo = m_FieldInfoList[i];
            if (fieldInfo.GetValue(null) == prefsBool)
            {
                return mDescriptionList[i];
            }
        }
        return string.Empty;
    }

    public static string GetFieldName(PrefsBool prefsBool)
    {
        foreach (FieldInfo fieldInfo in m_FieldInfoList)
        {
            object o = fieldInfo.GetValue(null);
            if (o == prefsBool)
            {
                return fieldInfo.Name;
            }
        }
        return string.Empty;
    }
}