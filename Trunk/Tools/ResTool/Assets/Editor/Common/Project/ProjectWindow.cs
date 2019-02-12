using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ProjectUtil
{
    public static string GetCurProjectName()
    {
        return ProjectWindow.GetProjectControler().GetCurProjectName();
    }
}


public class ProjectWindow : EditorWindow
{
    static void CreateProjectFile()
    {
        string createPath = "Assets/Resources/project_controler.asset";
        ProjectControler controler = CreateInstance<ProjectControler>();
        AssetDatabase.CreateAsset(controler, createPath);
    }

    public static ProjectControler GetProjectControler()
    {
        ProjectControler controler = Resources.Load<ProjectControler>("project_controler");
        if (controler == null)
        {
            CreateProjectFile();
            controler = Resources.Load<ProjectControler>("project_controler");
        }
        return controler;
    }

   

    static ProjectWindow target;

    [MenuItem("Project/Select Project")]
    public static void ShowWindow()
    {
        target = EditorWindow.GetWindow<ProjectWindow>(false, "select project");
        target.minSize = new Vector2(270f, 300f);
        target.maxSize = new Vector2(271, 301);
        DontDestroyOnLoad(target);
    }

    ProjectControler curProjectControler;
    string addProjectTxt = "";
    Vector2 scrollpostion = Vector2.zero;
    void OnGUI()
    {
        if(curProjectControler==null)
        {
            curProjectControler = GetProjectControler();
        }
        int oldIndex = curProjectControler.curProjectIndex;
        scrollpostion = EditorGUILayout.BeginScrollView(scrollpostion, GUILayout.Width(250), GUILayout.Height(250));

        for (int i=0;i< curProjectControler.projects.Count;i++)
        {
            GUILayout.BeginHorizontal();
            bool oldSign;
            if(i== curProjectControler.curProjectIndex)
            {
                oldSign = true;
            }
            else
            {
                oldSign = false;
            }
            bool newSign= EditorGUILayout.Toggle(curProjectControler.projects[i], oldSign);

            if(newSign)
            {
                curProjectControler.curProjectIndex = i;
                AssetDatabase.SaveAssets();
            }

            if (GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(60f) }))
            {
                curProjectControler.projects.RemoveAt(i);
                EditorUtility.SetDirty(curProjectControler);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (curProjectControler.curProjectIndex>=curProjectControler.projects.Count)
                {
                    curProjectControler.curProjectIndex = 0;
                   
                }
            }


            
            GUILayout.EndHorizontal();
        }


        EditorGUILayout.EndScrollView();


        if(oldIndex != curProjectControler.curProjectIndex)
        {
            EditorUtility.SetDirty(curProjectControler);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        GUILayout.BeginHorizontal();
        addProjectTxt=GUILayout.TextField(addProjectTxt, new GUILayoutOption[] { GUILayout.Width(100f) });
        if (GUILayout.Button("添加", new GUILayoutOption[] { GUILayout.Width(80f) }))
        {
            if(!string.IsNullOrEmpty(addProjectTxt)&&!curProjectControler.projects.Contains(addProjectTxt))
            {
                curProjectControler.projects.Add(addProjectTxt);
                EditorUtility.SetDirty(curProjectControler);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        GUILayout.EndHorizontal();

    }
}