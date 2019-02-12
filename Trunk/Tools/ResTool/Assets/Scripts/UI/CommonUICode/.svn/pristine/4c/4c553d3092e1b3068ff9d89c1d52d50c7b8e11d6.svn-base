using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class DropdownWidget : UIBaseWidget
{
    public override WidgetType GetWidgetType()
    {
        return WidgetType.Dropdown;
    }

    public Dropdown Drop;
    public Image Img;
    public Image Arrow;
    public Image Template;
    public Image ItemCheckmark;
    public Image ItemBackground;

    Action<Action<string, object>, object, int> onUpdateOptionData;

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = onEventHandler;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }
    public override bool RemoveEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler) {
        bool sign = true;
        switch (eventType) {
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;
            default:
                sign = false;
                break;
        }
        return sign;
    }

    public void AddOptionData(string optionName, string optionValue)
    {
        Dropdown.OptionData op = new Dropdown.OptionData();
        op.text = optionName;
        optionData.Add(optionName, optionValue);
        Drop.options.Add(op);
    }

    public object GetOptionData()
    {
        if (optionData.ContainsKey(Drop.options[Drop.value].text))
        {
            return Drop.options[Drop.value].text;
        }
        else
        {
            Debug.Log("没有获取到Value");
            return null;
        }
    }

    Dictionary<string, string> optionData = new Dictionary<string, string>();
    Action<GameObject, object, int> onUpdateCellData;
    public void SetOptionData(List<object> p_dataList, Action<GameObject, object, int> p_onUpdateCellData)
    {
        Drop.options.Clear();
        optionData.Clear();
        for (int i = 0; i < p_dataList.Count; ++i)
        {
            object obj = p_dataList[i] as object;
            p_onUpdateCellData(null, obj, i);
        }
    }

    public void SetOptionData<T>(List<T> p_dataList, Action<GameObject, object, int> p_onUpdateCellData)
    {
        SetOptionData(p_dataList, p_onUpdateCellData);
    }

}
