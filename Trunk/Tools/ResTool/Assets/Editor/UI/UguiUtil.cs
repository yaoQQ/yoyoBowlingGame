using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.UI.Extensions;
using Spine.Unity;

/// <summary>
/// This script adds the UI menu options to the Unity Editor.
/// </summary>

public static class UguiUtil
{
    private const string kUILayerName = "UI";
    private const float kWidth = 160f;
    private const float kThickHeight = 30f;
    private const float kThinHeight = 20f;
    private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";
    private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    private const string kKnobPath = "UI/Skin/Knob.psd";
    private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
    private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
    private const string kUIMaskPath = "UI/Skin/UIMask.psd";

    private static Font GetDefalutFont()
    {
        return AssetDatabase.LoadAssetAtPath<Font>("Assets/Project/" + ProjectUtil.GetCurProjectName() + "/ui/base/Font/font_1.otf");
    }

    private static Vector2 s_ThickGUIElementSize = new Vector2(kWidth, kThickHeight);
    private static Vector2 s_ThinGUIElementSize = new Vector2(kWidth, kThinHeight);
    private static Vector2 s_ImageGUIElementSize = new Vector2(100f, 100f);
    private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
    private static Color s_PanelColor = new Color(1f, 1f, 1f, 1f);
    private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);



    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        // Find the best scene view
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null && SceneView.sceneViews.Count > 0)
            sceneView = SceneView.sceneViews[0] as SceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }

    private static GameObject CreateUIElementRoot(string name, GameObject p_parent, Vector2 size)
    {
        GameObject parent = p_parent;
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            parent = GetOrCreateCanvasGameObject();
        }
        GameObject child = new GameObject(name);

        Undo.RegisterCreatedObjectUndo(child, "Create " + name);
        Undo.SetTransformParent(child.transform, parent.transform, "Parent " + child.name);
        GameObjectUtility.SetParentAndAlign(child, parent);

        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        if (parent != p_parent) // not a context click, so center in sceneview
        {
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), rectTransform);
        }
        Selection.activeGameObject = child;
        return child;
    }

    /// <summary>
    ///  添加 横向Grid/table 组件
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    static public HorizontalLayoutGroupWidget AddHorizontalGroupWidget(GameObject p_parent)
    {
        GameObject horizontalLayoutGroupObj = CreateUIElementRoot("HorizontalLayoutGroup", p_parent, new Vector2(Screen.width / 2, Screen.height / 2));

        HorizontalLayoutGroupWidget horizontalGroup = horizontalLayoutGroupObj.AddComponent<HorizontalLayoutGroupWidget>();
        horizontalGroup.InnerHorizontalGroup = horizontalLayoutGroupObj.AddComponent<HorizontalLayoutGroup>();
        return horizontalGroup;
    }

    /// <summary>
    /// 添加纵向 Grid/table 组件
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    static public VerticalLayoutGroupWidget AddVerticalLayoutGroupWidget(GameObject p_parent)
    {
        GameObject verticalLayoutGroupObj = CreateUIElementRoot("VerticalLayoutGroup", p_parent, new Vector2(Screen.width / 2, Screen.height / 2));

        VerticalLayoutGroupWidget vertucalGroup = verticalLayoutGroupObj.AddComponent<VerticalLayoutGroupWidget>();
        vertucalGroup.InnerVerticalGroup = verticalLayoutGroupObj.AddComponent<VerticalLayoutGroup>();
        return vertucalGroup;
    }

    /// <summary>
    /// 添加纵向  Grid/table 双向 组件
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    static public GridLayoutGroupWidget AddGridLayoutGroupWidget(GameObject p_parent)
    {
        GameObject gridLayoutGroupObj = CreateUIElementRoot("GridLayoutGroup", p_parent, new Vector2(Screen.width / 2, Screen.height / 2));

        GridLayoutGroupWidget gridLayoutGroup = gridLayoutGroupObj.AddComponent<GridLayoutGroupWidget>();
        gridLayoutGroup.InnerGridGroup = gridLayoutGroupObj.AddComponent<GridLayoutGroup>();
        return gridLayoutGroup;
    }

    static public ScrollPanelWithButtonWidget AddScrollPanelWithBtnWidgeHorizontal(GameObject p_parent)
    {
        GameObject scrollPanelWithBtnWidgeObj = CreateUIElementRoot("ScrollPanelWithBtnWidge", p_parent, new Vector2(Screen.width / 2, Screen.height / 2));
        ScrollPanelWithButtonWidget scrollPanelWithBtnWidge = scrollPanelWithBtnWidgeObj.AddComponent<ScrollPanelWithButtonWidget>();
        RectTransform scpRt = scrollPanelWithBtnWidge.transform as RectTransform;
        scpRt.pivot = new Vector2(0.5f, 0.5f);
        scpRt.anchorMin = new Vector2(0, 1);
        scpRt.anchorMax = new Vector2(0, 1);
        scpRt.anchoredPosition = Vector2.zero;
        scpRt.sizeDelta = new Vector2(560, 800);

        Image image = scrollPanelWithBtnWidgeObj.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        image.type = Image.Type.Sliced;
        image.color = s_PanelColor;

        scrollPanelWithBtnWidge.mask = scrollPanelWithBtnWidgeObj.AddComponent<RectMask2D>();
        ScrollRect scroll = scrollPanelWithBtnWidgeObj.AddComponent<ScrollRect>();
        scrollPanelWithBtnWidge.scrollRect = scroll;
        scrollPanelWithBtnWidge.scrollRT = scroll.gameObject.transform as RectTransform;

        //添加banner layout
        HorizontalLayoutGroupWidget bannerWidget = AddHorizontalGroupWidget(scrollPanelWithBtnWidgeObj);
        bannerWidget.name = "bannerLayout";
        RectTransform bRt = bannerWidget.transform as RectTransform;
        bRt.pivot = new Vector2(0, 1);
        bRt.anchorMin = new Vector2(0, 1);
        bRt.anchorMax = new Vector2(0, 1);
        bRt.anchoredPosition = Vector2.zero;
        bRt.sizeDelta = new Vector2(0, 800);
        scrollPanelWithBtnWidge.scrollRect.content = bRt;
        scrollPanelWithBtnWidge.scrollRect.vertical = false;
        scrollPanelWithBtnWidge.contentRT = bRt;

        //添加pointbt layout
        HorizontalLayoutGroupWidget pointWidget = AddHorizontalGroupWidget(scrollPanelWithBtnWidgeObj);
        pointWidget.name = "PointLayout";
        RectTransform pRt = pointWidget.transform as RectTransform;
        pRt.pivot = new Vector2(0, 1);
        pRt.anchorMin = new Vector2(0, 1);
        pRt.anchorMax = new Vector2(0, 1);
        pRt.anchoredPosition = Vector2.zero;
        pRt.sizeDelta = new Vector2(0, 800);

        scrollPanelWithBtnWidge.pointRt = pRt;

        return scrollPanelWithBtnWidge;
    }


    static public PanelWidget AddPanel(GameObject p_parent)
    {
        GameObject panelRoot = CreateUIElementRoot("Panel", p_parent, s_ThickGUIElementSize);

        PanelWidget panelWidget = panelRoot.AddComponent<PanelWidget>();
        Image image = panelRoot.AddComponent<Image>();
        panelWidget.Img = image;
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        image.type = Image.Type.Sliced;
        image.color = s_PanelColor;

        // Set RectTransform to stretch
        RectTransform rectTransform = panelRoot.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(100, 100);


        return panelWidget;
    }
    static public GameObject AddRange(GameObject p_parent)
    {
        GameObject rangeGO = CreateUIElementRoot("range", p_parent, s_ThickGUIElementSize);
        Image rangeImage = rangeGO.AddComponent<Image>();
        // Set RectTransform to stretch
        RectTransform rectTransform = rangeGO.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        return rangeGO;
    }
    static public ScrollPanelWidget AddScrollPanel(GameObject p_parent)
    {
        GameObject panelRoot = CreateUIElementRoot("ScrollPanel", p_parent, s_ThickGUIElementSize);
        ScrollPanelWidget scrollPanelWidget = panelRoot.AddComponent<ScrollPanelWidget>();
        // Set RectTransform to stretch
        RectTransform rectTransform = panelRoot.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(100, 100);

        Image image = panelRoot.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        image.type = Image.Type.Sliced;
        image.color = s_PanelColor;

        scrollPanelWidget.mask = panelRoot.AddComponent<RectMask2D>();
        ScrollRect scroll = panelRoot.AddComponent<ScrollRect>();
        scrollPanelWidget.scrollRect = scroll;

        scroll.content = (RectTransform)CreateUIElementRoot("content", panelRoot, s_ThickGUIElementSize).transform;


        scrollPanelWidget.scrollRT = scroll.gameObject.transform as RectTransform;
        scrollPanelWidget.contentRT = scroll.content.gameObject.transform as RectTransform;
        scrollPanelWidget.contentRT.anchoredPosition = Vector2.zero;

        return scrollPanelWidget;
    }

    static public CellRecycleScrollWidget AddCellRecycleScrollPanel(GameObject p_parent)
    {
        GameObject panelRoot = CreateUIElementRoot("CellRecycleScrollPanel", p_parent, s_ThickGUIElementSize);
        CellRecycleScrollWidget cellRecycleScrollWidget = panelRoot.AddComponent<CellRecycleScrollWidget>();
        // Set RectTransform to stretch
        RectTransform rectTransform = panelRoot.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(100, 100);

        Image image = panelRoot.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        image.type = Image.Type.Sliced;
        image.color = s_PanelColor;

        cellRecycleScrollWidget.mask = panelRoot.AddComponent<RectMask2D>();
        ScrollRect scroll = panelRoot.AddComponent<ScrollRect>();
        cellRecycleScrollWidget.scrollRect = scroll;

        scroll.content = (RectTransform)CreateUIElementRoot("content", panelRoot, s_ThickGUIElementSize).transform;

        cellRecycleScrollWidget.scrollRT = scroll.gameObject.transform as RectTransform;
        cellRecycleScrollWidget.contentRT = scroll.content.gameObject.transform as RectTransform;
        cellRecycleScrollWidget.contentRT.anchoredPosition = Vector2.zero;
        return cellRecycleScrollWidget;
    }
    static public GridRecycleScrollWidget AddGridRecycleScrollPanel(GameObject p_parent)
    {
        GameObject panelRoot = CreateUIElementRoot("GridRecycleScrollPanel", p_parent, s_ThickGUIElementSize);
        GridRecycleScrollWidget gridRecycleScrollWidget = panelRoot.AddComponent<GridRecycleScrollWidget>();
        // Set RectTransform to stretch
        RectTransform rectTransform = panelRoot.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = new Vector2(100, 100);

        Image image = panelRoot.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        image.type = Image.Type.Sliced;
        image.color = s_PanelColor;

        gridRecycleScrollWidget.mask = panelRoot.AddComponent<RectMask2D>();
        ScrollRect scroll = panelRoot.AddComponent<ScrollRect>();
        gridRecycleScrollWidget.scrollRect = scroll;

        scroll.content = (RectTransform)CreateUIElementRoot("content", panelRoot, s_ThickGUIElementSize).transform;

        gridRecycleScrollWidget.scrollRT = scroll.gameObject.transform as RectTransform;
        gridRecycleScrollWidget.contentRT = scroll.content.gameObject.transform as RectTransform;
        gridRecycleScrollWidget.contentRT.anchoredPosition = Vector2.zero;
        return gridRecycleScrollWidget;
    }
    static public CellItemWidget AddCellItem(GameObject p_parent)
    {
        GameObject cellRoot = CreateUIElementRoot("CellItem", p_parent, s_ThickGUIElementSize);
        CellItemWidget cellItemWidget = cellRoot.AddComponent<CellItemWidget>();
        return cellItemWidget;
    }
    static public CellGroupWidget AddCellGroup(GameObject p_parent)
    {
        GameObject cellRoot = CreateUIElementRoot("CellGroup", p_parent, s_ThickGUIElementSize);
        CellGroupWidget cellGroupWidget = cellRoot.AddComponent<CellGroupWidget>();
        return cellGroupWidget;
    }



    static public ButtonWidget AddButton(GameObject p_parent)
    {
        GameObject buttonRoot = CreateUIElementRoot("Button", p_parent, s_ThickGUIElementSize);

        GameObject childText = new GameObject("label");
        GameObjectUtility.SetParentAndAlign(childText, buttonRoot);

        Image image = buttonRoot.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        image.type = Image.Type.Sliced;
        image.color = s_DefaultSelectableColor;

        ButtonWidget bt = buttonRoot.AddComponent<ButtonWidget>();

        Button button = bt.gameObject.AddComponent<Button>();
        bt.Btn = button;
        button.transition = Selectable.Transition.SpriteSwap;
        SetDefaultColorTransitionValues(button);


        Text text = childText.AddComponent<Text>();
        text.font = (GetDefalutFont());
        text.gameObject.AddComponent<IgnoreUIEvent>();
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.text = "Button";
        text.alignment = TextAnchor.MiddleCenter;
        bt.Txt = text;
        SetDefaultTextValues(text);

        RectTransform textRectTransform = childText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        return bt;
    }

    static public TextWidget AddText(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("Text", p_parent, s_ThickGUIElementSize);
        TextWidget textWidget = go.AddComponent<TextWidget>();
        Text lbl = go.AddComponent<Text>();
        lbl.font = GetDefalutFont();
        textWidget.Txt = lbl;
        lbl.text = "New Text";

        SetDefaultTextValues(lbl);
        return textWidget;
    }


    private static void SetDefaultTextValues(Text lbl)
    {
        // Set text values we want across UI elements in default controls.
        // Don't set values which are the same as the default values for the Text component,
        // since there's no point in that, and it's good to keep them as consistent as possible.
        lbl.color = s_TextColor;
    }

    static public EffectWidget AddEffect(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("Effect", p_parent, s_ThickGUIElementSize);
        EffectWidget effectWidget = go.AddComponent<EffectWidget>();
        Image img = go.AddComponent<Image>();
        img.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/EffectMaskMat.mat", typeof(Material));
        if (img.material == null)
        {
            Debug.LogError("EffectMaskMat 材质球不存在");
        }
        effectWidget.maskImg = img;

        return effectWidget;
    }

    static public ImageWidget AddImage(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("Image", p_parent, s_ImageGUIElementSize);
        ImageWidget imageWidget = go.AddComponent<ImageWidget>();
        Image image = go.AddComponent<Image>();
        imageWidget.grayMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Project/YoYo/ui/base/Exp_mat/image_gray_mat.mat", typeof(Material));
        imageWidget.Img = image;
        return imageWidget;
    }

    static public CircleImageWidget AddCircleImage(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("CircleImage", p_parent, s_ImageGUIElementSize);
        CircleImageWidget imageWidget = go.AddComponent<CircleImageWidget>();
        CircleImage image = go.AddComponent<CircleImage>();
        imageWidget.Img = image;
        return imageWidget;
    }
    static public EmptyImageWidget AddEmptyImage(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("EmptyImageWidget", p_parent, s_ImageGUIElementSize);
        EmptyImageWidget imageWidget = go.AddComponent<EmptyImageWidget>();
        EmptyImage image = go.AddComponent<EmptyImage>();
        return imageWidget;
    }
    static public IconWidget AddIcon(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("Icon", p_parent, s_ImageGUIElementSize);
        IconWidget iconWidget = go.AddComponent<IconWidget>();
        Image image = go.AddComponent<Image>();
        iconWidget.Img = image;
        iconWidget.grayMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Project/YoYo/ui/base/Exp_mat/image_gray_mat.mat", typeof(Material));
        return iconWidget;
    }

    static public MaskWidget AddMask(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("Mask", p_parent, s_ImageGUIElementSize);
        MaskWidget maskWidget = go.AddComponent<MaskWidget>();
        Image image = go.AddComponent<Image>();
        image.color = new Color(255, 255, 255, 0);
        maskWidget.uiMask = go.AddComponent<RectMask2D>();
        maskWidget.maskImg = image;
        return maskWidget;
    }

    static public TabPanelWidget AddTabPanel(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("TabPanel", p_parent, s_ImageGUIElementSize);
        TabPanelWidget tabPanelWidget = go.AddComponent<TabPanelWidget>();


        return tabPanelWidget;
    }

    static public TextPicWidget AddTextPic(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("TextPic", p_parent, s_ImageGUIElementSize);
        TextPicWidget textPicWidget = go.AddComponent<TextPicWidget>();
        textPicWidget.textPic = go.AddComponent<TextPic>();
        return textPicWidget;
    }

    static public RawImageWidget AddRawImage(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("RawImage", p_parent, s_ImageGUIElementSize);
        RawImageWidget rawImageWidget = go.AddComponent<RawImageWidget>();
        rawImageWidget.rawImage = go.AddComponent<RawImage>();
        return rawImageWidget;
    }
    static public SpineWidget AddSpine(GameObject p_parent)
    {
        GameObject go = CreateUIElementRoot("spine", p_parent, s_ImageGUIElementSize);
        SpineWidget spineWidget = go.AddComponent<SpineWidget>();
        spineWidget.skeleton = go.AddComponent<SkeletonGraphic>();
        return spineWidget;
    }

    static public MarqueeWidget AddMarquee(GameObject p_parent)
    {
        GameObject marqueeRoot = CreateUIElementRoot("marquee", p_parent, s_ImageGUIElementSize);

        MarqueeWidget marqueeWidget = marqueeRoot.AddComponent<MarqueeWidget>();

        Mask mask = marqueeRoot.AddComponent<Mask>();
        marqueeWidget.maskImg = marqueeRoot.AddComponent<Image>();

        mask.showMaskGraphic = false;
        marqueeWidget.mask = mask.rectTransform;

        marqueeWidget.moveContainer = (RectTransform)(CreateUIElementRoot("content", marqueeRoot, s_ThickGUIElementSize).transform);
        marqueeWidget.moveContainer.anchorMin = new Vector2(0, 0.5f);
        marqueeWidget.moveContainer.anchorMax = new Vector2(0, 0.5f);
        marqueeWidget.moveContainer.pivot = new Vector2(0, 0.5f);
        marqueeWidget.moveContainer.sizeDelta = new Vector2(100, 20);
        for (int i = 0; i < 3; i++)
        {
            GameObject childLabel = CreateUIObject("txt_" + i, marqueeWidget.moveContainer.gameObject);

            Text label = childLabel.AddComponent<Text>();
            label.alignment = TextAnchor.MiddleLeft;
            label.rectTransform.anchorMin = new Vector2(0, 0.5f);
            label.rectTransform.anchorMax = new Vector2(0, 0.5f);
            label.rectTransform.pivot = new Vector2(0, 0.5f);
            label.font = GetDefalutFont();
            label.text = "Marquee";
            label.rectTransform.sizeDelta = new Vector2(100, 20);
            label.rectTransform.anchoredPosition = new Vector2(100 * i, 0);
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            SetDefaultTextValues(label);
            marqueeWidget.txtArr[i] = label;
        }
        return marqueeWidget;
    }

    static GameObject CreateUIObject(string name, GameObject parent)
    {
        GameObject go = new GameObject(name);
        go.AddComponent<RectTransform>();
        GameObjectUtility.SetParentAndAlign(go, parent);
        return go;
    }

    static public SliderWidget AddSlider(GameObject p_parent, bool isHandle = true)
    {
        // Create GOs Hierarchy
        GameObject root = CreateUIElementRoot("Slider", p_parent, s_ThinGUIElementSize);
        SliderWidget sliderWidget = root.AddComponent<SliderWidget>();
        GameObject background = CreateUIObject("Background", root);
        GameObject fillArea = CreateUIObject("Fill Area", root);
        GameObject fill = CreateUIObject("Fill", fillArea);
        GameObject handleArea = null;
        GameObject handle = null;
        if (isHandle)
        {
            handleArea = CreateUIObject("Handle Slide Area", root);
            handle = CreateUIObject("Handle", handleArea);
        }


        // Background
        Image backgroundImage = background.AddComponent<Image>();
        sliderWidget.bgImg = backgroundImage;
        backgroundImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        backgroundImage.type = Image.Type.Sliced;
        backgroundImage.color = s_DefaultSelectableColor;
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.anchorMin = new Vector2(0, 0.25f);
        backgroundRect.anchorMax = new Vector2(1, 0.75f);
        backgroundRect.sizeDelta = new Vector2(0, 0);


        // Fill Area
        RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0.25f);
        fillAreaRect.anchorMax = new Vector2(1, 0.75f);
        //fillAreaRect.anchoredPosition = new Vector2(-5, 0);
        fillAreaRect.anchoredPosition = new Vector2(0, 0);

        //fillAreaRect.sizeDelta = new Vector2(-20, 0);
        fillAreaRect.sizeDelta = new Vector2(0, 0);


        // Fill
        Image fillImage = fill.AddComponent<Image>();
        sliderWidget.fillImg = fillImage;
        fillImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        fillImage.type = Image.Type.Sliced;
        fillImage.color = s_DefaultSelectableColor;

        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.sizeDelta = new Vector2(0, 0);

        Image handleImage = null;
        if (isHandle)
        {
            // Handle Area
            RectTransform handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.sizeDelta = new Vector2(-20, 0);
            handleAreaRect.anchorMin = new Vector2(0, 0);
            handleAreaRect.anchorMax = new Vector2(1, 1);

            // Handle
            handleImage = handle.AddComponent<Image>();
            handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
            handleImage.color = s_DefaultSelectableColor;

            RectTransform handleRect = handle.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20, 0);
        }


        // Setup slider component
        Slider slider = root.AddComponent<Slider>();
        sliderWidget.slider = slider;
        slider.fillRect = fill.GetComponent<RectTransform>();
        if (isHandle)
        {
            slider.handleRect = handle.GetComponent<RectTransform>();
            slider.targetGraphic = handleImage;
        }

        slider.direction = Slider.Direction.LeftToRight;
        SetDefaultColorTransitionValues(slider);
        return sliderWidget;
    }



    private static void SetDefaultColorTransitionValues(Selectable slider)
    {
        ColorBlock colors = slider.colors;
        colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
        colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
        colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
    }

    static public void AddScrollbar(GameObject p_parent)
    {
        // Create GOs Hierarchy
        GameObject scrollbarRoot = CreateUIElementRoot("Scrollbar", p_parent, s_ThinGUIElementSize);

        GameObject sliderArea = CreateUIObject("Sliding Area", scrollbarRoot);
        GameObject handle = CreateUIObject("Handle", sliderArea);

        Image bgImage = scrollbarRoot.AddComponent<Image>();
        bgImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        bgImage.type = Image.Type.Sliced;
        bgImage.color = s_DefaultSelectableColor;

        Image handleImage = handle.AddComponent<Image>();
        handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        handleImage.type = Image.Type.Sliced;
        handleImage.color = s_DefaultSelectableColor;

        RectTransform sliderAreaRect = sliderArea.GetComponent<RectTransform>();
        sliderAreaRect.sizeDelta = new Vector2(-20, -20);
        sliderAreaRect.anchorMin = Vector2.zero;
        sliderAreaRect.anchorMax = Vector2.one;

        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20, 20);

        Scrollbar scrollbar = scrollbarRoot.AddComponent<Scrollbar>();
        scrollbar.handleRect = handleRect;
        scrollbar.targetGraphic = handleImage;
        SetDefaultColorTransitionValues(scrollbar);
    }

    static public ToggleWidget AddToggle(GameObject p_parent)
    {
        // Set up hierarchy
        GameObject toggleRoot = CreateUIElementRoot("Toggle", p_parent, s_ThinGUIElementSize);

        GameObject background = CreateUIObject("Background", toggleRoot);
        GameObject checkmark = CreateUIObject("Checkmark", background);
        GameObject childLabel = CreateUIObject("label", toggleRoot);

        // Set up components
        Toggle toggle = toggleRoot.AddComponent<Toggle>();

        ToggleWidget toggleWidget = toggleRoot.AddComponent<ToggleWidget>();
        toggleWidget.toggle = toggle;
        toggle.isOn = true;

        Image bgImage = background.AddComponent<Image>();
        toggleWidget.BgImg = bgImage;
        bgImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        bgImage.type = Image.Type.Sliced;
        bgImage.color = s_DefaultSelectableColor;

        Image checkmarkImage = checkmark.AddComponent<Image>();
        toggleWidget.CheackMaskImg = checkmarkImage;
        checkmarkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);

        Text label = childLabel.AddComponent<Text>();
        label.font = GetDefalutFont();
        toggleWidget.Txt = label;
        label.text = "Toggle";
        SetDefaultTextValues(label);

        toggle.graphic = checkmarkImage;
        toggle.targetGraphic = bgImage;
        SetDefaultColorTransitionValues(toggle);

        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0f, 1f);
        bgRect.anchorMax = new Vector2(0f, 1f);
        bgRect.anchoredPosition = new Vector2(10f, -10f);
        bgRect.sizeDelta = new Vector2(kThinHeight, kThinHeight);

        RectTransform checkmarkRect = checkmark.GetComponent<RectTransform>();
        checkmarkRect.anchorMin = new Vector2(0.5f, 0.5f);
        checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
        checkmarkRect.anchoredPosition = Vector2.zero;
        checkmarkRect.sizeDelta = new Vector2(20f, 20f);

        RectTransform labelRect = childLabel.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0f, 0f);
        labelRect.anchorMax = new Vector2(1f, 1f);
        labelRect.offsetMin = new Vector2(23f, 1f);
        labelRect.offsetMax = new Vector2(-5f, -2f);

        return toggleWidget;
    }

    public static InputFieldWidget AddInputField(GameObject p_parent)
    {
        GameObject root = CreateUIElementRoot("InputField", p_parent, s_ThickGUIElementSize);
        InputFieldWidget inputFieldWidget = root.AddComponent<InputFieldWidget>();
        GameObject childPlaceholder = CreateUIObject("Placeholder", root);
        GameObject childText = CreateUIObject("Text", root);

        Image image = root.AddComponent<Image>();
        image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
        image.type = Image.Type.Sliced;
        image.color = s_DefaultSelectableColor;

        InputField inputField = root.AddComponent<InputField>();
        inputFieldWidget.inputField = inputField;

        SetDefaultColorTransitionValues(inputField);

        Text text = childText.AddComponent<Text>();
        text.font = GetDefalutFont();
        text.text = "";
        text.supportRichText = false;
        SetDefaultTextValues(text);

        Text placeholder = childPlaceholder.AddComponent<Text>();
        placeholder.font = GetDefalutFont();
        placeholder.text = "Enter text...";
        placeholder.fontStyle = FontStyle.Italic;
        // Make placeholder color half as opaque as normal text color.
        Color placeholderColor = text.color;
        placeholderColor.a *= 0.5f;
        placeholder.color = placeholderColor;

        RectTransform textRectTransform = childText.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.sizeDelta = Vector2.zero;
        textRectTransform.offsetMin = new Vector2(10, 6);
        textRectTransform.offsetMax = new Vector2(-10, -7);

        RectTransform placeholderRectTransform = childPlaceholder.GetComponent<RectTransform>();
        placeholderRectTransform.anchorMin = Vector2.zero;
        placeholderRectTransform.anchorMax = Vector2.one;
        placeholderRectTransform.sizeDelta = Vector2.zero;
        placeholderRectTransform.offsetMin = new Vector2(10, 6);
        placeholderRectTransform.offsetMax = new Vector2(-10, -7);

        inputField.textComponent = text;
        inputField.placeholder = placeholder;

        return inputFieldWidget;
    }

    static public void AddCanvas(GameObject p_parent)
    {
        var go = CreateNewUI();
        GameObjectUtility.SetParentAndAlign(go, p_parent);
        if (go.transform.parent as RectTransform)
        {
            RectTransform rect = go.transform as RectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
        }
        Selection.activeGameObject = go;
    }

    static public GameObject CreateNewUI()
    {
        // Root for the UI
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

        // if there is no event system add one...
        CreateEventSystem(false);
        return root;
    }

    public static void CreateEventSystem(GameObject p_parent)
    {
        GameObject parent = p_parent;
        CreateEventSystem(true, parent);
    }

    private static void CreateEventSystem(bool select)
    {
        CreateEventSystem(select, null);
    }

    private static void CreateEventSystem(bool select, GameObject parent)
    {
        var esys = Object.FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            //eventSystem.AddComponent<TouchInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null)
        {
            Selection.activeGameObject = esys.gameObject;
        }
    }

    // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
    static public GameObject GetOrCreateCanvasGameObject()
    {
        GameObject selectedGo = Selection.activeGameObject;

        // Try to find a gameobject that is the selected GO or one if its parents.
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in selection or its parents? Then use just any canvas..
        canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in the scene at all? Then create a new one.
        return UguiUtil.CreateNewUI();
    }

    static public DropdownWidget AddDropdown(GameObject p_parent)
    {
        GameObject root = CreateUIElementRoot("Dropdown", p_parent, s_ThickGUIElementSize);
        GameObject label = CreateUIObject("Label", root);
        GameObject arrow = CreateUIObject("Arrow", root);
        GameObject template = CreateUIObject("Template", root);
        GameObject viewport = CreateUIObject("Viewport", template);
        GameObject content = CreateUIObject("Content", viewport);
        GameObject item = CreateUIObject("Item", content);
        GameObject itemBackground = CreateUIObject("Item Background", item);
        GameObject itemCheckmark = CreateUIObject("Item Checkmark", item);
        GameObject itemLabel = CreateUIObject("Item Label", item);
        GameObject scrollbar = CreateUIObject("Scrollbar", template);
        GameObject slidingArea = CreateUIObject("Sliding Area", scrollbar);
        GameObject handle = CreateUIObject("Handle", slidingArea);

        DropdownWidget dropdownWidget = root.AddComponent<DropdownWidget>();
        dropdownWidget.Img = root.AddComponent<Image>();
        dropdownWidget.Img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        dropdownWidget.Img.type = Image.Type.Sliced;
        dropdownWidget.Img.color = s_DefaultSelectableColor;
        dropdownWidget.Drop = root.AddComponent<Dropdown>();

        Text labelText = label.AddComponent<Text>();
        labelText.color = s_TextColor;
        labelText.alignment = TextAnchor.MiddleLeft;
        RectTransform labelTextRectTransform = labelText.GetComponent<RectTransform>();
        labelTextRectTransform.anchorMin = Vector2.zero;
        labelTextRectTransform.anchorMax = Vector2.one;
        labelTextRectTransform.sizeDelta = Vector2.zero;
        labelTextRectTransform.offsetMin = new Vector2(10, 6);
        labelTextRectTransform.offsetMax = new Vector2(-25, -7);

        Image arrowImage = arrow.AddComponent<Image>();
        arrowImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
        arrowImage.type = Image.Type.Simple;
        arrowImage.color = s_DefaultSelectableColor;

        RectTransform arrowRectTransform = arrow.GetComponent<RectTransform>();
        arrowRectTransform.anchorMin = new Vector2(1, 0.5f);
        arrowRectTransform.anchorMax = new Vector2(1, 0.5f);
        arrowRectTransform.sizeDelta = Vector2.zero;
        arrowRectTransform.offsetMin = new Vector2(-25, -10);
        arrowRectTransform.offsetMax = new Vector2(-5, 10);

        RectTransform templateRectTransform = template.GetComponent<RectTransform>();
        templateRectTransform.anchorMin = Vector2.zero;
        templateRectTransform.anchorMax = Vector2.right;
        templateRectTransform.pivot = new Vector2(0.5f, 1f);
        templateRectTransform.sizeDelta = Vector2.zero;
        templateRectTransform.offsetMin = new Vector2(0, -148);
        templateRectTransform.offsetMax = new Vector2(0, 2);

        Image templateImage = template.AddComponent<Image>();
        templateImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        templateImage.type = Image.Type.Sliced;
        templateImage.color = s_DefaultSelectableColor;

        ScrollRect templateScroolRect = template.AddComponent<ScrollRect>();

        RectTransform viewportRectTransform = viewport.GetComponent<RectTransform>();
        viewportRectTransform.anchorMin = Vector2.zero;
        viewportRectTransform.anchorMax = Vector2.one;
        viewportRectTransform.pivot = Vector2.up;
        viewportRectTransform.sizeDelta = Vector2.zero;
        viewportRectTransform.offsetMin = new Vector2(0, 0);
        viewportRectTransform.offsetMax = new Vector2(-17, 0);

        Mask viewportMask = viewport.AddComponent<Mask>();
        viewportMask.showMaskGraphic = false;
        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kUIMaskPath);
        viewportImage.type = Image.Type.Sliced;
        viewportImage.color = s_DefaultSelectableColor;

        RectTransform contentRectTransform = content.GetComponent<RectTransform>();
        contentRectTransform.anchorMin = Vector2.up;
        contentRectTransform.anchorMax = Vector2.one;
        contentRectTransform.pivot = new Vector2(0.5f, 1f);
        contentRectTransform.sizeDelta = Vector2.zero;
        contentRectTransform.offsetMin = new Vector2(0, -28);
        contentRectTransform.offsetMax = new Vector2(0, 0);

        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.anchorMin = new Vector2(0, 0.5f);
        itemRectTransform.anchorMax = new Vector2(1, 0.5f);
        itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
        itemRectTransform.sizeDelta = Vector2.zero;
        itemRectTransform.offsetMin = new Vector2(0, -10);
        itemRectTransform.offsetMax = new Vector2(0, 10);

        Toggle itemToggle = item.AddComponent<Toggle>();

        Image itemBackgroundImage = itemBackground.AddComponent<Image>();

        RectTransform itemBackgroundRectTransform = itemBackground.GetComponent<RectTransform>();
        itemBackgroundRectTransform.anchorMin = Vector2.zero;
        itemBackgroundRectTransform.anchorMax = Vector2.one;
        itemBackgroundRectTransform.pivot = new Vector2(0.5f, 0.5f);
        itemBackgroundRectTransform.sizeDelta = Vector2.zero;
        itemBackgroundRectTransform.offsetMin = new Vector2(0, 0);
        itemBackgroundRectTransform.offsetMax = new Vector2(0, 0);

        Image itemCheckmarkImage = itemCheckmark.AddComponent<Image>();
        itemCheckmarkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
        itemCheckmarkImage.type = Image.Type.Simple;
        itemCheckmarkImage.color = s_DefaultSelectableColor;

        RectTransform itemCheckmarkRectTransform = itemCheckmark.GetComponent<RectTransform>();
        itemCheckmarkRectTransform.anchorMin = new Vector2(0, 0.5f);
        itemCheckmarkRectTransform.anchorMax = new Vector2(0, 0.5f);
        itemCheckmarkRectTransform.pivot = new Vector2(0.5f, 0.5f);
        itemCheckmarkRectTransform.sizeDelta = Vector2.zero;
        itemCheckmarkRectTransform.offsetMin = new Vector2(0, -10);
        itemCheckmarkRectTransform.offsetMax = new Vector2(20, 10);

        RectTransform itemLabelRectTransform = itemLabel.GetComponent<RectTransform>();
        itemLabelRectTransform.anchorMin = Vector2.zero;
        itemLabelRectTransform.anchorMax = Vector2.one;
        itemLabelRectTransform.pivot = new Vector2(0.5f, 0.5f);
        itemLabelRectTransform.sizeDelta = Vector2.zero;
        itemLabelRectTransform.offsetMin = new Vector2(20, 1);
        itemLabelRectTransform.offsetMax = new Vector2(-10, -2);

        Text itemLabelText = itemLabel.AddComponent<Text>();
        itemLabelText.color = s_TextColor;
        itemLabelText.alignment = TextAnchor.MiddleLeft;

        RectTransform scrollbarRectTransform = scrollbar.GetComponent<RectTransform>();
        scrollbarRectTransform.anchorMin = Vector2.right;
        scrollbarRectTransform.anchorMax = Vector2.one;
        scrollbarRectTransform.pivot = Vector2.one;
        scrollbarRectTransform.sizeDelta = Vector2.zero;
        scrollbarRectTransform.offsetMin = new Vector2(-20, 0);
        scrollbarRectTransform.offsetMax = new Vector2(0, 0);

        Image scrollbarImage = scrollbar.AddComponent<Image>();
        scrollbarImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
        scrollbarImage.type = Image.Type.Sliced;
        scrollbarImage.color = s_DefaultSelectableColor;

        Scrollbar scrollbarScroll = scrollbar.AddComponent<Scrollbar>();

        RectTransform slidingAreaRectTransform = slidingArea.GetComponent<RectTransform>();
        slidingAreaRectTransform.anchorMin = Vector2.zero;
        slidingAreaRectTransform.anchorMax = Vector2.one;
        slidingAreaRectTransform.pivot = new Vector2(0.5f, 0.5f);
        slidingAreaRectTransform.sizeDelta = Vector2.zero;
        slidingAreaRectTransform.offsetMin = new Vector2(10, 10);
        slidingAreaRectTransform.offsetMax = new Vector2(-10, -10);

        RectTransform handleRectTransform = handle.GetComponent<RectTransform>();
        handleRectTransform.anchorMin = Vector2.zero;
        handleRectTransform.anchorMax = Vector2.one;
        handleRectTransform.pivot = new Vector2(0.5f, 0.5f);
        handleRectTransform.sizeDelta = Vector2.zero;
        handleRectTransform.offsetMin = new Vector2(-10, -10);
        handleRectTransform.offsetMax = new Vector2(10, 10);

        Image handleImage = handle.AddComponent<Image>();
        handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
        handleImage.type = Image.Type.Sliced;
        handleImage.color = s_DefaultSelectableColor;

        dropdownWidget.Drop.template = templateRectTransform;
        dropdownWidget.Drop.captionText = labelText;
        dropdownWidget.Drop.itemText = itemLabelText;

        templateScroolRect.content = contentRectTransform;
        templateScroolRect.viewport = viewportRectTransform;
        templateScroolRect.verticalScrollbar = scrollbarScroll;
        templateScroolRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        templateScroolRect.verticalScrollbarSpacing = -3;
        templateScroolRect.movementType = ScrollRect.MovementType.Clamped;
        templateScroolRect.horizontal = false;

        itemToggle.targetGraphic = itemBackgroundImage;
        itemToggle.isOn = true;
        itemToggle.graphic = itemCheckmarkImage;

        scrollbarScroll.targetGraphic = handleImage;
        scrollbarScroll.handleRect = handleRectTransform;
        scrollbarScroll.size = 0.2f;

        dropdownWidget.Arrow = arrowImage;
        dropdownWidget.Template = templateImage;
        dropdownWidget.ItemCheckmark = itemCheckmarkImage;
        dropdownWidget.ItemBackground = itemBackgroundImage;
        template.SetActive(false);

        return dropdownWidget;
    }

    static public BannerWidget AddBanner(GameObject p_parent)
    {
        GameObject panelRoot = CreateUIElementRoot("Banner", p_parent, s_ThickGUIElementSize);
        BannerWidget widget = panelRoot.AddComponent<BannerWidget>();
        // Set RectTransform to stretch
        RectTransform rectTransform = panelRoot.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        var bg = panelRoot.AddComponent<Image>();
        bg.color = Color.black;

        var viewport = (RectTransform)CreateUIElementRoot("viewPort", panelRoot, s_ThickGUIElementSize).transform;
        for (var i = 0; i < 3; i++)
        {
            var rt = (RectTransform)CreateUIElementRoot("bannerItem" + i, viewport.gameObject, s_ThickGUIElementSize).transform;
            rt.gameObject.AddComponent<Image>();
            if (i == 1)
                widget.content = rt;
        }
        widget.viewRect = viewport;
        return widget;
    }
    static public AnimatorWidget AddAnimator(GameObject p_parent)
    {
        var widget = p_parent.AddComponent<AnimatorWidget>();

        return widget;
    }
    static public AnimationWidget AddAnimation(GameObject p_parent)
    {
        var widget = p_parent.AddComponent<AnimationWidget>();
        widget.BaseAnimation = p_parent.GetComponent<Animation>();
        return widget;
    }
}
