using System;
using UnityEngine;

public class ResLoadManager
{
    /// <summary>
    /// 是否打印加载log
    /// </summary>
    private static bool m_isPrintLoadLog = false;
    public static bool isPrintLoadLog
    {
        set { m_isPrintLoadLog = value; }
    }

    public static void PrintLoadLog(string msg)
    {
        if (m_isPrintLoadLog)
            Logger.PrintLog(msg);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetType"></param>
    /// <param name="relativePath"></param>
    /// <param name="callback"></param>
    public static void LoadAsync(AssetType assetType, string packageName, string abRelativePath, Action<string, object> callback)
    {
        ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("请求异步加载：", packageName, ":", abRelativePath));

        if (string.IsNullOrEmpty(abRelativePath))
        {
            Logger.PrintError("不能输入空的字符串(ResLoadManager.LoadAsync)");
            return;
        }
        if (callback == null)
        {
            Logger.PrintError("callback为空(ResLoadManager.LoadAsync)");
            return;
        }

        AssetNodeManager.LoadNodeAsync(assetType, packageName, abRelativePath, (assetNode) =>
        {
            if (assetNode == null)
                callback(abRelativePath, null);
            if (assetNode is AssetBundleAssetNode)
            {
                callback(abRelativePath, assetNode.GetAsset());
            }
            else if (assetNode is TextAssetNode)
            {
                callback(abRelativePath, assetNode.GetAsset());
            }
        });
    }

    public static void SetManifest(AssetBundleManifest abManifest, AssetType assetType)
    {
        ABManifestManager.SetManifest(abManifest, assetType);
    }

    public static void SetManifest(AssetBundleManifest abManifest, AssetType assetType, string packageName)
    {
        ABManifestManager.SetManifest(abManifest, assetType, packageName);
    }
}