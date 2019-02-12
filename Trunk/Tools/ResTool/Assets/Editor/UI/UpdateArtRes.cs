using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CreateUpdateArtWindow : EditorWindow
{
    private static CreateUpdateArtWindow m_target;
    private static string m_resPath;
    private static Dictionary<ResPackage, string[]> m_dictPackIDArray = new Dictionary<ResPackage, string[]>();
    
    /// <summary>资源分包</summary>
    private static ResPackage m_buildResPackage = ResPackage.Base;

    [MenuItem("Tool/UI/更新UI图集切图")]
    public static void ShowUpdateArtWindow()
    {
        m_target = EditorWindow.GetWindow<CreateUpdateArtWindow>(false, "UpdateArtWindow");
        m_target.wantsMouseMove = true;
        DontDestroyOnLoad(m_target);

        m_resPath = Application.dataPath + "/../UI_art/";
        m_dictPackIDArray.Clear();
        
        string projectName = ProjectUtil.GetCurProjectName();
        if (string.IsNullOrEmpty(projectName))
        {
            EditorUtility.DisplayDialog("错误", "项目为空", "确定");
            return;
        }
    }

    public void OnGUI()
    {
        m_buildResPackage = (ResPackage)EditorGUILayout.EnumPopup("资源包：", m_buildResPackage);

        EditorGUILayout.Space();

        if (!m_dictPackIDArray.ContainsKey(m_buildResPackage))
        {
            DirectoryInfo folder = new DirectoryInfo(m_resPath + "UI_png/" + ResPackageUtil.GetPackageName(m_buildResPackage));
            if (folder.Exists)
            {
                var packArray = folder.GetDirectories();
                string[] packIDArray = packArray.Select(i => i.Name).ToArray();
                m_dictPackIDArray.Add(m_buildResPackage, packIDArray);
            }
            else
                m_dictPackIDArray.Add(m_buildResPackage, new string[0]);
        }

        foreach (var pack in m_dictPackIDArray[m_buildResPackage])
        {
            AddExportItem(pack);
        }

        if (m_dictPackIDArray[m_buildResPackage].Length > 1)
        {
            EditorGUILayout.Space();
            AddAllExportItem();
        }
    }

    void AddExportItem(string btnName)
    {
        if (GUILayout.Button(btnName, GUILayout.ExpandWidth(true)))
        {
            //Debug.Log("点击了" + btnName);
            string projectName = ProjectUtil.GetCurProjectName();

            string m_targetRootPath = Application.dataPath + "/Project/" + projectName + "/ui/" + ResPackageUtil.GetPackageName(m_buildResPackage) + "/Import_png/";
            CheckDir(m_targetRootPath);
            var m_targetPath = m_targetRootPath + btnName;
            CheckDir(m_targetPath);
            DirectoryInfo folder = new DirectoryInfo(m_resPath + "UI_png/" + ResPackageUtil.GetPackageName(m_buildResPackage) + "/" + btnName);
            FileInfo[] files = folder.GetFiles("*.png", SearchOption.AllDirectories);
            foreach (FileInfo info in files)
            {
                string[] fullNameArr = info.FullName.Split('\\');
                string packageID = fullNameArr[fullNameArr.Length - 2].Split('_')[0];
                if (packageID == "0")
                {
                    continue;
                }
                string name = info.Name.Trim();
                string[] nameArr = name.Split('@');

                if (nameArr.Length >= 2)
                {
                    File.Copy(info.FullName, m_targetPath + "/" + packageID + "@" + nameArr[0].ToLower() + "@" + nameArr[1], true);
                }
                else
                {
                    File.Copy(info.FullName, m_targetPath + "/" + packageID + "@" + nameArr[0].ToLower(), true);
                }
            }
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("更新UI图集切图", "项目 " + projectName + " 图集" + btnName + "更新成功", "确定");
        }
    }

    void AddAllExportItem()
    {
        if (GUILayout.Button("全部更新", GUILayout.ExpandWidth(true)))
        {
            ImportUpdateArtPng();
        }
    }

    public static void ImportUpdateArtPng()
    {
        string projectName = ProjectUtil.GetCurProjectName();
        if (string.IsNullOrEmpty(projectName))
        {
            EditorUtility.DisplayDialog("错误", "项目为空", "确定");
            return;
        }
        string pngPath = Application.dataPath + "/Project/" + projectName + "/ui/" + ResPackageUtil.GetPackageName(m_buildResPackage) + "/Import_png/";

        CheckDir(pngPath);

        DirectoryInfo folder = new DirectoryInfo(m_resPath + "UI_png/" + ResPackageUtil.GetPackageName(m_buildResPackage));
        FileInfo[] files = folder.GetFiles("*.png", SearchOption.AllDirectories);
        foreach (FileInfo info in files)
        {
            string[] fullNameArr = info.FullName.Split('\\');
            string packageID = fullNameArr[fullNameArr.Length - 2].Split('_')[0];
            if (packageID == "0")
            {
                continue;
            }
            string name = info.Name.Trim();
            string[] nameArr = name.Split('@');

            CheckDir(pngPath + packageID);

            if (nameArr.Length >= 2)
            {
                File.Copy(info.FullName, pngPath + packageID + "/" + packageID + "@" + nameArr[0].ToLower() + "@" + nameArr[1], true);
            }
            else
            {
                File.Copy(info.FullName, pngPath + packageID + "/" + packageID + "@" + nameArr[0].ToLower(), true);
            }
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("更新UI图集切图", "项目 " + projectName + " 全部图集更新成功", "确定");
    }

    static void CheckDir(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

public class UpdateArtRes
{

    private static string m_resPath = Application.dataPath + "/../UI_art/";

    [MenuItem("Tool/UI/Update Frame Png")]
    public static void ImportUpdateFramePng()
    {
        string projectName = ProjectUtil.GetCurProjectName();
        if (string.IsNullOrEmpty(projectName))
        {
            EditorUtility.DisplayDialog("错误", "项目为空", "确定");
            return;
        }
        string pngPath = Application.dataPath + "/Project/" + projectName + "/ui/Import_frame_png/";

        //固定128*128
        DirectoryInfo folder = new DirectoryInfo(m_resPath + "UI_frame");
        FileInfo[] files = folder.GetFiles("*.png", SearchOption.AllDirectories);
        foreach (FileInfo info in files)
        {
            string name = info.Name.Trim();
            string[] nameArr = name.Split('_');
            if (nameArr[0] == "frame")
            {
                File.Copy(info.FullName, pngPath + name, true);
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
}



