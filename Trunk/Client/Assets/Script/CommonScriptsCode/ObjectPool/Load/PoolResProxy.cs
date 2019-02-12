using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolResProxy
{
    bool loadedManifestSign = false;
    AssetBundleManifest effectPackManifest;
    
    public AssetBundleManifest GetPackManifest(string packageName)
    {
        if (!loadedManifestSign)
        {
            loadedManifestSign = true;
            //LoadManager.Instance.AddOrder(LoaderType.AssetBundleManifest,  "effect/effect", LoadEffectManifestEnd);
            ResLoadManager.LoadAsync(AssetType.Manifest, packageName, "effect/" + packageName + "/" + packageName, (relativePath, res) =>
            {
                effectPackManifest = res as AssetBundleManifest;
                ResLoadManager.SetManifest(effectPackManifest, AssetType.Effect, packageName);
            });
        }
        return effectPackManifest;
    }
}
