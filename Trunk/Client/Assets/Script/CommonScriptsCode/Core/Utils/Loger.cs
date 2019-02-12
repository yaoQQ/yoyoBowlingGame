using UnityEngine;
using XLua;

[LuaCallCSharp]
public class Loger
{
    public static void Print(LogType logType, params string[] msgs)
    {
        Logger.Print(logType, msgs);
    }

    public static void PrintDebug(params string[] msgs)
    {
        Logger.PrintDebug(msgs);
    }

    public static void PrintLog(params string[] msgs)
    {
        Logger.PrintLog(msgs);
    }

    public static void PrintWarning(params string[] msgs)
    {
        Logger.PrintWarning(msgs);
    }

    public static void PrintError(params string[] msgs)
    {
        Logger.PrintError(msgs);
    }
    
    public static void PrintColor(string color, params string[] msgs)
    {
        Logger.PrintColor(color, msgs);
    }

    public static bool IsStop
    {
        set
        {
            Logger.IsStop = value;
        }
    }
}