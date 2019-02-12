using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public class ScrollPanelWithButtonWidget : UIBaseWidget
{

    protected float contentTotalValue;

    public override WidgetType GetWidgetType()
    {
        return WidgetType.ScrollPanelWithBt;
    }

    public ScrollRect scrollRect;

    public RectTransform scrollRT;
    public RectTransform contentRT;
    public bool UseBottonPoint =false;
    public RectTransform pointRt;

    public RectMask2D mask;

    public GameObject BannerSample;
    public GameObject PointBtSample;

    private float startNorPos = 0f;

    public override bool AddEventListener(UIEvent eventType, Action<PointerEventData> onEventHandler)
    {
        //return false;

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
                DragingHandler = onEventHandler;
                //DragEventHandler.Get(gameObject).onDragHandler = onEventHandler;
                break;

            default:
                sign = false;
                break;
        }
        return sign;
    }
    public override bool RemoveEventListener(UIEvent eventType, System.Action<PointerEventData> onEventHandler) {

        bool sign = true;
        switch (eventType) {
            case UIEvent.DragBegin:
                beginDragHandler = null;
                break;
            case UIEvent.DragEnd:
                endDragHandler = null;
                break;
            case UIEvent.Drag:
                DragingHandler = null;
                //DragEventHandler.Get(gameObject).onDragHandler = onEventHandler;
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

        //DragEventHandler.Get(gameObject).onBeginDragHandler = OnBeginDrag;
        //DragEventHandler.Get(gameObject).onDragHandler = OnDrag;
        //DragEventHandler.Get(gameObject).onEndDragHandler = OnEndDrag;
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
        bool enabledSign;
        if (scrollRect.vertical)
        {
            contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, contentTotalValue);
            enabledSign = contentRT.sizeDelta.y <= scrollRT.sizeDelta.y ? false : true;
        }
        else
        {
            contentRT.sizeDelta = new Vector2(contentTotalValue, contentRT.sizeDelta.y);
            enabledSign = contentRT.sizeDelta.x <= scrollRT.sizeDelta.x ? false : true;
        }
        //enabledSign = resetPos == true ? enabledSign : false;
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

    private void ResetPointBarSize() 
    {
        int childC = pointRt.childCount;
        if (childC>1)
        {
            for (int i = 0; i < childC; i++)
            {
                if (i == 0)
                {
                    RectTransform first = pointRt.GetChild(0) as RectTransform;
                    RectTransform secend = pointRt.GetChild(2) as RectTransform;
                    first.sizeDelta = secend.sizeDelta;
                }
                else {
                    pointRt.GetChild(i).gameObject.SetActive(true);
                }
            }
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
            SetContentSize(scrollRT.sizeDelta.x * maxShowPageIndex);
        }
        else
        {
            SetContentSize(scrollRT.sizeDelta.y * maxShowPageIndex);
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
            SetContentSize(scrollRT.sizeDelta.x);
        }
        else
        {
            SetContentSize(scrollRT.sizeDelta.y);
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
        startNorPos = scrollRect.horizontalNormalizedPosition;
        scrollingSign = true;
        if (beginDragHandler != null)
        {
            beginDragHandler.Invoke(eventData);
        }
    }

    Action<PointerEventData> DragingHandler;
    protected virtual void OnDrag(PointerEventData eventData)
    {
        if (DragingHandler != null)
        {
            DragingHandler.Invoke(eventData);
        }
        //if (scrollRect.horizontalNormalizedPosition > 0.985f)
        //{
        //    scrollRect.horizontalNormalizedPosition = 0;
        //    //scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, initx, Time.deltaTime * 0.1f);
        //    return;
        //}
        //else if (scrollRect.horizontalNormalizedPosition < 0.015f)
        //{
        //    scrollRect.horizontalNormalizedPosition = 1;
        //    //scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, endx, Time.deltaTime * 0.1f);
        //    return;
        //}
    }
    Action<PointerEventData> endDragHandler;
    protected virtual void OnEndDrag(PointerEventData eventData)
    {
        scrollingSign = false;
        if (endDragHandler != null)
        {
            endDragHandler.Invoke(eventData);
        }
        if (!pageScrollSign) return;
        if (maxShowPageIndex == 1) return;

   

        bool scrollDir = CheckIsmove();
        //if (!scrollDir) { return; }
        if (scrollDir)
        {
            if (scrollRect.horizontalNormalizedPosition > startNorPos)
            {
                curPageIndex++;
            }
            else {
                curPageIndex --;
            }
        }
        startNorPos = 0f;
        SetPointBar();

        int newPageIndex;

        newPageIndex = Mathf.Min(maxShowPageIndex - 1, curPageIndex);

        if (newPageIndex != curPageIndex)
        {
            curPageIndex = newPageIndex;
            SetPointBar();
            return;
        }

        newPageIndex = Mathf.Max(0, curPageIndex);

        if (newPageIndex != curPageIndex)
        {
            curPageIndex = newPageIndex;
            SetPointBar();
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
        SetPointBar();
    }

    //找到当前banner的horizontalpos 与位移的 horizontalpos
    //当前的banner占比
    //如果当自己移动超过了 1/3
    private bool CheckIsmove() 
    {
        bool ismove = false;
        Debug.Log("CheckIsmove : " + curPageIndex);
        int newcur = Mathf.Max(0, curPageIndex);
        RectTransform curpg = contentRT.GetChild(newcur) as RectTransform;
        float sizeDis = (curpg.sizeDelta.x / contentRT.sizeDelta.x);
        float levelDis = sizeDis * 0.3f;
        ismove = Mathf.Abs( scrollRect.horizontalNormalizedPosition - startNorPos) >= levelDis;
    
       
        return ismove;
    }

    private void SetPointBar() 
    {
        if (!UseBottonPoint) { return; }
        int pcout = pointRt.childCount;
        if (pcout < 2) { return; }
        RectTransform firstrt = pointRt.GetChild(0) as RectTransform;
        RectTransform secendrt = pointRt.GetChild(1) as RectTransform;
        firstrt.sizeDelta = secendrt.sizeDelta;
        for (int i =1; i < pcout; ++i )
        {
            if (i <= curPageIndex)
            {
                
                firstrt.sizeDelta += new Vector2(secendrt.sizeDelta.x, 0);
                pointRt.GetChild(i).gameObject.SetActive(false);
               
               
            }
            else 
            {
                pointRt.GetChild(i).gameObject.SetActive(true);
            }
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

       isAuto = true;
       autoRellyTime = Time.time+1f;
        //是否循环播放;
        return false;
    }

#endregion


#region 
    ///添加 目前针对横向
    
    float initx = 0f;
    float endx = 0f;
    public void AddBannerThings(Sprite _bannerSpr =null) 
    {
           GameObject addBannerObj =  GameObject.Instantiate(BannerSample, contentRT.transform);
           if (_bannerSpr)
           {
               Image _bimg = addBannerObj.GetComponent<Image>();
                 _bimg.sprite = _bannerSpr;
           }

           RectTransform _brt = addBannerObj.GetComponent<RectTransform>();
           contentRT.sizeDelta += new Vector2(_brt.sizeDelta.x, 0);

           GameObject addpointObj = GameObject.Instantiate(PointBtSample, pointRt.transform);
           RectTransform _prt = addpointObj.GetComponent<RectTransform>();
           pointRt.sizeDelta += new Vector2(_prt.sizeDelta.x, 0);
           if (contentRT.childCount>1)
           {
               OpenPageScroll(contentRT.childCount);
               initx = 0.2f*(1f / contentRT.childCount);
              
               endx = 1 - initx;

               Debug.Log("endx : " + endx);
           }
          

    }


    private bool isAuto = true;
    private float autoRellyTime = 0f;
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            isAuto = true;
            autoRellyTime = 0;
        }
        if (contentRT.childCount <2) { return; }
        if (isAuto)
        {
            if (autoRellyTime==0)
            {
                autoRellyTime = Time.time;
                return;
            }
            else if (Time.time - autoRellyTime >= 3f)
            {
                int moveIndex = curPageIndex + 1;
                curPageIndex = moveIndex % contentRT.childCount;
                OnScrollBag();
                SetPointBar();
                isAuto = false;
            }
           
        }
    }

#endregion
}
