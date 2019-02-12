using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExportManager
{
    static string Get_PC_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/res/scene/";
    }
    static string Get_IOS_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/ios_res/scene/";
    }

    static string Get_Android_expPath()
    {
        return "/../../../" + PathUtil.GetClientName() + "/android_res/scene/";
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

    [MenuItem("Export/Scene/Exp Scene Res (PC)")]
    public static void ExpSceneRes_PC()
    {
        ExpSceneRes(BuildTarget.StandaloneWindows64);
    }
    [MenuItem("Export/Scene/Exp Scene Res (Android)")]
    public static void ExpSceneRes_Android()
    {
        ExpSceneRes(BuildTarget.Android);
    }
    [MenuItem("Export/Scene/Exp Scene Res (IOS)")]
    public static void ExpSceneRes_IOS()
    {
        ExpSceneRes(BuildTarget.iOS);
    }



    public static void ExpSceneRes(BuildTarget target)
    {
       
        string[] dirArr = Directory.GetDirectories(Application.dataPath + "/Project/"+ ProjectUtil.GetCurProjectName() + "/scene");
        foreach(string dir in dirArr)
        {
            DirectoryInfo directory = new DirectoryInfo(dir);
            if (directory.Name.Contains(".svn"))
                continue;
            ExpOneSceneRes(target,directory);
        }

    }

    public static void ExpOneSceneRes(BuildTarget target,DirectoryInfo directory)
    {
        string packageName = directory.Name;

        List<AssetBundleBuild> abbArr = new List<AssetBundleBuild>();

        if (Directory.Exists(directory.FullName + "/anim"))
        {
            //设置anim
            DirectoryInfo animDir = new DirectoryInfo(directory.FullName + "/anim");
            FileInfo[] fileArr = animDir.GetFiles("*.anim");
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");

                abb.assetBundleName = "anim/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }

        }

        if (Directory.Exists(directory.FullName + "/ctrl"))
        {
            //设置ctrl
            DirectoryInfo ctrlDir = new DirectoryInfo(directory.FullName + "/ctrl");
            FileInfo[] fileArr = ctrlDir.GetFiles("*.controller");
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");

                abb.assetBundleName = "ctrl/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }

        }

        if (Directory.Exists(directory.FullName + "/shader"))
        {
            //设置shader
            DirectoryInfo shaderDir = new DirectoryInfo(directory.FullName + "/shader");
            FileInfo[] fileArr = shaderDir.GetFiles("*.shader");
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");

                abb.assetBundleName = "shader/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }
        }



        if (Directory.Exists(directory.FullName+ "/mat"))
        {
            //设置mat
            DirectoryInfo matDir = new DirectoryInfo(directory.FullName + "/mat");
            FileInfo[] fileArr = matDir.GetFiles("*.mat");
            foreach(FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\","/").Replace(Application.dataPath, "Assets");
                
                abb.assetBundleName = "mat/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }
           
        }

        if (Directory.Exists(directory.FullName + "/mesh"))
        {
            //设置mesh
            DirectoryInfo meshDir = new DirectoryInfo(directory.FullName + "/mesh");
            FileInfo[] objArr = meshDir.GetFiles("*.obj");
            FileInfo[] fbxArr = meshDir.GetFiles("*.FBX");
            List<FileInfo> fileArr = new List<FileInfo>();
            fileArr.AddRange(objArr);
            fileArr.AddRange( fbxArr);
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                abb.assetBundleName = "mesh/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }
        }

        if (Directory.Exists(directory.FullName + "/texture"))
        {
            //设置mesh
            DirectoryInfo textureDir = new DirectoryInfo(directory.FullName + "/texture");
            FileInfo[] fileArr = textureDir.GetFiles("*.png");
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                abb.assetBundleName = "texture/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }
        }

        if (Directory.Exists(directory.FullName + "/prefab"))
        {
            //设置mesh
            DirectoryInfo matDir = new DirectoryInfo(directory.FullName + "/prefab");
            FileInfo[] fileArr = matDir.GetFiles("*.prefab");
            foreach (FileInfo file in fileArr)
            {
                AssetBundleBuild abb = new AssetBundleBuild();
                string path = file.FullName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                abb.assetBundleName = "prefab/" + Path.GetFileNameWithoutExtension(file.Name) + ".unity3d";
                abb.assetNames = new string[] { path };
                abbArr.Add(abb);
            }
        }



        string expPath = Application.dataPath + GetPath(target) + packageName;
        var expDir =new DirectoryInfo(expPath);
        if (!expDir.Exists)
        {
            expDir.Create();
        }

        string delPath;
        DirectoryInfo delDir;

        delPath = Application.dataPath + GetPath(target) + packageName + "/anim";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);

        delPath = Application.dataPath + GetPath(target) + packageName + "/ctrl";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);


        delPath = Application.dataPath + GetPath(target) + packageName + "/shader";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);

        delPath = Application.dataPath + GetPath(target) + packageName + "/mat";
         delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);

        delPath = Application.dataPath + GetPath(target) + packageName + "/mesh";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);

        delPath = Application.dataPath + GetPath(target) + packageName + "/texture";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);

        delPath = Application.dataPath + GetPath(target) + packageName + "/prefab";
        delDir = new DirectoryInfo(delPath);
        if (delDir.Exists) delDir.Delete(true);



        BuildPipeline.BuildAssetBundles(expPath, abbArr.ToArray(), BuildAssetBundleOptions.None, target);
        EditorUtility.DisplayDialog("导出","项目 "+ ProjectUtil.GetCurProjectName()+" 导出场景资源成功", "确定");
    }




    [MenuItem("Export/Scene/Exp Scene Data")]
    public static void ExpSceneData()
    {
        GameObject sceneGO = GameObject.Find("[Scene]");
        if(sceneGO==null)
        {
            Debug.LogError("此场景没有 [Scene] ");
            return;
        }
        if (sceneGO.transform.position != Vector3.zero)
        {
            Debug.LogError("[Scene] 必须原点位置");
            return;
        }

        SceneData sceneData = new SceneData();

        ExpLightInfo(ref sceneData);

        SceneContainer[] containerArr = sceneGO.GetComponentsInChildren<SceneContainer>();
        List<SceneContainerInfo> containerInfoList = new List<SceneContainerInfo>();
        foreach(SceneContainer sc in containerArr)
        {
            SceneContainerInfo info = new SceneContainerInfo();
            info.containerName = sc.containerName;
            info.layerMaskValue = sc.gameObject.layer;
            info.SetTransformInfo(sc.gameObject.transform);



            SceneCell[] cellArr = sc.GetComponentsInChildren<SceneCell>();

            List<SceneCellInfo> cellInfoList = new List<SceneCellInfo>();
            foreach(SceneCell cell in cellArr)
            {
                SceneCellInfo cellInfo = new SceneCellInfo();
                cellInfo.SetTransformInfo(cell.gameObject.transform);
                cellInfo.prefabName = cell.prefabName;
                cellInfoList.Add(cellInfo);
            }
            info.cellInfoArr = cellInfoList.ToArray();
            containerInfoList.Add(info);
        }
        sceneData.containerInfoArr = containerInfoList.ToArray();

        SceneCamera[] cameraArr= sceneGO.GetComponentsInChildren<SceneCamera>();
        List<SceneCameraInfo> cameraInfoList = new List<SceneCameraInfo>();
        foreach (SceneCamera sc in cameraArr)
        {
            SceneCameraInfo info = new SceneCameraInfo();
            Camera c = sc.GetComponent<Camera>();
            info.cameraName = sc.cameraName;
            info.cameraEnable = c.enabled;
            info.cullingMask = c.cullingMask;
            info.nearValue = c.nearClipPlane;
            info.farValue = c.farClipPlane;
            info.cameraDepth = c.depth;
			info.fieldOfView = c.fieldOfView;
			info.clearFlags = (int)c.clearFlags;
			info.orthographic = c.orthographic;
			info.orthographicSize = c.orthographicSize;
            info.SetTransformInfo(sc.gameObject.transform);
           
            cameraInfoList.Add(info);
        }

        sceneData.cameraInfoArr = cameraInfoList.ToArray();

        ScenePosPoint[] posPointArr = sceneGO.GetComponentsInChildren<ScenePosPoint>();
        List<ScenePosPointInfo> posPointInfoList = new List<ScenePosPointInfo>();
        foreach (ScenePosPoint sp in posPointArr)
        {
            ScenePosPointInfo info = new ScenePosPointInfo();
            info.SetTransformInfo(sp.gameObject.transform);
            info.pointName = sp.pointName;
            posPointInfoList.Add(info);
        }
        sceneData.posPointInfoArr = posPointInfoList.ToArray();


        string dataJson=JsonUtility.ToJson(sceneData);
        //string p = "Assets/SysData.asset";
        //AssetDatabase.CreateAsset(sceneData, p);

        SaveData(Application.dataPath+ Get_PC_expPath(), Application.loadedLevelName, dataJson);
        SaveData(Application.dataPath + Get_IOS_expPath(), Application.loadedLevelName, dataJson);
        SaveData(Application.dataPath + Get_Android_expPath(), Application.loadedLevelName, dataJson);

        EditorUtility.DisplayDialog("导出", "导出场景数据成功", "确定");
    }
    static void SaveData(string savePath, string sceneName,string dataJson)
    {
        if (!Directory.Exists(savePath))
        {
            Debug.LogWarning(savePath +"   路径不存在");
            return;
        }
        string fileName = savePath + sceneName + "/data/" + sceneName + ".json";
        FileInfo fi = new FileInfo(fileName);
        var di = fi.Directory;
        if (!di.Exists)
            di.Create();
        FileStream fs;
        try
        {
            fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        }
        catch (Exception ex)
        {
            Debug.LogError("目录被打开，不能生成新文件：" + fileName);
            return;
        }
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII);
        sw.WriteLine(dataJson);
        sw.Close();
        fs.Close();
    }


    static void ExpLightInfo(ref SceneData sceneData)
    {
        Scene curScene= SceneManager.GetActiveScene();
        GameObject[] gos= curScene.GetRootGameObjects();
        foreach (GameObject go in gos)
        {
            Light l = go.GetComponent<Light>();
            if (l != null)
            {
                sceneData.lightInfo = new SceneLightInfo();
                sceneData.lightInfo.color = l.color;
                sceneData.lightInfo.intensity = l.intensity;
                sceneData.lightInfo.SetTransformInfo(l.transform);
                break;
            }
        }
    }

}
