using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.XCodeEditor;
using UnityEngine;
using PBXProject = UnityEditor.iOS.Xcode.Custom.PBXProject;

public static class XCodePostProcess
{
    [PostProcessBuild(100)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.Log("XCodePostProcess.OnPostProcessBuild");

        if (buildTarget != BuildTarget.iOS)
        {
            Debug.LogWarning("Target is not IOS. XCodePostProcess will not run");
            return;
        }

        /*#region Bugly
        // Create a new project object from build target
        XCProject project = new XCProject(pathToBuiltProject);

        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        string[] files = Directory.GetFiles(Application.dataPath, "*.projmods", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            UnityEngine.Debug.Log("ProjMod File: " + file);
            project.ApplyMod(file);
        }

        //TODO disable the bitcode for iOS 9
        //project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Release");
        //project.overwriteBuildSetting("ENABLE_BITCODE", "NO", "Debug");

        //TODO implement generic settings as a module option
        //		project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution", "Release");

        // Finally save the xcode project
        project.Save();
        #endregion*/

        //得到xcode工程的路径
        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        string target = proj.TargetGuidByName("Unity-iPhone");

        //proj.SetTeamId(target, "MS85CXHJPZ");

        // ENABLE_BITCODE=False
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");
        //proj.SetBuildProperty(target, "OBJECTIVE-C_AUTOMATIC_REFERENCE_COUNTING", "false");

        //proj.SetBuildPropertyForConfig();//other inker flags

        // add extra framework(s)
        proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);
        proj.AddFrameworkToProject(target, "CFNetwork.framework", true);
        proj.AddFrameworkToProject(target, "ImageIO.framework", true);
        proj.AddFrameworkToProject(target, "JavaScriptCore.framework", true);
        proj.AddFrameworkToProject(target, "SafariServices.framework", true);
        proj.AddFrameworkToProject(target, "Security.framework", true);
        proj.AddFrameworkToProject(target, "libc++.tbd", true);
        proj.AddFrameworkToProject(target, "libsqlite3.0.tbd", true);
        proj.AddFrameworkToProject(target, "libz.tbd", true);

        //第三方framework
        /*
        string iosFrameworkPath = Application.dataPath.Replace("/Assets", "/ios_framework");
        string xcodeRootPath = projPath.Replace("/Unity-iPhone.xcodeproj/project.pbxproj", "");
        IOUtil.CopyDirectory(iosFrameworkPath + "/AMapFoundationKit.framework", Path.Combine(pathToBuiltProject, "AMapFoundationKit.framework"));
        IOUtil.CopyDirectory(iosFrameworkPath + "/AMapLocationKit.framework", Path.Combine(pathToBuiltProject, "AMapLocationKit.framework"));
        IOUtil.CopyDirectory(iosFrameworkPath + "/AMapSearchKit.framework", Path.Combine(pathToBuiltProject, "AMapSearchKit.framework"));
        proj.AddFileToBuild(target, proj.AddFile("AMapFoundationKit.framework", "AMapFoundationKit.framework", PBXSourceTree.Source));
        proj.AddFileToBuild(target, proj.AddFile("AMapLocationKit.framework", "AMapLocationKit.framework", PBXSourceTree.Source));
        proj.AddFileToBuild(target, proj.AddFile("AMapSearchKit.framework", "AMapSearchKit.framework", PBXSourceTree.Source));
        */

        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
        //proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-force_load $(SRCROOT)/Libraries/Plugins/iOS/EZCodeScanner/libzbar.a");

        //微信
        //string WXSdk_m_guid = proj.FindFileGuidByProjectPath("Libraries/Plugins/iOS/WXSdk.m");
        //var flags = proj.GetCompileFlagsForFile(target, WXSdk_m_guid);
        //flags.Add("-fno-objc-arc");
        //proj.SetCompileFlagsForFile(target, WXSdk_m_guid, flags);

        //扫码
        string fileGuidEZCodeScanner = proj.FindFileGuidByProjectPath("Libraries/Plugins/iOS/EZCodeScanner/EZCodeScannerViewController.mm");
        var flags = proj.GetCompileFlagsForFile(target, fileGuidEZCodeScanner);
        flags.Add("-fno-objc-arc");
        proj.SetCompileFlagsForFile(target, fileGuidEZCodeScanner, flags);

        // rewrite to file
        File.WriteAllText(projPath, proj.WriteToString());


        //========================================plist=======================================================

        // Get plist
        string plistPath = pathToBuiltProject + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        // Get root
        PlistElementDict rootDict = plist.root;
        //本地化相关，如果用户所在地没有相应的语言资源，则用这个key的value来作为默认
        rootDict.SetString("CFBundleDevelopmentRegion", "zh_CN");
        //rootDict.SetBoolean("UIFileSharingEnabled", true);

        //权限
        rootDict.SetString("NSCameraUsageDescription", "需要获取相机权限用于拍照并上传头像或发送聊天图片，还能用于扫描好友的二维码添加好友");
        rootDict.SetString("NSPhotoLibraryUsageDescription", "需要获取相册权限用于从相册选择图片上传头像或发送聊天图片");
        //rootDict.SetString("NSMicrophoneUsageDescription", "需要获取麦克风权限用于语音聊天");
        rootDict.SetString("NSLocationAlwaysUsageDescription", "需要获取定位信息用于在地图上显示您的位置，您也可以看到您附近正在举办的活动");
        rootDict.SetString("NSLocationWhenInUseUsageDescription", "需要获取定位信息用于在地图上显示您的位置，您也可以看到您附近正在举办的活动");
        rootDict.SetString("NSLocationAlwaysAndWhenInUseUsageDescription", "需要获取定位信息用于在地图上显示您的位置，您也可以看到您附近正在举办的活动");

        //状态栏
        //View controller-based status bar appearance  设置为 NO，如果设置成YES，整个APP中都不会隐藏状态栏
        //Status bar is initially hidden 设置为 NO，如果设置为YES，整个APP中都不会显示状态栏
        rootDict.SetString("View controller-based status bar appearance", "NO");
        rootDict.SetString("Status bar is initially hidden", "NO");

        PlistElementDict securityDict = rootDict.CreateDict("NSAppTransportSecurity");
        securityDict.SetBoolean("NSAllowsArbitraryLoads", true);

        //微信
        PlistElementArray lsApplicationQueriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
        lsApplicationQueriesSchemes.AddString("weixin");
        lsApplicationQueriesSchemes.AddString("wechat");
        lsApplicationQueriesSchemes.AddString("iosamap");//高德
        PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");
        PlistElementDict wxUrl = urlTypes.AddDict();
        wxUrl.SetString("CFBundleTypeRole", "Editor");
        wxUrl.SetString("CFBundleURLName", "com.zhongyu.yoyo");
        wxUrl.SetString("CFBundleURLIconFile", "LaunchScreen-iPhoneLandscape");
        PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
        wxUrlScheme.AddString("wx001cf6e637ffbe12");

        PlistElementDict zhifuUrl = urlTypes.AddDict();
        zhifuUrl.SetString("CFBundleTypeRole", "Editor");
        zhifuUrl.SetString("CFBundleURLName", "com.zhongyu.yoyo");
        zhifuUrl.SetString("CFBundleURLIconFile", "LaunchScreen-iPhoneLandscape");
        PlistElementArray zhiFuUrlScheme = zhifuUrl.CreateArray("CFBundleURLSchemes");
        zhiFuUrlScheme.AddString("zhifu002cf6e637ffbe33");

        

        plist.WriteToFile(plistPath);




        //插入代码

        //读取UnityAppController.h文件
        string unityAppControllerHeaderPath = pathToBuiltProject + "/Classes/UnityAppController.h";
        XClass UnityAppControllerHeader = new XClass(unityAppControllerHeaderPath);

        UnityAppControllerHeader.Replace("@interface UnityAppController : NSObject<UIApplicationDelegate>", "@interface UnityAppController : NSObject<UIApplicationDelegate,WXApiDelegate>");

        StringBuilder sb = new StringBuilder();
        sb.Append("\n#import \"WXApi.h\"\n");

        //在指定代码后面增加一行代码
        UnityAppControllerHeader.WriteBelow("#import <QuartzCore/CADisplayLink.h>", sb.ToString());

        
        //读取UnityAppController.mm文件
        string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        XClass UnityAppController = new XClass(unityAppControllerPath);

        sb = new StringBuilder();
        sb.Append("\n#import \"WXSdk.h\"\n");
        sb.Append("\n#import \"AliPayPlatform.h\"\n");
        sb.Append("\n#import <AlipaySDK/AlipaySDK.h>\n");
        //在指定代码后面增加一行代码
        UnityAppController.WriteBelow("#import \"iPhone_Sensors.h\"", sb.ToString());
        
        sb = new StringBuilder();
        sb.Append("\n\n");
        sb.Append("- (void) onReq:(BaseReq *)req{}\n");
        sb.Append("- (void) onResp:(BaseResp *)resp\n");
        sb.Append("{\n");
        sb.Append("    [WXSdk onResp:resp];\n");
        sb.Append("}\n\n");

        sb.Append("-(BOOL)application:(UIApplication*)application handleOpenURL: (NSURL*)url\n");
        sb.Append("{\n");
        sb.Append("    return [WXApi handleOpenURL: url delegate:self];\n");
        sb.Append("}\n");
        
        UnityAppController.WriteBelow(" SensorsCleanup();\n}", sb.ToString());
        
        sb = new StringBuilder();
        sb.Append("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n");
        appendXcodeZhiFuBao(sb);
        //微信支付
        sb.Append("  if ([url.host isEqualToString:@\"pay\"]) {\n");
        sb.Append("      return [WXApi handleOpenURL:url delegate:self];\n");
        sb.Append("  }\n");
        sb.Append("    return [WXApi handleOpenURL: url delegate:self];\n");
        
        UnityAppController.Replace("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);\n    return YES;", sb.ToString());
        appendXcodeStatusBar(pathToBuiltProject);
    }

    private static void appendXcodeStatusBar(string pathToBuiltProject){
        string unityAppControllerHeaderPath = pathToBuiltProject + "/Classes/UI/UnityViewControllerBaseiOS.h";
        XClass UnityAppControllerHeader = new XClass(unityAppControllerHeaderPath);
        StringBuilder sb = new StringBuilder(); //显示隐藏
        sb.Append("\n  -(void)setPrefersStatusBarHidden:(BOOL)isShow;\n");

        //添加颜色样式
        sb.Append("\n  -(void)setStatusBarStyleValue:(BOOL)isWhite;\n");


        //在指定代码后面增加一行代码
        UnityAppControllerHeader.WriteBelow("- (BOOL)prefersStatusBarHidden;\n", sb.ToString());

        string UnityViewControllerBaseiOS = pathToBuiltProject + "/Classes/UI/UnityViewControllerBaseiOS.mm";
        XClass UnityAppController = new XClass(UnityViewControllerBaseiOS);

        sb = new StringBuilder(); //显示隐藏
        sb.Append(" return prefersStatusBarHiddenValue;");
        UnityAppController.Replace("return _PrefersStatusBarHidden;", sb.ToString());

        sb = new StringBuilder(); //显示隐藏
        sb.Append("prefersStatusBarHiddenValue = _PrefersStatusBarHidden;\n");
        UnityAppController.WriteBelow(" _PrefersStatusBarHiddenInited = true;\n", sb.ToString());

        sb = new StringBuilder(); //显示隐藏
        sb.Append(" static bool prefersStatusBarHiddenValue = true;\n");
        sb.Append(" static bool myStatusBarStyleValue = false;\n");
        sb.Append("- (void) setPrefersStatusBarHidden:(BOOL)isShow\n");
        sb.Append("{\n");
        sb.Append("    prefersStatusBarHiddenValue = isShow;\n");
        sb.Append("}\n");
        UnityAppController.WriteBelow("@implementation UnityViewControllerBase\n", sb.ToString());


        sb = new StringBuilder(); //添加颜色样式
        sb.Append(" -(void)setStatusBarStyleValue:(BOOL)isWhite\n");
        sb.Append("{\n");
        sb.Append("    myStatusBarStyleValue = isWhite;\n");
        sb.Append("}\n");
        UnityAppController.WriteBelow("@implementation UnityViewControllerBase\n", sb.ToString());
        sb = new StringBuilder();  //添加颜色样式
        sb.Append("if (myStatusBarStyleValue)\n");
        sb.Append("{\n");
        sb.Append("    _PreferredStatusBarStyle = UIStatusBarStyleLightContent;\n");
        sb.Append("}\n");
        sb.Append("else\n");
        sb.Append("{\n");
        sb.Append("     _PreferredStatusBarStyle = UIStatusBarStyleDefault;\n");
        sb.Append("}\n");
        sb.Append("     return _PreferredStatusBarStyle;\n");
        UnityAppController.Replace(" return _PreferredStatusBarStyle;", sb.ToString());

    }
    private static void appendXcodeZhiFuBao(StringBuilder sb)
    {
        sb.Append("    if ([url.host isEqualToString:@\"safepay\"]) { \n");
        sb.Append("        [[AlipaySDK defaultService] processOrderWithPaymentResult:url standbyCallback:^(NSDictionary *resultDic) {\n");
        sb.Append("            NSString *resultStatus =resultDic[@\"resultStatus\"];\n");
        sb.Append("            UnitySendMessage(\"[Login]\", \"OnAliPay\", [resultStatus UTF8String]);\n");
        sb.Append("        }];\n");
        sb.Append("\n");
        sb.Append("        [[AlipaySDK defaultService] processAuth_V2Result:url standbyCallback:^(NSDictionary *resultDic) {\n");
        sb.Append("            NSString *resultStatus = resultDic[@\"resultStatus\"];\n");
        sb.Append("            if ([resultStatus isEqualToString:@\"9000\"]) {\n");
        sb.Append("                NSString *result = resultDic[@\"result\"];\n");
        sb.Append("                UnitySendMessage(\"[Login]\", \"OnAlipayAuthSucceed\", [result UTF8String]);\n");
        sb.Append("            } else {\n");
        sb.Append("                UnitySendMessage(\"[Login]\", \"OnAlipayAuthFail\", [resultStatus UTF8String]);\n");
        sb.Append("            }\n");
        sb.Append("        }];\n");
        sb.Append("        return YES;\n");
        sb.Append("    }\n");
    }
}