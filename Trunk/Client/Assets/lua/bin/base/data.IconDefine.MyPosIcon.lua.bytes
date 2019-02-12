MyPosIcon = {}
local this = MyPosIcon

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
--我的性别
this.sex = 0
--我的头像
this.headUrl = nil
--我的id
this.player_id = 0
--我的账号名称
this.account_name = nil
--我的昵称
this.nickName = nil
--我的等级
this.level = 0
--我的活跃等级
this.active_level = 0
--我的签名
this.sign = nil
--我的图片
this.photos = {}
--我的地址
this.area = nil
--我的出生日期
this.birthday = 0
--我的职业
this.profession = nil
--我的资产
this.properties = 0
--我的红包
this.red_packets = 0
--我的获利
this.reap_profile = 0
--我的手机号
this.phoneNumber = nil


function this:initMyPosIcon(iconType,pos,sex,headUrl,player_id,account_name,nickName,level,active_level,sign,photos,area,birthday,profession,properties,red_packets,reap_profile,phoneNumber)
	self.iconType = iconType
	self.pos = pos
	self.sex = sex
	self.headUrl = headUrl
	self.player_id = player_id
	self.account_name = account_name
	self.nickName=nickName
	self.level=level
	self.active_level=active_level
	self.sign=sign
	self.photos=photos
	self.area=area
	self.birthday=birthday
	self.profession=profession
	self.properties=properties
	self.red_packets=red_packets
	self.reap_profile=reap_profile
	self.phoneNumber=phoneNumber
end