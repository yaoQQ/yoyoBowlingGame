using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ApkUpdate : MonoBehaviour
{
    private static ApkUpdate m_instance;
    public static ApkUpdate Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("ApkUpdate").AddComponent<ApkUpdate>();
            }
            return m_instance;
        }
    }

    public void StartUpdate()
    {
        TimeSpan span = (TimeSpan) (DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
        string str = Convert.ToInt64(span.TotalSeconds).ToString();
        string url = CommonPathUtils.SERVER_ROOT_PATH + "/" + CommonPathUtils.APK_UPDATE_FILE_NAME + "?" + str;
        WWWManager.Instance.load(url,
            (www) =>
            {
                char[] separator = new char[] { '.' };
                string[] strArray = www.text.Split(separator);
                char[] chArray2 = new char[] { '.' };
                string[] strArray2 = GameVersionManager.serverVersion.version.Split(chArray2);
                if (((strArray.Length != 4) || (strArray2.Length != 4)) || (((int.Parse(strArray[0]) != int.Parse(strArray2[0])) || (int.Parse(strArray[1]) != int.Parse(strArray2[1]))) || (int.Parse(strArray[2]) != int.Parse(strArray2[2]))))
                {
                    LoadingBarController.ShowNotice("新版本还未上架，请稍后再试", "退出", () => Application.Quit());
                    return;
                }
                
                string downloadPath = CommonPathUtils.SERVER_ROOT_PATH + "/apk/";
                string saveDir = CommonPathUtils.PERSISTENT_DATA_ROOT_PATH + "/apk/";
                IOUtil.CreateDirectory(saveDir);
                string saveFileName = "yoyo";
                switch (UtilMethod.channel)
                {
                    case 1:
                        name += "_zhongju";
                        break;
                    case 2:
                        name += "_huawei";
                        break;
                    case 3:
                        name += "_oppo";
                        break;
                    case 4:
                        name += "_vivo";
                        break;
                    case 5:
                        name += "_xiaomi";
                        break;
                    case 6:
                        name += "_cctv";
                        break;
                }
                if (UtilMethod.isBetaCDN)
                    saveFileName += "_beta";
                else
                    saveFileName += "_stable";
                saveFileName += "_" + www.text + ".apk";
                string downloadUrl = downloadPath + saveFileName;
                string saveFullName = saveDir + saveFileName;
                LoadingBarController.isAutoClose = false;
                StartCoroutine(DownloadContinue(downloadUrl, saveFullName));
            }, null, null, null, true, 0);
    }

    private void showErrorTips(string downloadUrl, string saveFullName)
    {
        LoadingBarController.ShowNotice2("网络连接失败，请检查网络后重新连接", "重新连接",
            () =>
            {
                StartCoroutine(DownloadContinue(downloadUrl, saveFullName));
            }, "退出", () => Application.Quit());
    }

    private void showInstallTips(string saveFullName)
    {
        LoadingBarController.ShowNotice("请安装最新版本", "开始安装",
            () =>
            {
                Loger.PrintLog("安装Apk:" + saveFullName);
                AndroidSDK.Instance.InstallApk(saveFullName);
            });
    }

    
    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {
        return true; //总是接受     
    }

    private IEnumerator DownloadContinue(string downloadUrl, string saveFullName)
    {
        Loger.PrintLog("开始下载Apk:" + downloadUrl);
        //先获取Apk的size
        HttpWebRequest httpWebRequest = null;
        long countLength = 0L;
        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            httpWebRequest = WebRequest.Create(downloadUrl) as HttpWebRequest;
            countLength = (httpWebRequest.GetResponse() as HttpWebResponse).ContentLength;
        }
        catch (Exception e)
        {
            Loger.PrintError("Get Apk Size Error:" + e.Message);
            if (httpWebRequest != null)
                httpWebRequest.Abort();
            showErrorTips(downloadUrl, saveFullName);
            yield break;
        }
        if (httpWebRequest != null)
            httpWebRequest.Abort();

        long lStartPos = 0L;
        FileStream fs;
        //如果文件存在，则上次已下载了一部分
        if (File.Exists(saveFullName))
        {
            Loger.PrintLog("读取已下载的Apk");
            fs = File.OpenWrite(saveFullName);
            lStartPos = fs.Length;
            if (countLength - lStartPos <= 0L)
            {
                fs.Close();
                Loger.PrintLog("下载完成");
                showInstallTips(saveFullName);
                yield break;
            }
            Loger.PrintLog("继续下载:" + lStartPos);
            fs.Seek(lStartPos, SeekOrigin.Current);
        }
        else
            fs = new FileStream(saveFullName, FileMode.Create);

        //下载
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        Stream ns = null;
        try
        {
            request = WebRequest.Create(downloadUrl) as HttpWebRequest;
            if (lStartPos > 0L)
                request.AddRange((int)lStartPos);
            Loger.PrintLog("获取服务器回应");
            response = request.GetResponse() as HttpWebResponse;
            Loger.PrintLog("获取数据流");
            ns = response.GetResponseStream();
        }
        catch (Exception e)
        {
            Loger.PrintError("Request Apk Error:" + e.Message);
            if (ns != null)
                ns.Close();
            fs.Close();
            if (request != null)
                request.Abort();
            showErrorTips(downloadUrl, saveFullName);
            yield break;
        }

        //暂时改为是否WIFI环境下都提示下载
        LoadingBarController.ShowNotice2("需要下载最新版本(" + ((countLength - lStartPos) / 1024f / 1024f).ToString("0.00") + "M)", "开始下载",
            () =>
            {
                StartCoroutine(Download(downloadUrl, saveFullName, countLength, ns, fs, request));
            }, "退出", () => Application.Quit());

        //非WIFI环境下提示下载
        /*if (Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            LoadingBarController.ShowNotice2("需要下载最新版本(" + ((countLength - lStartPos) / 1024f / 1024f).ToString("0.00") + "M)", "开始下载",
                () =>
                {
                    StartCoroutine(Download(downloadUrl, saveFullName, countLength, ns, fs, request));
                }, "退出", () => Application.Quit());
        }
        else
            StartCoroutine(Download(downloadUrl, saveFullName, countLength, ns, fs, request));*/
    }

    private IEnumerator Download(string downloadUrl, string saveFullName, long countLength, Stream ns, FileStream fs, HttpWebRequest request)
    {
        LoadingBarController.ShowProgressWindow();
        float totalLength = countLength / 1024f / 1024f;
        float startTime = Time.realtimeSinceStartup;
        int len = 0x2000;
        byte[] nbytes = new byte[len];
        int nReadSize = ns.Read(nbytes, 0, len);
        while (nReadSize > 0)
        {
            try
            {
                fs.Write(nbytes, 0, nReadSize);
                float curLength = (float)fs.Length / 1024f / 1024f;
                float progress = curLength / totalLength;
                LoadingBarController.SetProgress(progress, 0);
                LoadingBarController.SetLoadContent("正在下载新版本：" + curLength.ToString("0.00") + "M / " + totalLength.ToString("0.00") + "M");
                nReadSize = ns.Read(nbytes, 0, len);
            }
            catch (Exception e)
            {
                Loger.PrintError("Download Apk Error:" + e.Message);
                ns.Close();
                fs.Close();
                request.Abort();
                showErrorTips(downloadUrl, saveFullName);
                yield break;
            }
            float curTime = Time.realtimeSinceStartup;
            if (curTime > startTime + 0.033f)
            {
                startTime = curTime;
                yield return 0;
            }
        }
        LoadingBarController.HideProgressWindow();

        //下载完成
        long length = fs.Length;
        Loger.PrintLog("fs.Length:" + length);
        if (length < countLength)
        {
            ns.Close();
            fs.Close();
            request.Abort();
            showErrorTips(downloadUrl, saveFullName);
        }
        else
        {
            ns.Close();
            fs.Close();
            request.Abort();
            showInstallTips(saveFullName);
        }
    }
}

