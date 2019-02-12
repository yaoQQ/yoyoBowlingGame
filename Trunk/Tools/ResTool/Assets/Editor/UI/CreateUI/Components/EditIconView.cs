using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

public class EditIconView : BaseEditView
{
    Vector2 scrollpostion = Vector2.zero;

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
       
        IconWidget iconWidget = widget as IconWidget;
        DrawCommon(window, widget.gameObject, widget);

        IconWidget.IconType oldIconType = iconWidget.iconType;
        iconWidget.iconType =(IconWidget.IconType) EditorGUILayout.EnumPopup("图标类型：",iconWidget.iconType);

        if (iconWidget.iconType!= oldIconType)
        {
            //类型发生改变
            iconWidget.IconArr = null;
            iconWidget.mcArr = null;
        }

        switch (iconWidget.iconType)
        {
            case IconWidget.IconType.Sprite:
                RenderSpriteType(iconWidget);
                break;
            case IconWidget.IconType.MovicClip:
                RenderMovicClipType(iconWidget);
                break;
        }


    }


    void RenderSpriteType(IconWidget iconWidget)
    {
        int i;
        int curIconNum = 0;
        if (iconWidget.IconArr != null)
        {
            curIconNum = iconWidget.IconArr.Length;
        }
        int oldIconNum = curIconNum;

        curIconNum = EditorGUILayout.DelayedIntField("icon数目 ：", curIconNum, GUILayout.ExpandWidth(true));

        iconWidget.useImgSize = EditorGUILayout.Toggle("使用原图大小： ", iconWidget.useImgSize, GUILayout.ExpandWidth(true));

        if (iconWidget.useImgSize)
        {
            if (iconWidget.IconArr != null && iconWidget.IconArr.Length > 0)
            {
                Sprite sprite = iconWidget.IconArr[iconWidget.initIndex];
                if (sprite != null)
                {
                    float x = sprite.rect.width;
                    float y = sprite.rect.height;
                    setImgSize(iconWidget.gameObject, x, y);
                }

            }
        }

        if (curIconNum != oldIconNum)
        {
            if (curIconNum == 0)
            {
                iconWidget.IconArr = null;
            }
            else
            {
                int minLen = 0;
                if (iconWidget.IconArr != null && iconWidget.IconArr.Length != 0)
                {
                    minLen = Mathf.Min(curIconNum, iconWidget.IconArr.Length);
                }
                Sprite[] tempArr = new Sprite[curIconNum];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = iconWidget.IconArr[i];
                }
                iconWidget.IconArr = tempArr;
            }
        }

        if (iconWidget.IconArr != null && iconWidget.IconArr.Length > 0)
        {
            scrollpostion = EditorGUILayout.BeginScrollView(scrollpostion, GUILayout.Width(400), GUILayout.Height(200));
            for (i = 0; i < iconWidget.IconArr.Length; i++)
            {
                iconWidget.IconArr[i] = EditorGUILayout.ObjectField(i + "：",
                    iconWidget.IconArr[i], typeof(Sprite), true, GUILayout.ExpandWidth(true)
                ) as Sprite;
               
            }
            EditorGUILayout.EndScrollView();

           
        }


        if (GUILayout.Button("自动填充"))
        {
            if (iconWidget.IconArr != null && iconWidget.IconArr.Length >= 1)
            {
                List<Sprite> allSpriteList = new List<Sprite>();
                Sprite[] spriteArr = Selection.GetFiltered<Sprite>( SelectionMode.Deep);
                Texture2D[] textureArr = Selection.GetFiltered<Texture2D>(SelectionMode.TopLevel);
                for (i= 0;i< spriteArr.Length;i++)
                {
                    allSpriteList.Add(spriteArr[i]);
                }
                for (i = 0; i < textureArr.Length; i++  )
                {
                    string str = AssetDatabase.GetAssetPath(textureArr[i]);
                    UnityEngine.Object[] obs = AssetDatabase.LoadAllAssetsAtPath(str);
                    for (int j = 0; j < obs.Length; j++   )
                    {
                        Sprite tempSP = obs[j] as Sprite;
                        if(tempSP!=null)
                        {
                            allSpriteList.Add(tempSP);
                        }
                    }
                }

                if (allSpriteList.Count>1)
                {
                    allSpriteList.Sort((a,b)=> {
                        return string.CompareOrdinal(a.name, b.name);
                    });
                    int len = Mathf.Min(iconWidget.IconArr.Length, allSpriteList.Count);
                    for (i = 0; i < len; i++)
                    {
                        iconWidget.IconArr[i] = allSpriteList[i];
                    }
                }
               
                else if(iconWidget.IconArr[0]!=null)
                {
                    UnityEngine.Object[] oArr = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(iconWidget.IconArr[0]));
                    Sprite[] sprites = Array.ConvertAll<UnityEngine.Object, Sprite>(oArr, s => s as Sprite);
                    if (sprites.Length > 2)
                    {
                        for (i = 1; i < sprites.Length; i++)
                        {
                            Sprite sp = sprites[i];

                            if (sp.name == iconWidget.IconArr[0].name)
                            {
                                int len = Mathf.Min(sprites.Length, i + iconWidget.IconArr.Length);
                                for (int j = i; j < len; j++)
                                {
                                    Sprite autoSP = sprites[j];
                                    iconWidget.IconArr[j - i] = autoSP;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        string tempPath = AssetDatabase.GetAssetPath(iconWidget.IconArr[0]);

                        tempPath = Path.GetDirectoryName(tempPath);
                        //Debug.LogError(tempPath);

                        DirectoryInfo direction = new DirectoryInfo(tempPath);
                        FileInfo[] files = direction.GetFiles("*.png", SearchOption.TopDirectoryOnly);
                        List<Sprite> spArr = new List<Sprite>();
                        List<string> spPathArr = new List<string>();
                        for (i = 0; i < files.Length; i++)
                        {
                            FileInfo f = files[i];
                            //Debug.LogError(Application.dataPath.Replace("/","\\"));
                            string pngPath = f.FullName.Replace(Application.dataPath.Replace("/", "\\"), "");
                            //Debug.LogError("Assets"+pngPath);
                            spPathArr.Add("Assets" + pngPath);
                        }
                        //没用。先加个排序流程
                        spPathArr.Sort();

                        for (i = 0; i < spPathArr.Count; i++)
                        {
                            string spPath = spPathArr[i];

                            Sprite png = AssetDatabase.LoadAssetAtPath<Sprite>(spPath);
                            if (png != null)
                            {
                                spArr.Add(png);
                            }
                        }

                        for (i = 0; i < spArr.Count; i++)
                        {
                            Sprite sp = spArr[i];
                            if (sp.name == iconWidget.IconArr[0].name)
                            {
                                int len = Mathf.Min(spArr.Count, i + iconWidget.IconArr.Length);
                                for (int j = i; j < len; j++)
                                {
                                    Sprite autoSP = spArr[j];
                                    iconWidget.IconArr[j - i] = autoSP;
                                }
                                break;
                            }
                        }
                    }
                }
            }
 
        }
        if (GUILayout.Button("清空图片"))
        {
            if(iconWidget.IconArr!=null&& iconWidget.IconArr.Length>0)
            {
                for (i = 0; i < iconWidget.IconArr.Length; i++)
                {
                    iconWidget.IconArr[i] = null;
                }
            }
           
        }

        if (iconWidget.IconArr != null && iconWidget.IconArr.Length > 0)
        {
            iconWidget.initIndex = EditorGUILayout.IntSlider("初始化显示的索引图标", iconWidget.initIndex, 0, iconWidget.IconArr.Length - 1, GUILayout.ExpandWidth(true));
            iconWidget.Img.sprite = iconWidget.IconArr[iconWidget.initIndex];
        }
    }



    void RenderMovicClipType(IconWidget iconWidget)
    {
        int i;
        int curMcNum = 0;
        if (iconWidget.IconArr != null)
        {
            curMcNum = iconWidget.mcArr.Length;
        }
        int oldMcNum = curMcNum;

        curMcNum = EditorGUILayout.DelayedIntField("mc数目 ：", curMcNum, GUILayout.ExpandWidth(true));

        if (curMcNum != oldMcNum)
        {
            if (curMcNum == 0)
            {
                iconWidget.mcArr = null;
            }
            else
            {
                int minLen = 0;
                if (iconWidget.mcArr != null && iconWidget.mcArr.Length != 0)
                {
                    minLen = Mathf.Min(curMcNum, iconWidget.mcArr.Length);
                }
                MovieClip[] tempArr = new MovieClip[curMcNum];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = iconWidget.mcArr[i];
                }
                iconWidget.mcArr = tempArr;
            }
        }

        if (iconWidget.mcArr != null && iconWidget.mcArr.Length > 0)
        {
            scrollpostion = EditorGUILayout.BeginScrollView(scrollpostion, GUILayout.Width(400), GUILayout.Height(200));
            for (i = 0; i < iconWidget.mcArr.Length; i++)
            {
                iconWidget.mcArr[i] = EditorGUILayout.ObjectField(i + "：",
                    iconWidget.mcArr[i], typeof(MovieClip), true, GUILayout.ExpandWidth(true)
                ) as MovieClip;
                if(iconWidget.mcArr[i]!=null&& iconWidget.mcArr[i].frameSprites !=null&& iconWidget.mcArr[i].frameSprites.Length>0)
                {
                    EditorGUILayout.ObjectField( "", iconWidget.mcArr[i].frameSprites[0], typeof(Sprite), true, GUILayout.ExpandWidth(true));
                }
            }
            EditorGUILayout.EndScrollView();


        }
        if (GUILayout.Button("自动填充"))
        {
            if (iconWidget.mcArr != null && iconWidget.mcArr.Length >= 1)
            {
                MovieClip[] tempMCArr = Selection.GetFiltered<MovieClip>(SelectionMode.TopLevel);
                List<MovieClip> allMCList = new List<MovieClip>(tempMCArr);

                if (allMCList.Count > 1)
                {
                    allMCList.Sort((a, b) => {
                        return string.CompareOrdinal(a.name, b.name);
                    });
                    int len = Mathf.Min(iconWidget.mcArr.Length, allMCList.Count);
                    for (i = 0; i < len; i++)
                    {
                        iconWidget.mcArr[i] = allMCList[i];
                    }
                }
                else if (iconWidget.mcArr[0] != null)
                {
                    UnityEngine.Object[] oArr = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(iconWidget.mcArr[0]));

                    MovieClip[] mcs = Array.ConvertAll<UnityEngine.Object, MovieClip>(oArr, s => s as MovieClip);
                    List<MovieClip> mcList = new List<MovieClip>();
                    foreach(MovieClip mc in mcs)
                    {
                        if(mc!=null)
                        {
                            mcList.Add(mc);
                        }
                    }
                    mcList.Sort((a, b) => {
                        return string.CompareOrdinal(a.name, b.name);
                    });

                    if (mcList.Count > 2)
                    {
                        for (i = 1; i < mcList.Count; i++)
                        {
                            MovieClip mc = mcList[i];

                            if (mc.name == iconWidget.mcArr[0].name)
                            {
                                int len = Mathf.Min(mcList.Count, i + iconWidget.mcArr.Length);
                                for (int j = i; j < len; j++)
                                {
                                    MovieClip autoMC = mcList[j];
                                    iconWidget.mcArr[j - i] = autoMC;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        string tempPath = AssetDatabase.GetAssetPath(iconWidget.mcArr[0]);

                        tempPath = Path.GetDirectoryName(tempPath);
                        //Debug.LogError(tempPath);

                        DirectoryInfo direction = new DirectoryInfo(tempPath);
                        FileInfo[] files = direction.GetFiles("*.asset", SearchOption.TopDirectoryOnly);
                        List<MovieClip> mc_Arr = new List<MovieClip>();
                        List<string> mcPathArr = new List<string>();
                        for (i = 0; i < files.Length; i++)
                        {
                            FileInfo f = files[i];
                            //Debug.LogError(Application.dataPath.Replace("/","\\"));
                            string pngPath = f.FullName.Replace(Application.dataPath.Replace("/", "\\"), "");
                            //Debug.LogError("Assets"+pngPath);
                            mcPathArr.Add("Assets" + pngPath);
                        }
                        mcPathArr.Sort();

                        for (i = 0; i < mcPathArr.Count; i++)
                        {
                            string spPath = mcPathArr[i];

                            MovieClip png = AssetDatabase.LoadAssetAtPath<MovieClip>(spPath);
                            if (png != null)
                            {
                                mc_Arr.Add(png);
                            }
                        }

                        for (i = 0; i < mc_Arr.Count; i++)
                        {
                            MovieClip sp = mc_Arr[i];
                            if (sp.name == iconWidget.mcArr[0].name)
                            {
                                int len = Mathf.Min(mc_Arr.Count, i + iconWidget.mcArr.Length);
                                for (int j = i; j < len; j++)
                                {
                                    MovieClip autoSP = mc_Arr[j];
                                    iconWidget.mcArr[j - i] = autoSP;
                                }
                                break;
                            }
                        }
                    }
                }
            }

        }



        if (GUILayout.Button("清空MC"))
        {
            if (iconWidget.mcArr != null && iconWidget.mcArr.Length > 0)
            {
                for (i = 0; i < iconWidget.mcArr.Length; i++)
                {
                    iconWidget.mcArr[i] = null;
                }
            }

        }

        if (iconWidget.mcArr != null && iconWidget.mcArr.Length > 0)
        {
            iconWidget.initIndex = EditorGUILayout.IntSlider("初始化显示的索引影片", iconWidget.initIndex, 0, iconWidget.mcArr.Length - 1, GUILayout.ExpandWidth(true));
            if(iconWidget.mcArr!=null&& iconWidget.mcArr.Length>iconWidget.initIndex&& iconWidget.mcArr[iconWidget.initIndex]!=null && iconWidget.mcArr[iconWidget.initIndex] !=null&& iconWidget.mcArr[iconWidget.initIndex].frameSprites.Length>0)
            {
                iconWidget.Img.sprite = iconWidget.mcArr[iconWidget.initIndex].frameSprites[0];
            }
            else
            {
                iconWidget.Img.sprite = null;
            }
            
        }
    }
}
