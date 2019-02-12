using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

public class WWWManager : MonoBehaviour
{
    private static WWWManager m_instance;
    private Dictionary<string, WWWLoader> m_wwwLoaderDic = new Dictionary<string, WWWLoader>();

    public WWWManager()
    {
        m_instance = this;
    }

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (this.m_wwwLoaderDic.Count > 0)
        {
            List<WWWLoader> list = new List<WWWLoader>();
            foreach (KeyValuePair<string, WWWLoader> pair in this.m_wwwLoaderDic)
            {
                WWWLoader item = pair.Value;
                if ((item != null) && !item.isTimeOut)
                {
                    list.Add(item);
                }
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((i < list.Count) && (list[i] != null))
                {
                    list[i].checkTimeout();
                }
            }
        }
    }

    public void load(string url, Action<WWW> succeedCallback = null, Action<string> errorCallback = null, Action timeoutCallback = null, Action<float> progressCallback = null, bool isShowRetryDialog = false, int autoRetryCount = 0)
    {
        WWWLoader loader;
        if (this.m_wwwLoaderDic.ContainsKey(url))
        {
            loader = this.m_wwwLoaderDic[url];
            loader.isRetry = loader.isRetry || isShowRetryDialog;
            loader.succeedCallback = (Action<WWW>) Delegate.Combine(loader.succeedCallback, succeedCallback);
            loader.errorCallback = (Action<string>) Delegate.Combine(loader.errorCallback, errorCallback);
            loader.timeoutCallback = (Action) Delegate.Combine(loader.timeoutCallback, timeoutCallback);
            loader.progressCallback = (Action<float>) Delegate.Combine(loader.progressCallback, progressCallback);
        }
        else
        {
            loader = new WWWLoader(url) {
                autoRetryCount = autoRetryCount,
                isRetry = isShowRetryDialog
            };
            loader.succeedCallback = (Action<WWW>) Delegate.Combine(loader.succeedCallback, succeedCallback);
            loader.errorCallback = (Action<string>) Delegate.Combine(loader.errorCallback, errorCallback);
            loader.timeoutCallback = (Action) Delegate.Combine(loader.timeoutCallback, timeoutCallback);
            loader.progressCallback = (Action<float>) Delegate.Combine(loader.progressCallback, progressCallback);
            this.m_wwwLoaderDic.Add(url, loader);
            base.StartCoroutine(this.m_wwwLoaderDic[url].wwwLoad());
        }
    }

    public void removeLoader(string url)
    {
        if (this.m_wwwLoaderDic.ContainsKey(url))
        {
            this.m_wwwLoaderDic.Remove(url);
        }
    }

    public void retryLoader(string url)
    {
        if (this.m_wwwLoaderDic.ContainsKey(url))
        {
            if (this.m_wwwLoaderDic[url].isTimeOut)
            {
                this.m_wwwLoaderDic[url] = this.m_wwwLoaderDic[url].clone();
            }
            base.StartCoroutine(this.m_wwwLoaderDic[url].wwwLoad());
        }
    }

    public static WWWManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                if (GameObject.Find("WWWManager") != null)
                {
                    m_instance = GameObject.Find("WWWManager").GetComponent<WWWManager>();
                }
                if (m_instance == null)
                {
                    m_instance = new GameObject("WWWManager").AddComponent<WWWManager>();
                }
            }
            return m_instance;
        }
    }
}

