using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class EditCellItemView : BaseEditView
{
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        CellItemWidget cellItemWidget = widget as CellItemWidget;
        DrawCommon(window, widget.gameObject, widget);


        //遍历子对象。 有导出的。显示出来


        UIBaseWidget[] widgetArr = cellItemWidget.gameObject.transform.GetComponentsInChildren<UIBaseWidget>();
        
        for(int i=1;i< widgetArr.Length;i++)
        {
            UIBaseWidget childWidget = widgetArr[i];
            if(childWidget.exportSign)
            {
                EditorGUILayout.LabelField(childWidget.gameObject.name);
            }
        }

    }
}
