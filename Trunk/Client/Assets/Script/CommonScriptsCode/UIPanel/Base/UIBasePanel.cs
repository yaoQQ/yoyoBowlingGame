using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UIBasePanel<T> : IPanel where T:class
{
    /// <summary>
    /// 用于下挂此面板的容器;
    /// </summary>
   protected GameObject Container;

    bool isInit = false;

    /// <summary>
    /// 正在初始化标识;
    /// </summary>
    bool init_running;



    public bool GetInitRunning()
    {
        return init_running;
    }

    public void SetContainerGO(GameObject go)
    {
        Container = go;
    }

    public bool GetIsInit()
    {
        return isInit;
    }
    public void SetIsInit(bool value)
    {
        isInit = value;
        if (isInit)
        {
            init_running = false;
        }
    }

    public abstract GameObject GetPanelGO();

    public virtual void Init()
    {
        init_running = true;
    }
    public abstract void Open(object msg);



    public abstract void Del();

    public virtual void OpenAnimation()
    {
        if (titleGOSign)
        {
            GlobalTimeManager.Instance.timerController.AddTimer(titleRT, 250, 1, ScaleIcon);

        }

    }

    bool titleGOSign = false;
    RectTransform titleRT;
    Image iconImage;
    Text pageName;

    public void SetTileGO(RectTransform p_titleRT,Image p_iconImage,Text p_pageName)
    {
        titleGOSign = true;
        titleRT = p_titleRT;
        iconImage = p_iconImage;
        pageName = p_pageName;
    }


    protected Tween tween;
    void ScaleIcon(int i)
    {
        if (tween != null)
        {
            tween.Stop();
        }
        tween = Tween.AutoManagerTween(titleRT.gameObject, 0.16f);
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 0.35f, 1f);
        tween.SetCurve(curve);
        tween.OnUpdate += ScaleTween;
        tween.Play();
        iconImage.gameObject.SetActive(true);
        pageName.gameObject.SetActive(true);
        iconImage.CrossFadeAlpha(0.15f, 0f, true);
        iconImage.CrossFadeAlpha(1f, 0.35f, true);
        pageName.CrossFadeAlpha(0.15f, 0f, true);
        pageName.CrossFadeAlpha(1f, 0.35f, true);
    }

    void ScaleTween(float scale)
    {
        titleRT.localScale = new Vector3(1 + 5 * (1 - scale), 1 + 5 * (1 - scale), 1f);
    }


    public virtual bool CloseAnimation()
    {
        return false;
    }

    Animator anim;
    public void PlayAnimation(GameObject animGO,string animName)
    {
        anim = animGO.GetComponent<Animator>();
        if (anim == null)
        {
            anim = animGO.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("PanelAC");
        }
        if (animName=="PanelFadeIn")
        {
            anim.SetBool("fade",false);
            OnFadIn();
        }
        else
        {
            anim.SetBool("fade", true);
            
            OnFadOut();
        }
    }

    void OnFadIn()
    {
        Component[] comps = GetPanelGO().GetComponentsInChildren<Component>();
        foreach (Component c in comps)
        {
            if (c is Graphic)
            {
                (c as Graphic).CrossFadeAlpha(0, 0f, true);
                (c as Graphic).CrossFadeAlpha(1,0.4f, true);
            }
        }
    }
    void OnFadOut()
    {
        Component[] comps = GetPanelGO().GetComponentsInChildren<Component>();
        foreach (Component c in comps)
        {
            if (c is Graphic)
            {
                (c as Graphic).CrossFadeAlpha(0,0.33f, true);
            }
        }

    }



    public virtual void OnClosedPanel() 
    {
        if (GetPanelGO() != null)
        {
            GetPanelGO().SetActive(false);
        }
    }



    public  UIPanelEnum GetPanelEnum()
    {
        System.Reflection.MemberInfo info = typeof(T);
        PanelAttribute attribute = (PanelAttribute)Attribute.GetCustomAttribute(info, typeof(PanelAttribute));
        if (attribute==null)
        {
            Loger.PrintError("面板没有初始化特性;");
            return UIPanelEnum.None;
        }
        return attribute.PanelEnum;
    }
    public  UIViewType GetPanelType() 
    {
        System.Reflection.MemberInfo info = typeof(T);
        PanelAttribute attribute = (PanelAttribute)Attribute.GetCustomAttribute(info, typeof(PanelAttribute));
        if (attribute == null)
        {
            Loger.PrintError("面板没有初始化特性;");
            return UIViewType.None;
        }
        return attribute.PanelType;
    }
    public void Close(PointerEventData eventData=null)
    {

        //UIViewManager.Instance.Close(GetPanelEnum());
        if(GetPanelType()==UIViewType.None)
        {
            //AudioManager.Instance.Play2D("closeroleUI");
        }
        
    }
    

    protected void SetBgBlurMat(GameObject bgGO)
    {
        bgGO.transform.Find("Bg").GetComponent<Image>().material = Resources.Load("UIBlurGB") as Material;
    }
}
