using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public static class ResListEditor
{
    /// <summary>是否是打更新包</summary>
    private static bool m_isUpdatePack = false;

    /// <summary>配置表数据对象</summary>
    private static SortedDictionary<string, ResItem> m_resDict = new SortedDictionary<string, ResItem>();
    
    /// <summary>加载资源清单</summary>
    public static void LoadResList(string packageName, bool isUpdatePack)
    {
        m_isUpdatePack = isUpdatePack;
        m_resDict.Clear();
        if (!m_isUpdatePack)
            return;

        string filepath = PathEditor.GetPackagePathEditor(packageName) + "/" + PathUtil.RES_LIST_FILE_NAME;
        if (AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY)
            filepath = PathEditor.RES_INCLUDE_GAME_ROOT_PATH_EDITOR + "/" + PathUtil.RES_LIST_FILE_NAME;
        if (File.Exists(filepath))
        {
            StreamReader sr = new StreamReader(File.OpenRead(filepath));
            sr.ReadLine();
            string line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                string[] strs = line.Split('\t');
                ResItem resItem = new ResItem();
                resItem.packageName = packageName;
                resItem.relativePath = strs[0];
                resItem.md5 = strs[1];
                resItem.size = long.Parse(strs[2]);
                resItem.isUpdate = strs[3] == "1";
                resItem.versionCode = int.Parse(strs[4]);
                resItem.isEx = strs[5] == "1";
                m_resDict[resItem.relativePath] = resItem;
                line = sr.ReadLine();
            }
            sr.Close();
        }
    }

    /// <summary>添加资源</summary>
    public static void AddRes(FileInfo fileInfo, string packageName, int versionCode)
    {
        IOUtil.WriteLog(fileInfo.FullName);
        string relativePath = fileInfo.FullName.Replace("\\", "/").Replace(PathEditor.GetResPathEditor(packageName) + "/", "");
        if (relativePath.StartsWith("res_login"))
            return;
        ResItem resItem = new ResItem();
        resItem.packageName = packageName;
        resItem.relativePath = relativePath;
        resItem.md5 = MD5Util.GetMD5HashFromFile(fileInfo.FullName);
        resItem.size = fileInfo.Length;
        if (m_resDict.ContainsKey(resItem.relativePath) && m_resDict[resItem.relativePath].md5.Equals(resItem.md5))
            resItem.versionCode = m_resDict[resItem.relativePath].versionCode;
        else
        {
            resItem.versionCode = versionCode;
            //如果是打更新包，则把需要更新的资源复制出来
            if (m_isUpdatePack)
            {
                string resPath = PathEditor.GetResPathEditor(packageName) + "/" + resItem.relativePath;
                string resDir = IOUtil.GetFileDir(resPath);
                string packPath = resPath.Replace("/" + packageName + "/res/", "/" + packageName + "/" + versionCode + "/res/");
                string packDir = IOUtil.GetFileDir(packPath);
                // 创建目的文件夹
                IOUtil.CreateDirectory(packDir);
                // 拷贝文件
                File.Copy(resPath, packPath, true);
            }
        }
        m_resDict[resItem.relativePath] = resItem;
    }

    /// <summary>添加场景资源</summary>
    /*public static void AddSceneRes(string fullName, int versionCode)
    {
        IOUtil.WriteLog(fullName);
        int lastIndex = fullName.LastIndexOf('/');
        string fileName = fullName.Substring(lastIndex + 1);
        ResItem resItem = new ResItem();
        resItem.fileName = fileName;
        resItem.relativePath = fullName.Substring(0, lastIndex + 1).Replace(PathEditor.RES_PATH_EDITOR.ToLower() + "/", "");
        resItem.md5 = AssetNodeUtility.md5file(fullName);
        FileInfo fileInfo = new FileInfo(fullName);
        resItem.size = fileInfo.Length;
        if (m_resDict.ContainsKey(resItem.fileName) && m_resDict[resItem.fileName].md5.Equals(resItem.md5))
            resItem.versionCode = m_resDict[resItem.fileName].versionCode;
        else
        {
            resItem.versionCode = versionCode;
            //如果是打更新包，则把需要更新的资源复制出来
            if (m_isUpdatePack)
            {
                string resDir = PathEditor.RES_PATH_EDITOR + "/" + resItem.relativePath;
                string packDir = resDir.Replace("/" + PathUtil.platformStr + "/", "/" + PathUtil.platformStr + "/" + versionCode + "/");
                // 创建目的文件夹
                IOUtil.CreateDirectory(packDir);
                // 拷贝文件
                File.Copy(resDir + resItem.fileName, packDir + resItem.fileName, true);
            }
        }
        m_resDict[resItem.fileName] = resItem;
    }

    /// <summary>添加Manifest资源</summary>
    public static void AddManifest(int versionCode)
    {
        string fileName = PathUtil.BUNDLE_MANIFEST_FILE_NAME;
        ResItem resItem = new ResItem();
        resItem.fileName = fileName;
        resItem.relativePath = "../" + PathUtil.RES_DIR_NAME + "/";
        string fullName = PathEditor.RES_PATH_EDITOR + "/" + fileName;
        resItem.md5 = AssetNodeUtility.md5file(fullName);
        FileInfo fileInfo = new FileInfo(fullName);
        resItem.size = fileInfo.Length;
        if (m_resDict.ContainsKey(resItem.fileName) && m_resDict[resItem.fileName].md5.Equals(resItem.md5))
            resItem.versionCode = m_resDict[resItem.fileName].versionCode;
        else
        {
            resItem.versionCode = versionCode;
            //如果是打更新包，则把需要更新的资源复制出来
            if (m_isUpdatePack)
            {
                string resDir = PathEditor.RES_PATH_EDITOR + "/" + resItem.relativePath;
                string packDir = resDir.Replace("/" + PathUtil.platformStr + "/", "/" + PathUtil.platformStr + "/" + versionCode + "/");
                // 创建目的文件夹
                IOUtil.CreateDirectory(packDir);
                // 拷贝文件
                File.Copy(resDir + resItem.fileName, packDir + resItem.fileName, true);
            }
        }
        m_resDict[resItem.fileName] = resItem;
    }*/

    /// <summary>保存资源清单</summary>
    public static void SaveResList(string packageName, int versionCode)
    {
        string dir = PathEditor.GetPackagePathEditor(packageName) + "/";
        string filepath = dir + PathUtil.RES_LIST_FILE_NAME;
        IOUtil.CreateDirectory(dir);
        IOUtil.DeleteFile(filepath);
        //BuildAssetBundles.WriteLog("保存VerFile path:" + filepath);
        using (StreamWriter writer = File.CreateText(filepath))
        {
            string title = "relativePath\tmd5\tsize\tisUpdate\tversionCode\tisEx";
            writer.WriteLine(title);
            foreach (ResItem item in m_resDict.Values)
            {
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
        //把资源清单和版本号文件复制出来
        string packDir = dir + versionCode + "/";
        // 创建目的文件夹
        IOUtil.CreateDirectory(packDir);
        // 拷贝文件
        File.Copy(filepath, packDir + PathUtil.RES_LIST_FILE_NAME, true);
        filepath = dir + PathUtil.VERSION_FILE_NAME;
        File.Copy(filepath, packDir + PathUtil.VERSION_FILE_NAME, true);
    }
}