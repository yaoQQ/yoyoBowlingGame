using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class SoundInfo
{

    public string packName;
    public string soundName;

    public int soundType;

    public float volume;

    public bool isLoop;

    public bool isThreeD;
	public bool isMute;
    /// <summary>
    /// 如果是3D音，应挂在某个物体上;
    /// </summary>
    public GameObject threeDGO;



    public float minDistance;

    public float maxDistance;


    #region 渐入渐出声音(2D音用)

    public int fadeIn_ms = 0;

    public int fadeOut_ms = 0;

    #endregion
}
