using UnityEngine;
using System.Text;
using XLua;
using System;

[LuaCallCSharp]
public class UtilMethod 
{
    /// <summary>是否游戏打入Base包</summary>
    public static bool isIncludeGame = false;

    /// <summary>是否跳过更新</summary>
    public static bool isSkipUpdate = false;

    /// <summary>是否连接测试服务器</summary>
    public static bool isTestServer = false;

    /// <summary>是否连接测试CDN</summary>
    public static bool isBetaCDN = false;

    /// <summary>渠道号</summary>
    public static int channel = 0;

    public static bool IsUnityEditor()
    {
        return Application.isEditor;
    }

    public static bool isAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }
    public static bool isIOS()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }

    /// <summary>
    /// 是否是超前版本（预发版）
    /// </summary>
    public static bool IsSuperVersion()
    {
        return GameVersionManager.localVersion.versionCode > GameVersionManager.serverVersion.versionCode;
    }

    /// <summary>
    /// 获取当前版本号
    /// </summary>
    public static string GetCurVersion()
    {
        return GameVersionManager.updateVersion.version;
    }

    /// <summary>
    /// 获取persistent目录
    /// </summary>
    public static string GetPersistentDataRootPath()
    {
        return CommonPathUtils.PERSISTENT_DATA_ROOT_PATH;
    }

    /// <summary>
    /// 清除本地缓存
    /// </summary>
    public static void CleanCache(string path)
    {
        IOUtil.ClearDirectory(path);
    }

    public static string ConnectStrs(params string[] strs)
    {
        StringBuilder sb = new StringBuilder();
        int len = strs.Length;
        for (int i = 0; i < len; ++i)
            sb.Append(strs[i]);
        return sb.ToString();
    }

    public static string Md5Sum(string input)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

	public static TimeSpan GetDeltaDataTime(DateTime left, DateTime right)
	{
		return left - right;
	}

	public static TimeSpan ParseFromFloat(float curTime)
	{
		var hour = (int)curTime / 3600;
		var minute = ((int)curTime - hour * 3600) / 60;
		var second = (int)curTime - hour * 3600 - minute * 60;
		var millisecond = (int)((curTime - (int)curTime) * 1000);
		var normalize = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
		TimeSpan time;
		if (TimeSpan.TryParse(normalize, out time))
			return time;
		else
			return new TimeSpan();
	}

    public static Vector2 GetScreenWidthHeight()
    {
        return new Vector2(Screen.width, Screen.height);
    }

    public static Texture2D GetQrCode(string text, int size = 256)
    {
        return QrCode.Encode(text, size);
    }

    public static string GetMD5HashFromString(string str)
    {
        return MD5Util.GetMD5HashFromString(str);
    }

    public static void SetSendGMResult(bool isSucceed)
    {
        DebugConsole.Instance.SetSendGMResult(isSucceed);
    }

    public static bool PhysicsRaycast(Ray ray, out RaycastHit hit)
    {
        bool isHit = Physics.Raycast(ray, out hit);
        return isHit;
    }

    public static void SwitchScreenOrientation(bool isHorizontal)
    {
        if (isHorizontal)
        {
            Logger.PrintLog("转横屏");
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            UIViewManager.Instance.SwitchScreenOrientation(isHorizontal);
        }
        else
        {
            Logger.PrintLog("转竖屏");
            Screen.orientation = ScreenOrientation.Portrait;
            UIViewManager.Instance.SwitchScreenOrientation(isHorizontal);
        }
    }
}
