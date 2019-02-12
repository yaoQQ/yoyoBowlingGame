using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;

public class OneKeyExpAudio
{


    static string Get_import_resPath()
    {
        return Application.dataPath +"/Project/"+ ProjectUtil.GetCurProjectName() + "/audio/";
    }
   


    static string Get_PC_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/res/audio/";
    }
    static string Get_IOS_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/ios_res/audio/";
    }

    static string Get_Android_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/android_res/audio/";
    }


    [MenuItem("Export/Audio/Exp Audio Res (PC)")]
    public static void ExportAudioRes_PC()
    {
        ExportAudioRes(BuildTarget.StandaloneWindows64, Get_PC_expPath());
    }

    [MenuItem("Export/Audio/Exp Audio Res (IOS)")]
    public static void ExportAudioRes_IOS()
    {
        ExportAudioRes(BuildTarget.iOS, Get_IOS_expPath());
    }
    [MenuItem("Export/Audio/Exp Audio Res (Android)")]
    public static void ExportAudioRes_Android()
    {
        ExportAudioRes(BuildTarget.Android, Get_Android_expPath());
    }


    public static void ExportAudioRes(BuildTarget buildTarget, string expStr)
    {
        DirectoryInfo folder = new DirectoryInfo(Get_import_resPath());
        DirectoryInfo[] packDirArr = folder.GetDirectories();
        for (int i=0;i< packDirArr.Length;i++)
        {
            DirectoryInfo packDir = packDirArr[i];
            if (packDir.Name.Contains(".svn"))
                continue;
            List<AssetBundleBuild> load_abbList = new List<AssetBundleBuild>();
            string packName = packDir.Name;
            //Debug.Log(packName);
            string expPath = expStr + packName + "/";

            List<FileInfo> audioFileList = new List<FileInfo>();
            FileInfo[] filesOgg = packDir.GetFiles("*.ogg", SearchOption.AllDirectories);
            FileInfo[] filesMp3 = packDir.GetFiles("*.mp3", SearchOption.AllDirectories);
            audioFileList.AddRange(filesOgg);
            audioFileList.AddRange(filesMp3);
            for (int j = 0; j < audioFileList.Count; j++)
            {
                FileInfo file = audioFileList[j];
                string relativePath = file.DirectoryName.Replace(packDir.FullName, "").Replace("\\", "/");
                string path = file.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AssetBundleBuild soundABB = new AssetBundleBuild();
                if (relativePath == "")
                    soundABB.assetBundleName = Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                else
                    soundABB.assetBundleName = relativePath.Substring(1) + "/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                soundABB.assetNames = new string[] { path };
                load_abbList.Add(soundABB);
            }
            DirectoryInfo expDirectoryInfo = new DirectoryInfo(Application.dataPath + expPath);
            if(expDirectoryInfo.Exists)
            {
                expDirectoryInfo.Delete(true);
            }
            expDirectoryInfo.Create();
            BuildPipeline.BuildAssetBundles(Application.dataPath + expPath, load_abbList.ToArray(), BuildAssetBundleOptions.None, buildTarget);
            load_abbList.Clear();
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
        EditorUtility.DisplayDialog("导出", "项目 " + ProjectUtil.GetCurProjectName() + " 导出音频资源成功", "确定");
    }
}
