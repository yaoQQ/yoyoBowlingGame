using UnityEngine;
using System.Collections.Generic;
using ProtoBufSpace;
using XLua;
using SimpleJSON;

/// <summary>
/// 与java后台通信注册模块
/// </summary>
[LuaCallCSharp]
public class JavaNetWorkManager
{

    protected static JavaNetWorkManager instance;
    public JavadataConnect m_jsonConnect;
    public static JavaNetWorkManager Instance
    {
        get
        {
            if (instance == null) {
                instance = new JavaNetWorkManager();
            }
            return instance;
        }
    }

    protected Dictionary<string, JavadataMessageReceive> dicEventHandler = new Dictionary<string, JavadataMessageReceive>();


    public void RegisterEventHandler(string code, JavadataMessageReceive handler) {
        Loger.PrintLog("@@Jsion JavadataMessageReceive注册 ===>>>  ", code.ToString());
        if (dicEventHandler.ContainsKey(code)) {
            dicEventHandler[code] += handler;
        }
        else {
            dicEventHandler[code] = handler;
        }
    }

    public void RemoveEventHandler(string code, JavadataMessageReceive handler) {
        if (dicEventHandler.ContainsKey(code)) {
            dicEventHandler[code] -= handler;
        }
    }
    public void accept(byte[] data) {
      
        if (data.Length > 0) {
            string result = string.Empty;
            result = System.Text.Encoding.Default.GetString(data, 0, data.Length);
           // Debug.Log("<color='red'服务器对我说：" + result+"</color>");

         //   Loger.PrintLog("setJavaToU3D:" + result);
            if (string.IsNullOrEmpty(result)) {
                Debug.LogError("java 返回的数据为空！！");
                return;
            }
            JSONNode root = JSONNode.Parse(result);
            if (root == null) {
                Debug.LogError("java 返回的数据格式不是json格式！！");
                return;
            }
            string pid = root["pid"];
         //   Loger.PrintLog(" @@@@ 获取java端MVP数据 pid=" + pid);
            NetworkManager.Instance.RecieveJavaDataPackage(pid, result);

        }

    }




    public void InvokeCallBack(string protoID, string jsonData) {
        if (dicEventHandler.ContainsKey(protoID)) {
            if (dicEventHandler[protoID] != null) {
                dicEventHandler[protoID].Invoke(protoID, jsonData);
            }
            else {
                Debug.Log(protoID + " 无监听");
            }
        }
        else {
            Debug.Log(protoID + " 无监听");
        }
    }

    public bool ContainsKey(string code) {
        return dicEventHandler.ContainsKey(code);
    }

    public int Count { get { return this.dicEventHandler.Count; } }


    protected void reset() {
        this.dicEventHandler.Clear();
    }
}