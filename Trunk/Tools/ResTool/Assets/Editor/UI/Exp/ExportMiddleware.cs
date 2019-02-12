using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;

public static class ExportMiddleware
{
    public class ExpWidget
    {
        public string typeName;
        public string name;
        public string path;
    }

    public class ExpCell
    {
        public string className;
        //单个cell的导出
        public Dictionary<string, ExpWidget> expWidgetDic = new Dictionary<string, ExpWidget>();
        public List<string> cellPathArr = new List<string>();

        //cell里再有cell
        public Dictionary<string, ExpCell> expCellDic = new Dictionary<string, ExpCell>();
    }
    public class UIPrefabData
    {
        public string packName;
        public string name;
        public string relativePath;
        public Dictionary<string, ExpCell> expCellDic = new Dictionary<string, ExpCell>();
        public Dictionary<string, ExpWidget> expWidgetDic = new Dictionary<string, ExpWidget>();
    }
    public static void ExpMid(GameObject go, string packName, string relativePath)
    {
        //收集数据
        UIPrefabData data = CollectData(go);
        data.packName = packName;
        data.relativePath = relativePath;
        //ExecuteExp(data);
        Lua_ExecuteExp(data);
    }

    #region 收集
    public static UIPrefabData CollectData(GameObject go)
    {
        UIPrefabData data = new UIPrefabData();
        data.name = go.name;

        RecursionCollect(data, go, null, false, true);

        return data;
    }


    static string GetRootClass(GameObject go)
    {
        string str = string.Empty;
        while (go.transform.parent != null)
        {
            UIBaseWidget widget = go.GetComponent<UIBaseWidget>();
            if (widget != null)
            {
                if (widget.GetWidgetType() == WidgetType.CellItem)
                {
                    return widget.name;
                }
            }
            go = go.transform.parent.gameObject;
        }

        return str;
    }


    static void RecursionCollect(UIPrefabData data, GameObject go, ExpCell parentCell, bool onlyDealCell, bool isRoot)
    {
        ExpCell newParentCell = parentCell;
        int i;
        UIBaseWidget widget = go.GetComponent<UIBaseWidget>();
        if (widget != null)
        {


            WidgetType widgetType = widget.GetWidgetType();
            string typeName = GetWidgetTypeName(widgetType);

            if (widgetType == WidgetType.CellGroup || widgetType == WidgetType.CellRecycleScroll || widgetType == WidgetType.GridRecycleScroll)
            {
                Dictionary<string, ExpCell> expCellDic = null;
                string cellParentName = null;
                if (parentCell == null)
                {
                    expCellDic = data.expCellDic;
                    cellParentName = data.name;
                }
                else
                {
                    expCellDic = parentCell.expCellDic;
                    cellParentName = GetRootClass(go);

                }



                if (widgetType == WidgetType.CellGroup)
                {
                    CellGroupWidget cellGroupWidget = widget as CellGroupWidget;
                    CollectCell(ref newParentCell, expCellDic, cellParentName, cellGroupWidget.cellItemName, cellGroupWidget.cellItemArr);
                }
                else if (widgetType == WidgetType.CellRecycleScroll)
                {
                    CellRecycleScrollWidget cellRecycleScrollWidget = widget as CellRecycleScrollWidget;
                    CollectCell(ref newParentCell, expCellDic, cellParentName, cellRecycleScrollWidget.cellItemName, cellRecycleScrollWidget.cellItemArr);
                }
                else if (widgetType == WidgetType.GridRecycleScroll)
                {
                    GridRecycleScrollWidget gridRecycleScrollWidget = widget as GridRecycleScrollWidget;
                    CollectCell(ref newParentCell, expCellDic, cellParentName, gridRecycleScrollWidget.cellItemName, gridRecycleScrollWidget.cellItemArr);
                }
            }


            if (!onlyDealCell && typeName != string.Empty && widget.exportSign)
            {
                CollectWidget(data, typeName, widget.gameObject.name, go);
            }

            if (!isRoot && widgetType == WidgetType.CellItem)
            {
                //复用单元下。只遍历复用容器
                onlyDealCell = true;
            }
        }

        for (i = 0; i < go.transform.childCount; i++)
        {
            GameObject childGO = go.transform.GetChild(i).gameObject;


            RecursionCollect(data, childGO, newParentCell, onlyDealCell, false);
        }
    }

    static string GetWidgetTypeName(WidgetType widgetType)
    {
        string typeName = string.Empty;

        switch (widgetType)
        {
            case WidgetType.Button:
                typeName = typeof(ButtonWidget).ToString();
                break;
            case WidgetType.CellGroup:
                typeName = typeof(CellGroupWidget).ToString();

                break;
            case WidgetType.CellItem:

                break;
            case WidgetType.CellRecycleScroll:
                typeName = typeof(CellRecycleScrollWidget).ToString();

                break;
            case WidgetType.Effect:
                typeName = typeof(EffectWidget).ToString();
                break;
            case WidgetType.Icon:
                typeName = typeof(IconWidget).ToString();
                break;
            case WidgetType.Image:
                typeName = typeof(ImageWidget).ToString();
                break;
            case WidgetType.InputField:
                typeName = typeof(InputFieldWidget).ToString();
                break;
            case WidgetType.Mask:
                typeName = typeof(MaskWidget).ToString();
                break;
            case WidgetType.Panel:
                typeName = typeof(PanelWidget).ToString();
                break;
            case WidgetType.ScrollPanel:
                typeName = typeof(ScrollPanelWidget).ToString();
                break;
            case WidgetType.Slider:
                typeName = typeof(SliderWidget).ToString();
                break;
            case WidgetType.TabPanel:
                typeName = typeof(TabPanelWidget).ToString();
                break;
            case WidgetType.Text:
                typeName = typeof(TextWidget).ToString();
                break;
            case WidgetType.Toggle:
                typeName = typeof(ToggleWidget).ToString();
                break;
            case WidgetType.TextPic:
                typeName = typeof(TextPicWidget).ToString();
                break;
            case WidgetType.RawImage:
                typeName = typeof(RawImageWidget).ToString();
                break;
            case WidgetType.Spine:
                typeName = typeof(SpineWidget).ToString();
                break;
            case WidgetType.GridRecycleScroll:
                typeName = typeof(GridRecycleScrollWidget).ToString();
                break;
            case WidgetType.Marquee:
                typeName = typeof(MarqueeWidget).ToString();
                break;
            case WidgetType.CircleImage:
                typeName = typeof(CircleImageWidget).ToString();
                break;
            case WidgetType.EmptyImage:
                typeName = typeof(EmptyImageWidget).ToString();
                break;
            case WidgetType.CellRecycleNewScroll:
                typeName = typeof(CellRecycleNewScrollWidget).ToString();
                break;
            case WidgetType.HorizontalLayout:
                typeName = typeof(HorizontalLayoutGroupWidget).ToString();
                break;
            case WidgetType.VerticalLayout:
                typeName = typeof(VerticalLayoutGroupWidget).ToString();
                break;
            case WidgetType.GridLayout:
                typeName = typeof(GridLayoutGroupWidget).ToString();
                break;
            case WidgetType.ScrollPanelWithBt:
                typeName = typeof(ScrollPanelWithButtonWidget).ToString();
                break;
            case WidgetType.Banner:
                typeName = typeof(BannerWidget).ToString();
                break;
            case WidgetType.Animator:
                typeName = typeof(AnimatorWidget).ToString();
                break;
            case WidgetType.Animation:
                typeName = typeof(AnimationWidget).ToString();
                break;
            case WidgetType.NumberPicker:
                typeName = typeof(NumberPickerWidget).ToString();
                break;
            default:
                Debug.LogError("RecursionCollect 有类型没有处理 " + widgetType);
                break;
        }
        return typeName;
    }

    static void CollectCell(ref ExpCell newExpCell, Dictionary<string, ExpCell> expCellDic, string rootClass, string cellName, CellItemWidget[] cellItemArr)
    {
        if (cellName == string.Empty || cellName.Length == 1)
        {
            Debug.LogError(cellName + " ===> CollectCell 导出名字不符合约定！");
            return;
        }
        cellName = char.ToUpper(cellName[0]) + cellName.Substring(1);

        //if (expCellDic.ContainsKey(cellName))
        //{
        //    Debug.LogError(cellName + " ===> CollectCell 重名，赶紧改资源好吧！");
        //    //return;
        //}

        ExpCell expCell = new ExpCell()
        {
            className = cellName,
        };
        Dictionary<string, ExpWidget> widgetDic = null;
        for (int i = 0; i < cellItemArr.Length; i++)
        {
            CellItemWidget cellItemWidget = cellItemArr[i];
            if (cellItemWidget == null)
            {
                Debug.LogError(cellName + " ===> CollectCell  不能有空的cell ！");
                return;
            }
            expCell.cellPathArr.Add(GetPathStr(cellItemWidget.gameObject, rootClass));
            Dictionary<string, ExpWidget> tempWidgetDic = GetCellWidgetDic(cellItemWidget);
            if (widgetDic != null)
            {
                //对比
                if (!CompareCellExpDic(widgetDic, tempWidgetDic))
                {
                    //不一样。报错。
                    Debug.LogError(cellName + " ===> CollectCell  每个cell导出的要一样 ！");
                    return;
                }
            }

            widgetDic = tempWidgetDic;
        }
        expCell.expWidgetDic = widgetDic;

        newExpCell = expCell;
        if (!expCellDic.ContainsKey(cellName))
        {
            expCellDic.Add(cellName, expCell);
        }

    }

    static bool CompareCellExpDic(Dictionary<string, ExpWidget> dicA, Dictionary<string, ExpWidget> dicB)
    {
        if (dicA.Count != dicB.Count) return false;
        foreach (KeyValuePair<string, ExpWidget> kvp in dicA)
        {
            if (!dicB.ContainsKey(kvp.Key)) return false;
            ExpWidget expWidgetA = kvp.Value;
            ExpWidget expWidgetB = dicB[kvp.Key];
            if (expWidgetA.name != expWidgetB.name) return false;
            if (expWidgetA.typeName != expWidgetB.typeName) return false;
            if (expWidgetA.path != expWidgetB.path) return false;
        }
        return true;
    }

    static Dictionary<string, ExpWidget> GetCellWidgetDic(CellItemWidget cellItemWidget)
    {
        Dictionary<string, ExpWidget> tempWidgetDic = new Dictionary<string, ExpWidget>();
        for (int i = 0; i < cellItemWidget.gameObject.transform.childCount; i++)
        {
            CollectCellWidget(cellItemWidget, cellItemWidget.gameObject.transform.GetChild(i).gameObject, ref tempWidgetDic);
        }

        return tempWidgetDic;
    }
    static void CollectCellWidget(CellItemWidget cellItemWidget, GameObject go, ref Dictionary<string, ExpWidget> tempWidgetDic)
    {
        UIBaseWidget widget = go.GetComponent<UIBaseWidget>();
        if (widget == null) return;
        //if (widget.GetWidgetType() == WidgetType.CellGroup) return;
        if (widget.GetWidgetType() == WidgetType.CellItem) return;
        if (widget.exportSign)
        {
            ExpWidget expWidget = new ExpWidget();
            expWidget.name = go.name;
            expWidget.typeName = GetWidgetTypeName(widget.GetWidgetType());
            expWidget.path = GetPathStr(widget.gameObject, cellItemWidget.gameObject.name);
            if (tempWidgetDic.ContainsKey(expWidget.name))
            {
                Debug.LogError(expWidget.name + " ===> GetCellWidgetDic  导出重名 ！");
            }
            else
            {
                tempWidgetDic.Add(go.name, expWidget);
            }
        }

        for (int i = 0; i < go.transform.childCount; i++)
        {
            CollectCellWidget(cellItemWidget, go.transform.GetChild(i).gameObject, ref tempWidgetDic);
        }
    }

    static string GetPathStr(GameObject go, string rootName)
    {
        string widgetPath = string.Empty;

        while (go.name != rootName)
        {
            if (widgetPath == string.Empty)
            {
                widgetPath = go.name;
            }
            else
            {
                widgetPath = (go.name + "/" + widgetPath);

            }
            go = go.transform.parent.gameObject;
        }
        return widgetPath;
    }

    static void CollectWidget(UIPrefabData data, string widgetType, string widgetName, GameObject go)
    {
        if (data.expWidgetDic.ContainsKey(widgetName))
        {
            Debug.LogError(widgetName + " ===> CollectWidget 重名，赶紧改资源好吧！");
            return;
        }
        string widgetPath = GetPathStr(go, data.name);
        ExpWidget expWidget = new ExpWidget()
        {
            typeName = widgetType,
            name = widgetName,
            path = widgetPath
        };
        data.expWidgetDic.Add(expWidget.name, expWidget);
    }


    #endregion

    #region 导出

    static void ExecuteExp(UIPrefabData data)
    {
        string className = "Mid_" + data.name;

        string fileName = Application.dataPath + "/Script/Core/UI/Middleware/" + className + ".cs";
        FileInfo fi = new FileInfo(fileName);
        var di = fi.Directory;
        if (!di.Exists)
            di.Create();
        FileStream fs;
        try
        {
            fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        }
        catch (Exception ex)
        {
            Debug.LogError("目录被打开，不能生成新文件：" + fileName);
            return;
        }
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

        StringBuilder sb = new StringBuilder();
        WriteClassImport(ref sb);
        WriteClassHead(data, ref sb);
        WriteItemClass(data.expCellDic, ref sb);
        WriteClassAttribute(data, ref sb);
        WriteClassMainFun(data, ref sb);
        WriteDelReferenceFun(data, ref sb);
        WriteClassTail(ref sb);
        sw.WriteLine(sb);
        sw.Close();
        fs.Close();

    }

    static void WriteClassImport(ref StringBuilder sb)
    {
        sb.Append("using System;\n");
        sb.Append("using System.Collections;\n");
        sb.Append("using System.Collections.Generic;\n");
        sb.Append("using UnityEngine;\n");
        sb.Append("using UnityEngine.UI;\n\n");
    }
    static void WriteClassHead(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("public class " + "Mid_" + data.name + ":IMiddleware\n{\n");

    }

    static void WriteItemClass(Dictionary<string, ExpCell> expCellDic, ref StringBuilder sb)
    {
        foreach (KeyValuePair<string, ExpCell> kvp in expCellDic)
        {
            ExpCell expCell = kvp.Value;
            sb.Append("\tpublic class " + kvp.Key + "\n\t{\n");

            if (expCell.expCellDic.Count > 0)
            {
                WriteItemClass(expCell.expCellDic, ref sb);
            }
            foreach (ExpCell cell in expCell.expCellDic.Values)
            {
                string cellName = char.ToLower(cell.className[0]) + cell.className.Substring(1);
                sb.Append("\tpublic " + cell.className + "[] " + cellName + "Arr" + ";\n");
            }

            sb.Append("\t\tpublic GameObject go;\n");

            foreach (ExpWidget expWidget in expCell.expWidgetDic.Values)
            {
                sb.Append("\t\tpublic " + expWidget.typeName + " " + expWidget.name + ";\n");
            }
            // 构建方法;  
            sb.Append("\t\tpublic " + kvp.Key + "(GameObject itemGo)\n\t\t{\n");
            sb.Append("\t\t\tthis.go=itemGo;\n");
            foreach (ExpWidget expWidget in expCell.expWidgetDic.Values)
            {
                sb.Append("\t\t\t" + expWidget.name + " =  itemGo.transform.Find(\"" + expWidget.path + "\").GetComponent<" + expWidget.typeName + ">();\n");
            }

            foreach (ExpCell cell in expCell.expCellDic.Values)
            {
                string cellName = char.ToLower(cell.className[0]) + cell.className.Substring(1);
                sb.Append("\t\tList<" + cell.className + "> " + cellName + "List=new List<" + cell.className + ">() ;\n");
                for (int i = 0; i < cell.cellPathArr.Count; i++)
                {
                    sb.Append("\t\t" + cellName + "List.Add(new " + cell.className + "(go.transform.Find(\"" + cell.cellPathArr[i] + "\").gameObject));\n");
                }

                sb.Append("\t\t" + cellName + "Arr=" + cellName + "List.ToArray();\n");
            }

            sb.Append("\t\t}\n");
            sb.Append("\t}\n");
        }
    }

    static void WriteClassAttribute(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("\tpublic GameObject go;\n");
        foreach (ExpWidget expWidget in data.expWidgetDic.Values)
        {
            sb.Append("\tpublic " + expWidget.typeName + " " + expWidget.name + ";\n");
        }
        foreach (ExpCell expCell in data.expCellDic.Values)
        {
            string cellName = char.ToLower(expCell.className[0]) + expCell.className.Substring(1);
            sb.Append("\tpublic " + expCell.className + "[] " + cellName + "Arr" + ";\n");
        }
    }

    static void WriteClassMainFun(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("\n\tpublic " + "Mid_" + data.name + "(GameObject go) \n\t{\n");
        sb.Append("\t\tthis.go =  go;\n");
        List<string> filerList = new List<string>();
        foreach (ExpWidget expWidget in data.expWidgetDic.Values)
        {
            if (string.IsNullOrEmpty(expWidget.path))
            {
                sb.Append("\t\t" + expWidget.name + " =  go.GetComponent<" + expWidget.typeName + ">();\n");
            }
            else
            {
                sb.Append("\t\t" + expWidget.name + " =  go.transform.Find(\"" + expWidget.path + "\").GetComponent<" + expWidget.typeName + ">();\n");

            }
        }
        foreach (ExpCell expCell in data.expCellDic.Values)
        {
            string cellName = char.ToLower(expCell.className[0]) + expCell.className.Substring(1);
            sb.Append("\t\tList<" + expCell.className + "> " + cellName + "List=new List<" + expCell.className + ">() ;\n");
            for (int i = 0; i < expCell.cellPathArr.Count; i++)
            {
                sb.Append("\t\t" + cellName + "List.Add(new " + expCell.className + "(go.transform.Find(\"" + expCell.cellPathArr[i] + "\").gameObject));\n");
            }

            sb.Append("\t\t" + cellName + "Arr=" + cellName + "List.ToArray();\n");
        }
        sb.Append("\t}\n");
    }

    static void WriteDelReferenceFun(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("\n\tpublic void DelReference() \n\t{\n");
        sb.Append("#if TOOL\n");
        sb.Append("#else\n");
        sb.Append("\t\tif(go!=null) GameObject.Destroy(go);\n");
        //sb.Append("\t\tUILoadTool.Instance.DelUIReference(" + "\"" + data.name + "\"" + ",1);\n");
        sb.Append("#endif\n");
        sb.Append("\t}\n");
    }

    static void WriteClassTail(ref StringBuilder sb)
    {
        sb.Append("\n}");
    }

    #endregion


    #region 导出lua mid

    static void Lua_ExecuteExp(UIPrefabData data)
    {
        string className = "Mid_" + data.name;

        string projectName = ProjectUtil.GetCurProjectName();

        string fileName = Application.dataPath + "/../../../" + PathUtil.GetClientName() + "/res/lua/" + data.packName + "/mid/" + data.relativePath + className + ".lua";
        FileInfo fi = new FileInfo(fileName);
        var di = fi.Directory;
        if (!di.Exists)
            di.Create();
        FileStream fs;
        try
        {
            fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        }
        catch (Exception ex)
        {
            Debug.LogError("目录被打开，不能生成新文件：" + fileName);
            return;
        }
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

        StringBuilder sb = new StringBuilder();
        Lua_WriteClassImport(data, ref sb);
        Lua_WriteClassHead(data, ref sb);
        Lua_WriteClassMainFun(data, ref sb);

        Lua_WriteClassAttribute(data, ref sb);

        Lua_WriteInitFun(data, ref sb);
        //WriteClassTail(ref sb);
        Lua_WriteItemClass(data.expCellDic, ref sb);

        sw.WriteLine(sb);
        sw.Close();
        fs.Close();

    }

    static void Lua_WriteClassImport(UIPrefabData data, ref StringBuilder sb)
    {
        List<string> importTypeList = new List<string>();
        foreach (ExpWidget expWidget in data.expWidgetDic.Values)
        {
            if (!importTypeList.Contains(expWidget.typeName))
            {
                importTypeList.Add(expWidget.typeName);
            }
        }
        foreach (ExpCell expCell in data.expCellDic.Values)
        {
            foreach (ExpWidget expWidget in expCell.expWidgetDic.Values)
            {
                if (!importTypeList.Contains(expWidget.typeName))
                {
                    importTypeList.Add(expWidget.typeName);
                }
            }
        }
        for (int i = 0; i < importTypeList.Count; i++)
        {
            string widgetType = importTypeList[i];
            sb.Append("local " + widgetType + "=CS." + widgetType + "\n");
        }
    }
    static void Lua_WriteClassHead(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("\nMid_" + data.name + "={}\n");
        sb.Append("local this = " + "Mid_" + data.name + "\n\n");
    }
    static void Lua_WriteClassMainFun(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("function this:new(gameObject)\n");
        sb.Append("\tlocal o = { }\n");
        sb.Append("\tsetmetatable(o, self)\n");
        sb.Append("\tself.__index = self\n");
        sb.Append("\to:init(gameObject)\n");
        sb.Append("\treturn o\n");
        sb.Append("end\n\n");
    }
    static void Lua_WriteClassAttribute(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("this.go = nil\n");
        foreach (ExpWidget expWidget in data.expWidgetDic.Values)
        {
            sb.Append("this." + expWidget.name + "=nil\n");
        }
        foreach (ExpCell expCell in data.expCellDic.Values)
        {
            string cellName = char.ToLower(expCell.className[0]) + expCell.className.Substring(1);
            sb.Append("--" + expCell.className + "数组\n");
            sb.Append("this." + cellName + "Arr={}\n");
        }
    }
    static void Lua_WriteInitFun(UIPrefabData data, ref StringBuilder sb)
    {
        sb.Append("\nfunction this:init(gameObject)\n");
        sb.Append("\tself.go=gameObject\n");
        foreach (ExpWidget expWidget in data.expWidgetDic.Values)
        {
            sb.Append("\tself." + expWidget.name + "=self.go.transform:Find(\"" + expWidget.path + "\"):GetComponent(typeof(" + expWidget.typeName + "))\n");
        }
        foreach (ExpCell expCell in data.expCellDic.Values)
        {
            string cellName = char.ToLower(expCell.className[0]) + expCell.className.Substring(1);
            sb.Append("\tself." + cellName + "Arr={}\n");
            for (int i = 0; i < expCell.cellPathArr.Count; i++)
            {
                sb.Append("\ttable.insert(self." + cellName + "Arr,self.new_" + expCell.className + "(self.go.transform:Find(\"" + expCell.cellPathArr[i] + "\").gameObject))\n");
            }

        }
        sb.Append("end\n\n");
    }

    static void Lua_WriteItemClass(Dictionary<string, ExpCell> expCellDic, ref StringBuilder sb)
    {
        foreach (ExpCell expCell in expCellDic.Values)
        {
            sb.Append("--" + expCell.className + "复用单元\n");
            sb.Append("function this.new_" + expCell.className + "(itemGo)\n");
            sb.Append("\tlocal item = { }\n");
            sb.Append("\titem.go = itemGo\n");
            foreach (ExpWidget expWidget in expCell.expWidgetDic.Values)
            {
                sb.Append("\titem." + expWidget.name + "=itemGo.transform:Find(\"" + expWidget.path + "\"):GetComponent(typeof(" + expWidget.typeName + "))\n");
            }
            sb.Append("\treturn item\n");
            sb.Append("end\n");
        }
    }
    #endregion
}
