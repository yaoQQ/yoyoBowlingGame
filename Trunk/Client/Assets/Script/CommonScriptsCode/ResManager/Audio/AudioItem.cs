using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioItem
{

    public enum AudioState
    {
        Playing,
        Pause,
        /// <summary>
        /// 闲置;
        /// </summary>
        Idle,
        /// <summary> 
        /// 要播放的音源,还在加载中;
        /// </summary>
        Loading
    }
    /// <summary>
    /// 用于挂声音的组件;
    /// </summary>
    AudioSource audioSource;

	public AudioSource AudioSource
	{
		get
		{
			return this.audioSource;
		}
	}
    //AudioType audioType;

    public int guid;

    static int curMaxGuid;
    public static int CreateGuid()
    {
        return curMaxGuid++;
    }

    SoundClip soundClip;

    public SoundClip SoundClip
    {
        get { return soundClip; }
    }

    AudioState curState = AudioState.Idle;

    public AudioState CurState
    {
        get { return curState; }
    }

    bool isPlayed = false;

    SoundInfo soundInfo;

    public bool CheckEquals(SoundInfo p_soundInfo)
    {
        if (soundClip == null) return false;
        if(soundClip.info.packName== p_soundInfo.packName&& soundClip.info.soundName==p_soundInfo.soundName)
        {
            return true;
        }
        
        return false;
    }

    public AudioItem(AudioSource source)
    {
        audioSource = source;
        //spatialBlend 决定是不是3D音
        //audioSource.spatialBlend = 0;
        audioSource.enabled = false;
        guid = CreateGuid();
    }



    public void Play(SoundInfo info)
    {

        curState = AudioState.Loading;
        Clear();
        SoundObject soundObject = AudioManager.Instance.GetSoundObject(info.packName,info.soundName);
        soundClip = new SoundClip(info, soundObject);
        MainThread.Instance.StartCoroutine(LoadedPlay());
    }

    /// <summary>
    /// 频道内的所有声音加载完成后开始播放;
    /// </summary>
    IEnumerator LoadedPlay()
    {
        while(soundClip.soundObject.CurLoadState != SoundObject.LoadState.Loaded)
        {
            yield return 0;
        }

        audioSource.clip = soundClip.soundObject.GetAudioClip();
        audioSource.enabled = true;
        audioSource.volume = soundClip.info.volume;
        audioSource.loop = soundClip.info.isLoop;
		audioSource.mute = soundClip.info.isMute;

		audioSource.Play();
        curState = AudioState.Playing;
        

        isPlayed = true;
    }


    public void AdjustVolume()
    {
        if (curState != AudioState.Playing) return;
        if (!isPlayed) return;
        if (curState == AudioState.Playing && !audioSource.isPlaying)
        {
            AudioManager.Instance.Remove2DItem(this);
            return;
        }
        if (!soundClip.info.isThreeD)
        {
            if (soundClip.info.fadeIn_ms != 0)
            {
                float runTime = audioSource.time * 1000f;
                if (runTime < soundClip.info.fadeIn_ms)
                {

                    //渐入;
                    audioSource.volume = runTime / soundClip.info.fadeIn_ms * soundClip.info.volume;
                    //Debug.Log("渐入音量：" + audioSource.volume);
                }
            }
            if (!soundClip.info.isLoop)
            {
                if (soundClip.info.fadeOut_ms != 0)
                {
                    float runTime = audioSource.time * 1000f;
                    float fadeOutTimePos = audioSource.clip.length * 1000f - soundClip.info.fadeOut_ms;
                    if (runTime > fadeOutTimePos)
                    {
                        //淡出;
                        audioSource.volume = (1f - (runTime - fadeOutTimePos) / soundClip.info.fadeOut_ms) * soundClip.info.volume;
                        //Debug.Log("渐出音量：" + audioSource.volume);
                    }
                }
            }
        }
    }


    /// <summary>
    /// AudioManager调用;
    /// </summary>
    public void Clear()
    {
        if (audioSource != null)
        {
            audioSource.enabled = false;
            audioSource.clip = null;
        }

        //减少对应的声源引用;
        if (soundClip != null && soundClip.soundObject != null)
        {
            soundClip.soundObject.RemoveReference();
            soundClip = null;
        }

        curState = AudioState.Idle;

        isPlayed = false;
    }

    public void PauseSound()
    {
        curState = AudioState.Pause;
        if (isPlayed)
        {
            //if (soundClip.info.fadeOut_ms <= 0)
            //{
            audioSource.Pause();
            //}
            //else
            //{
            //    pausePos = audioSource.time * 1000f;
            //    PlaySoundWaitID = MainThread.Instance.StartWait(AsynPauseSound, 2);

            //}

        }
    }

    public void StopSound()
    {
        curState = AudioState.Idle;
        if (isPlayed)
        {
            audioSource.Stop();
            isPlayed = false;
        }
    }

    public void RecoverSound()
    {
        curState = AudioState.Playing;
        isPlayed = true;
        audioSource.Play();
    }

    float pausePos;
    int AsynPauseSound(int index, int waitCount, System.Object[] args)
    {
        switch (index)
        {
            case 0:
                if (audioSource.isPlaying && audioSource.volume != 0)
                {
                    float runTime = audioSource.time * 1000f;
                    float fadeOutTimePos = pausePos;
                    if (runTime > fadeOutTimePos)
                    {
                        //淡出;
                        audioSource.volume = 1f - (runTime - fadeOutTimePos) / soundClip.info.fadeOut_ms;
                        //Debug.Log("渐出音量：" + audioSource.volume);
                    }
                }
                else
                {
                    index++;
                }
                break;
            case 1:
                audioSource.Pause();
                isPlayed = false;
                index++;
                //Debug.Log("渐出音量结束");
                break;

        }
        return index;
    }
}
