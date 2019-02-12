using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class CreateTotalWindow : EditorWindow
{
    static CreateTotalWindow target;
    [MenuItem("Tool/UI/Create UI Window %Q", false, 20002)]
    public static void ShowWindow()
    {
        target = EditorWindow.GetWindow<CreateTotalWindow>(false, "Create UI");
        target.minSize = new Vector2(300f, 700f);
        target.maxSize = new Vector2(301f, 700f);

        target.wantsMouseMove = true;

        UnityEngine.Object.DontDestroyOnLoad(target);
    }
    public static void CloseWindow()
    {
        if (target != null) target.Close();
    }


    WidgetType curWidgetType;
    public void OnGUI()
    {
        curWidgetType = WidgetType.None;
        bool selectedSign = false;
        GUILayout.BeginVertical();
        string selectedName = "";
        if (Selection.activeGameObject != null && Selection.activeGameObject.activeInHierarchy)
        {
            selectedName = Selection.activeGameObject.name;
            selectedSign = true;
        }

        GUILayout.BeginHorizontal();
        if (!selectedSign)
        {
            this.ShowNotification(new GUIContent("要选中一个容器！"));
            return;
        }

        EditorGUILayout.LabelField("选中：" + selectedName, new GUILayoutOption[] { GUILayout.Width(200f) });
        UIBaseWidget widget = Selection.activeGameObject.GetComponent<UIBaseWidget>();
        if (widget != null)
        {

            if (GUILayout.Button("编辑", new GUILayoutOption[] { GUILayout.Width(42f), GUILayout.ExpandWidth(true) }))
            {
                EditWidgetWindow.ShowWindow(widget, widget.GetWidgetType());
            }
        }
        Transform parentTF = Selection.activeGameObject.transform.parent;

        string pathStr = "Assets/Project/" + ProjectUtil.GetCurProjectName() + "/ui/Exp_prefab/" + Selection.activeGameObject.name + ".prefab";


        if (widget != null && parentTF != null && parentTF.GetComponent<UIBaseWidget>() == null)
        {
            Selection.activeGameObject.name = Selection.activeGameObject.name.ToLower();
            if (GUILayout.Button("提交", new GUILayoutOption[] { GUILayout.Width(42f), GUILayout.ExpandWidth(true) }))
            {
                string[] folderArr = AssetDatabase.GetSubFolders("Assets/Project/" + ProjectUtil.GetCurProjectName() + "/ui/Exp_prefab");
                for (int i = 0; i < folderArr.Length; i++)
                {
                    string oldPath = folderArr[i] + "/" + Selection.activeGameObject.name + ".prefab";
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(oldPath);
                    if (go != null)
                    {
                        pathStr = oldPath;
                        break;
                    }
                }
                PrefabUtility.CreatePrefab(pathStr, Selection.activeGameObject, ReplacePrefabOptions.ConnectToPrefab);
                AssetDatabase.SaveAssets();
            }

        }


        GUILayout.EndHorizontal();

        this.RemoveNotification();
        AddCreateItem("面板Panel", WidgetType.Panel);
        AddCreateItem("按钮Button", WidgetType.Button);
        AddCreateItem("图片Image", WidgetType.Image);
        AddCreateItem("空Image用于接收点击", WidgetType.EmptyImage);
        AddCreateItem("圆形图片CircleImage", WidgetType.CircleImage);
        AddCreateItem("文本Text", WidgetType.Text);
        AddCreateItem("图标Icon", WidgetType.Icon);
        AddCreateItem("特效Effect", WidgetType.Effect);
        AddCreateItem("滚动面板ScrollPanel", WidgetType.ScrollPanel);
        AddCreateItem("单元复用滚动面板 CellRecycleScroll", WidgetType.CellRecycleScroll);
        AddCreateItem("复用单元CellItem", WidgetType.CellItem);
        AddCreateItem("单元组ItemGroup ", WidgetType.CellGroup);
        AddCreateItem("输入文本InputField", WidgetType.InputField);
        AddCreateItem("开关按钮Toggle", WidgetType.Toggle);
        AddCreateItem("遮罩Mask", WidgetType.Mask);
        AddCreateItem("滑动器Slider", WidgetType.Slider);
        AddCreateItem("分页面板 TabPanel", WidgetType.TabPanel);
        AddCreateItem("超文本 TextPic", WidgetType.TextPic);
        AddCreateItem("投影图像 RawImage", WidgetType.RawImage);
        AddCreateItem("骨骼动画 Spine", WidgetType.Spine);
        AddCreateItem("格子复用单元滚动面板 GridRecycleScroll", WidgetType.GridRecycleScroll);
        AddCreateItem("跑马灯文本 Marquee", WidgetType.Marquee);
        AddCreateItem("动态Grid/Table组件_横向", WidgetType.HorizontalLayout);
        AddCreateItem("动态Grid/Table组件_纵向", WidgetType.VerticalLayout);
        AddCreateItem("动态Grid/Table组件_双向", WidgetType.GridLayout);
        AddCreateItem("滚动面板ScrollPanel+Btn", WidgetType.ScrollPanelWithBt);
        AddCreateItem("下拉列表 Dropdown", WidgetType.Dropdown);
        AddCreateItem("横幅 Banner", WidgetType.Banner);
        AddCreateItem("UI动画 Animator", WidgetType.Animator);
        AddCreateItem("UI动画 Animation", WidgetType.Animation);

        OpenCreateWindow(curWidgetType);


        //显示依赖

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (widget != null && parentTF != null && parentTF.GetComponent<UIBaseWidget>() == null)
        {
            ShowDepend(Selection.activeGameObject);
            //Image[] components = Selection.activeGameObject.GetComponentsInChildren<Image>();

            //foreach (var c in components)
            //{

            //    if (c.sprite==null)
            //    {
            //        Debug.LogError(c.name);
            //    }

            //}
        }



        GUILayout.EndVertical();
    }
    void ShowDepend(GameObject go)
    {
        int i;
        List<string> atlaArr = new List<string>();
        GameObject ob = (GameObject)PrefabUtility.GetPrefabParent(go);
        if (ob != null)
        {
            string[] pathArr = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(ob.GetInstanceID()));

            for (i = 0; i < pathArr.Length; i++)
            {

                string[] tempArr = pathArr[i].Split('/');
                string tempName = tempArr[tempArr.Length - 1];
                string[] pngArr = tempName.Split('@');
                if (pngArr.Length >= 2)
                {
                    if (!atlaArr.Contains(pngArr[0]))
                    {
                        atlaArr.Add(pngArr[0]);
                        //if (int.Parse(pngArr[0]) < 16)
                        //{
                        //    Debug.LogError("依赖图集  " + pngArr[0] + "的go ：" + tempName);
                        //}
                    }
                }
            }
            for (i = 0; i < atlaArr.Count; i++)
            {
                EditorGUILayout.LabelField("依赖图集  :  " + atlaArr[i], GUILayout.ExpandWidth(true));
            }
        }
        //Component


    }

    void OnInspectorUpdate()
    {

        this.Repaint();
    }

    void AddCreateItem(string btnName, WidgetType widgetType)
    {
        if (GUILayout.Button(btnName, GUILayout.ExpandWidth(true)))
        {
            curWidgetType = widgetType;
        }
    }

    void OpenCreateWindow(WidgetType widgetType)
    {
        if (widgetType != WidgetType.None)
        {
            UIBaseWidget widget = CreateWidget(widgetType, Selection.activeGameObject);
            Selection.activeGameObject = widget.gameObject;
            EditWidgetWindow.ShowWindow(widget, widgetType);
        }
    }
    UIBaseWidget CreateWidget(WidgetType widgetType, GameObject parentGO)
    {
        UIBaseWidget widget = null;
        switch (widgetType)
        {
            case WidgetType.Panel:
                widget = UguiUtil.AddPanel(parentGO);
                break;
            case WidgetType.Button:
                widget = UguiUtil.AddButton(parentGO);
                break;
            case WidgetType.Text:
                widget = UguiUtil.AddText(parentGO);
                break;
            case WidgetType.Image:
                widget = UguiUtil.AddImage(parentGO);
                break;
            case WidgetType.Toggle:
                widget = UguiUtil.AddToggle(parentGO);
                break;
            case WidgetType.Effect:
                widget = UguiUtil.AddEffect(parentGO);
                break;
            case WidgetType.Icon:
                widget = UguiUtil.AddIcon(parentGO);
                break;
            case WidgetType.InputField:
                widget = UguiUtil.AddInputField(parentGO);
                break;
            case WidgetType.ScrollPanel:
                widget = UguiUtil.AddScrollPanel(parentGO);
                break;
            case WidgetType.CellRecycleScroll:
                widget = UguiUtil.AddCellRecycleScrollPanel(parentGO);
                break;
            case WidgetType.CellItem:
                widget = UguiUtil.AddCellItem(parentGO);
                break;
            case WidgetType.CellGroup:
                widget = UguiUtil.AddCellGroup(parentGO);
                break;
            case WidgetType.Slider:
                widget = UguiUtil.AddSlider(parentGO);
                break;
            case WidgetType.Mask:
                widget = UguiUtil.AddMask(parentGO);
                break;
            case WidgetType.TabPanel:
                widget = UguiUtil.AddTabPanel(parentGO);
                break;
            case WidgetType.TextPic:
                widget = UguiUtil.AddTextPic(parentGO);
                break;
            case WidgetType.RawImage:
                widget = UguiUtil.AddRawImage(parentGO);
                break;
            case WidgetType.Spine:
                widget = UguiUtil.AddSpine(parentGO);
                break;
            case WidgetType.GridRecycleScroll:
                widget = UguiUtil.AddGridRecycleScrollPanel(parentGO);
                break;
            case WidgetType.Marquee:
                widget = UguiUtil.AddMarquee(parentGO);
                break;
            case WidgetType.CircleImage:
                widget = UguiUtil.AddCircleImage(parentGO);
                break;
            case WidgetType.EmptyImage:
                widget = UguiUtil.AddEmptyImage(parentGO);
                break;
            case WidgetType.HorizontalLayout:
                widget = UguiUtil.AddHorizontalGroupWidget(parentGO);
                break;
            case WidgetType.VerticalLayout:
                widget = UguiUtil.AddVerticalLayoutGroupWidget(parentGO);
                break;
            case WidgetType.GridLayout:
                widget = UguiUtil.AddGridLayoutGroupWidget(parentGO);
                break;
            case WidgetType.ScrollPanelWithBt:
                widget = UguiUtil.AddScrollPanelWithBtnWidgeHorizontal(parentGO);
                break;
            case WidgetType.Banner:
                widget = UguiUtil.AddBanner(parentGO);
                break;
            case WidgetType.Animator:
                widget = UguiUtil.AddAnimator(parentGO);
                break;
            case WidgetType.Dropdown:
                widget = UguiUtil.AddDropdown(parentGO);
                break;
            case WidgetType.Animation:
                widget = UguiUtil.AddAnimation(parentGO);
                break;
        }

        return widget;
    }


}
