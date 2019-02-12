using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class SaveResListConfig
{
    public string filePath = string.Empty;
    public List<ResItem> resList;
}

public class ResListManager
{
    private static bool m_isSaving = false;
    private static Dictionary<string, ResItem> m_tempDict = new Dictionary<string, ResItem>();

    public static void FixResList()
    {
        if (PlayerPrefs.GetInt("WRITE_RES_LIST_FINISH", 1) != 1)
        {
            Logger.PrintLog("FixResList开始");
            string dir = CommonPathUtils.PERSISTENT_DATA_ROOT_PATH;
            string tempFullName = dir + "/TempResList.txt";
            string fullName = dir + "/" + CommonPathUtils.RES_LIST_FILE_NAME;
            File.Copy(tempFullName, fullName, true);
            PlayerPrefs.SetInt("WRITE_RES_LIST_FINISH", 1);
            PlayerPrefs.Save();
            IOUtil.DeleteFile(tempFullName);
            Logger.PrintLog("FixResList完成：" + fullName);
        }
    }

    public static void SaveResList(string packageName, Dictionary<string, ResItem> resDict, bool isNewThread = false)
    {
        Logger.PrintLog("SaveResList");
        if (m_isSaving)
        {
            Logger.PrintLog("SaveResList排队");
            m_tempDict.Clear();
            foreach (KeyValuePair<string, ResItem> pair in resDict)
            {
                m_tempDict.Add(pair.Key, pair.Value.Clone());
            }
        }
        else
        {
            m_isSaving = true;
            SaveResListData(packageName, resDict, isNewThread);
        }
    }

    private static void SaveResListData(string packageName, Dictionary<string, ResItem> resDict, bool isNewThread = false)
    {
        SaveResListConfig parameter = new SaveResListConfig {
            filePath = CommonUtils.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/", packageName),
            resList = new List<ResItem>()
        };
        foreach (KeyValuePair<string, ResItem> pair in resDict)
            parameter.resList.Add(pair.Value.Clone());
        if (isNewThread)
        {
            new Thread(new ParameterizedThreadStart(ResListManager.WriteResListThread)).Start(parameter);
        }
        else
        {
            WriteResList(parameter.filePath, parameter.resList);
            WriteResListEnd(packageName);
        }
    }

    /// <summary>保存资源清单</summary>
    public static void WriteResList(string fileDir, List<ResItem> resList)
    {
        Logger.PrintLog("WriteResList开始");
        string tempFullName = fileDir + "/TempResList.txt";
        IOUtil.CreateDirectory(fileDir);
        IOUtil.DeleteFile(tempFullName);
        Logger.PrintLog("保存临时资源清单：" + tempFullName);
        using (StreamWriter writer = File.CreateText(tempFullName))
        {
            string title = "relativePath\tmd5\tsize\tisUpdate\tversionCode\tisEx";
            writer.WriteLine(title);
            int count = resList.Count;
            for (int i = 0; i < count; ++i)
            {
                ResItem item = resList[i];
                if (item != null)
                {
                    string str = item.relativePath + "\t" + item.md5 + "\t" + item.size.ToString() + "\t" + (item.isUpdate ? "1" : "0") +
                        "\t" + item.versionCode.ToString() + "\t" + (item.isEx ? "1" : "0");
                    writer.WriteLine(str);
                }
            }
            writer.Flush();
            writer.Close();
        }

        Logger.PrintLog("WriteResList设置标记");
        PlayerPrefs.SetInt("WRITE_RES_LIST_FINISH", 0);
        PlayerPrefs.Save();
        string fullName = fileDir + "/" + CommonPathUtils.RES_LIST_FILE_NAME;
        Logger.PrintLog("保存资源清单：" + fullName);
        File.Copy(tempFullName, fullName, true);
        Logger.PrintLog("WriteResList清除标记");
        PlayerPrefs.SetInt("WRITE_RES_LIST_FINISH", 1);
        PlayerPrefs.Save();
        Logger.PrintLog("WriteResList删除临时资源清单");
        IOUtil.DeleteFile(tempFullName);
        Logger.PrintLog("WriteResList完成：" + fullName);
    }

    private static void WriteResListEnd(string packageName)
    {
        if (m_tempDict.Count == 0)
        {
            m_isSaving = false;
        }
        else
        {
            SaveResListData(packageName, m_tempDict, true);
            m_tempDict.Clear();
        }
    }

    private static void WriteResListThread(object saveObject)
    {
        SaveResListConfig config = (SaveResListConfig) saveObject;
        WriteResList(config.filePath, config.resList);
        //TODO 调用主线程WriteResListEnd()
    }
}

