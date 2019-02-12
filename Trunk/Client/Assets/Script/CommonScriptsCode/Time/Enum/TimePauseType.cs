using UnityEngine;
using System.Collections;

public enum TimePauseType 
{
    /// <summary>
    /// 没有暂停;
    /// </summary>
    None=0,
    /// <summary>
    /// 所有的暂停;
    /// </summary>
    All=1,
    /// <summary>
    /// 暂停场景逻辑，除了剧情;
    /// </summary>
    SceneExceptPlot = 2
}
