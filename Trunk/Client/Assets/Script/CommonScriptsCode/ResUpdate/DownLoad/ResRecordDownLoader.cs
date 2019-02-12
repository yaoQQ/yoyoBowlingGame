using System.IO;
using System.Text;
using UnityEngine;

public class ResRecordDownLoader
{

    public enum LoadState
    {
        free,
        downloading,//下载中
        finish,//下载完成
        fail,
    }



    WWW contant;
    ResVersions.ComparisonRecord comparisonRecord;

    public long TotalLength
    {
        get
        {
            return comparisonRecord.serverRecord.size;
        }
    }

    public int FileLength
    {
        get
        {
            return Mathf.FloorToInt(TotalLength* progress);
        }
    }


    public float progress {
        get { return contant.progress; }
    }

    public LoadState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public ResVersions.ComparisonRecord ComparisonRecord
    {
        get
        {
            return comparisonRecord;
        }
    }

    LoadState currentState = LoadState.downloading;
    string resPath;

    public void StartDown(ResVersions.ComparisonRecord p_comparisonRecord)
    {
        /*currentState = LoadState.downloading;
        comparisonRecord = p_comparisonRecord;
        StringBuilder sb = new StringBuilder();
        sb.Append("http://");
        sb.Append(ResUpdateManager.GetInstance().resIP);
        sb.Append("/yoyo_pack_res/");
        sb.Append(ResUpdateManager.GetInstance().VersionCode);
        sb.Append("/res/");
        sb.Append(comparisonRecord.resPath);
        resPath = sb.ToString();
        //Debug.LogError("resPath  "+ resPath);
        contant = new WWW(resPath);*/
    }

    public void RunDown()
    {
        if (currentState != LoadState.downloading) return;
        if (contant.error != null && contant.error.Length > 0)
        {
            Loger.PrintError("Error:", contant.error, "(", resPath, ")");
            currentState = LoadState.fail;

            //是否重新加载

        }
        else
        {
            if (contant.isDone)
            {
                //Debug.Log("文件创建成功   " + comparisonRecord.resPath);
                bool sign= CreatFile(contant.bytes);
                if(sign)
                {
                    currentState = LoadState.finish;
                    
                }
                else
                {

                    currentState = LoadState.finish;
                }
            }
        }
    }
     bool CreatFile(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Application.persistentDataPath);
        sb.Append("/res/");
        sb.Append(comparisonRecord.resPath);
        string localPath = sb.ToString();
        FileInfo resFileInfo = new FileInfo(localPath);
        Stream stream;
        stream = resFileInfo.Create();
        stream.Write(bytes, 0, bytes.Length);

        string md5 = MD5Util.GetMD5HashFromFile(stream);
        stream.Close();
        stream.Dispose();

        if (md5== comparisonRecord.serverRecord.md5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
