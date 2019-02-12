using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditCellRecycleScrollView : BaseEditView
{



    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        CellRecycleScrollWidget cellRecycleScrollWidget = widget as CellRecycleScrollWidget;
        DrawCommon(window, widget.gameObject, widget);
        cellRecycleScrollWidget.scrollRect.horizontal = !cellRecycleScrollWidget.scrollRect.vertical;
        cellRecycleScrollWidget.scrollRect.horizontal = EditorGUILayout.Toggle("水平滚动开启：", cellRecycleScrollWidget.scrollRect.horizontal, GUILayout.ExpandWidth(true));
        cellRecycleScrollWidget.scrollRect.vertical = !cellRecycleScrollWidget.scrollRect.horizontal;
        cellRecycleScrollWidget.scrollRect.vertical = EditorGUILayout.Toggle("垂直滚动开启：", cellRecycleScrollWidget.scrollRect.vertical, GUILayout.ExpandWidth(true));

        //UITools.SetWidgetAlign(cellRecycleScrollWidget.scrollRect.content, UIAlign.Top_Left);
        cellRecycleScrollWidget.scrollRect.content.anchoredPosition = Vector2.zero;

        if (cellRecycleScrollWidget.cellItemArr != null && cellRecycleScrollWidget.cellItemArr[0] != null)
        {
            GameObject modelCell = cellRecycleScrollWidget.cellItemArr[0].gameObject;
            RectTransform rt = (RectTransform)modelCell.transform;
            float wVlaue;
            float hValue;
            if (cellRecycleScrollWidget.scrollRect.vertical)
            {
                wVlaue= rt.sizeDelta.x;
                hValue = (cellRecycleScrollWidget.cellPosValue + cellRecycleScrollWidget.cellSpaceValue) * cellRecycleScrollWidget.cellItemArr.Length - cellRecycleScrollWidget.cellSpaceValue;
            }
            else
            {
                wVlaue = (cellRecycleScrollWidget.cellPosValue + cellRecycleScrollWidget.cellSpaceValue) * cellRecycleScrollWidget.cellItemArr.Length - cellRecycleScrollWidget.cellSpaceValue;
                hValue = rt.sizeDelta.y;
            }
            cellRecycleScrollWidget.scrollRect.content.sizeDelta = new Vector2(wVlaue, hValue);
        }
       

        cellRecycleScrollWidget.scrollRect.content = EditorGUILayout.ObjectField("滚动容器   ：",
                cellRecycleScrollWidget.scrollRect.content, typeof(RectTransform), true, GUILayout.ExpandWidth(true)
              ) as RectTransform;


        UIAlign align = UITools.GetWidgetAlign(cellRecycleScrollWidget.scrollRect.content);
        align = (UIAlign)EditorGUILayout.EnumPopup("容器对齐方式 ", align, GUILayout.ExpandWidth(true));
        UITools.SetWidgetAlign(cellRecycleScrollWidget.scrollRect.content, align);



        cellRecycleScrollWidget.cellItemName = EditorGUILayout.TextField("cell导出名字", cellRecycleScrollWidget.cellItemName, GUILayout.ExpandWidth(true));


        int curIconNum = 0;

        int i;


        if (cellRecycleScrollWidget.cellItemArr != null)
        {
            curIconNum = Mathf.Max(0, cellRecycleScrollWidget.cellItemArr.Length - 2);
        }
        int oldIconNum = curIconNum;

        curIconNum = Mathf.Max(0, EditorGUILayout.DelayedIntField("cell个数(数组值会比输入大2) ", curIconNum, GUILayout.ExpandWidth(true)));



        if (curIconNum != oldIconNum)
        {
            if (curIconNum == 0)
            {
                cellRecycleScrollWidget.cellItemArr = new CellItemWidget[0] { };
            }
            else
            {
                int minLen = 0;
                if (cellRecycleScrollWidget.cellItemArr != null && cellRecycleScrollWidget.cellItemArr.Length != 0)
                {
                    minLen = Mathf.Min(curIconNum, cellRecycleScrollWidget.cellItemArr.Length);
                }
                CellItemWidget[] tempArr = new CellItemWidget[curIconNum + 2];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = cellRecycleScrollWidget.cellItemArr[i];
                }
                cellRecycleScrollWidget.cellItemArr = tempArr;
            }
        }
        if (cellRecycleScrollWidget.cellItemArr != null && cellRecycleScrollWidget.cellItemArr.Length > 0)
        {
            for (i = 0; i < cellRecycleScrollWidget.cellItemArr.Length; i++)
            {
                cellRecycleScrollWidget.cellItemArr[i] = EditorGUILayout.ObjectField(i + " ：",
                    cellRecycleScrollWidget.cellItemArr[i], typeof(CellItemWidget), true, GUILayout.ExpandWidth(true)
                ) as CellItemWidget;
            }


            if (cellRecycleScrollWidget.cellItemArr[0] != null)
            {
                RectTransform rt = (RectTransform)cellRecycleScrollWidget.cellItemArr[0].transform;

                if (cellRecycleScrollWidget.scrollRect.vertical)
                {
                    cellRecycleScrollWidget.cellPosValue = EditorGUILayout.FloatField("单元尺寸  ", rt.sizeDelta.y, GUILayout.ExpandWidth(true));
                }
                else
                {
                    cellRecycleScrollWidget.cellPosValue = EditorGUILayout.FloatField("单元尺寸  ", rt.sizeDelta.x, GUILayout.ExpandWidth(true));
                }
            }
            else
            {
                cellRecycleScrollWidget.cellPosValue = EditorGUILayout.FloatField("单元尺寸  ", 0, GUILayout.ExpandWidth(true));
            }
        }



        cellRecycleScrollWidget.cellSpaceValue = EditorGUILayout.FloatField("单元间隔  ", cellRecycleScrollWidget.cellSpaceValue, GUILayout.ExpandWidth(true));



        #region cell辅助生成

        if(cellRecycleScrollWidget.cellItemArr != null  && cellRecycleScrollWidget.cellItemArr[0] != null)
        {
            if(GUILayout.Button("自助生成", new GUILayoutOption[] { GUILayout.Width(42f), GUILayout.ExpandWidth(true) }))
            {
                GameObject modelCell = cellRecycleScrollWidget.cellItemArr[0].gameObject;
                RectTransform rt =(RectTransform) modelCell.transform;
                float space = 0;
                bool isVertical = cellRecycleScrollWidget.scrollRect.vertical;
                if (isVertical)
                {
                    space = cellRecycleScrollWidget.cellPosValue + cellRecycleScrollWidget.cellSpaceValue;
                    UITools.SetWidgetAlign(rt, UIAlign.Top_Left);
                }
                else
                {
                    space = cellRecycleScrollWidget.cellPosValue + cellRecycleScrollWidget.cellSpaceValue;
                    UITools.SetWidgetAlign(rt, UIAlign.Top_Left);
                }
              
                rt .anchoredPosition= GetCellPos(isVertical, 0, space);


                for ( i=1;i < cellRecycleScrollWidget.cellItemArr.Length;i++)
                {
                    GameObject dupGO = (GameObject)UnityEngine.Object.Instantiate(modelCell,modelCell.transform.parent);
                    dupGO.name = modelCell.name + "_" + i;
                    RectTransform dupRT = (RectTransform)dupGO.transform;
                    dupRT.anchoredPosition = GetCellPos(isVertical, i, space);
                    cellRecycleScrollWidget.cellItemArr[i] = dupGO.GetComponent<CellItemWidget>();
                }
            }
        }


        #endregion

    }

    Vector2 GetCellPos(bool isVertical,int index,float space)
    {
        Vector2 pos;
        if (isVertical)
        {
            pos = new Vector3(0, -space * index);
        }
        else
        {
            pos = new Vector3( space * index,0);
        }
        return pos;
    }


}
