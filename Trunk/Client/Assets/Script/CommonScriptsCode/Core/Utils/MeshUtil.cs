using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class MeshUtil
{
    public static void SetUV(MeshFilter mf,LuaTable luaTable)
    {      
        Vector2[] uvArr = null;
        List<Vector2> uvList = new List<Vector2>();
        for(int i=1;i<= luaTable.Length;i++)
        {
            LuaTable v2_lt = luaTable.Get<int, LuaTable>(i);
            //Debug.Log(v2_lt.Length);
            //Debug.Log(v2_lt.Get<int, float>(1));

            uvList.Add(new Vector2(v2_lt.Get<int, float>(1), v2_lt.Get<int, float>(2)));
            //Debug.Log(uvList[i - 1]);
        }

        uvArr = uvList.ToArray();
        //Debug.Log(" fdsfdsf    "+ ()[1].ToString());
        mf.mesh.uv = uvArr;
        //mf.gameObject.transform.parent.gameObject.SetActive(false);
    }
	
}
