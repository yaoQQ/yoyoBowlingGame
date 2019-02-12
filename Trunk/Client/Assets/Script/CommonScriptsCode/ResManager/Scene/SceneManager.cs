using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;
using System.Text;

[LuaCallCSharp]
public class SceneManager : Singleton<SceneManager>
{
    SceneResProxy resProxy = new SceneResProxy();

    public SceneResProxy ResProxy
    {
        get
        {
            return resProxy;
        }
    }

    public void Init()
    {
        

    }

    LuaScene curLuaScene;
    SceneEntity curSceneEntity=null;

    public bool HasSceneEntity()
    {
        return curSceneEntity != null;
    }

    public SceneEntity GetSceneEntity()
    {
        return curSceneEntity;
    }

    /// <summary>
    /// 重置当前场景
    /// </summary>
    public void Reset()
    {
        if(curSceneEntity!=null)
        {
            curSceneEntity.Reset();
            if (curLuaScene!=null)
            {
                curLuaScene.onReset();
            }
        }
    }


    public void Change(LuaScene luaScene, Action onChangeEnd = null)
    {
        curLuaScene = luaScene;
        MainThread.Instance.StartCoroutine(AsynLoadScene(curLuaScene, onChangeEnd));
    }

    IEnumerator AsynLoadScene(LuaScene luaScene, Action onChangeEnd)
    {
        //Debug.LogError("AsynLoadScene   " + luaScene.getSceneName());
        //清掉原来的场景
        Clear();
        //yield return new WaitForSeconds(2);

        while (ResProxy.GetSceneData(luaScene.getSceneName()) == null)
        {
            yield return 0;
        }
        while (ResProxy.GetSceneManifest(luaScene.getSceneName()) == null)
        {
            yield return 0;
        }
        CreateSceneEntity(ResProxy.GetSceneData(luaScene.getSceneName()));

        while (!curSceneEntity.InitSign)
        {
            yield return 0;
        }

        if(onChangeEnd!=null)
        {
            onChangeEnd();
        }

        OnLoadedScene();
        
    }

    void OnLoadedScene()
    {
        //LoadingBarController.Instance.Show(false);

        curLuaScene.onEnter();

    }

    void CreateSceneEntity(SceneData sceneData)
    {
        GameObject sceneRoot = new GameObject("[Scene]");
        curSceneEntity = sceneRoot.AddComponent<SceneEntity>();
        curSceneEntity.Init(curLuaScene.getSceneName(),sceneData);

    }
   



    public void Clear()
    {
        if (curLuaScene!=null)
        {
            curLuaScene.onLeave();
        }
        if (curSceneEntity!=null)
        {
            curSceneEntity.Del();

        }
    }





    public void CreateSceneCell(string cellName, LuaTable param, Action<SceneCell, LuaTable> onLoadend )
    {
        MainThread.Instance.StartCoroutine(AsyncCreatePrefab(cellName, param, onLoadend));
    }
    IEnumerator AsyncCreatePrefab(string cellName, LuaTable param, Action<SceneCell, LuaTable> onLoadend)
    {
        string sceneName = curLuaScene.getSceneName();
        string abRelativePath = UtilMethod.ConnectStrs("scene/", sceneName, "/prefab/", cellName, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.Scene, sceneName, abRelativePath, (relativePath, res) =>
        {
            if (onLoadend != null)
            {
                GameObject go = GameObject.Instantiate(res as GameObject);
                SceneCell sc = go.GetComponent<SceneCell>();
                if (sc != null)
                    onLoadend.Invoke(sc, param);
                else
                    Debug.LogError("实例化的场景对象不能没有SceneCell：  " + cellName);
            }
        });
        yield return null;
    }
}
