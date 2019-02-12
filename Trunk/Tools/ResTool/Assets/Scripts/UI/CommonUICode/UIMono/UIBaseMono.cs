﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
#if !TOOL
using XLua;
#endif

[Serializable]
public class ItemArrClass
{
    public int Index;
    public GameObject Go;
    public UIBaseWidget[] ItemBaseWidgets;
}
[Serializable]
public class ItemArrListClass
{
    public string ItemArrName;
    public ItemArrClass[] ItemBaseArr;
}
#if !TOOL
[LuaCallCSharp]
#endif
public class UIBaseMono : MonoBehaviour
{
#if !TOOL
    [BlackList]
#endif
    public UIBaseWidget[] MonoWidgets;
#if !TOOL
    [BlackList]
#endif
    public ItemArrListClass[] ItemArrClassList;
#if !TOOL
    public void BindMonoTable(LuaTable monoLuaTable)
    {
        LuaEnv luaEnv = LuaManager.Instance.GetLuaEnv();
        monoLuaTable.Set("go", gameObject);
        for (int i = 0; i < MonoWidgets.Length; i++)
        {
            UIBaseWidget info = MonoWidgets[i];
            if (i == 0)
            {
                info.name = info.name.Replace("(Clone)", "");
            }
            monoLuaTable.Set(info.name.Trim(), info);
        }
        //todo 为了兼容itemArr的拆装逻辑，优化可考虑直接将item的field序列化访问
        for (int i = 0; i < ItemArrClassList.Length; i++)
        {
            ItemArrListClass itemArrList = ItemArrClassList[i];
            if (itemArrList != null)
            {
                LuaTable itemLuaTable = luaEnv.NewTable();
                for (int j = 0; j < itemArrList.ItemBaseArr.Length; j++)
                {
                    ItemArrClass itemArrClass = itemArrList.ItemBaseArr[j];
                    LuaTable itemWidgetLuaTable = luaEnv.NewTable();
                    if (itemArrClass.Go != null) itemWidgetLuaTable.Set("go", itemArrClass.Go);
                    for (int k = 0; k < itemArrClass.ItemBaseWidgets.Length; k++)
                    {
                        UIBaseWidget itemBaseWidget = itemArrClass.ItemBaseWidgets[k];
                        itemWidgetLuaTable.Set(itemBaseWidget.name.Trim(), itemBaseWidget);
                    }
                    itemLuaTable.Set(itemArrClass.Index + 1, itemWidgetLuaTable);
                }

                monoLuaTable.Set(itemArrList.ItemArrName, itemLuaTable);
            }
        }
    }
#endif
}