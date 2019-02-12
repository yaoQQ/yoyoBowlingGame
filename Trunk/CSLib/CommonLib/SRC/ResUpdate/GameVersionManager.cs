using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class ResItem
{
    public string packageName;
    /// <summary>相对路径</summary>
    public string relativePath;
    public string md5;
    /// <summary>AssetBundle文件大小</summary>
    public long size;
    /// <summary>是否已更新</summary>
    public bool isUpdate = false;
    /// <summary>资源版本号</summary>
    public int versionCode = 0;
    /// <summary>是否是扩展包资源（暂时不用，预留）</summary>
    public bool isEx = false;

    public ResItem Clone()
    {
        ResItem resItem = new ResItem();
        resItem.packageName = packageName;
        resItem.relativePath = relativePath;
        resItem.md5 = md5;
        resItem.size = size;
        resItem.isUpdate = isUpdate;
        resItem.versionCode = versionCode;
        resItem.isEx = isEx;
        return resItem;
    }

    private string m_fullName = null;
    public string fullName
    {
        get
        {
            if (m_fullName == null)
                m_fullName = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/", packageName, "/", CommonPathUtils.RES_DIR_NAME, "/", relativePath);
            return m_fullName;
        }
    }
}

public class GameVersionManager
{
    public static GameVersion localVersion = new GameVersion();
    public static GameVersion updateVersion = new GameVersion();
    public static GameVersion serverVersion = new GameVersion();

    /// <summary>游戏使用的资源清单字典</summary>
    public static Dictionary<string, ResItem> gameResDict = new Dictionary<string, ResItem>();

    public static bool isAndroidSmallApk = false;

    public static void SetUpdateVersionFromServer()
    {
        updateVersion.version = serverVersion.version;
        updateVersion.versionCode = serverVersion.versionCode;
        //LoadingBarController.SetVersions(updateVersion.version);
    }

    private static void SetGameVersion(string str, int type)
    {
        Logger.PrintLog("Version(" + type + "):" + str);
        GameVersion version = null;
        if (type == 0)
            version = localVersion;
        else if (type == 1)
        {
            version = updateVersion;
            //LoadingBarController.SetVersions(str);
        }
        else if (type == 2)
            version = serverVersion;
        version.version = str;

        if (str.Contains("."))
            version.versionCode = int.Parse(str.Substring(str.LastIndexOf(".") + 1));
    }

    /// <summary>
    /// 读取本地版本号
    /// </summary>
    public static IEnumerator LoadConfig(Action<bool> callback)
    {
        //读取包内版本号
        string versionFilePathLocal = CommonUtils.ConnectStrs(CommonPathUtils.STREAMING_ASSETS_ROOT_PATH, "/base/", CommonPathUtils.VERSION_FILE_NAME);
        if (!Application.isEditor && Application.platform == RuntimePlatform.IPhonePlayer)
            versionFilePathLocal = CommonUtils.ConnectStrs("file://", versionFilePathLocal);
        Logger.PrintLog("versionFilePathLocal:" + versionFilePathLocal);
        WWW wwwLocal = new WWW(versionFilePathLocal);
        yield return wwwLocal;
        if (!string.IsNullOrEmpty(wwwLocal.error))
        {
            Logger.PrintError("加载包内版本号失败：" + wwwLocal.error);
            if (callback != null)
                callback(false);
            yield break;
        }
        SetGameVersion(wwwLocal.text, 0);

        //若更新版本号不存在，则使用包内版本号
        string versionFilePathUpdate = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/base/", CommonPathUtils.VERSION_FILE_NAME);
        Logger.PrintLog("versionFilePathUpdate:" + versionFilePathUpdate);
        if (!File.Exists(versionFilePathUpdate))
        {
            SetGameVersion(wwwLocal.text, 1);
            if (callback != null)
                callback(true);
            yield break;
        }
        
        //读取更新版本号
        WWW wwwUpdate = new WWW("file://" + versionFilePathUpdate);
        yield return wwwUpdate;
        if (!string.IsNullOrEmpty(wwwUpdate.error))
        {
            Logger.PrintError("加载更新版本号失败：" + wwwUpdate.error);
            SetGameVersion(wwwLocal.text, 1);
            if (callback != null)
                callback(true);
            yield break;
        }
        SetGameVersion(wwwUpdate.text, 1);
        if (updateVersion.versionCode < localVersion.versionCode)
            SetGameVersion(wwwLocal.text, 1);
        if (callback != null)
            callback(true);
    }

    /// <summary>
    /// 读取服务器版本号
    /// </summary>
    public static void LoadServerConfig(Action callback)
    {
        TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
        string str = Convert.ToInt64(span.TotalSeconds).ToString();
        //CommonPathUtils.SERVER_ROOT_PATH="http://yoyores.1plustore.com/"
        string versionFilePathServer = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/base/", CommonPathUtils.VERSION_FILE_NAME, "?", str);
        Logger.PrintLog("versionFilePathServer:" + versionFilePathServer);
        WWWManager.Instance.load(versionFilePathServer,
            (www) =>
            {
                SetGameVersion(www.text, 2);
                if(callback != null)
                    callback();
            }, null, null, null, true, 0);
    }

    public static void LoadResList(string packageName, string text, Dictionary<string, ResItem> resDict)
    {
        resDict.Clear();
        string[] lines = text.Split('\n');
        int len = lines.Length;
        for (int i = 1; i < len; ++i)
        {
            string[] strs = lines[i].Replace("\r", "").Split('\t');
            if (strs.Length < 6)
                break;
            ResItem resItem = new ResItem();
            resItem.packageName = packageName;
            resItem.relativePath = strs[0];
            resItem.md5 = strs[1];
            resItem.size = long.Parse(strs[2]);
            resItem.isUpdate = strs[3] == "1";
            resItem.versionCode = int.Parse(strs[4]);
            resItem.isEx = strs[5] == "1";
            resDict[resItem.relativePath] = resItem;
        }
    }

    public static ResItem GetResItem(string relativePath)
    {
        if (gameResDict.ContainsKey(relativePath))
            return gameResDict[relativePath];
        return null;
    }
}

public class GameVersion
{
    public string version = "";
    public int versionCode = 0;
}


public class SubGameVersionManager
{
    private bool m_isDownload = false;

    private string m_packageName = "";
    public string packageName
    {
        get { return m_packageName; }
    }
    private string m_persistentResRootPath = "";
    private string m_serverResRootPath = "";
    private long m_resLength = 0;
    private bool m_isDownloadZip = true;
    private Action<float> m_progressCallback = null;
    private bool m_isCancelDownload = false;
    public bool isCancelDownload
    {
        get { return m_isCancelDownload; }
    }

    public GameVersion updateVersion = new GameVersion();
    public GameVersion serverVersion = new GameVersion();

    /// <summary>游戏使用的资源清单字典</summary>
    public Dictionary<string, ResItem> gameResDict = new Dictionary<string, ResItem>();

    public SubGameVersionManager(string packageName)
    {
        m_packageName = packageName;
        m_persistentResRootPath = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/", m_packageName);
        m_serverResRootPath = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", m_packageName);
    }
    
    public void CheckDownload(Action<uint> checkCallback)
    {
        if (m_isDownload)
        {
            checkCallback(0);
            return;
        }

        Logger.PrintLog("ResZip更新检测");
        CommonUtils.StartCoroutine(LoadConfig(
            (isSuccess) =>
            {
                if (isSuccess)
                {
                    //走热更流程
                    m_isDownloadZip = false;
                    LoadServerConfig(() =>
                    {
                        GameResUpdateManager.Instance.CheckUpdateRes(this,
                        (size) =>
                        {
                            if (size == 0)
                                m_isDownload = true;
                            checkCallback(size);
                        },
                        () =>
                        {
                            /*LoadingBarController.ShowNotice("游戏发生错误，请尝试重新启动游戏", "退出游戏",
                                () => Driver.Instance.QuitGame());*/
                        });
                    });
                }
                else
                {
                    m_isDownloadZip = true;
                    LoadServerConfig(() =>
                    {
                        //更新ResZip
                        string requestUriString = CommonUtils.ConnectStrs(m_serverResRootPath, "/", serverVersion.versionCode.ToString(), "/Res.zip");
                        //string str3 = SystemConfig.GetVerFilePath(true, true) + "Res.zip";
                        Logger.PrintLog("获取ResZip文件大小：" + requestUriString);
                        HttpWebRequest request = null;
                        try
                        {
                            request = (HttpWebRequest)WebRequest.Create(requestUriString);
                            m_resLength = request.GetResponse().ContentLength;
                            Logger.PrintLog("resLength:" + m_resLength);
                        }
                        catch (Exception exception)
                        {
                            Logger.PrintLog("error1:" + exception.Message);
                            if (request != null)
                            {
                                request.Abort();
                            }
                            //MogoForwardLoadingUIManager.Instance.OpenTipsView(1, LocalizedMsgManager.Instance.GetLocalizedMsg("MESS_ID03"), LocalizedMsgManager.Instance.GetLocalizedMsg("MESS_ID22"), () => this.Check(), string.Empty, null, true);
                            return;
                        }
                        if (request != null)
                        {
                            request.Abort();
                        }
                        
                        checkCallback((uint)m_resLength);
                    });
                }
            }));
    }

    public void StartDownload(Action<float> progressCallback = null)
    {
        if (progressCallback != null)
            m_progressCallback = progressCallback;
        m_isCancelDownload = false;
        if (m_isDownloadZip)
        {
            ThreadManager.RunThread(DownloadResZipThread);
        }
        else
        {
            GameResUpdateManager.Instance.StartUpdateRes(
                (progress, str) =>
                {
                    m_progressCallback(0.99f * progress);
                },
                () =>
                {
                    m_progressCallback(1f);
                },
                () =>
                {
                    /*LoadingBarController.ShowNotice("游戏发生错误，请尝试重新启动游戏", "退出游戏",
                        () => Driver.Instance.QuitGame());*/
                });
        }
    }

    public void CancelDownload()
    {
        m_isCancelDownload = true;
    }

    public void SetUpdateVersionFromServer()
    {
        updateVersion.version = serverVersion.version;
        updateVersion.versionCode = serverVersion.versionCode;
        //LoadingBarController.SetVersions(updateVersion.version);
    }

    private void SetGameVersion(string str, int type)
    {
        Logger.PrintLog("Version(" + type + "):" + str);
        GameVersion version = null;
        if (type == 1)
            version = updateVersion;
        else if (type == 2)
            version = serverVersion;
        version.version = str;

        if (str.Contains("."))
            version.versionCode = int.Parse(str.Substring(str.LastIndexOf(".") + 1));
        else
            version.versionCode = int.Parse(str);
    }

    /// <summary>
    /// 读取本地版本号
    /// </summary>
    private IEnumerator LoadConfig(Action<bool> callback)
    {
        //读取更新版本号
        //若更新版本号不存在，则更新zip包
        string versionFilePathUpdate = CommonUtils.ConnectStrs(m_persistentResRootPath, "/", CommonPathUtils.VERSION_FILE_NAME);
        Logger.PrintLog("versionFilePathUpdate:" + versionFilePathUpdate);
        if (!File.Exists(versionFilePathUpdate))
        {
            if (callback != null)
                callback(false);
            yield break;
        }

        //读取更新版本号
        WWW wwwUpdate = new WWW("file://" + versionFilePathUpdate);
        yield return wwwUpdate;
        if (!string.IsNullOrEmpty(wwwUpdate.error))
        {
            Logger.PrintError("加载更新版本号失败：" + wwwUpdate.error);
            if (callback != null)
                callback(false);
            yield break;
        }
        SetGameVersion(wwwUpdate.text, 1);
        if (callback != null)
            callback(true);
    }

    /// <summary>
    /// 读取服务器版本号
    /// </summary>
    public void LoadServerConfig(Action callback)
    {
        TimeSpan span = (TimeSpan)(DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
        string str = Convert.ToInt64(span.TotalSeconds).ToString();
        string versionFilePathServer = CommonUtils.ConnectStrs(m_serverResRootPath, "/", CommonPathUtils.VERSION_FILE_NAME, "?", str);
        Logger.PrintLog("versionFilePathServer:" + versionFilePathServer);
        WWWManager.Instance.load(versionFilePathServer,
            (www) =>
            {
                SetGameVersion(www.text, 2);
                if (callback != null)
                    callback();
            }, null, null, null, true, 0);
    }

    private void DownloadResZipThread()
    {
        FileStream stream;
        string requestUriString = CommonUtils.ConnectStrs(m_serverResRootPath, "/", serverVersion.versionCode.ToString(), "/Res.zip");
        string path = m_persistentResRootPath + "Res.zip";
        if (m_resLength == 0)
        {
            Logger.PrintLog("获取ResZip文件大小：" + requestUriString);
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(requestUriString);
                m_resLength = request.GetResponse().ContentLength;
                Logger.PrintLog("resLength:" + m_resLength);
            }
            catch (Exception exception)
            {
                Logger.PrintLog("error1:" + exception.Message);
                if (request != null)
                {
                    request.Abort();
                }
                this.showErrorTips();
                return;
            }
            if (request != null)
            {
                request.Abort();
            }
        }
        Logger.PrintLog("开始下载ResZip：" + requestUriString);
        Logger.PrintLog("保存文件路径：" + path);
        long offset = 0L;
        if (File.Exists(path))
        {
            Logger.PrintLog("读取已下载的ResZip");
            stream = File.OpenWrite(path);
            offset = stream.Length;
            if ((m_resLength - offset) <= 0L)
            {
                stream.Close();
                Logger.PrintLog("下载完成");
                this.UnzipResZip();
                return;
            }
            Logger.PrintLog("继续下载:" + offset);
            stream.Seek(offset, SeekOrigin.Current);
        }
        else
        {
            Logger.PrintLog("创建保存文件：" + path);
            IOUtil.CreateDirectory(m_persistentResRootPath);
            stream = new FileStream(path, FileMode.Create);
        }
        HttpWebRequest request2 = null;
        HttpWebResponse response = null;
        Stream responseStream = null;
        try
        {
            Logger.PrintLog("打开网络连接：" + requestUriString);
            request2 = (HttpWebRequest)WebRequest.Create(requestUriString);
            if (offset > 0L)
            {
                request2.AddRange((int)offset);
            }
            Logger.PrintLog("获取服务器回应");
            response = (HttpWebResponse)request2.GetResponse();
            Logger.PrintLog("获取数据流");
            responseStream = response.GetResponseStream();
        }
        catch (Exception exception2)
        {
            Logger.PrintLog("error2:" + exception2.Message);
            if (responseStream != null)
            {
                responseStream.Close();
            }
            stream.Close();
            if (request2 != null)
            {
                request2.Abort();
            }
            this.showErrorTips();
            return;
        }
        
        int count = 0x2000;
        byte[] buffer = new byte[count];
        int len = responseStream.Read(buffer, 0, count);
        while (len > 0)
        {
            if (m_isCancelDownload)
            {
                responseStream.Close();
                stream.Close();
                request2.Abort();
                return;
            }
            try
            {
                stream.Write(buffer, 0, len);
                len = responseStream.Read(buffer, 0, count);
                float curLength = (((float)stream.Length) / 1024f) / 1024f;
                float totalLength = (((float)m_resLength) / 1024f) / 1024f;
                ThreadManager.RunMainThread(() =>
                {
                    //这里走到99%，解压完再到100%
                    m_progressCallback(0.99f * curLength / totalLength);
                    //MogoForwardLoadingUIManager.Instance.setProgress((100f * curLength) / totalLength, 0);
                    //string[] textArray1 = new string[] { LocalizedMsgManager.Instance.GetLocalizedMsg("MESS_ID28"), curLength.ToString("0.00"), "M / ", totalLength.ToString("0.00"), "M" };
                    //MogoForwardLoadingUIManager.Instance.SetLoadingStatusTip(string.Concat(textArray1));
                });
                continue;
            }
            catch (Exception exception3)
            {
                Logger.PrintLog("error3:" + exception3.Message);
                responseStream.Close();
                stream.Close();
                request2.Abort();
                this.showErrorTips();
                return;
            }
        }
        long length = stream.Length;
        Logger.PrintLog("fs.Length:" + length);
        if (length < m_resLength)
        {
            responseStream.Close();
            stream.Close();
            request2.Abort();
            this.showErrorTips();
        }
        else
        {
            responseStream.Close();
            stream.Close();
            request2.Abort();
            this.UnzipResZip();
        }
    }

    private void UnzipResZip()
    {
        string resZipFilePath = m_persistentResRootPath + "Res.zip";
        try
        {
            CompressUtil.UnzipFile(resZipFilePath, m_persistentResRootPath);
        }
        catch (Exception exception)
        {
            Logger.PrintLog("解压ResZip错误：" + exception.Message);
            IOUtil.DeleteFile(resZipFilePath);
            return;
        }
        Logger.PrintLog("解压ResZip完成");
        IOUtil.DeleteFile(resZipFilePath);
        ThreadManager.RunMainThread(() =>
        {
            m_progressCallback(1f);
        });
    }

    private void showErrorTips()
    {
        //重试
        ThreadManager.RunMainThread(() =>
        {
            StartDownload();
        });
    }

    public void LoadResList(string packageName, string text, Dictionary<string, ResItem> resDict)
    {
        resDict.Clear();
        string[] lines = text.Split('\n');
        int len = lines.Length;
        for (int i = 1; i < len; ++i)
        {
            string[] strs = lines[i].Replace("\r", "").Split('\t');
            if (strs.Length < 6)
                break;
            ResItem resItem = new ResItem();
            resItem.packageName = packageName;
            resItem.relativePath = strs[0];
            resItem.md5 = strs[1];
            resItem.size = long.Parse(strs[2]);
            resItem.isUpdate = strs[3] == "1";
            resItem.versionCode = int.Parse(strs[4]);
            resItem.isEx = strs[5] == "1";
            resDict[resItem.relativePath] = resItem;
        }
    }

    public ResItem GetResItem(string relativePath)
    {
        if (gameResDict.ContainsKey(relativePath))
            return gameResDict[relativePath];
        return null;
    }
}