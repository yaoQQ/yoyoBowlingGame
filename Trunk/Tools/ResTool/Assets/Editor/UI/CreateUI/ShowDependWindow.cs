using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class ShowDependWindow : EditorWindow
{
    int minDependNum = 1;
    int maxDependNum = 15;
    Vector2 scrollpostion01 = Vector2.zero;
    Vector2 scrollpostion02 = Vector2.zero;
    List<Image> imageList = new List<Image>();
    string[] options = { "Image", "Button", "Icon" };
    int index = 0;
    static ShowDependWindow dependWinow;
    [MenuItem("Tool/UI/ShowDepend %F", false, 20002)]
    public static void ShowWindow()
    {
        dependWinow = EditorWindow.GetWindow<ShowDependWindow>(false, "查看图集");
        dependWinow.minSize = new Vector2(500f, 700f);
        dependWinow.maxSize = new Vector2(800f, 1500f);
        dependWinow.wantsMouseMove = true;
        DontDestroyOnLoad(dependWinow);
    }
    public static void CloseWindow()
    {
        if (dependWinow != null) dependWinow.Close();
    }

    public void OnGUI()
    {
        bool selectedSign = false;
        GUILayout.BeginVertical();
        string selectedName = "";
        if (Selection.activeGameObject != null )
        {
            selectedName = Selection.activeGameObject.name;
            selectedSign = true;
        }

        if (!selectedSign)
        {
            this.ShowNotification(new GUIContent("要选中一个容器！"));
            return;
        }
        EditorGUILayout.LabelField("选中：" + selectedName, new GUILayoutOption[] { GUILayout.Width(200f) });
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择组件：", new GUILayoutOption[] { GUILayout.Width(100f) });
        index = EditorGUILayout.Popup(index, options, new GUILayoutOption[] { GUILayout.Width(100f) });
        GUILayout.EndHorizontal();
        switch (index)
        {
            case 0:
                showImageDependS();
                break;
            case 1:
                showButtonDependS();
                break;
            case 2:
                showIconDepends();
                break;
            default:
                break;
        }
        GUILayout.EndVertical();
    }


    void showImageDependS()
    {

        Image[] components = Selection.activeGameObject.GetComponentsInChildren<Image>(true);
        scrollpostion01 = EditorGUILayout.BeginScrollView(scrollpostion01, GUILayout.ExpandWidth(true), GUILayout.Height(600));
        List<Image> atlaArr = new List<Image>();
        int dependNum;
        foreach (var c in components)
        {
            if (c.sprite != null)
            {
                string[] tempArr = c.sprite.name.Split('@');
                int.TryParse(tempArr[0], out dependNum);
                if (dependNum <= maxDependNum && dependNum >= minDependNum)
                {
                    EditorGUILayout.LabelField("依赖图集  :  " + c.sprite.name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("图    片 :", c.sprite, typeof(Sprite), true, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("gemeObject ：", c, typeof(GameObject), false, GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", new GUILayoutOption[] { GUILayout.Width(400f) });
                    EditorGUILayout.Space();
                }
            }

        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        scrollpostion02 = EditorGUILayout.BeginScrollView(scrollpostion02, GUILayout.ExpandWidth(true), GUILayout.Height(400));
        ShowDepend(Selection.activeGameObject);
        EditorGUILayout.EndScrollView();
    }



    void ShowDepend(GameObject go)
    {
        int i;
        int dependNum;
        List<Sprite> results = new List<Sprite>();
        List<string> atlaArr = new List<string>();
        GameObject ob = (GameObject)PrefabUtility.GetPrefabParent(go);
        if (ob != null)
        {
            string[] pathArr = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(ob.GetInstanceID()));
            for (i = 0; i < pathArr.Length; i++)
            {
                //Debug.Log()
                string[] tempArr = pathArr[i].Split('/');
                string tempName = tempArr[tempArr.Length - 1];
                string[] pngArr = tempName.Split('@');

                if (pngArr.Length >= 2)
                {
                    if (!atlaArr.Contains(pngArr[0]))
                    {
                        int.TryParse(pngArr[0], out dependNum);
                        if (dependNum <= maxDependNum && dependNum >= minDependNum)
                        {
                            atlaArr.Add(tempName);
                        }
                    }
                }
            }
            for (i = 0; i < atlaArr.Count; i++)
            {
                EditorGUILayout.LabelField("依赖图集  :  " + atlaArr[i], GUILayout.ExpandWidth(true));
            }
            if (atlaArr.Count ==0)
            {
                EditorGUILayout.LabelField("没有"+ minDependNum + "至"+ maxDependNum + "的依赖！！", GUILayout.ExpandWidth(true));
            }
        }
    }

    void showButtonDependS()
    {
        int HdependNum = -1;
        int PdependNum = -1;
        int DdependNum = -1;
        EditorGUILayout.LabelField("请查看Button组件中Transition-Sprite Swap 中是否存有旧图集图片。", new GUILayoutOption[] { GUILayout.Width(400f) });
        Button[] buttonComponents = Selection.activeGameObject.GetComponentsInChildren<Button>(true);
        scrollpostion01 = EditorGUILayout.BeginScrollView(scrollpostion01, GUILayout.ExpandWidth(true), GUILayout.Height(600));
        foreach (var buttonItem in buttonComponents)
        {
            if (buttonItem.spriteState.highlightedSprite != null || buttonItem.spriteState.pressedSprite != null || buttonItem.spriteState.disabledSprite != null)
            {
                if (buttonItem.spriteState.highlightedSprite != null)
                {
                    string[] highlightedSpriteSTRArr = buttonItem.spriteState.highlightedSprite.name.Split('@');
                    int.TryParse(highlightedSpriteSTRArr[0], out HdependNum);
                }
                if (buttonItem.spriteState.pressedSprite != null)
                {
                    string[] pressedSpriteSTRArr = buttonItem.spriteState.pressedSprite.name.Split('@');
                    int.TryParse(pressedSpriteSTRArr[0], out PdependNum);
                }
                if (buttonItem.spriteState.disabledSprite != null)
                {
                    string[] disabledSpriteSTRArr = buttonItem.spriteState.disabledSprite.name.Split('@');
                    int.TryParse(disabledSpriteSTRArr[0], out DdependNum);
                }
                if ((HdependNum <= maxDependNum && HdependNum >= minDependNum)|| (PdependNum <=maxDependNum && PdependNum >=minDependNum) || (DdependNum <=maxDependNum && DdependNum >=minDependNum))
                {
                    EditorGUILayout.ObjectField("Button ：", buttonItem, typeof(Button), false, GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("依赖图集  :  " + buttonItem.spriteState.highlightedSprite.name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("highlightedSprite :", buttonItem.spriteState.highlightedSprite, typeof(Sprite), false, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("依赖图集  :  " + buttonItem.spriteState.pressedSprite.name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("pressedSprite :", buttonItem.spriteState.pressedSprite, typeof(Sprite), false, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("依赖图集  :  " + buttonItem.spriteState.pressedSprite.name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("disabledSprite :", buttonItem.spriteState.disabledSprite, typeof(Sprite), false, GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", new GUILayoutOption[] { GUILayout.Width(400f) });
                }
            }
        }
        EditorGUILayout.EndScrollView();
        scrollpostion02 = EditorGUILayout.BeginScrollView(scrollpostion02, GUILayout.ExpandWidth(true), GUILayout.Height(400));
        ShowDepend(Selection.activeGameObject);
        EditorGUILayout.EndScrollView();
    }


    void showIconDepends()
    {
        int iconDependNum;
        EditorGUILayout.LabelField("请查看IconWidget组件中是否存有旧图集图片。", new GUILayoutOption[] { GUILayout.Width(400f) });
        IconWidget[] iconWidgetComponents = Selection.activeGameObject.GetComponentsInChildren<IconWidget>(true);
        scrollpostion01 = EditorGUILayout.BeginScrollView(scrollpostion01, GUILayout.ExpandWidth(true), GUILayout.Height(600));
        foreach (var iconWidgetItem in iconWidgetComponents)
        {
            bool showGameObject = false;
            foreach (var iconItem in iconWidgetItem.IconArr)
            {
                string[] highlightedSpriteSTRArr = iconItem.name.Split('@');
                int.TryParse(highlightedSpriteSTRArr[0], out iconDependNum);
                if ((iconDependNum <=maxDependNum && iconDependNum >=minDependNum))
                {
                    showGameObject = true;
                    if (!showGameObject)
                    {
                        EditorGUILayout.ObjectField("gemeObject ：", iconWidgetItem, typeof(GameObject), false, GUILayout.ExpandWidth(true));
                    }
                    EditorGUILayout.LabelField("依赖图集  :  " + iconItem.name, GUILayout.ExpandWidth(true));
                    EditorGUILayout.ObjectField("pressedSprite :", iconItem, typeof(Sprite), false, GUILayout.ExpandWidth(true));
                }
            }
            if (showGameObject)
            {
               EditorGUILayout.LabelField("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~", new GUILayoutOption[] { GUILayout.Width(400f) });
            }
        }
        EditorGUILayout.EndScrollView();
        scrollpostion02 = EditorGUILayout.BeginScrollView(scrollpostion02, GUILayout.ExpandWidth(true), GUILayout.Height(400));
        ShowDepend(Selection.activeGameObject);
        EditorGUILayout.EndScrollView();
    }

    void OnFocus()
    {
        


    }



}
