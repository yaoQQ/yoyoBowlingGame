---
--- Created by zj34.
--- DateTime: 2017/11/27 18:07
---
require "eliminate:module/eliminate/data/EliminatePlayerInfo"
require "eliminate:module/eliminate/data/EliminateRoomInfo"
require "base:enum/NoticeType"


local Loger = CS.Loger

EliminateDataProxy = {}
local this = EliminateDataProxy

this.roomInfo = nil
this.meUid = 0
this.totalPreLoadCount = 0
this.curPreLoadCount = 0
this.gameMulMode = 0
this.progress = 0

-- v2
this.playerDic = nil

function EliminateDataProxy:new(o)
    o = o or {}
    setmetatable(o,self)
    self.__index = self
    return o
end

function EliminateDataProxy:GetInstance()
    if self._instance == nil then
        self._instance = EliminateDataProxy:new()
    end
    return self._instance
end

function this:initDataProxy()
    self.roomInfo = EliminateRoomInfo:new()
    self.meUid = LoginDataProxy.playerId
    --print("初始化单例, uid: "..self.meUid)
end

function this:addMeScore(score)
    local me = self:getMe()
    me:addScore(score)
end

--===========================访问器==========================
function this:getMeScore()
    return self:getMe():getScore()
end

function this:getAllPlayerInfo()
    return self.playerDic
end

function this:getMe()
    return self.playerDic[self.meUid]
end

function this:GetMeUid()
    return self.meUid
end

function this:getGameTimerCount()
    return self.roomInfo:getCount()
end

function this:getGameProgress()
    return self.roomInfo:getCountProgress()
end

function this:getGameMode()
    return self.roomInfo:getGameMode()
end

function this:getStartTimerCount()
    return self.roomInfo:getStartTimer()
end

function this:getOverTimerCount()
    return self.roomInfo:getOverTimer()
end

function this:getRoomId()
    return self.roomInfo:getRoomId()
end

function this:getRoomMaxPlayerCount()
    return self.roomInfo:getMaxPlayerCount()
end

-- test
function this:rankAllPlayerInfo()
    --if table.empty(self.playerInfoList) then
    --    return
    --end
    --table.sort(self.playerInfoList, function (a, b)
    --    local left = a:getScore()
    --    local right = b:getScore()
    --    if left == nil or right  == nil then
    --        return false
    --    end
    --    if left == right then
    --        return false
    --    end
    --    return left > right
    --end)
    --for i = 1, #self.playerInfoList do
    --    self.playerInfoList[i]:setRank(i)
    --end
    --print("排名更新, playerInfoList = "..table.tostring(self.playerInfoList))
end

--===========================事件响应========================
function this:onReadyDataProxy()
    self.roomInfo:onReadyStartRoomInfo()
    self:getMe():onStartPlayerInfo()
end

function this:onStartDataProxy()
    self.roomInfo:onStartRoomInfo()
end

function this:onTimerCountDecreaseDataProxy()
    self.roomInfo:onTimerCountDecreaseRoomInfo()
end

function this:onOverDataProxy()
    self.roomInfo:onOverRoomInfo()
end

function this:onExitDataProxy()
    if self.playerDic ~= nil then
        for k, v in pairs(self.playerDic) do
            v:onExitPlayerInfo()
        end
    end
    if self.roomInfo ~= nil  then
        self.roomInfo:onExitRoomInfo()
    end
    self.roomInfo = nil
    self.totalPreLoadCount = 0
    self.curPreLoadCount = 0
    self.meUid = 0
    self._instance = nil
end


--============================ V2 ===============================

function this:GetMeLoadProgress()
    return self:getMe():getLoadProgress()
end

---@param msg table
function this:InitPlayerInfo(match_info_list)
    self:initDataProxy()
    self.playerDic = {}
    for _, v in pairs(match_info_list) do
        local player = EliminatePlayerInfo:new()
        player:InitPlayerData(v)
        self.playerDic[v.player_id] = player
    end
end

function this:GetAllPlayer()
    return self.playerDic
end

function this:onLoadStartDataProxy()
    local preCount = EliminatePreload:getPreLoadCount()
    local poolCount = ObjectPool:GetInstance():getPoolCount()
    self.totalPreLoadCount = preCount + poolCount
    --print("preCount = "..preCount)
    --print("poolCount = "..poolCount)
    CS.PreloadManager.Instance:ExecuteOrder(EliminatePreload)
end

function this:OnLoadStep()
    self.curPreLoadCount = self.curPreLoadCount + 1
    local progress = math.floor((self.curPreLoadCount / self.totalPreLoadCount) * 100)
    self:getMe():setLoadProgress(progress)
    if self.loadViewCro ~= nil then
        coroutine.stop(self.loadViewCro)
    end
    self.loadViewCro = coroutine.start(function ()
        while(EliminateLoadView:GetIsProSuccess() == false) do
            coroutine.step(self.loadViewCro)
        end
        EliminateNetModule.sendReqLoadingProgress(progress)
    end)

end