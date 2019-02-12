

PlatformShopChatProxy = {}
local this=PlatformShopChatProxy


function PlatformShopChatProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformShopChatProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

this.shopChatMsgData = {}
--设置聊天信息数据
function this:addShopChatMsgData(data)
    -- this.shopChatMsgData[#this.shopChatMsgData+1] = data
    table.insert(this.shopChatMsgData,data)

    table.sort(this.shopChatMsgData,this.sortFunc)

end

function this.sortFunc(a,b)
    return a.chat_time < b.chat_time
end

--获取聊天信息数据
function this:getShopChatMsgData()
    table.sort(this.shopChatMsgData,this.sortFunc)
	return this.shopChatMsgData
end

--获取最后一个聊天信息
function this:getLastShopChatMsgData()
    if this.shopChatMsgData == nil and #this.shopChatMsgData == 0 then return end
    return this.shopChatMsgData[#this.shopChatMsgData]
end

this.secondGetRedBagData = nil
--设置领取红包界面数据
function this:setSecondGetRedBagData(data)
	this.secondGetRedBagData = data
end

--获取领取红包界面数据
function this:getSecondGetRedBagData()
	return this.secondGetRedBagData
end

this.thirdShowRedBagData = nil
--设置查看红包界面数据
function this:setThirdShowRedBagData(data)
	this.thirdShowRedBagData = data
end

--获取查看红包界面数据
function this:getThirdShowRedBagData()
	return this.thirdShowRedBagData
end

this.forthMoneyChargeData = nil
--设置充值红包界面数据
function this:setForthMoneyChargeData(data)
	this.forthMoneyChargeData = data
end

--获取充值红包界面数据
function this:getForthMoneyChargeData()
	return this.forthMoneyChargeData
end