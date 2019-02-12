using UnityEngine;
using System.Collections;
using System;

public enum LoadState
    {
        free,
        downloading,//下载中
        finish,//下载完成
        fail,
    }



public abstract class BaseLoader 
{

    protected LoadState currentState;

    protected LoaderManager.LoadOrder loadOrder;

    public virtual  void StartDown(LoaderManager.LoadOrder order)
    {
        currentState = LoadState.downloading;
        loadOrder = order;
    }

    public abstract void RunDown();
    

    public void EndDown()
    {
        currentState = LoadState.free;
        loadOrder = null;
    }

    public bool CheckFinish()
    {
        if (currentState == LoadState.finish)
        {
            return true;
        }
        return false;
    }

}
