using UnityEngine;
using System.Collections;

public class TextLoader : BaseLoader
{
    WWW contant;
    public override void StartDown(LoaderManager.LoadOrder order)
    {
        base.StartDown(order);
        string fullPath = CommonUtils.ConnectStrs(CommonPathUtils.getLoadRootDir(order.packageName, order.resUrl), CommonPathUtils.PathWithENcrypt(order.resUrl));
        if (CommonUtils.isUnityEditor)
            fullPath = CommonUtils.ConnectStrs("file://", fullPath);
        Logger.PrintLog(CommonUtils.ConnectStrs("加载资源：", fullPath));
        contant = new WWW(fullPath);
    }

    public override void RunDown()
    {
        if (contant.error != null && contant.error.Length > 0)
        {
            Logger.PrintError("Error:", contant.error, "(", contant.url, ")");
            currentState = LoadState.fail;
        }
        else
        {
            if (contant.isDone)
            {
                if (loadOrder.OnLoadFinish != null)
                {
                    if (loadOrder.orderType == LoaderType.PB)
                    {
                        loadOrder.OnLoadFinish(loadOrder.resUrl, contant.bytes);
                        loadOrder.OnLoadFinish = null;
                    }
                    else
                    {
                        loadOrder.OnLoadFinish(loadOrder.resUrl, contant.text);
                        loadOrder.OnLoadFinish = null;
                    }
                }
                currentState = LoadState.finish;
                contant = null;
            }
        }
    }
}