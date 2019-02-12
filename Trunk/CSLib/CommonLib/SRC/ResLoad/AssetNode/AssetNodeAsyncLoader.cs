using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 异步下载资源
/// </summary>
public class AssetNodeAsyncLoader
{
    /*// 下载数据
    class ABRequest : UniqueKeyData<string>
    {
        /// <summary>资源节点</summary>
        public AssetNode mAssetNode;

        public AssetBundleCreateRequest createRequest = null;

        private AssetBundle m_AssetBundle = null;

        /// <summary>重试次数</summary>
        public int retryCount = 0;

        public string mUniqueKey
        {
            get { return mAssetNode.relativePath; }
        }

        public ABRequest(AssetNode assetNode)
        {
            mAssetNode = assetNode;
        }

        public void LoadFromFileAsync()
        {
            string fullPath = CommonUtils.ConnectStrs(PathUtil.getLoadRootDir(mAssetNode.relativePath), PathUtil.Instance.PathWithENcrypt(mAssetNode.relativePath));

            Logger.PrintDebug("开始异步加载资源：" + fullPath);
            try
            {
                createRequest = AssetBundle.LoadFromFileAsync(fullPath);
            }
            catch (Exception e)
            {
                Logger.PrintError("异步加载资源错误：" + fullPath + "\n" + e.Message);
                return;
            }
        }

        public AssetBundle GetAssetBundle()
        {
            if (m_AssetBundle != null)
                return m_AssetBundle;
            if (createRequest != null)
                m_AssetBundle = createRequest.assetBundle;
            return m_AssetBundle;
        }
    }*/

    /// <summary>同时存在的最大的下载的个数</summary>
    public const int MAX_PROCESSING = 1;

    /// <summary>等待下载的队列</summary>
    private static SimpleList<string, AssetNode> m_WaitingRequest = new SimpleList<string, AssetNode>();
    /// <summary>正在下载的队列</summary>
    private static SimpleList<string, AssetNode> m_ProcessingRequest = new SimpleList<string, AssetNode>();

    /// <summary>当前是否处于逻辑更新的状态</summary>
    private static bool m_IsUpdate;

    /// <summary>
    /// 开始下载一个资源节点
    /// </summary>
    /// <param name="request">下载对象</param>
    public static void Download(AssetNode assetNode)
    {
        if (IsLoadSuccess(assetNode))
        {
            //Logger.PrintError(CommonUtils.ConnectStrs("已经加载过了：", assetNode.relativePath));
            return;
        }
        if (IsDownloadingOrWaiting(assetNode))
        {
            //Logger.PrintDebug(CommonUtils.ConnectStrs("已经在加载队列中了：", assetNode.relativePath));
            return;
        }
        
        if (assetNode is AssetBundleAssetNode)
        {
            AssetBundleAssetNode assetBundleAssetNode = assetNode as AssetBundleAssetNode;
            for (int i = 0; i < assetBundleAssetNode.mChild.Length; i++)
                Download(assetBundleAssetNode.mChild[i]);
        }

        ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("加入等待加载队列：", assetNode.relativePath));
        AddRequestToWaitingList(assetNode);

        StartUpdate();
    }

    /// <summary>
    /// 是在下载或等待状态
    /// </summary>
    /// <param name="request">检查的节点</param>
    /// <returns>节点状态</returns>
    private static bool IsDownloadingOrWaiting(AssetNode assetNode)
    {
        return IsInWaitingList(assetNode) || m_ProcessingRequest.ContainsKey(assetNode.mUniqueKey);
    }

    /// <summary>
    /// 是在等待队列中
    /// </summary>
    /// <param name="request">检查的节点</param>
    private static bool IsInWaitingList(AssetNode assetNode)
    {
        return m_WaitingRequest.ContainsKey(assetNode.mUniqueKey);
    }

    /// <summary>
    /// 是下载完成的状态
    /// </summary>
    /// <param name="request">检查的节点</param>
    /// <returns>节点状态</returns>
    private static bool IsLoadSuccess(AssetNode assetNode)
    {
        return assetNode.IsAssetLoadSuccess;
    }

    /// <summary>
    /// 添加一个资源节点到等待下载的队列
    /// </summary>
    /// <param name="request">资源节点</param>
    private static void AddRequestToWaitingList(AssetNode assetNode)
    {
        if (IsLoadSuccess(assetNode) || IsInWaitingList(assetNode))
            return;
        m_WaitingRequest.Add(assetNode);
    }

    /// <summary>
    /// 开始逻辑更新的帧
    /// </summary>
    private static void StartUpdate()
    {
        if (m_IsUpdate)
            return;
        m_IsUpdate = true;
        CommonUtils.StartCoroutine(Update());
    }

    /// <summary>
    /// 逻辑更新帧
    /// </summary>
    private static IEnumerator Update()
    {
        while (m_IsUpdate)
        {
            m_IsUpdate = false;

            // 检查处理队列的状态
            for (int i = 0; i < m_ProcessingRequest.Count; ++i)
            {
                m_IsUpdate = true;

                AssetNode assetNode = m_ProcessingRequest.GetByIndex(i);
                if (assetNode is AssetBundleAssetNode)
                {
                    AssetBundleAssetNode assetBundleAssetNode = assetNode as AssetBundleAssetNode;
                    AssetBundleCreateRequest createRequest = assetBundleAssetNode.createRequest;

                    if (createRequest.isDone)
                    {
                        AssetBundle ab = createRequest.assetBundle;
                        if (ab == null)
                        {
                            string path = CommonUtils.ConnectStrs(CommonPathUtils.getLoadRootDir(assetBundleAssetNode.packageName, assetBundleAssetNode.relativePath), CommonPathUtils.PathWithENcrypt(assetBundleAssetNode.relativePath));
                            Logger.PrintWarning("异步加载失败：" + path);
                            //重试
                            //++abRequest.retryCount;
                            //if (abRequest.retryCount < 3)
                            //{
                            //    abRequest.LoadFromFileAsync();
                            //}
                            //else
                            {
                                //尝试同步加载
                                ab = AssetBundle.LoadFromFile(path);
                                if (ab == null)
                                {
                                    ResLoadManager.PrintLoadLog("开始重试同步加载资源1：" + path);
                                    ab = AssetBundle.LoadFromFile(path);
                                }
                                if (ab == null)
                                {
                                    ResLoadManager.PrintLoadLog("开始重试同步加载资源2：" + path);
                                    ab = AssetBundle.LoadFromFile(path);
                                }
                            }
                        }

                        m_ProcessingRequest.RemoveAt(i);
                        --i;
                        
                        // 确认该资源是没有被加载过的
                        if (!assetBundleAssetNode.IsAssetBundleLoadSuccess)
                        {
                            // 等待资源加载完成
                            yield return CommonUtils.StartCoroutine(assetBundleAssetNode.OnAssetBundleLoadedAsync(ab));
                        }
                        createRequest = null;

                        //加载完成回调
                        InvokeCallback(assetBundleAssetNode);
                    }
                }
                else if (assetNode is TextAssetNode)
                {
                    TextAssetNode textAssetNode = assetNode as TextAssetNode;
                    WWW www = textAssetNode.www;

                    if (www.isDone)
                    {
                        ResLoadManager.PrintLoadLog(CommonUtils.ConnectStrs("加载完成：", textAssetNode.relativePath));
                        if (www.error != null)
                        {
                            Logger.PrintError("异步加载失败(" + www.url + ")：" + www.error);
                        }

                        m_ProcessingRequest.RemoveAt(i);
                        --i;
                        
                        // 确认该资源是没有被加载过的
                        if (!textAssetNode.IsAssetLoadSuccess)
                        {
                            // 资源加载完成
                            textAssetNode.OnAssetLoadedAsync();
                        }

                        //加载完成回调
                        InvokeCallback(textAssetNode);
                    }
                }
            }

            int index = 0;
            // 添加新的任务
            while (m_ProcessingRequest.Count < MAX_PROCESSING && index < m_WaitingRequest.Count)
            {
                m_IsUpdate = true;
                AssetNode assetNode = m_WaitingRequest.GetByIndex(index);
                if (assetNode is AssetBundleAssetNode)
                {
                    AssetBundleAssetNode assetBundleAssetNode = assetNode as AssetBundleAssetNode;
                    if (IsAssetDependenciesReady(assetBundleAssetNode))
                    {
                        m_WaitingRequest.RemoveAt(index);
                        // 如果没有加载完成
                        if (!assetBundleAssetNode.IsAssetBundleLoadSuccess)
                        {
                            assetBundleAssetNode.LoadFromFileAsync();
                            //curRequest.retryCount = 0;
                            m_ProcessingRequest.Add(assetBundleAssetNode);
                        }
                    }
                    else
                        ++index;
                }
                else if (assetNode is TextAssetNode)
                {
                    TextAssetNode textAssetNode = assetNode as TextAssetNode;
                    m_WaitingRequest.RemoveAt(index);
                    // 如果没有加载完成
                    if (!assetNode.IsAssetLoadSuccess)
                    {
                        textAssetNode.LoadFromFileAsync();
                        //curRequest.retryCount = 0;
                        m_ProcessingRequest.Add(textAssetNode);
                    }
                }
            }

            if (!m_IsUpdate)
                yield break;

            yield return null;
        }
    }

    /// <summary>
    /// 节点依赖的节点是否是已经下载完成的状态
    /// </summary>
    /// <param name="assetNode">检查的节点</param>
    /// <returns>下载状态</returns>
    private static bool IsAssetDependenciesReady(AssetBundleAssetNode assetBundleAssetNode)
    {
        for (int i = 0; i < assetBundleAssetNode.mChild.Length; i++)
        {
            AssetBundleAssetNode node = assetBundleAssetNode.mChild[i];
            if (!node.IsAssetLoadSuccess)
                return false;
            if (!IsAssetDependenciesReady(node))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 执行回调事件
    /// </summary>
    /// <param name="bundleNameID">回调的节点</param>
    private static void InvokeCallback(AssetNode assetNode)
    {
        if (assetNode != null && assetNode is AssetBundleAssetNode)
        {
            // 通知所有节点任务完成
            (assetNode as AssetBundleAssetNode).OnParentLoadDone();
        }

        AssetNodeManager.OnAssetDownloadCallback(assetNode);
    }

    /// <summary>
    /// 是否加载完还没处理
    /// </summary>
    public bool IsAssetBundleDone(string bundleNameID)
    {
        if (m_ProcessingRequest.ContainsKey(bundleNameID))
        {
            AssetNode assetNode = m_ProcessingRequest.GetByKey(bundleNameID);
            /*if (abRequest != null && abRequest.createRequest != null && abRequest.mAssetNode != null)
            {
                return abRequest.createRequest.isDone;
            }*/
        }
        return false;
    }

    /// <summary>
    /// 从进度中获取assetbundle
    /// </summary>
    public AssetBundle GetAssetBundle(string bundleNameID)
    {
        if (m_ProcessingRequest.ContainsKey(bundleNameID))
        {
            /*ABRequest abRequest = m_ProcessingRequest.GetByKey(bundleNameID);
            if (abRequest != null && abRequest.createRequest != null && abRequest.createRequest.isDone)
            {
                return abRequest.GetAssetBundle();
            }*/
        }
        return null;
    }

    /// <summary>
    /// 停止下载一个对象
    /// </summary>
    /// <param name="bundleNameID"></param>
    public void StopDownload(string bundleNameID)
    {
        // 从等待队列中移除
        if (m_WaitingRequest.ContainsKey(bundleNameID))
            m_WaitingRequest.Remove(bundleNameID);

        // 从下载队列中移除
        if (m_ProcessingRequest.ContainsKey(bundleNameID))
        {
            //m_ProcessingRequest.GetByKey(bundleNameID).createRequest = null;
            m_ProcessingRequest.Remove(bundleNameID);
        }
    }

    /// <summary>
    /// 停止所有的下载
    /// </summary>
    public void StopAll()
    {
        Logger.PrintLog("AssetNodeAsyncLoader.StopAll()");

        m_WaitingRequest.Clear();
        for (int i = 0, UPPER = m_ProcessingRequest.Count; i < UPPER; i++)
        {
            //ABRequest request = m_ProcessingRequest.GetByIndex(i);
            //request.createRequest = null;
        }
        m_ProcessingRequest.Clear();
    }
}