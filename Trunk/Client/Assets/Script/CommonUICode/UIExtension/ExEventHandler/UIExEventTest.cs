using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIExEventTest : MonoBehaviour
{

    public NumberPickerWidget y;
    public NumberPickerWidget m;
    public NumberPickerWidget d;

    void Start()
    {
        List<int> year = new List<int>();
        List<int> month = new List<int>();
        List<int> day = new List<int>();
        for (var i = 1900; i <= 2018; i++)
        {
            year.Add(i);
        }
        for (var i = 1; i <= 12; i++)
        {
            month.Add(i);
        }
        for (var i = 1; i <= 28; i++)
        {
            day.Add(i);
        }
        y.SetScrollPageData(year, 70, (index) => { return string.Format("{0}年", year[0] + index); });
        m.SetScrollPageData(month, 4, (index) => { return string.Format("{0}月", month[0] + index); });
        d.SetScrollPageData(day, 2, (index) => { return string.Format("{0}日", day[0] + index); });
        y.onEndSelect = (index) =>
        {
            Debug.Log("执行选择年的停止事件");

            var preCount = day.Count;
            int count = DateTime.DaysInMonth(y.GetCurData(), m.GetCurData());
            if (preCount != count)
            {
                day.Clear();
                for (var i = 1; i <= count; i++)
                {
                    day.Add(i);
                }
                d.ChangeCount(day, count);
            }
        };
        m.onEndSelect = (index) =>
        {
            Debug.Log("执行选择月的停止事件");
            var preCount = day.Count;
            int count = DateTime.DaysInMonth(y.GetCurData(), m.GetCurData());
            if (preCount != count)
            {
                day.Clear();
                for (var i = 1; i <= count; i++)
                {
                    day.Add(i);
                }
                d.ChangeCount(day, count);
            }
        };
        //var curSelectDate = new System.DateTime(y.GetCurData(), m.GetCurData(), d.GetCurData());

    }

    void updateDayView()
    {

    }

}
