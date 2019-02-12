---
--- Created by lichongzhi.
--- DateTime: 2017/11/27 17:59
---
require "eliminate:module/eliminate/EliminateConfig"

local Loger = CS.Loger
local GlobalTimeManager = CS.GlobalTimeManager

EliminateRoomInfo = {}
local this = EliminateRoomInfo
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.timerCount = 0
this.totalTimeCount = 0
this.mode = 0
this.startTimerCount = 0
this.overTimerCount = 0
this.roomId = 0
this.maxPlayerCount = 0

local StartTimerKey  = "EliminateStartTimerCount"
local OverTimerKey   = "EliminateOverTimerCount"
local GameTimerKey   = "EliminateGameTimerKey"

function this:initRoomInfoByConfig(timerCount)
    self.totalTimeCount = timerCount - EliminateConfig.START_TIMER_COUNT
    self.timerCount = timerCount - EliminateConfig.START_TIMER_COUNT
    self.mode = EliminateConfig.GameMode.TIMER
    self.startTimerCount = EliminateConfig.START_TIMER_COUNT
    self.overTimerCount = EliminateConfig.OVER_TIMER_COUNT
end

function this:getCount()
    return self.timerCount
end

function this:getCountProgress()
    return self.timerCount /  self.totalTimeCount
end

function this:getGameMode()
    return self.mode
end

function this:getStartTimer()
    return self.startTimerCount
end

function this:getOverTimer()
    return self.overTimerCount
end

function this:setRoomId(roomId)
    self.roomId = roomId
end

function this:getRoomId()
    return self.roomId
end

function this:setMaxPlayerCount(count)
    self.maxPlayerCount = count
end

function this:getMaxPlayerCount()
    return self.maxPlayerCount
end

--===========================事件响应=======================

function this:onStartRoomInfo()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(StartTimerKey)
    GlobalTimeManager.Instance.timerController:AddTimer(GameTimerKey, 1000, -1, function ()
        NoticeManager.Instance:Dispatch(EliminateNoticeType.TimerCountDecrease)
    end)
end

function this:onOverRoomInfo()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(GameTimerKey)
end

function this:onTimerCountDecreaseRoomInfo()
    if self.mode ~= EliminateConfig.GameMode.TIMER then
        return
    end
    if self.timerCount <= 0 then
        return
    end
    self.timerCount = self.timerCount - 1
    if self.timerCount == EliminateConfig.OVER_TIMER_COUNT + 1 then
        GlobalTimeManager.Instance.timerController:AddTimer(OverTimerKey, 1000, -1, function ()
            if self.overTimerCount > 0  then
                self.overTimerCount = self.overTimerCount - 1
                NoticeManager.Instance:Dispatch(EliminateNoticeType.OverTimerCount)
            end
        end)
    end

    if self.timerCount <= 0 then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey(OverTimerKey)
        self.waitOverCro = nil
        self.waitOverCro = coroutine.start(function ()
            while(EliminateGrid:GetInstance():getFillState()) do
                coroutine.step(self.waitOverCro)
            end
            NoticeManager.Instance:Dispatch(EliminateNoticeType.TimeUp)
        end)
    end
end

function this:onReadyStartRoomInfo()
    self:initRoomInfoByConfig(64)
    --self:initRoomInfoByConfig(6)
    GlobalTimeManager.Instance.timerController:AddTimer(StartTimerKey, 1000, -1, function ()
        if self.startTimerCount > 0  then
            self.startTimerCount = self.startTimerCount - 1
            if self.startTimerCount ~= 0 then
                AudioManager.playSound("eliminate", "game_start_timer")
            else
                AudioManager.playSound("eliminate", "game_formal_start")
            end
            NoticeManager.Instance:Dispatch(EliminateNoticeType.StartTimerCount)
        elseif self.startTimerCount == 0 then
            NoticeManager.Instance:Dispatch(EliminateNoticeType.GameFormalStart)
        end
    end)
end

function this:onExitRoomInfo()
    coroutine.stop(self.waitOverCro)
    self.waitOverCro = nil
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(StartTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(OverTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(GameTimerKey)
    self.timerCount = 0
    self.totalTimeCount = 0
    self.mode = 0
    self.maxPlayerCount = 0
    self.startTimerCount = 0
    self.overTimerCount = 0
    self.roomId = 0
end
