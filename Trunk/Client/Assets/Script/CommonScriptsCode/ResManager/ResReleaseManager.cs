using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ResReleaseManager
{
    public static void ReleasePackage(string packageName)
    {
        //Logger.PrintLog(CommonUtils.ConnectStrs("卸载包：", packageName));
        AudioManager.Instance.DestroyPackageAudio(packageName);
        UIViewManager.Instance.DestroyPackageView(packageName);
        EffectManager.Instance.DestroyPackageEffect(packageName);
        ModelManager.Instance.DestroyPackageModel(packageName);
        Resources.UnloadUnusedAssets();
        //Logger.PrintLog(CommonUtils.ConnectStrs("卸载包结束：", packageName));
    }
}