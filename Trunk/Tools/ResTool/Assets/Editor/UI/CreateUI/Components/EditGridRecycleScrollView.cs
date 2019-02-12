using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditGridRecycleScrollView : BaseEditView
{
    Vector2 cellScrollPos;
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        if (window.maxSize.y<700)
        {
            window.maxSize = new Vector2(400f, 700f);
            window.position = new Rect(window.position.x, window.position.y, window.position.width, 700);
        }
        GridRecycleScrollWidget gridRecycleScrollWidget = widget as GridRecycleScrollWidget;
        DrawCommon(window, widget.gameObject, widget);
        gridRecycleScrollWidget.scrollRect.horizontal = !gridRecycleScrollWidget.scrollRect.vertical;
        gridRecycleScrollWidget.scrollRect.horizontal = EditorGUILayout.Toggle("水平滚动开启：", gridRecycleScrollWidget.scrollRect.horizontal, GUILayout.ExpandWidth(true));
        gridRecycleScrollWidget.scrollRect.vertical = !gridRecycleScrollWidget.scrollRect.horizontal;
        gridRecycleScrollWidget.scrollRect.vertical = EditorGUILayout.Toggle("垂直滚动开启：", gridRecycleScrollWidget.scrollRect.vertical, GUILayout.ExpandWidth(true));

        UITools.SetWidgetAlign(gridRecycleScrollWidget.scrollRect.content, UIAlign.Top_Left);
        gridRecycleScrollWidget.scrollRect.content.anchoredPosition = Vector2.zero;

        if (gridRecycleScrollWidget.cellItemArr != null&& gridRecycleScrollWidget.cellItemArr.Length>=1 && gridRecycleScrollWidget.cellItemArr[0] != null)
        {
            GameObject modelCell = gridRecycleScrollWidget.cellItemArr[0].gameObject;
            RectTransform rt = (RectTransform)modelCell.transform;
            float wVlaue;
            float hValue;
            //if (gridRecycleScrollWidget.scrollRect.vertical)
            //{
                //竖 宽度固定
                wVlaue = (gridRecycleScrollWidget.cellPosValue.x+ gridRecycleScrollWidget.cellSpaceValue.x)* gridRecycleScrollWidget.columnValue- gridRecycleScrollWidget.cellSpaceValue.x;
                hValue = (gridRecycleScrollWidget.cellPosValue.y + gridRecycleScrollWidget.cellSpaceValue.y) * gridRecycleScrollWidget.rowValue - gridRecycleScrollWidget.cellSpaceValue.y;
            //}
            //else
            //{
            //    wVlaue = (gridRecycleScrollWidget.cellPosValue.x + gridRecycleScrollWidget.cellSpaceValue.x) * gridRecycleScrollWidget.cellItemArr.Length - gridRecycleScrollWidget.cellSpaceValue.x;
            //    hValue= (gridRecycleScrollWidget.cellPosValue.y + gridRecycleScrollWidget.cellSpaceValue.y) * gridRecycleScrollWidget.rowValue - gridRecycleScrollWidget.cellSpaceValue.y;
            //}
            gridRecycleScrollWidget.scrollRect.content.sizeDelta = new Vector2(wVlaue, hValue);
        }

        gridRecycleScrollWidget.scrollRect.content = EditorGUILayout.ObjectField("滚动容器   ：",
                gridRecycleScrollWidget.scrollRect.content, typeof(RectTransform), true, GUILayout.ExpandWidth(true)
              ) as RectTransform;


        UIAlign align = UITools.GetWidgetAlign(gridRecycleScrollWidget.scrollRect.content);
        align = (UIAlign)EditorGUILayout.EnumPopup("容器对齐方式 ", align, GUILayout.ExpandWidth(true));
        UITools.SetWidgetAlign(gridRecycleScrollWidget.scrollRect.content, align);



        gridRecycleScrollWidget.cellItemName = EditorGUILayout.TextField("cell导出名字", gridRecycleScrollWidget.cellItemName, GUILayout.ExpandWidth(true));



        int oldRowNum = gridRecycleScrollWidget.rowValue;
        int oldColumnNum = gridRecycleScrollWidget.columnValue;

        int i;

        //curIconNum = Mathf.Max(0, EditorGUILayout.DelayedIntField("cell个数(数组值会比输入大2) ", curIconNum, GUILayout.ExpandWidth(true)));
        gridRecycleScrollWidget.rowValue = Mathf.Max(0, EditorGUILayout.DelayedIntField("多少行 ", gridRecycleScrollWidget.rowValue, GUILayout.ExpandWidth(true)));
        gridRecycleScrollWidget.columnValue = Mathf.Max(0, EditorGUILayout.DelayedIntField("多少列 ", gridRecycleScrollWidget.columnValue, GUILayout.ExpandWidth(true)));


        int newNum = (gridRecycleScrollWidget.rowValue + 2) * (gridRecycleScrollWidget.columnValue + 2);
        if (gridRecycleScrollWidget.rowValue != oldRowNum|| gridRecycleScrollWidget.columnValue!= oldColumnNum)
        {
            if (gridRecycleScrollWidget.rowValue <= 0|| gridRecycleScrollWidget.columnValue <= 0)
            {
                gridRecycleScrollWidget.cellItemArr = new CellItemWidget[0] { };
            }
            else
            {
                int minLen = 0;
                if (gridRecycleScrollWidget.cellItemArr != null && gridRecycleScrollWidget.cellItemArr.Length != 0)
                {
                    minLen = Mathf.Min(newNum, gridRecycleScrollWidget.cellItemArr.Length);
                }
                CellItemWidget[] tempArr = new CellItemWidget[newNum];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = gridRecycleScrollWidget.cellItemArr[i];
                }
                gridRecycleScrollWidget.cellItemArr = tempArr;
            }
        }
        if (gridRecycleScrollWidget.cellItemArr != null && gridRecycleScrollWidget.cellItemArr.Length > 0)
        {
            EditorGUILayout.BeginVertical();

            cellScrollPos = EditorGUILayout.BeginScrollView(cellScrollPos, GUILayout.Width(EditWidgetWindow.GetWindow<EditWidgetWindow>().maxSize.x), GUILayout.Height(200));

            for (i = 0; i < gridRecycleScrollWidget.cellItemArr.Length; i++)
            {
                gridRecycleScrollWidget.cellItemArr[i] = EditorGUILayout.ObjectField(i + " ：",
                    gridRecycleScrollWidget.cellItemArr[i], typeof(CellItemWidget), true, GUILayout.ExpandWidth(true)
                ) as CellItemWidget;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();


            if (gridRecycleScrollWidget.cellItemArr[0] != null)
            {
                RectTransform rt = (RectTransform)gridRecycleScrollWidget.cellItemArr[0].transform;
               
                gridRecycleScrollWidget.cellPosValue = EditorGUILayout.Vector2Field("单元尺寸  ", new Vector2(rt.sizeDelta.x, rt.sizeDelta.y), GUILayout.ExpandWidth(true));
                
            }
            else
            {
                gridRecycleScrollWidget.cellPosValue = EditorGUILayout.Vector2Field("单元尺寸  ", Vector2.zero, GUILayout.ExpandWidth(true));
            }
        }

        gridRecycleScrollWidget.cellSpaceValue = EditorGUILayout.Vector2Field("单元间隔  ", gridRecycleScrollWidget.cellSpaceValue, GUILayout.ExpandWidth(true));


        #region cell辅助生成

        if (gridRecycleScrollWidget.cellItemArr != null && gridRecycleScrollWidget.cellItemArr[0] != null)
        {
            if (GUILayout.Button("自助生成", new GUILayoutOption[] { GUILayout.Width(42f), GUILayout.ExpandWidth(true) }))
            {
                GameObject modelCell = gridRecycleScrollWidget.cellItemArr[0].gameObject;
                RectTransform rt = (RectTransform)modelCell.transform;
                
                bool isVertical = gridRecycleScrollWidget.scrollRect.vertical;

               
                UITools.SetWidgetAlign(rt, UIAlign.Top_Left);

                float x_space = gridRecycleScrollWidget.cellPosValue.x+ gridRecycleScrollWidget.cellSpaceValue.x;
                float y_space = gridRecycleScrollWidget.cellPosValue.y + gridRecycleScrollWidget.cellSpaceValue.y;
                rt.anchoredPosition = GetCellPos(0, 0, x_space, y_space);
                rt.name = rt.name ;

                int rowMaxValue = gridRecycleScrollWidget.rowValue + 2;
                int columnMaxValue = gridRecycleScrollWidget.columnValue + 2;
                for (i = 0; i < rowMaxValue; i++)
                {
                    for(int j=0;j< columnMaxValue; j++)
                    {
                        if (i==0 && j==0 ) continue;
                        GameObject dupGO = (GameObject)UnityEngine.Object.Instantiate(modelCell, modelCell.transform.parent);
                        dupGO.name = modelCell.name + "_" + i+"_"+j;
                        RectTransform dupRT = (RectTransform)dupGO.transform;
                        UITools.SetWidgetAlign(dupRT, UIAlign.Top_Left);
                        dupRT.anchoredPosition = GetCellPos( i,j, x_space, y_space);
                        //Debug.Log(i * columnMaxValue + j);
                        gridRecycleScrollWidget.cellItemArr[i * columnMaxValue + j] = dupGO.GetComponent<CellItemWidget>();
                    }
                }
            }
        }

        #endregion

    }

   

    Vector2 GetCellPos(int row,int column, float x_space,float y_space)
    {
        Vector2 pos;
        
        pos = new Vector2(x_space*(column  -1), (-y_space) * (row - 1));
        
        return pos;
    }
}
