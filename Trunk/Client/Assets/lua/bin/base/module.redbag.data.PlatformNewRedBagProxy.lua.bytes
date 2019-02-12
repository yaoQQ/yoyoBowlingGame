require "base:module/global/view/PlatformGlobalView"

PlatformNewRedBagProxy = {}
local this = PlatformNewRedBagProxy

function PlatformNewRedBagProxy:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function PlatformNewRedBagProxy:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
        --初始化一下
        self:init()
    end

    return self._instance
end

function this:init()
end
-------------------------------------------------------------------------------
this.stealRedBagData = nil
this.stealRedBagNum = 0

--设置可偷好友红包数据
function this:setStealRedBagData(data)
    this.stealRedBagData = data
    -- 对于player_id为秘书, 从数据源改变头像url
    if not table.empty(this.stealRedBagData) then
        for _, v in pairs(this.stealRedBagData) do
            if v.player_id == 0 then
                v.header = string.format("%s/%s",ImageType.UserHead, "secretary")
            end
        end
    end
end

--获取可偷好友红包数据
function this:getStealRedBagData()
    return this.stealRedBagData
end

--设置可偷好友红包次数
function this:setStealRedBagNum(data)
    this.stealRedBagNum = tonumber(data)
end

--获取可偷好友红包次数
function this:getStealRedBagNum()
    return this.stealRedBagNum
end
---------------------------------------------------------------------------------
this.myselfRedBagData = nil

--设置自己的红包数据
function this:setMyselfRedBagData(data)
    this.myselfRedBagData = data
end

--获取自己的红包数据
function this:getMyselfRedBagData()
    return this.myselfRedBagData
end
-------------------------------------------------------------------------------------
this.robbedOnlineRedPacketData = nil

--设置自己的红包数据
function this:setRobbedOnlineRedPacketData(data)
    this.robbedOnlineRedPacketData = data
end

--获取自己的红包数据
function this:getRobbedOnlineRedPacketData()
    return this.robbedOnlineRedPacketData
end

-------------------------------------------------------------------------------------
this.redPacketMailList = nil

--设置自己的红包消息数据
function this:setRedPacketMailList(index, data)
    if index == 0 then
        this.redPacketMailList = {}
    end
    if data then
        for i = 1, #data do
            table.insert(this.redPacketMailList, data[i])
        end
    end
end
--插入自己的红包消息数据
function this:insertRedPacketMailList(data)
    if data then
        data.award_flag = false
        for i = 1, #this.redPacketMailList do
            if tonumber(data.id) == tonumber(this.redPacketMailList[i].id) then
                return
            end
        end
        table.insert(this.redPacketMailList, data)
    end
end
--删除自己的红包消息数据
function this:delRedPacketMailList(mailId)
    if id then
        for i = 1, #this.redPacketMailList do
            if tonumber(mailId) == tonumber(this.redPacketMailList[i].id) then
                table.remove(this.redPacketMailList, i)
                break
            end
        end
    end
end
--获取自己的红包消息数据
function this:getRedPacketMailList()
    return this.redPacketMailList
end

--移除最后一位
function this:removeRedPacketMailListLast()
    table.remove(this.redPacketMailList, #this.redPacketMailList)
end
