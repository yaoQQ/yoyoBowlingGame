using System.Collections.Generic;
using UnityEngine;

public class ModelResProxy
{
    private Dictionary<string, AssetBundleManifest> m_dictModelManifest = new Dictionary<string, AssetBundleManifest>();

    public AssetBundleManifest GetPackManifest(string packageName)
    {
        if (!m_dictModelManifest.ContainsKey(packageName))
        {
            m_dictModelManifest.Add(packageName, null);
            ResLoadManager.LoadAsync(AssetType.Manifest, packageName, UtilMethod.ConnectStrs("model/", packageName, "/", packageName), (relativePath, res) =>
            {
                m_dictModelManifest[packageName] = res as AssetBundleManifest;
                ResLoadManager.SetManifest(m_dictModelManifest[packageName], AssetType.Model, packageName);
            });
        }
        return m_dictModelManifest[packageName];
    }
}