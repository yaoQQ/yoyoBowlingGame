using UnityEngine;
using System.Collections;

public interface IPanel 
{
    bool GetInitRunning();

    bool GetIsInit();

    GameObject GetPanelGO();

    void SetContainerGO(GameObject go);

    void SetIsInit(bool value);
   
    void Init();

    void Open(object msg);

    void OpenAnimation();

    bool CloseAnimation();

    void OnClosedPanel();


    void Del();

    UIPanelEnum GetPanelEnum();

    UIViewType GetPanelType();

}
