using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XLua;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using System.Collections.Generic;

[LuaCallCSharp]
public class AliyunOSSManager : MonoBehaviour
{
    const string accessKeyId = "LTAIECYckwf0WL8D";
    const string accessKeySecret = "6fCujfXhvjn7UNtAB5hmkG6jqrKXd5";
    const string endpoint = "oss-cn-shenzhen.aliyuncs.com";
    private string localSaveRootDir = ""; // 本地存储图片时的根目录

    private static AliyunOSSManager m_instance;

    public static AliyunOSSManager Instance
    {
        get { return m_instance; }
    }


    private OssClient client;
    /// <summary>
    /// 正式服的Bucket
    /// </summary>
    private const string BucktNameOfficial = "yoyo2018-official";
    /// <summary>
    /// 开发服的Bucket
    /// </summary>
    private const string BucketNameDevelop = "yoyo2018";
    private string m_bucketName = "yoyo2018";
    void Awake()
    {
        m_instance = this;
        this.localSaveRootDir = string.Format("{0}/{1}/{2}", Application.persistentDataPath, "aliyunFiles", "Image");
    }

    private KeyboardHandler keyboard;
    void Start()
    {
        this.InitOss();
    }

    private void InitOss()
    {
        if (UtilMethod.isTestServer || UtilMethod.IsSuperVersion())
            this.m_bucketName = BucketNameDevelop;
        else
            this.m_bucketName = BucktNameOfficial;

        client = CreateClient(endpoint, accessKeyId, accessKeySecret);
    }

    public OssClient CreateClient(string endpoint, string accessKeyId, string accessKeySecret)
    {
        return new OssClient(endpoint, accessKeyId, accessKeySecret);
    }

    void Update()
    {
        
    }
    private void ExecuteKeyBorad()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            this.UploadRoot();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            this.DownloadRoot();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.GetObjectResizeTest();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            var testUrl = "http://thirdwx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTJUB8g2YibdsrPgianrQwcSRHOHbv25BzEuiaHhh8gqeNluarcb1sEw8qYPSB9P8SmOjLiao3jWqliaceA/132.jpg";
            var result = this.TryLoadFromHttp(testUrl, null);
            Debug.Log("result = " + result);
        }
    }

    private void UploadRoot()
    {
        Debug.Log("根目录的异步上传测试");
        var photoBytes = File.ReadAllBytes("D:/26/test.png");
        this.UploadImage("test.jpg", photoBytes, "", (state) =>
         {
             if (state)
                 Debug.Log("上传成功");
         });
    }
    private void UploadDir_1()
    {
        Debug.Log("一级目录的异步上传测试");
        var photoBytes = File.ReadAllBytes("D:/26/a/test_a.png");
        this.UploadImage("a/test_a.jpg", photoBytes, "a", (state) =>
         {
             if (state)
                 Debug.Log("上传成功");
         });
    }
    private void UploadDir_2()
    {
        Debug.Log("多级目录的异步上传测试");
        var photoBytes = File.ReadAllBytes("D:/26/a/b/test_a_b.png");
        this.UploadImage("a/b/test_a_b.jpg", photoBytes, "a/b", (state) =>
         {
             if (state)
                 Debug.Log("上传成功");
         });
    }

    private void DownloadRoot()
    {
        Debug.Log("根目录的异步下载测试");
        this.DownloadImage("test.jpg", 889337, (textrue, str) => { });
    }
    private void DownloadDir_1()
    {
        Debug.Log("一级目录的异步下载测试");
        this.DownloadImage("a/test_a.jpg", 889337, (textrue, str) => { });
    }
    private void DownloadDir_2()
    {
        Debug.Log("多级目录的异步下载测试");
        this.DownloadImage("a/b/test_a_b.jpg", 889337, (textrue, str) => { });

    }

    private void GetObjectResizeTest()
    {
        Debug.Log("带目录的异步下载缩略图测试");
        this.DownloadResizeImage("a/b/test_a_b.jpg", "Pad", "", 889337, (a, b) => { });
    }
    private void ShowObjectListTest()
    {
        Debug.Log("显示测试");
    }

    public void UploadImage(string name, byte[] photoBytes, string type, Action<bool> finishCallback)
    {
        string nameDir;
        string typeDir = type;
        if (name.Contains("/"))
        {
            nameDir = name.Substring(0, name.LastIndexOf("/"));
        }
        else
        {
            nameDir = "";
        }
        if (nameDir != typeDir)
            Debug.LogErrorFormat("错误, 名称目录和类型目录不匹配, 直接根据名称目录上传图片. name: {0}", name);

        string remoteKey = name;
        try
        {
            var stream = new MemoryStream(photoBytes);
            var result = "Notice user: AsyncPutObject finish".ToCharArray();
            client.BeginPutObject(m_bucketName, remoteKey, stream, (ar) =>
            {
                try
                {
                    client.EndPutObject(ar);
                    ThreadManager.RunMainThread(() =>
                    {
                        finishCallback(true);
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    ThreadManager.RunMainThread(() =>
                    {
                        finishCallback(false);
                    });
                }

            }, result);
        }
        catch (OssException ex)
        {
            ThreadManager.RunMainThread(() =>
            {
                Debug.LogWarningFormat("PutObject Failed : {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                finishCallback(false);
            });

        }
        catch (Exception ex)
        {
            ThreadManager.RunMainThread(() =>
            {
                Debug.LogWarningFormat("PutObject Failed : {0}", ex.Message);
                finishCallback(false);
            });
        }
    }
    /// <summary>
    /// 下载带目录的图片
    /// </summary>
    /// <param name="name">带目录结构的图片名字</param>
    /// <param name="uid">uid, 用于区分本地目录</param>
    /// <param name="type">类型, 已废弃</param>
    /// <param name="finishCallback">回调</param>
    public void DownloadImage(string name, int uid, Action<Texture2D, string> finishCallback)
    {
        if (this.TryLoadFromHttp(name, finishCallback))
            return;
        if (this.TryLoadFromLocal(name, uid, finishCallback))
            return;
        string result = "Notice user: AsyncGetObject finish";
        var localSavePath = string.Format("{0}/{1}/{2}", localSaveRootDir, uid, name);
        client.BeginGetObject(m_bucketName, name, (ar) =>
        {
            try
            {
                var getObj = client.EndGetObject(ar);
                using (var requestStream = getObj.Content)
                {
                    using (var fs = File.Open(localSavePath, FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        } while (length != 0);
                    }
                }

                ThreadManager.RunMainThread(() =>
                {
                    StartCoroutine(LoadLocal(localSavePath, finishCallback));
                });
            }
            catch (OssException ex)
            {
                ThreadManager.RunMainThread(() =>
                {
                    Debug.LogWarningFormat("GetObject Failed : {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                    if (finishCallback != null)
                    {
                        finishCallback.Invoke(null, null);
                    }
                });

            }
            catch (Exception ex)
            {
                ThreadManager.RunMainThread(() =>
                {
                    Debug.LogWarningFormat("GetObject OtherFailed: {0}", ex.Message);
                    if (finishCallback != null)
                    {
                        finishCallback.Invoke(null, null);
                    }
                });
            }

        }, result.Clone());
    }

    public void DownloadResizeImage(string name, string resizeType, string process, int uid, Action<Texture2D, string> finishCallback)
    {
        var localKey = "";
        if (name.Contains("/"))
        {
            var relativeDir = name.Substring(0, name.LastIndexOf("/"));
            var purelyName = name.Substring(name.LastIndexOf("/") + 1);
            localKey = string.Format("{0}/{1}_{2}", relativeDir, resizeType.ToLower(), purelyName);
        }
        else
        {
            localKey = string.Format("{0}_{1}", resizeType.ToLower(), name);
        }
        if (this.TryLoadFromLocal(localKey, uid, finishCallback))
            return;
        if (string.IsNullOrEmpty(process))
            process = string.Format("image/resize,m_fixed,h_{0},w_{1}", 100, 100);
        string result = "Notice user: AsyncGetResizeObject finish";
        var personalDirectory = string.Format("{0}/{1}", localSaveRootDir, uid);
        var fullFilePath = string.Format("{0}/{1}", personalDirectory, localKey);
        client.BeginGetObject(new GetObjectRequest(m_bucketName, name, process), (ar) =>
        {
            try
            {
                var ossObject = client.EndGetObject(ar);
                using (var requestStream = ossObject.Content)
                {
                    using (var fs = File.Open(fullFilePath, FileMode.OpenOrCreate))
                    {
                        int length = 4 * 1024;
                        var buf = new byte[length];
                        do
                        {
                            length = requestStream.Read(buf, 0, length);
                            fs.Write(buf, 0, length);
                        } while (length != 0);
                    }
                }
                ThreadManager.RunMainThread(() =>
                {
                    StartCoroutine(LoadLocal(fullFilePath, finishCallback));
                });
            }
            catch (OssException ex)
            {
                ThreadManager.RunMainThread(() =>
                {
                    Debug.LogWarningFormat("GetResizeObject Failed : key: {0}: {1}; Error info: {2}. \nRequestID:{3}\tHostID:{4}", name,
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                    if (finishCallback != null)
                    {
                        finishCallback.Invoke(null, null);
                    }
                });
            }
            catch (Exception ex)
            {
                ThreadManager.RunMainThread(() =>
                {
                    Debug.LogWarningFormat("GetResizeObject OtherFailed: {0}", ex.Message);
                    if (finishCallback != null)
                    {
                        finishCallback.Invoke(null, null);
                    }
                });
            }
        }, result.Clone());
    }

    IEnumerator LoadLocal(string url, Action<Texture2D, string> finishCallback)
    {
        var filePath = string.Format("{0}{1}", "file:///", url);
        WWW www = new WWW(@filePath);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            if (finishCallback != null)
            {
                finishCallback.Invoke(www.texture, www.url);
            }
        }
        else
        {
            Debug.LogWarningFormat("Failed with error load: {0}", www.error);
            if (finishCallback != null)
            {
                finishCallback.Invoke(null, null);
            }
        }
    }
    IEnumerator LoadHttp(string url, Action<Texture2D, string> finishCallback)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            if (finishCallback != null)
            {
                finishCallback.Invoke(www.texture, www.url);
            }
        }
        else
        {
            Debug.LogWarningFormat("Failed with error load: {0}", www.error);
            if (finishCallback != null)
            {
                finishCallback.Invoke(null, null);
            }
        }
    }
    private bool TryLoadFromHttp(string url, Action<Texture2D, string> finishCallback)
    {
        if (url.StartsWith("http:", true, null) || url.StartsWith("https:", true, null))
        {
            var noExtensionUrl = url.Substring(0, url.LastIndexOf("."));
            StartCoroutine(LoadHttp(noExtensionUrl, finishCallback));
            return true;
        }
        return false;
    }

    private bool TryLoadFromLocal(string name, int uid, Action<Texture2D, string> finishCallback)
    {
        if (!Directory.Exists(localSaveRootDir))
            Directory.CreateDirectory(localSaveRootDir);
        string personalDirectory = string.Format("{0}/{1}", localSaveRootDir, uid);
        if (!Directory.Exists(personalDirectory))
            Directory.CreateDirectory(personalDirectory);
        if (name.Contains("/"))
        {
            var relativeDir = name.Substring(0, name.LastIndexOf("/"));
            var absoluteDir = string.Format("{0}/{1}", personalDirectory, relativeDir);
            if (!Directory.Exists(absoluteDir))
                Directory.CreateDirectory(absoluteDir);
        }
        string fullPath = string.Format("{0}/{1}", personalDirectory, name);
        if (File.Exists(fullPath))
        {
            StartCoroutine(LoadLocal(fullPath, finishCallback));
            return true;
        }
        else
            return false;
    }

}