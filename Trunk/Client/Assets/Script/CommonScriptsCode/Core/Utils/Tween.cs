using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;

[LuaCallCSharp]
public class Tween : MonoBehaviour
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>true will play more once</returns>
    public delegate bool TweenCompleteEventHandler();
    public delegate void TweenUpdateEventHandler(float scale);
    public event TweenCompleteEventHandler OnComplete;
    public event TweenUpdateEventHandler OnUpdate;

    public AnimationCurve m_pCurve;

    private bool m_bPlaying = false;

    public float Duration = 0;
    private float m_fLastTime = 0;

    private bool m_bAutoManager = false;

    private float m_fMinValue = 0;
    private float m_fMaxValue = 1;

    /// <summary>
    /// set replay times,default one times,equals zero means loop
    /// </summary>
    public uint RepeatTimes = 1;
    private uint m_iHadRepeatTimes = 0;

    public bool Revert { get; set; }

    void Update()
    {
        if (m_pCurve == null)
        {
            m_pCurve = AnimationCurve.Linear(0, m_fMinValue, Duration, m_fMaxValue);
        }

        if (m_bPlaying)
        {
            if (m_fLastTime < Duration)
            {
                float time = Revert ? Duration - m_fLastTime : m_fLastTime;
                float scale = m_pCurve.Evaluate(time);
                if (OnUpdate != null)
                {
                    OnUpdate(scale);
                }
            }
            else
            {
                m_bPlaying = false;

                if (OnUpdate != null)
                {
                    OnUpdate(Revert ? m_fMinValue : m_fMaxValue);
                }

                bool end = true;
                m_iHadRepeatTimes++;
                if (RepeatTimes > 0)
                {
                    if (RepeatTimes > m_iHadRepeatTimes)
                    {
                        end = false;
                    }
                }
                else
                {
                    Play();
                    return;
                }
                //结束时;
                if (end)
                {
                    bool moreOnce = false;
                    if (OnComplete != null)
                    {
                        moreOnce = OnComplete();
                        if (moreOnce)
                        {
                            m_iHadRepeatTimes = RepeatTimes - 1;
                            Play();
                        }
                    }

                    if (m_bAutoManager && !moreOnce)
                    {
                        Destroy(this);
                    }
                }
            }

            m_fLastTime += Time.deltaTime;
        }
    }

    void OnDisable()
    {
        if (OnComplete != null)
        {
            OnComplete();
        }
    }
    void OnDestroy()
    {
        if (OnComplete != null)
        {
            OnComplete();
        }
    }

    public void SetCurve(AnimationCurve curve, float duration = -1)
    {
        if (duration > 0)
        {
            SetDuration(duration);
        }

        float maxTime = float.MinValue;
        float minTime = float.MaxValue;
        float maxValue = float.MinValue;
        float minValue = float.MaxValue;
        foreach (Keyframe key in curve.keys)
        {
            if (maxTime < key.time)
            {
                maxTime = key.time;
            }
            if (minTime > key.time)
            {
                minTime = key.time;
            }

            if (maxValue < key.value)
            {
                maxValue = key.value;
            }
            if (minValue > key.value)
            {
                minValue = key.value;
            }
        }

        float offsetTime = maxTime - minTime;
        offsetTime = offsetTime == 0 ? 1 : maxTime - minTime;
        float offsetValue = maxValue - minValue;
        offsetValue = offsetValue == 0 ? 1 : offsetValue;

        Keyframe[] keys = new Keyframe[curve.keys.Length];
        float tv = offsetValue / offsetTime / Duration;
        for (int i = 0; i < curve.keys.Length; i++)
        {
            Keyframe key = curve.keys[i];

            float time = (key.time - minTime) * Duration / offsetTime;
            float value = (key.value - minValue) / offsetValue;

            keys[i] = new Keyframe(time, value, key.inTangent * tv, key.outTangent * tv);
        }

        if (keys.Length > 0)
        {
            m_pCurve = new AnimationCurve(keys);

            m_pCurve.preWrapMode = curve.preWrapMode;
            m_pCurve.postWrapMode = curve.postWrapMode;
        }
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }

    public void Play()
    {
        m_bPlaying = true;
        m_fLastTime = 0;
        m_iHadRepeatTimes = 0;
    }

    public void Stop()
    {
        m_bPlaying = false;
    }

    public static Tween AutoManagerTween(GameObject go, float duration)
    {
        if (go == null)
        {
            Debug.LogError("Tween不能加载到空对象GameObject");
            return null;
        } 
        Tween tween = go.GetComponent<Tween>();
        if (tween == null)
        {
            tween = go.AddComponent<Tween>();
        }
        tween.Duration = duration;
        tween.m_bAutoManager = true;

        return tween;
    }
}