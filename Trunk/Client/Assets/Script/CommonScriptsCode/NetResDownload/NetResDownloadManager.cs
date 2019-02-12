using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class NetResDownloadManager : Singleton<NetResDownloadManager>
{
    private class NetResItem
    {
        public Action<WWW> callback = null;
        public int retryCount = 0;
    }

    private const int MAX_RETRY_COUNT = 3;

    private Dictionary<string, NetResItem> m_dict = new Dictionary<string, NetResItem>();

    public void Download(string url, Action<WWW> callback)
    {
        if (callback == null)
            return;
        if (m_dict.ContainsKey(url))
            m_dict[url].callback += callback;
        else
        {
            NetResItem item = new NetResItem();
            item.callback = callback;
            m_dict[url] = item;
            MainThread.Instance.StartCoroutine(StartDownload(url));
        }
    }

    private IEnumerator StartDownload(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www == null || www.error != null)
        {
            if (m_dict[url].retryCount < MAX_RETRY_COUNT)
            {
                ++m_dict[url].retryCount;
                Loger.PrintWarning(url + "下载失败，重试：" + m_dict[url].retryCount);
                StartDownload(url);
            }
            else
            {
                if (www != null)
                    Loger.PrintError(url + "下载失败：" + www.error);
                m_dict[url].callback(null);
                m_dict.Remove(url);
            }
        }
        else
        {
            m_dict[url].callback(www);
            m_dict.Remove(url);
        }
    }
}