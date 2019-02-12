using System;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LoadingBarController : MonoBehaviour
{
    private static LoadingBarController m_instance = null;
    
    private LoadingBarView m_loadingBar;

    /// <summary>是否在进度条满时自动关闭</summary>
    public static bool isAutoClose = true;

    /// <summary>结束回调</summary>
    private Action m_finishCallback = null;
    /// <summary>目标进度值</summary>
    private float m_progressRunValue = 0f;
    /// <summary>达到目标进度值需要的帧数</summary>
    private int m_progressRunFrame = 0;
    /// <summary>是否需要关闭</summary>
    private bool m_isNeedClose = false;
    /// <summary>需要关闭的时间</summary>
    //private float m_closeTime = 0f;
    /// <summary>是否在播动画</summary>
    private bool m_isShowSpine = false;
    private static int m_spineIndex = 0;


    private static bool m_isInit = false;
    [BlackList]
    public static void Init(GameObject container)
    {
        if (m_isInit)
            return;

#if UNITY_ANDROID && !UNITY_EDITOR
        AssetBundle resAB = AssetBundle.LoadFromFile(Application.dataPath + "!assets/res_login/login.unity3d");
#else
        AssetBundle resAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/res_login/login.unity3d");
#endif
        GameObject barGO = resAB.LoadAsset<GameObject>("load_bar_view.prefab");

        GameObject barGOInstantiate = GameObject.Instantiate(barGO);
        barGOInstantiate.AddComponent<LoadingBarController>();
        m_instance.m_loadingBar = new LoadingBarView();
        m_instance.m_loadingBar.Init(barGOInstantiate, container);
        m_isInit = true;
    }

    public static void SetContainer(GameObject container)
    {
        m_instance.m_loadingBar.SetContainer(container);
    }

    void Awake()
    {
        m_instance = this;
    }
    
    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;
        float curTime = Time.realtimeSinceStartup;
        if (m_isNeedClose/* && curTime > m_closeTime*/)
        {
            Loger.PrintLog("现在关闭Loading界面");
            if (m_instance.m_finishCallback != null)
                m_instance.m_finishCallback();
            Hide();
            return;
        }
        if (m_progressRunFrame > 0 && m_progressRunValue > GetProgressValue())
        {
            SetProgressValue(GetProgressValue() + ((m_progressRunValue - GetProgressValue()) / ((float)m_progressRunFrame)));
            m_progressRunFrame--;
        }
        if (isAutoClose && !m_isNeedClose && !m_isShowSpine && GetProgressValue() >= 1f)
        {
            Loger.PrintLog("准备关闭Loading界面");
            if (m_spineIndex == 0)
            {
                m_isShowSpine = true;
                m_instance.m_loadingBar.ShowSpine(() =>
                {
                    m_instance.m_isShowSpine = false;
                    m_isNeedClose = true;
                    m_spineIndex = 1;
                    //m_closeTime = curTime + 0.3f;
                });
            }
            else
                m_isNeedClose = true;
        }

        m_instance.m_loadingBar.Update();
    }

    public static void Show()
    {
        if (m_instance == null)
            return;
        Loger.PrintLog("打开Loading界面");
        m_instance.m_loadingBar.Show(true);
        m_instance.m_isNeedClose = false;
        m_instance.m_isShowSpine = false;
        SetLoadContent("");
        SetProgress(0f, 0);
        NoticeManager.Instance.Dispatch(NoticeType.Loading_Bar_Show);
    }

    public static void Hide()
    {
        if (m_instance == null)
            return;
        Loger.PrintLog("关闭Loading界面");
        m_instance.m_loadingBar.Show(false);
        m_instance.m_finishCallback = null;
        NoticeManager.Instance.Dispatch(NoticeType.Loading_Bar_Hide);
    }

    public static void SetLoadContent(string content)
    {
        m_instance.m_loadingBar.SetLoadContent(content);
    }

    public static void ShowProgressWindow()
    {
        m_instance.m_loadingBar.ShowProgressWindow();
    }

    public static void HideProgressWindow()
    {
        m_instance.m_loadingBar.HideProgressWindow();
    }

    public static void SetProgress(float value, int frame = 30)
    {
        if (m_instance == null)
            return;
        value = Mathf.Clamp(value, 0f, 100f);
        if (GetProgressValue() >= value || frame <= 0)
        {
            SetProgressValue(value);
            m_instance.m_progressRunValue = value;
            m_instance.m_progressRunFrame = 0;
            if (isAutoClose && !m_instance.m_isShowSpine && value >= 1f)
            {
                if (m_spineIndex == 0)
                {
                    m_instance.m_isShowSpine = true;
                    m_instance.m_loadingBar.ShowSpine(() =>
                    {
                        m_instance.m_isShowSpine = false;
                        m_instance.m_isNeedClose = true;
                        //m_instance.m_closeTime = Time.realtimeSinceStartup + 0.3f;
                    });
                }
                else
                    m_instance.m_isNeedClose = true;
            }
        }
        else
        {
            m_instance.m_progressRunValue = value;
            m_instance.m_progressRunFrame = frame;
        }
    }

    private static float GetProgressValue()
    {
        return m_instance.m_loadingBar.GetProgressValue();
    }

    private static void SetProgressValue(float value)
    {
        m_instance.m_loadingBar.SetProgress(value);
    }
    
    public static void SetVersions(string str)
    {
        m_instance.m_loadingBar.SetVersions(str);
    }

    [BlackList]
    public static void ShowNotice(string msg, string btnName, Action onBtnfunc)
    {
        m_instance.m_loadingBar.ShowNotice(msg, btnName, onBtnfunc);
    }

    [BlackList]
    public static void ShowNotice2(string msg, string btnName1, Action onBtnfunc1, string btnName2, Action onBtnfunc2)
    {
        m_instance.m_loadingBar.ShowNotice2(msg, btnName1, onBtnfunc1, btnName2, onBtnfunc2);
    }
}
