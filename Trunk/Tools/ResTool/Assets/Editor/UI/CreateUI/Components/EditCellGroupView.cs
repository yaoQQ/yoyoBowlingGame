using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditCellGroupView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        CellGroupWidget cellGroupWidget = widget as CellGroupWidget;
        DrawCommon(window, widget.gameObject, widget);


        cellGroupWidget.cellItemName = EditorGUILayout.TextField("cell导出名字", cellGroupWidget.cellItemName, GUILayout.ExpandWidth(true));


        int curIconNum = 0;

        int i;


        if (cellGroupWidget.cellItemArr != null)
        {
            curIconNum = Mathf.Max(0, cellGroupWidget.cellItemArr.Length );
        }
        int oldIconNum = curIconNum;

        curIconNum = Mathf.Max(0, EditorGUILayout.DelayedIntField("cell个数 ", curIconNum, GUILayout.ExpandWidth(true)));



        if (curIconNum != oldIconNum)
        {
            if (curIconNum == 0)
            {
                cellGroupWidget.cellItemArr = null;
            }
            else
            {
                int minLen = 0;
                if (cellGroupWidget.cellItemArr != null && cellGroupWidget.cellItemArr.Length != 0)
                {
                    minLen = Mathf.Min(curIconNum, cellGroupWidget.cellItemArr.Length);
                }
                CellItemWidget[] tempArr = new CellItemWidget[curIconNum ];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = cellGroupWidget.cellItemArr[i];
                }
                cellGroupWidget.cellItemArr = tempArr;
            }
        }
        if (cellGroupWidget.cellItemArr != null && cellGroupWidget.cellItemArr.Length > 0)
        {
            for (i = 0; i < cellGroupWidget.cellItemArr.Length; i++)
            {
                cellGroupWidget.cellItemArr[i] = EditorGUILayout.ObjectField(i + " ：",
                    cellGroupWidget.cellItemArr[i], typeof(CellItemWidget), true, GUILayout.ExpandWidth(true)
                ) as CellItemWidget;
            }
        }
        LayoutGroup curLayoutGroup = CheckLayoutGroup(widget);
        LayoutGroup oldLayoutGroup = curLayoutGroup;
        curLayoutGroup = (LayoutGroup)EditorGUILayout.EnumPopup("布局组 :", curLayoutGroup, GUILayout.ExpandWidth(true));
        if (curLayoutGroup != oldLayoutGroup)
        {
            AddLayoutGroup(widget, curLayoutGroup);
        }

        #region cell辅助生成

        if (cellGroupWidget.cellItemArr != null && cellGroupWidget.cellItemArr[0] != null)
        {
            if (GUILayout.Button("自助生成", new GUILayoutOption[] { GUILayout.Width(42f), GUILayout.ExpandWidth(true) }))
            {
                GameObject modelCell = cellGroupWidget.cellItemArr[0].gameObject;
                RectTransform rt = (RectTransform)modelCell.transform;
               

                for (i = 1; i < cellGroupWidget.cellItemArr.Length; i++)
                {
                    GameObject dupGO = (GameObject)UnityEngine.Object.Instantiate(modelCell, modelCell.transform.parent);
                    dupGO.name = modelCell.name + "_" + i;
                    cellGroupWidget.cellItemArr[i] = dupGO.GetComponent<CellItemWidget>();
                }
            }
        }


        #endregion
    }
}
