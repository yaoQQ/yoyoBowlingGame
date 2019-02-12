using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditInputFieldView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        InputFieldWidget iconWidget = widget as InputFieldWidget;
        DrawCommon(window, widget.gameObject, widget);


    }
}
