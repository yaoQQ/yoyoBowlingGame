BaseScene={}
local this=BaseScene
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.isInit=false
this.sceneName="重写场景名字"

function this:enter()
	SceneManager.Instance:Change(self)
end

function this:leave()
	self.isInit=false
	SceneManager.Instance:Clear()
end

function this:reset()
	SceneManager.Instance:Reset()
end

function this:getSceneName()
	return  self.sceneName
end

function this:getIsInit()
	return self.isInit
end

--结束加载后调用
function this:endInit()
	self.isInit=true
end

function this:onEnter()
	Loger.PrintError(self.sceneName," 抽象方法需重写：onEnter")
end

function this:onReset()
	Loger.PrintError(self.sceneName," 抽象方法需重写：onReset")
end

function this:onLeave()
	Loger.PrintError(self.sceneName," 抽象方法需重写：onLeave")
end

function this:getCamera( cameraName )
	return SceneManager.Instance:GetSceneEntity():GetCamera(cameraName)
end

function this:getContainer( containerName )
	return SceneManager.Instance:GetSceneEntity():GetContainer(containerName)
end

function this:getPosPoint(pointName)
	return SceneManager.Instance:GetSceneEntity():GetPosPoint(pointName)
end

function this:createSceneCell(cellName,param)
	SceneManager.Instance:CreateSceneCell(cellName,param,self.onCellCreated)
end

function this.onCellCreated(cellGameObject,param)
	Loger.PrintError(self.sceneName," 创建cell侦听方法要重写：onCellCreated")
end

function this.setEffectToCamera(effectControler,sceneCamera)
	EffectManager.SetEffectToSceneCamera(effectControler,sceneCamera)
end





