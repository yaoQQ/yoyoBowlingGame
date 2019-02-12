using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour 
{

    public Rect windowRect;

    Rect toolbarRect = new Rect(10, 20, 500, 20);
    int toolbarIndex = 0;
    GUI.WindowFunction[] windowMethods;
    string[] tabs = new string[]{ "标签1", "标签2", "标签3" };

    Rect scrollRect = new Rect(5, 45, 590, 390);
    Rect innerRect = new Rect(0, 0, 570, 100);
    Vector2 scrollPos = Vector2.zero;
    int innerHeight = 0;


    Rect controlsRect = new Rect(0, 0, 160, 22);

    Rect closeButtonRect = new Rect(270, 440, 60, 20);

	// Use this for initialization
	void Start () 
    {
        windowMethods = new GUI.WindowFunction[] { OnTab1, OnTab2, OnTab3 };
        windowRect = new Rect(Screen.width / 2 - 300, 30, 600, 470);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    // This function is called when the object becomes enabled and active
    public void OnEnable()
    {
        //_windowRect = new Rect(Screen.width / 2 - 300, 30, 600, 470);
    }

    // This function is called when the behaviour becomes disabled or inactive
    public void OnDisable()
    {

    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {

    }

    // OnGUI is called for rendering and handling GUI events
    public void OnGUI()
    {
        windowRect = GUI.Window(-1500, windowRect, windowMethods[toolbarIndex], "test_window");
        GUI.BringWindowToFront(-1500);


    }

    void DrawCommonControlBegin()
    {
        var index = GUI.Toolbar(toolbarRect, toolbarIndex, tabs);

        if (index != toolbarIndex)
        {
            toolbarIndex = index;
        }

        GUI.Box(scrollRect, string.Empty);

        innerHeight = 1000;

        innerRect.height = innerHeight < scrollRect.height ? scrollRect.height : innerHeight;
        scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, innerRect);
    
    }

    void DrawCommonControlEnd()
    {
        GUI.EndScrollView();

        if (GUI.Button(closeButtonRect, "Close"))
        {
            enabled = false;
        }

        GUI.DragWindow();
    }



    void OnTab1(int id)
    {
        DrawCommonControlBegin();

        controlsRect.x = 30;
        controlsRect.y = 10;
        if (GUI.Button(controlsRect, "test1"))
        {
            UIViewManager.Instance.CloseStackTopView();
        }

        controlsRect.x = controlsRect.x + controlsRect.width + 10;
        //controlsRect.y = controlsRect.y + controlsRect.height + 10;
        if (GUI.Button(controlsRect, "test2"))
        {
            Debug.LogWarning("test2");
        }

        controlsRect.x = controlsRect.x + controlsRect.width + 10;
        //controlsRect.y = controlsRect.y + controlsRect.height + 10;
        if (GUI.Button(controlsRect, "test3"))
        {
            Debug.Log("test3");
        }

        controlsRect.x = 30;
        controlsRect.y = controlsRect.y + controlsRect.height + 10;
        if (GUI.Button(controlsRect, "test4"))
        {
         /*  ScreenSnapManager.Instance.StartScreenSnap(
              0,
              0,
              1080,
              1920,
            captureScreen
          );*/
        }

        DrawCommonControlEnd();
    }
    private void captureScreen() {
        Loger.PrintLog("IOS:截图成功");
        PlatformSDK.WxShare(
                      1, 1,
                      ScreenSnapManager.Instance.GetSnapPath(),
                      "", "", "",
                      succeedCallback,
                      cancelCallback,
                      failCallback
                  );
      
    }
    private void succeedCallback() {
        Loger.PrintLog("IOS:上传成功");
    }
    private void cancelCallback() {
        Loger.PrintLog("IOS:上传取消");
    }
    private void failCallback() {
        Loger.PrintLog("IOS:上传失败");
    }

    void OnTab2(int id)
    {
        DrawCommonControlBegin();

        DrawCommonControlEnd();
    }

    void OnTab3(int id)
    {
        DrawCommonControlBegin();

        DrawCommonControlEnd();
    }

}
