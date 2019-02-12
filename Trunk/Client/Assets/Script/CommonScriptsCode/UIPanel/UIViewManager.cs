using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XLua;
using DG.Tweening;

[LuaCallCSharp]
public class UIViewManager : Singleton<UIViewManager>
{
    private class UIView
    {
        public string packageName;
        public LuaUIView luaUIView;
    }
    private Dictionary<int, GameObject> m_UILayerPanelDict = new Dictionary<int, GameObject>();
    private Dictionary<int, UIView> m_UIViewDict = new Dictionary<int, UIView>();

    /// <summary>
    /// 创建UI层级面板
    /// </summary>
    public GameObject CreateUILayerPanel(int layerIndex, string name)
    {
        if (m_UILayerPanelDict.ContainsKey(layerIndex))
        {
            Logger.PrintError(CommonUtils.ConnectStrs("与 ", name, " 相同序号的层级已存在"));
            return null;
        }
        //Logger.PrintLog(CommonUtils.ConnectStrs("创建层级(", layerIndex.ToString(), "):", name));
        GameObject uiLayerPanel = new GameObject(name);
        UITools.CanvasInfo canvasInfo = UITools.SetCanvasToUIGo(uiLayerPanel);
        canvasInfo.SetCanvasParam(layerIndex);

        GameObject uiRoot = UIManager.Instance.UIRoot;
        UITools.SetParentAndAlign(uiLayerPanel, uiRoot);

        m_UILayerPanelDict[layerIndex] = uiLayerPanel;
        return uiLayerPanel;
    }

  
    public void SwitchScreenOrientation(bool isHorizontal)
    {
        foreach(GameObject go in m_UILayerPanelDict.Values)
        {
            UITools.SwitchScreenOrientation(go, isHorizontal);
        }
    }

    public void SetCanvasMatch(int uiViewType, float value)
    {
        if (m_UILayerPanelDict.ContainsKey(uiViewType))
            UITools.SetCanvasMatch(m_UILayerPanelDict[uiViewType], value);
    }

    /// <summary>
    /// 注册栈底界面（主界面）
    /// </summary>
    public void RegisterStackButtomView(int viewEnum)
    {
        UIViewStack.RegisterStackButtomView(viewEnum);
    }

    /// <summary>
    /// 注册界面
    /// </summary>
    public void RegisterView(string packageName, LuaUIView view)
    {
        int viewEnum = view.getViewEnum();
        if (m_UIViewDict.ContainsKey(viewEnum))
            return;
        
        int layerIndex = view.getViewType();
        if (!m_UILayerPanelDict.ContainsKey(layerIndex))
        {
            Logger.PrintError(CommonUtils.ConnectStrs("UI对应的层级(", layerIndex.ToString(), ")不存在：", viewEnum.ToString()));
            return;
        }

        UIView uiView = new UIView();
        uiView.packageName = packageName;
        uiView.luaUIView = view;
        m_UIViewDict.Add(viewEnum, uiView);
        GameObject go = m_UILayerPanelDict[layerIndex];
        view.setContainerGO(go);
    }

    /// <summary>
    /// 预加载界面
    /// </summary>
    public void Preload(int viewEnum, Action preloadCallback = null)
    {
        UIView uiView;
        m_UIViewDict.TryGetValue(viewEnum, out uiView);

        if (uiView == null)
        {
            Debug.LogError(viewEnum.ToString() + " 窗口没有注册");
            return;
        }
        if (uiView.luaUIView.getIsOpen())
        {
            preloadCallback();
            return;
        }

        if (!uiView.luaUIView.getIsLoaded())
        {
            if (!uiView.luaUIView.getIsLoading())
            {
                uiView.luaUIView.startLoad();
            }
        }
        uiView.luaUIView.setOpening(false);
        MainThread.Instance.StartCoroutine(AsynOpen(uiView.luaUIView, null, preloadCallback));
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    public void Open(int viewEnum, object msg = null, Action openCallback = null, bool isPush = true)
    {
        //Logger.PrintLog("尝试打开界面：" + viewEnum);
        UIView uiView;
        m_UIViewDict.TryGetValue(viewEnum, out uiView);

        if (uiView == null)
        {
            Debug.LogError(viewEnum.ToString() + " 窗口没有注册");
            return;
        }

        //打开界面时，如果是栈界面，就尝试入栈
        if (isPush && uiView.luaUIView.getIsStackView())
            UIViewStack.Push(viewEnum, msg, openCallback);

        if (!uiView.luaUIView.getIsLoaded())
        {
            if (!uiView.luaUIView.getIsLoading())
            {
                //Debug.LogError(viewEnum.ToString() + " 开启=======================================================");
                uiView.luaUIView.startLoad();
            }
        }
        uiView.luaUIView.setOpening(true);

        bool stateBarColor = uiView.luaUIView.getStateBarWhiteColor();
        PlatformSDK.SetStatusBarColor(stateBarColor);
        MainThread.Instance.StartCoroutine(AsynOpen(uiView.luaUIView, msg, openCallback));
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public void Close(int viewEnum, bool isDel = true, bool isAnim = true)
    {
        UIView uiView;
        m_UIViewDict.TryGetValue(viewEnum, out uiView);
        if (uiView == null)
        {
            Debug.LogError(viewEnum.ToString() + " 窗口没有注册");
            return;
        }

        //关闭界面时，如果是栈界面且是栈顶界面，就尝试先打开栈顶的下一个界面
        if (uiView.luaUIView.getIsStackView())
        {
            OpenViewInfo stackTop = UIViewStack.GetStackTop();
            if (stackTop != null && stackTop.viewEnum == viewEnum)
            {
                UIViewStack.Pop();
                stackTop = UIViewStack.GetStackTop();
                if (stackTop != null)
                {
                    Open(stackTop.viewEnum, stackTop.msg, stackTop.openCallback);
                    return;
                }
            }
        }

        uiView.luaUIView.setOpening(false);
        if (uiView.luaUIView.getIsOpen())
            uiView.luaUIView.hide();
    }

    public void CloseAllView()
    {
        UIViewStack.ClearStack();
        UIViewStack.ClearCloseList();

        foreach (UIView uiView in m_UIViewDict.Values)
        {
            uiView.luaUIView.setOpening(false);
            if (uiView.luaUIView.getIsOpen())
                uiView.luaUIView.hide();
        }
    }

    public void SaveStackAndCloseAllView()
    {
        UIViewStack.SaveStack();
        CloseAllView();
    }

    public void CloseAllViewAndRevertStack()
    {
        CloseAllView();
        UIViewStack.RevertStack();
        OpenViewInfo stackTop = UIViewStack.GetStackTop();
        Open(stackTop.viewEnum, stackTop.msg, stackTop.openCallback, false);
    }

    public void MoveZoomAllView(Vector2 pivotV2, Vector3 value, float time)
    {
        Vector3 scaleValue = Vector3.one;
        Tweener scaleTween = DOTween.To(() => scaleValue, x => scaleValue = x, value, time);
        scaleTween.OnUpdate(() => ZoomAllView(pivotV2, scaleValue));
    }

    public void ZoomAllView(Vector2 pivotV2, Vector3 value)
    {
        foreach (UIView uiView in m_UIViewDict.Values)
        {
            if (uiView.luaUIView.getViewType() != (int)UIViewType.Alert_box)
            {
                if (uiView.luaUIView.getIsLoaded())
                {
                    RectTransform rt = uiView.luaUIView.getViewGO().GetComponent<RectTransform>();
                    rt.pivot = pivotV2;
                    rt.localScale = value;
                }

            }
        }
    }
    
    private IEnumerator AsynOpen(LuaUIView uiView, object msg, Action callback)
    {
        while (!uiView.getIsLoaded())
        {
            yield return null;
        }
        if (uiView.getOpening())
        {
            //显示界面时，如果是栈界面
            if (uiView.getIsStackView())
            {
                //先判断加载完界面时该界面还是不是栈顶界面
                OpenViewInfo stackTop = UIViewStack.GetStackTop();
                if (stackTop != null && stackTop.viewEnum != uiView.getViewEnum())
                {
                    uiView.setOpening(false);
                    GameObject go = uiView.getViewGO();
                    if (go != null)
                        go.SetActive(false);
                    yield break;
                }
                
                //关闭准备关闭的界面列表
                List<int> closeList = UIViewStack.GetCloseList();
                for (int i = 0, count = closeList.Count; i < count; ++i)
                {
                    if (closeList[i] != uiView.getViewEnum())
                    {
                        Close(closeList[i]);
                    }
                }
                UIViewStack.ClearCloseList();
            }
            //Logger.PrintLog("显示界面：" + uiView.getViewEnum());
            
            uiView.show(msg);
            if (callback != null)
            {
                callback.Invoke();
            }
            yield return null;
        }
        else
        {
            //不需要打开窗口
            GameObject go = uiView.getViewGO();
            if (go != null)
                go.SetActive(false);
            if (callback != null)
                callback.Invoke();
        }
    }

    [BlackList]
    /// <summary>
    /// 销毁一个包的所有界面
    /// </summary>
    /// <param name="packageName"></param>
    public void DestroyPackageView(string packageName)
    {
        foreach (UIView uiView in m_UIViewDict.Values)
        {
            if (uiView.packageName == packageName)
                DestroyView(uiView.luaUIView);
        }
    }

    private void DestroyView(LuaUIView luaUIView)
    {
        if (luaUIView.getIsLoaded())
        {
            //加载完成的
            GameObject go = luaUIView.getViewGO();
            GameObject.DestroyImmediate(go);
            luaUIView.onDestroy();
        }
    }

    public void DestroyView(int viewEnum)
    {
        UIView uiView;
        m_UIViewDict.TryGetValue(viewEnum, out uiView);
        if (uiView == null)
        {
            Debug.LogError(viewEnum.ToString() + " 窗口没有注册");
            return;
        }

        LuaUIView luaUIView = uiView.luaUIView;
        if (luaUIView.getIsLoaded())
        {
            //加载完成的
            GameObject go = luaUIView.getViewGO();
            GameObject.DestroyImmediate(go);
            luaUIView.onDestroy();
        }
    }

    public void DestroyUIRes(string packageName, string relativePath)
    {
        string abRelativePath = UtilMethod.ConnectStrs("ui/", packageName, "/prefab/", relativePath, ".unity3d");
        //Logger.PrintLog(CommonUtils.ConnectStrs("卸载UI：", abRelativePath));
        AssetNodeManager.ReleaseNode(AssetType.UI, packageName, abRelativePath);
    }
    #region android设备返回
    private enum returnTypeEnum
    {
        CloseDirect = 0,//直接关闭
        CloseStack = 1,//堆栈界面关闭
        ExistApp = 2,//平台一级界面询问是否退出APP
        GameMain = 3,//游戏的主界面（麻将和弹一弹）询问是否返回平台
        GameCompetition = 4,//游戏中金币场和比赛场询问是否返回游戏主界面
        PlatformCompetition = 5,//   平台比赛中询问是否返回平台
        None //不操作
    }
    /// <summary>
    /// 获取顶层对象
    /// Main_view 到 Feedback_Tip 层
    /// </summary>
    private LuaUIView getCurrPopView() {
        Array arr = Enum.GetValues(typeof(UIViewType));
        for (int i = arr.Length - 1; i >= 0; i--) {
            int index = (int)arr.GetValue(i);

            //其他层级排除
            if (index >= (int)UIViewType.Feedback_Tip || index < (int)UIViewType.Main_view) {
                continue;
            }

            if (!m_UILayerPanelDict.ContainsKey(index)) {
                continue;
            }
            //获取顶层对象界面
            Transform popView = getPopHaveActive(index);
            if (popView == null) {
                //当前层不存在界面
               // Debug.Log("@@index=" + index + "  @@@@@not have popView");
                continue;
            }
            PanelWidget panel = popView.GetComponent<PanelWidget>();
            int viewEnum = panel.UIViewEnum;
            int loginScene = 1001; //UIViewEnum.LoginView
            if (viewEnum <= loginScene) {//登入界面时不炒作
                return null;
            }
            if (!m_UIViewDict.ContainsKey(viewEnum)) {
               // Debug.LogError("getCurrPopView() did not have viewEnum=" + viewEnum);
                continue;
            }
            LuaUIView view = m_UIViewDict[viewEnum].luaUIView;
            return view;
        }

        return null;
    }
    /// <summary>
    /// 获取容器最上层界面
    /// </summary>
    /// <param name="viewType"></param>
    /// <returns></returns>
    private Transform getPopHaveActive(int viewType) {
        if (m_UILayerPanelDict.ContainsKey(viewType)) {
            GameObject LayoutGameObject = m_UILayerPanelDict[viewType];
            int childNum = LayoutGameObject.transform.childCount;
            for (int i = childNum - 1; i >= 0; i--) {
                Transform panel = LayoutGameObject.transform.GetChild(i);
                if (panel == null) {
                    continue;
                }
                if (panel.gameObject.activeSelf == true) {
                    return panel;
                }
            }
        }
        return null;
    }

    private returnTypeEnum GetDeviceReturnType(LuaUIView view) {
        int layerIndex = view.getViewType();
        if (!m_UILayerPanelDict.ContainsKey(layerIndex)) {
            Logger.PrintError(CommonUtils.ConnectStrs("UI对应的层级(", layerIndex.ToString(), ")不存在：", view.getViewEnum().ToString()));
            return returnTypeEnum.None;
        }
        UIViewType viewType = (UIViewType)layerIndex;
        //判断层级对应的 返回状态
        if (viewType > UIViewType.Feedback_Tip || viewType < UIViewType.Main_view || viewType == UIViewType.Loading_View) {
            return returnTypeEnum.None;
        }
        if (viewType == UIViewType.Main_view) {//平台一级界面询问是否退出APP
            return returnTypeEnum.ExistApp;
        }
        if (viewType == UIViewType.Platform_Second_View) {
          //  Debug.LogFormat("Platform_Second_View");
            if (view.getIsStackView()) {//堆栈界面
                if (UIViewStack.IsStackButtomView(view.getViewEnum())) {//最底层界面
                    return returnTypeEnum.ExistApp;
                }
                return returnTypeEnum.CloseStack;
            }
            return returnTypeEnum.CloseDirect;
        }

        //三级界面或弹窗
        if (viewType > UIViewType.Platform_Second_View && viewType <= UIViewType.Feedback_Tip) {
            return returnTypeEnum.CloseDirect;
        }

        return returnTypeEnum.None;
    }

    public void CloseStackTopView() {
        LuaUIView luaView = getCurrPopView();
        if (luaView == null) {
            return;
        }
        returnTypeEnum typeView = GetDeviceReturnType(luaView);
       // Debug.LogFormat("@@@@@@@@@@@@@@@@@@@ typeView=" + typeView.ToString() + " @@@@@@@@@@@@@@@@@@");
        switch (typeView) {
            case returnTypeEnum.CloseDirect:
                luaView.closeByEsc();
                break;
            case returnTypeEnum.CloseStack:
                luaView.closeByEsc();
                break;
            case returnTypeEnum.ExistApp:
                //Debug.LogFormat("@@@@@@@@@@@@@@@@@@@ ExistApp！！！ @@@@@@@@@@@@@@@@@@");
                //LoadingBarController.ShowNotice("是否返回平台？", "退出",
                //            () => Driver.Instance.QuitGame());
                // UIViewManager.Instance.Open(Alert, msg, openCallback)
                NoticeManager.Instance.Dispatch(NoticeType.Device_ReturnEvent, "是否退出APP?");
                break;
            case returnTypeEnum.GameCompetition:
                //LoadingBarController.ShowNotice("是否返回游戏主界面？", "退出",
                //           () => Driver.Instance.QuitGame());
                NoticeManager.Instance.Dispatch(NoticeType.Device_ReturnEvent, "是否返回游戏主界面？");
                break;
            case returnTypeEnum.GameMain:
                NoticeManager.Instance.Dispatch(NoticeType.Device_ReturnEvent, "是否返回平台?");
                break;
            case returnTypeEnum.PlatformCompetition:
                NoticeManager.Instance.Dispatch(NoticeType.Device_ReturnEvent, "是否返回平台?");
                break;

            case returnTypeEnum.None:
                break;
        }
    }
    #endregion

}
