using System;
using System.Runtime.InteropServices;
using UnityEngine;
using SimpleJSON;

//相关SDK的方法


public enum SDKCallBackEvent
{
    OnWxAuthSucceed,
    OnWxAuthFail,
}


public class IOSSDK : MonoBehaviour
{
    private static IOSSDK m_instance = null;

    public static IOSSDK Instance
    {
        get
        {
            return m_instance;
        }
    }

    //引入内部动态链接库函数
    [DllImport("__Internal")]
    private static extern void sdk_Init();

    [DllImport("__Internal")]
    private static extern void activeInitializeUI();

    [DllImport("__Internal")]
    private static extern bool is_wx_app_installed();
    [DllImport("__Internal")]
    private static extern void wx_send_auth();
    //微信支付
    [DllImport("__Internal")]
    private static extern void wx_pay(string appid, string partnerid, string prepayid, string noncster, string time, string sign);

    //[DllImport("__Internal")]
    //private static extern void shareToWechat(int shareType, int mode, string url, string title, string description, string bmpUrl);

    //支付宝授权
    [DllImport("__Internal")]
    private static extern void alipay_send_auth(string authInfo);
    //支付宝支付
    [DllImport("__Internal")]
    private static extern void alipay_pay(string param);

    //拍照相册
    [DllImport("__Internal")]
    private static extern void _iosOpenPhotoAlbums_allowsEditing(string param);//相册
    [DllImport("__Internal")]
    private static extern void _iosOpenCamera_allowsEditing(string param);//拍照
    [DllImport("__Internal")]
    private static extern void _iosSaveImageToPhotosAlbum(string readAddr);//保存到相册



    //地图
    [DllImport("__Internal")]
    private static extern void start_Location();//开启地图导航
    [DllImport("__Internal")]
    private static extern void openGaodeMapApp(string data);//打开高德APP

    //显示、关闭状态栏
    [DllImport("__Internal")]
    private static extern void _showStatusBar(string data,string isWhite);//显示、关闭状态栏
     //设置状态栏颜色                 
    [DllImport("__Internal")]
    private static extern void _showStatusBarColor(string isWhite);//true白色字体，false黑色字体

    ////// C#调用OC 统一调用
    [DllImport("__Internal")]
    private static extern void call_native(string str);//之后考虑合并位json格式 :{平台：数据，参数等} 
    ////// C#调用OC 统一调用
    [DllImport("__Internal")]
    private static extern string get_native_value(string str);//传回数据考虑string为json格式  

    /*[DllImport("__Internal")]
    private static extern void takePhotoFromCamera();
    [DllImport("__Internal")]
    private static extern void takePhotoFromAlbum();*/
    void Awake() {
        m_instance = this;
        sdk_Init();

    }

    #region  testFrame
    //调用SDK相关功能
    private string CallIOSMethod(string methodName) {
        return get_native_value(methodName);
    }
    #endregion

    public void ActiveInitializeUI()
    {
        activeInitializeUI();
    }

    #region  微信

    public static bool IsWXAppInstalled() {
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
        return is_wx_app_installed();
#else
        return false;
#endif
    }

  //  private bool isSHow = false;
  //  private void OnGUI()
  //  {
   //     if(GUI.Button(new Rect(220,150,150,100),"Test="+isSHow)){
    //        isSHow = !isSHow;
    //        ShowStatusBar(isSHow);
           
     //   }
    //}

    public static bool isToShow = false;
    /// <summary>
    /// 微信授权
    /// </summary>
    public static void WxSendAuth() {
        wx_send_auth();
    }

    /// <summary>
    /// 微信授权成功
    /// </summary>
    private void OnWxAuthSucceed(string authCode) {
        OnPrintLog("微信登录授权码：" + authCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_WxAuthSucceed, authCode);

    }

    /// <summary>
    /// 微信授权失败
    /// </summary>
    private void OnWxAuthFail(string errCode) {
        OnPrintLog("微信授权失败：" + errCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_WxAuthFail, errCode);
    }

    private Action m_wxShareSucceedCallback = null;
    private Action m_wxShareCancelCallback = null;
    private Action m_wxShareFailCallback = null;
    /// <summary>
    /// 微信分享
    /// </summary>
    /// <param name="shareType">分享内容的类型</param>
    /// <param name="mode">分享模式 0：分享到微信好友 1：分享到微信朋友圈</param>
    /// <param name="url">文件路径</param>
    /// <param name="title">分享标题</param>
    /// <param name="description">分享描述</param>
    /// <param name="bmpUrl">分享的缩略图</param>
    public void WxShare(ShareType shareType, int mode, string url, string title, string description, string bmpUrl,
    Action succeedCallback, Action cancelCallback, Action failCallback) {
        m_wxShareSucceedCallback = succeedCallback;
        m_wxShareCancelCallback = cancelCallback;
        m_wxShareFailCallback = failCallback;
        SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
        json["IOSMethod"] = new JSONData("shareToWechat");
        json["shareType"] = new JSONData((int)shareType);
        json["mode"] = new JSONData(mode);
        json["url"] = new JSONData(url);
        json["title"] = new JSONData(title);
        json["description"] = new JSONData(description);
        json["bmpUrl"] = new JSONData(bmpUrl);
        string CallIOSMethodName = "";
        switch (shareType) {
            case ShareType.Text:
                CallIOSMethodName = "shareMessage";
                // CallAndroidMethod("wxShareMessage", mode, description);
                break;
            case ShareType.Image:
                 CallIOSMethodName = "shareImage";
                //  CallAndroidMethod("wxShareImage", mode, url);
                break;
            case ShareType.Music:
                CallIOSMethodName = "shareMusic";
                // CallAndroidMethod("wxShareMusic", mode, url, title, description, bmpUrl);
                break;
            case ShareType.Video:
                CallIOSMethodName = "shareVideo";
                // CallAndroidMethod("wxShareVideo", mode, url, title, description, bmpUrl);
                break;
            case ShareType.Webpage:
                CallIOSMethodName = "shareWebpage";
                //  CallAndroidMethod("wxShareWebpage", mode, url, title, description, bmpUrl);
                break;
        }
        json["action"] = CallIOSMethodName;
        call_native(json.ToString());
    }
    void OnWxShareSucceed(string result) {
        OnPrintLog("分享成功");
        if (m_wxShareSucceedCallback != null) {
            m_wxShareSucceedCallback();
            m_wxShareSucceedCallback = null;
        }
    }

    void OnWxShareCancel(string result) {
        OnPrintLog("分享取消");
        if (m_wxShareCancelCallback != null) {
            m_wxShareCancelCallback();
            m_wxShareCancelCallback = null;
        }
    }

    void OnWxShareFail(string result) {
        OnPrintLog("分享失败" + result);
        if (m_wxShareFailCallback != null) {
            m_wxShareFailCallback();
            m_wxShareFailCallback = null;
        }
    }
    /// <summary>
    /// 微信支付回调
    /// </summary>

    private Action<string> WxPayCallBack = null;
    public void WxPay(string appid, string partnerid, string prepayid, string noncster, string time, string sign, Action<string> callback) {
        wx_pay(appid, partnerid, prepayid, noncster, time, sign);
        WxPayCallBack = callback;
    }

    private void WxPayIosCallBack(string msg) {
        Loger.PrintLog("WxPayIosCallBack:" + msg);
        if (WxPayCallBack != null) {
            WxPayCallBack(msg);
            WxPayCallBack = null;
        }
    }
    #endregion
    
    #region 支付宝
    /// <summary>
    /// 支付宝授权
    /// </summary>
    public void AlipaySendAuth(string authInfo)
    {
        Loger.PrintLog("支付宝授权：" + authInfo);
        alipay_send_auth(authInfo);
    }

    /// <summary>
    /// 支付宝授权成功
    /// </summary>
    private void OnAlipayAuthSucceed(string authCode)
    {
        Loger.PrintLog("支付宝登录授权码：" + authCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_AlipayAuthSucceed, authCode);
    }

    /// <summary>
    /// 支付宝授权失败
    /// </summary>
    private void OnAlipayAuthFail(string errCode)
    {
        Loger.PrintError("支付宝授权失败：" + errCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_AlipayAuthFail, errCode);
    }

    private Action<string> m_aliPayCallback = null;
    public  void AliPay(string orderInfo, Action<string> callback) {
        alipay_pay(orderInfo);
        m_aliPayCallback = callback;
    }

    private void OnAliPay(string msg)
    {
        Loger.PrintLog("OnAliPay:" + msg);
        if (m_aliPayCallback != null)
        {
            m_aliPayCallback(msg);
            m_aliPayCallback = null;
        }
    }

    #endregion

    private void OnPrintLog(string msg) {
        Loger.PrintLog("IOS:" + msg);
    }

    #region  高德
    private static Action<string> m_locationCallback = null;
    public static void StartLocation(Action<string> callback) {
        m_locationCallback = callback;
        start_Location();
    }

    private void OnLocation(string msg) {
        OnPrintLog("OnLocation:" + msg);
        if (m_locationCallback != null) {
            m_locationCallback(msg);
            m_locationCallback = null;
        }
    }

    /// <summary>
    /// 打开外部高德地图App导航
    /// </summary>
    private Action<string> m_openGaodeMapAppCallback = null;
    public void OpenGaodeMapApp(float fromLng, float fromLat, string fromName, float toLng, float toLat, string toName, Action<string> callback) {
        m_openGaodeMapAppCallback = callback;
        OnPrintLog("打开外部高德地图App导航");
        SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
        json["fromLng"] = new JSONData(fromLng);
        json["fromLat"] = new JSONData(fromLat);
        json["fromName"] = new JSONData(fromName);
        json["toLng"] = new JSONData(toLng);
        json["toLat"] = new JSONData(toLat);
        json["toName"] = new JSONData(toName);
        openGaodeMapApp(json.ToString());
    }

    private void OnOpenGaodeMapApp(string msg) {
        OnPrintLog("打开外部高德地图App导航  返回 msg=" + msg);
        OnPrintLog("OnOpenGaodeMapApp:" + msg);
        if (m_openGaodeMapAppCallback != null) {
            m_openGaodeMapAppCallback(msg);
            m_openGaodeMapAppCallback = null;
        }
    }
    #endregion

    #region 相册、拍照
    ///// <summary>
    ///// 打开照片
    ///// </summary>
    ///// <param name="allowsEditing"></param>
    //public static void iosOpenPhotoLibrary() {
    //   // _iosOpenPhotoLibrary_allowsEditing();
    //}
    /// <summary>
    /// 打开相册
    /// </summary>
    /// <param name="allowsEditing"></param>
    public static void iosOpenPhotoAlbums(bool allowEditong, int width, int height) {
        SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
        json["allowEditong"] = new JSONData(allowEditong);
        json["width"] = new JSONData(width);
        json["height"] = new JSONData(height);
        _iosOpenPhotoAlbums_allowsEditing(json.ToString());
        //isToShow = !isToShow;
        //IOSSDK.Instance.ShowStatusBar(isToShow);
    }
    /// <summary>
    /// 打开相机
    /// </summary>
    /// <param name="allowsEditing"></param>
    public static void iosOpenCamera(bool allowEditong, int width, int height) {
        SimpleJSON.JSONClass json = new SimpleJSON.JSONClass();
        json["allowEditong"] = new JSONData(allowEditong);
        json["width"] = new JSONData(width);
        json["height"] = new JSONData(height);
        _iosOpenCamera_allowsEditing(json.ToString());
      //  IOSSDK.Instance.WxShare(0, 1, "htt\\:www.hao123.com", "title:hellowixin", "是文字 分享", "yupian Url:123213", null, null, null);

    }
    /// <summary>
    /// 保存图片到相册
    /// </summary>
    /// <param name="readAddr"></param>
    public static void iosSaveImageToPhotosAlbum(string readAddr) {
        //    string path = Application.persistentDataPath + "/lzhscreenshot.png";
        //    Debug.Log(path);
        //    byte[] bytes = (rawImage.texture as Texture2D).EncodeToPNG();
        //    System.IO.File.WriteAllBytes(path, bytes);

        //   iosSaveImageToPhotosAlbum(path);
        _iosSaveImageToPhotosAlbum(readAddr);
    }


    /// <summary>
    /// 将ios传过的string转成u3d中的texture
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public static Texture2D Base64StringToTexture2D(string base64) {
        Texture2D tex = new Texture2D(4, 4, TextureFormat.ARGB32, false);
        try {
            byte[] bytes = System.Convert.FromBase64String(base64);
            tex.LoadImage(bytes);
        }
        catch (System.Exception ex) {
            Debug.LogError(ex.Message);
        }
        return tex;
    }

    public System.Action<string> CallBack_PickImage_With_Base64;
    public System.Action<string> CallBack_ImageSavedToAlbum;
    /// <summary>
	/// 打开相册相机后的从ios回调到unity的方法
	/// </summary>
	/// <param name="base64">Base64.</param>
	void PickImageCallBack_Base64(string base64) {
        //if (CallBack_PickImage_With_Base64 != null) {
        //    CallBack_PickImage_With_Base64(base64);
        //}
        callback_PickImage_With_Base64Fun(base64);
    }

    //在目录生成tempImage图片
    void callback_PickImage_With_Base64Fun(string base64) {
        Texture2D tex = Base64StringToTexture2D(base64);//传过来的图片生成
        string path = Application.persistentDataPath + "/tempImage.jpg";
        Debug.Log(path);
        byte[] bytes = (tex as Texture2D).EncodeToJPG();
        System.IO.File.WriteAllBytes(path, bytes);
        
        if (m_takePhonePhotoCallback != null) {
            m_takePhonePhotoCallback(bytes);
            m_takePhonePhotoCallback = null;
        }
    }
    /// <summary>
    /// 保存图片到相册后，从ios回调到unity的方法
    /// </summary>
    /// <param name="msg">Message.</param>
    void SaveImageToPhotosAlbumCallBack(string msg) {
        Debug.Log(" -- msg: 保存图片到相册" + msg);
        if (CallBack_ImageSavedToAlbum != null) {
            CallBack_ImageSavedToAlbum(msg);
        }
    }

    //---------------------------------------------------------
    private static Action<byte[]> m_takePhonePhotoCallback = null;
    public static void TakePhonePhoto(bool isFromCamera, Action<byte[]> callback, bool isCut, int width, int height) {
        m_takePhonePhotoCallback = callback;

        if (isFromCamera) {
            Loger.PrintLog("相机拍照");
            iosOpenCamera(isCut, width, height);
        }
        else {
            Loger.PrintLog("相册选择");
            iosOpenPhotoAlbums(isCut, width, height);
        }

    }

    private void OnTakePhonePhoto(string msg) {
        Loger.PrintLog("获取图片返回：" + msg);
        string path = UtilMethod.ConnectStrs("file://", Application.persistentDataPath, "/tempImage.jpg");
        WWWManager.Instance.load(path, (www) => {
            if (m_takePhonePhotoCallback != null) {
                m_takePhonePhotoCallback(www.bytes);
                m_takePhonePhotoCallback = null;
            }
        });
    }
    #endregion

    //显示、关闭状态栏
    public  void ShowStatusBar(bool isShow,bool isWhite=false) {
        OnPrintLog("ShowStatusBar isShow="+ isShow);
        _showStatusBar(isShow.ToString(),isWhite.ToString());
    }
    //显示、关闭状态栏
    public void SetStatusBarColor(bool isWhite) {
        OnPrintLog("SetStatusBarColor isWhite=" + isWhite);
        _showStatusBarColor(isWhite.ToString());
    }
}