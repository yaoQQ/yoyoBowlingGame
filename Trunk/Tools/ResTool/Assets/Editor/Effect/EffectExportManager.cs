using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EffectExportManager
{
    private static BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

    static string Get_PC_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/res/effect/";
    }
    static string Get_IOS_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/ios_res/effect/";
    }

    static string Get_Android_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/android_res/effect/";
    }


    static string GetPath(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.StandaloneWindows64:
                return Get_PC_expPath();
            case BuildTarget.iOS:
                return Get_IOS_expPath();
            case BuildTarget.Android:
                return Get_Android_expPath();
            default:
                return Get_PC_expPath();

        }
    }

    [MenuItem("Export/Effect/Exp Effect Res (PC)")]
    public static void ExpEffectRes_PC()
    {
        ExpEffectRes(BuildTarget.StandaloneWindows64);
    }
    [MenuItem("Export/Effect/Exp Effect Res (Android)")]
    public static void ExpEffectRes_Android()
    {
        ExpEffectRes(BuildTarget.Android);
    }
    [MenuItem("Export/Effect/Exp Effect Res (IOS)")]
    public static void ExpEffectRes_IOS()
    {
        ExpEffectRes(BuildTarget.iOS);
    }

    static void ExpEffectRes(BuildTarget target)
    {
        string[] packageDirArr = Directory.GetDirectories(Application.dataPath + "/Project/" + ProjectUtil.GetCurProjectName() + "/effect");
        foreach (string packageDir in packageDirArr)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(packageDir);
            string packageName = directoryInfo.Name;

            string[] dirArr = Directory.GetDirectories(packageDir);
            nameList.Clear();
            List<AssetBundleBuild> abbArr = new List<AssetBundleBuild>();
            /*foreach (string dir in dirArr)
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                ExpOnePackRes(target, directory, ref abbArr);
            }*/
            ExpOnePackRes(target, directoryInfo, ref abbArr);

            string expPath = Application.dataPath + GetPath(target) + packageName;
            var expDir = new DirectoryInfo(expPath);
            DeleteDirButSvn(expDir);
            expDir.Create();

            BuildPipeline.BuildAssetBundles(expPath, abbArr.ToArray(), options, target);
        }
        
        EditorUtility.DisplayDialog("导出", "项目 " + ProjectUtil.GetCurProjectName() + " 导出特效资源成功", "确定");
    }

    private static void DeleteDirButSvn(DirectoryInfo dir)
    {
        if (dir.Exists)
        {
            FileInfo[] files = dir.GetFiles();
            for (int i = 0, len = files.Length; i < len; ++i)
                files[i].Delete();
            DirectoryInfo[] dirs = dir.GetDirectories();
            for (int i = 0, len = dirs.Length; i < len; ++i)
            {
                if (dirs[i].Name.Contains(".svn"))
                    continue;
                dirs[i].Delete(true);
            }
        }
    }

    static List<string> nameList = new List<string>();
    static void ExpOnePackRes(BuildTarget target, DirectoryInfo directory, ref List<AssetBundleBuild> abbArr)
    {
        string packageName = directory.Name;

        SetOneTypeRes("anim", new string[] { "anim" }, directory, ref abbArr, packageName);
        SetOneTypeRes("ctrl", new string[] { "controller" }, directory, ref abbArr, packageName);
        SetOneTypeRes("shader", new string[] { "shader" }, directory, ref abbArr, packageName);
        SetOneTypeRes("mat", new string[] { "mat" }, directory, ref abbArr, packageName);
        SetOneTypeRes("mesh", new string[] { "obj", "FBX" }, directory, ref abbArr, packageName);
        SetOneTypeRes("texture", new string[] { "png" }, directory, ref abbArr, packageName);
        SetOneTypeRes("prefab", new string[] { "prefab" }, directory, ref abbArr, packageName);
    }


    static void SetOneTypeRes(string resTypeName, string[] suffixArr, DirectoryInfo directory, ref List<AssetBundleBuild> abbArr, string packageName = null)
    {
        if (Directory.Exists(directory.FullName + "/" + resTypeName))
        {
            string resTypePath = directory.FullName.Replace("\\", "/") + "/" + resTypeName;
            DirectoryInfo dir = new DirectoryInfo(resTypePath);

            List<FileInfo> fileArr = new List<FileInfo>();
            for (int i = 0; i < suffixArr.Length; i++)
            {
                FileInfo[] file_suffix = dir.GetFiles("*." + suffixArr[i], SearchOption.AllDirectories);
                fileArr.AddRange(file_suffix);
            }
            foreach (FileInfo file in fileArr)
            {
                string relativePath = file.DirectoryName.Replace("\\", "/").Replace(resTypePath, "");
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                abb.assetBundleName = resTypeName + relativePath + "/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";

                if (nameList.Contains(abb.assetBundleName))
                {
                    EditorUtility.DisplayDialog("错误", "存在同名资源： " + abb.assetBundleName, "确定");
                    return;
                }
                nameList.Add(abb.assetBundleName);

                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }

        }
    }

    static void DelOldDir(string prefixPath, string dirName)
    {
        string delPath;
        DirectoryInfo delDir;

        delPath = prefixPath + dirName;
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);
    }

}
