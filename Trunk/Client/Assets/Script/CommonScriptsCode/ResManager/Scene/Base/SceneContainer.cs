using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class SceneContainer : MonoBehaviour
{

    public string containerName;

    public SceneCell[] cellArr;

    public SceneContainerInfo containerInfo;

    /// <summary>
    /// 动态的cell 集合。重置时，会清空
    /// </summary>
    public List<SceneCell> dynamicCellList=new List<SceneCell>();


    int loadedNum;
    public void Init(int num)
    {
        loadedNum = 0;
        cellArr = new SceneCell[num];
    }

    public void AddCell(int index,SceneCell cell)
    {
        loadedNum++;
        cellArr[index] = cell;
    }

    public bool CheckInitSign()
    {
        return loadedNum >= cellArr.Length;
    }

    public void DynamicAddCell(SceneCell cell)
    {
        if(!dynamicCellList.Contains(cell))
        {
            dynamicCellList.Add(cell);
        }
        cell.transform.parent = this.transform;

    }

    public void DynamicRemoveCell(SceneCell cell)
    {
        for (int i = 0; i < dynamicCellList.Count; i++)
        {
            SceneCell sc = dynamicCellList[i];
            if(sc==cell)
            {
                dynamicCellList.RemoveAt(i);
                cell.transform.parent = null;
            }
        }
    }

    public void Reset()
    {
        int i;
        this.gameObject.SetActive(true);
        this.transform.position = containerInfo.posVector;
        this.transform.rotation = Quaternion.Euler(containerInfo.rotationVector);
        this.transform.localScale = containerInfo.scaleVector;
        this.gameObject.layer = containerInfo.layerMaskValue;
        for ( i=0;i< cellArr.Length;i++)
        {
            SceneCell cell = cellArr[i];
            cell.Reset(this.transform);
        }
        int len = dynamicCellList.Count;
        for (i= len-1;i>=0;i--)
        {
            SceneCell sc = dynamicCellList[i];
            sc.Del();
        }
        if(len>0)
        {
            dynamicCellList.Clear();
        }
        
    }
}
