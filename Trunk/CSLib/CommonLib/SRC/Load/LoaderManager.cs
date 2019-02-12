using UnityEngine;
using System.Collections.Generic;

public class LoaderManager
{
    public delegate void LoadedFinishDelegate(string resName, System.Object res);

    public class LoadOrder
    {
        public LoaderType orderType;
        public string packageName;
        public string resUrl;
        public LoadedFinishDelegate OnLoadFinish;

        public void AppendFinishDelegate(LoadedFinishDelegate onFinish)
        {
            if (OnLoadFinish != null)
            {
                OnLoadFinish += onFinish;
            }
            else
            {
                Logger.Print(LogType.Error, "上层管理出现问题", onFinish.ToString());
            }
        }
    }

    private static LoaderManager m_instance;

    public static LoaderManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new LoaderManager();
            }
            return m_instance;
        }
    }

    const int abLoaderMaxNum = 50;
    const int textLoaderMaxNum = 50;

    //排队的订单
    Queue<LoadOrder> abOrderQueue = new Queue<LoadOrder>();
    Queue<LoadOrder> textOrderQueue = new Queue<LoadOrder>();

    //池
    Queue<ABLoader> abLoaderPool = new Queue<ABLoader>();
    Queue<TextLoader> textLoaderPool = new Queue<TextLoader>();


    /// <summary>
    /// 所有订单，防止重复加载相同的资源 
    /// </summary>
    Dictionary<string, LoadOrder> allOrderDic = new Dictionary<string, LoadOrder>();

    /// <summary>
    /// 正在运行的加载器
    /// </summary>
    List<BaseLoader> allLoaderSet = new List<BaseLoader>();

    public void Init()
    {
        //初始化加载器
        while (abLoaderPool.Count < abLoaderMaxNum)
        {
            abLoaderPool.Enqueue(new ABLoader());
        }
        while (textLoaderPool.Count < textLoaderMaxNum)
        {
            textLoaderPool.Enqueue(new TextLoader());
        }
    }


    public void Update()
    {
        //输出一下加载器概况
        PrintReport(false);

        //启动加载
        TryExecuteQueue();

        //进行加载
        ExecuteLoad();
    }

    void ExecuteLoad()
    {
        for (int i = allLoaderSet.Count - 1; i >= 0; i--)
        {
            BaseLoader bl = allLoaderSet[i];
            bl.RunDown();
            if (bl.CheckFinish())
            {
                allLoaderSet.RemoveAt(i);
                bl.EndDown();
                RecycleLoaderPool(bl);
            }
        }
    }

    //回收函数
    void RecycleLoaderPool(BaseLoader bl)
    {
        if (bl is ABLoader)
        {
            abLoaderPool.Enqueue(bl as ABLoader);
        }
        else if (bl is TextLoader)
        {
            textLoaderPool.Enqueue(bl as TextLoader);
        }
    }

    void TryExecuteQueue()
    {
        while (abOrderQueue.Count > 0 && abLoaderPool.Count > 0)
        {
            ABLoader abLoader = abLoaderPool.Dequeue();

            StartLoad(abLoader, abOrderQueue.Dequeue());
        }
        while (textOrderQueue.Count > 0 && textLoaderPool.Count > 0)
        {
            TextLoader textLoader = textLoaderPool.Dequeue();

            StartLoad(textLoader, textOrderQueue.Dequeue());
        }
    }
    void StartLoad(BaseLoader loader, LoadOrder order)
    {

        allLoaderSet.Add(loader);
        loader.StartDown(order);
    }

    void PrintReport(bool sign)
    {
        if (!sign) return;
        Logger.Print(LogType.Log, "# LoaderManager # 正在排队的AB加载订单数：", abOrderQueue.Count.ToString());
        Logger.Print(LogType.Log, "# LoaderManager # 正在排队的Text加载订单数：", abOrderQueue.Count.ToString());
        Logger.Print(LogType.Log, "# LoaderManager # ABLoader空闲数目：", abLoaderPool.Count.ToString());
        Logger.Print(LogType.Log, "# LoaderManager # TextLoader空闲数目：", textLoaderPool.Count.ToString());
    }


    public void AddOrderPB(string packageName, string url, LoadedFinishDelegate finishDelegate)
    {
        //AddOrder(LoaderType.PB, url, finishDelegate);
        ResLoadManager.LoadAsync(AssetType.PB, packageName, url, (relativePath, res) =>
        {
            finishDelegate(relativePath, res);
        });
    }


    public void AddOrder(LoaderType loadtype, string url, LoadedFinishDelegate finishDelegate)
    {
        //Logger.PrintLog("加载的路径 ", url);
        //如果要加载的资源已在队列或已在加载，那么追加回调。不加订单
        LoadOrder order;
        if (allOrderDic.TryGetValue(url, out order))
        {
            order.AppendFinishDelegate(finishDelegate);
        }
        else
        {
            //不存在，新增订单
            order = new LoadOrder();
            order.orderType = loadtype;
            order.resUrl = url;
            order.OnLoadFinish = finishDelegate;
            switch (order.orderType)
            {
                case LoaderType.AssetBundle:
                    abOrderQueue.Enqueue(order);
                    break;
                case LoaderType.Text:
                    textOrderQueue.Enqueue(order);
                    break;
                case LoaderType.AssetBundleManifest:
                    abOrderQueue.Enqueue(order);
                    break;
                case LoaderType.PB:
                    textOrderQueue.Enqueue(order);
                    break;
            }
            allOrderDic.Add(url, order);
        }
    }
    

    public void UnlockAB(AssetBundle ab)
    {
        Logger.Print(LogType.Log, "UnlockAB   :", ab.name);
        ab.Unload(false);
    }

}
