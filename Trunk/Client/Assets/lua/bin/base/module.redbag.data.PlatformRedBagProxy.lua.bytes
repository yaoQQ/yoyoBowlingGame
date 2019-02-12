
PlatformRedBagProxy={}
local this=PlatformRedBagProxy


function PlatformRedBagProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformRedBagProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

---------------------------------------------------------------
this.mainBaseData = nil     --红包数据
this.mainBaseDataId = nil   --红包id
this.gameScore = 0          --游戏分数
this.redBagAwardData = {}   --红包奖励
local params={}
this.endData = {}
this.CDData = {}

--通过id来获取红包数据
-- function this:getSingleDataById(redBagId)
--     if self.allRedBagData == nil then return end

--     for i=1,#self.allRedBagData do
--         if tostring(self.allRedBagData[i].singleId) == redBagId then
--             self.mainBaseData = self.allRedBagData[i]
--             return self.allRedBagData[i]
--         end
--     end
-- end

--设置当前点击的红包id
function this:setSingleDataId(id)
    this.mainBaseDataId = id
end

--获取当前点击的红包id
function this:getSingleDataId()
    return this.mainBaseDataId
end

--设置当前红包游戏分数
function this:setGameScore(score)
    this.gameScore = score
end

--获取当前红包游戏分数
function this:getGameScore()
    return this.gameScore
end

--点击红包浮标之后请求服务器数据
function this:setSingleData(data)
    this.mainBaseData = data
end

--获取红包数据
function this:getRedBagMainBase()
    return this.mainBaseData
end

function this:SetOpenRedBagData(data)
this.openData = data
end



-- this.allRedBagData = nil
--从iconproxy设置所有红包数据
function this:setAllRedBagData(data)
    this.allRedBagData = data
    --printDebug("proxy收到的设置数据为："..table.tostring(data))
end

--获取所有红包数据
function this:getAllRedBagData()
    return this.allRedBagData
end

--加入红包奖励数据
function this:setRedBagAward(data)
    this.redBagAwardData = data
end

--获取红包奖励数据
function this:getRedBagAward()
    return this.redBagAwardData
end

--游戏完了之后收到游戏奖励通知
function this:addRedBagAward(data)
    if this.redBagAwardData == nil then return end
    table.insert(this.redBagAwardData,data)
end

--删除红包奖励数据
function this:removeRedBagAward(id)
    for i=1,#this.redBagAwardData do
        if this.redBagAwardData[i] ~= nil and this.redBagAwardData[i].id == id then
            this.redBagAwardData[i] = nil
        end
    end
end

this.redBagListData = nil
--设置领取红包列表人的数据
function this:setRedBagListData(data)
    
    this.redBagListData = data
end

--获取领取红包列表人的数据
function this:getRedBagListData()
    return this.redBagListData
end


function this:setOpenRedBagData(key,value)
    params[key]=value
end

function this:getOpenRedBagData(key)
   return params[key]
end

function this:setRedBagEndData(data)
    this.endData = data
end

function this:getRedBagEndData()
   return this.endData
end


--PlatformRedBagProxy.SetOpenRedBagData = setOpenRedBagData
--PlatformRedBagProxy.GetOpenRedBagData = getOpenRedBagData

function this:setCDData(data)
    this.CDData = data
end

function this:getCDData()
    return this.CDData
end