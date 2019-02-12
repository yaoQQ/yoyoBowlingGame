using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager : Singleton<LuaManager>
{

    private LuaEnv luaEvn = null;

    public T GetGlobalValue<T>(string key)
    {
        return luaEvn.Global.Get<T>(key);
    }
    public LuaEnv GetLuaEnv()
    {
        return luaEvn;
    }
    public void Init()
    {
        //创建lua解释器  
        luaEvn = new LuaEnv();
        luaEvn.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        luaEvn.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

        //添加自定义装载机Loader  
        luaEvn.AddLoader(CustomLoaderMethod);

        //加载lua主类
        luaEvn.DoString("require 'base:Main'");
    }

    public void Execute()
    {
        if (luaEvn != null)
        {
            luaEvn.Tick();
        }
    }
    

    public void OnDestroy()
    {
        luaEvn.Dispose();
    }

    private Dictionary<string, AssetBundle> m_dict = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 加载方法
    /// </summary>
    private byte[] CustomLoaderMethod(ref string fileName)
    {
        string[] fileArr = fileName.Split(':');
        if (fileArr.Length != 2) return null;
        string packageName = fileArr[0];
        string filePath = fileArr[1];

        if (CommonUtils.isUnityEditor && (packageName == "base" || CommonPathUtils.isLoadEditorRes))
        {
            string relativePath = UtilMethod.ConnectStrs("lua/", packageName, "/", filePath, ".lua");
            string dir = UtilMethod.ConnectStrs(PathUtil.STREAMING_ASSETS_ROOT_PATH, "/res/");
            string path = UtilMethod.ConnectStrs(dir, relativePath);
            //Debug.Log(path);
            FileInfo fInfo = new FileInfo(path);
            if (fInfo.Exists)
            {
                StreamReader sr = new StreamReader(path);
                string str = (sr.ReadToEnd());
                //Debug.Log(str);
                return System.Text.Encoding.UTF8.GetBytes(str);
            }
            return null;
        }
        else
        {
            if (!m_dict.ContainsKey(packageName))
                LoadLuaAB(packageName);
            TextAsset textAsset = m_dict[packageName].LoadAsset(filePath.Replace("/", ".") + ".lua") as TextAsset;
            if (textAsset != null)
            {
                string str = textAsset.text;
                if ((byte)str[0] == 0xff)
                    str = str.Substring(1);
                return System.Text.Encoding.UTF8.GetBytes(str);
            }
            return null;
        }
    }

    /// <summary>
    /// 加载lua的AssetBundle
    /// </summary>
    private void LoadLuaAB(string packageName)
    {
        string relativePath = UtilMethod.ConnectStrs("lua/", packageName.ToLower(), ".unity3d");
        string fullPath = UtilMethod.ConnectStrs(CommonPathUtils.getLoadRootDir(packageName, relativePath), relativePath);
        Loger.PrintLog(UtilMethod.ConnectStrs("加载资源：", fullPath));
        AssetBundle ab = AssetBundle.LoadFromFile(fullPath);
        m_dict.Add(packageName, ab);
    }


    delegate void ShowAlert(string title,string msg,Action onCallBack);
    ShowAlert showAlert;
    public void Alert(string title, string msg, Action onCallBack)
    {
        if(showAlert==null)
        {
            showAlert = luaEvn.Global.Get<ShowAlert>("Alert.showAlertMsg");
        }
        showAlert(title, msg, onCallBack);
    }

}
