


PlatformBusiCooperPngMgrProxy={}
local this=PlatformBusiCooperPngMgrProxy


function PlatformBusiCooperPngMgrProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformBusiCooperPngMgrProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

this.userHeadTable = {}
--添加用户头像
function this:addUserHeadTable(userId,headPng)

    this.userHeadTable[userId] = headPng
    printDebug("============================更改了用户头像："..table.tostring(this.userHeadTable))
end

--获取用户id获取用户头像
function this:getUserHeadById(userId)
    return this.userHeadTable[userId]
end

this.myUserInfoPngNameTable = {}
--添加用户信息名字
function this:addUserInfoPngNameTable(pngPos,pngName)
    this.myUserInfoPngNameTable[pngPos] = pngName
end

--获取用户信息名字
function this:getUserInfoPngNameByPos(pngPos)
    return this.myUserInfoPngNameTable[pngPos]
end

this.myUserInfoPngTable = {}
--添加用户信息图片
function this:addUserInfoPngTable(pngPos,pngTexture2D)
    this.myUserInfoPngTable[pngPos] = pngTexture2D
end

--获取用户信息图片
function this:getUserInfoPngTable(pngPos)
    return this.myUserInfoPngTable[pngPos]
end