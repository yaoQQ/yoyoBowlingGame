UserIcon ={}
local this = UserIcon

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
--用户名称
this.name = nil
--用户性别
this.sex = nil
--用户头像url
this.head = nil
--用户头像框url
this.bg = nil
--活跃等级度
this.activeLevel = 0
--用户等级
this.level = 0
--聊天内容
this.chatMsg = nil
--签名
this.signiture = nil
--距离玩家距离
this.distance = 0
--用户独立ID
this.singleId = 0
--用户省市(格式：广东·广州)
this.location = nil
--用户年龄
this.age = 0
--用户职业
this.job = nil
--用户身份
this.identification = nil
--用户资产
this.assest = 0
--用户红包
this.money = 0
--用户积分
this.point = 0
--用户获利
this.gain = 0
--用户4张图片
this.photos = nil

function this:initUserInfo(iconType,pos, name,sex,head,bg,activeLevel,level,chatMsg,signiture,singleId, distance, location,age,job,identification,assest,money,point,gain,photos)
	self.iconType = iconType
	self.pos = pos
	self.name = name
	self.sex = sex
	self.head = head
	self.bg = bg
	self.activeLevel = activeLevel
	self.level = level
	self.chatMsg = chatMsg
	self.signiture = signiture
	self.distance = distance
	self.location = location
	self.age = age
	self.job = job
	self.singleId = singleId
	self.identification = identification
	self.assest = assest
	self.money = money
	self.point = point
	self.gain = gain
	self.photos = photos
end
