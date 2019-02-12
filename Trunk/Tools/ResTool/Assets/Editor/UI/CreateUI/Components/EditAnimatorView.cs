using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditAnimatorView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        var component = widget as AnimatorWidget;
        DrawCommon(window, widget.gameObject, widget);
    }

}
