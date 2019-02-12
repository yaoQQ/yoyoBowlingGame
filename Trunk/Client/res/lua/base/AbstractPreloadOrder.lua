PreloadStyle={
	None=-1,
	--全屏加载条
	FullLoadingBar=1,
	--光圈加载
	Annulus=2
}

AbstractPreloadOrder={}
local this=AbstractPreloadOrder

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end


this.preloadStyle=PreloadStyle.None
this.uiPreloadList=nil
this.scenePreload=nil

function this:getPreloadStyle()
	return self.preloadStyle
end

function this:getUIPreload()
	return self.uiPreloadList
end

function this:getScenePreload()
	return self.scenePreload
end

function this:getPreLoadCount()
	local uiCount = 0
	local sceneCount = 0
	if table.empty(self.uiPreloadList) == false then
		uiCount = #self.uiPreloadList
	end
	if table.empty(self.scenePreload) == false then
		sceneCount = 1
	end
	return uiCount + sceneCount
end

function this:onPreloadEnd()
	CSLoger.printError("预加载完成,需重写:",self)
end

function this:onPreloadStepEnd()
    CSLoger.printError("预加载单步完成,需重写:",self)
end


