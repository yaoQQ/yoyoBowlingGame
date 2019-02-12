using UnityEngine;
using System.Collections;
using UnityEditor;
public class EditRawImageView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        RawImageWidget rawImageWidget = widget as RawImageWidget;
        DrawCommon(window, widget.gameObject, widget);


        rawImageWidget.rawImage.enabled = rawImageWidget.rawImage.texture != null ? true : false;  
    }

}
