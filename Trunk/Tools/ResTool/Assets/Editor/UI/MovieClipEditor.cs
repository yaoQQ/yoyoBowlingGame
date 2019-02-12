using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MovieClip))]
public class MovieClipEditor : Editor
{

    [MenuItem("Assets/Create/Movie Clip", false, 1)]
    public static void CreateIcon()
    {


        string path = "";
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);
        if(arr.Length>=1)
        {
            UnityEngine.Object o = arr[0];
            if(o.GetType()== typeof(UnityEditor.DefaultAsset))
            {
                path = AssetDatabase.GetAssetPath(o);
            }
            else
            {
                path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(o));
            }
            MovieClip icon = ScriptableObject.CreateInstance<MovieClip>();
            string iconName = "New Movie Clip";
            string createPath = "";
            int num = 0;
            do
            {
                if(num==0)
                {
                    createPath = path + "/" + iconName + ".asset";
                }
                else
                {
                    createPath = path + "/" + iconName + " " + num + ".asset";
                }
                
                num++;
            }
            while (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(createPath) != null);
           
            AssetDatabase.CreateAsset(icon, createPath);
        }




    }

    public override void OnInspectorGUI()
    {
        bool forceSave = false;
        int i;
        MovieClip icon = target as MovieClip;

        int oldSpaceTime = icon.spaceTime;
        Sprite[] oldFrameSprites = icon.frameSprites;


        icon.spaceTime = EditorGUILayout.DelayedIntField("帧动画时间间隔（MS）：",icon.spaceTime);
        icon.loopNum= EditorGUILayout.DelayedIntField("循环播放次数（-1为永远播放）：", icon.loopNum);
        if(icon.loopNum!=-1)
        {
            icon.loopNum = Mathf.Max(1, icon.loopNum);
        }
        int curIconNum = 0;
        if (icon.frameSprites != null)
        {
            curIconNum = icon.frameSprites.Length;
        }
        int oldIconNum = curIconNum;

        curIconNum = EditorGUILayout.DelayedIntField("icon数目 ：", curIconNum, GUILayout.ExpandWidth(true));


        if (curIconNum != oldIconNum)
        {
            if (curIconNum == 0)
            {
                icon.frameSprites = null;
            }
            else
            {
                int minLen = 0;
                if (icon.frameSprites != null && icon.frameSprites.Length != 0)
                {
                    minLen = Mathf.Min(curIconNum, icon.frameSprites.Length);
                }
                Sprite[] tempArr = new Sprite[curIconNum];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = icon.frameSprites[i];
                }
                icon.frameSprites = tempArr;
            }
        }

        if (icon.frameSprites != null && icon.frameSprites.Length > 0)
        {

            for (i = 0; i < icon.frameSprites.Length; i++)
            {
                Sprite oldSP = icon.frameSprites[i];
                icon.frameSprites[i] = EditorGUILayout.ObjectField(i + "：",
                    icon.frameSprites[i], typeof(Sprite), true, GUILayout.ExpandWidth(true)
                ) as Sprite;
                if(oldSP!= icon.frameSprites[i])
                {
                    forceSave = true;
                }
            }
        }

        if(GUILayout.Button("自动填充"))
        {
            if(icon.frameSprites!=null&& icon.frameSprites.Length>1)
            {
                UnityEngine.Object[] oArr= AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(icon.frameSprites[0]));
                Sprite[] sprites= Array.ConvertAll<UnityEngine.Object, Sprite>(oArr, s => s as Sprite);
                
                for(i=1;i< sprites.Length;i++)
                {
                    Sprite sp = sprites[i];
                   
                    if(sp.name== icon.frameSprites[0].name)
                    {
                        int len = Mathf.Min(sprites.Length, i+ icon.frameSprites.Length);
                        for(int j=i;j< len;j++)
                        {
                            Sprite autoSP = sprites[j];
                            icon.frameSprites[j - i] = autoSP;
                        }
                        break;
                    }
                }
                forceSave = true;
            }
            else
            {

            }
        }
        if (GUILayout.Button("清空图片"))
        {
            if (icon.frameSprites != null && icon.frameSprites.Length > 0)
            {
                for (i = 0; i < icon.frameSprites.Length; i++)
                {
                    icon.frameSprites[i] = null;
                }
            }

        }
        if(icon.frameSprites!=null)
        {
            if (oldSpaceTime != icon.spaceTime || !oldFrameSprites.Equals(icon.frameSprites) || forceSave)
            {
                EditorUtility.SetDirty(icon);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
       

    }
}
