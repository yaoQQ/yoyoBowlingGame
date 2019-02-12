using XLua;

[LuaCallCSharp]
public class LoadManager : Singleton<LoadManager>
{
    public delegate void LoadedFinishDelegate(string resName, System.Object res);

    public void AddOrderPB(string packageName, string url, LoadedFinishDelegate finishDelegate)
    {
        ResLoadManager.LoadAsync(AssetType.PB, packageName, url, (relativePath, res) =>
        {
            finishDelegate(relativePath, res);
        });
    }
}