OtherIcon = {}
local this = OtherIcon

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
--备注
this.memo = nil

function this:initOtherIcon(iconType,pos,memo)
	self.iconType = iconType
	self.pos = pos
	self.memo = memo
end