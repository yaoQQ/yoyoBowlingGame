using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeRecorderController 
{

    public class TimeRecorder
    {
        public bool paused = false;

        public System.Object key;

        public long runningTime_ns;


        /// <summary>
        /// 忽略的时间暂停类型;
        /// </summary>
        public TimePauseType ignoreType;

        public TimeRecorder(System.Object p_key, TimePauseType p_ignoreType = TimePauseType.None)
        {
            key = p_key; ignoreType = p_ignoreType;
        }
        public bool Execute(long deltaTime_ns)
        {
            if (key==null)
            {
                return false;
            }
            paused = CheckPause();
            if (!paused)
            {
                runningTime_ns += deltaTime_ns;
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
    }

    Dictionary<System.Object, TimeRecorder> recorderDic = new Dictionary<object, TimeRecorder>();

    public void Init()
    {
        recorderDic.Clear();

    }
    public void Execute(long deltaTime_ns)
    {

        List<TimeRecorder> delList = new List<TimeRecorder>();
        foreach (TimeRecorder recorder in recorderDic.Values)
        {
            if (!recorder.Execute(deltaTime_ns))
            {
                delList.Add(recorder);
            }
        }
        foreach (TimeRecorder temp in delList)
        {
            GlobalTimeManager.Instance.timeRecorderController.RemoveRecorder(temp);
        }
        delList = null;
        
    }
    public bool AddRecorder(System.Object p_key, TimePauseType p_ignoreType = TimePauseType.None)
    {

        if (p_key == null) return false;
        if (recorderDic.ContainsKey(p_key))
        {
            Loger.PrintWarning("TimeRecorderController已存在key值：",  p_key.ToString());
            return false;
        }
        else
        {
            TimeRecorder recorder = new TimeRecorder(p_key, p_ignoreType);
            recorderDic.Add(p_key, recorder);
            return true;
        }
    }
    public bool RemoveRecorder(TimeRecorder recorder)
    {
        if (recorderDic.ContainsKey(recorder.key))
        {
            recorderDic.Remove(recorder.key);
            return true;
        }
        return false;
    }
    public bool RemoveRecorderByKey(System.Object key)
    {
        if (recorderDic.ContainsKey(key))
        {
            recorderDic.Remove(recorderDic[key]);
            return true;
        }
        return false;
    }
    public TimeRecorder GetRecorderByKey(System.Object p_key)
    {
        TimeRecorder recorder;
        recorderDic.TryGetValue(p_key, out recorder);
        return recorder;
    }
    public long GetRecorderTime(System.Object p_key,TimeUnit unit)
    {
        TimeRecorder recorder = GetRecorderByKey(p_key);
        if (recorder!=null)
        {
            switch (unit)
            {
                case TimeUnit.S:
                    return Mathf.RoundToInt(recorder.runningTime_ns * 0.0000001f);
                case TimeUnit.MS:
                    return Mathf.RoundToInt(recorder.runningTime_ns * 0.0001f);
                case TimeUnit.NS:
                    return recorder.runningTime_ns;
            }
        }
        return -1;
    }
}
