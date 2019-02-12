using UnityEngine;
using System.Collections;
using ProtoBufSpace;
public class GMView 
{

    int BtnW
    {
        get { return 100 * Screen.width / 1600; }
    }

    public int BtnH
    {
        get { return 40 * Screen.width / 1600; }
    }

    int fontSize = 20;

    //GM 标识;
    bool isExpand = false;

    string gmValue = "";

    public bool IsExpand
    {
        get { return isExpand; }
    }
   

    GUIStyle style;

    GUIStyle GetBtnStyle()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.button);
        }
        style.fontSize = Mathf.FloorToInt(fontSize * Screen.width / 1600);
        return style;
    }

    public void Render()
    {

        string expandName;
        if (isExpand)
        {
            expandName = "GM关闭";
            gmValue = GUI.TextArea(new Rect(0, BtnH, BtnW, BtnH), gmValue, GetBtnStyle());
        }
        else
        {
            expandName = "GM打开";
        }
        if (GUI.Button(new Rect(0, 0, BtnW, BtnH), expandName, GetBtnStyle()))
        {
            isExpand = !isExpand;
        }
        if (isExpand)
        {
            MenuRender();
        }

    }
    void MenuRender()
    {
        if (GUI.Button(new Rect(BtnW, 0, BtnW, BtnH), "通信", GetBtnStyle()))
        {
            curMenuType = GMMenuType.Socket;
        }
        SubmenuRender();
    }

    enum GMMenuType
    {
        None,
        Socket,

    }

    GMMenuType curMenuType = GMMenuType.None;


    void SubmenuRender()
    {
        switch (curMenuType)
        {
            case GMMenuType.Socket:
                SocketRender();
                break;
           
        }
    }

    string connectSockeMsg;
    void SocketRender()
    {
        connectSockeMsg = GUI.TextField(new Rect(BtnW, BtnH * 3, 150, BtnH), "127.0.0.1:8080", GetBtnStyle());
        if (GUI.Button(new Rect(BtnW, BtnH * 5, BtnW, BtnH), "连接", GetBtnStyle()))
        {
              string[] arr=connectSockeMsg.Split(':');
            if(arr.Length==2)
            {
                 //发送连服务器
                ConnectSocketNotice notice = new ConnectSocketNotice()
                {
                    ip = arr[0],
                    port =int.Parse(arr[1])
                };
                NoticeManager.Instance.Dispatch(notice.GetNotificationType(),notice);
            }
        }

        if (GUI.Button(new Rect(BtnW, BtnH * 7, BtnW, BtnH), "测试", GetBtnStyle()))
        {
            //NameCheckReq req = new NameCheckReq();
            //req.name = "大嘴";
            //req.age = 102;
            //Package package = new Package(ProtoBufSpace.TProtocol.Login_Req, ProtobufTool.PSerializer(req));
            //NetworkManager.Instance.SendMessage(package);
        }
    }
}
