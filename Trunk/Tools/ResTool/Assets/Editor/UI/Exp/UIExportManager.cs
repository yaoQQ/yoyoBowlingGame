using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UIExportManager
{
    private static BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;

    [MenuItem("Export/UI/Export One Mid ")]
    public static void ExportOneUI_Mid()
    {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.activeInHierarchy && Selection.activeGameObject.GetComponent<UIBaseWidget>() != null)
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
            string relativePath = path.Substring(path.IndexOf("/ui/") + 4).Replace("/Exp_prefab/", "/");
            string packageName = relativePath.Substring(0, relativePath.IndexOf("/"));
            relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);
            relativePath = relativePath.Substring(0, relativePath.LastIndexOf("/") + 1);
            ExportMiddleware.ExpMid(Selection.activeGameObject, packageName, relativePath);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("导出", "导出单个中间键成功", "确定");
        }
        else
            EditorUtility.DisplayDialog("导出失败", "请先选中一个prefab", "确定");
    }

    static string GetPrefabPath(string packageName)
    {
        return Application.dataPath + "/Project/" + ProjectUtil.GetCurProjectName() + "/ui/" + packageName + "/Exp_prefab/";
    }

    [MenuItem("Export/UI/Export All Mid")]
    public static void ExportAllUI_Mid()
    {
        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(Application.dataPath + "/Project/" + ProjectUtil.GetCurProjectName() + "/ui/");
        DirectoryInfo[] packages = rootDirectoryInfo.GetDirectories();
        foreach(DirectoryInfo package in packages)
        {
            DirectoryInfo folder = new DirectoryInfo(GetPrefabPath(package.Name));
            FileInfo[] files = folder.GetFiles("*.prefab", SearchOption.AllDirectories);
            
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                string path = fileInfo.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                string relativePath = path.Substring(path.IndexOf("/ui/") + 4).Replace("/Exp_prefab/", "/");
                string packageName = relativePath.Substring(0, relativePath.IndexOf("/"));
                relativePath = relativePath.Substring(relativePath.IndexOf("/") + 1);
                relativePath = relativePath.Substring(0, relativePath.LastIndexOf("/") + 1);
                ExportMiddleware.ExpMid(prefab, packageName, relativePath);
            }
        }
        
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("导出", "导出所有中间键成功", "确定");
    }

    [MenuItem("Export/UI/Bind One Mono %E")]
    public static void BindOneUI_Mono()
    {
        if (EditorApplication.isCompiling || Application.isPlaying)
        {
            EditorUtility.DisplayDialog("绑定失败", "请等待编译完成", "确定");
            return;
        }
        if (Selection.activeGameObject != null && !Selection.activeGameObject.activeInHierarchy && Selection.activeGameObject.GetComponent<UIBaseWidget>() != null)
        {
            BindMonoware.BindMono(Selection.activeGameObject);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("绑定", "绑定单个UIMono成功", "确定");
        }
        else
            EditorUtility.DisplayDialog("绑定失败", "请先选中一个prefab或确定Prefab挂载UIBaseMono", "确定");
    }
    static string Get_PC_ExpPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/res/ui/";
    }

    static string Get_IOS_ExpPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/ios_res/ui/";
    }

    static string Get_Android_ExpPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/android_res/ui/";
    }


    static string Get_PC_LoginPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/res/res_login/";
    }

    static string Get_IOS_LoginPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/ios_res/res_login/";
    }

    static string Get_Android_LoginPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/android_res/res_login/";
    }


    static Dictionary<string, List<string>> abbDic = new Dictionary<string, List<string>>();

    static void AddToAbbDic(string key, string resPath)
    {
        List<string> pathList = null;
        abbDic.TryGetValue(key, out pathList);
        if (pathList==null)
        {
            pathList = new List<string>();
            abbDic.Add(key, pathList);
        }
        if(!pathList.Contains(resPath))
        {
            pathList.Add(resPath);
        }
    }
    public static void ExportAllUIRes(string expPath, BuildTarget buildTarget, string loginPath)
    {
        string projectName = ProjectUtil.GetCurProjectName();
        if(string.IsNullOrEmpty(projectName))
        {
            EditorUtility.DisplayDialog("错误",  " 项目 不能为空", "确定");
            return;
        }

        ExportLoginUIRes(projectName, loginPath, buildTarget);


        abbDic.Clear();
        string[] dirArr = Directory.GetDirectories(Application.dataPath + "/Project/" + projectName + "/ui");
        foreach (string dir in dirArr)
        {
            DirectoryInfo directory = new DirectoryInfo(dir);
            ExportOnePackageUIRes(projectName, directory.Name, expPath, buildTarget);
        }
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        AssetDatabase.RemoveUnusedAssetBundleNames();
        List<AssetBundleBuild> ui_abbList = new List<AssetBundleBuild>();
        foreach (KeyValuePair<string, List<string>> kvp in abbDic)
        {
            AssetBundleBuild tempABB = new AssetBundleBuild();
            tempABB.assetBundleName = kvp.Key;
            tempABB.assetNames = kvp.Value.ToArray();
            ui_abbList.Add(tempABB);
        }
        string outputPath = Application.dataPath + expPath;
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, ui_abbList.ToArray(), options, buildTarget);


        EditorUtility.DisplayDialog("导出", ProjectUtil.GetCurProjectName() +" 项目 导出UI资源成功", "确定");
    }

    /// <summary>
    /// 游戏第一个加载界面的打包
    /// </summary>
    public static void ExportLoginUIRes(string projectName, string loginPath, BuildTarget buildTarget)
    {
        List<AssetBundleBuild> load_abbList = new List<AssetBundleBuild>();
        AssetBundleBuild loginABB = new AssetBundleBuild();
        loginABB.assetBundleName = "login.unity3d";
        loginABB.assetNames = new string[] { "Assets/Project/" + projectName + "/ui/base/Exp_login" };
        load_abbList.Add(loginABB);

        string outputPath = Application.dataPath + loginPath;
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, load_abbList.ToArray(), options, buildTarget);
    }

    public static void ExportOnePackageUIRes(string projectName, string packageName, string expPath, BuildTarget buildTarget)
    {
        int i;
        DirectoryInfo fontFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Font/");
        if (fontFolder.Exists)
        {
            FileInfo[] files = fontFolder.GetFiles("*.*", SearchOption.AllDirectories);
            for (i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                string path = fileInfo.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                if (path.EndsWith(".ttf") || path.EndsWith(".TTF") ||
                    path.EndsWith(".otf") || path.EndsWith(".OTF") ||
                    path.EndsWith(".fontsettings") || path.EndsWith(".mat") || path.EndsWith(".png"))
                {
                    Object ob = AssetDatabase.LoadAssetAtPath<Object>(path);
                    ob.name = ob.name.ToLower();
                    AssetImporter fontImporter = AssetImporter.GetAtPath(path);

                    if (fontImporter is TextureImporter)
                    {
                        TextureImporter textureImporter = fontImporter as TextureImporter;
                        textureImporter.filterMode = FilterMode.Bilinear;
                        textureImporter.maxTextureSize = 1024;
                        textureImporter.textureFormat = TextureImporterFormat.Automatic16bit;
                    }

                    AddToAbbDic(packageName + "/font/font_" + ob.name + ".unity3d", path);
                }
            }
        }

        //帧动画图集
        DirectoryInfo framePngFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Import_frame_png/");
        if (framePngFolder.Exists)
        {
            FileInfo[] framePngFiles = framePngFolder.GetFiles("*.png");
            foreach (FileInfo framePng in framePngFiles)
            {
                string path = framePng.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AddToAbbDic(packageName + "/framepng/frame_" + framePng.Name + ".unity3d", path);
            }

            DirectoryInfo singlePngFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Import_single_png/");
            FileInfo[] singlePngFiles = singlePngFolder.GetFiles("*.png");
            foreach (FileInfo singlePng in singlePngFiles)
            {
                string path = singlePng.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AddToAbbDic(packageName + "/singlepng/single_" + singlePng.Name + ".unity3d", path);
            }
        }

        //帧动画序列文件
        DirectoryInfo mcFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Exp_movieclip/");
        if (mcFolder.Exists)
        {
            FileInfo[] mcFiles = mcFolder.GetFiles("*.asset");
            foreach (FileInfo mc in mcFiles)
            {
                string path = mc.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AddToAbbDic(packageName + "/movieclip/movieclip_" + mc.Name + ".unity3d", path);
            }
        }

        //shader打包
        DirectoryInfo shaderFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Shader/");
        if (shaderFolder.Exists)
        {
            FileInfo[] shaderFiles = mcFolder.GetFiles("*.shader");
            foreach (FileInfo shader in shaderFiles)
            {
                string path = shader.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AddToAbbDic(packageName + "/shader/shader_" + shader.Name + ".unity3d", path);
            }
        }

        //mat打包
        DirectoryInfo matFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Mat/");
        if (matFolder.Exists)
        {
            FileInfo[] matFiles = matFolder.GetFiles("*.mat");
            foreach (FileInfo mat in matFiles)
            {
                string path = mat.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                AddToAbbDic(packageName + "/mat/mat_" + mat.Name + ".unity3d", path);
            }
        }



        //骨骼动画打包
        DirectoryInfo spineFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Import_spine/");
        if (spineFolder.Exists)
        {
            DirectoryInfo[] spineDirArr = spineFolder.GetDirectories();
            for (i = 0; i < spineDirArr.Length; i++)
            {
                DirectoryInfo dirInfo = spineDirArr[i];
                string path = dirInfo.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");

                path = "Assets" + path;
                AssetImporter spineImporter = AssetImporter.GetAtPath(path);
                //spineImporter.assetBundleName = "spine/" + dirInfo.Name + ".unity3d";

                AddToAbbDic(packageName + "/spine/spine_" + dirInfo.Name + ".unity3d", path);
            }
        }

        //普通图集
        DirectoryInfo pngFolder = new DirectoryInfo(Application.dataPath + "/Project/" + projectName + "/ui/" + packageName + "/Import_png/");
        if (pngFolder.Exists)
        {
            DirectoryInfo[] pngDirArr = pngFolder.GetDirectories();
            foreach (DirectoryInfo dirInfo in pngDirArr)
            {
                FileInfo[] pngArr = dirInfo.GetFiles("*.png");
                foreach (FileInfo pngFile in pngArr)
                {
                    string path = pngFile.FullName.Replace("\\", "/");
                    path = path.Replace(Application.dataPath, "");
                    path = "Assets" + path;
                    string packerName = pngFile.Name.Split('@')[0];
                    AddToAbbDic(packageName + "/png/packer_" + packerName + ".unity3d", path);
                }
            }
        }

        
        //预置体
        DirectoryInfo prefabFolder = new DirectoryInfo(GetPrefabPath(packageName));
        if (prefabFolder.Exists)
        {
            FileInfo[] prefabFiles = prefabFolder.GetFiles("*.prefab", SearchOption.AllDirectories);
            for (int j = 0; j < prefabFiles.Length; j++)
            {
                FileInfo fileInfo = prefabFiles[j];
                string path = fileInfo.FullName.Replace("\\", "/");
                path = path.Replace(Application.dataPath, "");
                path = "Assets" + path;
                //Debug.Log("预置体路径  ：   " + path);
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                prefab.name = prefab.name.ToLower();
                //AssetImporter assetImporter = AssetImporter.GetAtPath(path);
                //assetImporter.assetBundleName = "prefab/"+ packName +"/"+ prefab.name + ".unity3d";
                string relativePath = path.Substring(path.IndexOf("/Exp_prefab/") + 12);
                relativePath = relativePath.Substring(0, relativePath.LastIndexOf("/") + 1);
                AddToAbbDic(packageName + "/prefab/" + relativePath + prefab.name + ".unity3d", path);
            }
        }
    }

    [MenuItem("Export/UI/Export All UI Res (pc) %W")]
    public static void ExportAllUIRes_PC()
    {
        ExportAllUIRes(Get_PC_ExpPath(), BuildTarget.StandaloneWindows64, Get_PC_LoginPath());
    }

    [MenuItem("Export/UI/Export All UI Res (ios)")]
    public static void ExportAllUIRes_IOS()
    {
        ExportAllUIRes(Get_IOS_ExpPath(), BuildTarget.iOS, Get_IOS_LoginPath());
    }
    [MenuItem("Export/UI/Export All UI Res (android)")]
    public static void ExportAllUIRes_Android()
    {
        ExportAllUIRes(Get_Android_ExpPath(), BuildTarget.Android, Get_Android_LoginPath());
    }


    [MenuItem("Export/UI/Del All Res")]
    public static void DelAllRes()
    {
        DelRes("/../../../" + PathUtil.GetClientName() + "/res/");
        DelRes("/../../../" + PathUtil.GetClientName() + "/ios_res/");
        DelRes("/../../../" + PathUtil.GetClientName() + "/android_res/");
        EditorUtility.DisplayDialog("清理", ProjectUtil.GetCurProjectName()+" 删除所有打包资源成功", "确定");
    }
    static void DelRes(string path)
    {
        FileInfo fi = new FileInfo(Application.dataPath + path);
        var di = fi.Directory;
        if (di.Exists)
        {
            di.Create();
            DeleteFolder(Application.dataPath + path);
            Directory.Delete(Application.dataPath + path, true);
        }
    }
    static void DeleteFolder(string dirPath)
    {
        if (Directory.Exists(dirPath))
        {
            foreach (string content in Directory.GetFileSystemEntries(dirPath))
            {
                if (Directory.Exists(content))
                {
                    Directory.Delete(content, true);
                }
                else if (File.Exists(content))
                {
                    File.Delete(content);
                }
            }
        }
    }

    /*[MenuItem("Export/UI/Check Reference Missing")]
    public static void CheckReferenceMissing()
    {
        Debug.ClearDeveloperConsole();
        int i;
        int j;
        DirectoryInfo prefabFolder = new DirectoryInfo(GetPrefabPath());
        FileInfo[] prefabFiles = prefabFolder.GetFiles("*.prefab", SearchOption.AllDirectories);
        for (i = 0; i < prefabFiles.Length; i++)
        {
            FileInfo fileInfo = prefabFiles[i];
            string path = fileInfo.FullName.Replace("\\", "/");
            path = path.Replace(Application.dataPath, "");
            path = "Assets" + path;
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            Image[] imageArr = prefab.GetComponentsInChildren<Image>();
            for( j=0;j< imageArr.Length;j++)
            {
                Image image = imageArr[j];
                if(image.enabled)
                {
                    if(image.sprite==null&& image.gameObject.GetComponent<EffectWidget>()==null)
                    {
                        Debug.LogError("图片缺失： " + GetPathStr(image.gameObject, prefab.name));
                    }
                   
                }
            }

            Text[] textArr = prefab.GetComponentsInChildren<Text>();
            for (j = 0; j < textArr.Length; j++)
            {
                Text txt = textArr[j];
                if (txt.font.name == "Arial")
                {
                    Debug.LogError("不能用Arial字体=====》 "+ prefab.name+"   "+ txt.gameObject.name);
                }
            }
        }
    }*/
    static string GetPathStr(GameObject go, string rootName)
    {
        string widgetPath = string.Empty;

        while (go.name != rootName)
        {
            if (widgetPath == string.Empty)
            {
                widgetPath = go.name;
            }
            else
            {
                widgetPath = (go.name + "/" + widgetPath);

            }
            go = go.transform.parent.gameObject;
        }
        return "@ " + rootName + " @ " +widgetPath;
    }
}