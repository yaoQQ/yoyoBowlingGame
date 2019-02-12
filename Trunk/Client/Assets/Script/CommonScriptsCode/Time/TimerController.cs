using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimerController
{
    public const int TimerMinInterval_ms = 50;
    public class Timer
    {
        public bool isDefer = false;
        public bool paused = false;

        public System.Object key;
        /// <summary>
        /// 单位：100纳秒;
        /// </summary>
        public long interval_ns;
        public int maxCount;
        public Action<int> onTimeFun;
        /// <summary>
        /// 忽略的时间暂停类型;
        /// </summary>
        public TimePauseType ignoreType;
        int curCount = 0;
        long totalTime = 0;

        public Timer(System.Object p_key, long p_interval_ms, int p_maxCount, Action<int> p_onTimeFun,bool p_isDefer, TimePauseType p_ignoreType = TimePauseType.None)
        {

            key = p_key; interval_ns = p_interval_ms * 10000; maxCount = p_maxCount; onTimeFun = p_onTimeFun; isDefer = p_isDefer; ignoreType = p_ignoreType;
        }
        public void resetTimerData(long p_interval_ms, int p_maxCount, Action<int> p_onTimeFun, bool p_isDefer, TimePauseType p_ignoreType = TimePauseType.None)
        {
            interval_ns = p_interval_ms * 10000; maxCount = p_maxCount; onTimeFun = p_onTimeFun; isDefer = p_isDefer; ignoreType = p_ignoreType;
            reset();
        }
        public bool Execute(long deltaTime)
        {
            paused=CheckPause();
            if (!paused)
            {
                if (key == null) return false;
                long oleTime = totalTime;
                totalTime += deltaTime;
                long newTime = totalTime;
                long targetTime = interval_ns * (curCount + 1);
                if (interval_ns<0)
                {
                    curCount++;

                    if (curCount > maxCount && maxCount!=-1)
                    {
                        //计时器结束;
                        return false;
                    }
                    else
                    {
                        onTimeFun.Invoke(curCount);
                    }
                }else{
                
                    if (oleTime < targetTime && newTime >= targetTime)
                    {
                      
                        //Debug.Log("触发时间点：" + totalTime);
                        curCount++;

                        if (curCount > maxCount && maxCount != -1)
                        {
                            //计时器结束;
                            return false;
                        }
                        else
                        {
                            onTimeFun.Invoke(curCount);
                        }
                    }
                }
               
            }
            return true;
        }
        bool CheckPause()
        {
            switch (ignoreType)
            {
                case TimePauseType.None:
                    if (GlobalTimeManager.Instance.CurPauseType != TimePauseType.None)
                    {
                        return true;
                    }
                    break;
                case TimePauseType.All:
                    return false;
                case TimePauseType.SceneExceptPlot:
                    if (GlobalTimeManager.Instance.CurPauseType != TimePauseType.None || GlobalTimeManager.Instance.CurPauseType != TimePauseType.SceneExceptPlot)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        /// <summary>
        /// 重置时间;
        /// </summary>
        public void reset()
        {
            totalTime = 0;
            curCount = 0;
           
        }
        public void Clear()
        {
            key = null; onTimeFun = null; 
        }
    }
    Dictionary<System.Object, Timer> timerDic = new Dictionary<object, Timer>();
    List<Timer> timerList = new List<Timer>();
    Dictionary<System.Object, Timer> timerDeferDic = new Dictionary<object, Timer>();
    List<Timer> timerDeferList = new List<Timer>();



    public void Init()
    {
        timerDic.Clear();
        timerList.Clear();
        timerDeferDic.Clear();
        timerDeferList.Clear();
    }
    public int lastDeltaTime;
    public void Execute(long deltaTime)
    {
        lastDeltaTime =(int)(deltaTime/ 10000f);
        for (int i = timerList.Count-1; i >=0; i--)
        {
            if (i >= timerList.Count)
                continue;
            Timer timer=timerList[i];
            if (!timer.Execute(deltaTime))
            {
                RemoveTimer(timer);
            }
            if (timer.onTimeFun==null)
            {
                RemoveTimer(timer);
            }
        }
       
    }
    public void DeferExecute(long deltaTime)
    {

        for (int i = timerDeferList.Count - 1; i >= 0; i--)
        {
           
            Timer timer = timerDeferList[i];
             //Debug.LogError(timer.key.ToString());
            if (!timer.Execute(deltaTime))
            {
                RemoveTimer(timer);
            }
            if (timer.onTimeFun == null)
            {
                RemoveTimer(timer);
            }
        }
    }

    /// <summary>
    /// 增加计时器
    /// </summary>
    /// <param name="p_key"></param>
    /// <param name="p_interval_ms">间隔时间，-1为按帧</param>
    /// <param name="p_maxCount"></param>
    /// <param name="p_onTimeFun"></param>
    /// <param name="isDefer">推迟标识，推迟的在LateUpdate执行</param>
    /// <param name="p_ignoreType"></param>
    /// <returns></returns>
    public bool AddTimer(System.Object p_key, long p_interval_ms, int p_maxCount, Action<int> p_onTimeFun, bool p_isDefer = false, TimePauseType p_ignoreType = TimePauseType.None)
    {
        if (p_interval_ms!=-1&&p_interval_ms < TimerMinInterval_ms)
        {
            Loger.PrintWarning("AddTimer,时间间隔必须大于50毫秒，否则等同0");
            return false;
        }
        if (p_key == null) return false;

        Dictionary<System.Object, Timer> tempTimerDic;
        List<Timer> tempTimerList; 

        if (p_isDefer)
        {
            tempTimerDic = timerDeferDic;
            tempTimerList = timerDeferList;
        }
        else
        {
            tempTimerDic = timerDic;
            tempTimerList = timerList;
        }

        Timer tempTimer;
        if (tempTimerDic.TryGetValue(p_key, out tempTimer))
        {
            //Loger.PrintWarning("TimerController已存在key值：" , p_key.ToString());
            tempTimer.resetTimerData(p_interval_ms, p_maxCount, p_onTimeFun, p_isDefer, p_ignoreType);
        }
        else
        {
            Timer timer = new Timer(p_key, p_interval_ms, p_maxCount, p_onTimeFun, p_isDefer, p_ignoreType);
            tempTimerDic.Add(p_key, timer);
            tempTimerList.Add( timer);
        }
       
        return true;
    }
   
    public bool RemoveTimer(Timer timer)
    {
        if (timerDic.ContainsValue(timer))
        {
            timerDic.Remove(timer.key);
            timerList.Remove(timer);
            timer.Clear();
            return true;
        }
        else if (timerDeferDic.ContainsValue(timer))
        {
            timerDeferDic.Remove(timer.key);
            timerDeferList.Remove(timer);
            timer.Clear();
            return true;
        }
        return false;
    }
    public bool RemoveTimerByKey(System.Object key)
    {
       Timer timer;
        if (timerDic.TryGetValue(key,out timer))
        {
            timerDic.Remove(key);
            timerList.Remove(timer);
            timer.Clear();
            return true;
        }
        else if (timerDeferDic.TryGetValue(key, out timer))
        {
            timerDeferDic.Remove(key);
            timerDeferList.Remove(timer);
            timer.Clear();
            return true;
        }
        return false;
    }
    public Timer GetTimerByKey(System.Object p_key)
    {
        Timer timer;
        if(timerDic.TryGetValue(p_key, out timer))
        {
            return timer;
        }
        else if (timerDeferDic.TryGetValue(p_key, out timer))
        {
            return timer;
        }
        return null;
    }
    public bool CheckExistByKey(System.Object p_key)
    {
        if (timerDic.ContainsKey(p_key))
        {
            return true;
        }
        else if (timerDeferDic.ContainsKey(p_key))
        {
            return true;
        }
        return false;
    }
}
