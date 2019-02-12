using UnityEngine;
using System.Collections;
using System;

public class ABLoader : BaseLoader
{

    WWW contant;
    public override void StartDown(LoaderManager.LoadOrder order)
    {
        base.StartDown(order);
        string fullPath = order.resUrl;
        if (order.orderType != LoaderType.AssetBundleManifest)
        {
            fullPath = FillPathWithExtension(order.resUrl);
        }
        fullPath = CommonUtils.ConnectStrs(CommonPathUtils.getLoadRootDir(order.packageName, order.resUrl), fullPath);
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
                    loadOrder.OnLoadFinish(loadOrder.resUrl, contant.assetBundle);
                    loadOrder.OnLoadFinish = null;
                }
                currentState = LoadState.finish;
                contant = null;
            }
        }
    }


    string FillPathWithExtension(string path)
    {
        string extension = ".unity3d";
        if (!path.EndsWith(extension))
        {
            return path + extension;
        }
        return path;
    }

}
