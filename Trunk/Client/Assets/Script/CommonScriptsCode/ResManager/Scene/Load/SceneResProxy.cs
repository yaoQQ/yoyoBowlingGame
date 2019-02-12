using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResProxy
{
    #region 场景数据
    
    Dictionary<string, SceneData> sceneDataDic = new Dictionary<string, SceneData>();

    public SceneData GetSceneData(string sceneName)
    {
        sceneName = sceneName.ToLower();
        if (!sceneDataDic.ContainsKey(sceneName))
        {
            sceneDataDic.Add(sceneName, null);
            //LoadManager.Instance.AddOrder(LoaderType.Text, "scene/" + sceneName + "/data/" + sceneName + ".json", LoadSceneDataEnd);
            ResLoadManager.LoadAsync(AssetType.Text, sceneName, "scene/" + sceneName + "/data/" + sceneName + ".json", (relativePath, res) =>
            {
                SceneData sceneData = JsonUtility.FromJson<SceneData>(res as string);
                sceneDataDic[sceneName] = sceneData;
            });
        }
        return sceneDataDic[sceneName];
    }

    void LoadSceneDataEnd(string fileName, System.Object res)
    {
       
        string sceneName = fileName.Split('/')[1];
        SceneData sceneData = JsonUtility.FromJson<SceneData>(res.ToString());
        sceneDataDic[sceneName]= sceneData;
    }

    #endregion




    Dictionary<string, AssetBundleManifest> sceneManifestDic = new Dictionary<string, AssetBundleManifest>();

	public AssetBundleManifest GetSceneManifest(string sceneName)
    {
        sceneName = sceneName.ToLower();
        if (!sceneManifestDic.ContainsKey(sceneName))
        {
            sceneManifestDic.Add(sceneName, null);
           
            //LoadManager.Instance.AddOrder(LoaderType.AssetBundleManifest, "scene/" + sceneName.ToLower()+ "/"+sceneName.ToLower(), LoadSceneManifestEnd);
            ResLoadManager.LoadAsync(AssetType.Manifest, sceneName, UtilMethod.ConnectStrs("scene/", sceneName, "/", sceneName), (relativePath, res) =>
            {
                sceneManifestDic[sceneName] = res as AssetBundleManifest;
                ResLoadManager.SetManifest(sceneManifestDic[sceneName], AssetType.Scene, sceneName);
            });
        }
        return sceneManifestDic[sceneName];
    }
}
