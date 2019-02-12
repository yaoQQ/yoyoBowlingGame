using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XLua;

[LuaCallCSharp]
public class DisableTermsManager : Singleton<DisableTermsManager>
{
    List<string> table = new List<string>();

    public void Init(List<string> _list)
    {
        this.table = _list;
    }

    /// <summary>
    /// 所给字符串中是否包含敏感词
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public bool IsMatch(string s)
    {
        if (string.IsNullOrEmpty(s) || table.Count == 0)
            return false;
        return table.Any(i => s.Contains(i));
    }

}
