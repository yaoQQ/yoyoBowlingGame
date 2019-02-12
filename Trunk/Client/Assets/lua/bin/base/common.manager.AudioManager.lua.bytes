AudioManager = {}

--播放背景音乐
function AudioManager.playBGM(packName, soundName)
	CSAudioManager.Instance:PlayBgMusic(packName, soundName)
end

--停止背景音乐
function AudioManager.stopBGM()
	CSAudioManager.Instance:StopBgMusic()
end

--调节背景音乐音量
function AudioManager.changeBgmVolume(value)
	CSAudioManager.Instance:ChangeBgmVolume(value)
end

--获取背景音乐音源
function AudioManager.getBGMAudioSource()
    return CSAudioManager.Instance:GetAudioSource()
end

--播放音效
function AudioManager.playSound(packName, soundName)
	CSAudioManager.Instance:Play2D(packName, soundName)
end

--调节音效音量
function AudioManager.changeSoundVolume(value)
	CSAudioManager.Instance:ChangeSoundVolume(value)
end

--预加载（一般用于背景音乐）
function AudioManager.preload(packName, soundName, finishCallBack) 
	CSAudioManager.Instance:Preload(packName, soundName, finishCallBack)
end


--------------------录音相关--------------------
--登录呀呀云
--参数1：nickname  昵称
--参数2：uid  唯一id
function AudioManager.loginRecord(nickname, uid)
	CSRecordManager.LoginRecord(nickname, uid)
end

--开始录音
--参数1：fileName  录音文件名
function AudioManager.startRecord(fileName)
	CSRecordManager.StartRecord(fileName)
end

--停止录音
--停止录音后会派发Notice：Record_End
--Notice参数格式：
--成功：0|文件名|录音时长（毫秒）|本地路径|网络路径
--录音失败：1|文件名|错误说明
--上传录音失败：2|文件名|错误说明
function AudioManager.stopRecord()
	CSRecordManager.StopRecord()
end

--开始播放本地录音
--参数1：filePath  录音本地路径
function AudioManager.startPlayRecordByFilePath(filePath)
	CSRecordManager.StartPlayRecordByFilePath(filePath)
end

--开始播放网络录音
--参数1：url  录音网络路径
function AudioManager.startPlayRecordByUrl(url)
	CSRecordManager.StartPlayRecordByUrl(url)
end

--停止播放录音
function AudioManager.stopPlayRecord()
	CSRecordManager.StopPlayRecord()
end