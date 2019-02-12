using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

/// <summary>
/// 多线程管理器,负责主线程(MainThread)和子线程(Thread)之间的调度
/// </summary>
public class ThreadManager : MonoBehaviour {
    private List<Action> m_actionList = new List<Action>();
    private List<Action> m_actionDoList = new List<Action>();
    private object m_actionListLock = new object();

    private static ThreadManager m_instance;
    public static ThreadManager Instance {
        get {
            if (m_instance == null) {
                GameObject go = new GameObject("ThreadManager");
                DontDestroyOnLoad(go);
                m_instance = go.AddComponent<ThreadManager>();
            }
            return m_instance;
        }
    }

    void Update() {
        RunMainAction();
    }

    private void RunMainAction()
    {
        lock (m_actionListLock)
        {
            if (m_actionList.Count > 0)
            {
                foreach (Action action in m_actionList)
                {
                    m_actionDoList.Add(action);
                }
                m_actionList.Clear();
                foreach (Action action in m_actionDoList)
                {
                    action();
                }
                m_actionDoList.Clear();
            }
        }
    }

    /// <summary>
    /// 执行主线程操作
    /// </summary>
    public static void RunMainThread(Action action)
    {
        lock (Instance.m_actionListLock)
        {
            Instance.m_actionList.Add(action);
        }
    }
    
    /// <summary>
    /// 执行子线程操作
    /// </summary>
    public static void RunThread(Action action)
    {
        ThreadPool.QueueUserWorkItem(Instance.RunAction, action);
    }

    private void RunAction(object action) {
        ((Action)action)();
    }
}