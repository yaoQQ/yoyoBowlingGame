using System;
using UnityEngine;
using XLua;

public enum ShareType
{
    Text = 0,
    Image = 1,
    Music = 2,
    Video = 3,
    Webpage = 4
}

[LuaCallCSharp]
public class PlatformSDK
{

    public static void ActiveInitializeUI()
    {
#if UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.ActiveInitializeUI();
#endif
    }

    public static string GetSimOperator()
    {
#if UNITY_EDITOR
        return "未知";
#elif UNITY_ANDROID
        return AndroidSDK.Instance.GetSimOperator();
#elif UNITY_IPHONE || UNITY_IOS
        return "未知";//IOSSDK.GetSimOperator();
#else
        return "未知";
#endif
    }

    public static bool IsWXAppInstalled()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID
        return AndroidSDK.Instance.IsWXAppInstalled();
#elif UNITY_IPHONE || UNITY_IOS
        return IOSSDK.IsWXAppInstalled();
#else
        return false;
#endif
    }

    /// <summary>
    /// 微信授权
    /// </summary>
    public static void WxSendAuth()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.WxSendAuth();
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.WxSendAuth();
#endif
    }

    /// <summary>
    /// 微信分享
    /// </summary>
    /// <param name="shareType">分享内容的类型</param>
    /// <param name="mode">分享模式 0：分享到微信好友 1：分享到微信朋友圈</param>
    /// <param name="url">文件路径</param>
    /// <param name="title">分享标题</param>
    /// <param name="description">分享描述</param>
    /// <param name="bmpUrl">分享的缩略图</param>
    public static void WxShare(int shareType, int mode, string url, string title, string description, string bmpUrl,
        Action succeedCallback, Action cancelCallback, Action failCallback)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.WxShare((ShareType)shareType, mode, url, title, description, bmpUrl,
            succeedCallback, cancelCallback, failCallback);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.WxShare((ShareType)shareType, mode, url, title, description, bmpUrl, succeedCallback, cancelCallback, failCallback);
#endif
        Loger.PrintLog("微信分享=====shareType==>>" + shareType + "=============description=====================>>>" + description + "===============mode=====================>>>" + mode+ "===============url=====================>>>" + url);
    }

    /// <summary>
    /// 微信充值
    /// </summary>
    public static void WxPay(string prepayId, string nonceStr, string sign, string timeStamp, string packageValue, Action<string> callback)
    {
        Loger.PrintLog("微信充值");
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.WxPay(prepayId, nonceStr, sign, timeStamp, packageValue, callback);
#elif UNITY_IPHONE || UNITY_IOS
        //IOSSDK.WxPay(prepayId, nonceStr, sign, timeStamp, packageValue, callback);
#endif
    }

    /// <summary>
    /// 支付宝授权
    /// </summary>
    public static void AlipaySendAuth(string authInfo)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.AlipaySendAuth(authInfo);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.AlipaySendAuth(authInfo);
#endif
    }

    /// <summary>
    /// 支付宝充值
    /// </summary>
    public static void AliPay(string orderInfo, Action<string> callback)
    {

#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.AliPay(orderInfo, callback);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.AliPay(orderInfo,callback);
#endif
    }

    /// <summary>
    /// QQ登录
    /// </summary>
    public static void QQLogin()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        //AndroidSDK.Instance.QQLogin();
#elif UNITY_IPHONE || UNITY_IOS
        //IOSSDK.QQLogin();
#endif
    }

    public static void RestartApp()
    {
#if UNITY_EDITOR
        Driver.Instance.QuitGame();
#elif UNITY_ANDROID
        AndroidSDK.Instance.RestartApp();
#elif UNITY_IPHONE || UNITY_IOS
        Driver.Instance.QuitGame();
#endif
    }

    /// <summary>
    /// 开始定位
    /// </summary>
    public static void StartLocation(Action<string> callback)
    {
        Debug.Log("@@@@@@@@@@@@@@@@@@开始定位");
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.StartLocation(callback);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.StartLocation(callback);
#endif
    }

    /// <summary>
    /// 打开外部高德地图App导航
    /// </summary>
    public static void OpenGaodeMapApp(float fromLng, float fromLat, string fromName, float toLng, float toLat, string toName, Action<string> callback)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.OpenGaodeMapApp(fromLng, fromLat, fromName, toLng, toLat, toName, callback);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.OpenGaodeMapApp(fromLng, fromLat, fromName, toLng, toLat, toName, callback);
#endif
    }

    /// <summary>
    /// 获取照片
    /// </summary>
    public static void TakePhonePhoto(bool isFromCamera, Action<byte[]> callback, bool isCut = false, int width = 1024, int height = 1024)
    {
#if UNITY_EDITOR
        string msg = UtilMethod.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/tempImage.jpg");
        if (!System.IO.File.Exists(msg))
            return;
        string path = UtilMethod.ConnectStrs("file://", msg);
        WWWManager.Instance.load(path, (www) =>
        {
            if (callback != null)
            {
                callback(www.bytes);
            }
        });
#elif UNITY_ANDROID
        AndroidSDK.Instance.TakePhonePhoto(isFromCamera, callback, isCut, width, height);
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.TakePhonePhoto(isFromCamera, callback, isCut, width, height);
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    //private static UniWebView m_uniWebView = null;
#endif
    /// <summary>
    /// 打开内嵌网页
    /// </summary>
    /// <param name="url"></param>
    public static void OpenUniWebView(string url)
    {
/*#if UNITY_ANDROID || UNITY_IOS
        if (m_uniWebView == null)
        {
            m_uniWebView = Driver.Instance.gameObject.AddComponent<UniWebView>();
            m_uniWebView.autoShowWhenLoadComplete = true;
        }
        m_uniWebView.insets = new UniWebViewEdgeInsets(200, 200, 200, 200);
        m_uniWebView.url = url;
        m_uniWebView.Load();
#else
        Loger.PrintLog("当前平台不支持打开内嵌网页，改为弹出网页");
        Application.OpenURL(url);
#endif*/
    }

    /// <summary>
    /// 选择日期
    /// </summary>
    public static void PickDate(Action<string> callback)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.PickDate(callback);
#elif UNITY_IPHONE || UNITY_IOS
        //IOSSDK.PickDate(callback);
#endif
    }

    /// <summary>
    /// 选择时间
    /// </summary>
    public static void PickTime(Action<string> callback)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.PickTime(callback);
#elif UNITY_IPHONE || UNITY_IOS
        //IOSSDK.PickTime(callback);
#endif
    }

    /// <summary>
    /// 选择日期时间
    /// </summary>
    public static void PickDateTime(Action<string> callback)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.PickDateTime(callback);
#elif UNITY_IPHONE || UNITY_IOS
        //IOSSDK.PickDateTime(callback);
#endif
    }

    private static bool _currStateBarColor;//当前stateBar颜色
    //显示、关闭状态栏
    public static void ShowStatusBar(bool isShow, bool isWhite = false) {
        Screen.fullScreen = !isShow;
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.ShowStatusBar(isShow , isWhite); 
#elif UNITY_IPHONE || UNITY_IOS
        IOSSDK.Instance.ShowStatusBar(isShow,isWhite);
#endif
        _currStateBarColor = isWhite;
    }
     //设置状态栏颜色       
    public static void SetStatusBarColor(bool isWhite = false) {
        if (_currStateBarColor == isWhite) {//相同颜色不更新刷新stateBar
            return;
        }
#if UNITY_EDITOR
#elif UNITY_ANDROID
              //  AndroidSDK.Instance.SetStatusBarColor(isShow);
#elif UNITY_IPHONE || UNITY_IOS
                //TODO
                IOSSDK.Instance.SetStatusBarColor(isWhite);
#endif
        _currStateBarColor = isWhite;
    }

    /// <summary>扫码</summary>
    public static void ScanQRCode(Action<string> onScannerMessage, Action<string> onScannerEvent, Action<string> onDecoderMessage)
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        AndroidSDK.Instance.ScanQRCode(onScannerMessage);
#elif UNITY_IPHONE || UNITY_IOS
        EasyCodeScanner.Initialize();
        EasyCodeScanner.OnScannerMessage = onScannerMessage;
        EasyCodeScanner.OnScannerEvent = onScannerEvent;
        EasyCodeScanner.OnDecoderMessage = onDecoderMessage;
        EasyCodeScanner.launchScanner(true, "请对准二维码，轻触照亮", -1, false);
#endif
    }
}