RedBagIcon = {}
local this = RedBagIcon

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
--红包独立id
this.singleId = nil
--游戏图片url
this.gameIconUrl = nil
--游戏奖金
this.prize = 0
--游戏剩余时间
this.leftTime = 0
--游戏总人数
this.totalPeople = 0
--游戏玩家信息列表
this.playerList = nil
--游戏id
this.gameId = 0

function this:initRedBagInfo(iconType,pos,singleId,gameIconUrl,prize,leftTime,totalPeople,playerList,gameId)
	self.iconType = iconType
	self.pos = pos
	self.singleId = singleId
	self.gameIconUrl = gameIconUrl
	self.prize = prize
	self.leftTime = leftTime
	self.totalPeople = totalPeople
	self.playerList = playerList
	self.gameId = gameId
end