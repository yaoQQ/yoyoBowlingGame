using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class InputManager : Singleton<InputManager>
{

#if UNITY_STANDALONE
    [BlackList]
    public MouseHandler mouseHandler;
    [BlackList]
    public KeyboardHandler keyboardHandler;
#endif
#if UNITY_IPHONE
     [BlackList]
	public TouchHandler touchHandler;
#endif

    bool isCanOperate = true;

    PointerEventData curEventData;

    public InputManager()
    {
#if UNITY_STANDALONE
        mouseHandler = new MouseHandler();
        keyboardHandler = new KeyboardHandler();
#endif
#if UNITY_IPHONE
		touchHandler = new TouchHandler ();
#endif

    }

    /// <summary>
    /// 外层每帧调用;
    /// </summary>
    public void Execute()
    {
        if (!isCanOperate) return;
        //if (curEventData != null)
        //{
        //    OnDragHandler(curEventData);

        //}
#if UNITY_STANDALONE
        //mouseHandler.Execute();
        keyboardHandler.Execute();
#endif
#if UNITY_IPHONE
        //touchHandler.Execute ();
#endif
    }



    GameObject controlRootGO;
    UITools.CanvasInfo canvasInfo;
    Mid_base_control_panel mid_Control;


    public void Init()
    {
        CreatControlRoot(UIManager.Instance.UIRoot);
    }

    void CreatControlRoot(GameObject uiRoot)
    {
        controlRootGO = new GameObject("LayerPanel_Control");
        controlRootGO.transform.localScale = Vector3.one;
        canvasInfo = UITools.SetCanvasToUIGo(controlRootGO);
        canvasInfo.graphicRaycaster.enabled = true;
        canvasInfo.canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasInfo.canvasScaler.matchWidthOrHeight = 0f;
        canvasInfo.canvas.sortingOrder = UISortOrder.PLAYER_CONTROL;
        UITools.SetParentAndAlign(controlRootGO, uiRoot);

        UILoadControl.Instance.CreateUI("base","base_control_panel", OnViewLoadEnd);

    }
    void OnViewLoadEnd(string name, GameObject go)
    {
        UITools.SetParentAndAlign(go, controlRootGO);
        RectTransform rect = go.transform as RectTransform;
        rect.sizeDelta = new Vector2(0, 0);
        mid_Control = new Mid_base_control_panel(go);
        InitListener(mid_Control.control_img.gameObject);
    }

    void InitListener(GameObject screenGO)
    {
        //DragEventHandler.Get(screenGO).onBeginDragHandler = OnBeginDragHandler;
        //DragEventHandler.Get(screenGO).onDragHandler = OnDragHandler;
        //DragEventHandler.Get(screenGO).onEndDragHandler = OnEndDragHandler;
        //PointerDownListener.Get(screenGO).onHandler = OnBeginDragHandler;
        //PointerUpListener.Get(screenGO).onHandler = OnEndDragHandler;

        PointerClickListener.Get(screenGO).onHandler = OnScreenClickHandler;
    }

    /// <summary>
    /// 主要用于选中3D物体;
    /// </summary>
    void OnScreenClickHandler(PointerEventData eventData)
    {
        if (!isCanOperate) return;
        ExecuteClickScreenHandler(eventData);
        //GlobalTimeManager.Instance.timerController.AddTimer("OnScreenClickHandler", 100, 1, (i) => { ExecuteClickScreenHandler(eventData); });
    }

   

    void ExecuteClickScreenHandler(PointerEventData eventData)
    {
        if (!isCanOperate) return;
        if (!SceneManager.Instance.HasSceneEntity()) return;
        SceneCamera[] sceneCameraArr = SceneManager.Instance.GetSceneEntity().GetAllCamera();
        if (sceneCameraArr == null) return;
        for(int i=0;i< sceneCameraArr.Length;i++)
        {
            SceneCamera sceneCamera = sceneCameraArr[i];
            Ray ray = sceneCamera.GetComponent<Camera>().ScreenPointToRay(eventData.pointerCurrentRaycast.screenPosition);
            //RaycastHit[] hitInfoArr = Physics.RaycastAll(ray, 10000, 1 << LayerMask.NameToLayer("Hero"));
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, 10000, sceneCamera.cameraInfo.cullingMask))
            {
                Collider collider = hitInfo.collider;
                SceneCell sc = FindEventCell(collider.gameObject);
                if(sc!=null)
                {
                    SceneClickNotice sceneClickNotice = new SceneClickNotice();
                    sceneClickNotice.cameraName = sceneCamera.cameraName;
                    sceneClickNotice.sceneCell = sc;
                    NoticeManager.Instance.Dispatch(NoticeType.Scene_Click_Event, sceneClickNotice);
                    return;
                }
            }
        }
        SceneClickNotice nullClickNotice = new SceneClickNotice();
        nullClickNotice.cameraName = "";
        nullClickNotice.sceneCell = null;
        NoticeManager.Instance.Dispatch(NoticeType.Scene_Click_Event, nullClickNotice);

    }
    SceneCell FindEventCell(GameObject go)
    {
        SceneCell sc = go.GetComponent<SceneCell>();
        if(sc==null&&go.transform.parent!=null)
        {
            return FindEventCell(go.transform.parent.gameObject);
        }
        else
        {
            return sc;
        }
    }
}
