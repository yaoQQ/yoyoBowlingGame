using System.Collections;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public Canvas loginCanvas;

    private static Driver m_instance;

    public static Driver Instance
    {
        get { return m_instance; }
    }

    void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(gameObject);

        Logger.Init();
        GlobalTimeManager.Instance.Init();

#if INCLUDE_GAME
        UtilMethod.isIncludeGame = true;
        Loger.PrintLog("INCLUDE_GAME");
#endif
#if SKIP_UPDATE
        UtilMethod.isSkipUpdate = true;
        Loger.PrintLog("SKIP_UPDATE");
#endif
#if TEST_SERVER
        UtilMethod.isTestServer = true;
        Loger.PrintLog("TEST_SERVER");
#endif
#if BETA_CDN
        UtilMethod.isBetaCDN = true;
        Loger.PrintLog("BETA_CDN");
#endif

#if UNITY_EDITOR
        UtilMethod.isIncludeGame = false;
        UtilMethod.isSkipUpdate = true;
        UtilMethod.isTestServer = true;
        UtilMethod.isBetaCDN = true;
#endif

#if CHANNEL_1
        UtilMethod.channel = 1;
#elif CHANNEL_2
        UtilMethod.channel = 2;
#elif CHANNEL_3
        UtilMethod.channel = 3;
#elif CHANNEL_4
        UtilMethod.channel = 4;
#elif CHANNEL_5
        UtilMethod.channel = 5;
#endif
        Loger.PrintLog(CommonUtils.ConnectStrs("channel:", UtilMethod.channel.ToString()));

        CommonUtils.Init(this, UtilMethod.isBetaCDN, UtilMethod.isSkipUpdate, UtilMethod.isIncludeGame);
        CommonPathUtils.InitCdnRootPath("http://yoyores.51e-sport.com/");
        if (UtilMethod.isTestServer && !UtilMethod.IsUnityEditor())
		    ResLoadManager.isPrintLoadLog = true;
        else
            ResLoadManager.isPrintLoadLog = false;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        Loger.PrintLog("Application.targetFrameRate" + Application.targetFrameRate);

        ThreadManager threadManager = ThreadManager.Instance;

        //必须先设置平台，否则路径会有问题
#if UNITY_ANDROID
        CommonPathUtils.platformStr = "Android";
#elif UNITY_IPHONE || UNITY_IOS
        if (UtilMethod.channel > 0)
            CommonPathUtils.platformStr = "IOS_B";
        else
            CommonPathUtils.platformStr = "IOS";
#else
        CommonPathUtils.platformStr = "PC";
#endif
        CommonPathUtils.isLoadEditorRes = false ;

        //iOS输入框特殊处理
        InputFieldWidget.onSelect = () =>
        {
            PlatformSDK.ActiveInitializeUI();
        };

#if UNITY_IPHONE || UNITY_IOS
#else
        // 开启SDK的日志打印，发布版本请务必关闭
        BuglyAgent.ConfigDebugMode(true);
        // ဳ注册日志回调，替换使用 'Application.RegisterLogCallback(Application.LogCallback)'ဳ注册日志回调的方式
        BuglyAgent.RegisterLogCallback(BuglyCallbackDelegate.Instance.OnApplicationLogCallbackHandler);

#if UNITY_IPHONE || UNITY_IOS
        if (UtilMethod.isTestServer)
            BuglyAgent.InitWithAppId("1a7387089f");
        else
            BuglyAgent.InitWithAppId("104523e50f");
#elif UNITY_ANDROID
        if (UtilMethod.isTestServer)
            BuglyAgent.InitWithAppId("9a85db878b");
        else
            BuglyAgent.InitWithAppId("4875ef4da2");
#endif

        // 如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
        BuglyAgent.EnableExceptionHandler();
#endif

#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        Debug.Log("设置分辨率");
        Screen.SetResolution(432, 768, false);
#endif
    }

    void Start()
    {
        LoadingBarController.Init(loginCanvas.gameObject);
        LoadingBarController.SetLoadContent("正在读取版本信息");
        LoadingBarController.SetProgress(0.1f, 30);
        StartCoroutine(InitLoading());
    }

    private IEnumerator InitLoading()
    {
        yield return null;

        if (Application.isEditor || UtilMethod.isSkipUpdate || UtilMethod.isTestServer || UtilMethod.isBetaCDN)
        {
           // GameObject console = new GameObject("DebugConsole");
            //console.AddComponent<DebugConsole>();
           // Loger.PrintLog("OpenConsole");
        }

        yield return null;

        gameObject.AddComponent<AudioListener>();
        gameObject.AddComponent<HttpPostManager>();
        gameObject.AddComponent<AliyunOSSManager>();
#if UNITY_EDITOR
#elif UNITY_ANDROID
        gameObject.AddComponent<AndroidSDK>();
#elif UNITY_IPHONE || UNITY_IOS
        gameObject.AddComponent<IOSSDK>();
#endif

        //状态栏
        PlatformSDK.ShowStatusBar(true);

#if UNITY_EDITOR
        //gameObject.AddComponent<ShowFPS>();
#endif

        RecordManager.Init();

        yield return null;

        Init();
    }

    private void Init()
    {
        ResListManager.FixResList();

        WWWLoader.SetRetryFunc((url) =>
        {
            LoadingBarController.ShowNotice2("网络连接失败，请检查网络后重新连接", "重新连接",
                () => WWWManager.Instance.retryLoader(url), "退出",
                () => Driver.Instance.QuitGame());
        });

        if (!UtilMethod.IsUnityEditor() && !UtilMethod.isSkipUpdate)
        {
            StartCoroutine(GameVersionManager.LoadConfig(
                (isSuccess) =>
                {
                    if (isSuccess)
                        GameVersionManager.LoadServerConfig(ResUpdate);
                    else
                    {
                        LoadingBarController.ShowNotice("发生错误，请尝试重新启动", "退出",
                            () => Driver.Instance.QuitGame());
                    }
                }));
        }
        else
            InitGame();
    }

    private void ResUpdate()
    {
        LoadingBarController.SetLoadContent("正在检查版本更新");
        LoadingBarController.SetProgress(0.5f, 60);
        LoadingBarController.isAutoClose = false;
        //资源热更新
        ResUpdateManager.Instance.StartUpdateRes(
            (progress, str) =>
            {
                //LoadingBarController.SetLoadContent(string.Format("正在更新资源 {0}", str));
                //LoadingBarController.SetProgress(progress, 0);
                LoadingBarController.SetLoadContent(CommonUtils.ConnectStrs("正在加载资源：", ((int)(progress*100)).ToString(), "%"));
                LoadingBarController.ShowProgressWindow();
            },
            () =>
            {
                LoadingBarController.HideProgressWindow();
                InitGame();
            },
            () =>
            {
                LoadingBarController.ShowNotice("发生错误，请尝试重新启动", "退出",
                    () => Driver.Instance.QuitGame());
            },
            () =>
            {
#if UNITY_ANDROID
                ApkUpdate.Instance.StartUpdate();
#elif UNITY_IPHONE || UNITY_IOS
                if (UtilMethod.isTestServer)
                {
                    LoadingBarController.ShowNotice("有新版本需要更新", "退出",
                        () => Driver.Instance.QuitGame());
                }
                else
                {
                    if (UtilMethod.channel > 0)
                    {
                        LoadingBarController.ShowNotice("有新版本需要更新", "点击前往",
                            () => Application.OpenURL("https://www.pgyer.com/yoyo_ios"));
                    }
                    else
                    {
                        LoadingBarController.ShowNotice("发现新版本，请前往AppStore进行更新", "确定",
                            () => Driver.Instance.QuitGame());
                    }
                }
#endif
            });
    }

    private void InitGame()
    {
        LoadingBarController.SetLoadContent("正在初始化");
        LoadingBarController.SetProgress(0.7f, 30);
        
        DestroyImmediate(GameObject.Find("EventSystem"));
        gameObject.AddComponent<MainThread>();

        Logger.PrintColor("blue", "QualitySettings.GetQualityLevel="+QualitySettings.GetQualityLevel());
    }

    public void QuitGame()
    {
        Debug.LogWarning("退出");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    //失去焦点的时间
    private static float lostFocusTime = 0f;
    //焦点事件
    public static event System.Action<bool> OnApplicationFocusEvt;
    private void OnApplicationFocus(bool focus)
    {
        Logger.PrintLog("OnApplicationFocus:" + focus);
        if (OnApplicationFocusEvt != null)
            OnApplicationFocusEvt(focus);

        if (!CommonUtils.isUnityEditor)
        {
            float curTime = Time.realtimeSinceStartup;
            if (!focus)
                lostFocusTime = curTime;
            else
            {
                if (lostFocusTime > 0f && curTime - lostFocusTime > 60f)
                    NoticeManager.Instance.Dispatch(NoticeType.Focus_TimeOut);
            }
        }
    }

    //暂停事件
    public static event System.Action<bool> OnApplicationPauseEvt;
    private void OnApplicationPause(bool pause)
    {
        Logger.PrintLog("OnApplicationPause:" + pause);
        if (OnApplicationPauseEvt != null)
            OnApplicationPauseEvt(pause);
    }
}