using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ResDownLoadContoller : Singleton<ResDownLoadContoller>
{
    Dictionary<string, ResPackDownLoader> packDownLoaderDic = new Dictionary<string, ResPackDownLoader>();

    public void DownLoadResPack(ResVersions.ResPack resPack)
    {
        ResPackDownLoader loader;
        packDownLoaderDic.TryGetValue(resPack.zipPath, out loader);
        if (loader == null)
        {
            loader = new ResPackDownLoader();
            packDownLoaderDic.Add(resPack.zipPath, loader);
        }
        loader.StartDown(resPack);
    }

    public void CancelDownLoadResPack()
    {
        List<string> delArr = new List<string>();
        var enumerator = packDownLoaderDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ResPackDownLoader loader = enumerator.Current.Value;
            loader.Close();
            delArr.Add(enumerator.Current.Key);
        }
        for (int i = 0; i < delArr.Count; i++)
        {
            packDownLoaderDic.Remove(delArr[i]);
        }
    }


    public float GetTotalProgress()
    {
        long totalLength = 0;
        long fileLength = 0;
        var enumerator = packDownLoaderDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ResPackDownLoader loader = enumerator.Current.Value;
            totalLength += loader.TotalLength;
            fileLength += loader.FileLength;
        }


        if (totalLength == 0 || fileLength == 0)
        {
            return 0f;
        }
        else
        {
            float value = (float)fileLength / (float)totalLength;
            return value;
        }
    }

    public long GetTotalSize()
    {
        long totalLength = 0;
        var enumerator = packDownLoaderDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ResPackDownLoader loader = enumerator.Current.Value;
            totalLength += loader.TotalLength;
        }
        return totalLength;
    }
    public void ClearCompleter()
    {
        List<string> delArr = new List<string>();
        var enumerator = packDownLoaderDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            ResPackDownLoader loader = enumerator.Current.Value;
            if (loader.progress == 1f)
            {
                loader.Close();
                delArr.Add(enumerator.Current.Key);
            }
        }
        for (int i = 0; i < delArr.Count; i++)
        {
            packDownLoaderDic.Remove(delArr[i]);
        }
    }


    public void OnApplicationQuit()
    {
        foreach (ResPackDownLoader loader in packDownLoaderDic.Values)
        {
            loader.Close();
        }
    }


    List<ResRecordDownLoader> resRecordDownLoaderList = new List<ResRecordDownLoader>();

    public void AddResRecordDown(ResVersions.ComparisonRecord comparisonRecord)
    {
        ResRecordDownLoader loader = new ResRecordDownLoader();
        loader.StartDown(comparisonRecord);
        resRecordDownLoaderList.Add(loader);
    }

    public float GetUpdateRecordsProgress()
    {
        long totalLength = 0;
        long fileLength = 0;
        for (int i = 0; i < resRecordDownLoaderList.Count; i++)
        {
            ResRecordDownLoader loader = resRecordDownLoaderList[i];
            totalLength += loader.TotalLength;
            fileLength += loader.FileLength;
        }

        if (totalLength == 0 || fileLength == 0)
        {
            return 0f;
        }
        else
        {
            float value = (float)fileLength / (float)totalLength;
            return value;
        }
    }

    public bool ResRecordDownEndSign()
    {
        for (int i = 0; i < resRecordDownLoaderList.Count; i++)
        {
            ResRecordDownLoader loader = resRecordDownLoaderList[i];
            loader.RunDown();
            //Debug.Log("loader.CurrentState "+ loader.CurrentState);
            if (loader.CurrentState == ResRecordDownLoader.LoadState.downloading)
            {

                return false;
            }
        }
        return true;
    }

    //获取加载失败的列表。再重新加载
    public ResVersions.ComparisonRecord[] GetFailComparisonRecordList()
    {
        List<ResVersions.ComparisonRecord> list = new List<ResVersions.ComparisonRecord>();
        for (int i = 0; i < resRecordDownLoaderList.Count; i++)
        {
            ResRecordDownLoader loader = resRecordDownLoaderList[i];
            if (loader.CurrentState == ResRecordDownLoader.LoadState.fail)
            {
                list.Add(loader.ComparisonRecord);
            }
        }
        return list.ToArray();
    }

    public void ClearRecordDown()
    {
        resRecordDownLoaderList.Clear();
    }



}
