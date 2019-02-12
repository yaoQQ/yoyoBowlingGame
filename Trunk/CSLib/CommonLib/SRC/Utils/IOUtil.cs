using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IOUtil
{
    /// <summary>
    /// 创建目录
    /// </summary>
    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    /// <summary>
    /// 删除目录
    /// </summary>
    public static void DeleteDirectory(string path)
    {
        if (!Directory.Exists(path))
            return;
        Directory.Delete(path, true);
    }

    /// <summary>
    /// 清空目录
    /// </summary>
    public static void ClearDirectory(string path)
    {
        if (!Directory.Exists(path))
            return;
        try
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
                Directory.Delete(directoryInfo.FullName, true);
            foreach (FileInfo fileInfo in dir.GetFiles())
                File.Delete(fileInfo.FullName);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 复制目录
    /// </summary>
    public static void CopyDirectory(string sPath, string dPath, params string[] excludeFileType)
    {
        HashSet<string> hashSet = new HashSet<string>(excludeFileType ?? new string[0]);
        try
        {
            // 创建目的文件夹
            IOUtil.CreateDirectory(dPath);

            // 拷贝文件
            DirectoryInfo sDir = new DirectoryInfo(sPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                if (hashSet.Contains(file.Extension))
                    continue;
                file.CopyTo(dPath + "/" + file.Name, true);
            }

            // 循环子文件夹
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyDirectory(subDir.FullName, dPath + "/" + subDir.Name, excludeFileType);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static string GetFileDir(string path)
    {
        return path.Substring(0, path.LastIndexOf('/') + 1);
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void WriteLog(string text)
    {
        Debug.Log(text);
        using (StreamWriter writer = File.AppendText(Application.dataPath + "/log/editorLog.txt"))
        {
            writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "[Info]" + text);
            writer.Flush();
            writer.Close();
        }
    }

    public static void WriteLogError(string text)
    {
        Debug.LogError(text);
        using (StreamWriter writer = File.AppendText(Application.dataPath + "/log/editorLog.txt"))
        {
            writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "[Error]" + text);
            writer.Flush();
            writer.Close();
        }
        using (StreamWriter writer2 = File.AppendText(Application.dataPath + "/log/editorLogError.txt"))
        {
            writer2.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "[Error]" + text);
            writer2.Flush();
            writer2.Close();
        }
    }

    public static void WriteLogWarning(string text)
    {
        Debug.LogWarning(text);
        using (StreamWriter writer = File.AppendText(Application.dataPath + "/log/editorLog.txt"))
        {
            writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "[Warning]" + text);
            writer.Flush();
            writer.Close();
        }
    }
}
