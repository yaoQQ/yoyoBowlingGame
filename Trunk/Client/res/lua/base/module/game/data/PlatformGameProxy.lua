



PlatformGameProxy={}
local this=PlatformGameProxy


function PlatformGameProxy:new(o)
    o = o or {}
    setmetatable(o,self)
    self.__index = self
    return o
end


function PlatformGameProxy:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
    end
    return self._instance
end

this.GameInfoType =
{
    Headline = 1,
    NewGame = 2,
    HotGame = 3,
}
---------------------------------------------------------------
this.headInfoList = nil     -- 头条游戏信息
this.newInfoList = nil     -- 新游戏信息
this.hotInfoList = nil     -- 热门游戏信息

function this:updateGameInfo(info, infoType)
    if table.empty(info) then
        return
    end
    if infoType == this.GameInfoType.Headline then
        self.headInfoList = info
    elseif infoType == this.GameInfoType.NewGame then
        self.newInfoList = info
    elseif infoType == this.GameInfoType.HotGame then
        self.hotInfoList = info
    else
        error("不存在的GameInfoType")
    end

end

function this:getInfoListByType(infoType)
    local list = nil
    if infoType == this.GameInfoType.Headline then
        list = self.headInfoList
    elseif infoType == this.GameInfoType.NewGame then
        list = self.newInfoList
    elseif infoType == this.GameInfoType.HotGame then
        list = self.hotInfoList
    else
        error("不存在的GameInfoType")
    end
    return list
end


