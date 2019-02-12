using UnityEngine;
using System.Collections;

public abstract class ABObject 
{

    public string resPath;
    public enum LoadState
    {
        Loading,
        Loaded
    }
    #region abObject的被引用数目

    /// <summary>
    /// 被外界引用的数目;引用数为0时，才符合GC机制;
    /// </summary>
    int referenceCount;

    public int ReferenceCount
    {
        get { return referenceCount; }
    }
    public void AddReference()
    {
        referenceCount++;
    }
    public virtual void RemoveReference()
    {
        if (referenceCount > 0)
        {
            referenceCount--;
        }
    }

    #endregion

    protected LoadState curLoadState;

    public LoadState CurLoadState
    {
        get { return curLoadState; }
    }

    protected AssetBundle ab;

    
}
