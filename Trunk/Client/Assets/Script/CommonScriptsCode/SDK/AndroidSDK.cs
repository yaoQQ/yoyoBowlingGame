using System;
using UnityEngine;

public class AndroidSDK : MonoBehaviour
{
    private static AndroidSDK m_instance = null;

#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject m_ajo = null;
#endif

    public static AndroidSDK Instance
    {
        get
        {
            return m_instance;
        }
    }

    void Awake()
    {
        m_instance = this;
#if UNITY_ANDROID && !UNITY_EDITOR
        m_ajo = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
#endif
    }

    private void CallAndroidMethod(string methodName, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_ajo == null)
            return;
        m_ajo.Call(methodName, args);
#endif
    }

    private void OnPrintLog(string msg)
    {
        Loger.PrintLog(msg);
    }

    public string GetSimOperator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return m_ajo.Call<string>("getSimOperator");
#else
        return "";
#endif
    }

    public bool IsWXAppInstalled()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return m_ajo.Call<bool>("isWXAppInstalled");
#else
        return false;
#endif
    }

    /// <summary>
    /// 微信授权
    /// </summary>
    public void WxSendAuth()
    {
        CallAndroidMethod("wxSendAuth");
    }

    /// <summary>
    /// 微信授权成功
    /// </summary>
    private void OnWxAuthSucceed(string authCode)
    {
        Loger.PrintLog("微信登录授权码：" + authCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_WxAuthSucceed, authCode);
    }

    /// <summary>
    /// 微信授权失败
    /// </summary>
    private void OnWxAuthFail(string errCode)
    {
        Loger.PrintError("微信授权失败：" + errCode);
        NoticeManager.Instance.Dispatch(NoticeType.Login_WxAuthFail, errCode);
    }

    private Action m_wxShareSucceedCallback = null;
    private Action m_wxShareCancelCallback = null;
    private Action m_wxShareFailCallback = null;
    public void WxShare(ShareType shareType, int mode, string url, string title, string description, string bmpUrl,
        Action succeedCallback, Action cancelCallback, Action failCallback)
    {
        m_wxShareSucceedCallback = succeedCallback;
        m_wxShareCancelCallback = cancelCallback;
        m_wxShareFailCallback = failCallback;
        switch (shareType)
        {
            case ShareType.Text:
                CallAndroidMethod("wxShareMessage", mode, description);
                break;
            case ShareType.Image:
                CallAndroidMethod("wxShareImage", mode, url);
                break;
            case ShareType.Music:
                CallAndroidMethod("wxShareMusic", mode, url, title, description, bmpUrl);
                break;
            case ShareType.Video:
                CallAndroidMethod("wxShareVideo", mode, url, title, description, bmpUrl);
                break;
            case ShareType.Webpage:
                CallAndroidMethod("wxShareWebpage", mode, url, title, description, bmpUrl);
                break;
        }
    }

    void OnWxShareSucceed(string result)
    {
        Loger.PrintLog("分享成功");
        if (m_wxShareSucceedCallback != null)
        {
            m_wxShareSucceedCallback();
            m_wxShareSucceedCallback = null;
        }
    }

    void OnWxShareCancel(string result)
    {
        Loger.PrintLog("分享取消");
        if (m_wxShareCancelCallback != null)
        {
            m_wxShareCancelCallback();
            m_wxShareCancelCallback = null;
        }
    }

    void OnWxShareFail(string result)
    {
        Loger.PrintLog("分享失败" + result);
        if (m_wxShareFailCallback != null)
        {
            m_wxShareFailCallback();
            m_wxShareFailCallback = null;
        }
    }

    private Action<string> m_wxPayCallback = null;
    /// <summary>
    /// 微信充值
    /// </summary>
    public void WxPay(string prepayId, string nonceStr, string sign, string timeStamp, string packageValue, Action<string> callback)
    {
        m_wxPayCallback = callback;
        CallAndroidMethod("wxPay", prepayId, nonceStr, sign, timeStamp, packageValue);
    }

    private void OnWxPay(string msg)
    {
        Loger.PrintLog("OnWxPay:" + msg);
        if (m_wxPayCallback != null)
        {
            m_wxPayCallback(msg);
            m_wxPayCallback = null;
        }
    }
    
    /// <summary>
    /// 支付宝授权
    /// </summary>
    public void AlipaySendAuth(string authInfo)
    {
        CallAndroidMethod("alipaySendAuth", authInfo);
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
    /// <summary>
    /// 支付宝充值
    /// </summary>
    public void AliPay(string orderInfo, Action<string> callback)
    {
        m_aliPayCallback = callback;
        CallAndroidMethod("aliPay", orderInfo);
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

    /// <summary>
    /// QQ登录
    /// </summary>
    public void QQLogin()
    {
        CallAndroidMethod("qqLogin");
    }

    public void RestartApp()
    {
        CallAndroidMethod("restartApp");
    }

    public void InstallApk(string path)
    {
        CallAndroidMethod("installApk", path);
    }

    private Action<string> m_locationCallback = null;
    public void StartLocation(Action<string> callback)
    {
        m_locationCallback = callback;
        CallAndroidMethod("startLocation");
    }

    private void OnLocation(string msg)
    {
        //Loger.PrintLog("OnLocation:" + msg);
        if (m_locationCallback != null)
        {
            m_locationCallback(msg);
        }
    }

    private Action<string> m_openGaodeMapAppCallback = null;
    public void OpenGaodeMapApp(float fromLng, float fromLat, string fromName, float toLng, float toLat, string toName, Action<string> callback)
    {
        m_openGaodeMapAppCallback = callback;
        CallAndroidMethod("openGaodeMapApp", fromLng, fromLat, fromName, toLng, toLat, toName);
    }

    private void OnOpenGaodeMapApp(string msg)
    {
        Loger.PrintLog("OnOpenGaodeMapApp:" + msg);
        if (m_openGaodeMapAppCallback != null)
        {
            m_openGaodeMapAppCallback(msg);
            m_openGaodeMapAppCallback = null;
        }
    }

    private Action<byte[]> m_takePhonePhotoCallback = null;
    public void TakePhonePhoto(bool isFromCamera, Action<byte[]> callback, bool isCut, int width, int height)
    {
        m_takePhonePhotoCallback = callback;
        if (isFromCamera)
        {
            Loger.PrintLog("相机拍照");
            CallAndroidMethod("takePhotoFromCamera", UtilMethod.ConnectStrs(Application.persistentDataPath, "/tempImage.jpg"), isCut, width, height);
        }
        else
        {
            Loger.PrintLog("相册选择");
            CallAndroidMethod("takePhotoFromAlbum", UtilMethod.ConnectStrs(Application.persistentDataPath, "/tempImage.jpg"), isCut, width, height);
        }
    }

    private void OnTakePhonePhoto(string msg)
    {
        Loger.PrintLog("获取图片返回：" + msg);
        string path = UtilMethod.ConnectStrs("file://", msg);
        WWWManager.Instance.load(path, (www) =>
        {
            if (m_takePhonePhotoCallback != null)
            {
                m_takePhonePhotoCallback(www.bytes);
                m_takePhonePhotoCallback = null;
            }
        });
    }

    private Action<string> m_pickDateCallback = null;
    public void PickDate(Action<string> callback)
    {
        m_pickDateCallback = callback;
        CallAndroidMethod("pickDate");
    }

    private void OnPickDate(string msg)
    {
        Loger.PrintLog("OnPickDate:" + msg);
        if (m_pickDateCallback != null)
        {
            m_pickDateCallback(msg);
            m_pickDateCallback = null;
        }
    }

    private Action<string> m_pickTimeCallback = null;
    public void PickTime(Action<string> callback)
    {
        m_pickTimeCallback = callback;
        CallAndroidMethod("pickTime");
    }

    private void OnPickTime(string msg)
    {
        Loger.PrintLog("OnPickTime:" + msg);
        if (m_pickTimeCallback != null)
        {
            m_pickTimeCallback(msg);
            m_pickTimeCallback = null;
        }
    }

    private Action<string> m_pickDateTimeCallback = null;
    public void PickDateTime(Action<string> callback)
    {
        m_pickDateTimeCallback = callback;
        CallAndroidMethod("pickDateTime");
    }

    private void OnPickDateTime(string msg)
    {
        Loger.PrintLog("OnPickDateTime:" + msg);
        if (m_pickDateTimeCallback != null)
        {
            m_pickDateTimeCallback(msg);
            m_pickDateTimeCallback = null;
        }
    }

    private Action<string> m_wav2AmrCallback = null;
    public void Wav2Amr(string wavFileName, string amrFileName, Action<string> callback)
    {
        m_wav2AmrCallback = callback;
        CallAndroidMethod("wav2amr", wavFileName, amrFileName);
    }

    private void OnWav2Amr(string msg)
    {
        if (m_wav2AmrCallback != null)
        {
            m_wav2AmrCallback(msg);
            m_wav2AmrCallback = null;
        }
    }

    public void ShowStatusBar(bool isShow,bool isWhite)
    {
        CallAndroidMethod("showStatusBar", isShow , isWhite);
    }
    public void ClickDeviceReturn(string msg) {
        Loger.PrintLog("ClickDeviceReturn  点击返回按钮" + msg);
       
        UIViewManager.Instance.CloseStackTopView();
    }

    private Action<string> m_scanQRCodeCallback = null;
    public void ScanQRCode(Action<string> callback)
    {
        m_scanQRCodeCallback = callback;
        CallAndroidMethod("scanQRCode");
    }

    private void OnScanQRCode(string msg)
    {
        Loger.PrintLog("OnScanQRCode:" + msg);
        if (m_scanQRCodeCallback != null)
        {
            m_scanQRCodeCallback(msg);
            m_scanQRCodeCallback = null;
        }
    }
}