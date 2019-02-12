require "base:action/BaseAction"

BaseActionGroup={}
local this=BaseActionGroup

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.groupName=""
this.actionList={}

this.runningSign=true

function this:init()
	self.runningSign=true
	self.actionList={}
	self:configSetp()
end

function this:configSetp()
	--需重写
	
end

function this:addSetp(action_instance)
	table.insert(self.actionList,action_instance)
end

function this:getRunningSign( )
	return self.runningSign
end

function this:execute(deltaTime_ms)
	local curAction=nil
	local lastAction=nil
	self.runningSign=false
	for i=1,#self.actionList do
		 curAction= self.actionList[i]
		 --激活action
		 if curAction.playState==ActionPlayState.Unactivated then  
		 	if lastAction==nil then
		 		curAction:changeState(ActionPlayState.Ready)
		 	else
		 		if lastAction.playState==ActionPlayState.Over then
		 			curAction:changeState(ActionPlayState.Ready)
		 		end
		 	end
		 end
		 curAction:execute(deltaTime_ms)
		 if curAction.playState~=ActionPlayState.Over then
		 	self.runningSign=true
		 end
		 lastAction=curAction
	end
end

