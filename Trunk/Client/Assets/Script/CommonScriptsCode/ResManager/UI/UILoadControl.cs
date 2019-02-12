using UnityEngine;
using System;
using System.Collections;
using XLua;

[LuaCallCSharp]
public class UILoadControl : Singleton<UILoadControl>
{
    private UIresProxy resProxy = new UIresProxy();

    [BlackList]
    public void CreateUI(string packName, string name, Action<string, GameObject> onLoadUIEnd, bool isInstantiation = true)
    {
        MainThread.Instance.StartCoroutine(AsyncCreateUI(packName,name, onLoadUIEnd, isInstantiation));
    }
    private IEnumerator AsyncCreateUI(string packageName, string name, Action<string, GameObject> onLoadUIEnd, bool isInstantiation)
    {
        while (resProxy.GetManifest(packageName) == null)
        {
            yield return 0;
        }

        string abRelativePath = UtilMethod.ConnectStrs("ui/", packageName, "/prefab/", name, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.UI, packageName, abRelativePath, (relativePath, res) =>
        {
            GameObject go = GameObject.Instantiate(res as GameObject);
            if (onLoadUIEnd != null)
                onLoadUIEnd(UtilMethod.ConnectStrs(packageName, ":", name), go);
        });
    }

    public void CreateUI(string packName, string name, LuaUIView uiView, bool isInstantiation = true, LuaPreloadOrder order = null)
    {
        MainThread.Instance.StartCoroutine(AsyncCreateUI(packName,name, uiView, isInstantiation, order));
    }
    public IEnumerator AsyncCreateUI(string packageName, string name, LuaUIView uiView, bool isInstantiation, LuaPreloadOrder order = null)
    {
        if (packageName == "base" || packageName == "mahjonghul" || packageName == "marbles")
        {
            while (resProxy.GetManifest(packageName) == null)
            {
                yield return 0;
            }
        }

        string abRelativePath = UtilMethod.ConnectStrs("ui/", packageName, "/prefab/", name, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.UI, packageName, abRelativePath, (relativePath, res) =>
        {
            GameObject go = GameObject.Instantiate(res as GameObject);
            if (uiView != null)
                uiView.executeLoadUIEnd(UtilMethod.ConnectStrs(packageName, ":", name), go);
            if (order != null)
                order.onPreloadStepEnd();
        });
    }
}