using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ProtoBufSpace;
using XLua;

[LuaCallCSharp]
public class UIManager : Singleton<UIManager>
{
    public bool isInit = false;

    public GameObject UIRoot;

    private EventSystem esys;

    public Camera UICamera;

    public const float GlobalUIWidth = 1080f;
    public const float GlobalUIHigh = 1920f;

    public float UIRootWidthValue
    {
        get
        {
            return GlobalUIWidth;
        }
    }


    public float UIRootHighValue
    {
        get
        {
            return GlobalUIWidth * Screen.height / Screen.width;

        }
    }



    public void Init(GameObject root)
    {
        UIRoot = AddUICanvas(root);

    }

    #region 代码创建主UIRoot

    GameObject AddUICanvas(GameObject root)
    {
        if (isInit) return UIRoot;
        isInit = true;
        var go = CreateNewUI(root);
        go.transform.SetParent(root.transform);
        return go;
    }
    GameObject CreateNewUI(GameObject parent)
    {
        // Root for the UI
        GameObject rootGo = new GameObject("UIRoot");
        rootGo.layer = LayerMask.NameToLayer("UI");
        CreateEventSystem(rootGo);
        CreateUICamera(rootGo);

        return rootGo;
    }
    void CreateEventSystem(GameObject root)
    {
        if (esys == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.layer = LayerMask.NameToLayer("UI");
            eventSystem.transform.SetParent(root.transform);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            //TouchInputModule touchInputModule = eventSystem.AddComponent<TouchInputModule>();
            //touchInputModule.forceModuleActive = true;
        }
    }
    void CreateUICamera(GameObject root)
    {
        GameObject uiCameraGo = new GameObject("UICamera");
        uiCameraGo.transform.SetParent(root.transform);
        UICamera = uiCameraGo.AddComponent<Camera>();
        UICamera.depth = 50;
        UICamera.orthographic = true;
        UICamera.clearFlags = CameraClearFlags.Depth;
        UICamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
        UICamera.cullingMask |= 1 << LayerMask.NameToLayer("UI_Effect");
        UICamera.nearClipPlane = -10000f;
        UICamera.farClipPlane = 2000f;



    }

    #endregion


    #region public



    IEnumerator Preload(Action OnLoadEnd)
    {
        List<String> loadList = new List<string>();
        int loadedValue = 0;
        for (int i = 0; i < loadList.Count; i++)
        {
            string value = loadList[i];
            //UILoadTool.Instance.CreateUI(value, (g) => { loadedValue++; }, false);
        }
        while (loadedValue != loadList.Count)
        {
            yield return 0;
        }

        if (OnLoadEnd != null)
        {
            OnLoadEnd.Invoke();
        }

    }


    //public void OnSceneChangeBefore(Action OnLoadEnd)
    //{
    //    //UIManager.Instance.GC();
    //    MainThread.Instance.StartCoroutine(Preload(OnLoadEnd));

    //}
    public Texture2D GetSnapshot(int x, int y, int w, int h)
    {
        if (UICamera == null) return null;

        float _xScale = Screen.width / 1080;
        float _yScale = Screen.height / 1920;
        float x_pix = _xScale * x;
        float y_pix = _yScale * y;

        RenderTexture tempRt = RenderTexture.GetTemporary(Screen.width, Screen.height);
        UICamera.targetTexture = tempRt;
        UICamera.Render();
        Texture2D tex2D = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = UICamera.targetTexture;
        tex2D.ReadPixels(new Rect(x_pix, y_pix, w, h), 0, 0);
        tex2D.Apply();
        UICamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRt);
        tempRt = null;
        return tex2D;
    }


    public Texture2D GetSnapshot(int w)
    {
        if (UICamera == null) return null;
        int h = Mathf.FloorToInt((float)w / UICamera.aspect);
        RenderTexture tempRt = RenderTexture.GetTemporary(w, h);
        UICamera.targetTexture = tempRt;
        UICamera.Render();
        Texture2D tex2d = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = UICamera.targetTexture;
        tex2d.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2d.Apply();
        UICamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRt);
        tempRt = null;
        return tex2d;
    }

    public Texture2D GetSnapshot(Vector2 size)
    {
        if (UICamera == null) return null;
        int w = Mathf.CeilToInt(size.x);
        int h = Mathf.CeilToInt(size.y);
        RenderTexture tempRt = RenderTexture.GetTemporary(w, h);
        UICamera.targetTexture = tempRt;
        UICamera.Render();
        Texture2D tex2D = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = UICamera.targetTexture;
        tex2D.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2D.Apply();
        UICamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRt);
        tempRt = null;
        return tex2D;
    }
    public Texture2D GetSnapshot()
    {
        if (UICamera == null) return null;
        int w = Mathf.CeilToInt(UIRootWidthValue * 0.25f);
        int h = Mathf.CeilToInt(UIRootHighValue * 0.25f);
        RenderTexture tempRt = RenderTexture.GetTemporary(w, h);
        UICamera.targetTexture = tempRt;
        UICamera.Render();
        Texture2D tex2D = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = UICamera.targetTexture;
        tex2D.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2D.Apply();
        UICamera.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRt);
        tempRt = null;
        return tex2D;
    }






    #endregion



    public void GC()
    {
        //UILoadTool.Instance.GetUIPoolProxy().GC();
        //HudManager.Instance.ClearPool();
    }


}
