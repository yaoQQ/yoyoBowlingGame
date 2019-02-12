﻿using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class ScrollPanelWidget : UIBaseWidget
{
    protected float contentTotalValue;

    public override WidgetType GetWidgetType()
    {
        return WidgetType.ScrollPanel;
    }

    public ScrollRect scrollRect;

    public RectTransform scrollRT;
    public RectTransform contentRT;

    public RectMask2D mask;


    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {


        bool sign = true;
        switch (eventType)
        {
            case UIEvent.DragBegin:
                beginDragHandler = onEventHandler;
                break;
            case UIEvent.DragEnd:
                endDragHandler = onEventHandler;
                break;
            case UIEvent.Drag:
                DragEventHandler.Get(gameObject).onDragHandler = onEventHandler;
                break;
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = onEventHandler;
                break;

            default:
                sign = false;
                break;
        }
        return sign;
    }
    public override bool RemoveEventListener(UIEvent eventType, System.Action<PointerEventData> onEventHandler)
    {
        bool sign = true;
        switch (eventType)
        {
            case UIEvent.DragBegin:
                beginDragHandler = null;
                break;
            case UIEvent.DragEnd:
                endDragHandler = null;
                break;
            case UIEvent.Drag:
                DragEventHandler.Get(gameObject).onDragHandler = null;
                break;
            case UIEvent.PointerClick:
                PointerClickListener.Get(gameObject).onHandler = null;
                break;

            default:
                sign = false;
                break;
        }
        return sign;
    }

    void Start()
    {
        scrollRect.onValueChanged.AddListener(OnValueChanged);
        DragEventHandler.Get(gameObject).onBeginDragHandler = OnBeginDrag;
        DragEventHandler.Get(gameObject).onEndDragHandler = OnEndDrag;
    }

    float lastPos = -1;
    bool _forwardSign;
    protected virtual void OnValueChanged(Vector2 v2)
    {
        if (scrollingSign)
        {
            if (scrollRect.vertical)
            {
                if (lastPos == -1)
                {
                    lastPos = v2.y;
                }
                else
                {
                    _forwardSign = v2.y <= lastPos ? true : false;
                }
            }
            else
            {
                if (lastPos == -1)
                {
                    lastPos = v2.x;
                }
                else
                {
                    _forwardSign = v2.x >= lastPos ? true : false;
                }
            }
#if TOOL
#else
            GlobalTimeManager.Instance.timerController.RemoveTimerByKey(this);
            GlobalTimeManager.Instance.timerController.AddTimer(this, 100, 1, (count) =>
            {
                lastPos = -1;
                OnScrollEnd(_forwardSign, v2);
            });
#endif

        }
    }
    protected virtual void OnScrollEnd(bool forwardSign, Vector2 v2)
    {
        scrollingSign = false;
    }



    public void SetContentSize(float sizeValue, bool resetPos = true)
    {
        contentTotalValue = sizeValue;
        bool enabledSign = false;
        if (scrollRect.vertical)
        {
            contentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentTotalValue);
            enabledSign = contentRT.rect.height > scrollRT.rect.height;
        }
        else
        {
            contentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentTotalValue);
            enabledSign = contentRT.rect.width > scrollRT.rect.width;
        }
        mask.enabled = enabledSign;
        scrollRect.enabled = enabledSign;
        if (resetPos || !enabledSign)
        {
            ResetPos();
        }

    }
    public void ResetPos()
    {
        if (scrollRect.vertical)
        {
            scrollRect.verticalNormalizedPosition = 0;
            contentRT.anchoredPosition = new Vector2(contentRT.anchoredPosition.x, 0);
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = 0;
            contentRT.anchoredPosition = new Vector2(0f, contentRT.anchoredPosition.y);

        }

    }




    #region 自动滚屏

    protected float lastScrollValue;
    protected float targetScrollValue;
    protected float changeScrollValue;


    protected Tween tween;
    protected float tweenDuration = 0.5f;

    int curPageIndex;
    int maxShowPageIndex;

    bool pageScrollSign;

    Action<int> onScrollPageEnd;

    public void OpenPageScroll(int maxPage, Action<int> scrollPageEndFun = null)
    {
        onScrollPageEnd = scrollPageEndFun;
        pageScrollSign = true;
        ResetPos();
        curPageIndex = 0;
        lastScrollValue = 0;
        targetScrollValue = 0;
        changeScrollValue = 0;
        maxShowPageIndex = maxPage;
        if (scrollRect.horizontal)
        {
            SetContentSize(scrollRT.rect.width * maxShowPageIndex);
        }
        else
        {
            SetContentSize(scrollRT.rect.height * maxShowPageIndex);
        }

    }

    public void ClosePageScroll()
    {
        onScrollPageEnd = null;
        pageScrollSign = false;
        ResetPos();
        curPageIndex = 0;
        maxShowPageIndex = 1;
        if (scrollRect.horizontal)
        {
            SetContentSize(scrollRT.rect.width);
        }
        else
        {
            SetContentSize(scrollRT.rect.height);
        }
        if (tween != null)
        {
            GameObject.Destroy(tween);
        }
    }

    protected bool scrollingSign;


    Action<PointerEventData> beginDragHandler;
    protected virtual void OnBeginDrag(PointerEventData eventData)
    {
        scrollingSign = true;
        if (beginDragHandler != null)
        {
            beginDragHandler.Invoke(eventData);
        }
    }


    Action<PointerEventData> endDragHandler;
    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        if (endDragHandler != null)
        {
            endDragHandler.Invoke(eventData);
        }
        if (!pageScrollSign) return;
        if (maxShowPageIndex == 1) return;
        float d_value = scrollRect.horizontalNormalizedPosition - targetScrollValue;
        if (Mathf.Abs(d_value) < 0.01f) return;
        bool scrollDir = d_value > 0 ? true : false;
        if (scrollDir)
        {
            curPageIndex++;
        }
        else
        {
            curPageIndex--;
        }
        int newPageIndex;

        newPageIndex = Mathf.Min(maxShowPageIndex - 1, curPageIndex);

        if (newPageIndex != curPageIndex)
        {
            curPageIndex = newPageIndex;
            return;
        }

        newPageIndex = Mathf.Max(0, curPageIndex);

        if (newPageIndex != curPageIndex)
        {
            curPageIndex = newPageIndex;
            return;
        }
        lastScrollValue = scrollRect.horizontalNormalizedPosition;
        //Debug.Log("curPageIndex===>>>" + _curPageIndex);
        OnScrollBag();
    }

    public void ScrollToAssignPage(int pageIndex)
    {
        if (maxShowPageIndex == 1) return;
        if (curPageIndex == pageIndex) return;
        if (pageIndex < 0 || pageIndex >= maxShowPageIndex)
        {
            Debug.LogError("指定页超出范围");
            return;
        }
        curPageIndex = pageIndex;
        if (pageScrollSign)
        {
            if (tween != null)
            {
                tween.Stop();
            }
            OnScrollBag(false);//被动式
        }
        else
        {
            targetScrollValue = (float)curPageIndex / (float)(maxShowPageIndex - 1f);
            changeScrollValue = targetScrollValue - scrollRect.horizontalNormalizedPosition;
            scrollRect.horizontalNormalizedPosition = lastScrollValue + changeScrollValue;
        }
    }

    void OnScrollBag(bool scrollSign = true)
    {
        initiativeScrollSign = scrollSign;
        targetScrollValue = (float)curPageIndex / (float)(maxShowPageIndex - 1f);
        changeScrollValue = targetScrollValue - scrollRect.horizontalNormalizedPosition;

        tween = Tween.AutoManagerTween(scrollRect.content.gameObject, tweenDuration);
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, tweenDuration, 1f);
        tween.SetCurve(curve);
        tween.OnUpdate += OnTweenHandle;
        tween.OnComplete += OnTweenEnd;
        tween.Play();

    }
    void OnTweenHandle(float scale)
    {
        scrollRect.horizontalNormalizedPosition = lastScrollValue + changeScrollValue * scale;
    }

    /// <summary>
    /// 主动，滚动标识;
    /// </summary>
    bool initiativeScrollSign = true;

    bool OnTweenEnd()
    {
        if (tween != null)
        {
            tween.Stop();
        }
        lastScrollValue = scrollRect.horizontalNormalizedPosition;
        if (onScrollPageEnd != null && initiativeScrollSign)//被动不执行回调;
        {
            onScrollPageEnd.Invoke(curPageIndex);

        }
        //是否循环播放;
        return false;
    }

    #endregion
}