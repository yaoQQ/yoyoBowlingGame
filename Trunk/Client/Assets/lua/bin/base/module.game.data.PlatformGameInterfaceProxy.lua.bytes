



PlatformGameInterfaceProxy={}
local this=PlatformGameInterfaceProxy


function PlatformGameInterfaceProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformGameInterfaceProxy:GetInstance()  
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
this.gameInterfaceData = {}     --游戏通用界面信息

--从iconproxy设置游戏通用界面数据
function this:setGameInterfaceData(data)
    this.gameInterfaceData = data
    printDebug("proxy收到的设置数据为："..table.tostring(data))
end

--获取游戏通用界面数据
function this:getGameInterfaceData()
    return this.gameInterfaceData
end
