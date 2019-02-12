using UnityEngine;
using System.Collections;
using System.IO;

public class CommonPathUtils
{
    /// <summary>资源目录的名称</summary>
    public const string RES_DIR_NAME = "res";

    /// <summary>标记版本号文件的名称</summary>
    public const string VERSION_FILE_NAME = "VERSION";

    /// <summary>资源清单文件的名称</summary>
    public const string RES_LIST_FILE_NAME = "ResList.txt";

    /// <summary>标记版本号文件的名称</summary>
    public const string APK_UPDATE_FILE_NAME = "ApkUpdate.bytes";

    public static bool isLoadEditorRes = false;

    public static string platformStr = string.Empty;

    private static string m_cdnRootPath = string.Empty;

    public static void InitCdnRootPath(string cdnRootPath)
    {
        m_cdnRootPath = cdnRootPath;
    }

    private static string m_streamingAssetsRootPath = string.Empty;
    /// <summary>游戏包内资源根目录</summary>
    public static string STREAMING_ASSETS_ROOT_PATH
    {
        get
        {
            if (m_streamingAssetsRootPath.Equals(string.Empty))
            {
                if (Application.isEditor)
                    m_streamingAssetsRootPath = Application.dataPath.Replace("/Assets", "");
                else if (Application.platform == RuntimePlatform.Android)
                    m_streamingAssetsRootPath = Application.streamingAssetsPath;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    m_streamingAssetsRootPath = Application.streamingAssetsPath;
                else
                    m_streamingAssetsRootPath = Application.streamingAssetsPath;
            }
            return m_streamingAssetsRootPath;
        }
    }

    private static string m_persistentDataRootPath = string.Empty;
    /// <summary>游戏持久化资源根目录</summary>
    public static string PERSISTENT_DATA_ROOT_PATH
    {
        get
        {
            if (m_persistentDataRootPath.Equals(string.Empty))
            {
                if (Application.platform == RuntimePlatform.Android)
                    m_persistentDataRootPath = Application.persistentDataPath;
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    m_persistentDataRootPath = Application.persistentDataPath;
                else
                    m_persistentDataRootPath = CommonUtils.ConnectStrs(Application.dataPath.Replace("/Assets", ""), "/PersistentData");
            }
            return m_persistentDataRootPath;
        }
    }

    private static string m_serverRootPath = string.Empty;
    /// <summary>服务器资源根目录</summary>
    public static string SERVER_ROOT_PATH
    {
        get
        {
            if (m_serverRootPath.Equals(string.Empty))
            {
                if (CommonUtils.isBetaCDN)
                    m_serverRootPath = CommonUtils.ConnectStrs(m_cdnRootPath, "beta/", platformStr);
                else
                    m_serverRootPath = CommonUtils.ConnectStrs(m_cdnRootPath, "stable/", platformStr);
            }
            return m_serverRootPath;
        }
    }

    public static string getLoadRootDir(string packageName, string relativePath)
    {
        if (CommonUtils.isUnityEditor)
        {
            if (packageName == "base" || isLoadEditorRes)
                return CommonUtils.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/res/");
            else
                return CommonUtils.ConnectStrs(PERSISTENT_DATA_ROOT_PATH, "/", packageName, "/res/");
            /*#if UNITY_ANDROID
                        return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/android_res/");
            #elif UNITY_IOS
                        return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/ios_res/");
            #else
                        return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/res/");
            #endif*/
        }
        else
        {
            if (packageName == "base" || CommonUtils.isIncludeGame)
            {
                if (CommonUtils.isSkipUpdate)
                    return CommonUtils.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/base/res/");
                else
                {
                    ResItem resItem = GameVersionManager.GetResItem(relativePath);
                    if (resItem == null)
                        return "";
                    if (resItem.isUpdate)
                        return CommonUtils.ConnectStrs(PERSISTENT_DATA_ROOT_PATH, "/base/res/");
                    else
                        return CommonUtils.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/base/res/");
                }
            }
            else
                return CommonUtils.ConnectStrs(PERSISTENT_DATA_ROOT_PATH, "/", packageName, "/res/");
        }
    }

    public static string PathWithENcrypt(string orgPath)
    {
        string fileName = Path.GetFileName(orgPath);
        string fileNameWOExt = Path.GetFileNameWithoutExtension(orgPath);
        string ext = Path.GetExtension(orgPath);
        string newPath = orgPath.Replace(fileName, fileNameWOExt + ext);
        return newPath;
    }
}