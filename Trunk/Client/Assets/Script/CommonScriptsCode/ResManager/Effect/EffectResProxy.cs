using System.Collections.Generic;
using UnityEngine;

public class EffectResProxy
{
    private Dictionary<string, AssetBundleManifest> m_dictEffectManifest = new Dictionary<string, AssetBundleManifest>();

    public AssetBundleManifest GetPackManifest(string packageName)
    {
        if (!m_dictEffectManifest.ContainsKey(packageName))
        {
            m_dictEffectManifest.Add(packageName, null);
            ResLoadManager.LoadAsync(AssetType.Manifest, packageName, UtilMethod.ConnectStrs("effect/", packageName, "/", packageName), (relativePath, res) =>
            {
                m_dictEffectManifest[packageName] = res as AssetBundleManifest;
                ResLoadManager.SetManifest(m_dictEffectManifest[packageName], AssetType.Effect, packageName);
            });
        }
        return m_dictEffectManifest[packageName];
    }
}
