
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class UpdateSoundRes : AssetPostprocessor
{
    void OnPreprocessAudio()
    {
        AudioImporter importer = (AudioImporter)assetImporter;
    }

    private static string resPath = Application.dataPath + "/../Sound_Resource_File/";


    private static string updatePath = Application.dataPath+"/Project/"+ProjectUtil.GetCurProjectName() + "/";

    [MenuItem("Tool/Audio/Update Sound Res")]

    public static void ImportUpdateSound()
    {
        DelDirectory(updatePath);
        CheckDir(updatePath);
        DirectoryInfo folder = new DirectoryInfo(resPath);
        if (folder == null)
        {
            Debug.LogError("导入音频原文件路径为空：" + resPath);
        }
        else
        {
            DirectoryCopy(resPath, updatePath);
        }

        AssetDatabase.Refresh();

    }

    static void DirectoryCopy(string sourceDirectory, string targetDirectory)
    {
       
        if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory))
        {
            return;
        }
        DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);


        FileInfo[] fileInfo = sourceInfo.GetFiles();
        foreach (FileInfo fiTemp in fileInfo)
        {
            File.Copy(fiTemp.FullName, targetDirectory + "\\" + fiTemp.Name, true);
        }

        DirectoryInfo[] diInfo = sourceInfo.GetDirectories();
        foreach (DirectoryInfo diTemp in diInfo)
        {
            string sourcePath = diTemp.FullName;

            string targetPath = sourcePath.Replace("Sound_Resource_File", "Assets/Project/"+ProjectUtil.GetCurProjectName() );
            

            if (diTemp.Name != ".svn")
            {
                //Debug.Log(targetPath);
                FileInfo[] soundArr = diTemp.GetFiles("*", SearchOption.AllDirectories);
                CheckDir(targetPath);
                //DirectoryCopy(sourcePath, targetPath);
                for(int i=0;i< soundArr.Length;i++)
                {
                    FileInfo soundFile = soundArr[i];
                    soundFile.CopyTo(targetPath+"/"+ soundFile.Name);
                }
                
            }

        }
    }

    static void CheckDir(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    static void DelDirectory(string pahtStr)
    {
        FileInfo fi = new FileInfo(pahtStr);
        var di = fi.Directory;
        if (di.Exists)
        {
            Directory.Delete(pahtStr, true);
        }

    }
}
