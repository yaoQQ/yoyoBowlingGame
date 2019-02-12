using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EffectLayerTool
{
    static string Get_PC_expPath()
    {
        return "/../" + ProjectUtil.GetCurProjectName() + "_pc_res/";
    }
    static string Get_IOS_expPath()
    {
        return "/../" + ProjectUtil.GetCurProjectName() + "_ios_res/";
    }

    static string Get_Android_expPath()
    {
        return "/../" + ProjectUtil.GetCurProjectName() + "_android_res/";
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

    //[MenuItem("Tool/Effect/EffectLayerTool/SetEffectLayer")]
    public static void ExpEffectRes_PC()
    {
        ExpEffectRes();
    }

    static void ExpEffectRes()
    {
        string[] dirArr = Directory.GetDirectories(Application.dataPath + "/Project/" + ProjectUtil.GetCurProjectName());
        nameList.Clear();
        foreach (string dir in dirArr)
        {
            if (dir.Contains(".svn"))
                continue;
            DirectoryInfo directory = new DirectoryInfo(dir);
            //ExpOnePackRes("prefab", new string[] { "prefab" }, directory);
        }

    }

    static List<string> nameList = new List<string>();
    static void ExpOnePackRes(string resTypeName, string[] suffixArr, DirectoryInfo directory)
    {

        if (Directory.Exists(directory.FullName + "/" + resTypeName))
        {
            DirectoryInfo animDir = new DirectoryInfo(directory.FullName + "/" + resTypeName);

            List<FileInfo> fileArr = new List<FileInfo>();
            for (int i = 0; i < suffixArr.Length; i++)
            {
                FileInfo[] file_suffix = animDir.GetFiles("*." + suffixArr[i]);
                fileArr.AddRange(file_suffix);
            }
            foreach (FileInfo file in fileArr)
            {
                //if (file.Name != "fx_1002_01.prefab")
                //    continue;
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                var res = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var allRender = res.GetComponentsInChildren<Renderer>();
                if (allRender == null)
                    continue;
                foreach (var r in allRender)
                {
                    r.sortingOrder = 100;
                }
                AssetDatabase.SaveAssets();

            }
            EditorUtility.DisplayDialog("设置", "设置项目 " + ProjectUtil.GetCurProjectName() + " 特效层级成功", "确定");

        }

    }

}
public class SetEffectLayerWindow : EditorWindow
{
    private static SetEffectLayerWindow m_target;


    [MenuItem("Tool/Effect/批量设置特效层级")]
    public static void ShowUpdateArtWindow()
    {
        m_target = EditorWindow.GetWindow<SetEffectLayerWindow>(false, "SetEffectLayerWindow");
        m_target.wantsMouseMove = true;
        DontDestroyOnLoad(m_target);

        string projectName = ProjectUtil.GetCurProjectName();
        if (string.IsNullOrEmpty(projectName))
        {
            EditorUtility.DisplayDialog("错误", "项目为空", "确定");
            return;
        }
    }

    void OnGUI()
    {
        ShowPackageList();
    }

    private void ShowPackageList()
    {
        var projectName = ProjectUtil.GetCurProjectName();
        if (string.IsNullOrEmpty(projectName))
        {
            return;
        }
        string[] packList = { };
        DirectoryInfo folder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/effect");
        if (folder.Exists)
        {
            packList = folder.GetDirectories().Select(i => i.Name).ToArray();
        }
        foreach (var pack in packList)
        {
            AddExportItem(pack);
        }
    }

    private const int LayerSort = 1000;
    void AddExportItem(string btnName)
    {
        if (GUILayout.Button(btnName, GUILayout.ExpandWidth(true)))
        {
            Debug.Log("点击了" + btnName);
            var projectName = ProjectUtil.GetCurProjectName();
            DirectoryInfo prefabDir = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/effect/" + btnName + "/prefab");
            var files = prefabDir.GetFiles().Where(i => i.FullName.Contains(".meta") == false).ToList();
            foreach (FileInfo file in files)
            {
                if (file.FullName.Contains(".svn"))
                    continue;
                if (file.FullName.Contains(".prefab") == false)
                    continue;
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                var res = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var allRender = res.GetComponentsInChildren<Renderer>();
                if (allRender == null)
                    continue;
                foreach (var r in allRender)
                {
                    r.sortingOrder = r.sortingOrder + LayerSort;
                }
                AssetDatabase.SaveAssets();

            }
            EditorUtility.DisplayDialog("设置", "设置项目 " + ProjectUtil.GetCurProjectName() + " 特效层级成功", "确定");
        }
    }
}
