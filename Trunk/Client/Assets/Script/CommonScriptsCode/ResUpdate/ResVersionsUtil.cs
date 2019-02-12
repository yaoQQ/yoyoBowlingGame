using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

public static  class ResVersionsUtil  {

    static string localPath = Application.persistentDataPath + "/resVersions.json";
    static FileInfo resVersionsFileInfo;



    /*public static IEnumerator StartDownloadResVersions(int versionCode)
    {
        if (!CheckLocalExist())
        {
            string path = "http://"+ ResUpdateManager.GetInstance().resIP + "/pack_res/" + versionCode + "/resVersions.json";
            WWW www = new WWW(path);
            yield return www;
            if (www.error != null)
            {
                Debug.LogError("下载本地Versions出错");
                NoticeManager.Instance.Dispatch(NoticeType.Update_Res_Error);
            }
            else
            {
                byte[] bytes = www.bytes;
                if(bytes.Length>0)
                {
                    CreatResVersionsFile(bytes);
                    resVersionsFileInfo = new FileInfo(localPath);
                }
                else
                {
                    Loger.PrintError("本地Versions长度出错");
                    NoticeManager.Instance.Dispatch(NoticeType.Update_Res_Error);
                }
               
            }
           
        }
    }*/

    public static ResVersions serverVersions=null;
    /*public static IEnumerator StartDownloadSeverVersions(int versionCode)
    {
        serverVersions = null;
        string path = "http://"+ ResUpdateManager.GetInstance().resIP +"/pack_res/" + versionCode + "/resVersions.json";
        WWW www = new WWW(path);
       
        yield return www;
        if(www.error != null)
        {
            
            Debug.LogError("StartDownloadSeverVersions资源服连不上 ： " + www.error);
            //资源服连不上
            NoticeManager.Instance.Dispatch(NoticeType.Update_Res_Error);
        }
        else
        {
            string jsonStr = Encoding.ASCII.GetString(www.bytes);
            serverVersions = JsonConvert.DeserializeObject<ResVersions>(jsonStr);
        }
       
    }*/

    public static void DelResVersionsFile()
    {
        resVersionsFileInfo = new FileInfo(localPath);
        if(resVersionsFileInfo.Exists)
        {
            resVersionsFileInfo.Delete();
        }
    }

    public static void CreatResVersionsFile(byte[] bytes)
    {

        Stream stream;
        resVersionsFileInfo = new FileInfo(localPath);
        stream = resVersionsFileInfo.Create();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        stream.Dispose();
    }


    public static bool CheckLocalExist()
    {
        if(resVersionsFileInfo==null)
        {
            resVersionsFileInfo = new FileInfo(localPath);

            if(resVersionsFileInfo.Exists && resVersionsFileInfo.Length==0)
            {
                Loger.PrintError("文件大小出错有问题");
                NoticeManager.Instance.Dispatch(NoticeType.Update_Res_Error);
            }
        }
        
        return resVersionsFileInfo.Exists;
    }



    public static ResVersions GetLocalResVersions()
    {
        string allText = System.IO.File.ReadAllText(localPath);
        return JsonConvert.DeserializeObject<ResVersions>(allText);
    }


    public static void StartDownloadBasePack(ResVersions resVersions)
    {
        List<ResVersions.ResPack> resPackList;
        resVersions.packageRecords.TryGetValue("base", out resPackList);
        for (int i = 0; i < resPackList.Count; i++)
        {
            ResVersions.ResPack resPack = resPackList[i];
            if (resPack.loadedSign == false)
            {
                ResDownLoadContoller.Instance.DownLoadResPack(resPack);
            }
        }
    }

    public static void StartDownloadResPack(ResVersions resVersions,string packName)
    {

        List<ResVersions.ResPack> resPackList;
        resVersions.packageRecords.TryGetValue(packName, out resPackList);
        for (int i = 0; i < resPackList.Count; i++)
        {
            ResVersions.ResPack resPack = resPackList[i];
            if (resPack.loadedSign == false)
            {

                ResDownLoadContoller.Instance.DownLoadResPack(resPack);
            }
        }

    }
    public static long GetPackSize(ResVersions resVersions, string packName)
    {
        long size=0;
        List<ResVersions.ResPack> resPackList;
        resVersions.packageRecords.TryGetValue(packName, out resPackList);
        if(resPackList==null)
        {
            return 0;
        }
        for (int i = 0; i < resPackList.Count; i++)
        {
            ResVersions.ResPack resPack = resPackList[i];
            size += resPack.size;
        }
        //Debug.LogError("size================> "+ size);
        return size;
    }

    public static bool CheckPackExist(ResVersions resVersions,string packName)
    {
        
        List<ResVersions.ResPack> resPackList;
        resVersions.packageRecords.TryGetValue(packName, out resPackList);
        bool packLoadEnd = true;
        for(int i=0;i< resPackList.Count;i++)
        {
            ResVersions.ResPack resPack = resPackList[i];
            if(resPack.loadedSign==false)
            {
                packLoadEnd = false;
                break;
            }
        }
        return packLoadEnd;
    }
   
}
