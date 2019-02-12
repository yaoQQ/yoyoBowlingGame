BusinessSingleIcon = {}
local this = BusinessSingleIcon

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--图标类型
this.iconType = nil
--浮标底座url
this.baseUrl = nil
--图标位置
this.pos = nil
--商户类型
this.type = nil
--商户简称
this.name= nil
--商户头像
this.headUrl= nil
--浮标框样式
this.bgUrl = nil
--总奖金 
this.prize = 0
--推荐度
this.recomend = 0
--商户等级
this.level = 0
--活动信息
this.infoList = {}
--距离玩家距离
this.distance = 0
--商户地址
this.address = nil
--商户店铺图片
this.shopPngs = {}
--商户独立id
this.singleId = 0

--分组（客户端自身标识，不需要服务端传数据）
this.group = 0

function this:initBusinessSingleInfo(iconType,baseUrl,pos,type,name,headUrl,bgUrl,prize,recomend,level,infoList,distance,address,shopPngs,singleId)
	self.iconType = iconType
	self.baseUrl = baseUrl
	self.pos = pos
	self.type = type
	self.name = name
	self.headUrl = headUrl
	self.bgUrl = bgUrl
	self.prize = prize
	self.recomend = recomend
	self.level = level
	self.infoList = infoList
	self.distance = distance
	self.address = address
	self.shopPngs = shopPngs
	self.singleId = singleId
end