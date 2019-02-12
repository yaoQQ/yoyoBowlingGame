using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditEmptyImageView : BaseEditView
{

	// Use this for initialization
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {

        EmptyImageWidget imageWidget = widget as EmptyImageWidget;
        DrawCommon(window, widget.gameObject, widget);

    }
}
