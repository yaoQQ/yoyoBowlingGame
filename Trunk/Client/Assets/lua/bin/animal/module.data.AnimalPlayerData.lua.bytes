---
--- Created by Administrator.
--- DateTime: 2018\11\15 0015 9:25
---

AnimalPlayerData = {}
local this = AnimalPlayerData

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.player_name = ""
this.player_icon = ""  -- 头像
this.player_seat = 0  -- 座位号
this.player_id = 0
this.loadProgress = 0   -- 加载进度, 0-100
this.surviveList = nil     -- 存活的动物, 初始时全部存活

---@param _info MsgRoomMemberInfo
function this:InitPlayerData(_info)
    self.player_name = _info.player_name
    self.player_icon = _info.player_icon
    self.player_seat = _info.player_seat
    self.player_id = _info.player_id
    self.loadProgress = 0
    self.surviveList = {}
    for i = 0, 7 do
        self.surviveList[i] = i
    end
    --print("surviveList: "..table.tostring(self.surviveList))
end

function this:SetLoadProgress(progress)
    self.loadProgress = progress
end

function this:GetLoadProgress()
    return self.loadProgress
end

function this:GetPlayerSeat()
    return self.player_seat
end

function this:GetPlayerName()
    return self.player_name
end

function this:GetPlayerId()
    return self.player_id
end

function this:GetPlayerIcon()
    return self.player_icon
end

function this:GetSurviveList()
    return self.surviveList
end

function this:GetSurviveById(id)
    return self.surviveList[id]
end

function this:OnDeadAnimal(id)
    --print(string.format("有棋子死了, seat: %s, AnimalId: %s", self.player_seat, id))
    self.surviveList[id] = nil
    --print("surviveList: "..table.tostring(self.surviveList))

end
