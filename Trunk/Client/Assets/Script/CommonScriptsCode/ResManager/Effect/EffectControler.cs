using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControler : MonoBehaviour {

    /// <summary>
    /// 播放时间配置
    /// </summary>
    public int play_time_ms = -1;



    public void Play()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
#if TOOL
#else
        if(play_time_ms!=-1)
        {
            GlobalTimeManager.Instance.timerController.RemoveTimerByKey(this);
            GlobalTimeManager.Instance.timerController.AddTimer(this, play_time_ms, 1, OnTimeUp);
        }
#endif
    }

    void OnTimeUp(int count)
    {
        Stop();
    }

    public void Stop()
    {
        gameObject.SetActive(false);
#if TOOL
#else
        if(play_time_ms!=-1)
        {
             GlobalTimeManager.Instance.timerController.RemoveTimerByKey(this);
        }
#endif
    }
}
