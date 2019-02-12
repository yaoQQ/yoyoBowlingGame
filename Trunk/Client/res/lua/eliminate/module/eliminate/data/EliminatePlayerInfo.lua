---
--- Created by zj34.
--- DateTime: 2017/11/27 18:04
---

require "eliminate:module/eliminate/EliminateConfig"
require "eliminate:table/ProbabilityDataBase"

local NoticeManager = CS.NoticeManager

EliminatePlayerInfo = {}
local this = EliminatePlayerInfo
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.score = 0
this.rank = 1
this.player_name = ""
this.player_seat = 0  -- 座位号
this.player_id = 0
this.player_icon = ""  -- 头像
this.loadProgress = 0
this.sex = 0
this.scoreStateList = nil

function this:checkSpawnItem()
    if table.empty(self.scoreStateList) then
        return
    end
    local maxCount = EliminateDataProxy:GetInstance():getRoomMaxPlayerCount()
    if maxCount < 2 then
        return
    end
    local isGameStarted = EliminateGrid:GetInstance():getGameStartState()
    if isGameStarted == false then
        return
    end

    local score = self.score
    for i = 1, #ProbabilityDataBase do
        local info = ProbabilityDataBase[i]
        if score >= info.score and self.scoreStateList[i] == false then
            self.scoreStateList[i] = true
            local result = math.single_prob(info.probability)
            if result == 1 then
                local itemType = math.random(1, #ItemDataBase)
                --print(string.format("道具产生判定通过, 层数=%s,该层要求分数=%s,该层产生概率=%s,产生道具类型=%s",
                --        i,info.score,info.probability,itemType))
                NoticeManager.Instance:Dispatch(EliminateNoticeType.ItemSpawnStart, {itemType = itemType})
            else
                --print(string.format("道具产生判定不通过, 层数=%s,该层要求分数=%s,该层产生概率=%s",
                --        i,info.score,info.probability))
            end
        end
    end
end

function this:setRank(rank)
    self.rank = rank
end

function this:getRank()
    return self.rank
end

function this:setUid(uid)
    self.player_id = uid
end

function this:getUid()
    return self.player_id
end

function this:addScore(score)
    if score >= 0 then
        local target = self.score + score
        EliminateUtility.CountScroll(self.score, target, EliminateGameView:GetScoreTextWidget(), 0.4)
        self.score = target
    end
end

function this:getScore()
    return self.score
end

function this:setName(name)
    self.player_name = name
end

function this:getName()
    return self.player_name
end

function this:getIsMe()
    return false
end


function this:getSeat()
    return self.player_seat
end

function this:getPortrait()
    return self.player_icon
end

function this:setLoadProgress(progress)
    self.loadProgress = progress
end

function this:getLoadProgress()
    return self.loadProgress
end

function this:resetPlayerInfo()
    self.score = 0
    self.rank = 0
    self.player_name = ""
    self.player_seat = 0  -- 座位号
    self.player_id = 0
    self.player_icon = ""  -- 头像
    self.loadProgress = 0
    self.scoreStateList = nil
end

--===============================事件响应===================================
function this:onStartPlayerInfo()
    self.scoreStateList = {}
    for i = 1, #ProbabilityDataBase do
        self.scoreStateList[i] = false
    end
end

function this:onExitPlayerInfo()
    self:resetPlayerInfo()
end

--============================ V2 ===============================

---@param info MsgRoomMemberInfo
function this:InitPlayerData(info)
    self.player_name = info.player_name
    self.sex = info.sex
    self.player_icon = info.player_icon
    self.player_seat = info.player_seat
    self.player_id = info.player_id
    self.rank = 1
    self.loadProgress = 0
end