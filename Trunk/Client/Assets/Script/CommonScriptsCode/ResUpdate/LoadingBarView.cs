using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadingBarView
{
    private GameObject gameObject;

    private ImageWidget logo_bg;
    private ImageWidget logo;
    private SpineWidget spine;

    private TextWidget versionsTxt;
    private TextWidget contentTxt;
    private GameObject progressValueGO;
    private TextWidget progressValueTxt;
    private float progressValue = 0f;

    private GameObject progressWindow;

    private GameObject popup_window;
    private TextWidget popupTipsTxt;
    private ButtonWidget popupBtn1;
    private ButtonWidget popupBtn2;
    
    private Action onPopupBtn1Handler;
    private Action onPopupBtn2Handler;

    private int spineIndex = 0;

    public void Init(GameObject go, GameObject container)
    {
        gameObject = go;
        SetContainer(container);

        logo_bg = gameObject.transform.Find("logo_bg").GetComponent<ImageWidget>();
        logo = gameObject.transform.Find("logo_bg/logo").GetComponent<ImageWidget>();
        spine = gameObject.transform.Find("spine").GetComponent<SpineWidget>();
        spine.gameObject.SetActive(false);

        versionsTxt = gameObject.transform.Find("version_txt").GetComponent<TextWidget>();
        //contentTxt = gameObject.transform.Find("loading/progress_text").GetComponent<TextWidget>();
        contentTxt = gameObject.transform.Find("progress_window/progress_text").GetComponent<TextWidget>();
        contentTxt.text = "";
        progressValueGO = gameObject.transform.Find("loading/progress_value").gameObject;
        progressValueTxt = gameObject.transform.Find("loading/progress_value/Text").GetComponent<TextWidget>();
        SetProgress(0f);

        progressWindow = gameObject.transform.Find("progress_window").gameObject;
        progressWindow.SetActive(false);

        popup_window = gameObject.transform.Find("popup_window").gameObject;
        popupTipsTxt = gameObject.transform.Find("popup_window/popup_panel/info_text").GetComponent<TextWidget>();
        popupBtn1 = gameObject.transform.Find("popup_window/popup_panel/buttons_container/btn_1").GetComponent<ButtonWidget>();
        popupBtn2 = gameObject.transform.Find("popup_window/popup_panel/buttons_container/btn_2").GetComponent<ButtonWidget>();
        popupBtn1.AddEventListener(UIEvent.PointerClick, OnPopupBtn1);
        popupBtn2.AddEventListener(UIEvent.PointerClick, OnPopupBtn2);
        popup_window.SetActive(false);
    }
    
    public void Update()
    {
    }

    public void SetContainer(GameObject container)
    {
        UITools.SetParentAndAlign(gameObject, container);
    }

    public void SetVersions(string str)
    {
        versionsTxt.text = str;
    }

    public void SetLoadContent(string content)
    {
        contentTxt.text = content;
    }

    public void ShowProgressWindow()
    {
        progressWindow.SetActive(true);
    }

    public void HideProgressWindow()
    {
        progressWindow.SetActive(false);
    }

    public void SetProgress(float value)
    {
        progressValue = value;
        progressValueTxt.text = UtilMethod.ConnectStrs(((int)(value*100)).ToString(), "%");
        progressValueGO.transform.localPosition = new Vector3(-310 + 620 * value, 0, 0);
    }

    public float GetProgressValue()
    {
        return progressValue;
    }

    public void Show(bool sign)
    {
        gameObject.SetActive(sign);
    }

    public void ShowSpine(Action callback)
    {
        logo_bg.gameObject.SetActive(false);
        //暂时屏蔽Spine
        if (callback != null)
            callback();

        /*spine.gameObject.SetActive(true);
        spine.skeleton.AnimationState.SetAnimation(0, "fx_jiazaiye_001", false);
        spine.AddSpineCustomEventListener(UISpineEvent.Complete, (obj) =>
        {
            if (spineIndex <= 0)
            {
                spineIndex = 1;
                spine.skeleton.AnimationState.SetAnimation(0, "fx_jiazaiye_002", true);
                GlobalTimeManager.Instance.timerController.AddTimer("loading_spine", 500, 1, (i) =>
                {
                    if (callback != null)
                        callback();
                });
                
            }
        });*/
    }

    public void ShowNotice(string msg, string btnName, Action onBtnfunc)
    {
        popupTipsTxt.text = msg;
        popupBtn1.Txt.text = btnName;
        onPopupBtn1Handler = onBtnfunc;
        popupBtn2.gameObject.SetActive(false);
        popup_window.SetActive(true);
    }

    public void ShowNotice2(string msg, string btnName1, Action onBtnfunc1, string btnName2, Action onBtnfunc2)
    {
        popupTipsTxt.text = msg;
        popupBtn1.Txt.text = btnName1;
        onPopupBtn1Handler = onBtnfunc1;
        popupBtn2.Txt.text = btnName2;
        onPopupBtn2Handler = onBtnfunc2;
        popupBtn2.gameObject.SetActive(true);
        popup_window.SetActive(true);
    }

    private void HideNotice()
    {
        popup_window.SetActive(false);
        onPopupBtn1Handler = null;
        onPopupBtn2Handler = null;
    }

    void OnPopupBtn1(PointerEventData eventData)
    {
        if (onPopupBtn1Handler!=null)
        {          
            onPopupBtn1Handler.Invoke();
        }
        HideNotice();
    }

    void OnPopupBtn2(PointerEventData eventData)
    {
        if (onPopupBtn2Handler != null)
        {
            onPopupBtn2Handler.Invoke();
        }
        HideNotice();
    }





}
