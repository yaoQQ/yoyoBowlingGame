using UnityEngine;
using System.Collections.Generic;
using System;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class CellRecycleNewScrollWidget : ScrollPanelWidget
{
    private class CellData
    {
        public object data;
        public float top;
        public float bottom;
    }
    public override WidgetType GetWidgetType()
    {
        return WidgetType.CellRecycleNewScroll;
    }

    [SerializeField]
    private CellItemWidget m_cellBase;
    private List<CellItemWidget> m_cells = new List<CellItemWidget>();
    private List<CellItemWidget> m_unusedCellList = new List<CellItemWidget>();

    //m_visibleRect范围内的cell才显示
    private Rect m_visibleRect;
    [SerializeField]
    private RectOffset m_visibleRectPadding;

    [SerializeField]
    private RectOffset m_padding;
    [SerializeField]
    private float m_spacingHeight = 4f;

    private Action<GameObject, object, int> m_onUpdateCellData;
    private List<CellData> m_dataList = new List<CellData>();
    private Dictionary<int, float> m_cellHeightDict = new Dictionary<int, float>();

    private float m_contentPos = 0f;

    public void SetCellBase(GameObject cellBaseGO, GameObject contentGO)
    {
        m_cellBase = cellBaseGO.GetComponent<CellItemWidget>();
        UITools.SetParentAndAlign(cellBaseGO, contentGO);
    }
    
    private void UpdateVisibleRect()
    {
        m_visibleRect.x = scrollRect.content.anchoredPosition.x - m_visibleRectPadding.left;
        m_visibleRect.y = m_visibleRectPadding.top - m_contentPos;
        m_visibleRect.width = scrollRT.rect.width + m_visibleRectPadding.left + m_visibleRectPadding.right;
        m_visibleRect.height = scrollRT.rect.height + m_visibleRectPadding.top + m_visibleRectPadding.bottom;
    }

    private Vector2 prevScrollPos;

    protected override void OnValueChanged(Vector2 v2)
    {
        base.OnValueChanged(v2);

        UpdateContentSize(false);
        UpdateVisibleRect();

        ReuseCells((v2.y < prevScrollPos.y) ? 1 : -1);

        prevScrollPos = v2;
    }

    private void ReuseCells(int scrollDirection)
    {
        if (m_cells.Count < 1)
            return;

        if (scrollDirection > 0)
        {
            CheckCellsFromFirst();
            FillVisibleRectWithCells();
        }
        else if (scrollDirection < 0)
        {
            CheckCellsFromLast();
            FillBackVisibleRectWithCells();
        }

        float contentH = contentRT.sizeDelta.y;
        float scrollH = scrollRT.sizeDelta.y;
        m_contentPos = (contentH - scrollH) / 2f + contentRT.anchoredPosition.y;
        //Logger.PrintWarning("m_contentPos 2:" + m_contentPos);
    }

    private void CheckCellsFromFirst()
    {
        for (int i = 0, count = m_cells.Count;i < count; ++i)
        {
            CellItemWidget cell = m_cells[i];
            cell.Top = new Vector2(0f, m_dataList[cell.index].top);
            if (cell.Bottom.y > m_visibleRect.y)
            {
                cell.gameObject.SetActive(false);
                m_unusedCellList.Add(cell);
                m_cells.RemoveAt(i);
                --i;
                --count;
            }
        }
    }

    private void CheckCellsFromLast()
    {
        for (int i = m_cells.Count - 1; i >= 0; --i)
        {
            CellItemWidget cell = m_cells[i];
            cell.Top = new Vector2(0f, m_dataList[cell.index].top);
            if (cell.Top.y < m_visibleRect.y - m_visibleRect.height)
            {
                cell.gameObject.SetActive(false);
                m_unusedCellList.Add(cell);
                m_cells.RemoveAt(i);
            }
        }
    }

    /*private void UpdateCells()
    {
        for (int i = 0, count = m_cells.Count; i < count; ++i)
        {
            CellItemWidget cell = m_cells[i];
            UpdateCellForIndex(cell, cell.index);
        }
    }*/

    private void UpdateCellForIndex(CellItemWidget cell, int index)
    {
        cell.index = index;
        cell.name = index.ToString();

        if (cell.index >= 0 && cell.index <= m_dataList.Count - 1)
        {
            cell.Top = new Vector2(0f, m_dataList[cell.index].top);
            cell.gameObject.SetActive(true);
            //更新数据 cell.UpdateContent
            m_onUpdateCellData(cell.gameObject, m_dataList[cell.index].data, cell.index);
            cell.Height = CellHeightAtIndex(cell.index);
        }
        else
            cell.gameObject.SetActive(false);
    }
    
    public void SetCellHeightAtIndex(int index, float height)
    {
        if (!m_cellHeightDict.ContainsKey(index) || m_cellHeightDict[index] != 600f)
            m_cellHeightDict[index] = height;
    }

    private float CellHeightAtIndex(int index)
    {
        if (m_cellHeightDict.ContainsKey(index))
            return m_cellHeightDict[index];
        else
            return 200f;
    }

    private void FillVisibleRectWithCells()
    {
        if (m_cells.Count < 1)
            return;

        CellItemWidget lastCell = m_cells[m_cells.Count - 1];
        int nextCellIndex = lastCell.index + 1;
        Vector2 nextCellTop = lastCell.Bottom + new Vector2(0f, -m_spacingHeight);

        bool b = false;
        while (nextCellIndex < m_dataList.Count && nextCellTop.y >= m_visibleRect.y - m_visibleRect.height)
        {
            b = true;
            CellItemWidget cell = CreateCellForIndex(nextCellIndex);
            cell.Top = nextCellTop;

            lastCell = cell;
            nextCellIndex = lastCell.index + 1;
            nextCellTop = lastCell.Bottom + new Vector2(0f, -m_spacingHeight);
        }

        if (b)
            UpdateContentSize();
    }

    private void FillBackVisibleRectWithCells()
    {
        if (m_cells.Count < 1)
            return;

        CellItemWidget firstCell = m_cells[0];
        int prevCellIndex = firstCell.index - 1;
        Vector2 prevCellBottom = firstCell.Top + new Vector2(0f, m_spacingHeight);

        bool b = false;
        while (prevCellIndex >= 0 && prevCellBottom.y <= m_visibleRect.y)
        {
            b = true;
            CellItemWidget cell = CreateCellForIndex(prevCellIndex, false);
            cell.Bottom = prevCellBottom;

            firstCell = cell;
            prevCellIndex = firstCell.index - 1;
            prevCellBottom = firstCell.Bottom + new Vector2(0f, m_spacingHeight);
        }

        if (b)
            UpdateContentSize();
    }

    private CellItemWidget CreateCellForIndex(int index, bool isLast = true)
    {
        CellItemWidget cell;
        int count = m_unusedCellList.Count;
        if (count > 0)
        {
            cell = m_unusedCellList[count - 1];
            m_unusedCellList.RemoveAt(count - 1);
        }
        else
        {
            GameObject obj = Instantiate(m_cellBase.gameObject) as GameObject;
            cell = obj.GetComponent<CellItemWidget>();

            Vector3 scale = cell.transform.localScale;
            Vector2 sizeDelta = cell.rt.sizeDelta;
            Vector2 offsetMin = cell.rt.offsetMin;
            Vector2 offsetMax = cell.rt.offsetMax;

            cell.transform.SetParent(m_cellBase.transform.parent);

            cell.transform.localScale = scale;
            cell.rt.sizeDelta = sizeDelta;
            cell.rt.offsetMin = offsetMin;
            cell.rt.offsetMax = offsetMax;
        }
        
        UpdateCellForIndex(cell, index);

        if (isLast)
            m_cells.Add(cell);
        else
            m_cells.Insert(0, cell);

        return cell;
    }

    public void SetCellData(List<object> p_dataList, Action<GameObject, object, int> onUpdateCellData)
    {
        //float time1 = Time.realtimeSinceStartup;
        //Debug.LogWarning("SetCellData 1:" + time1);

        m_onUpdateCellData = onUpdateCellData;
        
        m_dataList.Clear();
        if (p_dataList == null || p_dataList.Count == 0)
        {
            m_cellHeightDict.Clear();
            for (int i = m_cells.Count - 1; i >= 0; --i)
            {
                CellItemWidget cell = m_cells[i];
                cell.gameObject.SetActive(false);
                m_unusedCellList.Add(cell);
                m_cells.RemoveAt(i);
            }
        }
        CellData cellData;
        for (int i = 0; i < p_dataList.Count; ++i)
        {
            cellData = new CellData();
            cellData.data = p_dataList[i];
            m_dataList.Add(cellData);
        }
        
        //这里必须执行2次UpdateContent();
        UpdateContent();
        UpdateContent();

        ResetContentPos();

        //float time2 = Time.realtimeSinceStartup;
        //Debug.LogWarning("SetCellData 2:" + time2);
        //Debug.LogWarning("SetCellData 2-1:" + (time2-time1));
    }

    private void UpdateContent()
    {
        UpdateContentSize();
        UpdateVisibleRect();

        
        if (m_cells.Count < 1)
        {
            Vector2 cellTop = new Vector2(0, -m_padding.top);
            for (int i = 0, count = m_dataList.Count; i < count; ++i)
            {
                float cellHeight = CellHeightAtIndex(i);
                Vector2 cellBottom = cellTop + new Vector2(0, -cellHeight);
                if ((cellTop.y <= m_visibleRect.y && cellTop.y >= m_visibleRect.y - m_visibleRect.height) ||
                    (cellBottom.y <= m_visibleRect.y && cellBottom.y >= m_visibleRect.y - m_visibleRect.height))
                {
                    CellItemWidget cell = CreateCellForIndex(i);
                    cell.Top = cellTop;
                    break;
                }
                cellTop = cellBottom + new Vector2(0f, m_spacingHeight);
            }

            FillVisibleRectWithCells();
        }
        else
        {
            CheckCellsFromFirst();
            CheckCellsFromLast();
            //UpdateCells();
            FillVisibleRectWithCells();
            FillBackVisibleRectWithCells();
        }
    }

    private void UpdateContentSize(bool isUpdateContent = true)
    {
        float contentHeight = 0f;
        float cellHeight;
        for (int i = 0, count = m_dataList.Count; i < count; ++i)
        {
            if (i > 0)
                contentHeight += m_spacingHeight;
            m_dataList[i].top = -contentHeight;
            cellHeight = CellHeightAtIndex(i);
            //Logger.PrintWarning("cellHeight:" + cellHeight);
            contentHeight += cellHeight;
            m_dataList[i].bottom = -contentHeight;
        }

        if (!isUpdateContent)
            return;

        Vector2 sizeDelta = contentRT.sizeDelta;
        sizeDelta.y = m_padding.top + contentHeight + m_padding.bottom;
        if (sizeDelta.y < scrollRT.sizeDelta.y)
            sizeDelta.y = scrollRT.sizeDelta.y;
        
        contentRT.sizeDelta = sizeDelta;
        //Logger.PrintWarning("sizeDelta.y:" + sizeDelta.y);

        float contentH = contentRT.sizeDelta.y;
        float scrollH = scrollRT.sizeDelta.y;
        contentRT.anchoredPosition = new Vector2(0f, -(contentH - scrollH) / 2f + m_contentPos);
    }
    
    private void ResetContentPos()
    {
        //如果接近置底，则置底，否则尽量保持原状
        float contentH = contentRT.sizeDelta.y;
        float scrollH = scrollRT.sizeDelta.y;
        if (contentH > scrollH)
        {
            int count = m_dataList.Count;
            if (count >= 2 && contentH - scrollH - m_contentPos <= CellHeightAtIndex(count - 2) + CellHeightAtIndex(count - 1))
            {
                //置底
                contentRT.anchoredPosition = new Vector2(0f, (contentH - scrollH) / 2f);
                m_contentPos = (contentH - scrollH) / 2f + contentRT.anchoredPosition.y;
                //Logger.PrintWarning("m_contentPos 1:" + m_contentPos);
            }
            else
            {
                contentRT.anchoredPosition = new Vector2(0f, - (contentH - scrollH) / 2f + m_contentPos);
            }
        }
    }

    /// <summary>
    /// 单独刷新一个cell的数据
    /// </summary>
    /// <param name="index"></param>
    public void UpdateCell(int index)
    {
        int count = m_cells.Count;
        if (count > 0 && m_cells[0].index <= index && m_cells[count - 1].index >= index)
        {
            CellItemWidget cell = m_cells[index - m_cells[0].index];
            UpdateCellForIndex(cell, cell.index);
        }
    }
}
