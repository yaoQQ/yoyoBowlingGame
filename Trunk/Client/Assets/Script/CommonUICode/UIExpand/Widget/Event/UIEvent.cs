#if !TOOL
using XLua;

[LuaCallCSharp]
#endif
public enum UIEvent
{
    TriggleEnter,

    PointerClick,


    PointerDown,
    PointerEnter,
    PointerUp,
    PointerExit,

    DragBegin,
    DragEnd,
    Drag,

    PointerDoubleClick,
    PointerShortClick,
    PointerLongClick,
    PinchIn,
    PinchOut,
}
