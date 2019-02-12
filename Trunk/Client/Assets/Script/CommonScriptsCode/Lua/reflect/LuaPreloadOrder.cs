using System.Collections.Generic;
using XLua;

[CSharpCallLua]
public interface LuaPreloadOrder
{

    int getPreloadStyle();

    List<LuaUIView> getUIPreload();
    LuaScene getScenePreload();
    
    void onPreloadEnd();
    void onPreloadStepEnd();

}
