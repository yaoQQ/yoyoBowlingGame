using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectControler :ScriptableObject
{
    public int curProjectIndex;
    public List<string> projects=new List<string>();

    public string GetCurProjectName()
    {
        if(projects.Count>0)
        {
            return projects[curProjectIndex];
        }
        Debug.LogError("没有创建项目！！！！！！！！！！");
        return "";
    }
}
