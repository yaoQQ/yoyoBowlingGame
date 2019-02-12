using System.Collections.Generic;
using XLua;

[CSharpCallLua]
public interface LuaModule
{

    string getModuleName();

    List<string> getRegisterNotificationList();


    void onNotificationLister(string noticeType, BaseNotice vo);


    void initRegisterNet();

}
