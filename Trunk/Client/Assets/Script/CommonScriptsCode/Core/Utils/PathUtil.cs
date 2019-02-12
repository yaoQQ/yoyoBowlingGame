using System;
using System.IO;
using System.Text;
using UnityEngine;

public enum PathType
{
	dataPath,
	persistentDataPath,
}

public class PathUtil
{
    /// <summary>资源目录的名称</summary>
    public const string RES_DIR_NAME = "res";

    /// <summary>标记版本号文件的名称</summary>
    public const string VERSION_FILE_NAME = "VERSION";

    /// <summary>资源清单文件的名称</summary>
    public const string RES_LIST_FILE_NAME = "ResList.txt";

    /// <summary>标记版本号文件的名称</summary>
    public const string APK_UPDATE_FILE_NAME = "ApkUpdate.bytes";

    public static string platformStr = string.Empty;

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
                    m_persistentDataRootPath = UtilMethod.ConnectStrs(Application.dataPath.Replace("/Assets", ""), "/PersistentData");
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
                if (UtilMethod.isBetaCDN)
                    m_serverRootPath = UtilMethod.ConnectStrs("http://106.75.130.202/yoyo_pack_res/beta/", platformStr);
                else
                    m_serverRootPath = UtilMethod.ConnectStrs("http://106.75.130.202/yoyo_pack_res/stable/", platformStr);
            }
            return m_serverRootPath;
        }
    }

    /// <summary>游戏持久化assetbundle资源目录</summary>
    public static string PERSISTENT_DATA_RES_PATH
    {
        get { return UtilMethod.ConnectStrs(PERSISTENT_DATA_ROOT_PATH, "/", RES_DIR_NAME); }
    }


    protected static PathUtil instance;

	public static PathUtil Instance {
		get {
			if (instance == null) {
				instance = new PathUtil ();
			}
			return instance;
		}
	}

	public PathUtil ()
	{
    }

    public static string getLoadRootDir(string relativePath)
    {
        if (UtilMethod.IsUnityEditor())
        {
            return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/res/");
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
            if (UtilMethod.isSkipUpdate)
                return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/res/");
            else
            {
                ResItem resItem = GameVersionManager.GetResItem(relativePath);
                if (resItem == null)
                    return "";
                if (resItem.isUpdate)
                    return UtilMethod.ConnectStrs(PERSISTENT_DATA_ROOT_PATH, "/res/");
                else
                    return UtilMethod.ConnectStrs(STREAMING_ASSETS_ROOT_PATH, "/res/");
            }
        }
    }

#region sn

    public string PathWithENcrypt(string orgPath)
    {
        string fileName = Path.GetFileName(orgPath);
        string fileNameWOExt = Path.GetFileNameWithoutExtension(orgPath);
        string ext = Path.GetExtension(orgPath);
        string newPath = orgPath.Replace(fileName, fileNameWOExt  + ext);
        return newPath;
    }

#endregion
    
#region AUDIO

    public string GetAudioPath(string packName, string soundName)
    {
        StringBuilder sb =new StringBuilder();
        sb.Append("audio/");
        sb.Append(packName);
        sb.Append("/");
        sb.Append(soundName);
        sb.Append(".unity3d");
        return sb.ToString();
    }
#endregion
    
#region Icon资源路径
    public string GetIconResDirPath()
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append(absolutePath);
        sb.Append("icon");
        return sb.ToString();
    }
#endregion


}
