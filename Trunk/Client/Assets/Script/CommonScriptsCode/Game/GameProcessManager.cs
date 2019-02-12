using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

/// <summary>检查游戏下载函数</summary>
public delegate void CheckGameDownloadFunc(Action<uint> checkCallback);

/// <summary>
/// 游戏流程管理器
/// </summary>
[LuaCallCSharp]
public class GameProcessManager
{
    private static Dictionary<string, SubGameVersionManager> m_dictSubGameVersionManager = new Dictionary<string, SubGameVersionManager>();

    /// <summary>游戏包名字典</summary>
    private static Dictionary<int, string> m_dictPackageName = new Dictionary<int, string>();
    /// <summary>检查下载函数字典</summary>
    private static Dictionary<int, CheckGameDownloadFunc> m_dictCheckDownloadFunc = new Dictionary<int, CheckGameDownloadFunc>();
    /// <summary>开始下载函数字典</summary>
    private static Dictionary<int, Action> m_dictStartDownloadFunc = new Dictionary<int, Action>();
    /// <summary>取消下载函数字典</summary>
    private static Dictionary<int, Action> m_dictCancelDownloadFunc = new Dictionary<int, Action>();
    /// <summary>开始游戏函数字典</summary>
    private static Dictionary<int, Action<object>> m_dictStartGameFunc = new Dictionary<int, Action<object>>();
    /// <summary>销毁游戏函数字典</summary>
    private static Dictionary<int, Action> m_dictDestroyGameFunc = new Dictionary<int, Action>();

    /// <summary>
    /// 注册游戏包名
    /// </summary>
    public static void RegisterPackageName(int gameID, string packageName)
    {
        m_dictPackageName[gameID] = packageName;
    }

    /// <summary>
    /// 注册检查下载游戏包函数
    /// </summary>
    public static void RegisterCheckDownloadFunc(int gameID, CheckGameDownloadFunc checkGameDownloadFunc)
    {
        if (checkGameDownloadFunc == null)
        {
            Logger.PrintError("注册检查下载函数不能为空");
            return;
        }
        m_dictCheckDownloadFunc[gameID] = checkGameDownloadFunc;
    }

    /// <summary>
    /// 注册开始下载游戏包函数
    /// </summary>
    public static void RegisterStartDownloadFunc(int gameID, Action startGameDownloadFunc)
    {
        if (startGameDownloadFunc == null)
        {
            Logger.PrintError("注册开始下载函数不能为空");
            return;
        }
        m_dictStartDownloadFunc[gameID] = startGameDownloadFunc;
    }

    /// <summary>
    /// 注册取消下载游戏包函数
    /// </summary>
    public static void RegisterCancelDownloadFunc(int gameID, Action cancelGameDownloadFunc)
    {
        if (cancelGameDownloadFunc == null)
        {
            Logger.PrintError("注册取消下载函数不能为空");
            return;
        }
        m_dictCancelDownloadFunc[gameID] = cancelGameDownloadFunc;
    }

    /// <summary>
    /// 注册开始游戏函数
    /// </summary>
    public static void RegisterStartGameFunc(int gameID, Action<object> startGameFunc)
    {
        if (startGameFunc == null)
        {
            Logger.PrintError("注册开始游戏函数不能为空");
            return;
        }
        m_dictStartGameFunc[gameID] = startGameFunc;
    }

    /// <summary>
    /// 注册销毁游戏函数
    /// </summary>
    public static void RegisterDestroyGameFunc(int gameID, Action destroyGameFunc)
    {
        if (destroyGameFunc == null)
        {
            Logger.PrintError("注册销毁游戏函数不能为空");
            return;
        }
        m_dictDestroyGameFunc[gameID] = destroyGameFunc;
    }

    /// <summary>
    /// 检查下载
    /// </summary>
    public static void CheckDownload(int gameID, Action<uint> checkCallback)
    {
        if (CommonPathUtils.isLoadEditorRes)
        {
            checkCallback(0);
            return;
        }
            
        Logger.PrintLog(CommonUtils.ConnectStrs("检查下载：", gameID.ToString()));
        if (true)
        {
            //内部游戏
            string packageName = GetPackageName(gameID);
            string m_persistentResRootPath = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/", packageName);
            string m_serverResRootPath = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", packageName);
            Logger.PrintColor("yellow", "m_persistentResRootPath=" + m_persistentResRootPath);
            Logger.PrintColor("yellow", "m_serverResRootPath=" + m_serverResRootPath);


            if (!m_dictSubGameVersionManager.ContainsKey(packageName))
                m_dictSubGameVersionManager.Add(packageName, new SubGameVersionManager(packageName));
            SubGameVersionManager subGameVersionManager = m_dictSubGameVersionManager[packageName];
            subGameVersionManager.CheckDownload(checkCallback);
            string requestUriString = CommonUtils.ConnectStrs(m_serverResRootPath, "/", subGameVersionManager.serverVersion.versionCode.ToString(), "/Res.zip");
            Logger.PrintColor("yellow", "requestUriString=" + requestUriString);
            return;
        }
        
        if (!m_dictCheckDownloadFunc.ContainsKey(gameID))
        {
            Logger.PrintError("检查下载函数未注册");
            if (checkCallback != null)
                checkCallback(0);
            return;
        }
        m_dictCheckDownloadFunc[gameID](checkCallback);
    }

    /// <summary>
    /// 开始下载
    /// </summary>
    public static void StartDownload(int gameID)
    {
        Logger.PrintLog(CommonUtils.ConnectStrs("开始下载：", gameID.ToString()));
        if (true)
        {
            //内部游戏
            string packageName = GetPackageName(gameID);
            if (!m_dictSubGameVersionManager.ContainsKey(packageName))
                m_dictSubGameVersionManager.Add(packageName, new SubGameVersionManager(packageName));
            SubGameVersionManager subGameVersionManager = m_dictSubGameVersionManager[packageName];
            subGameVersionManager.StartDownload((progress) =>
            {
                UpdateDownloadProgress(gameID, progress);
            });
            return;
        }

        if (!m_dictStartDownloadFunc.ContainsKey(gameID))
        {
            Logger.PrintError("开始下载函数未注册");
            return;
        }
        m_dictStartDownloadFunc[gameID]();
    }

    /// <summary>
    /// 取消下载
    /// </summary>
    public static void CancelDownload(int gameID)
    {
        Logger.PrintLog(CommonUtils.ConnectStrs("取消下载：", gameID.ToString()));
        if (true)
        {
            //内部游戏
            string packageName = GetPackageName(gameID);
            if (!m_dictSubGameVersionManager.ContainsKey(packageName))
                m_dictSubGameVersionManager.Add(packageName, new SubGameVersionManager(packageName));
            SubGameVersionManager subGameVersionManager = m_dictSubGameVersionManager[packageName];
            subGameVersionManager.CancelDownload();
            return;
        }

        if (!m_dictCancelDownloadFunc.ContainsKey(gameID))
        {
            Logger.PrintError("取消下载函数未注册");
            return;
        }
        m_dictCancelDownloadFunc[gameID]();
    }

    /// <summary>
    /// 更新下载进度
    /// </summary>
    public static void UpdateDownloadProgress(int gameID, float value)
    {
        NoticeManager.Instance.Dispatch(NoticeType.Game_Update_Progress, value);
        //Logger.PrintLog(CommonUtils.ConnectStrs("游戏下载进度：", value.ToString()));
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public static void StartGame(int gameID, object param)
    {
        Logger.PrintLog(CommonUtils.ConnectStrs("开始游戏：", gameID.ToString()));
        if (!m_dictStartGameFunc.ContainsKey(gameID))
        {
            Logger.PrintError("开始游戏函数未注册");
            return;
        }
        UIViewManager.Instance.SaveStackAndCloseAllView();
        //隐藏状态栏
        if (UtilMethod.IsUnityEditor())
            UIViewManager.Instance.Close(3);
        else
            PlatformSDK.ShowStatusBar(false);

        //关闭UI摄像机
        Camera camera = UIManager.Instance.UICamera;
        if (camera != null)
            camera.gameObject.SetActive(false);

        m_dictStartGameFunc[gameID](param);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public static void ExitGame(int gameID, bool isNeedSwitchScene)
    {
        Logger.PrintLog(CommonUtils.ConnectStrs("退出游戏：", gameID.ToString()));

        //打开UI摄像机
        Camera camera = UIManager.Instance.UICamera;
        if (camera != null)
            camera.gameObject.SetActive(true);

        if (isNeedSwitchScene)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

        NoticeManager.Instance.Dispatch(NoticeType.Game_Exit, gameID);
    }

    /// <summary>
    /// 销毁游戏
    /// </summary>
    public static void DestroyGame(int gameID)
    {
        Logger.PrintLog(CommonUtils.ConnectStrs("销毁游戏：", gameID.ToString()));
        if (!m_dictDestroyGameFunc.ContainsKey(gameID))
        {
            Logger.PrintError("销毁游戏函数未注册");
            return;
        }
        m_dictDestroyGameFunc[gameID]();
    }

    private static string GetPackageName(int gameID)
    {
        if (m_dictPackageName.ContainsKey(gameID))
            return m_dictPackageName[gameID];
        else
        {
            Logger.PrintError("游戏包名未注册");
            return "";
        }
    }

    /// <summary>
    /// 打开竖版商城
    /// </summary>
    /// <param name="shopType">商城类型 1.钻石 2.金币 3.优卡</param>
    public static void OpenShop(int shopType)
    {
        NoticeManager.Instance.Dispatch(NoticeType.Game_Open_Shop, shopType);
    }

    /// <summary>
    /// 打开横版商城
    /// </summary>
    /// <param name="shopType">商城类型 1.钻石 2.金币 3.优卡</param>
    public static void OpenShopLands(int shopType)
    {
        NoticeManager.Instance.Dispatch(NoticeType.Game_Open_Shop_Lands, shopType);
    }
}