using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public enum AssetType
{
    PB = 1,
    Text = 2,

    Manifest = 100,
    UI = 101,
    Audio = 102,
    Effect = 103,
    Scene = 104,
    Model = 105,

    UI_Dep = 201,
    Effect_Dep = 203,
    Scene_Dep = 204,
    Model_Dep = 205,
}

/// <summary>
/// 资源节点的AssetBundle的加载状态
/// </summary>
public enum AssetBundleLoadStatus
{
    None,
    Loading,    // 正在加载
    Success,    // 加载成功
    Failed      // 加载失败
}

/// <summary>
/// 资源节点的Asset的加载状态
/// </summary>
public enum AssetLoadStatus
{
    None,
    Loading,    // 正在加载
    Success,    // 加载成功
    Failed      // 加载失败
}

public class AssetBundleAssetNode : AssetNode
{
    public AssetBundleCreateRequest createRequest = null;

    /// <summary>已经加载的AssetBundle</summary>
    protected AssetBundle m_assetBundle;
    /// <summary>已经加载的Asset资源</summary>
    protected Object[] m_allAssets = new Object[0];

    /// <summary>资源节点的AssetBundle的加载状态</summary>
    public AssetBundleLoadStatus mAssetBundleLoadStatus = AssetBundleLoadStatus.None;
    /// <summary>资源节点的Asset的加载状态</summary>
    public AssetLoadStatus mAssetLoadStatus = AssetLoadStatus.None;

    /// <summary>AssetBundle已经加载完成</summary>
    public bool IsAssetBundleLoadSuccess
    {
        get { return mAssetBundleLoadStatus == AssetBundleLoadStatus.Success; }
    }
    /// <summary>Asset已经加载完成</summary>
    public override bool IsAssetLoadSuccess
    {
        get { return mAssetLoadStatus == AssetLoadStatus.Success; }
    }

    /// <summary>父节点</summary>
    public HashSet<AssetBundleAssetNode> mParent = new HashSet<AssetBundleAssetNode>();
    /// <summary>子节点</summary>
    public AssetBundleAssetNode[] mChild = new AssetBundleAssetNode[0];

    public AssetBundleAssetNode(AssetType assetType, string packageName, string relativePath) : base(assetType, packageName, relativePath)
    {
        string[] dependencies;
        switch (assetType)
        {
            case AssetType.UI:
            case AssetType.UI_Dep:
                if (!relativePath.StartsWith("ui/"))
                {
                    Logger.PrintError("资源路径错误");
                    return;
                }
                dependencies = ABManifestManager.GetDependencies(relativePath.Substring(3), AssetType.UI, packageName);
                if (dependencies != null)
                {
                    int len = dependencies.Length;
                    if (len > 0)
                    {
                        mChild = new AssetBundleAssetNode[len];
                        for (int i = 0; i < len; ++i)
                        {
                            if (packageName != "base" && dependencies[i].StartsWith("base/"))
                                mChild[i] = AssetNodeManager.GetOrCreateAssetNode(AssetType.UI_Dep, "base", CommonUtils.ConnectStrs("ui/", dependencies[i]), this) as AssetBundleAssetNode;
                            else
                                mChild[i] = AssetNodeManager.GetOrCreateAssetNode(AssetType.UI_Dep, packageName, CommonUtils.ConnectStrs("ui/", dependencies[i]), this) as AssetBundleAssetNode;
                        }
                    }
                }
                break;
            case AssetType.Scene:
            case AssetType.Scene_Dep:
                if (!relativePath.StartsWith("scene/"))
                {
                    Logger.PrintError("资源路径错误");
                    return;
                }
                string path = relativePath.Substring(6);
                int index = path.IndexOf('/');
                string sceneName = path.Substring(0, index);
                path = path.Substring(index + 1);
                dependencies = ABManifestManager.GetDependencies(path, AssetType.Scene, sceneName);
                if (dependencies != null)
                {
                    int len = dependencies.Length;
                    if (len > 0)
                    {
                        mChild = new AssetBundleAssetNode[len];
                        for (int i = 0; i < len; ++i)
                        {
                            mChild[i] = AssetNodeManager.GetOrCreateAssetNode(AssetType.Scene_Dep, packageName, CommonUtils.ConnectStrs("scene/", sceneName, "/", dependencies[i]), this) as AssetBundleAssetNode;
                        }
                    }
                }
                break;
            case AssetType.Effect:
            case AssetType.Effect_Dep:
                if (!relativePath.StartsWith("effect/"))
                {
                    Logger.PrintError("资源路径错误");
                    return;
                }
                string pathEffect = relativePath.Substring(7);
                int indexEffect = pathEffect.IndexOf('/');
                pathEffect = pathEffect.Substring(indexEffect + 1);
                dependencies = ABManifestManager.GetDependencies(pathEffect, AssetType.Effect, packageName);
                if (dependencies != null)
                {
                    int len = dependencies.Length;
                    if (len > 0)
                    {
                        mChild = new AssetBundleAssetNode[len];
                        for (int i = 0; i < len; ++i)
                        {
                            mChild[i] = AssetNodeManager.GetOrCreateAssetNode(AssetType.Effect_Dep, packageName, CommonUtils.ConnectStrs("effect/", packageName, "/", dependencies[i]), this) as AssetBundleAssetNode;
                        }
                    }
                }
                break;
            case AssetType.Model:
            case AssetType.Model_Dep:
                if (!relativePath.StartsWith("model/"))
                {
                    Logger.PrintError("资源路径错误:" + relativePath);
                    return;
                }
                string pathModel = relativePath.Substring(6);
                int indexModel = pathModel.IndexOf('/');
                pathModel = pathModel.Substring(indexModel + 1);
                dependencies = ABManifestManager.GetDependencies(pathModel, AssetType.Model, packageName);
                if (dependencies != null)
                {
                    int len = dependencies.Length;
                    if (len > 0)
                    {
                        mChild = new AssetBundleAssetNode[len];
                        for (int i = 0; i < len; ++i)
                        {
                            mChild[i] = AssetNodeManager.GetOrCreateAssetNode(AssetType.Model_Dep, packageName, CommonUtils.ConnectStrs("model/", packageName, "/", dependencies[i]), this) as AssetBundleAssetNode;
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 添加一个父节点
    /// </summary>
    public void AddParent(AssetBundleAssetNode parent)
    {
        mParent.Add(parent);
    }

    /// <summary>
    /// 移除一个父节点
    /// </summary>
    /// <param name="parent">父节点</param>
    public void RemoveParent(AssetBundleAssetNode parent)
    {
        if (!mParent.Contains(parent))
            return;

        mParent.Remove(parent);

        // 如果没有父节点了，说明该节点已经没有引用，应该释放掉
        if (mParent.Count <= 0)
            AssetNodeManager.ReleaseNode(assetType, packageName, relativePath);
    }

    private bool m_IsWeakUnload = false;
    /// <summary>
    /// 父类加载完成的回调，用来处理资源的释放情况
    /// </summary>
    public void OnParentLoadDone()
    {
        for (int i = 0; i < mChild.Length; i++)
            mChild[i].OnParentLoadDone();

        if (m_IsWeakUnload)
            return;

        foreach (AssetBundleAssetNode node in mParent)
        {
            if (!node.IsAssetLoadSuccess)
                return;
        }

        // 对于单个引用的资源可以释放掉
        if (IsWeakNode())
        {
            m_assetBundle.Unload(false);
            m_assetBundle = null;
            m_IsWeakUnload = true;
        }
    }

    // 是需要及时释放的节点
    public bool IsWeakNode()
    {
        switch (assetType)
        {
            case AssetType.Manifest:
            case AssetType.UI:
            //case AssetType.Audio:
            case AssetType.Effect:
            //case AssetType.Scene:
            case AssetType.Model:
                return true;
        }
        return false;
    }

    public void LoadFromFileAsync()
    {
        string fullPath = CommonUtils.ConnectStrs(CommonPathUtils.getLoadRootDir(packageName, relativePath), CommonPathUtils.PathWithENcrypt(relativePath));
        if (assetType == AssetType.Manifest &&
            Application.platform == RuntimePlatform.Android && fullPath.Contains(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH))
            fullPath = CommonUtils.ConnectStrs("file://", fullPath);

        ResLoadManager.PrintLoadLog("AB开始异步加载资源：" + fullPath);
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

    /// <summary>
    /// 异步加载资源完成的回调
    /// </summary>
    /// <param name="assetBundle">加载完成的资源</param>
    public virtual IEnumerator OnAssetBundleLoadedAsync(AssetBundle assetBundle)
    {
        if (!SetAssetBundleLoaded(assetBundle))
            yield break;

        // 缓存包含的资源
        if (!m_assetBundle.isStreamedSceneAssetBundle)
        {
            AssetBundleRequest assetBundleRequest = m_assetBundle.LoadAllAssetsAsync();
            yield return assetBundleRequest;

            m_allAssets = assetBundleRequest.allAssets;
            SetIncludeAssets();
        }
    }

    /// <summary>
    /// 设置加载状态
    /// </summary>
    /// <param name="assetBundle"></param>
    /// <returns>是否加载成功</returns>
    protected bool SetAssetBundleLoaded(AssetBundle assetBundle)
    {
        if (assetBundle == null)
        {
            // 加载失败
            mAssetBundleLoadStatus = AssetBundleLoadStatus.Failed;
            Logger.PrintError("AssetNode::OnAssetBundleLoaded 资源加载失败 => " + relativePath);
            return false;
        }
        m_assetBundle = assetBundle;
        // 加载成功
        mAssetBundleLoadStatus = AssetBundleLoadStatus.Success;
        return true;
    }

    /// <summary>
    /// 设置包含的资源
    /// </summary>
    private void SetIncludeAssets()
    {
        mAssetLoadStatus = AssetLoadStatus.Success;
    }

    /// <summary>
    /// 释放节点资源
    /// </summary>
    public override void Release()
    {
        //Logger.PrintColor("#00ffff", "释放节点的引用:" + relativePath);
        // 释放子节点的引用
        for (int i = 0; i < mChild.Length; ++i)
        {
            //Logger.PrintColor("#00ffff", "释放子节点的引用:" + mChild[i].relativePath);
            mChild[i].RemoveParent(this);
        }
        mChild = new AssetBundleAssetNode[0];

        CleanAssets();

        // 重置数据
        mParent.Clear();
        //mBundleData = null;
        m_IsWeakUnload = false;
        mAssetBundleLoadStatus = AssetBundleLoadStatus.None;
        mAssetLoadStatus = AssetLoadStatus.None;
    }

    private void CleanAssets()
    {
        //        LoggerHelper.Log(">>> Clean Assets " + (mBundleData != null ? mContext.m_PathBundleData.mIDToPath[mBundleData.mBundleNameID] : " unkown "));

        Object[] objs = m_allAssets;
        m_allAssets = new Object[0];

        // 卸载AssetBundle
        if (m_assetBundle != null)
        {
            //Logger.PrintColor("#0000ff", "卸载AssetBundle(true):" + relativePath);
            m_assetBundle.Unload(true);
            m_assetBundle = null;
        }
        else
        {
            //Logger.PrintColor("#0000ff", "卸载Assets:" + relativePath);
            for (int i = 0, len = objs.Length; i < len; i++)
            {
                Object obj = objs[i];
                if (!(obj is GameObject) && !(obj is Component))
                {
                    //Logger.PrintColor("#0000ff", "卸载Asset:" + obj.name);
                    Resources.UnloadAsset(obj);
                }
            }
        }
    }

    /// <summary>
    /// 获取该资源包中的资源
    /// </summary>
    /// <param name="fileNameID">资源的路径名称</param>
    /// <returns>对应的名称的资源</returns>
    public override object GetAsset()
    {
        switch (assetType)
        {
            case AssetType.Manifest:
            case AssetType.UI:
            case AssetType.Audio:
            case AssetType.Effect:
            case AssetType.Scene:
            case AssetType.Model:
                return m_allAssets[0];
            default:
                return null;
        }
    }
}

public class TextAssetNode : AssetNode
{
    public WWW www = null;

    /// <summary>资源节点的Asset的加载状态</summary>
    public AssetLoadStatus mAssetLoadStatus = AssetLoadStatus.None;

    /// <summary>Asset已经加载完成</summary>
    public override bool IsAssetLoadSuccess
    {
        get { return mAssetLoadStatus == AssetLoadStatus.Success; }
    }

    public TextAssetNode(AssetType assetType, string packageName, string relativePath) : base(assetType, packageName, relativePath)
    {
    }

    public void LoadFromFileAsync()
    {
        string loadRootDir = CommonPathUtils.getLoadRootDir(packageName, relativePath);
        if (Application.isEditor && assetType == AssetType.PB && packageName == "base")
            loadRootDir = CommonUtils.ConnectStrs(CommonPathUtils.STREAMING_ASSETS_ROOT_PATH, "/res/");
        
        string fullPath = CommonUtils.ConnectStrs(loadRootDir, CommonPathUtils.PathWithENcrypt(relativePath));
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
            (Application.platform == RuntimePlatform.Android && fullPath.Contains(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH)) ||
            Application.platform == RuntimePlatform.IPhonePlayer)
            fullPath = CommonUtils.ConnectStrs("file://", fullPath);

        ResLoadManager.PrintLoadLog("WWW开始异步加载资源：" + fullPath);
        try
        {
            www = new WWW(fullPath);
        }
        catch (Exception e)
        {
            Logger.PrintError("异步加载资源错误：" + fullPath + "\n" + e.Message);
            return;
        }
    }

    /// <summary>
    /// 异步加载资源完成的回调
    /// </summary>
    /// <param name="assetBundle">加载完成的资源</param>
    public void OnAssetLoadedAsync()
    {
        mAssetLoadStatus = AssetLoadStatus.Success;
    }

    public override object GetAsset()
    {
        switch(assetType)
        {
            case AssetType.PB:
                return www.bytes;
            case AssetType.Text:
                return www.text;
            default:
                return null;
        }
    }
}

public class AssetNode : UniqueKeyData<string>
{
    public AssetType assetType;
    public string packageName;
    public string relativePath;

    public string mUniqueKey
    {
        get { return relativePath; }
    }

    public AssetNode(AssetType assetType, string packageName, string relativePath)
    {
        this.assetType = assetType;
        this.packageName = packageName;
        this.relativePath = relativePath;
    }

    /// <summary>Asset已经加载完成</summary>
    public virtual bool IsAssetLoadSuccess
    {
        get { return false; }
    }

    public virtual object GetAsset()
    {
        return null;
    }

    /// <summary>
    /// 释放节点资源
    /// </summary>
    public virtual void Release()
    {
    }
}