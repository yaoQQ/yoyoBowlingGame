BowlingEZReplayManager={}
local this=BowlingEZReplayManager--回放录像管理

this.isRecording=false --是否在录像
this.gameObj = nil
function BowlingEZReplayManager.init()
	--EZReplayManager.close()
	printDebug("<color='red'>begain BowlingEZReplayManager.init()  EZReplayManager="..tostring(EZReplayManager).."</color>")
	--this.gameObj=GameObject("BowlingEZReplayManager")
	--this.gameObj:AddComponent("EZReplayManager")
	--EZReplayManager.singleton
	printDebug("<color='red'>end BowlingEZReplayManager.init()  EZReplayManager.singleton="..tostring(EZReplayManager.singleton).."</color>")
	
	printDebug("<color='red'>end BowlingEZReplayManager.init() EZReplayManager.record="..tostring(EZReplayManager.record).."</color>")
end
--开始录像
function BowlingEZReplayManager.record()
	if this.isRecording then
		return
	end
	EZReplayManager.record()
	--showFloatTips("开始录像")
	printDebug("BowlingEZReplayManager.record() 开始录像 ")
	this.isRecording = true
end

--停止录像
function BowlingEZReplayManager.stop()
	EZReplayManager.stop()
	--showFloatTips("停止录像")
	printDebug("BowlingEZReplayManager.stop() 停止录像 ")
	this.isRecording = false
end

--播放录像 speed:播放速度 0正常  playImmediately是否直接播放，backwards 是否回退播放，exitOnFinished播放结束是否退出
function BowlingEZReplayManager.play(speed,playImmediately,backwards,exitOnFinished)
	printDebug("BowlingEZReplayManager.play() 开始播放录像 ")
	EZReplayManager.stop()
	EZReplayManager.play(speed,playImmediately,backwards,exitOnFinished)
--	showFloatTips("播放录像")
	NoticeManager.Instance:Dispatch(BowlingEvent.recordPlaying)
	printDebug("BowlingEZReplayManager.play() 开始播放录像 ")
end

--暂停播放
function BowlingEZReplayManager.pause()
	EZReplayManager.pause()
	printDebug("BowlingEZReplayManager.pause() 暂停 ")
end

--注册对象为录像对象
function BowlingEZReplayManager.mark4Recording(obj)
	EZReplayManager.mark4Recording(obj,false)
end


--[[--播放 速度  0 正常 2加快  -2 变慢
function BowlingEZReplayManager.playSpeed(speed)
	EZReplayManager.stop()
	EZReplayManager.play(speed)
	showFloatTips("播放录像")
	NoticeManager.Instance:Dispatch(BowlingEvent.recordPlaying)
end--]]

--暂停播放
function BowlingEZReplayManager.close()
	EZReplayManager.close()
	NoticeManager.Instance:Dispatch(BowlingEvent.recordEnd)
--	showFloatTips("@@@播放录像结束")
	printDebug("BowlingEZReplayManager.close() 播放录像结束 ")
	this.isRecording = false
end

--消除对象
function BowlingEZReplayManager.dispose()
	EZReplayManager.dispose()
end