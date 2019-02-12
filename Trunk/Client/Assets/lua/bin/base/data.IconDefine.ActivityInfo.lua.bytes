ActivityInfo = {}
local this = ActivityInfo

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--参与人数
this.playerCount = 0
--活动优先级
this.preference = 0
--活动游戏图标url
this.iconUrl = nil
--活动名称
this.name = nil
--游戏开放时间
this.openTime = nil
--游戏奖品价值
this.prize = 0
--游戏id
this.id = 0

function this:initActivityInfo(playerCount,preference,iconUrl,name,openTime,prize,id)
	self.playerCount = playerCount
	self.preference = preference
	self.iconUrl = iconUrl
	self.name = name
	self.openTime = openTime
	self.prize = prize
	self.id = id
end
