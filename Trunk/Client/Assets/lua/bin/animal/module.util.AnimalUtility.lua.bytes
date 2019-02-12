---
--- Created by Administrator.
--- DateTime: 2018\11\19 0019 15:13
---

AnimalUtility = {}
local this = AnimalUtility

function this.GetPosByGrid(x, y)
    return Vector2(AnimalConfig.FirstPos.x + x * AnimalConfig.GridDeltaX, AnimalConfig.FirstPos.y - y * AnimalConfig.GridDeltaY)
end

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
end

-- 获取指定点的四邻接点集
--[[
         o x o
         x p x
         o x o
--]]
function this.GetNeighbor(x, y)
    return
    {
      --["up"]    = {x, y - 1},
      --["down"]  = {x, y + 1},
      --["left"]  = {x - 1, y},
      --["right"] = {x + 1, y},
      [1] = {x = x, y = y - 1},
      [2] = {x = x + 1, y = y},
      [3] = {x = x, y = y + 1},
      [4] = {x = x - 1, y = y},
    }
end