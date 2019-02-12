using System;
using System.Collections.Generic;
using UnityEngine;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class GridRecycleScrollWidget : ScrollPanelWidget
{

    public override WidgetType GetWidgetType()
    {
        return WidgetType.GridRecycleScroll;
    }


    bool initDataSign = false;

    public string cellItemName;
    public CellItemWidget[] cellItemArr;
    List<object> dataList;
    Action<GameObject, object, int> onUpdateCellData;

    public int rowValue;
    public int columnValue;

    //单元间隔
    public Vector2 cellSpaceValue;

    //单元尺寸
    public Vector2 cellPosValue;

    int startIndex;

    public int StartIndex
    {
        get { return startIndex; }
    }

    protected override void OnValueChanged(Vector2 v2)
    {
        base.OnValueChanged(v2);

        float contentPosValue;

        if (scrollRect.vertical)
        {
            contentPosValue = scrollRect.content.anchoredPosition.y;
            startIndex = Mathf.Abs((int)contentPosValue / (int)cellPosValue.y);
        }
        else
        {
            contentPosValue = scrollRect.content.anchoredPosition.x;
            startIndex = Mathf.Abs((int)contentPosValue / (int)cellPosValue.x);

        }

        //先改变布局;
        Setitemslayout(startIndex);


        //再设置数据;
        if (initDataSign)
        {
            object[] dList = GetShowDataList(startIndex);

            int index = 0;
            for (int i = 0; i < dList.Length; i++)
            {

                if (dList[i] == null)
                {
                    cellItemArr[i].gameObject.SetActive(false);
                }
                else
                {
                    if (i < cellItemArr.Length && cellItemArr[i] != null && onUpdateCellData != null)
                    {
                        cellItemArr[i].gameObject.SetActive(true);
                        onUpdateCellData(cellItemArr[i].gameObject, dList[i], i);
                        index++;
                    }

                }
            }

        }
    }

    void Setitemslayout(int startIndex)
    {
        int rowLen = rowValue + 2;
        int columnLen = columnValue + 2;
        float xSpace = cellPosValue.x + cellSpaceValue.x;
        float ySpace = cellPosValue.y + cellSpaceValue.y;
        if (scrollRect.vertical)
        {
            for (int i = 0; i < rowLen; i++)  //行
            {
                for (int j = 0; j < columnLen; j++) //列
                {
                    GameObject itemGo = cellItemArr[i * columnLen + j].gameObject;
                    RectTransform itemRt = itemGo.GetComponent<RectTransform>();

                    itemRt.anchoredPosition = new Vector2(xSpace * (j - 1), (-ySpace) * (startIndex + i - 1));

                }
            }
        }
        else
        {
            for (int i = 0; i < columnLen; i++)  //列
            {
                for (int j = 0; j < rowLen; j++) //行
                {
                    GameObject itemGo = cellItemArr[i * rowLen + j].gameObject;
                    RectTransform itemRt = itemGo.GetComponent<RectTransform>();

                    itemRt.anchoredPosition = new Vector2(xSpace * (startIndex + i - 1), (-ySpace) * (j - 1));
                }
            }
        }

    }

    object[] GetShowDataList(int startIndex)
    {
        int rowLen = rowValue + 2;
        int columnLen = columnValue + 2;
        object[] dList = new object[rowLen * columnLen];

        int dataIndex = scrollRect.vertical ? (startIndex - 1) * columnValue : (startIndex - 1) * rowValue;
        dataIndex = Mathf.Max(0, dataIndex);
        if (scrollRect.vertical)
        {
            //竖
            for (int i = 0; i < rowLen; i++) //行k
            {
                for (int j = 1; j < columnLen - 1; j++) //列  第一列和最后一列是没有的
                {
                    if (startIndex > 0)
                    {
                        if (dataIndex < dataList.Count)
                        {
                            dList[i * columnLen + j] = dataList[dataIndex];
                            dataIndex++;
                        }
                    }
                }
                startIndex++;
            }
        }
        else
        {
            //横
            for (int i = 0; i < columnLen; i++) //列
            {
                for (int j = 1; j < rowLen - 1; j++) //行  第一行和最后一行是没有的
                {
                    if (startIndex > 0)
                    {
                        if (dataIndex < dataList.Count)
                        {
                            dList[i * rowLen + j] = dataList[dataIndex];
                            dataIndex++;
                        }
                    }
                }
                startIndex++;
            }

        }

        return dList;
    }
    public void SetCellData(List<object> p_dataList, Action<GameObject, object, int> p_onUpdateCellData, bool resetPos = false)
    {
        if (p_dataList.Count > cellItemArr.Length - 2)
        {
            scrollRect.enabled = true;
        }
        else
        {
            scrollRect.enabled = false;
            scrollRect.content.anchoredPosition = Vector2.zero;
        }

        initDataSign = true;


        dataList = new List<object>();
        for (int i = 0; i < p_dataList.Count; ++i)
        {
            object obj = p_dataList[i] as object;
            dataList.Add(obj);

        }

        onUpdateCellData = p_onUpdateCellData;

        if (scrollRect.vertical)//竖  宽固定
        {
            int num = Mathf.CeilToInt((float)p_dataList.Count / (float)columnValue);
            SetContentSize((cellPosValue.y + cellSpaceValue.y) * num - cellSpaceValue.y, resetPos);
            scrollRect.content.anchorMin = new Vector2(0.5f, 1f);
            scrollRect.content.anchorMax = new Vector2(0.5f, 1f);
            scrollRect.content.pivot = new Vector2(0.5f, 1f);
        }
        else
        {
            int num = Mathf.CeilToInt((float)p_dataList.Count / (float)rowValue);
            SetContentSize((cellPosValue.x + cellSpaceValue.x) * num - cellSpaceValue.x, resetPos);
            scrollRect.content.anchorMin = new Vector2(0f, 0.5f);
            scrollRect.content.anchorMax = new Vector2(0f, 0.5f);
            scrollRect.content.pivot = new Vector2(0f, 0.5f);
        }
        if (resetPos)
        {
            OnValueChanged(new Vector2());
        }
        else
        {
            OnValueChanged(contentRT.anchoredPosition);
        }
    }

    public void SetCellData<T>(List<T> p_dataList, Action<GameObject, object, int> p_onUpdateCellData, bool resetPos = false)
    {
        SetCellData(p_dataList, p_onUpdateCellData, resetPos);
    }

}
