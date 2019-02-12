using System;
using System.Collections;
using System.Collections.Generic;

public class AssetNodeManager
{
    /// <summary>所有资源节点的数据</summary>
    private static Dictionary<string, AssetNode> m_assetNodeDict = new Dictionary<string, AssetNode>();
    /// <summary>所有资源节点的加载回调</summary>
    private static Dictionary<string, List<Action<AssetNode>>> m_callbackListDict = new Dictionary<string, List<Action<AssetNode>>>();
    /// <summary>资源引用计数</summary>
    private static Dictionary<string, int> m_assetRefCounterDict = new Dictionary<string, int>();

    /// <summary>
    /// 获取资源节点
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>资源节点</returns>
    private static AssetNode GetAssetNode(string key)
    {
        AssetNode assetNode = null;
        if (m_assetNodeDict.ContainsKey(key))
            assetNode = m_assetNodeDict[key];
        return assetNode;
    }

    /// <summary>
    /// 创建资源节点
    /// </summary>
    /// <param name="assetType">资源类型</param>
    /// <param name="packageName">包名</param>
    /// <param name="relativePath">相对路径</param>
    /// <returns>资源节点</returns>
    private static AssetNode CreateAssetNode(AssetType assetType, string packageName, string relativePath)
    {
        switch (assetType)
        {
            case AssetType.PB:
            case AssetType.Text:
                return new TextAssetNode(assetType, packageName, relativePath);
            default:
                return new AssetBundleAssetNode(assetType, packageName, relativePath);
        }
    }

    /// <summary>
    /// 获取或创建资源节点
    /// </summary>
    /// <param name="assetType">资源类型</param>
    /// <param name="packageName">包名</param>
    /// <param name="relativePath">相对路径</param>
    /// <param name="parent">父节点</param>
    /// <returns>资源节点</returns>
    public static AssetNode GetOrCreateAssetNode(AssetType assetType, string packageName, string relativePath, AssetBundleAssetNode parent = null)
    {
        string key = CommonUtils.ConnectStrs(packageName, ":", relativePath);
        AssetNode assetNode = GetAssetNode(key);
        if (assetNode == null)
        {
            assetNode = CreateAssetNode(assetType, packageName, relativePath);
            m_assetNodeDict.Add(key, assetNode);
        }
        if (assetNode is AssetBundleAssetNode && parent != null)
            (assetNode as AssetBundleAssetNode).AddParent(parent);
        return assetNode;
    }

    /// <summary>
    /// 异步加载节点资源
    /// </summary>
    /// <param name="assetType">资源类型</param>
    /// <param name="packageName">包名</param>
    /// <param name="relativePath">相对路径</param>
    /// <param name="callback">回调</param>
    public static void LoadNodeAsync(AssetType assetType, string packageName, string relativePath, Action<AssetNode> callback)
    {
        AssetNode assetNode = GetOrCreateAssetNode(assetType, packageName, relativePath);

        //设置回调
        string key = CommonUtils.ConnectStrs(packageName, ":", relativePath);
        if (m_callbackListDict.ContainsKey(key))
            m_callbackListDict[key].Add(callback);
        else
            m_callbackListDict.Add(key, new List<Action<AssetNode>>() { callback });

        if (assetNode.IsAssetLoadSuccess)
        {
            ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("资源已经加载过：", key));
            CommonUtils.StartCoroutine(LoadNodeDelayCallback(assetNode));
        }
        else
        {
            ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("资源未加载过，准备加载：", key));
            AssetNodeAsyncLoader.Download(assetNode);
        }
    }

    /// <summary>
    /// 已经加载过的文件，再进行异步加载的时候，等待一帧再返回
    /// </summary>
    private static IEnumerator LoadNodeDelayCallback(AssetNode assetNode)
    {
        yield return null;
        OnAssetDownloadCallback(assetNode);
    }

    /// <summary>
    /// 异步加载完成的回调
    /// </summary>
    public static void OnAssetDownloadCallback(AssetNode assetNode)
    {
        if (assetNode == null)
            return;

        string key = CommonUtils.ConnectStrs(assetNode.packageName, ":", assetNode.relativePath);
        ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("加载完成回调开始：", key));
        
        if (m_callbackListDict.ContainsKey(key))
        {
            for (int i = 0; i < m_callbackListDict[key].Count; ++i)
            {
                ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("加载完成回调执行：", key));
                m_callbackListDict[key][i](assetNode);
            }
            m_callbackListDict.Remove(key);
        }
    }

    /// <summary>
    /// 释放一个节点
    /// </summary>
    /// <param name="assetType">资源类型</param>
    /// <param name="packageName">包名</param>
    /// <param name="relativePath">相对路径</param>
    public static void ReleaseNode(AssetType assetType, string packageName, string relativePath)
    {
        string key = CommonUtils.ConnectStrs(packageName, ":", relativePath);
        AssetNode assetNode = GetAssetNode(key);
        if (assetNode == null)
            return;

        //m_AssetNodeAsyncLoader.StopDownload(bundleNameID);
        
        m_assetNodeDict.Remove(key);
        assetNode.Release();
    }
}