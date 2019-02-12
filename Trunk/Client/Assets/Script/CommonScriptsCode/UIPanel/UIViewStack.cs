using System;
using System.Collections.Generic;

public class OpenViewInfo
{
    public int viewEnum;
    public object msg;
    public Action openCallback;
}

public class UIViewStack
{
    /// <summary>栈底界面列表</summary>
    private static List<int> m_stackButtomViewList = new List<int>();

    /// <summary>界面栈</summary>
    private static List<OpenViewInfo> m_stack = new List<OpenViewInfo>();

    /// <summary>准备关闭的界面列表</summary>
    private static List<int> m_closeList = new List<int>();

    /// <summary>界面栈缓存</summary>
    private static List<OpenViewInfo> m_saveStack = new List<OpenViewInfo>();

    /// <summary>
    /// 注册栈底界面（主界面）
    /// </summary>
    /// <param name="viewEnum"></param>
    public static void RegisterStackButtomView(int viewEnum)
    {
        m_stackButtomViewList.Add(viewEnum);
    }

    public static bool IsStackButtomView(int viewEnum)
    {
        for (int i = 0, count = m_stackButtomViewList.Count; i < count; ++i)
        {
            if (m_stackButtomViewList[i] == viewEnum)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 获取栈顶界面
    /// </summary>
    /// <returns></returns>
    public static OpenViewInfo GetStackTop()
    {
        if (m_stack.Count == 0)
            return null;
        else
            return m_stack[m_stack.Count - 1];
    }

    /// <summary>
    /// 获取准备关闭的界面列表
    /// </summary>
    /// <returns></returns>
    public static List<int> GetCloseList()
    {
        return m_closeList;
    }

    /// <summary>
    /// 清空准备关闭的界面列表
    /// </summary>
    public static void ClearCloseList()
    {
        m_closeList.Clear();
    }

    public static void AddCloseView(int viewEnum)
    {
        for (int i = 0, count = m_closeList.Count; i < count; ++i)
        {
            if (m_closeList[i] == viewEnum)
                return;
        }
        m_closeList.Add(viewEnum);
    }

    public static void RemoveCloseView(int viewEnum)
    {
        for (int i = 0, count = m_closeList.Count; i < count; ++i)
        {
            if (m_closeList[i] == viewEnum)
            {
                m_closeList.RemoveAt(i);
                return;
            }
        }
    }

    /// <summary>
    /// 入栈
    /// </summary>
    /// <param name="viewEnum"></param>
    /// <param name="msg"></param>
    /// <param name="openCallback"></param>
    public static void Push(int viewEnum, object msg, Action openCallback)
    {
        //取消关闭新入栈的界面
        RemoveCloseView(viewEnum);
        //栈顶界面不重复打开
        OpenViewInfo stackTop = GetStackTop();
        if (stackTop != null && stackTop.viewEnum == viewEnum && stackTop.msg == msg && stackTop.openCallback == openCallback)
            return;
        //将栈顶界面加入待关闭列表
        if (stackTop != null)
            AddCloseView(stackTop.viewEnum);
        //如果是栈底界面则先清空栈
        if (IsStackButtomView(viewEnum))
            ClearStack();
        //入栈
        OpenViewInfo ovi = new OpenViewInfo();
        ovi.viewEnum = viewEnum;
        ovi.msg = msg;
        ovi.openCallback = openCallback;
        m_stack.Add(ovi);
        //Logger.PrintLog("入栈：" + viewEnum);
        //Logger.PrintLog("栈数量：" + m_stack.Count);
    }

    /// <summary>
    /// 出栈
    /// </summary>
    public static void Pop()
    {
        OpenViewInfo stackTop = GetStackTop();
        if (stackTop == null)
            return;
        AddCloseView(stackTop.viewEnum);
        //Logger.PrintLog("出栈：" + stackTop.viewEnum);
        m_stack.RemoveAt(m_stack.Count - 1);
        //Logger.PrintLog("栈数量：" + m_stack.Count);
    }

    /// <summary>
    /// 清空栈
    /// </summary>
    public static void ClearStack()
    {
        m_stack.Clear();
    }

    /// <summary>
    /// 缓存栈
    /// </summary>
    public static void SaveStack()
    {
        m_saveStack.Clear();
        for (int i = 0, count = m_stack.Count; i < count; ++i)
        {
            m_saveStack.Add(m_stack[i]);
        }
    }

    /// <summary>
    /// 还原栈
    /// </summary>
    public static void RevertStack()
    {
        m_stack.Clear();
        for (int i = 0, count = m_saveStack.Count; i < count; ++i)
        {
            m_stack.Add(m_saveStack[i]);
        }
    }
}
