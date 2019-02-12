using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class TimeManager
{
    private static DateTime m_defaultTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
    private static int m_timeZone = 8;
    /// <summary>心跳返回的服务器时间(秒)</summary>
    private static uint m_heartbeatServerTime = 0;
    /// <summary>心跳返回时的客户端时间(秒)</summary>
    private static float m_heartbeatClientTime = 0f;

    /// <summary>
    /// 设置心跳返回的服务器时间
    /// </summary>
    /// <param name="serverTime"></param>
    public static void SetHeartbeatServerTime(uint heartbeatServerTime)
    {
        m_heartbeatServerTime = heartbeatServerTime;
        m_heartbeatClientTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    public static DateTime GetServerDateTime()
    {
        return m_defaultTime.AddSeconds((double)m_heartbeatServerTime + Time.realtimeSinceStartup - m_heartbeatClientTime + m_timeZone * 3600);
    }

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    public static uint GetServerUnixTime()
    {
        return m_heartbeatServerTime + (uint)Time.realtimeSinceStartup - (uint)m_heartbeatClientTime;
    }

    /// <summary>
    /// 根据UnixTime获取DateTime
    /// </summary>
    public static DateTime GetDateTimeByUnixTime(uint unixTime)
    {
        return m_defaultTime.AddSeconds(unixTime + m_timeZone * 3600);
    }

    /// <summary>
    /// 根据DateTime获取UnixTime
    /// </summary>
    public static uint GetUnixTimeByDateTime(DateTime dateTime)
    {
        return (uint)(dateTime - m_defaultTime.AddSeconds(m_timeZone * 3600)).TotalSeconds;
    }
    /// <summary>
    /// 根据DateTime获取UnixTime
    /// </summary>
    public static string GetlocalTime() {
        var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();

    }
}