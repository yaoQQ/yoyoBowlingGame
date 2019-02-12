using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class EffectManager : Singleton<EffectManager>
{
    private EffectResProxy resProxy = new EffectResProxy();

    private Dictionary<string, List<string>> m_dict = new Dictionary<string, List<string>>();

    public void DelEffect(EffectControler effectControler)
    {
        if(effectControler != null)
            GameObject.Destroy(effectControler.gameObject);
    }

    public void CreateEffect(string packName, string effectName, Action<EffectControler> onLoadend)
    {
        packName = packName.ToLower();
        effectName = effectName.ToLower();
        MainThread.Instance.StartCoroutine(AsyncCreateEffect(packName, effectName, onLoadend));

    }
    private IEnumerator AsyncCreateEffect(string packName, string effectName, Action<EffectControler> onLoadend)
    {
        while (resProxy.GetPackManifest(packName) == null)
        {
            yield return 0;
        }

        string abRelativePath = UtilMethod.ConnectStrs("effect/", packName, "/prefab/", effectName, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.Effect, packName, abRelativePath, (relativePath, res) =>
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
                GameObject go = GameObject.Instantiate(res as GameObject);
                go.SetActive(false);
                EffectControler ec = go.GetComponent<EffectControler>();
                if (ec == null)
                {
                    ec = go.AddComponent<EffectControler>();
                }
                onLoadend.Invoke(ec);
            }
        });
    }

    public static void SetEffectToSceneCamera(EffectControler effectControler, SceneCamera sceneCamera)
    {
        effectControler.gameObject.transform.parent = sceneCamera.gameObject.transform;
        SetGosToLayer(effectControler.gameObject, GetLayerFrom(sceneCamera.cameraInfo.cullingMask));
    }

    private static void SetGosToLayer(GameObject go,int layerValue)
    {
        go.layer = layerValue;
        for(int i=0;i<go.transform.childCount;i++)
        {
            SetGosToLayer(go.transform.GetChild(i).gameObject,layerValue);
        }
    }


    private static int GetLayerFrom(int cullingMask)
    {
        for(int i=8;i<=32;i++)
        {
            int layerCM = 1 << i;
            if(layerCM== cullingMask)
            {
                return i;
            }
        }
        return 0;
    }

    [BlackList]
    /// <summary>
    /// 销毁一个包的所有特效
    /// </summary>
    /// <param name="packageName"></param>
    public void DestroyPackageEffect(string packageName)
    {
        if (!m_dict.ContainsKey(packageName))
            return;
        for (int i = 0, count = m_dict[packageName].Count; i < count; ++i)
        {
            //Logger.PrintLog(CommonUtils.ConnectStrs("卸载特效：", m_dict[packageName][i]));
            AssetNodeManager.ReleaseNode(AssetType.Effect, packageName, m_dict[packageName][i]);
        }
    }
}
