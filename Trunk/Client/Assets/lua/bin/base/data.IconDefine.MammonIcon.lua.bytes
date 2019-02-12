MammonIcon = {}
local this = MammonIcon

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
--this.gameIconUrl = nil
--游戏奖金
this.prize = 0
--游戏剩余时间
this.level = 0
--游戏总人数
this.totalPeople = 0
--游戏玩家信息列表
this.playerList = nil
--商家人气
this.busiPopul=0
--用户人气
this.userPopul=0
--商家名字
this.name = name

function this:initMammonInfo(iconType,pos,singleId,prize,level,totalPeople,name,playerList)
	self.iconType = iconType
	self.pos = pos
	self.singleId = singleId
	self.prize = prize
	self.level = level
	self.totalPeople = totalPeople
    self.name = name
	self.playerList = playerList
end