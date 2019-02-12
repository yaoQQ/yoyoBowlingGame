OfficialIcon = {}
local this = OfficialIcon

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--图标类型
this.iconType = nil
--图标位置
this.pos = nil
--赛点名称
this.name = nil
--最大参赛距离（只有在该距离内的玩家可见到该官方赛事）
this.distance = 0
--总奖金
this.prize = 0
--活动信息
this.infoList = {}

function this:initOfficialInfo(iconType,pos,name,distance,prize,infoList)
	self.iconType = iconType
	self.pos = pos
	self.name = name
	self.distance = distance
	self.prize = prize
	self.infoList = infoList
end