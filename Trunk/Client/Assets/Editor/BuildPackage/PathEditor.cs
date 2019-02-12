using UnityEngine;

public class PathEditor
{
    /// <summary>游戏发布资源根目录</summary>
    public static string RES_ROOT_PATH_EDITOR
    {
        get { return UtilMethod.ConnectStrs(Application.dataPath.Replace("/Assets", ""), "/ReleaseRes/", PathUtil.platformStr); }
    }

    /// <summary>游戏发布AssetBundle资源目录</summary>
    public static string GetPackagePathEditor(string packageName)
    {
        return UtilMethod.ConnectStrs(RES_ROOT_PATH_EDITOR, "/", packageName);
    }

    /// <summary>游戏发布AssetBundle资源目录</summary>
    public static string GetResPathEditor(string packageName)
    {
        return UtilMethod.ConnectStrs(RES_ROOT_PATH_EDITOR, "/", packageName, "/", PathUtil.RES_DIR_NAME);
    }

    /// <summary>独立游戏发布资源根目录</summary>
    public static string RES_INCLUDE_GAME_ROOT_PATH_EDITOR
    {
        get { return UtilMethod.ConnectStrs(Application.dataPath.Replace("/Assets", ""), "/ReleaseResIncludeGame/", PathUtil.platformStr); }
    }

    /// <summary>游戏发布AssetBundle资源目录</summary>
    public static string GetPackagePathIncludeGameEditor(string packageName)
    {
        return UtilMethod.ConnectStrs(RES_INCLUDE_GAME_ROOT_PATH_EDITOR, "/", packageName);
    }

    /// <summary>游戏发布AssetBundle资源目录</summary>
    public static string GetResPathIncludeGameEditor(string packageName)
    {
        return UtilMethod.ConnectStrs(RES_INCLUDE_GAME_ROOT_PATH_EDITOR, "/", packageName, "/", PathUtil.RES_DIR_NAME);
    }

    /// <summary>游戏打包assetbundle资源目录</summary>
    public static string GetStreamingAssetsResPathEditor(string packageName)
    {
        return UtilMethod.ConnectStrs(Application.streamingAssetsPath, "/", packageName, "/", PathUtil.RES_DIR_NAME);
    }
}