using UnityEngine;
using System.Text;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

public class Logger
{
    public static void Print(LogType logType, params string[] msgs)
    {
        string msg = ConnectStrs(msgs);
        switch (logType)
        {
            case LogType.Log:
                Debug.Log(msg);
                break;
            case LogType.Warning:
                Debug.LogWarning(msg);
                break;
            case LogType.Error:
                Debug.LogError(msg);
                break;
        }
    }

    public static void PrintDebug(params string[] msgs)
    {
        if (Application.isEditor)
            PrintLog(msgs);
    }

    public static void PrintLog(params string[] msgs)
    {
        string msg = CommonUtils.ConnectStrs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff  "), ConnectStrs(msgs));
        Debug.Log(msg);
    }

    public static void PrintWarning(params string[] msgs)
    {
        string msg = CommonUtils.ConnectStrs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff  "), ConnectStrs(msgs));
        Debug.LogWarning(msg);
    }

    public static void PrintError(params string[] msgs)
    {
        string msg = CommonUtils.ConnectStrs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff  "), ConnectStrs(msgs));
        Debug.LogError(msg);
    }


    public static void PrintColor(string color, params string[] msgs)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(CommonUtils.ConnectStrs(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff  ")));
        sb.Append("<color=");
        sb.Append(color);
        sb.Append(">");

        for(int i=0;i< msgs.Length;i++)
        {
            sb.Append(msgs[i]);
        }
        sb.Append("</color>");
        Debug.Log(sb.ToString());
    }




    public static string ConnectStrs(params string[] strs)
    {
        StringBuilder sb = new StringBuilder();
        int len = strs.Length;
        for (int i = 0; i < len; ++i)
            sb.Append(strs[i]);
        return sb.ToString();
    }



    class LogData
    {
        public LogType level;
        public string text;
        public DateTime time;
        public int threadid;

        public string Time
        {
            get
            {
                return string.Format("{0}-{1}-{2},{3}:{4}:{5}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            }
        }
    }

    static bool isInit = false;

    static StreamWriter streamWriter;

    static string Filename = "Logger{0}.txt";

    static Thread logThread;

    private static bool isStop;

    static Queue<LogData> poolList = new Queue<LogData>();
    static Queue<LogData> writeList = new Queue<LogData>();

    public static void Init()
    {
        if (isInit) { Debug.LogWarning("Logger工具过滤重复初始化！"); return; }
        string pathStr = GetLogPath;
        if (File.Exists(pathStr)) File.Delete(pathStr);
        //创建文本
        try
        {
            streamWriter = new StreamWriter(pathStr, false, System.Text.Encoding.UTF8);
            streamWriter.WriteLine(System.DateTime.Now);
            streamWriter.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+--+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            streamWriter.WriteLine("+                                                                          +");
            streamWriter.WriteLine("+                         Logger singleton created.                        +");
            streamWriter.WriteLine("+                                                   	                   +");
            streamWriter.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+--+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            streamWriter.Flush();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Logger工具 初始化时，文本创建 Error!" + ex.Data);
            return;
        }
        //注册侦听系统本身的LOG
        Application.logMessageReceivedThreaded+= OnDebugLog;
        //开始日志的线程
        logThread = new Thread(UpdateLogThread);
        logThread.Start();
        isInit = true;
    }
    //string condition, string stackTrace, LogType type
    static void OnDebugLog(string logString, string stackTrace, LogType type)
    {
        LogInfo(type, "logString:{0}, stackTrace:{1}", logString, stackTrace);
    }

    static void LogInfo(LogType level, string format, params object[] objs)
    {
        if (!isInit)
        {
            Debug.LogError("Logger工具类没有初始化！");
            return;
        }
        LogData ld = GetEmpty();
        ld.level = level;
        ld.text = (objs == null || objs.Length == 0) ? format : string.Format(format, objs);
        ld.time = System.DateTime.Now;
        ld.threadid = Thread.CurrentThread.ManagedThreadId;

        lock (writeList)
        {
            writeList.Enqueue(ld);
        }

    }

    static LogData GetEmpty()
    {
        lock (poolList)
        {
            if (poolList.Count != 0)
            {
                LogData ld = poolList.Dequeue();
                return ld;
            }
        }
        return new LogData();
    }

    static int LifeCount = 0;
    static bool isWriteEnd = true;
    static void UpdateLogThread()
    {
        while (isWriteEnd)
        {
           
            LifeCount++;
            if (LifeCount >= 200)
            {
                //LogInfo(LogType.Log, "UpdateThread Life!");
                LifeCount = 0;
            }

            if (writeList.Count == 0)
            { 
                Thread.Sleep(200);
                continue;
            }

            string text;
            LogData ld;
            lock (writeList)
            {
                while (writeList.Count > 0)
                {
                    ld = writeList.Dequeue();
                    text = string.Format("tid:{2} {0} {1}", ld.level, ld.text, ld.threadid);
                    streamWriter.WriteLine(text);
                    ld.text = "";
                    poolList.Enqueue(ld);
                }
                streamWriter.Flush();
            }
            Thread.Sleep(200);
            if (isStop)
            {
                break;
            }
        }
    }



    static string GetLogPath
    {
        get
        {
            if(CommonUtils.isUnityEditor)
                return Application.dataPath + "/../" + string.Format(Filename, "");
            else if(CommonUtils.isAndroid || CommonUtils.isIOS)
                return Application.persistentDataPath + "/" + string.Format(Filename, "");
            else
                return Application.dataPath + "/../" + string.Format(Filename, "");
        }
    }

    public static bool IsStop
    {
        set
        {
            isStop = value;
            UpdateLogThread();
            
        }
    }
}
