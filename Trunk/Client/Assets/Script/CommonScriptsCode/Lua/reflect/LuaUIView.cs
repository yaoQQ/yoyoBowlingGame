
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface LuaUIView
{
    /// <summary>获取界面类型（层级）</summary>
    int getViewType();

    /// <summary>获取界面枚举</summary>
    int getViewEnum();

    /// <summary>获取是否是栈界面</summary>
    bool getIsStackView();

    /// <summary>获取是否是白色状态栏</summary>
    bool getStateBarWhiteColor();

    /// <summary>获取加载列表</summary>
    List<string> getLoadOrders();

    /// <summary>获取界面GameObject</summary>
    GameObject getViewGO();

    /// <summary>设置容器（层级）GameObject</summary>
    void setContainerGO(GameObject containerGO);

    /// <summary>获取是否正在加载</summary>
    bool getIsLoading();

    /// <summary>获取是否已加载</summary>
    bool getIsLoaded();

    /// <summary>设置是否正在打开过程中（加载并显示）</summary>
    void setOpening(bool value);

    /// <summary>获取是否正在打开过程中（加载并显示）</summary>
    bool getOpening();

    /// <summary>获取是否已经打开（显示）</summary>
    bool getIsOpen();

    /// <summary>开始加载</summary>
    void startLoad();

    /// <summary>一个UI资源加载结束</summary>
    void executeLoadUIEnd(string uiName, GameObject go);

    /// <summary>显示界面</summary>
    void show(object msg);

    /// <summary>隐藏界面</summary>
    void hide();

    /// <summary>返回键关闭界面</summary>
    void closeByEsc();

    /// <summary>销毁界面</summary>
    void onDestroy();
}
