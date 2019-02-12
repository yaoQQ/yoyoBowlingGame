using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditAnimationView : BaseEditView
{

    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        var component = widget as AnimationWidget;
        DrawCommon(window, widget.gameObject, widget);
    }

}
