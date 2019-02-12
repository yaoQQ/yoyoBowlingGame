using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditTabPanelView : BaseEditView
{
    const int maxLen = 10;
    bool isFoldSign;
    bool[] elementFoldSignArr = new bool[maxLen];
    public override void Render(EditorWindow window, UIBaseWidget widget)
    {
        TabPanelWidget tabPanelWidget = widget as TabPanelWidget;
        DrawCommon(window, widget.gameObject, widget);

        int i;
        int curEntryNum = 0;
        if (tabPanelWidget.entities != null)
        {
            curEntryNum = tabPanelWidget.entities.Length;
        }
        int oldEntryNum = curEntryNum;

        curEntryNum = EditorGUILayout.DelayedIntField("entities数目 ：", curEntryNum, GUILayout.ExpandWidth(true));
        curEntryNum = Mathf.Min(curEntryNum, maxLen);
        if (curEntryNum != oldEntryNum)
        {
            if (curEntryNum == 0)
            {
                tabPanelWidget.entities = null;
            }
            else
            {
                int minLen = 0;
                if (tabPanelWidget.entities != null && tabPanelWidget.entities.Length != 0)
                {
                    minLen = Mathf.Min(curEntryNum, tabPanelWidget.entities.Length);
                    
                }
                TabPanelWidget. TabControlEntry[] tempArr = new TabPanelWidget.TabControlEntry[curEntryNum];
                for (i = 0; i < minLen; i++)
                {
                    tempArr[i] = tabPanelWidget.entities[i];
                }
                tabPanelWidget.entities = tempArr;
            }
        }

        isFoldSign = EditorGUILayout.Foldout(isFoldSign, "Entities");

        if (isFoldSign&& tabPanelWidget.entities!=null)
        {
            for(i = 0; i < tabPanelWidget.entities.Length; i++)
            {
                TabPanelWidget.TabControlEntry entry = tabPanelWidget.entities[i];
                elementFoldSignArr[i] = EditorGUILayout.Foldout(elementFoldSignArr[i], "Element "+i);
                if (elementFoldSignArr[i])
                {
                    entry.tab = EditorGUILayout.ObjectField("Tab ",
                         entry.tab, typeof(ButtonWidget), true, GUILayout.ExpandWidth(true)
                    ) as ButtonWidget;
                    entry.panel = EditorGUILayout.ObjectField("Panel ",
                        entry.panel, typeof(GameObject), true, GUILayout.ExpandWidth(true)
                   ) as GameObject;

                    entry.forbiddenSign = EditorGUILayout.Toggle("是否启用禁用提示TIP ", entry.forbiddenSign, GUILayout.ExpandWidth(true));
                    if(entry.forbiddenSign)
                    {
                        entry.forbiddenTip = EditorGUILayout.TextField("提示内容 ", entry.forbiddenTip, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        entry.forbiddenTip = string.Empty;
                    }
                }
            }
        }

    }
}
