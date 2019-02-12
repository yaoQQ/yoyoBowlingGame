using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WWWLoader
{
    private bool m_isTimeOut;
    public int autoRetryCount;
    public Action<string> errorCallback;
    public bool isRetry;
    public float lastLoadTime;
    public static Action<string> openRetryFunc;
    public float progress;
    public Action<float> progressCallback;
    public Action<WWW> succeedCallback;
    public Action timeoutCallback;
    public string url;
    public WWW www;

    public WWWLoader(string url)
    {
        this.url = url;
    }

    public static void SetRetryFunc(Action<string> func)
    {
        openRetryFunc = func;
    }

    public void checkTimeout()
    {
        if ((www != null) && !m_isTimeOut)
        {
            float num = Time.realtimeSinceStartup;
            float num2 = www.progress;
            if (progressCallback != null)
            {
                progressCallback(num2);
            }
            if (num2 > progress)
            {
                progress = num2;
                lastLoadTime = num;
            }
            else if ((num - lastLoadTime) > 15f)
            {
                Logger.PrintLog("加载(" + url + ")超时");
                m_isTimeOut = true;
                if (autoRetryCount > 0)
                {
                    autoRetryCount--;
                    WWWManager.Instance.retryLoader(url);
                }
                else if (isRetry)
                {
                    if (openRetryFunc != null)
                        openRetryFunc(url);
                }
                else
                {
                    if (timeoutCallback != null)
                    {
                        timeoutCallback();
                    }
                    WWWManager.Instance.removeLoader(url);
                }
            }
        }
    }

    public WWWLoader clone()
    {
        return new WWWLoader(url)
        {
            isRetry = isRetry,
            succeedCallback = succeedCallback,
            errorCallback = errorCallback,
            timeoutCallback = timeoutCallback,
            progressCallback = progressCallback
        };
    }

    public IEnumerator wwwLoad()
    {
        lastLoadTime = Time.realtimeSinceStartup;
        www = new WWW(url);
        yield return www;
        if (!m_isTimeOut)
        {
            if (www.error != null)
            {
                Logger.PrintLog("加载(" + url + ")失败：" + www.error);
                if (autoRetryCount > 0)
                {
                    autoRetryCount--;
                    WWWManager.Instance.retryLoader(url);
                }
                else if (isRetry)
                {
                    if (openRetryFunc != null)
                        openRetryFunc(url);
                }
                else
                {
                    if (errorCallback != null)
                    {
                        errorCallback(www.error);
                    }
                    WWWManager.Instance.removeLoader(url);
                }
            }
            else
            {
                if (succeedCallback != null)
                {
                    succeedCallback(www);
                }
                WWWManager.Instance.removeLoader(url);
            }
        }
    }

    public bool isTimeOut
    {
        get { return m_isTimeOut; }
    }
}

