AGameExecuteQueue={}

local this=AGameExecuteQueue

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    o.executeQueue=Queue:new()
    return o
end

--基础信息,对象结构
this.baseInfo={}
--待执行队列
this.executeQueue=nil
--已执行的记录
this.executedRecord={}
--阻塞标识符
this.blockSign=false

function this:unBlock()
	self.blockSign=false
end

function this:reset()
	self:unBlock()
	self.baseInfo={}
	self.executeQueue=Queue:new()
	self.executedRecord={}
end


function this:setBaseInfo(p_baseInfo)
	self.baseInfo=p_baseInfo
end

function this:addExecute(index,executeStr)
	local executedValue = self:getExecutedIndex()
	local waitExecuteValue=self:getOrderNum()
	CSLoger.printError("index： "..index)
	CSLoger.printError("executedValue "..executedValue)
	CSLoger.printError("waitExecuteValue "..waitExecuteValue)
	if executedValue+waitExecuteValue == index-1 then
		Queue.enqueue(self.executeQueue,executeStr) 
	else
		CSLoger.printError("插入的游戏执行队列位置有误： "..index)
	end
end

function this:getOrderNum()
	return self.executeQueue.count
end


function this:getExecuteOrder()
	if self.executeQueue ~= nil and self.executeQueue.count>0 and not self.blockSign then
		self.blockSign=true
		return Queue.dequeue(self.executeQueue)
	else
		return nil
	end
end

function this:pushExecuted(executeStr)
    table.insert(self.executedRecord,executeStr)	
end

function this:getExecutedIndex()
	return #self.executedRecord
end


--TODO
function this:saveSerializeRecord()
	
end