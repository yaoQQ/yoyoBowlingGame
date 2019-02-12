using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public partial class ScrollPageWidget
{
    [SerializeField]
    private ScrollRect m_scrollRect;
    public ScrollRect scrollRect
    {
        get
        {
            if (m_scrollRect == null)
                m_scrollRect = GetComponent<ScrollRect>();
            return m_scrollRect;
        }
    }

    public RectTransform content
    {
        get { return scrollRect.content; }
    }

    private int m_maxPage = 0;
    private int m_curPage = 0;
    private float m_topOffset = 0;
    List<int> m_dataList = new List<int>();

    public Action<int> onPageChange;

    public int CurPage
    {
        get { return m_curPage; }
        set
        {
            m_curPage = value;
            //Debug.Log("当前数据 = " + m_curPage);
            if (onPageChange != null)
            {
                onPageChange.Invoke(m_curPage);
            }
        }
    }
    private void InitView(int curPage)
    {
        m_curPage = curPage;
        var verticleGroup = this.content.GetComponent<VerticalLayoutGroup>();
        if (verticleGroup != null)
            m_topOffset = verticleGroup.padding.top;
        else
            m_topOffset = 0;
        var centerPage = m_maxPage / 2;

        var diffPage = curPage - centerPage;
        m_dataList.Clear();
        for (var i = 0; i < m_maxPage; i++)
        {
            var data = i + diffPage;
            data = (data + m_maxPage) % m_maxPage;
            m_dataList.Add(data);
        }
        for (var i = 0; i < m_maxPage; i++)
        {
            var view = content.GetChild(i) as RectTransform;
            var data = m_dataList[i];
            if (view != null)
            {
                //view.gameObject.name = string.Format("{0:D2}", data);
                var text = view.GetComponent<Text>();
                text.text = string.Format("{0:D2}", data);
            }
        }
        var centerY = (content.GetChild(centerPage) as RectTransform).anchoredPosition.y;
        this.SetContentAnchoredPosition(new Vector2(0, 0 - centerY - m_topOffset));

    }

    private void SetContentAnchoredPosition(Vector2 position)
    {
        if (content != null)
            this.content.anchoredPosition = (position);
    }
    private void ResetContentView()
    {
        InitView(m_curPage);
    }
    private bool isStopComplete = false;


    public void InitScrollPage(int maxPage, int curPage)
    {
        this.m_maxPage = maxPage;
        for (var i = 0; i < maxPage; i++)
        {
            var data = i;
            m_dataList.Add(data);
        }
        InitView(curPage);
    }

    void Awake()
    {
        this.scrollRect.onValueChanged.AddListener(handler);
    }
    private Vector2 pre;
    private void handler(Vector2 arg0)
    {

        //Debug.Log(string.Format("arg0 = ({0},{1})",arg0.x , arg0.y));

        var delata = arg0 - pre;
        //Debug.Log("delataY = " + delata.y);
        //if (Mathf.Abs(delata.y) < Vector2.kEpsilon)
        //{
        //    Debug.Log("理论停止");
        //}
        if (Mathf.Abs(delata.y) < 0.001f)
        {
            if (m_dataList.Count == 0)
                return;
            this.scrollRect.StopMovement();
            Vector3 position = this.content.anchoredPosition;
            var viewIndex = Mathf.RoundToInt(position.y / 100);
            viewIndex = Mathf.Clamp(viewIndex, 0, m_maxPage - 1);
            //Debug.Log("viewIndex = " + viewIndex);
            CurPage = m_dataList[viewIndex];
            if (position.y % 100 != 0)
            {
                var targetPos = new Vector2(0, viewIndex * 100);
                this.SetContentAnchoredPosition(targetPos);
            }
            ResetContentView();
            //if (arg0.y >= 1)
            //{
            //    Debug.Log("到顶部了");
            //    ResetContentView();
            //}
            //else if (arg0.y <= 0)
            //{
            //    Debug.Log("到底部了");
            //    ResetContentView();

            //}
        }
        pre = arg0;

    }


}
public partial class ScrollPageWidget : UIBaseWidget
{
    public override WidgetType GetWidgetType()
    {
        return WidgetType.ScrollPagePanel;
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