using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ModelManager : Singleton<ModelManager>
{
    private ModelResProxy resProxy = new ModelResProxy();

    private Dictionary<string, List<string>> m_dict = new Dictionary<string, List<string>>();

    public void DestroyModel(GameObject model)
    {
        if (model != null)
            GameObject.Destroy(model);
    }

    public void CreateModel(string packName, string modelName, Action<GameObject> onLoadend)
    {
        packName = packName.ToLower();
        modelName = modelName.ToLower();
        MainThread.Instance.StartCoroutine(AsyncCreateModel(packName, modelName, onLoadend));
    }

    private IEnumerator AsyncCreateModel(string packName, string modelName, Action<GameObject> onLoadend)
    {
        while (resProxy.GetPackManifest(packName) == null)
        {
            yield return 0;
        }

        string abRelativePath = UtilMethod.ConnectStrs("model/", packName, "/prefab/", modelName, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.Model, packName, abRelativePath, (relativePath, res) =>
        {
            if (!m_dict.ContainsKey(packName))
            {
                m_dict.Add(packName, new List<string>());
                m_dict[packName].Add(abRelativePath);
            }
            else
            {
                if (!m_dict[packName].Contains(abRelativePath))
                    m_dict[packName].Add(abRelativePath);
            }
            if (onLoadend != null)
            {
                //GameObject go = GameObject.Instantiate(res as GameObject);
                //go.SetActive(false);
                //onLoadend.Invoke(go);
                
                onLoadend.Invoke(res as GameObject);
            }
        });
    }

    [BlackList]
    /// <summary>
    /// 销毁一个包的所有模型
    /// </summary>
    /// <param name="packageName"></param>
    public void DestroyPackageModel(string packageName)
    {
        if (!m_dict.ContainsKey(packageName))
            return;
        for (int i = 0, count = m_dict[packageName].Count; i < count; ++i)
        {
            //Logger.PrintLog(CommonUtils.ConnectStrs("卸载模型：", m_dict[packageName][i]));
            AssetNodeManager.ReleaseNode(AssetType.Model, packageName, m_dict[packageName][i]);
        }
    }
}