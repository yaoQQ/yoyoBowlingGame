using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Picker;
using UnityEngine.Events;
#if !TOOL
using XLua;
[LuaCallCSharp]
#endif

public partial class NumberPickerWidget : IMassiveStringPicker
{
    public delegate string SetTextDelegate(int index);
    private SetTextDelegate setText = null;

    [SerializeField]
    MassivePickerScrollRect m_MassivePickerScrollRect = null;
    public Action<int> onSelectChange;
    public Action<int> onEndSelect;
    private List<int> m_dataList = new List<int>();
    public string GetText(int columnIndex, int index)
    {
        if (m_dataList == null)
            return "";
        if (setText == null)
            return string.Format("{0:00}", m_dataList[0] + index);
        return setText.Invoke(index);
    }
    public int GetCurData()
    {
        var dataIndex = m_MassivePickerScrollRect.GetSelectedItemIndex();
        return m_dataList[dataIndex];
    }
    public void OnSelectItem(int index)
    {
        var dataIndex = m_MassivePickerScrollRect.GetSelectedItemIndex();
        if (onSelectChange != null)
        {
            onSelectChange.Invoke(dataIndex);
        }
#if UNITY_EDITOR
        //Debug.Log("Select " + m_dataList[dataIndex].ToString());
#endif
    }
    public void SetScrollPageData(List<int> p_dataList, int curDataIndex, SetTextDelegate p_setText)
    {
        if (m_MassivePickerScrollRect == null || p_dataList == null || p_dataList.Count == 0)
            return;
        m_dataList = p_dataList;
        m_MassivePickerScrollRect.itemCount = p_dataList.Count;
        m_MassivePickerScrollRect.initialPositionItemIndex = curDataIndex;
        m_MassivePickerScrollRect.onEndSelectItem = onEndSelectHandler;
        setText = p_setText;
        gameObject.SetActive(true);
    }

    private void onEndSelectHandler(int obj)
    {
        if (onEndSelect != null)
            onEndSelect.Invoke(obj);
    }

    public void ChangeCount(List<int> p_dataList, int count)
    {
        m_dataList = p_dataList;
        m_MassivePickerScrollRect.itemCount = count;
    }
    protected override void Awake()
    {
        gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_MassivePickerScrollRect.IsSetInitialPosition = false;
    }


}


public partial class NumberPickerWidget : UIBaseWidget
{
    public override WidgetType GetWidgetType()
    {
        return WidgetType.NumberPicker;
    }

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        throw new NotImplementedException();
    }

    public override bool RemoveEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        throw new NotImplementedException();
    }
}