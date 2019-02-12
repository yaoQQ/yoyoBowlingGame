using UnityEngine;
using XLua;

[LuaCallCSharp]
public class DeviceUtil
{
    /// <summary>
    /// 获取操作系统
    /// </summary>
    public static string GetOperatingSystem()
    {
        return SystemInfo.operatingSystem;
    }

    /// <summary>
    /// 获取内存大小
    /// </summary>
    public static int GetSystemMemorySize()
    {
        return SystemInfo.systemMemorySize;
    }

    /// <summary>
    /// 获取处理器类型
    /// </summary>
    public static string GetProcessorType()
    {
        return SystemInfo.processorType;
    }

    /// <summary>
    /// 获取设备型号
    /// </summary>
    public static string GetDeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    /// <summary>
    /// 获取设备唯一码
    /// </summary>
    public static string GetDeviceUniqueIdentifier()
    {
        string deviceUnique = SystemInfo.deviceUniqueIdentifier;
        Logger.PrintLog("deviceUnique======================" + deviceUnique);
        return deviceUnique;
    }
}
