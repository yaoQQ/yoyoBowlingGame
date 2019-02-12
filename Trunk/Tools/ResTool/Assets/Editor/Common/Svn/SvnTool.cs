using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public static class SvnTool
{

    [MenuItem("SVN/Commit", false, 1)]
    [MenuItem("Assets/SVN Commit", false, 1)]
    static void SVNCommit()
    {
        List<string> pathList = new List<string>();
        string basePath = SVNProjectPath + "/Assets";
        pathList.Add(basePath);
        //pathList.Add(SVNProjectPath + "/ProjectSettings");

        string commitPath = string.Join("*", pathList.ToArray());
        ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + commitPath);
    }

    [MenuItem("SVN/Proejct Commit", false, 1)]
    static void Proejct_SVNCommit()
    {
        List<string> pathList = new List<string>();
        string basePath = SVNProjectPath + "/Assets/Project/" + ProjectUtil.GetCurProjectName();
        pathList.Add(basePath);
        //pathList.Add(SVNProjectPath + "/ProjectSettings");

        string commitPath = string.Join("*", pathList.ToArray());
        ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + commitPath);
    }

    [MenuItem("SVN/Update", false, 2)]
    [MenuItem("Assets/SVN Update", false, 2)]
    static void SVNUpdate()
    {
        List<string> pathList = new List<string>();
        string basePath = SVNProjectPath + "/Assets";
        pathList.Add(basePath);
        //pathList.Add(SVNProjectPath + "/ProjectSettings");

        string updatePath = string.Join("*", pathList.ToArray());

        ProcessCommand("TortoiseProc.exe", "/command:update /path:" + updatePath + " /closeonend:0");
    }

    [MenuItem("SVN/Proejct Update", false, 2)]
    static void Proejct_SVNUpdate()
    {
        List<string> pathList = new List<string>();
        string basePath = SVNProjectPath + "/Assets/Project/" + ProjectUtil.GetCurProjectName();
        pathList.Add(basePath);

        string updatePath = string.Join("*", pathList.ToArray());

        ProcessCommand("TortoiseProc.exe", "/command:update /path:" + updatePath + " /closeonend:0");
    }



    [MenuItem("SVN/CleanUp", false, 4)]
    [MenuItem("Assets/SVN CleanUp", false, 4)]
    static void SVNCleanUp()
    {
        ProcessCommand("TortoiseProc.exe", "/command:cleanup /path:" + SVNProjectPath);
    }

    [MenuItem("SVN/Log", false, 5)]
    static void SVNLog()
    {
        ProcessCommand("TortoiseProc.exe", "/command:log /path:" + SVNProjectPath);
    }


    [MenuItem("SVN/Proejct Log", false, 5)]
    static void Proejct_SVNLog()
    {
        ProcessCommand("TortoiseProc.exe", "/command:log /path:" + SVNProjectPath + "/Assets/Project/" + ProjectUtil.GetCurProjectName());
    }



    static string SVNProjectPath
    {
        get
        {
            System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
            return parent.ToString();
        }
    }

    static void ProcessCommand(string command, string argument)
    {
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(command);
        info.Arguments = argument;
        info.CreateNoWindow = false;
        info.ErrorDialog = true;
        info.UseShellExecute = true;

        if (info.UseShellExecute)
        {
            info.RedirectStandardOutput = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
        }
        else
        {
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            info.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        }

        System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

        if (!info.UseShellExecute)
        {
            Debug.Log(process.StandardOutput);
            Debug.Log(process.StandardError);
        }

        process.WaitForExit();
        process.Close();
    }
}
