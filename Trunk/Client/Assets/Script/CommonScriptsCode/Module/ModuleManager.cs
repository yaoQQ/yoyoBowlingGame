
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;

[LuaCallCSharp]
public class ModuleManager : Singleton<ModuleManager>
{
    Dictionary<ModuleEnum, BaseModule> moduleDic = new Dictionary<ModuleEnum, BaseModule>();
    Dictionary<Type, BaseModule> moduleTypeDic = new Dictionary<Type, BaseModule>();

    Dictionary<string, LuaModule> luaModuleDic = new Dictionary<string, LuaModule>();


    public void Init()
    {
        InitModule();

    }
    /// <summary>
    /// 每个模块在此注册一次;
    /// </summary>
    void InitModule()
    {
        RegisterModule<CommonModule>();
    }

    void RegisterModule<T>() where T : BaseModule
    {
        if (moduleTypeDic.ContainsKey(typeof(T)))
        {
            Debug.LogError("重复注册模块" );
            return;
        }
        BaseModule bm  = (BaseModule)Activator.CreateInstance(typeof(T), true);
        moduleDic.Add(bm.ModuleName(),bm);
        moduleTypeDic.Add(bm.GetType(),bm);
    }

    public void RegisterLuaModule(LuaModule luaModule) 
    {
        string moduleName = luaModule.getModuleName();
        if (!luaModuleDic.ContainsKey(moduleName))
        {
            luaModule.initRegisterNet();
            luaModuleDic.Add(moduleName, luaModule);
        }
    }


    public void ExecuteNotificationHandle(string noticeType, BaseNotice vo)
    {
        foreach (BaseModule bm in moduleDic.Values)
        {
            List<string> notificationlist = bm.GetRegisterNotificationList();
            if (notificationlist.Contains(noticeType))
            {
                bm.OnNotificationLister(noticeType, vo);
            }
        }

        var buffer = new List<LuaModule>(luaModuleDic.Values);
        var enumerator = buffer.GetEnumerator();
         
        while (enumerator.MoveNext())
        {
            LuaModule lm = enumerator.Current;
            List<string> notificationlist = lm.getRegisterNotificationList();
            if (notificationlist.Contains(noticeType))
            {
                lm.onNotificationLister(noticeType, vo);
            }
        }
    }



    /// <summary>  
    /// 获取某个模块实例;
    /// </summary>
    public T GetModule<T>() where T : BaseModule
    {
        BaseModule bm = null;
        moduleTypeDic.TryGetValue(typeof(T),out bm);
        if (bm == null)
        {
            Debug.LogError("获取不存在的模块");
        }
        return (T)bm;
    }

    
}
