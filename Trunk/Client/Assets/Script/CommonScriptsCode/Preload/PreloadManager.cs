using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class PreloadManager : Singleton<PreloadManager>
{
    bool preloadSign = false;
    public void ExecuteOrder(LuaPreloadOrder order)
    {
        if (preloadSign)
        {
            Loger.PrintError("预加载同一时间只能执行一个");
            return;
        }
        preloadSign = true;
        MainThread.Instance.StartCoroutine(AsynPreload(order));
    }

    IEnumerator AsynPreload(LuaPreloadOrder order)
    {
        List<LuaUIView> uiViewList = order.getUIPreload();
        LuaScene scene = order.getScenePreload();
        if (uiViewList != null)
        {
            for (int i = 0; i < uiViewList.Count; i++)
            {
                if (uiViewList[i].getIsLoaded())
                {
                    order.onPreloadStepEnd();
                    continue;
                }
                var loadOrders = uiViewList[i].getLoadOrders();
                foreach (var loadPath in loadOrders)
                {
                    string[] orderArr = loadPath.Split(':');
                    if (orderArr.Length != 2)
                    {
                        orderArr = new string[2];
                        orderArr[0] = "base";
                        orderArr[1] = loadPath;
                    }
                    yield return MainThread.Instance.StartCoroutine(UILoadControl.Instance.AsyncCreateUI(orderArr[0], orderArr[1], uiViewList[i], false, order));
                }
                while (!uiViewList[i].getIsLoaded())
                    yield return 0;
                //Debug.LogFormat("加载单步UI完毕. name: {0}, state:{1}", loadOrders[0], uiViewList[i].getIsLoaded());
            }
        }
        bool changeSign = true;
        if (scene != null)
        {
            if (scene.getIsInit())
            {
                order.onPreloadStepEnd();
            }
            else
            {
                changeSign = false;
                SceneManager.Instance.Change(scene, () =>
                {
                    changeSign = true;
                    order.onPreloadStepEnd();
                });
            }
        }
        while (!changeSign)
        {
            yield return 0;
        }

        order.onPreloadEnd();
        preloadSign = false;
    }

}
