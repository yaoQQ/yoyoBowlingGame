using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// 打包工具
/// </summary>
public class BuildPackageWindow : EditorWindow
{
    enum Platform
    {
        PC,
        Android,
        IOS,
    }

    /// <summary>打包平台</summary>
#if UNITY_ANDROID
    private static Platform buildPlatform = Platform.Android;
#elif UNITY_IPHONE || UNITY_IOS
    private static Platform buildPlatform = Platform.IOS;
#else
    private static Platform buildPlatform = Platform.PC;
#endif
    
    /// <summary>资源分包</summary>
    private static string[] m_resPackageOptions;
    private static int m_resPackageIndex = 0;
    private static string m_resPackageName = "";

    /// <summary>资源的SVN版本号</summary>
    private int m_TempNewAssetsVersionCode = 0;
    /// <summary>安装包版本号</summary>
    private string m_TempNewVersionCode = "0.0.0";
    /// <summary>新版本号的格式规则</summary>
    private static readonly Regex m_NewVersionRegex = new Regex(@"^\d+\.\d+\.\d+(?=\r?$)");

    enum Channel
    {
        None,
        Zhongju,
        Huawei,
        Oppo,
        Vivo,
        Xiaomi,
        CCTV,
    }

    /// <summary>打包渠道</summary>
    private static Channel buildChannel = Channel.None;

    [MenuItem("Tools/打包工具", false, 0x1000)]
    public static void OpenBuildPackageWindow()
    {
        AssetBundlePrefsData.Init();
        Open();
    }

    public static void Open()
    {
        BuildPackageWindow window = GetWindow<BuildPackageWindow>();
        window.titleContent = new GUIContent("打包工具");
        window.minSize = new Vector2(342, 696);
        window.Show();
    }

#region GUI方法

    bool EditorGUILayout_Toggle(AssetBundlePrefsData.PrefsBool key)
    {
        return EditorGUILayout_Toggle(AssetBundlePrefsData.GetDes(key), key);
    }

    bool EditorGUILayout_Toggle(string label, AssetBundlePrefsData.PrefsBool key)
    {
        EditorGUI.BeginChangeCheck();
        key.mBool = EditorGUILayout.Toggle(label, key.mBool);
        if (EditorGUI.EndChangeCheck())
        {
            AssetBundlePrefsData.Save();
        }
        return key.mBool;
    }

    /// <summary>
    /// 显示工作的进度
    /// </summary>
    void EditorUtility_DisplayCancelableProgressBar(AssetBundlePrefsData.PrefsBool data)
    {
        string des = AssetBundlePrefsData.GetDes(data);
        if (EditorUtility.DisplayCancelableProgressBar("正在打包资源", des, AssetBundlePrefsData.mDescriptionList.IndexOf(des) * 1.0f / AssetBundlePrefsData.mDescriptionList.Count))
            throw new Exception("用户取消了操作，终止进度=>" + des);
    }

#endregion

    void OnEnable()
    {
        AssetBundlePrefsData.Init();

        InitResPackage();

        m_TempNewAssetsVersionCode = EditorPrefs.GetInt("VERSION_CODE", 0);
        m_TempNewVersionCode = EditorPrefs.GetString("VERSION_NAME_PART_1", "0.0.0");
    }

    private void InitResPackage()
    {
        string resRoot = "";
        if (buildPlatform == Platform.Android)
            resRoot = Application.dataPath.Replace("/Assets", "/android_res");
        else if (buildPlatform == Platform.IOS)
            resRoot = Application.dataPath.Replace("/Assets", "/ios_res");
        else if (buildPlatform == Platform.PC)
            resRoot = Application.dataPath.Replace("/Assets", "/res");
        
        DirectoryInfo dir = new DirectoryInfo(resRoot + "/ui/");
        List<string> dirName = new List<string>();
        foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
        {
            dirName.Add(directoryInfo.Name);
        }
        m_resPackageOptions = dirName.ToArray();
    }

    void OnGUI()
    {
        BaseGUI.Init();

        bool isStartBuild = false;
        BaseGUI.BeginContents();

        isStartBuild = BuildTool();

        BaseGUI.EndContents();

        if (isStartBuild)
            StartBuild();
    }

    /// <summary>
    /// 编译工具栏
    /// </summary>
    private bool BuildTool()
    {
        bool isStartBuild;

        GUILayout.BeginVertical();
        {
            BaseGUI.BeginVBox();
            {
                buildPlatform = (Platform)EditorGUILayout.EnumPopup("平台：", buildPlatform);
                EditorGUILayout.Space();
                m_resPackageIndex = EditorGUILayout.Popup("资源包：", m_resPackageIndex, m_resPackageOptions);
                m_resPackageName = m_resPackageOptions[m_resPackageIndex];
                EditorGUILayout.Space();

                if (m_resPackageName == "base")
                {
                    m_TempNewAssetsVersionCode = EditorGUILayout.IntField("输入资源的svn版本号", m_TempNewAssetsVersionCode, BaseGUI.mNormalTextField);
                    if (m_TempNewAssetsVersionCode < 1)
                        EditorGUILayout.LabelField("需要设置合法的资源svn版本号", BaseGUI.ErrorBox);
                    else
                        EditorPrefs.SetInt("VERSION_CODE", m_TempNewAssetsVersionCode);

                    m_TempNewVersionCode = EditorGUILayout.TextField("输入安装包版本号", m_TempNewVersionCode, BaseGUI.mNormalTextField);
                    if (string.IsNullOrEmpty(m_TempNewVersionCode) || !m_NewVersionRegex.IsMatch(m_TempNewVersionCode))
                        EditorGUILayout.LabelField("需要设置合法版本号(x.x.x)", BaseGUI.ErrorBox);
                    else
                    {
                        EditorPrefs.SetString("VERSION_NAME_PART_1", m_TempNewVersionCode);
                        EditorGUILayout.LabelField("版本号：", CodeVersionFormat(m_TempNewVersionCode, m_TempNewAssetsVersionCode));
                    }
                }
                else
                {
                    m_TempNewAssetsVersionCode = EditorGUILayout.IntField("输入资源的svn版本号", m_TempNewAssetsVersionCode, BaseGUI.mNormalTextField);
                }
            }
            BaseGUI.EndVBox();

            EditorGUILayout.Space();

            BaseGUI.BeginVBox();
            {
                EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_CLEAR_KEY);
            }
            BaseGUI.EndVBox();

            EditorGUILayout.Space();

            BaseGUI.BeginVBox();
            {
                if (m_resPackageName == "base")
                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY);
                else
                    AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY = false;
                EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_RES_KEY);
            }
            BaseGUI.EndVBox();

            if (m_resPackageName == "base")
            {
                EditorGUILayout.Space();

                BaseGUI.BeginVBox();
                {
                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_COPY_STREAMINGASSETS_KEY);
                }
                BaseGUI.EndVBox();

                EditorGUILayout.Space();

                BaseGUI.BeginVBox();
                {
                    buildChannel = (Channel)EditorGUILayout.EnumPopup("渠道：", buildChannel);

                    EditorGUILayout.Space();

                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_CONNECT_PROFILER_KEY);

                    EditorGUILayout.Space();

                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_SKIP_UPDATE_KEY);
                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_BETA_CDN_KEY);
                    EditorGUILayout_Toggle(AssetBundlePrefsData.PREFAB_BUILD_TEST_SERVER_KEY);

                    EditorGUILayout.Space();

                    if (EditorGUILayout_Toggle("编译安装包", AssetBundlePrefsData.PREFAB_BUILD_BUILD_PLAYER_KEY))
                    {
                        if (buildPlatform == Platform.Android)
                        {
                            PlayerSettings.Android.keystoreName = "keystore/yoyo.keystore";
                            PlayerSettings.Android.keystorePass = "zhongju";
                            PlayerSettings.Android.keyaliasName = "yoyo";
                            PlayerSettings.Android.keyaliasPass = "zhongju";
                        }
                    }
                }
                BaseGUI.EndVBox();
            }

            EditorGUILayout.Space();

            isStartBuild = GUILayout.Button("开始");
        }
        GUILayout.EndVertical();

        return isStartBuild;
    }

    /// <summary>
    /// 开始编译，由选项确认编译的条件
    /// </summary>
    void StartBuild()
    {
        if (!EditorUtility.DisplayDialog("打包", "确定开始打包选中的类型资源", "确定", "取消"))
        {
            Debug.LogError("用户取消操作");
            return;
        }

        // 检查数据的正确性
        if (m_TempNewAssetsVersionCode < 1)
        {
            Debug.LogError("如果需要发布新的版本资源，请正确设置资源的svn版本号！！！");
            return;
        }

        if (string.IsNullOrEmpty(m_TempNewVersionCode) || !m_NewVersionRegex.IsMatch(m_TempNewVersionCode))
        {
            Debug.LogError("如果需要发布新的版本资源，请正确设置版本号！！！");
            return;
        }

        //必须先设置平台，否则路径会有问题
        if (buildPlatform == Platform.Android)
            PathUtil.platformStr = "Android";
        else if (buildPlatform == Platform.IOS)
            PathUtil.platformStr = "IOS";
        else if (buildPlatform == Platform.PC)
            PathUtil.platformStr = "PC";

        bool isException = false;

        // 开始执行用户操作
        //加载资源配置，如资源是否压缩
        //ABConfig.Reload();
        //加载打包配置
        //AssetExportConfig.Reload();
        try
        {
            if (AssetBundlePrefsData.PREFAB_BUILD_CLEAR_KEY)
            {
                EditorUtility_DisplayCancelableProgressBar(AssetBundlePrefsData.PREFAB_BUILD_CLEAR_KEY);
                //清理目录
                if (AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY)
                    IOUtil.ClearDirectory(PathEditor.RES_INCLUDE_GAME_ROOT_PATH_EDITOR);
                else
                    IOUtil.ClearDirectory(PathEditor.GetResPathEditor(m_resPackageName));
                //加载资源清单
                ResListEditor.LoadResList(m_resPackageName, false);
            }
            else
                ResListEditor.LoadResList(m_resPackageName, true);

            if (AssetBundlePrefsData.PREFAB_BUILD_RES_KEY)
            {
                EditorUtility_DisplayCancelableProgressBar(AssetBundlePrefsData.PREFAB_BUILD_RES_KEY);
                BuildReleaseRes();
            }

            if (m_resPackageName == "base")
            {
                if (AssetBundlePrefsData.PREFAB_BUILD_COPY_STREAMINGASSETS_KEY)
                {
                    EditorUtility_DisplayCancelableProgressBar(AssetBundlePrefsData.PREFAB_BUILD_COPY_STREAMINGASSETS_KEY);
                    Copy2StreamingAssets();
                }

                if (AssetBundlePrefsData.PREFAB_BUILD_BUILD_PLAYER_KEY)
                {
                    InnerBuildPlayer();
                }
            }
        }
        catch (Exception e)
        {
            isException = true;
            Debug.LogError(e);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("打包", isException ? "异常" : "任务结束", "确定");
    }

    /// <summary>
    /// 生成发布资源
    /// </summary>
    private void BuildReleaseRes()
    {
		BuildReleaseResPackage();
    }

    private void BuildReleaseResPackage()
    {
        //先加密打包lua
        BuildLua();

        //拷贝筛选资源到PathEditor.RES_PATH_EDITOR目录
        string packageResPathEditor = PathEditor.GetResPathEditor(m_resPackageName);
        IOUtil.CreateDirectory(packageResPathEditor);
        IOUtil.ClearDirectory(packageResPathEditor);
        if (buildPlatform == Platform.Android || buildPlatform == Platform.IOS || buildPlatform == Platform.PC)
        {
            string resRoot = "";
            if (buildPlatform == Platform.Android)
                resRoot = Application.dataPath.Replace("/Assets", "/android_res");
            else if (buildPlatform == Platform.IOS)
                resRoot = Application.dataPath.Replace("/Assets", "/ios_res");
            else if (buildPlatform == Platform.PC)
                resRoot = Application.dataPath.Replace("/Assets", "/res");

            if (AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY)
            {
                if (buildPlatform == Platform.Android || buildPlatform == Platform.IOS)
                {
                    //游戏打入Base包
                    IOUtil.CopyDirectory(resRoot, packageResPathEditor, ".manifest");

                    //PB
                    IOUtil.CopyDirectory(Application.dataPath.Replace("/Assets", "/res/pb"), packageResPathEditor + "/pb", ".manifest");
                }
                else if (buildPlatform == Platform.PC)
                {
                    //lua代码
                    IOUtil.CreateDirectory(packageResPathEditor + "/lua");
                    FileInfo[] luaFiles = (new DirectoryInfo(resRoot + "/lua/")).GetFiles();
                    for (int i = 0, len = luaFiles.Length; i < len; ++i)
                    {
                        File.Copy(resRoot + "/lua/" + luaFiles[i].Name, packageResPathEditor + "/lua/" + luaFiles[i].Name);
                    }

                    //File.Copy(resRoot + "/lua/" + packageName + ".unity3d", packageResPathEditor + "/lua/base.unity3d");
                    //File.Copy(resRoot + "/lua/" + packageName + ".unity3d", packageResPathEditor + "/lua/" + packageName + ".unity3d");
                    //音效
                    if (Directory.Exists(resRoot + "/audio/" + m_resPackageName))
                        IOUtil.CopyDirectory(resRoot + "/audio/" + m_resPackageName, packageResPathEditor + "/audio/" + m_resPackageName, ".manifest");
                    //特效
                    if (Directory.Exists(resRoot + "/effect/" + m_resPackageName))
                        IOUtil.CopyDirectory(resRoot + "/effect/" + m_resPackageName, packageResPathEditor + "/effect/" + m_resPackageName, ".manifest");
                    //模型
                    if (Directory.Exists(resRoot + "/model/" + m_resPackageName))
                        IOUtil.CopyDirectory(resRoot + "/model/" + m_resPackageName, packageResPathEditor + "/model/" + m_resPackageName, ".manifest");
                    //场景
                    if (Directory.Exists(resRoot + "/scene/" + m_resPackageName))
                        IOUtil.CopyDirectory(resRoot + "/scene/" + m_resPackageName, packageResPathEditor + "/scene/" + m_resPackageName, ".manifest");
                    //初始UI
                    if (m_resPackageName == "base" && Directory.Exists(resRoot + "/res_login"))
                        IOUtil.CopyDirectory(resRoot + "/res_login", packageResPathEditor + "/res_login", ".manifest");
                    //UI
                    if (Directory.Exists(resRoot + "/ui"))
                    {
                        IOUtil.CopyDirectory(resRoot + "/ui/" + m_resPackageName, packageResPathEditor + "/ui/" + m_resPackageName, ".manifest");
                        if (m_resPackageName == "base" || m_resPackageName == "mahjonghul" || m_resPackageName == "marbles")
                            File.Copy(resRoot + "/ui/ui", packageResPathEditor + "/ui/ui");
                    }

                    //PB
                    IOUtil.CopyDirectory(Application.dataPath.Replace("/Assets", "/res/pb/" + m_resPackageName), packageResPathEditor + "/pb/" + m_resPackageName, ".manifest");
                }
            }
            else
            {
                //游戏不打入Base包

                //lua代码
                IOUtil.CreateDirectory(packageResPathEditor + "/lua");
                File.Copy(resRoot + "/lua/" + m_resPackageName + ".unity3d", packageResPathEditor + "/lua/" + m_resPackageName + ".unity3d");
                //音效
                if (Directory.Exists(resRoot + "/audio/" + m_resPackageName))
                    IOUtil.CopyDirectory(resRoot + "/audio/" + m_resPackageName, packageResPathEditor + "/audio/" + m_resPackageName, ".manifest");
                //特效
                if (Directory.Exists(resRoot + "/effect/" + m_resPackageName))
                    IOUtil.CopyDirectory(resRoot + "/effect/" + m_resPackageName, packageResPathEditor + "/effect/" + m_resPackageName, ".manifest");
                //模型
                if (Directory.Exists(resRoot + "/model/" + m_resPackageName))
                    IOUtil.CopyDirectory(resRoot + "/model/" + m_resPackageName, packageResPathEditor + "/model/" + m_resPackageName, ".manifest");
                //场景
                if (Directory.Exists(resRoot + "/scene/" + m_resPackageName))
                    IOUtil.CopyDirectory(resRoot + "/scene/" + m_resPackageName, packageResPathEditor + "/scene/" + m_resPackageName, ".manifest");
                //初始UI
                if (m_resPackageName == "base" && Directory.Exists(resRoot + "/res_login"))
                    IOUtil.CopyDirectory(resRoot + "/res_login", packageResPathEditor + "/res_login", ".manifest");
                //UI
                if (Directory.Exists(resRoot + "/ui"))
                {
                    IOUtil.CopyDirectory(resRoot + "/ui/" + m_resPackageName, packageResPathEditor + "/ui/" + m_resPackageName, ".manifest");
                    if (m_resPackageName == "base" || m_resPackageName == "mahjonghul" || m_resPackageName == "marbles")
                        File.Copy(resRoot + "/ui/ui", packageResPathEditor + "/ui/ui");
                }

                //PB
                IOUtil.CopyDirectory(Application.dataPath.Replace("/Assets", "/res/pb/" + m_resPackageName), packageResPathEditor + "/pb/" + m_resPackageName, ".manifest");
            }
        }

        // 生成版本号文件
        string versionFullName;
        if (true)
            versionFullName = PathEditor.GetPackagePathEditor(m_resPackageName) + "/" + PathUtil.VERSION_FILE_NAME;
        else
            versionFullName = PathEditor.GetPackagePathIncludeGameEditor(m_resPackageName) + "/" + PathUtil.VERSION_FILE_NAME;
        if (m_resPackageName == "base")
            File.WriteAllText(versionFullName, CodeVersionFormat(m_TempNewVersionCode, m_TempNewAssetsVersionCode), new UTF8Encoding(false));
        else
            File.WriteAllText(versionFullName, m_TempNewAssetsVersionCode.ToString(), new UTF8Encoding(false));

        // 生成资源清单文件
        SaveResList();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

#region Lua加密打包
    private const string LUA_BIN_DIR = "/lua/bin/";
    private static string LUA_PATH { get { return Application.dataPath.Replace("/Assets", "/res/lua"); } }
    private static string LUA_BIN_PATH { get { return Application.dataPath + LUA_BIN_DIR; } }
    private static string LUA_BIN_INFO_PATH { get { return Application.dataPath + LUA_BIN_DIR + "ticks"; } }

    private static string LUA_OUT_PATH
    {
        get
        {
            if (buildPlatform == Platform.Android)
                return Application.dataPath.Replace("/Assets", "/android_res/lua");
            else if (buildPlatform == Platform.IOS)
                return Application.dataPath.Replace("/Assets", "/ios_res/lua");
            else
                return Application.dataPath.Replace("/Assets", "/res/lua");
        }
    }

    private static readonly Dictionary<string, string> FILE_TICKS = new Dictionary<string, string>();

    public bool BuildLua()
    {
        List<string> dirName = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(LUA_PATH);
        foreach (DirectoryInfo directoryInfo in dir.GetDirectories())
        {
            dirName.Add(directoryInfo.Name);
            BuildLuas(directoryInfo.Name);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        List<AssetBundleBuild> list = new List<AssetBundleBuild>();

        int count = dirName.Count;
        for (int i = 0; i < count; i++)
        {
            string name = dirName[i];
            AssetBundleBuild? abb = BuildLuaByType(name);
            if (abb == null)
            {
                Debug.LogWarning("没有对应类型的Lua文件=>" + name);
                continue;
            }
            list.Add(abb.Value);
        }

        IOUtil.CreateDirectory(LUA_OUT_PATH);
        if (buildPlatform == Platform.Android)
        {
            AssetBundleManifest abm = BuildPipeline.BuildAssetBundles(LUA_OUT_PATH, list.ToArray(),
                BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
        }
        else if (buildPlatform == Platform.IOS)
        {
            AssetBundleManifest abm = BuildPipeline.BuildAssetBundles(LUA_OUT_PATH, list.ToArray(),
                BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.iOS);
        }
        else if (buildPlatform == Platform.PC)
        {
            AssetBundleManifest abm = BuildPipeline.BuildAssetBundles(LUA_OUT_PATH, list.ToArray(),
                BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
        }

        return true;
    }

    private static void BuildLuas(string dir)
    {
        IOUtil.CreateDirectory(IOUtil.GetFileDir(LUA_BIN_INFO_PATH));
        FILE_TICKS.Clear();
        if (File.Exists(LUA_BIN_INFO_PATH))
        {
            Hashtable hashtable = (Hashtable)JsonUtil.jsonDecode(File.ReadAllText(LUA_BIN_INFO_PATH));
            foreach (DictionaryEntry entry in hashtable)
            {
                FILE_TICKS[(string)entry.Key] = (string)entry.Value;
            }
        }


        string luaPath = LUA_PATH + "/" + dir + "/";
        string luaOutPath = LUA_BIN_PATH + dir + "/";

        string[] files = Directory.GetFiles(luaPath, "*.lua", SearchOption.AllDirectories);
        HashSet<string> allLuaFiles = new HashSet<string>();
        foreach (string file in files)
        {
            if (!file.EndsWith(".lua"))
                continue;
            allLuaFiles.Add(file.Replace('\\', '/'));
        }

        // 删除不存在的文件
        if (Directory.Exists(luaOutPath))
        {
            string[] allOutFiles = Directory.GetFiles(luaOutPath, "*.bytes", SearchOption.AllDirectories);
            foreach (string allOutFile in allOutFiles)
            {
                int a1 = allOutFile.LastIndexOf('/') + 1;
                string outFileName = allOutFile.Substring(a1, allOutFile.LastIndexOf(".lua.bytes") - a1);
                string filePath = luaPath + outFileName.Replace('.', '/') + ".lua";
                if (!allLuaFiles.Contains(filePath))
                {
                    File.Delete(allOutFile);
                    if (FILE_TICKS.ContainsKey(filePath))
                    {
                        FILE_TICKS.Remove(filePath);
                    }
                }
            }
        }

        foreach (string filePath in allLuaFiles)
        {
            string newFileName = filePath.Replace(luaPath, String.Empty);
            newFileName = newFileName.Replace("/", ".") + ".bytes";

            string newPath = luaOutPath + newFileName;
            string directoryName = Path.GetDirectoryName(newPath);
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            FileInfo fileInfo = new FileInfo(filePath);
            string lastWriteTime = fileInfo.LastWriteTime.Ticks.ToString();

            string saveTicks;
            if (!File.Exists(newPath) || !FILE_TICKS.TryGetValue(filePath, out saveTicks) || saveTicks != lastWriteTime)
            {
                EncodeLuaFile(filePath, newPath);
                FILE_TICKS[filePath] = lastWriteTime;
            }
        }
        EditorUtility.ClearProgressBar();

        File.WriteAllText(LUA_BIN_INFO_PATH, JsonUtil.jsonEncode(FILE_TICKS));
        FILE_TICKS.Clear();
    }

    static void EncodeLuaFile(string srcFile, string outFile)
    {
        string str = File.ReadAllText(srcFile);
        File.WriteAllText(outFile, str);
    }

    static AssetBundleBuild? BuildLuaByType(string typeName)
    {
        if (!Directory.Exists(LUA_BIN_PATH + typeName))
            return null;

        string[] allFiles = Directory.GetFiles("assets" + LUA_BIN_DIR + typeName, "*.bytes", SearchOption.AllDirectories);
        if (allFiles.Length == 0)
        {
            Debug.LogWarning("未找到对应类型的脚本 => " + typeName);
            return null;
        }

        return new AssetBundleBuild
        {
            // 路径+类型
            assetBundleName = String.Format("{0}.{1}", typeName, "unity3d"),
            assetNames = allFiles
        };
    }
#endregion

    private void SaveResList()
    {
        DirectoryInfo sDir = new DirectoryInfo(PathEditor.GetResPathEditor(m_resPackageName));
        LoadResList(sDir);

        ResListEditor.SaveResList(m_resPackageName, m_TempNewAssetsVersionCode);

        if (m_resPackageName == "base")
        {
            string versionFullName = PathEditor.GetPackagePathEditor(m_resPackageName) + "/" + PathUtil.VERSION_FILE_NAME;
            File.WriteAllText(versionFullName, CodeVersionFormat(m_TempNewVersionCode, m_TempNewAssetsVersionCode), new UTF8Encoding(false));
            if (AssetBundlePrefsData.PREFAB_BUILD_CLEAR_KEY)
                File.Copy(versionFullName, versionFullName.Replace(PathUtil.VERSION_FILE_NAME, PathUtil.APK_UPDATE_FILE_NAME), true);
        }
    }

    private void LoadResList(DirectoryInfo sDir)
    {
        FileInfo[] fileArray = sDir.GetFiles();
        foreach (FileInfo file in fileArray)
            ResListEditor.AddRes(file, m_resPackageName, m_TempNewAssetsVersionCode);

        // 循环子文件夹
        DirectoryInfo[] subDirArray = sDir.GetDirectories();
        foreach (DirectoryInfo subDir in subDirArray)
        {
            LoadResList(subDir);
        }
    }

    /// <summary>
    /// 拷贝资源到StreamingAsset
    /// </summary>
    private void Copy2StreamingAssets()
    {
        IOUtil.ClearDirectory(PathEditor.GetStreamingAssetsResPathEditor(m_resPackageName));

        IOUtil.CopyDirectory(PathEditor.GetResPathEditor(m_resPackageName), PathEditor.GetStreamingAssetsResPathEditor(m_resPackageName));

        if (buildPlatform == Platform.Android || buildPlatform == Platform.IOS)
        {
            IOUtil.ClearDirectory(UtilMethod.ConnectStrs(Application.streamingAssetsPath, "/res_login"));
            IOUtil.CopyDirectory(UtilMethod.ConnectStrs(PathEditor.GetStreamingAssetsResPathEditor(m_resPackageName), "/res_login"), UtilMethod.ConnectStrs(Application.streamingAssetsPath, "/res_login"));
            IOUtil.DeleteDirectory(UtilMethod.ConnectStrs(PathEditor.GetStreamingAssetsResPathEditor(m_resPackageName), "/res_login"));
        }

        // 拷贝版本号文件
        File.Copy(PathEditor.GetPackagePathEditor(m_resPackageName) + "/" + PathUtil.VERSION_FILE_NAME, Application.streamingAssetsPath + "/" + m_resPackageName + "/" + PathUtil.VERSION_FILE_NAME, true);
        // 资源清单文件
        File.Copy(PathEditor.GetPackagePathEditor(m_resPackageName) + "/" + PathUtil.RES_LIST_FILE_NAME, Application.streamingAssetsPath + "/" + m_resPackageName + "/" + PathUtil.RES_LIST_FILE_NAME, true);

        AssetDatabase.Refresh();
    }

    private string GetBuildPlayerName()
    {
        string name = "yoyo";
        switch(buildChannel)
        {
            case Channel.Zhongju:
                name += "_zhongju";
                break;
            case Channel.Huawei:
                name += "_huawei";
                break;
            case Channel.Oppo:
                name += "_oppo";
                break;
            case Channel.Vivo:
                name += "_vivo";
                break;
            case Channel.Xiaomi:
                name += "_xiaomi";
                break;
            case Channel.CCTV:
                name += "_cctv";
                break;
        }
        if (AssetBundlePrefsData.PREFAB_BUILD_BETA_CDN_KEY)
            name += "_beta";
        else
            name += "_stable";
        if (buildPlatform != Platform.IOS)
        {
            name += string.Format("_{0}_{1}", CodeVersionFormat(m_TempNewVersionCode, m_TempNewAssetsVersionCode),
                DateTime.Now.ToString("yyyyMMddHHmm"));
        }
        if (AssetBundlePrefsData.PREFAB_BUILD_SKIP_UPDATE_KEY)
            name += "_skipUpdate";
        if (AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY)
            name += "_includeGame";
        if (AssetBundlePrefsData.PREFAB_BUILD_CONNECT_PROFILER_KEY)
            name += "_profiler";
        return name;
    }

    /// <summary>
    /// 编译一个运行程序
    /// </summary>
    private void InnerBuildPlayer()
    {
        if (buildPlatform == Platform.IOS)
            PlayerSettings.bundleVersion = m_TempNewVersionCode;
        else
            PlayerSettings.bundleVersion = CodeVersionFormat(m_TempNewVersionCode, m_TempNewAssetsVersionCode);
        string fileNameSuffix = ".exe";
        string symbol = "";
        if (AssetBundlePrefsData.PREFAB_BUILD_GAME_RES_KEY)
            symbol += ";INCLUDE_GAME";
        if (AssetBundlePrefsData.PREFAB_BUILD_SKIP_UPDATE_KEY)
            symbol += ";SKIP_UPDATE";
        if (AssetBundlePrefsData.PREFAB_BUILD_TEST_SERVER_KEY)
            symbol += ";TEST_SERVER";
        if (AssetBundlePrefsData.PREFAB_BUILD_BETA_CDN_KEY)
            symbol += ";BETA_CDN";

        int channel = (int)buildChannel;
        symbol += ";CHANNEL_" + channel;

        string dir = "";
        if (buildPlatform == Platform.Android)
        {
            fileNameSuffix = ".apk";
            PlayerSettings.Android.bundleVersionCode = m_TempNewAssetsVersionCode;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbol);//宏定义的设置
            dir = Application.dataPath.Replace("/Assets", "/Build/Android");
        }
        else if (buildPlatform == Platform.IOS)
        {
            fileNameSuffix = "";
            PlayerSettings.iOS.buildNumber = m_TempNewAssetsVersionCode.ToString();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbol);//宏定义的设置
            dir = Application.dataPath.Replace("/Assets", "/Build/IOS");
        }
        else if (buildPlatform == Platform.PC)
        {
            PlayerSettings.iOS.buildNumber = m_TempNewAssetsVersionCode.ToString();
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbol);//宏定义的设置
            dir = Application.dataPath.Replace("/Assets", "/Build/PC");
        }
        IOUtil.CreateDirectory(dir);

        string path = UtilMethod.ConnectStrs(dir, "/", GetBuildPlayerName(), fileNameSuffix);
        if (buildPlatform == Platform.IOS)
            IOUtil.DeleteDirectory(path);
        if (AssetBundlePrefsData.PREFAB_BUILD_CONNECT_PROFILER_KEY)
            BuildPipeline.BuildPlayer(new string[] { "Assets/Login.unity", "Assets/Main.unity" }, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.ShowBuiltPlayer | BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);
        else
            BuildPipeline.BuildPlayer(new string[] { "Assets/Login.unity", "Assets/Main.unity" }, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.ShowBuiltPlayer);
    }

    /// <summary>
    /// 资源版本格式
    /// </summary>
    /// <param name="version">版本号</param>
    /// <param name="svnVersion">svn版本号</param>
    /// <returns>版本号</returns>
    static string CodeVersionFormat(string version, int svnVersion)
    {
        return string.Format("{0}.{1}", version, svnVersion);
    }
}