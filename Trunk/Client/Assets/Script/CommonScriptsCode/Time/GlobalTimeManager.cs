using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XLua;

[LuaCallCSharp]
public class GlobalTimeManager : Singleton<GlobalTimeManager>
{


    #region 同步后端世界时间

    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

    DateTime worldTime;

    public void SynchronizationTime(string timeStamp)
    {
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        worldTime = dtStart.Add(toNow);
    }

    public DateTime GetDateTimebyTimeStamp(long timeStamp)
    {
        TimeSpan toNow = new TimeSpan(timeStamp);
        return  dtStart.Add(toNow);
    }

   

    public TimeSpan GetInverseTimeSpan(DateTime targetDT)
    {
        TimeSpan inverseTimeSpan = new TimeSpan(targetDT.Ticks - worldTime.Ticks);
        return inverseTimeSpan;
    }

    public bool CheckTimeOut(DateTime targetDT)
    {
        return targetDT.Ticks < worldTime.Ticks;
    }

    public enum TimeAreaEnum
    {
        Early_Time,
        IN_Time,
        Time_Out
    }

    public TimeAreaEnum CheackTimeArea(string startTime, string endTime)
    {
        if (startTime == "0:00" && endTime == "24:00") return TimeAreaEnum.IN_Time;
        if (worldTime==null)  return TimeAreaEnum.IN_Time;
        DateTime startDT = Convert.ToDateTime(startTime);
        if (endTime == "24:00") endTime = "23:59";
        DateTime endDT = Convert.ToDateTime(endTime);
        if (DateTime.Compare(worldTime, startDT) < 0) 
        {
            return TimeAreaEnum.Early_Time;
        }else if(DateTime.Compare(worldTime, startDT) >0&&DateTime.Compare(worldTime, endDT) < 0)
        {
            return TimeAreaEnum.IN_Time;
        }
        else if (DateTime.Compare(worldTime, endDT) > 0)
        {
            return TimeAreaEnum.Time_Out;
        }
        return TimeAreaEnum.IN_Time;
    }


    #endregion

    bool initSign = false;





    TimerController _timerController;

    TimePauseType pauseType = TimePauseType.None;

    public TimerController timerController
    {
        get { return _timerController; }
    }


    TimeRecorderController _timeRecorderController;

    public TimeRecorderController timeRecorderController
    {
        get { return _timeRecorderController; }
    }

    TimePauseType curPauseType = TimePauseType.None;

    public TimePauseType CurPauseType
    {
        get { return curPauseType; }
    }

    float duplicateTime;

    public void Init()
    {
        initSign = true;


        _timerController = new TimerController();
        _timerController.Init();

        _timeRecorderController = new TimeRecorderController();
        _timeRecorderController.Init();

    }

    public void PauseGame()
    {
      
        //_timerController部分
        //_timeRecorderController部分
    }

    public void resumeGame()
    {
       
        //_timerController部分
        //_timeRecorderController部分
    }

    public long oldTicks;
    public float Execute()
    {

        if (!initSign)
        {
            Debug.LogWarning("GlobalTimeManager还没初始化");
            return 0;
        }
        if (_timerController == null)
        {
            Debug.LogWarning("如果输出这句，请联系大嘴");
            return 0;
        }
        if (oldTicks == 0)
        {
            oldTicks = System.DateTime.Now.Ticks;
            return 0;
        }
        //System.DateTime.Now.Ticks 以100纳秒为单位;
        //1秒=1,000,000,000 纳秒(ns) ;
        long newTicks = System.DateTime.Now.Ticks;
        long deltaTime_ns = (newTicks - oldTicks);

        TimeSpan toNow = new TimeSpan(deltaTime_ns);
        worldTime = worldTime.Add(toNow);

        //卡机时，帧值。为最低值;
        if (deltaTime_ns * 0.0001f >= TimerController.TimerMinInterval_ms)
        {
            deltaTime_ns = (TimerController.TimerMinInterval_ms - 1) * 10000;
        }
        _timerController.Execute(deltaTime_ns);
        _timeRecorderController.Execute(deltaTime_ns);
        oldTicks = newTicks;
        //Debug.Log("============>  "+deltaTime_ns / 10000f);
        return deltaTime_ns/ 10000f;
    }
    public void DeferExecute()
    {

        if (!initSign)
        {
            Debug.LogWarning("GlobalTimeManager还没初始化");
            return;
        }
        if (_timerController == null)
        {
            Debug.LogWarning("如果输出这句，请联系大嘴");
            return;
        }
        if (oldTicks == 0)
        {
            oldTicks = System.DateTime.Now.Ticks;
            return;
        }
        long newTicks = System.DateTime.Now.Ticks;
        long deltaTime_ns = (newTicks - oldTicks);
        //卡机时，帧值。为最低值;
        if (deltaTime_ns * 0.0001f >= TimerController.TimerMinInterval_ms)
        {
            deltaTime_ns = (TimerController.TimerMinInterval_ms - 1) * 10000;
        }

        _timerController.DeferExecute(deltaTime_ns);
    }

    /// <summary>
    /// 进入副本后调用;
    /// </summary>
    public void resetDuplicateTime()
    {
        duplicateTime = System.DateTime.Now.Ticks;
    }

    public float DupStartTime
    {
        get
        {
            return duplicateTime * 0.0001f;
        }
    }
    public float GetDuplicateTime()
    {
        return (System.DateTime.Now.Ticks - duplicateTime) * 0.0001f;
    }
}
