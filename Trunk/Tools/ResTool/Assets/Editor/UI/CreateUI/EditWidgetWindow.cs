using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


public class EditWidgetWindow : EditorWindow
{
    static EditWidgetWindow target;
    public static void ShowWindow(UIBaseWidget widget, WidgetType widgetType)
    {
        target = EditorWindow.GetWindow<EditWidgetWindow>(false, "Edit UI");
        target.minSize = new Vector2(300f, 500f);
        target.maxSize = new Vector2(400f, 600f);
        target.widgetType = widgetType;
        target.widget = widget;
        DontDestroyOnLoad(target);
    }
    public static void CloseWindow()
    {
        if (target != null) target.Close();
    }

    public WidgetType widgetType;
    public UIBaseWidget widget;

    Dictionary<WidgetType, BaseEditView> widgetEditViewDic = new Dictionary<WidgetType, BaseEditView>();

     void IniEditViewDic()
    {
        widgetEditViewDic.Add(WidgetType.Panel,new EditPanelView());
        widgetEditViewDic.Add(WidgetType.Button, new EditButtonView());
        widgetEditViewDic.Add(WidgetType.Text, new EditTextView());
        widgetEditViewDic.Add(WidgetType.Icon, new EditIconView());
        widgetEditViewDic.Add(WidgetType.Toggle, new EditToggleView());
        widgetEditViewDic.Add(WidgetType.InputField, new EditInputFieldView());
        widgetEditViewDic.Add(WidgetType.Image, new EditImageView());
        widgetEditViewDic.Add(WidgetType.Effect, new EditEffectView());
        widgetEditViewDic.Add(WidgetType.ScrollPanel, new EditScrollPanelView());
        widgetEditViewDic.Add(WidgetType.CellRecycleScroll, new EditCellRecycleScrollView());
        widgetEditViewDic.Add(WidgetType.CellItem, new EditCellItemView());
        widgetEditViewDic.Add(WidgetType.CellGroup, new EditCellGroupView());
        widgetEditViewDic.Add(WidgetType.Slider, new EditSliderView());
        widgetEditViewDic.Add(WidgetType.Mask, new EditMaskView());
        widgetEditViewDic.Add(WidgetType.TabPanel, new EditTabPanelView());
        widgetEditViewDic.Add(WidgetType.TextPic, new EditTextPicView());
        widgetEditViewDic.Add(WidgetType.RawImage, new EditRawImageView());
        widgetEditViewDic.Add(WidgetType.Spine, new EditSpineView());
        widgetEditViewDic.Add(WidgetType.GridRecycleScroll, new EditGridRecycleScrollView());
        widgetEditViewDic.Add(WidgetType.Marquee, new EditMarqueeView());
        widgetEditViewDic.Add(WidgetType.CircleImage, new EditCircleImageView());
        widgetEditViewDic.Add(WidgetType.EmptyImage, new EditEmptyImageView());
        widgetEditViewDic.Add(WidgetType.HorizontalLayout, new EditHorizontalLayoutView());
        widgetEditViewDic.Add(WidgetType.VerticalLayout, new EditVerticalLayoutGroupView());
        widgetEditViewDic.Add(WidgetType.GridLayout, new EditGridLayoutScrollView());
        widgetEditViewDic.Add(WidgetType.ScrollPanelWithBt, new EditScrollPanelWithButtonView());
        widgetEditViewDic.Add(WidgetType.Dropdown, new EditDropdownView());
        widgetEditViewDic.Add(WidgetType.Banner, new EditBannerView());
        widgetEditViewDic.Add(WidgetType.Animator, new EditAnimatorView());
        widgetEditViewDic.Add(WidgetType.Animation, new EditAnimationView());
    }

    BaseEditView GetEditViewByType(WidgetType type)
    {
        if (widgetEditViewDic.Count == 0) IniEditViewDic();
        BaseEditView editView = null;
        widgetEditViewDic.TryGetValue(type, out editView);
        return editView;
    }


     void OnGUI()
    {
        if(Application.isPlaying)
        {
            this.ShowNotification(new GUIContent("运行中不能编辑！"));
            return;
        }
        if (widget==null&& Selection.activeGameObject==null) return;

        UIBaseWidget activeWidget = null;
        if (Selection.activeGameObject!=null&&Selection.objects.Length == 1)
        {
             activeWidget = Selection.activeGameObject.GetComponent<UIBaseWidget>();
        }

        if (activeWidget!=null)
        {
            widget = activeWidget;
            widgetType = activeWidget.GetWidgetType();
        }
        BaseEditView editView = GetEditViewByType(widgetType);
        if(editView!=null)
        {
            RemoveNotification();
            if(widget==null)
            {
                Close();
            }
            else
            {
                editView.Render(this, widget);
            }
            
        }
        else
        {
            this.ShowNotification(new GUIContent("组件类型显示没注册！"));
        }


    }
    void OnInspectorUpdate()
    {

        this.Repaint();
    }
   
}
