using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class PanelAttribute : Attribute
{
    public PanelAttribute(UIViewType panelType, UIPanelEnum panelEnum)
    {
        _panelType = panelType;
        _panelEnum = panelEnum;
    }

    readonly UIViewType _panelType;

    public UIViewType PanelType
    {
        get { return _panelType; }
    }

    readonly UIPanelEnum _panelEnum;

    public UIPanelEnum PanelEnum
    {
        get { return _panelEnum; }
    } 
}
