---
--- Created by Administrator.
--- DateTime: 2018\11\19 0019 15:13
---

EliminateUtility = {}
local this = EliminateUtility
local croList = {}
--数字滚动
function this.CountScroll(origin, target, textWidget, time)
    local count = 0
    local maxCount = 60
    local oldValue = origin
    local newValue = target
    --print("oldValue: "..oldValue)
    --print("newValue: "..newValue)
    if time <= 0 then
        time = 0
    end
    local scrollCro = nil
    scrollCro = coroutine.start(function ()
        for t = 0, time, Time.deltaTime do
            count = count + 1
            local curCount = count - 10
            if curCount < 0 then
                curCount = 0
            end
            textWidget.text = math.floor((newValue - oldValue) * curCount / (maxCount - 10)) + oldValue
            coroutine.step(scrollCro)
        end
        textWidget.text = tostring(newValue)
    end)
    --table.insert(croList, scrollCro)
    --print("croList: "..table.tostring(croList))
    --EliminateUtility.StartCoroutine(scrollCro, function()
    --    for t = 0, time, Time.deltaTime do
    --        count = count + 1
    --        local curCount = count - 10
    --        if curCount < 0 then
    --            curCount = 0
    --        end
    --        textWidget.text = math.floor((newValue - oldValue) * curCount / (maxCount - 10)) + oldValue
    --        coroutine.step(scrollCro)
    --    end
    --    textWidget.text = tostring(newValue)
    --end)
end

EliminateUtility.StartCoroutine = function(cro, action)
    cro = coroutine.start(action)
    table.insert(croList, cro)
end


