using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

public static class BindMonoware
{
    /// <summary>
    /// Prefab自动保存和自动绑定Mono(主要为了防止不知道两套方案并行的同学，没有BindMono，这里自动绑定上就不会出错了)
    /// </summary>
    [InitializeOnLoadMethod]
    static void AutoBindMonoByApply()
    {
        PrefabUtility.prefabInstanceUpdated = delegate
        {
            GameObject selectGo = null;
            if (Selection.activeTransform)
            {
                selectGo = Selection.activeGameObject;
                if (selectGo == null)
                {
                    return;
                }
                PrefabType pType = PrefabUtility.GetPrefabType(selectGo);
                if (pType != PrefabType.PrefabInstance)
                {
                    return;
                }
                GameObject prefabGo = GetPrefabInstanceParent(selectGo);

                if (prefabGo != null && prefabGo.GetComponent<UIBaseMono>() != null)
                {

                    var prefabAsset = PrefabUtility.GetPrefabParent(prefabGo);
                    if (prefabAsset != null)
                    {
                        string path = AssetDatabase.GetAssetPath(prefabAsset);
                        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                        //绑定到监视面板上
                        BindMono(prefabGo);
                         //绑定到prefab
                        BindMono(prefab);
                        AssetDatabase.Refresh();
                    }

                }
            }
            AssetDatabase.SaveAssets();
            if (selectGo)
            {
                EditorApplication.delayCall = delegate
                {
                    Selection.activeGameObject = selectGo;
                };

            }
        };

    }


    static GameObject GetPrefabInstanceParent(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        PrefabType pType = PrefabUtility.GetPrefabType(go);
        if (pType != PrefabType.PrefabInstance)
        {
            return null;
        }
        if (go.transform.parent == null)
        {
            return go;
        }
        pType = PrefabUtility.GetPrefabType(go.transform.parent.gameObject);
        if (pType != PrefabType.PrefabInstance)
        {
            return go;
        }
        return GetPrefabInstanceParent(go.transform.parent.gameObject);
    }

    public static void BindMono(GameObject go)
    {
        //收集数据
        ExportMiddleware.UIPrefabData data = ExportMiddleware.CollectData(go);
        ExecuteBind(data, go);
    }


    #region 绑定

    static void ExecuteBind(ExportMiddleware.UIPrefabData data, GameObject go)
    {
        UIBaseMono monoBase = go.GetComponent<UIBaseMono>();
        if (monoBase == null)
        {
            monoBase = go.AddComponent<UIBaseMono>();
        }
        BindCompont(go, monoBase, data);
    }




    static void BindCompont(GameObject go, UIBaseMono monoBase, ExportMiddleware.UIPrefabData data)
    {
        FieldInfo injectionField = monoBase.GetType().GetField("MonoWidgets");
        FieldInfo injectionItemField = monoBase.GetType().GetField("ItemArrClassList");

        List<UIBaseWidget> monoWidgets = new List<UIBaseWidget>();
        List<ItemArrListClass> itemArrClassList = new List<ItemArrListClass>();


        foreach (ExportMiddleware.ExpWidget expWidget in data.expWidgetDic.Values)
        {

            UIBaseWidget bwBaseWidget;
            if (string.IsNullOrEmpty(expWidget.path))
            {
                bwBaseWidget = (UIBaseWidget)go.GetComponent(expWidget.typeName);
            }
            else
            {
                bwBaseWidget = (UIBaseWidget)go.transform.Find(expWidget.path).GetComponent(expWidget.typeName);
            }

            monoWidgets.Add(bwBaseWidget);
        }
        foreach (ExportMiddleware.ExpCell expCell in data.expCellDic.Values)
        {
            string baseCellName = char.ToLower(expCell.className[0]) + expCell.className.Substring(1) + "Arr";

            List<ItemArrClass> tempArrClassList = new List<ItemArrClass>();
            for (int j = 0; j < expCell.cellPathArr.Count; j++)
            {
                ItemArrClass tempArrClass = new ItemArrClass { Index = j };
                List<UIBaseWidget> tempBaseWidgets = new List<UIBaseWidget>();
                foreach (ExportMiddleware.ExpWidget expWidget in expCell.expWidgetDic.Values)
                {
                    GameObject itemGo = go.transform.Find(expCell.cellPathArr[j]).gameObject;
                    tempArrClass.Go = itemGo;
                    UIBaseWidget bw = itemGo.transform.Find(expWidget.path).GetComponent<UIBaseWidget>();
                    tempBaseWidgets.Add(bw);
                }
                tempArrClass.ItemBaseWidgets = tempBaseWidgets.ToArray();
                tempArrClassList.Add(tempArrClass);
            }
            ItemArrListClass tempItemArrList = new ItemArrListClass
            {
                ItemArrName = baseCellName,
                ItemBaseArr = tempArrClassList.ToArray()
            };
            itemArrClassList.Add(tempItemArrList);

        }
        injectionItemField.SetValue(monoBase, itemArrClassList.ToArray());
        injectionField.SetValue(monoBase, monoWidgets.ToArray());
    }

}


    #endregion




