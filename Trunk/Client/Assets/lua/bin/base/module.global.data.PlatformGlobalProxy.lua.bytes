PlatformGlobalProxy = {}
local this=PlatformGlobalProxy


function PlatformGlobalProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformGlobalProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

this.globalBaseData = nil
--设置global界面用户基本数据
function this:setGlobalBaseData(data)
    this.globalBaseData = data.user_info
end

--获取glocal界面用户基本数据
function this:getGlobalBaseData()
    return this.globalBaseData
end

--获取glocal界面用户的pid
function this:getGlobalPlayerId()
	return this.globalBaseData.player_id
end


this.globalGameData = nil
function this:setGlobalGameData(data)
    self.globalGameData  = data
end

function this:getGlobalGameData()
    return  self.globalGameData
end

--------------聊天室
--设置聊天内容
this.chatNotifyData = {}
function this:addChatNotify(shopId,notifyData)

    if this.chatNotifyData[shopId] == nil then 
        this.chatNotifyData[shopId] = {}
    end
    table.insert(this.chatNotifyData[shopId],notifyData)
end

this.currChatNotifyData = {}
--获取聊天内容
function this:getChatNotify(shopId)
    this.currChatNotifyData = this.chatNotifyData[shopId]
    return this.currChatNotifyData
end
