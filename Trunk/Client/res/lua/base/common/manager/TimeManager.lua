TimeManager = {}

TimeManager.isStartHeartbeat = false

function TimeManager.startHeartbeat()
	if TimeManager.isStartHeartbeat then
		return
	end
	LoginModule.sendReqHeartbeat()
	GlobalTimeManager.Instance.timerController:AddTimer("Heartbeat", 30000, -1, LoginModule.sendReqHeartbeat)
	TimeManager.isStartHeartbeat = true
end

function TimeManager.setHeartbeatServerTime(heartbeatServerTime)
	CSTimeManager.SetHeartbeatServerTime(heartbeatServerTime)
end

--获取服务器时间(DateTime)
function TimeManager.getServerDateTime()
	return CSTimeManager.GetServerDateTime()
end

--获取本地时间(DateTime)
function TimeManager.GetlocalTime()
	return CSTimeManager.GetlocalTime()
end

--获取服务器时间(格林威治时间)
function TimeManager.getServerUnixTime()
	return CSTimeManager.GetServerUnixTime()
end

--根据UnixTime获取DateTime
function TimeManager.getDateTimeByUnixTime(unixTime)
	return CSTimeManager.GetDateTimeByUnixTime(unixTime)
end

--根据DateTime获取UnixTime
function TimeManager.getUnixTimeByDateTime(dateTime)
	return CSTimeManager.GetUnixTimeByDateTime(dateTime)
end

--根据秒数格式化为 时:分:秒
function TimeManager.formatHourMinSecFromSec(secTime)
	local hour = math.floor(secTime / 3600)
	local min = math.floor((secTime - hour * 3600) / 60)
	local sec = secTime - hour * 3600 - min * 60
	
	local function full(num)
        if num < 10 then
            return string.concat("0", num)
        end
        return tostring(num)
    end
	
    return string.concat(full(hour), ":", full(min), ":", full(sec))
end