---
--- Created by lichongzhi.
--- DateTime: 2017/9/21 19:35
--- 坐标系为屏幕坐标系, 即左上角为(0,0)


EliminateExtension = {}
local this = EliminateExtension

-- 获取指定点的二环四邻接点集
--[[
        o o x o o
        o o x o o
        x x p x x
        o o x o o
        o o x o o
--]]
function this:getNeighbor(x, y)
    return
    {
        -- H方向
        { x - 1, y},{x - 2, y},{x + 1, y}, {x + 2, y},
        -- V方向
        { x, y - 1}, {x, y - 2},{x, y + 1}, {x, y + 2},
    }
end
-- 获取指定点的一环八邻接点集
--[[
         x x x
         x p x
         x x x
--]]
function this:getNeighbor_Eight(x, y)
    return
    {
        -- 逐行
        { x - 1, y - 1},{x , y - 1},{x + 1, y - 1},
        { x - 1, y},{x + 1, y},
        {x - 1, y + 1},{x, y + 1},{ x + 1, y + 1},
    }
end
-- 获取指定点的一环八邻接外嵌一环四邻接点集
--[[
         x x x
       x x x x x
       x x p x x
       x x x x x
         x x x
--]]
function this:getNeighbor_Eight_Cross(x, y)
    return
    {
    -- 逐行
        {x - 1, y - 2}, {x , y - 2},{x + 1, y - 2},
        {x - 2, y - 1}, {x - 1, y - 1},{x , y - 1},{x + 1, y - 1},{x + 2, y - 1},
        {x - 2, y}, {x - 1, y},{x + 1, y}, {x + 2 , y},
        {x - 2, y + 1}, {x - 1, y + 1},{x , y + 1},{x + 1, y + 1},{x + 2, y + 1},
        {x - 1, y + 2}, {x , y + 2},{x + 1, y + 2},
    }
end
-- 获取指定点的一环四邻接点集
--[[
         o x o
         x p x
         o x o
--]]
function this:getNeighbor_Four(x, y)
    return
    {
        -- H
        { x - 1, y},{x + 1, y },
        -- V
        { x, y - 1},{x, y + 1},
    }
end
--[[
        o o x o o
        o x o x o
        x o p o x
        o x o x o
        o o x o o
--]]
function this:getNeighbor_Four_2(x, y)
    return
    {
    -- Up
        {x, y - 2}, {x - 1, y - 1}, {x + 1, y - 1},
    -- H
        {x - 2, y}, { x + 2, y},
    -- Down
        {x - 1, y + 1}, {x + 1, y + 1}, {x, y + 2}
    }
end
--[[
        o o o x o o o
        o o x o x o o
        o x o o o x o
        x o o p o o x
        o x o o o x o
        o o x o x o o
        o o o x o o o
--]]
function this:getNeighbor_Four_3(x, y)
    return
    {
        -- Up
        {x, y - 3},{x - 1, y - 2}, {x + 1, y - 2}, {x - 2, y - 1}, {x + 2, y - 1},
        -- H
        {x - 3, y}, { x + 3, y},
        -- Down
        {x - 2, y + 1}, {x + 2, y + 1}, {x - 1, y + 2}, {x + 1, y + 2}, {x, y + 3}
    }
end
function this:getNeighbor_Four_4(x, y)
    return
    {
    -- Up
        {x, y - 4},{x - 1, y - 3}, {x + 1, y - 3}, {x - 2, y - 2}, {x + 2, y - 2}, {x - 3, y - 1}, {x + 3, y - 1},
    -- H
        {x - 4, y}, {x + 4, y},
    -- Down
        {x, y + 4},{x - 1, y + 3}, {x + 1, y + 3}, {x - 2, y + 2}, {x + 2, y + 2}, {x - 3, y + 1}, {x + 3, y + 1},
    }
end
function this:getNeighbor_Four_5(x, y)
    return
    {
    -- Up
        {x, y - 5},{x - 1, y - 4}, {x + 1, y - 4}, {x - 2, y - 3}, {x + 2, y - 3}, {x - 3, y - 2}, {x + 3, y - 2},{x - 2, y - 1}, {x + 2, y - 1},
        {x - 1, y - 4}, {x + 1, y - 4},
    -- H
        {x - 5, y}, {x + 5, y},
    -- Down
        {x, y + 5},{x - 1, y + 4}, {x + 1, y + 4}, {x - 2, y + 3}, {x + 2, y + 3}, {x - 3, y + 2}, {x + 3, y + 2},{x - 2, y + 3}, {x + 2, y + 3},
        {x - 1, y + 4}, {x + 1, y + 4},
    }
end
function this:getNeighbor_Four_6(x, y)
    return
    {
    -- Up
        {x, y - 6},{x - 1, y - 5}, {x + 1, y - 5}, {x - 2, y - 4}, {x + 2, y - 4}, {x - 3, y - 3}, {x + 3, y - 3},{x - 4, y - 2}, {x - 4, y - 2},
        {x - 5, y - 1}, {x - 5, y - 1},
    -- H
        {x - 6, y}, {x + 6, y},
    -- Down
        {x, y + 6},{x - 1, y + 5}, {x + 1, y + 5}, {x - 2, y + 4}, {x + 2, y + 4}, {x - 3, y + 3}, {x + 3, y + 3},{x - 4, y + 2}, {x - 4, y + 2},
        {x - 5, y + 1}, {x - 5, y + 1},
    }

end
-- 获取第一类可能的匹配邻居点
--[[
        o o o x o o o
        o o o x o o o
        o o o o o o o
        x x o p o x x
        o o o o o o o
        o o o x o o o
        o o o x o o o
--]]
function this:getNeighborMatch1(x, y)
    return
    {
    -- H方向
        {x - 3, y}, { x - 2, y}, {x + 2, y}, { x + 3, y},
    -- V方向
        {x, y - 3}, { x, y - 2}, {x, y + 2}, { x, y + 3},
    }
end

-- 获取第二类可能的匹配邻居点
--[[
        x o x
        x o x
        o p o
        x o x
        x o x
--]]
function this:getNeighborMatch2(x, y)
    return
    {
        -- 上方
        {x - 1, y - 2}, { x - 1, y - 1}, {x + 1, y - 2}, { x + 1, y - 1},
        -- 下方
        {x - 1, y + 2}, { x - 1, y + 1}, {x + 1, y + 2}, { x + 1, y + 1},
    }
end

-- 获取第三类可能的匹配邻居点
--[[
        o x o
        x o x
        o p o
        x o x
        o x o
--]]
function this:getNeighborMatch3(x, y)
    return
    {
        -- 上方
        {x - 1, y - 1}, { x, y - 2}, {x + 1, y - 1}, {x, y - 2},
        -- 下方
        {x - 1, y + 1}, { x, y + 2}, {x + 1, y + 1}, {x, y + 2},
    }
end

-- 获取第四类可能的匹配邻居点
--[[
       x x o x x
       o o p o o
       x x o x x
--]]
function this:getNeighborMatch4(x, y)
    return
    {
    -- 上方
        {x - 2, y - 1}, { x - 1, y - 1}, {x + 1, y - 1}, {x + 2, y - 1},
    -- 下方
        {x - 2, y + 1}, { x - 1, y + 1}, {x + 1, y + 1}, {x + 2, y + 1},
    }
end
-- 获取第五类可能的匹配邻居点
--[[
       o x o x o
       x o p o x
       o x o x o
--]]
function this:getNeighborMatch5(x, y)
    return
    {
    -- 上方
        {x - 2, y}, { x - 1, y - 1}, {x + 1, y - 1}, {x + 2, y},
    -- 下方
        {x - 2, y}, { x - 1, y + 1}, {x + 1, y + 1}, {x + 2, y},
    }
end

-- 寻找直线型个数为3的匹配集
function this:findLineThree(x, y, map)
    local lineList = {}
    local piece = map[x][y]
    local function tryLineThree(pos1, pos2)
        local isSame, color = self:isSameColor(pos1, pos2, map)
        if isSame and piece:getColor() == color then
            local p2 = map[pos1[1]][pos1[2]]
            local p3 = map[pos2[1]][pos2[2]]
            if p2:isCanMatch() and p3:isCanMatch() then
                table.insert(lineList, {x = piece.X, y = piece.Y})
                table.insert(lineList, {x = pos1[1], y = pos1[2]})
                table.insert(lineList, {x = pos2[1], y = pos2[2]})
            end
        end
    end
    local neighborList = self:getNeighbor(x, y)
    local neighborList2 = self:getNeighbor_Four(x, y)
    for i = 1, #neighborList do
        if i % 2 == 0 then
            local group = {neighborList[i - 1], neighborList[i]}
            tryLineThree(group[1], group[2])
        end
    end
    if table.empty(lineList)then
        for i = 1, #neighborList2 do
            if i % 2 == 0 then
                local group = {neighborList2[i - 1], neighborList2[i]}
                tryLineThree(group[1], group[2])
            end
        end
    end

    return lineList
end

-- 基于已经匹配上的3个同色集寻找直线上的是否有更多邻接同色点
function this:findLineMore(lineList, map)
    if #lineList ~= 3 then
        return lineList
    end
    local p1 = map[lineList[1].x][lineList[1].y]
    local p2 = map[lineList[2].x][lineList[2].y]
    local p3 = map[lineList[3].x][lineList[3].y]
    -- 三连之后从两个边缘往外扩散继续寻找邻接的下个同色点
    local hMin = math.min(p1.X, p2.X, p3.X)
    local hMax = math.max(p1.X, p2.X, p3.X)
    local vMin = math.min(p1.Y, p2.Y, p3.Y)
    local vMax = math.max(p1.Y, p2.Y, p3.Y)
    local color = p1:getColor()
    local isHorizontal = p1.Y == p2.Y
    if isHorizontal then
        --print("水平匹配")
        -- 左边
        local leftIndex = hMin
        while leftIndex > 0 do
            leftIndex = leftIndex - 1
            local adjPiece = map[leftIndex][p1.Y]
            if adjPiece:getColor() == color and adjPiece:isCanMatch() then
                table.insert(lineList, { x = adjPiece.X, y = adjPiece.Y })
            else
                break
            end
        end
        -- 右边
        local rightIndex = hMax
        while rightIndex < map.columnMax - 1 do
            rightIndex = rightIndex + 1
            local adjPiece = map[rightIndex][p1.Y]
            if adjPiece:getColor() == color and adjPiece:isCanMatch() then
                table.insert(lineList, { x = adjPiece.X, y = adjPiece.Y })
            else
                break
            end
        end
    else
        --print("垂直匹配")
        --  上面
        local upIndex = vMin
        while upIndex > 0 do
            upIndex = upIndex - 1
            local adjPiece = map[p1.X][upIndex]
            if adjPiece:getColor() == color and adjPiece:isCanMatch() then
                table.insert(lineList, { x = adjPiece.X, y = adjPiece.Y })
            else
                break
            end
        end
        -- 下面
        local downIndex = vMax
        while downIndex < map.rowMax - 1 do
            downIndex = downIndex + 1
            local adjPiece = map[p1.X][downIndex]
            if adjPiece:getColor() == color and adjPiece:isCanMatch() then
                table.insert(lineList, { x = adjPiece.X, y = adjPiece.Y })
            else
                break
            end
        end
    end
end

-- 基于直线寻找折线(L型或者T型)
function this:findPolyline(lineList, map)
    if #lineList < 3 then
        return lineList
    end
    --print(string.format("寻找折线, lineList = %s", table.tostring(lineList)))
    local p1 = map[lineList[1].x][lineList[1].y]
    local p2 = map[lineList[2].x][lineList[2].y]
    local function tryAddNear(pos1, pos2)
        local isSame, color = self:isSameColor(pos1, pos2, map)
        if isSame and p1:getColor() == color then
            local piece1 = map[pos1[1]][pos1[2]]
            local piece2 = map[pos2[1]][pos2[2]]
            if piece1:isCanMatch() and piece2:isCanMatch() then
                table.insert(lineList, {x = pos1[1], y = pos1[2]})
                table.insert(lineList, {x = pos2[1], y = pos2[2]})
            end
        end
    end
    local isHorizontalLine = p1.Y == p2.Y
    for i = 1, #lineList do
        local nearList = self:getNeighbor(lineList[i].x, lineList[i].y)
        local fourNearList = self:getNeighbor_Four(lineList[i].x, lineList[i].y)
        if isHorizontalLine then
            tryAddNear(nearList[5], nearList[6])
            tryAddNear(nearList[7], nearList[8])
            tryAddNear(fourNearList[3], fourNearList[4])
        else
            tryAddNear(nearList[1],nearList[2])
            tryAddNear(nearList[3],nearList[4])
            tryAddNear(fourNearList[1], fourNearList[2])
        end
    end
    return lineList
end

-- 检测两个点是否有相同的颜色, 有就返回true与color
function this:isSameColor(pos1, pos2, map)
    if (pos1[1] < 0 or pos1[1] >= map.columnMax) or (pos2[1] < 0 or pos2[1] >= map.columnMax)then
        return false
    end
    if (pos1[2] < 0 or pos1[2] >= map.rowMax) or (pos2[2] < 0 or pos2[2] >= map.rowMax)then
        return false
    end
    local piece1 = map[pos1[1]][pos1[2]]
    local piece2 = map[pos2[1]][pos2[2]]
    if piece1:getColor() == piece2:getColor() and piece1:getColor() ~= EliminateConfig.ColorType.NONE then
        return true, piece1:getColor()
    end
    return false
end

-- 找到所有可以移动一步消除的piece, 没有就说明是死图
function this:findMayEliminateList(map)
    local allMayList = {}
    local function tryAddToAllMayList(allMayList, mayItem)
        if table.empty(mayItem) then
            return
        end
        local isInList = false
        for i = 1, #allMayList do
            if allMayList[i][1].x == mayItem[1].x and allMayList[i][2].y == mayItem[2].y then
                isInList = true
            end
        end
        if isInList == false then
            table.insert(allMayList, mayItem)
        end
    end

    for x = 0, map.columnMax - 1 do
        for y = 0, map.rowMax - 1 do
            local mayItemList = self:getMaybeMatch(x, y, map)
            if table.empty(mayItemList) == false then
                --print(string.format("以(%s,%s)点为基点找到可能交换的点集mayItemList = %s", x, y,table.tostring(mayItemList)))
                for i = 1, #mayItemList do
                    tryAddToAllMayList(allMayList, mayItemList[i])
                end
            end
        end
    end
    --print("finalAllMayList = "..table.tostring(allMayList))
    return allMayList
end

-- 获取可能匹配
function this:getMaybeMatch(x, y, map)
    local neighborMatchList = {}
    local neighborList = {}
    local function addMatchList(matchList)
        if table.empty(matchList) == false then
            for i = 1, #matchList do
                table.insert(neighborMatchList, matchList[i])
            end
        end
    end
    table.insert(neighborList, self:getNeighborMatch1(x, y))
    table.insert(neighborList, self:getNeighborMatch2(x, y))
    table.insert(neighborList, self:getNeighborMatch3(x, y))
    table.insert(neighborList, self:getNeighborMatch4(x, y))
    table.insert(neighborList, self:getNeighborMatch5(x, y))
    for i = 1, #neighborList do
        local matchList = self:getMatchItemList(x,y, neighborList[i], map, i)
        addMatchList(matchList)
    end
    return neighborMatchList
end

-- 获取可能匹配的piece组合
function this:getMatchItemList(x, y, neighborMatch, map, mayMatchType)
    local matchItemList = {}
    for i = 1, #neighborMatch do
        if i % 2 == 0 then
            local group  = { neighborMatch[i - 1], neighborMatch[i] }
            local point1 = { x = x, y = y }
            local point2 = { x = group[1][1], y = group[1][2] }
            local point3 = { x = group[2][1], y = group[2][2] }
            local item = self:getMaybeMatchItem(point1, point2, point3, map)
            if table.empty(item) == false then
                table.insert(matchItemList, item)
            end
        end
    end
    return matchItemList
end

function this:getMaybeMatchItem(basePoint, point2, point3, map)
    local mayMatchPointList = {}
    local pointPiece = map[basePoint.x][basePoint.y]
    local isSame, color = self:isSameColor({point2.x, point2.y }, {point3.x, point3.y }, map)
    if isSame and pointPiece:getColor() == color and color ~= EliminateConfig.ColorType.NONE then
        table.insert(mayMatchPointList, basePoint)
        table.insert(mayMatchPointList, point2)
        table.insert(mayMatchPointList, point3)
    end
    return mayMatchPointList
end

-- 通过匹配方式获取能引起消除的交换组合(即两个点)
function this:getCanSwapToEliByMayMatchType(point1, point2, point3, map, mayMatchType)
    local mayMatchSwapPointList = {}
    local pointPiece = map[point1.x][point1.y]
    local tPoint1 = {}
    local tPoint2 = {}

    local isSame, color = self:isSameColor({point2.x, point2.y }, {point3.x, point3.y }, map)
    if isSame and pointPiece:getColor() == color and color ~= EliminateConfig.ColorType.NONE then
        local xMax = math.max(point1.x, point2.x, point3.x)
        local yMax = math.max(point1.y, point2.y, point3.y)
        if mayMatchType == 1 then
            tPoint1 = point1
            if point1.x > point2.x and point1.x > point3.x  then
                tPoint2 = { x = point1.x - 1, y = point1.y}
            elseif  point1.x < point2.x and point1.x < point3.x then
                tPoint2 = { x = point1.x + 1, y = point1.y}
            elseif point1.y > point2.y and point1.y > point3.y then
                tPoint2 = { x = point1.x, y = point1.y - 1}
            elseif point1.y < point2.y and point1.y < point3.y then
                tPoint2 = { x = point1.x , y = point1.y + 1}
            end
        elseif mayMatchType == 2 then
            tPoint1 = point1
            if point1.x > point2.x and point1.x > point3.x  then
                tPoint2 = { x = point1.x - 1, y = point1.y}
            elseif  point1.x < point2.x and point1.x < point3.x then
                tPoint2 = { x = point1.x + 1, y = point1.y}
            end
        elseif mayMatchType == 3 then
            if point1.y > point2.y and point1.y > point3.y  then
                if point1.x == xMax then
                    tPoint1 = { x = point1.x - 1, y = point1.y - 1}
                    tPoint2 = { x = point1.x, y = point1.y - 1}
                else
                    tPoint1 = { x = point1.x + 1, y = point1.y - 1}
                    tPoint2 = { x = point1.x, y = point1.y - 1}
                end
            elseif  point1.y < point2.y and point1.y < point3.y then
                if point1.x == xMax then
                    tPoint1 = { x = point1.x - 1, y = point1.y + 1}
                    tPoint2 = { x = point1.x, y = point1.y + 1}
                else
                    tPoint1 = { x = point1.x + 1, y = point1.y + 1}
                    tPoint2 = { x = point1.x, y = point1.y + 1}
                end
            end
        elseif mayMatchType == 4 then
            tPoint1 = point1
            if point1.y == yMax then
                tPoint2 = { x = point1.x, y = point1.y - 1}
            else
                tPoint2 = { x = point1.x, y = point1.y + 1}
            end
        elseif mayMatchType == 5 then
            if point1.x == xMax  then
                if point1.y == yMax then
                    tPoint1 = { x = point1.x - 1, y = point1.y - 1}
                    tPoint2 = { x = point1.x - 1, y = point1.y}
                else
                    tPoint1 = { x = point1.x - 1, y = point1.y + 1}
                    tPoint2 = { x = point1.x - 1, y = point1.y}
                end
            else
                if point1.y == yMax then
                    tPoint1 = { x = point1.x + 1, y = point1.y - 1}
                    tPoint2 = { x = point1.x + 1, y = point1.y}
                else
                    tPoint1 = { x = point1.x + 1, y = point1.y + 1}
                    tPoint2 = { x = point1.x + 1, y = point1.y}
                end
            end
        end

    end
    if table.empty(tPoint1) == false and table.empty(tPoint2) == false then
        table.insert(mayMatchSwapPointList, tPoint1)
        table.insert(mayMatchSwapPointList, tPoint2)
        --print(string.format("找到可交换的两个块: (%s,%s) <-> (%s,%s), mayMatchType = %s",
        --        tPoint1.x, tPoint1.y, tPoint2.x, tPoint2.y, mayMatchType ))
    end
    return mayMatchSwapPointList
end

---@param row number
---@param columnMax number
---@return table
function this:getSameRow(point, columnMax, isExclude)
    local list = {}
    for x = 0, columnMax - 1 do
        if isExclude then
            if x ~= point.x then
                table.insert(list, {x = x, y = point.y})
            end
        else
            table.insert(list, {x = x, y = point.y})
        end
    end
    return list
end

function this:getSameColumn(point, rowMax, isExclude)
    local list = {}
    for y = 0, rowMax - 1 do
        if isExclude then
            if y ~= point.y then
                table.insert(list, {x = point.x, y = y})
            end
        else
            table.insert(list, {x = point.x, y = y})
        end
    end
    return list
end

function this:getMaxSameColor(point, map)
    if table.empty(map) or table.empty(point) then
        return
    end
    local colorCountDic = {}
    for x = 0, map.rowMax - 1 do
        for y = 0, map.columnMax - 1 do
            local p = map[x][y]
            local color = p:getColor()
            if colorCountDic[color] == nil then
                colorCountDic[color] = 1
            else
                colorCountDic[color] = colorCountDic[color] + 1
            end
        end
    end
    local colorList = {}
    for k, v in pairs(colorCountDic) do
        if k ~= EliminateConfig.ColorType.NONE then
            table.insert(colorList, v)
        end
    end
    local maxColorCount = math.max(table.unpack(colorList))
    local maxList = {}
    for k, v in pairs(colorCountDic) do
        if v == maxColorCount then
            table.insert(maxList, k)
        end
    end
    local randomColor = maxList[math.random(1, #maxList)]
    local list = {}
    for x = 0, map.rowMax - 1 do
        for y = 0, map.columnMax - 1 do
            local p = map[x][y]
            local color = p:getColor()
            if color == randomColor and not(x == point.x and y == point.y) then
                table.insert(list, {x = x, y = y})
            end
        end
    end
    --print("colorCountDic = "..table.tostring(colorCountDic))
    --print("maxList = "..table.tostring(maxList))
    --print("randomColor = "..randomColor)
    --print("list = "..table.tostring(list))
    return list
end

function this:getSameColor(randomColor, map)
    local list = {}
    for x = 0, map.rowMax - 1 do
        for y = 0, map.columnMax - 1 do
            local p = map[x][y]
            local color = p:getColor()
            if color == randomColor then
                table.insert(list, {x = x, y = y})
            end
        end
    end
    return list
end

function this:getMaxSameColor_DanceMoment(point, map)
    local list = {}
    local sameList = self:getMaxSameColor(point, map)
    for i = 1, 10 do
        
    end
end

function this:getCross(point, map)
    local list = {}
    local rowList = self:getSameRow(point, map.columnMax, true)
    local columnList = self:getSameColumn(point, map.rowMax, true)
    table.extend(list, rowList)
    table.extend(list, columnList)
    return list
end

function this:getCross_DanceMoment(point, map)
    local list = {}
    for y = point.y - 1, point.y + 1 do
        for x = 0, map.columnMax - 1 do
            if not(x == point.x and y == point.y) and (y >= 0 and y < map.columnMax) then
                table.insert(list, {x = x, y = y})
            end
        end
    end
    for x = point.x - 1, point.x + 1 do
        for y = 0, map.rowMax - 1 do
            if (x >= 0 and x < map.rowMax) and y ~= point.y - 1 and y ~= point.y and y ~= point.y + 1 then
                table.insert(list, {x = x, y = y})
            end
        end
    end
    return list
end

function this:getBombNear(x, y, map)
    local neighbor = self:getNeighbor_Eight(x, y)
    local list = {}
    for i = 1, #neighbor do
        local group = neighbor[i]
        if (group[1] >= 0 and group[1] < map.columnMax) and (group[2] >= 0 and group[2] < map.rowMax) then
            table.insert(list, {x = group[1], y = group[2]})
        end
    end
    return list
end

function this:getBombNear_DanceMoment(x, y, map)
    local neighbor = self:getNeighbor_Eight_Cross(x, y)
    local list = {}
    for i = 1, #neighbor do
        local group = neighbor[i]
        if (group[1] >= 0 and group[1] < map.columnMax) and (group[2] >= 0 and group[2] < map.rowMax) then
            table.insert(list, {x = group[1], y = group[2]})
        end
    end
    return list
end

function this:getNormal_DanceMoment(point, map)
    local neighbor = self:getNeighbor_Four(point.x, point.y)
    local list = {}
    for i = 1, #neighbor do
        local group = neighbor[i]
        if (group[1] >= 0 and group[1] < map.columnMax) and (group[2] >= 0 and group[2] < map.rowMax) then
            table.insert(list, {x = group[1], y = group[2]})
        end
    end
    return list
end


