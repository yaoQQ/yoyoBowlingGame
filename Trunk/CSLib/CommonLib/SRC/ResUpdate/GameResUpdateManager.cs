using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 资源更新管理
/// </summary>
public class GameResUpdateManager : MonoBehaviour
{
    private SubGameVersionManager m_subGameVersionManager;
    private Action<uint> m_checkCallback;
    private Action<float, string> m_progressCallback;
    private Action m_finishCallback;
    private Action m_errorCallback;

    /// <summary>服务器上的资源清单字典</summary>
    private Dictionary<string, ResItem> m_serverResDict = new Dictionary<string, ResItem>();
    /// <summary>游戏使用的资源清单字典</summary>
    private Dictionary<string, ResItem> m_gameResDict;

    /// <summary>需要下载的资源列表</summary>
    private List<ResItem> m_downloadResList = new List<ResItem>();
    /// <summary>需要下载的资源总大小</summary>
    private long m_allDownloadSize;
    /// <summary>当前下载的资源大小</summary>
    private long m_curDownloadSize;
    /// <summary>已下载完成的资源总大小</summary>
    private long m_downloadCompleteSize;
    /// <summary>已下载的资源数量</summary>
    private int m_downloadCount;
    /// <summary>已下载完成但未保存资源清单的资源总大小</summary>
    private long m_noSaveSize;

    private static GameResUpdateManager m_instance = null;
    public static GameResUpdateManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new GameObject("GameResUpdateManager").AddComponent<GameResUpdateManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }

    /// <summary>检查资源更新</summary>
    public void CheckUpdateRes(SubGameVersionManager subGameVersionManager, Action<uint> checkCallback, Action errorCallback)
    {
        m_subGameVersionManager = subGameVersionManager;
        m_gameResDict = m_subGameVersionManager.gameResDict;

        m_checkCallback = checkCallback;
        m_errorCallback = errorCallback;
        LoadServerResListFile();
    }

    /// <summary>加载CDN服务器上的资源清单文件</summary>
    private void LoadServerResListFile()
    {
        string resListUrl;
        if (m_subGameVersionManager.updateVersion.versionCode <= m_subGameVersionManager.serverVersion.versionCode)
            resListUrl = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", m_subGameVersionManager.packageName, "/", m_subGameVersionManager.serverVersion.versionCode.ToString(), "/", CommonPathUtils.RES_LIST_FILE_NAME);
        else
            resListUrl = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", m_subGameVersionManager.packageName, "/", m_subGameVersionManager.updateVersion.versionCode.ToString(), "/", CommonPathUtils.RES_LIST_FILE_NAME);
        Logger.PrintLog("加载CDN服务器上的资源清单文件:" + resListUrl);
        WWWManager.Instance.load(resListUrl,
            (www) =>
            {
                if (www == null || www.error != null || www.bytes == null)
                {
                    if (www != null && www.error != null)
                        Logger.PrintError(www.error);
                    if (m_errorCallback != null)
                        m_errorCallback();
                }
                else
                {
                    //将资源清单信息读取到字典
                    m_subGameVersionManager.LoadResList(m_subGameVersionManager.packageName, www.text, m_serverResDict);
                    StartCoroutine(LoadGameResListFile());
                }
            },
            (error) =>
            {
                Logger.PrintError(error);
                if (m_errorCallback != null)
                    m_errorCallback();
            },
            () =>
            {
                if (m_errorCallback != null)
                    m_errorCallback();
            }, null, true, 0);
    }

    /// <summary>加载游戏中的资源清单文件</summary>
    public IEnumerator LoadGameResListFile()
    {
        //加载更新持久化目录的资源清单文件
        string updateResListUrl = "file://" + CommonPathUtils.PERSISTENT_DATA_ROOT_PATH + "/" + m_subGameVersionManager.packageName + "/" + CommonPathUtils.RES_LIST_FILE_NAME;
        string tempPath = updateResListUrl.Replace("file://", string.Empty);
        if (!File.Exists(tempPath))
        {
            ThreadManager.RunThread(CheckUpdateRes);
            yield break;
        }
        Logger.PrintLog("加载更新持久化目录的资源清单文件:" + updateResListUrl);
        WWW www = new WWW(updateResListUrl);
        yield return www;
        if (www != null && www.error == null && www.bytes != null)
        {
            m_subGameVersionManager.LoadResList(m_subGameVersionManager.packageName, www.text, m_gameResDict);
            ThreadManager.RunThread(CheckUpdateRes);
        }
        else
        {
            if (www != null && www.error != null)
                Logger.PrintError(www.error);
            if (m_errorCallback != null)
                m_errorCallback();
        }
    }

    /// <summary>检查需要更新的资源</summary>
    private void CheckUpdateRes()
    {
        Logger.PrintLog("检查需要更新的资源");
        m_downloadResList.Clear();
        m_allDownloadSize = 0;
        foreach (KeyValuePair<string, ResItem> pair in m_serverResDict)
        {
            ResItem item = pair.Value;
            if (m_gameResDict.ContainsKey(pair.Key))
            {
                ResItem item2 = m_gameResDict[pair.Key];
                if (item.isEx)
                {
                    if (!item2.isUpdate)
                    {
                        item2.relativePath = item.relativePath;
                        item2.md5 = item.md5;
                        item2.size = item.size;
                        item2.isEx = item.isEx;
                    }
                    else
                    {
                        if (File.Exists(item2.fullName))
                        {
                            if (item2.md5 == item.md5)
                            {
                                item2.isEx = item.isEx;
                                continue;
                            }
                            IOUtil.DeleteFile(item2.fullName);
                        }
                        item2.isUpdate = false;
                        item2.relativePath = item.relativePath;
                        item2.md5 = item.md5;
                        item2.size = item.size;
                        item2.isEx = item.isEx;
                    }
                }
                else if (item2.md5 != item.md5)
                {
                    if (item2.isUpdate)
                    {
                        IOUtil.DeleteFile(item2.fullName);
                        item2.isUpdate = false;
                    }
                    //需要更新(MD5值不同)
                    m_downloadResList.Add(item);
                    m_allDownloadSize += item.size;
                    Logger.PrintLog("文件需要更新(MD5值不同)：" + item.fullName);
                    Logger.PrintLog("旧MD5:" + item2.md5 + " 新MD5:" + item.md5);
                }
                else if (item2.isEx && !item2.isUpdate)
                {
                    //需要更新(原来是扩展包资源现在不是扩展包资源)
                    m_downloadResList.Add(item);
                    m_allDownloadSize += item.size;
                    Logger.PrintLog("文件需要更新(原来是扩展包资源现在不是扩展包资源)：" + item.fullName);
                }
                else if (item2.isUpdate && !File.Exists(item2.fullName))
                {
                    //需要更新(资源丢失)
                    m_downloadResList.Add(item);
                    m_allDownloadSize += item.size;
                    Logger.PrintLog("文件丢失：" + item.fullName);
                }
            }
            else if (!item.isEx)
            {
                //需要更新(新添加的资源)
                m_downloadResList.Add(item);
                m_allDownloadSize += item.size;
                Logger.PrintLog("文件需要更新(CDN新添加的资源)：" + item.fullName);
            }
            else
                m_gameResDict.Add(pair.Key, item.Clone());
        }

        ThreadManager.RunMainThread(
            () =>
            {
                if (m_checkCallback != null)
                    m_checkCallback((uint)m_allDownloadSize);
            });
    }

    /// <summary>资源更新开始</summary>
    public void StartUpdateRes(Action<float, string> progressCallback, Action finishCallback, Action errorCallback)
    {
        m_progressCallback = progressCallback;
        m_errorCallback = errorCallback;
        m_finishCallback = finishCallback;

        ThreadManager.RunThread(UpdateRes);
    }

    private void UpdateRes()
    {
        List<ResItem> removeList = new List<ResItem>();
        foreach (ResItem resItem in m_gameResDict.Values)
        {
            if (!m_serverResDict.ContainsKey(resItem.relativePath))
            {
                removeList.Add(resItem);
                Logger.PrintLog("文件需要删除：" + resItem.fullName);
            }
        }
        Logger.PrintLog("removeList.Count:" + removeList.Count);
        foreach (ResItem resItem in removeList)
        {
            if (resItem.isUpdate)
            {
                IOUtil.DeleteFile(resItem.fullName);
            }
            m_gameResDict.Remove(resItem.relativePath);
        }

        ThreadManager.RunMainThread(
            () =>
            {
                //保存资源清单
                ResListManager.SaveResList(m_subGameVersionManager.packageName, m_gameResDict);

                m_downloadCount = 0;
                DownloadNextRes();
            });
    }

    /// <summary>下载下一个需要更新的资源</summary>
    private void DownloadNextRes()
    {
        if (m_downloadCount < m_downloadResList.Count)
        {
            if (m_subGameVersionManager.isCancelDownload)
            {
                //保存资源清单
                ResListManager.SaveResList(m_subGameVersionManager.packageName, m_gameResDict);
                return;
            }
            m_curDownloadSize = m_downloadResList[m_downloadCount].size;
            string path;
            if (m_subGameVersionManager.serverVersion.versionCode < m_subGameVersionManager.updateVersion.versionCode)
                path = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", m_subGameVersionManager.packageName, "/", m_subGameVersionManager.updateVersion.versionCode.ToString(), "/", CommonPathUtils.RES_DIR_NAME, "/", m_downloadResList[m_downloadCount].relativePath);
            else
                path = CommonUtils.ConnectStrs(CommonPathUtils.SERVER_ROOT_PATH, "/", m_subGameVersionManager.packageName, "/", m_downloadResList[m_downloadCount].versionCode.ToString(), "/", CommonPathUtils.RES_DIR_NAME, "/", m_downloadResList[m_downloadCount].relativePath);
            DownLoadRes(path);
        }
        else
        {
            //资源全部下载完成
            try
            {
                //保存资源清单
                ResListManager.SaveResList(m_subGameVersionManager.packageName, m_gameResDict);
            }
            catch (Exception e)
            {
                Logger.PrintError("保存资源清单错误:" + e.Message);
                if (m_errorCallback != null)
                    m_errorCallback();
                return;
            }
            OnResDownloadFinish();
        }
    }

    /// <summary>下载资源</summary>
    public void DownLoadRes(string url)
    {
        WWWManager.Instance.load(url,
            (www) =>
            {
                if (www == null || www.error != null || www.bytes == null)
                    OnResDownloadFail();
                else
                    OnResDownloadSucceed(www, m_downloadCount);
            },
            (error) =>
            {
                OnResDownloadFail();
            },
            () =>
            {
                OnResDownloadFail();
            },
            (progress) =>
            {
                if (m_progressCallback != null)
                {
                    float num = m_downloadCompleteSize + (progress * m_curDownloadSize);
                    string str = "(" + (num / 1048576f).ToString("0.00") + "M / " + ((float)m_allDownloadSize / 1048576f).ToString("0.00") + "M) ";
                    m_progressCallback(num / ((float)m_allDownloadSize), str);
                }
            }, true, 0);
    }

    /// <summary>下载资源成功</summary>
    private void OnResDownloadSucceed(WWW www, int index)
    {
        //将下载的资源写入持久化目录
        string fullName = m_downloadResList[m_downloadCount].fullName;
        Logger.PrintLog("保存资源:" + fullName);
        try
        {
            string dir = IOUtil.GetFileDir(fullName);
            IOUtil.CreateDirectory(dir);
            IOUtil.DeleteFile(fullName);
            FileStream stream = new FileStream(fullName, FileMode.Create);
            stream.Write(www.bytes, 0, www.bytes.Length);
            stream.Close();
            m_gameResDict[m_downloadResList[m_downloadCount].relativePath] = m_downloadResList[m_downloadCount].Clone();
            m_gameResDict[m_downloadResList[m_downloadCount].relativePath].isUpdate = true;
            Logger.PrintLog("热更成功:" + www.url + " MD5:" + m_downloadResList[m_downloadCount].md5);
        }
        catch (Exception e)
        {
            Logger.PrintError("保存资源错误:" + e.Message);
            if (m_errorCallback != null)
                m_errorCallback();
            return;
        }

        m_downloadCompleteSize += m_downloadResList[m_downloadCount].size;
        //每下载超过1M就保存资源清单
        m_noSaveSize += m_downloadResList[m_downloadCount].size;
        if (m_noSaveSize >= 1048576)
        {
            ResListManager.SaveResList(m_subGameVersionManager.packageName, m_gameResDict);
            m_noSaveSize = 0;
        }
        ++m_downloadCount;
        DownloadNextRes();
    }

    /// <summary>下载资源失败</summary>
    private void OnResDownloadFail()
    {
        Logger.PrintLog("热更失败:" + m_downloadResList[m_downloadCount].fullName);
        ResListManager.SaveResList(m_subGameVersionManager.packageName, m_gameResDict);
        if (m_errorCallback != null)
            m_errorCallback();
    }

    /// <summary>下载资源完成</summary>
    private void OnResDownloadFinish()
    {
        try
        {
            string filepath = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/", m_subGameVersionManager.packageName, "/", CommonPathUtils.VERSION_FILE_NAME);
            m_subGameVersionManager.SetUpdateVersionFromServer();
            SaveText(filepath, m_subGameVersionManager.updateVersion.version);
        }
        catch (Exception e)
        {
            Logger.PrintError("保存版本文件错误:" + e.Message);
            if (m_errorCallback != null)
                m_errorCallback();
            return;
        }
        if (m_finishCallback != null)
            m_finishCallback();
    }

    public static void SaveText(string fileName, string text)
    {
        IOUtil.CreateDirectory(IOUtil.GetFileDir(fileName));
        IOUtil.DeleteFile(fileName);
        using (FileStream stream = new FileStream(fileName, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(text);
                writer.Flush();
                writer.Close();
            }
            stream.Close();
        }
    }
}