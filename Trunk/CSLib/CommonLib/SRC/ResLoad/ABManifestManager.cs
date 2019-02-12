using System.Collections.Generic;
using UnityEngine;

public class ABManifestManager
{
    private static Dictionary<AssetType, Dictionary<string, AssetBundleManifest>> m_dict = new Dictionary<AssetType, Dictionary<string, AssetBundleManifest>>();

    /// <summary>
    /// 设置引用
    /// </summary>
    /// <param name="abManifest"></param>
    /// <param name="assetType"></param>
    /// <param name="name"></param>
    public static void SetManifest(AssetBundleManifest abManifest, AssetType assetType, string name = "default")
    {
        if (!m_dict.ContainsKey(assetType))
            m_dict[assetType] = new Dictionary<string, AssetBundleManifest>();
        m_dict[assetType][name] = abManifest;
    }

    public static string[] GetDependencies(string abName, AssetType assetType, string name = "default")
    {
        if (assetType == AssetType.UI && name != "mahjonghul" && name != "marbles")
            name = "base";
        if (!m_dict.ContainsKey(assetType))
            return null;
        if (!m_dict[assetType].ContainsKey(name))
            return null;
        return m_dict[assetType][name].GetAllDependencies(abName);
    }
}