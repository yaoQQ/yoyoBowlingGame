using System.Collections.Generic;
using UnityEngine;

public class UIresProxy
{
    private Dictionary<string, AssetBundleManifest> m_dictUIManifest = new Dictionary<string, AssetBundleManifest>();

    public AssetBundleManifest GetManifest(string packageName)
    {
        if (!m_dictUIManifest.ContainsKey(packageName))
        {
            //开始加载
            m_dictUIManifest.Add(packageName, null);
            ResLoadManager.LoadAsync(AssetType.Manifest, packageName, "ui/ui", (relativePath, res) =>
            {
                m_dictUIManifest[packageName] = res as AssetBundleManifest;
                ResLoadManager.SetManifest(m_dictUIManifest[packageName], AssetType.UI, packageName);
            });
        }
        return m_dictUIManifest[packageName];
    }
}