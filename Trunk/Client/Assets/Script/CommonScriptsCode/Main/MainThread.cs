using UnityEngine;

public class MainThread : MonoBehaviour
{

    static MainThread _instance;

    public static MainThread Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;

        Init();
    }

    GameObject dontDestory;
    void Init()
    {
        LoadingBarController.isAutoClose = true;
        LoadingBarController.SetProgress(0.9f, 60);

        AddDstObj();
        AudioManager.Instance.Init(dontDestory);
        ModuleManager.Instance.Init();
        LoaderManager.Instance.Init();
        UIManager.Instance.Init(dontDestory);

        GameObject go = UIViewManager.Instance.CreateUILayerPanel((int)UIViewType.Loading_View, "LayerPanel_Loading");
        LoadingBarController.SetContainer(go);

        InputManager.Instance.Init();

        //test
        MyTest.CXTest.Instance.init();

        LuaManager.Instance.Init();
    }

    void SendConnectSocket()
    {
        ConnectSocketNotice notice = new ConnectSocketNotice()
        {
            ip = LuaManager.Instance.GetGlobalValue<string>("GameConfig_socketIP"),
            port = LuaManager.Instance.GetGlobalValue<int>("GameConfig_socketPort")
        };
        //Debug.Log("SendConnectSocket");
        NoticeManager.Instance.Dispatch(notice);
    }

    void AddDstObj()
    {
        dontDestory = new GameObject("DST");
        dontDestory.tag = "DST";
        DontDestroyOnLoad(dontDestory);
    }


    float deltaTime_ms = 0f;
    void Update() {
        NetworkManager.Instance.OnProcess();
        LoaderManager.Instance.Update();
        deltaTime_ms = GlobalTimeManager.Instance.Execute();
        //Debug.Log("deltaTime_ms    " + deltaTime_ms);
        LuaManager.Instance.Execute();
        InputManager.Instance.Execute();
        LuaActionController.Instance.Execute(deltaTime_ms);
        AudioManager.Instance.AdjustVolume();
        PhysicGameManager.Instance.Update();
        //MyTest.CXTest.Instance.Execute();
    }

    public void LateUpdate() {
        PhysicGameManager.Instance.LateUpdate();
    }

    void OnGUI() {
        // GMManager.Instance.GMRender();
    }

    void OnDestroy()
    {
        LuaManager.Instance.OnDestroy();
    }

    void OnApplicationQuit()
    {
        NetworkManager.Instance.OnApplicationQuit();
        StopAllCoroutines();
        //Loger.IsStop = true;
    }
}