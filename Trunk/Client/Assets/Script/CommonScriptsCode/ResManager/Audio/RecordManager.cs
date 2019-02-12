using System.Collections.Generic;
using UnityEngine;
using XLua;
//using CloudVoiceIM;

[LuaCallCSharp]
public class RecordManager
{
    private class RecordInfo
    {
        public string fileName;
        public string filePath;
        public uint time;
    }

    private static bool m_isLogin = false;
    private static bool m_isRecording = false;
    private static Dictionary<string, RecordInfo> m_recordDict = new Dictionary<string, RecordInfo>();

    public static void Init()
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*EventListenerManager.AddListener(ProtocolEnum.IM_RECORD_VOLUME_NOTIFY, ImRecordVolume);//录音音量大小回调监听
        int init = CloudVoiceImSDK.instance.Init(0, 1002832, Application.persistentDataPath, false);
        if (init == 0)
        {
            Logger.PrintLog("语音初始化成功...");
        }
        else
        {
            Debug.LogError("语音初始化失败...");
        }*/
    }

    private static void ImRecordVolume(object data)
    {
        /*ImRecordVolumeNotify RecordVolumeNotify = data as ImRecordVolumeNotify;
        Logger.PrintLog("ImRecordVolumeNotify:v_volume=" + RecordVolumeNotify.v_volume);*/
    }

    /// <summary>
    /// 登录呀呀云
    /// </summary>
    /// <param name="filePath"></param>
    public static void LoginRecord(string nickname, string uid)
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*Logger.PrintLog("登录呀呀云");
        string ttFormat = "{{\"nickname\":\"{0}\",\"uid\":\"{1}\"}}";
        string tt = string.Format(ttFormat, nickname, uid);
        string[] wildcard = new string[2];
        wildcard[0] = "0x001";
        wildcard[1] = "0x002";
        CloudVoiceImSDK.instance.Login(tt, "1", wildcard, 0, (data) =>
        {
            if (data.result == 0)
            {
                m_isLogin = true;
                Logger.PrintLog(CommonUtils.ConnectStrs("呀呀云登录成功"));
                //YunVaImSDK.instance.RecordSetInfoReq(true);//开启录音的音量大小回调
            }
            else
            {
                Logger.PrintError(CommonUtils.ConnectStrs("呀呀云登录失败，错误消息：", data.msg));
            }
        });*/
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="filePath"></param>
    public static void StartRecord(string fileName)
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*if (!m_isLogin)
        {
            Logger.PrintError("录音错误：呀呀云未登录");
            return;
        }
        if (m_isRecording)
        {
            Logger.PrintError("录音错误：上一段录音未停止");
            return;
        }
        m_isRecording = true;
        Logger.PrintLog("开始录音");
        string dir = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/record/");
        IOUtil.CreateDirectory(dir);
        string amrFileName = CommonUtils.ConnectStrs(dir, fileName, ".amr");
        RecordInfo recordInfo = new RecordInfo();
        recordInfo.fileName = fileName;
        recordInfo.filePath = amrFileName;
        m_recordDict[fileName] = recordInfo;
        CloudVoiceImSDK.instance.RecordStartRequest(amrFileName, 2, fileName);*/
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public static void StopRecord()
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*m_isRecording = false;
        Logger.PrintLog("停止录音");
        CloudVoiceImSDK.instance.RecordStopRequest((data1) =>
        {
            Logger.PrintLog("停止录音返回:" + data1.ext + "_" + data1.strfilepath + "_" + data1.time + "_" + data1.result + "_" + data1.msg);
            string fileName = data1.ext;
            if (!m_recordDict.ContainsKey(fileName))
                return;
            if (data1.result == 0)
            {
                Logger.PrintLog("录音成功");
                m_recordDict[fileName].time = data1.time;
            }
            else
            {
                Logger.PrintLog("录音失败");
                string msg = CommonUtils.ConnectStrs("1|", fileName, "|", data1.msg);
                NoticeManager.Instance.Dispatch(NoticeType.Record_End, msg);
                m_recordDict.Remove(fileName);
            }
        }, (data2) => {
            Logger.PrintLog("上传返回:" + data2.fileid + "_" + data2.fileurl + "_" + data2.result + "_" + data2.msg);
            string fileName = data2.fileid;
            if (!m_recordDict.ContainsKey(fileName))
                return;
            if (data2.result == 0)
            {
                Logger.PrintLog("上传录音成功");
                RecordInfo recordInfo = m_recordDict[fileName];
                string msg = CommonUtils.ConnectStrs("0|", fileName, "|", recordInfo.time.ToString(), "|", recordInfo.filePath, "|", data2.fileurl);
                NoticeManager.Instance.Dispatch(NoticeType.Record_End, msg);
            }
            else
            {
                Logger.PrintLog("上传录音失败");
                string msg = CommonUtils.ConnectStrs("1|", fileName, "|", data2.msg);
                NoticeManager.Instance.Dispatch(NoticeType.Record_End, msg);
                m_recordDict.Remove(fileName);
            }
        });*/
    }

    /// <summary>
    /// 开始播放本地录音
    /// </summary>
    /// <param name="filePath"></param>
    public static void StartPlayRecordByFilePath(string filePath)
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*CloudVoiceImSDK.instance.RecordStartPlayRequest(filePath, "", "", (data) =>
        {
            if (data.result == 0)
                Logger.PrintLog("播放成功");
            else
                Logger.PrintLog("播放失败");
        });*/
    }

    /// <summary>
    /// 开始播放网络录音
    /// </summary>
    /// <param name="url"></param>
    public static void StartPlayRecordByUrl(string url)
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*CloudVoiceImSDK.instance.RecordStartPlayRequest("", url, "", (data) =>
        {
            if (data.result == 0)
                Logger.PrintLog("播放成功");
            else
                Logger.PrintLog("播放失败");
        });*/
    }

    /// <summary>
    /// 停止播放录音
    /// </summary>
    /// <param name="fileName"></param>
    public static void StopPlayRecord()
    {
#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        return;
#endif
        /*Logger.PrintLog("停止播放录音");
        CloudVoiceImSDK.instance.RecordStopPlayRequest();*/
    }
}