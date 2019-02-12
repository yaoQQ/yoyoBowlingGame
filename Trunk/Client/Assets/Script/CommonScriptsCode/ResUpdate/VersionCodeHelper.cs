using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionCodeHelper
{
    public enum Platform
    {
        PC=1,
        Android=2,
        IOS=3
    }

    public enum Operator
    {
        A=1,
        B=2,
        C =3
    }

    public static int GetVersionCode(Platform platformValue, Operator operatorValue, int buildNum)
    {
        return (int)platformValue * 100000 + (int)operatorValue * 1000 + buildNum;
    }

    public static Platform GetPlatformByVersion(int code)
    {
        return (Platform) Mathf.FloorToInt( code / 100000);
    }

    /// <summary>
    /// 获取版本号
    /// </summary>
    public static int GetVersionsID(int code)
    {
        Debug.Log("[GetVersionsID]code ===================================="+code);
        //string codeStr = code.ToString().Substring(-2);
        return code;
        //return int.Parse(codeStr);
    }
}
