PlatformMallProxy = {}
local this = PlatformMallProxy

local myId

function PlatformMallProxy:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function PlatformMallProxy:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
        --初始化一下
        self:init()
    end

    return self._instance
end

this.MallData = nil
this.CountNum = 0

--设置消息数据
function this:setMallData(data)
    this.MallData = data
end

--获取消息数据
function this:getMallData()
    return this.MallData
end

function this:setCountNum()
    this.CountNum = 1
end

function this:getCountNum()
    return this.CountNum
end
-------

