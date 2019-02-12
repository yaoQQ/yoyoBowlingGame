#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public enum UISpineEvent
{
    Start,
    Interrupt,
    End,
    Dispose,
    Complete,
    Event,
}
