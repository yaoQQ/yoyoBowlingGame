using UnityEngine;
using System.Collections;
using System.Text;

public class CommonUtils
{
    private static MonoBehaviour m_mb = null;
    private static bool m_isBetaCDN = false;
    private static bool m_isSkipUpdate = false;
    private static bool m_isIncludeGame = false;

    public static void Init(MonoBehaviour mb, bool isBetaCDN, bool isSkipUpdate, bool isIncludeGame)
    {
        m_mb = mb;
        m_isBetaCDN = isBetaCDN;
        m_isSkipUpdate = isSkipUpdate;
        m_isIncludeGame = isIncludeGame;
    }

    public static bool isUnityEditor
    {
        get { return Application.isEditor; }
    }
    public static bool isAndroid
    {
        get { return Application.platform == RuntimePlatform.Android; }
    }
    public static bool isIOS
    {
        get { return Application.platform == RuntimePlatform.IPhonePlayer; }
    }
    public static bool isBetaCDN
    {
        get { return m_isBetaCDN; }
    }
    public static bool isSkipUpdate
    {
        get { return m_isSkipUpdate; }
    }
    public static bool isIncludeGame
    {
        get { return m_isIncludeGame; }
    }

    public static Coroutine StartCoroutine(IEnumerator ie)
    {
        if (m_mb != null)
            return m_mb.StartCoroutine(ie);
        else
            return null;
    }

    public static string ConnectStrs(params string[] strs)
    {
        StringBuilder sb = new StringBuilder();
        int len = strs.Length;
        for (int i = 0; i < len; ++i)
            sb.Append(strs[i]);
        return sb.ToString();
    }
}