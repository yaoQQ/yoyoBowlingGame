local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield
local error = error
local unpack = unpack
local debug = debug

local index = 0

local coExample = nil
--例子
function coroutine.example()
	coExample = coroutine.start(function ()
		printDebug("协程开始时间："..Time.time)
		coroutine.step(coExample)
		printDebug("协程等待一帧后的时间："..Time.time)
		coroutine.wait(coExample, 1000)
		printDebug("协程等待一秒后的时间："..Time.time)
	end)
end

--协程开始
function coroutine.start(f, ...)
	index = index + 1
	local co = {}
	co.co = create(f)
	co.index = index
	
	if running() == nil then
		local flag, msg = resume(co.co, ...)
	
		if not flag then					
			printDebug(debug.traceback(co.co, msg))
		end					
	else
		local args = {...}
		
		local action = function()												
			local flag, msg = resume(co.co, table.unpack(args))
	
			if not flag then														
				printDebug(debug.traceback(co.co, msg))						
			end		
		end
			
		GlobalTimeManager.Instance.timerController:AddTimer("coroutine"..co.index, -1, 1, action)	
	end

	return co
end

--按时间等待(时间单位：毫秒)
function coroutine.wait(co, t, ...)
	if co == nil or co.co == nil then
		printError("coroutine == nil")
		return
	end
	
	local args = {...}
	local timer = nil
		
	local action = function()				
		local flag, msg = resume(co.co, table.unpack(args))
		
		if not flag then	
			GlobalTimeManager.Instance.timerController:RemoveTimerByKey("coroutine"..co.index)
			printDebug(debug.traceback(co.co, msg))			
			return
		end
	end
	
	GlobalTimeManager.Instance.timerController:AddTimer("coroutine"..co.index, t, 1, action)
	return yield()
end

--单步，等一帧
function coroutine.step(co, ...)
	if co == nil or co.co == nil then
		printError("coroutine == nil")
		return
	end
	
	local args = {...}	
	local timer = nil
	
	local action = function()						
		local flag, msg = resume(co.co, table.unpack(args))
	
		if not flag then																			
			printDebug(debug.traceback(co.co, msg))
			return	
		end		
	end
				
	GlobalTimeManager.Instance.timerController:AddTimer("coroutine"..co.index, -1, 1, action)
	return yield()
end

function coroutine.stop(co)
	if co == nil then
		return
	end
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("coroutine"..co.index)
	co = nil
end