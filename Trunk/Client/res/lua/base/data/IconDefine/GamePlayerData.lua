GamePlayerData = {}
local this = GamePlayerData

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--玩家排名
this.rank = 0
--玩家头像url
this.iconUrl = nil
--玩家名字
this.name = nil
--玩家签名
this.sign = nil
--玩家性别
this.sex = 0
--玩家分数
this.score = 0
--玩家独立id
this.singleId = nil

function this:initGamePlayerData(rank,iconUrl,name,sign,sex,score,singleId)
	self.rank = rank
	self.iconUrl = iconUrl
	self.name = name
	self.sign = sign
	self.sex = sex
	self.score = score
	self.singleId = singleId
end