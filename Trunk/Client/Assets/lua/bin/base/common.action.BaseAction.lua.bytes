ActionPlayMoment=
{
	--立即播放
	AtOnce=1,
	--激活后隔多少毫秒
	Runtime_MS=2,
	--激活后事件触发
	ActionEvent=3
}

ActionPlayState=
{
	--未激活
	Unactivated=1,
	--准备中
	Ready=2,
	--运行时
	Running=3,
	--结束
	Over=4
}


BaseAction={}
local this=BaseAction

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.actionName=""

this.playMoment=ActionPlayMoment.AtOnce

this.play_need_runtime_ms=0

this.play_event=""

this.execute_total_time_ms=0


this.playState=ActionPlayState.Unactivated

--group里调用激活
function this:activate()
	self:changeState(ActionPlayState.Ready)
end


function this:changeState(state)
	if self.playState==state then
		return
	end
	self.playState=state
	if self.playState==ActionPlayState.Ready then
		self:onReady()
	elseif self.playState==ActionPlayState.Over then
		self:onOver()
	elseif self.playState==ActionPlayState.Running then
		--第一次run
		self:onStartRun()
	end
end

function this:onReady()
	Loger.PrintWarning(self.ModuleName,"Action 继承方法可以重写：onReady")
end

function this:onStartRun()
	Loger.PrintWarning(self.ModuleName,"Action 继承方法可以重写：onStartRun")
end

function this:onRunning()
	Loger.PrintWarning(self.ModuleName,"Action 继承方法可以重写：onRunning")
end

function this:onOver()
	Loger.PrintWarning(self.ModuleName,"Action 继承方法可以重写：onOver")
end

function this:execute(time_increment)
	if self.playState== ActionPlayState.Ready then
		self.execute_total_time_ms=self.execute_total_time_ms+time_increment
		if self:checkStartPlay() then
			--检测是否可开启成为运行状态
			self:changeState(ActionPlayState.Running)
		end
	elseif self.playState== ActionPlayState.Running then
		self.execute_total_time_ms=self.execute_total_time_ms+time_increment
		self:onRunning()
	end	

end

function this:executeActionEvent(event_name)
	if event_name==self.play_event then
		if self.playState== ActionPlayState.Ready then
			self:changeState(ActionPlayState.Running)
		end
	end
	self:onActionEvent(event_name)
end

--可重写
function this:onActionEvent(event_name)
	
end

function this:checkStartPlay()
	local switch = {
	    [ActionPlayMoment.AtOnce] = function()
	    	return true
	    end,
	    [ActionPlayMoment.Runtime_MS] = function()
    		if self.execute_total_time_ms>=self.play_need_runtime_ms then
    			return true
    		else
    			return false
    		end
	    end,
	     [ActionPlayMoment.ActionEvent] = function()
	    	return false
	    end
	} 
    local fSwitch = switch[self.playMoment] --switch func  
	if fSwitch then --key exists  
	   return fSwitch() --do func  
	end
end

