---
--- Created by Lichongzhi.
--- DateTime: 2017/11/29 9:41
---
require "eliminate:module/eliminate/view/EliminateGameView"
require "eliminate:module/eliminate/view/EliminateBaseGrid"
require "eliminate:module/eliminate/view/EliminatePoolView"
require "eliminate:module/eliminate/view/EliminatePiece"
require "eliminate:module/eliminate/view/EliminatePieceBg"
require "eliminate:module/eliminate/view/EliminateItem"
require "eliminate:module/eliminate/EliminateConfig"
require "eliminate:module/eliminate/EliminateExtension"

local NoticeManager = CS.NoticeManager
local Vector3 = CS.UnityEngine.Vector3
local Vector2 = CS.UnityEngine.Vector2
local GlobalTimeManager = CS.GlobalTimeManager
local Loger = CS.Loger
local Time = CS.UnityEngine.Time
local Mathf = CS.UnityEngine.Mathf
local TipTimerKey = "EliminateTipTimer"
EliminateGrid = EliminateBaseGrid:new()
local this = EliminateGrid

this.piecesDic = nil
this.piecesBgDic = nil
this.isGameStarted = false    -- 游戏是否已开始
this.isFilling = false      -- 是否正在填充
this.isShufflingMap = false   -- 是否正在洗牌地图
this.isAccountClear = false     -- 是否正在结算消除中
this.isDanceMoment = false      -- 是否在炫舞时刻中
this.fillTime = 0               -- 填充时间
this.swapTime = 0               -- 交换时间

this.pieceBgPool = nil
this.piecePool = nil

this.preEliTimestamp = 0        -- 上一次的消除时间
this.isTiping = false           -- 是否正在提示
this.waitSpecialQueue = nil      -- 等待特殊化的队列
this.waitEliAutoQueue = nil
this.waitEliHandQueue = nil
this.waitAccountQueue = nil         -- 等待交换点集队列
this.comboCount = 0             -- 当前combo数
this.comboCold  = 0             -- 连击冷却
this.preComboTimestamp = 0      -- 上一次连击的时刻
this.speSameNeedHandCount = 0   -- 特殊块单色消生成需要的手动消除次数
this.curHandClearPieceCount    = 0   -- 当前已手动消除的方块个数
this.swapPointList = {}         -- 交换的点集列表
-- item
this.droppingItemDic = nil
this.activatingItemDic = nil
this.itemPool = nil
--this.waitShuffleCount = 0       -- 等待洗牌次数
this.waitPointEliCount = 0      -- 等待点击消除次数
-- cro
this.itemShuffleCro = nil

function EliminateGrid:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
    end
    return self._instance
end

function this:initGrid()
    self:initConfig()
    self:initComponentRef()
    self:initPool()
    self:initPieceBg()
end

function this:getPositionByGrid(x, y)
    local firstPieceX = 0 -  EliminateConfig.pxPerUnit *  ((self.xDim - 1) / 2)
    local firstPieceY = -250 +  EliminateConfig.pxPerUnit *  ((self.yDim - 1)/ 2)
    return Vector3(firstPieceX + x * EliminateConfig.pxPerUnit, firstPieceY - y * EliminateConfig.pxPerUnit)
end

-- 填充grid
function this:fill()
    local fillCro = nil
    fillCro = coroutine.start(function ()
        local needsRefill = true
        self.isFilling = true
        while needsRefill do
            local emptyInfo = self:getColumnEmptyInfo()
            --print("emptyInfo: "..table.tostring(emptyInfo))
            while(self:fillStepNew(emptyInfo)) do
                coroutine.step(fillCro)
            end
            local curTime = Time.time
            local endTime = curTime + self.fillTime
            while(curTime <= endTime) do
                curTime = curTime + Time.fixedDeltaTime
                coroutine.step(fillCro)
            end
            coroutine.step(fillCro)
            --printError("下落完毕, 寻找匹配")
            self:updateAllMatches(EliminateConfig.OperateMode.Auto)
            needsRefill = self:clearAutoMatches()
            if needsRefill and self.isAccountClear == false then
                NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboChange, {index = 1})
            end
        end
        self.isFilling = false
        -- 还有移动的块时不该寻找可能匹配
        while self:checkHaveMoving() do
            --print("地图中有正在移动的, 等待它们移动完毕再找可能匹配列表")
            coroutine.step(fillCro)
        end
        local allMayList = EliminateExtension:findMayEliminateList(self.piecesDic)
        if table.empty(allMayList) then
            NoticeManager.Instance:Dispatch(EliminateNoticeType.RefreshMap)
        else
            self:tryFreezeGrid()
        end
    end)
end

function this:getPieceByPos(x, y )
    local piece = self.piecesDic[x][y]
    if piece == nil then
        Loger.PrintError("错误, 不存在对应键的Piece:(",x,",",y,")")
        return nil
    end
    return piece
end

function this:getColumnEmptyInfo()
    local columnEmptyInfo = {}
    for x = 0, self.xDim - 1 do
        local item = {}
        item.emptyCount = 0
        columnEmptyInfo[x] = item
    end
    -- 找出每一列的空洞数量
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local p = self.piecesDic[x][y]
            if p:getType() == EliminateConfig.PieceType.EMPTY then
                columnEmptyInfo[x].emptyCount = columnEmptyInfo[x].emptyCount + 1
            end
        end
    end
    local totalEmptyList = {}
    for _, v in pairs(columnEmptyInfo) do
        table.insert(totalEmptyList, v.emptyCount)
    end
    local maxCount = math.max(table.unpack(totalEmptyList))
    if maxCount == 0 then
        return columnEmptyInfo
    end
    local moveSpeed = maxCount / self.fillTime
    for x = 0, self.xDim - 1 do
        local info = columnEmptyInfo[x]
        if info.emptyCount == 1 then
            info.moveTime = 1 / moveSpeed
        elseif info.emptyCount ~= 0 then
            info.moveTime = 1 / moveSpeed
        else
            info.moveTime = 0
        end
    end
    --print("columnEmptyInfo = "..table.tostring(columnEmptyInfo))
    return columnEmptyInfo
end

-- 填充, 返回是否填充完成, (逐行生成, 逐列填充)
function this:fillStepNew(emptyInfo)
    local movedPiece = false;
    local baseY = EliminateConfig.SCREEN_UP_Y;
    -- 地图内的移位, 从倒数第二行逐个往下移位
    for x = 0, self.xDim - 1 do
        local info = emptyInfo[x]
        for y = self.yDim - 2, 0, -1 do
            local piece = self.piecesDic[x][y]
            if piece:isMoveable() then
                local pieceBelow = self:getPieceByPos(x, y + 1)
                if info.emptyCount ~= 0 and pieceBelow:getType() == EliminateConfig.PieceType.EMPTY then
                    self:poolPiece(pieceBelow)
                    self.piecesDic[x][y + 1] = piece
                    piece:onFallPiece(x, y + 1, info.moveTime)
                    self:spawnNewPiece(x, y, EliminateConfig.PieceType.EMPTY)
                    movedPiece = true
                end
            end
        end
    end
    -- 地图外的移位, 生成之后往下移位
    for x = 0, self.xDim - 1 do
        local info = emptyInfo[x]
        local pieceBelow = self:getPieceByPos(x, 0)
        if info.emptyCount ~= 0 and pieceBelow:getType() == EliminateConfig.PieceType.EMPTY then
            self:poolPiece(pieceBelow)
            local piece = self:spawnNewPiece(x, baseY, EliminateConfig.PieceType.NORMAL)
            --local color = math.random(1, EliminateConfig.ColorType.COUNT - 1)
            local colorList = EliminateConfig.GetColorList()
            local index = math.random(1, #colorList)
            local color = colorList[index]
            self.piecesDic[x][0] = piece
            piece:setColor(color, false)
            piece:onFallPiece(x, 0, info.moveTime)
            movedPiece = true
        end
    end
    return movedPiece
end

function this:spawnNewPiece(x, y, type)
    local piece = nil
    if self.piecePool.count < 1 then
        local item = self:createPiece()
        Queue.enqueue(self.piecePool, item)
    end
    piece = Queue.dequeue(self.piecePool)
    piece:setParent(self.pieceRoot)
    piece:initPieceLogicData(x, y, type)
    piece:setSize(EliminateConfig.pxPerUnit, EliminateConfig.pxPerUnit)
    piece:setPiecePosition(self:getPositionByGrid(x, y))
    self.piecesDic[x][y] = piece
    return piece
end

function this:spawnSpecialPiece(point, type, color)
    if table.empty(point) then
        error("错误-消消-特殊化的点为nil")
        return
    end
    local piece = self.piecesDic[point.x][point.y]
    if piece == nil then
        return
    end
    piece:setColor(color, true)
    piece:setType(type)
    piece:onSpecialPiece()
    -- 十字雷和炸弹是消除生成的, 要把它上面的块的下落状态重置
    if type == EliminateConfig.PieceType.CROSS_CLEAR or type == EliminateConfig.PieceType.BOMB then
        for y = 0, self.yDim - 1 do
            if y < point.y then
                local upPiece = self.piecesDic[point.x][y]
                if upPiece:getWaitFallState() then
                    upPiece:setWaitFallState(false, "spe")
                end
            end
        end
    end
end

function this:isAdjacent(left, right)
    return (left.x == right.x and math.abs(left.y - right.y) == 1)
    or (left.y == right.y and math.abs(left.x - right.x) == 1)
end

function this:swapPieceModel(point1, point2)
    --print(string.format("交换PieceModel(%s,%s)", table.tostring(point1), table.tostring(point2)))
    local piece1 = self.piecesDic[point1.x][point1.y]
    local piece2 = self.piecesDic[point2.x][point2.y]
    self.piecesDic[point1.x][point1.y] = piece2
    self.piecesDic[point2.x][point2.y] = piece1
    piece1:updatePieceIndex(point2.x, point2.y)
    piece2:updatePieceIndex(point1.x, point1.y)
    piece1:setSwapState(true)
    piece2:setSwapState(true)
end

function this:swapPieceView(piece1, piece2, afterAction)
    --print(string.format("交换PieceView(%s,%s)", table.tostring(piece1), table.tostring(piece2)))
    local swapCro = nil
    swapCro = coroutine.start(function ()
        local startPos_1 = piece1:getPiecePosition()
        local targetPos_1 = self:getPositionByGrid(piece1.X, piece1.Y)
        local startPos_2 = piece2:getPiecePosition()
        local targetPos_2 = self:getPositionByGrid(piece2.X, piece2.Y)
        local time = self.swapTime
        for t = 0, time, Time.deltaTime do
            local nextPos_1 =  Vector3.Lerp(startPos_1, targetPos_1, t / time);
            local nextPos_2 =  Vector3.Lerp(startPos_2, targetPos_2, t / time);
            piece1:setPiecePosition(nextPos_1)
            piece2:setPiecePosition(nextPos_2)
            coroutine.step(swapCro)
        end
        piece1:setPiecePosition(targetPos_1)
        piece2:setPiecePosition(targetPos_2)
        piece1:setSwapState(false)
        piece2:setSwapState(false)
        if afterAction ~= nil then
            afterAction()
        end
    end)
end

-- 更新交换结算数据
function this:updateSwapAccount()
    if table.empty(self.waitAccountQueue) or self.waitAccountQueue.count < 1 then
        return
    end
    local swapItem = Queue.dequeue(self.waitAccountQueue)
    local point1 = swapItem[1]
    local point2 = swapItem[2]
    local piece1 = self.piecesDic[point1.x][point1.y]
    local piece2 = self.piecesDic[point2.x][point2.y]
    if piece1:isCanMove() == false or piece2:isCanMove() == false then
        return
    end
    self:swapPieceModel(point1, point2)
    local isMatchByCurPoint = self:updateAllMatchesNew(swapItem, EliminateConfig.OperateMode.Hand)
    local function swapReturn()
        AudioManager.playSound("eliminate", "swap_error")
        --Loger.PrintLog("交换匹配结算完毕, 没有匹配, 交换回去, swapPoints = "..table.tostring(swapItem))
        self:swapPieceModel(point1, point2)
        self:swapPieceView(piece1, piece2, function ()
            piece1:setSwapFailState(false)
            piece2:setSwapFailState(false)
            --self:updateAllMatches(EliminateConfig.OperateMode.Auto)
            local isMatchedByReturn =  self:updateAllMatchesNew(swapItem, EliminateConfig.OperateMode.Hand)
            if isMatchedByReturn then
                self:clearHandMatches()
                self:fill()
            end
        end)
    end

    if isMatchByCurPoint then
        --Loger.PrintLog("交换匹配结算完毕, 成功匹配, swapPoints = "..table.tostring(swapItem))
        self:swapPieceView(piece1, piece2, function ()
            self:clearHandMatches()
        end)
    else
        piece1:setSwapFailState(true)
        piece2:setSwapFailState(true)
        self:swapPieceView(piece1, piece2, function ()
            swapReturn()
        end)
    end
end

---@return 匹配的piece的键集
function this:getMatchNew(x, y)
    local matchingPieces = {}
    matchingPieces.matchType = EliminateConfig.MatchType.None
    matchingPieces.pointList = {}
    local piece = self:getPieceByPos(x, y)
    if piece:isColored() == false then
        return matchingPieces
    end
    --print(string.format("尝试寻找匹配, 基准点为(%s,%s), 颜色为%s", x, y, piece:getColor()))
    local pointList = EliminateExtension:findLineThree(x, y, self.piecesDic)
    if table.empty(pointList)then
        return matchingPieces
    end
    EliminateExtension:findLineMore(pointList, self.piecesDic)
    local lineLength = table.len(pointList)
    if lineLength >= EliminateConfig.SPE_CROSS_MIN or lineLength >= EliminateConfig.SPE_BOMB_MIN then
        matchingPieces.matchType = EliminateConfig.MatchType.Line
    end
    EliminateExtension:findPolyline(pointList, self.piecesDic)
    local polyLength = table.len(pointList)
    if polyLength ~= lineLength then
        matchingPieces.matchType = EliminateConfig.MatchType.Poly
    end
    matchingPieces.pointList = pointList
    return matchingPieces
end

-- 找到所有匹配的, 并增加到待消除列表
function this:updateAllMatches(operateMode)
    --print("寻找匹配, 模式 = ".. operateMode)
    local curTurnEliList = {}
    for y = 0, self.yDim - 1 do
        for x = 0, self.xDim - 1 do
            local piece = self:getPieceByPos(x, y)
            if piece:isCanMatch() then
                local matchList = self:getMatchNew(x, y)
                if table.empty(matchList.pointList) == false then
                    --print(string.format("以(%s,%s)为基点自动寻找到匹配点集 = %s, pieceInfo = %s",
                    --        x, y, table.tostring(matchList.pointList), piece:logIsCanMatch()))
                    local curPointEliList = self:getWaitEliListByMatchList(matchList.pointList)
                    if table.empty(curPointEliList) == false then
                        table.extend(curTurnEliList, curPointEliList)
                    end
                    self:tryAddSpeByEli(matchList)
                end
            end
        end
    end
    self:updateWaitEliQueue(curTurnEliList, operateMode)
end

function this:updateAllMatchesNew(pointList, operateMode)
    local isMatch = false
    if table.empty(pointList) then
        return isMatch
    end
    --print("pointList: "..table.tostring(pointList))
    local curTurnEliList = {}
    for _, v in pairs(pointList) do
        local piece = self:getPieceByPos(v.x, v.y)
        if piece:isCanMatch() then
            local matchList = self:getMatchNew(v.x, v.y)
            if table.empty(matchList.pointList) == false then
                --print(string.format("以(%s,%s)为基点人工寻找到匹配点集 = %s, pieceInfo = %s",
                --        v.x, v.y, table.tostring(matchList.pointList), piece:logIsCanMatch()))
                local curPointEliList = self:getWaitEliListByMatchList(matchList.pointList)
                if table.empty(curPointEliList) == false then
                    table.extend(curTurnEliList, curPointEliList)
                end
                self:tryAddSpeByEli(matchList)
            end
        else
            --printError(string.format("当前点不可成为匹配基点: (%s,%s), %s", v.x, v.y, piece:logIsCanMatch()))
        end
    end
    self:updateWaitEliQueue(curTurnEliList, operateMode)
    if table.empty(curTurnEliList) == false then
        isMatch = true
    end
    return isMatch
end

function this:clearSpecialSuccess(speType)
    local needsRefill = false
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local piece = self.piecesDic[x][y]
            if piece:getType() == speType then
                self:activeSpecialPiece({x = x, y = y}, piece:getType(), EliminateConfig.ActiveSpeMode.Account)
                needsRefill = true
                break
            end
        end
        if needsRefill then
            break
        end
    end
    return needsRefill
end

-- 清除自动匹配的
function this:clearAutoMatches()
    local needsRefill = false;
    if table.empty(self.waitEliAutoQueue) == false and self.waitEliAutoQueue.count > 0 then
        local eliItem = Queue.dequeue(self.waitEliAutoQueue)
        for i = 1, #eliItem.eliList do
            self:clearPiece(eliItem.eliList[i].x, eliItem.eliList[i].y)
        end
        needsRefill = true
        AudioManager.playSound("eliminate", "eli_success")
        self:resetEliInfo()
        NoticeManager.Instance:Dispatch(EliminateNoticeType.ScoreChange, {deltaScore = EliminateConfig.SCORE_UNIT * #eliItem.eliList})
    end
    self:transformSpecialPiece()
    return needsRefill
end

function this:clearHandMatches()
    if self.waitEliHandQueue == nil or self.waitEliHandQueue.count < 1 then
        printError("消消-错误,等待手动消除队列中没有元素")
        return
    end
    local eliItem = Queue.dequeue(self.waitEliHandQueue)
    local eliList = eliItem.eliList
    --print(string.format("手动消除当前轮块, eliList = %s", table.tostring(eliList)))
    -- 在消除时如果上面有因为无匹配而交换回复时要等所有的交换失败回移结束才消除
    local upSwapFailList = {}
    for i = 1, #eliList do
        local curP = eliList[i]
        for y = 0, curP.y - 1 do
            local upP = self.piecesDic[curP.x][y]
            if upP:getSwapFailState() then
                table.insert(upSwapFailList, upP)
            end
        end
    end
    local function clearAction(eliList)
        AudioManager.playSound("eliminate", "eli_success")
        NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboChange, {index = 1})
        self:tryAddSpeSameItem(eliList)
        self:fill()
        self:resetEliInfo()
        self:transformSpecialPiece()
        NoticeManager.Instance:Dispatch(EliminateNoticeType.ScoreChange, {deltaScore = EliminateConfig.SCORE_UNIT * #eliList})
    end
    local waitSwapReturn = nil
    waitSwapReturn = coroutine.start(function ()
        while self:checkHaveSwapReturn(upSwapFailList) do
            --print("楼上交换失败了, 等待回移中")
            coroutine.step(waitSwapReturn)
        end
        for i = 1, #eliList do
            self:clearPiece(eliList[i].x, eliList[i].y)
        end
        clearAction(eliList)
    end)
end

-- 基于广度优先的消除列表搜索
function this:findEliByBFS(point)
    if table.empty(point) then
        return
    end
    local findList = {}
    local cur = self.piecesDic[point.x][point.y]
    cur:setFindState(true)
    table.insert(findList, point)
    local waitQueue = Queue.new()
    Queue.enqueue(waitQueue, point)

    if self.isDanceMoment and cur:isSecondKindSpe() == false then
        local neighborList = EliminateExtension:getNormal_DanceMoment(point, self.piecesDic)
        for i = 1, #neighborList do
            local neighbor = self.piecesDic[neighborList[i].x][neighborList[i].y]
            if neighbor:isNormal() or neighbor:isFirstKindSpe() then
                table.insert(findList, neighborList[i])
                Queue.enqueue(waitQueue, neighborList[i])
            end
        end
    end
    while waitQueue.count~= 0 do
        local v = Queue.dequeue(waitQueue)
        --print("扫描, 点 = "..table.tostring(v))
        local v_p = self.piecesDic[v.x][v.y]
        local neighborList = v_p:getEliRange()
        --print("neighborList = "..table.tostring(neighborList))
        for i = 1, #neighborList do
            local neighbor = self.piecesDic[neighborList[i].x][neighborList[i].y]
            if neighbor:getFindState() == false then
                neighbor:setFindState(true)
                -- 链式反应触发规则
                if cur:isSecondKindSpe() and (neighbor:isNormal() or neighbor:isFirstKindSpe()) then
                    table.insert(findList, neighborList[i])
                    Queue.enqueue(waitQueue, neighborList[i])
                elseif cur:isFirstKindSpe() and (neighbor:isNormal() or neighbor:isFirstKindSpe()) then
                    table.insert(findList, neighborList[i])
                    Queue.enqueue(waitQueue, neighborList[i])
                elseif cur:isNormal() and (neighbor:isNormal() or neighbor:isFirstKindSpe()) then
                    table.insert(findList, neighborList[i])
                    Queue.enqueue(waitQueue, neighborList[i])
                end
            end
        end
    end
    return findList
end

-- 获取消除列表
function this:getWaitEliListByMatchList(matchList)
    local waitEliList = {}
    local eliIndirect = {}
    for i = 1, #matchList do
        local list = self:findEliByBFS(matchList[i])
        table.extend(eliIndirect, list)
    end
    --print("eliIndirect = "..table.tostring(eliIndirect))
    for i = 1, #eliIndirect do
        local piece = self.piecesDic[eliIndirect[i].x][eliIndirect[i].y]
        if piece:getWaitEliState() == false and piece:isCanMatch() then
            piece:setWaitEliState(true)
            table.insert(waitEliList, eliIndirect[i])
        end
    end
    --print("waitEliList = "..table.tostring(waitEliList))
    return waitEliList
end

-- 更新等待消除列表的队列
function this:updateWaitEliQueue(eliList, operateMode)
    if self.waitEliAutoQueue == nil then
        self.waitEliAutoQueue = Queue.new()
    end
    if self.waitEliHandQueue == nil then
        self.waitEliHandQueue = Queue.new()
    end
    if table.empty(eliList) then
        return
    end
    if operateMode == EliminateConfig.OperateMode.Auto then
        Queue.enqueue(self.waitEliAutoQueue, {eliList = eliList})
    else
        Queue.enqueue(self.waitEliHandQueue, {eliList = eliList})
    end
    --print("更新等待消除列表的队列, eliList = "..table.tostring(eliList))
    -- 待消除的块的上面的块都将要落下
    for i = 1, #eliList do
        local curP = eliList[i]
        for y = 0, curP.y - 1 do
            local upP = self.piecesDic[curP.x][y]
            local isInEliList = false
            for j = 1, #eliList do
                if upP.X == eliList[j].x and upP.Y == eliList[j].y then
                    isInEliList = true
                end
            end
            if isInEliList == false and upP:isCanMatch()then
                if upP:getWaitFallState() == false then
                    upP:setWaitFallState(true, "eli")
                end
            end
        end
    end
end

-- 尝试增加特殊雷
---!警告 如果前一次的填充与后一次的交换结束在同一帧, 会造成本来是以自动模式形成的特殊雷变成以手动模式形成引起报错,
---因此临时使用填充模式识别以当次匹配点集和当次交换点集是否有交集进行判断
function this:tryAddSpeByEli(matchList)
    if table.len(matchList.pointList) < EliminateConfig.SPE_MIN then
        return
    end
    local function getHandPoint(pointList)
        local spePoint = {}
        local randomIndex = math.random(1, table.len(pointList))
        if table.empty(self.swapPointList) then
            spePoint = pointList[randomIndex]
        else
            local handPoint = nil
            for i = 1, #self.swapPointList do
                for j = 1, #pointList do
                    if self.swapPointList[i].x == pointList[j].x and self.swapPointList[i].y == pointList[j].y then
                        handPoint = self.swapPointList[i]
                        break
                    end
                end
            end
            if handPoint == nil then
                spePoint = pointList[randomIndex]
            else
                spePoint = handPoint
            end
        end

        return spePoint
    end
    local pointList = matchList.pointList
    local specialItem = {}
    local spePoint = {}
    local speType = -1
    local speColor = -1
    local length = table.len(pointList)
    if length >= EliminateConfig.SPE_CROSS_MIN and matchList.matchType == EliminateConfig.MatchType.Line then
        speType = EliminateConfig.PieceType.CROSS_CLEAR
        speColor = EliminateConfig.ColorType.NONE
    elseif length > 6 and matchList.matchType == EliminateConfig.MatchType.Poly then
        speType = EliminateConfig.PieceType.CROSS_CLEAR
        speColor = EliminateConfig.ColorType.NONE
    elseif length >= EliminateConfig.SPE_BOMB_MIN then
        speType = EliminateConfig.PieceType.BOMB
        speColor = self.piecesDic[pointList[1].x][pointList[1].y]:getColor()
    end
    spePoint = getHandPoint(pointList)
    specialItem.point = spePoint
    specialItem.type = speType
    specialItem.color = speColor
    self:updateWaitSpecial(specialItem)
end

function this:tryAddSpeSameItem(eliList)
    local specialItem = {}
    local eliCount = #eliList
    self.curHandClearPieceCount = self.curHandClearPieceCount + eliCount
    if self.curHandClearPieceCount >= self.speSameNeedHandCount then
        self.curHandClearPieceCount = 0
        local normalList = {}
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                local p = self.piecesDic[x][y]
                if p:getType() == EliminateConfig.PieceType.NORMAL then
                    table.insert(normalList, {x = x, y = y})
                end
            end
        end
        if table.empty(normalList) == false then
            local randomPoint = normalList[math.random(1, #normalList)]
            specialItem.point = randomPoint
            specialItem.type = EliminateConfig.PieceType.SAME_CLEAR
            specialItem.color = EliminateConfig.ColorType.NONE
        end
        self:randomNeedHandCount()
    end
    self:updateWaitSpecial(specialItem)
end

function this:updateWaitSpecial(specialItem)
    if self.waitSpecialQueue == nil then
        self.waitSpecialQueue = Queue.new()
    end
    if table.empty(specialItem) then
        return
    end
    Queue.enqueue(self.waitSpecialQueue, specialItem)
end

-- 激活特殊块
function this:activeSpecialPiece(point, type, activeMode)
    print("激活特殊块, point: "..table.tostring(point))
    local function afterAction()
        if activeMode == EliminateConfig.ActiveSpeMode.Normal then
            NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboChange, {index = 1})
        end
        self:fill()
        self:resetEliInfo()
        self:resetSwapInfo()
    end

    local speFillCro = nil
    speFillCro = coroutine.start(function ()
        local piece = self:getPieceByPos(point.x, point.y)
        piece:setActivedState(true)
        while self:checkHaveMoving() do
            print("激活特殊块时有非静止块, 等待全图静止中")
            coroutine.step(speFillCro)
        end
        local waitEliList = self:findEliByBFS(point)
        self:updateWaitEliQueue(waitEliList, EliminateConfig.OperateMode.Auto)
        for i = 1, #waitEliList do
            local piece = self:getPieceByPos(waitEliList[i].x, waitEliList[i].y)
            piece:onSelectedPiece()
            if piece:getWaitEliState() == false then
                piece:setWaitEliState(true)
            end
        end
        self:clearAutoMatches()
        if type == EliminateConfig.PieceType.CROSS_CLEAR  then
            AudioManager.playSound("eliminate", "spe_cross")
            local crossEffect_h = ObjectPool:GetInstance():getObject("fx_cross_eli_h")
            local crossEffect_v = ObjectPool:GetInstance():getObject("fx_cross_eli_v")
            local localPos = self:getPositionByGrid(point.x, point.y)
            local pos = self.pieceRoot:TransformPoint(localPos)
            crossEffect_h.transform.localPosition = Vector3(0, pos.y, 0)
            crossEffect_v.transform.localPosition = Vector3(pos.x, 0, 0)
            local crossEffectCro = nil
            crossEffectCro = coroutine.start(function ()
                coroutine.wait(crossEffectCro, 600)
                afterAction()
                ObjectPool:GetInstance():poolObject(crossEffect_h)
                ObjectPool:GetInstance():poolObject(crossEffect_v)
            end)
        elseif type == EliminateConfig.PieceType.SAME_CLEAR then
            AudioManager.playSound("eliminate", "spe_same")
            coroutine.wait(speFillCro, EliminateConfig.SPE_WAIT_TIME * 1000)
            afterAction()
        elseif type == EliminateConfig.PieceType.BOMB then
            AudioManager.playSound("eliminate", "spe_bomb")
            coroutine.wait(speFillCro, EliminateConfig.SPE_WAIT_TIME * 1000)
            afterAction()
        else
            Loger.PrintError("暂不支持的特殊雷类型..type = "..type)
        end
        --print("使用第二类特殊雷消除, waitEliList: "..table.tostring(waitEliList))
    end)

end

function this:resetEliInfo()
    self.preEliTimestamp = Time.time
    self.isTiping = false
    self:poolAllTip()
end

-- 转化特殊化piece
function this:transformSpecialPiece()
    if table.empty(self.waitSpecialQueue) or self.waitSpecialQueue.count < 1 then
        return
    end
    local specialItem = Queue.dequeue(self.waitSpecialQueue)
    if specialItem == nil then
        Loger.PrintError("错误-消消-Grid, 没有对应的特殊块信息, waitSpecialQueue = ", table.tostring(self.waitSpecialQueue))
    else
        self:spawnSpecialPiece(specialItem.point, specialItem.type, specialItem.color)
    end
    --print("转化特殊piece, specialItem = "..table.tostring(specialItem))
end

function this:tryFreezeGrid()
    if table.empty(self.activatingItemDic) then
        return
    end
    local totalFreezeArea = {}
    for _, v in pairs(self.activatingItemDic) do
        if v:getItemType() == EliminateConfig.ItemType.Freeze then
            local curFreezeArea = v:getItemResultArea()
            for i = 1, #curFreezeArea do
                table.insert(totalFreezeArea, curFreezeArea[i])
            end
        end
    end
    for _, v in pairs(totalFreezeArea) do
        local p = self.piecesDic[v.x][v.y]
        if p:getFreezeState() == false then
            p:setFreezeState(true, "filledTry")
        end
    end
end

function this:clearPiece(x, y)
    local piece = self:getPieceByPos(x, y)
    if piece == nil then
        Loger.PrintError("清除Piece错误-",x, y, "piece为nil")
        return
    end
    if piece:isClearable() == false then
        --Loger.PrintError("清除Piece错误-",x, y, "piece不可清除"..table.tostring(piece))
        return
    end
    piece:onClearPiece()
    self:spawnNewPiece(x, y ,EliminateConfig.PieceType.EMPTY)
end

-- 洗牌地图, 用于发现没有能匹配的piece对之后
function this:shuffleMap()
    --printError("消消-Grid-开始刷新地图")
    if self:checkCanShuffle() == false then
        Loger.PrintError("错误-消消-Grid-最大同色个数小于3, 无法洗牌成一个可能活图")
        NoticeManager.Instance:Dispatch(EliminateNoticeType.GameOver)
        return
    end
    local function testLogState()
        -- test
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                local piece = self.piecesDic[x][y]
                if piece:getWaitFallState() or piece:getWaitEliState() then
                    Loger.PrintError("错误, 洗牌时该牌处于异常状态", piece:logIsCanMatch())
                end
            end
        end
    end

    self:poolAllTip()
    self:resetSwapInfo()
    local shuffleCro = nil
    shuffleCro = coroutine.start(function ()
        while self.isShufflingMap do
            --print("地图已经在刷新中, 等待刷新完毕")
            coroutine.step(shuffleCro)
        end
        while self:checkHaveMoving() do
            --print("地图中有正在移动的, 等待它们")
            coroutine.step(shuffleCro)
        end
        while self.isFilling  do
            --print("地图正在填充, 等待填充完毕")
            coroutine.step(shuffleCro)
        end
        testLogState()
        self.isShufflingMap = true
        local isLive, oldPieceDic, randomPointList = self:shuffleToAliveMapModel()
        while(isLive == false)do
            isLive, oldPieceDic, randomPointList = self:shuffleToAliveMapModel()
            coroutine.step(shuffleCro)
        end
        self:shuffleGridView(oldPieceDic, randomPointList)
        local curTime = Time.time
        local endTime = curTime + EliminateConfig.SHUFFLE_TIME
        while(curTime < endTime) do
            curTime = curTime + Time.fixedDeltaTime
            coroutine.step(shuffleCro)
        end
        Loger.PrintLog("消消-Grid-刷新地图完毕")
        EliminateGameView:hideShuffleBtn()
        self.isShufflingMap = false
    end)
end

-- 创建地图
function this:createMapRandom()
    if self:createToAliveMap() then
        --print("创建活图成功")
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                local piece = self:getPieceByPos(x, y)
                piece:updateSprite()
            end
        end
    end
end

-- 根据配置表来生成关卡
function this:createMapByConfig()
    print("消消-根据配置生成关卡")
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local piece = self:getPieceByPos(x, y)
            local index = (y * self.xDim + 1 + x)
            local info = EliminateConfig.initMapConfig[index]
            if info[2] == nil then
                piece:setType(EliminateConfig.PieceType.NORMAL)
            else
                piece:setType(info[2])
            end
            local color = info[1]
            piece:setColor(color, false)
            piece:onSpecialPiece()
            piece:updatePieceIndex(x, y)
        end
    end
end

-- 初始化地图, 用于地图异色初始化
function this:initMap()
    self.piecesDic = {}
    self.piecesDic.rowMax = self.yDim
    self.piecesDic.columnMax = self.xDim
    for x = 0, self.xDim - 1 do
        self.piecesDic[x] = {}
        for y = 0, self.yDim - 1 do
           self:spawnNewPiece(x, y, EliminateConfig.PieceType.EMPTY)
        end
    end
    self:createMapRandom()
    --test
    --self:createMapByConfig()
    --NoticeManager.Instance:Dispatch(EliminateNoticeType.DazzleMomentStart)

end

-- 创建到一个不能直接消除但也不是死图的地图
function this:createToAliveMap()
    local isAlive = false
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local piece = self:getPieceByPos(x, y)
            local canRanColorList = self:getCanRandColor(x, y)
            local colorIndex = math.random(1, #canRanColorList)
            local color = canRanColorList[colorIndex]
            piece:setColor(color, true)
            piece:setType(EliminateConfig.PieceType.NORMAL)
            piece:updatePieceIndex(x, y)
        end
    end
    local allMayList = EliminateExtension:findMayEliminateList(self.piecesDic)
    if table.empty(allMayList)then
        Loger.PrintLog("消消-消消-创建的地图为死图, 递归创建地图")
        return self:createToAliveMap()
    else
        isAlive = true
        return isAlive
    end
end

-- 洗牌到一个不能直接消除但也不是死图的地图
function this:shuffleToAliveMapModel()
    local randomList = {}
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local index = x * 10 + y
            table.insert(randomList, index)
        end
    end
    math.shuffle(randomList)
    local randomPointList = {}
    for i = 1, #randomList do
        local point = {}
        point.x = math.floor(randomList[i] / 10) % 10
        point.y = randomList[i] % 10
        table.insert(randomPointList, point)
    end
    --print("随机的点集为, randomPointList = "..table.tostring(randomPointList))
    local newPieceDic = {}
    newPieceDic.rowMax = self.piecesDic.rowMax
    newPieceDic.columnMax = self.piecesDic.columnMax
    for x = 0, self.xDim - 1 do
        newPieceDic[x] = {}
        for y = 0, self.yDim - 1 do
            newPieceDic[x][y] = {}
        end
    end
    local oldPieceDic = self.piecesDic
    -- 根据打乱的顺序移动piece
    local index = 1
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local point = randomPointList[index]
            local piece = self.piecesDic[x][y]
            piece:updatePieceIndex(point.x, point.y)
            index = index + 1
            newPieceDic[point.x][point.y] = piece
        end
    end
    --更新pieceDic
    self.piecesDic = nil
    self.piecesDic = newPieceDic

    for y = 0, self.yDim - 1 do
        for x = 0, self.xDim - 1 do
            local piece = self:getPieceByPos(x, y)
            if piece:isCanMatch() then
                local matchList = self:getMatchNew(x, y)
                if table.empty(matchList.pointList) == false then
                    --Loger.PrintLog("消消-消消-洗牌的地图为直接活图, 递归洗牌地图")
                    return false
                end
            end
        end
    end
    local allMayList = EliminateExtension:findMayEliminateList(self.piecesDic)
    if table.empty(allMayList)then
        --Loger.PrintLog("消消-消消-洗牌的地图为死图, 递归洗牌地图")
        return false
    end

    return true, oldPieceDic, randomPointList
end

function this:shuffleGridView(oldPieceDic, randomPointList)
    local index = 1
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local point = randomPointList[index]
            local piece = oldPieceDic[x][y]
            piece:onShufflePiece(point.x, point.y, EliminateConfig.SHUFFLE_TIME)
            index = index + 1
        end
    end
end

-- 获取可随机的颜色
function this:getCanRandColor(x, y)
    local neighbor = EliminateExtension:getNeighbor(x, y)
    local defaultColor = {}
    for _, v in pairs(EliminateConfig.GetColorList()) do
        table.insert(defaultColor, v)
    end
    local function excludeColor(colorList, color)
        for i = 1, #colorList do
            if colorList[i] == color then
                table.remove(colorList, i)
            end
        end
        return colorList
    end
    local function checkAndExclude(pos1, pos2)
        local isSame, color = EliminateExtension:isSameColor(pos1, pos2, self.piecesDic)
        if isSame then
            excludeColor(defaultColor, color)
        end
    end
    for i = 1, #neighbor do
        if i % 2 == 0 then
            local group  = { neighbor[i - 1], neighbor[i] }
            checkAndExclude(group[1], group[2])
        end
    end
    return defaultColor
end

-- 设置选择框的位置
function this:setSelectedPiece(point)
    local piece = self:getPieceByPos(point.x, point.y)
    piece:onSelectedPiece()
end

-- 重置选择框
function this:resetSelected(point)
    local piece = self:getPieceByPos(point.x, point.y)
    piece:updateSprite()
end

function this:resetSwapInfo()
    --print("重置交换点列表")
    self.swapPointList = {}
end

function this:isCanPointed()
    if self.isGameStarted == false then
        return false
    end
    if self.isShufflingMap then
        return false
    end
    if self.isAccountClear then
        return false
    end
    return true
end

function this:trySwap()
    if table.empty(self.swapPointList) or table.len(self.swapPointList) ~= 2 then
        --Loger.PrintError("错误-消消-Grid, swapPointList = ", table.tostring(self.swapPointList))
        return
    end

    local p1 = self.swapPointList[1]
    local p2 = self.swapPointList[2]
    local piece1 = self.piecesDic[p1.x][p1.y]
    local piece2 = self.piecesDic[p2.x][p2.y]
    self:resetSelected(p1)
    --print("swapPointList = "..table.tostring(self.swapPointList))
    if piece1:isSecondKindSpe() and piece2:isSecondKindSpe() == false then
        self:resetSelected(p2)
        self:resetSwapInfo()
    else
        if self:isAdjacent(p1, p2) then
            self:updateWaitAccountQueue()
            self:resetSelected(p2)
            self:updateSwapAccount()
            self:resetSwapInfo()
        else
            table.remove(self.swapPointList, 1)
            -- 新选的不在旧选的旁边,更新选择的piece为新选的
            self:setSelectedPiece(p2)
        end
    end
end

function this:updateWaitAccountQueue()
    if self.waitAccountQueue == nil then
        self.waitAccountQueue = Queue.new()
    end
    if #self.swapPointList ~= 2 then
        return
    end
    Queue.enqueue(self.waitAccountQueue, self.swapPointList)
    --print("更新交换点集队列, waitAccountQueue = "..table.tostring(self.waitAccountQueue))
end

function this:randomNeedHandCount()
    self.speSameNeedHandCount = math.random(EliminateConfig.SPE_SAME_HAND_MIN, EliminateConfig.SPE_SAME_HAND_MAX)
    --print("speSameNeedHandCount = "..self.speSameNeedHandCount)
end

-- 道具使用的逻辑效果
function this:itemActivatingGrid(item)
    if table.empty(item) then
        return
    end
    self:poolAllTip()
    local type = item:getItemType()
    if type  == EliminateConfig.ItemType.Craze then
        self.fillTime = EliminateConfig.EACH_COLUMN_FILL_TIME / 2
        self.swapTime = EliminateConfig.SWAP_TIME / 2
    elseif type  == EliminateConfig.ItemType.RandomEli then
        local randomColor = math.random(1, EliminateConfig.ColorType.COUNT - 1)
        local list = EliminateExtension:getSameColor(randomColor, self.piecesDic)
        local curPointEliList = self:getWaitEliListByMatchList(list)
        self:updateWaitEliQueue(curPointEliList, EliminateConfig.OperateMode.Auto)
        self:clearAutoMatches()
        self:fill()
        NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboChange, {index = 1})
    elseif type  == EliminateConfig.ItemType.PointEli then
        self.waitPointEliCount = self.waitPointEliCount + 2
    elseif type  == EliminateConfig.ItemType.ClearObstacle then
        if table.empty(self.activatingItemDic) == false then
            for _, v in pairs(self.activatingItemDic) do
                if v:getItemNature() == 2 and v:getItemType() ~= EliminateConfig.ItemType.Shuffle then
                    v:itemTimeUp()
                end
            end
        end
    elseif type  == EliminateConfig.ItemType.Freeze then
        local function itemFreeze()
            local x = math.random(0, self.xDim - 2)
            local y = math.random(0, self.yDim - 2)
            for x = x, x + 1 do
                for y = y, y + 1 do
                    --print(string.format("被冰冻影响的点: (%s,%s)", x, y))
                    local p = self.piecesDic[x][y]
                    p:setFreezeState(true, "itemActivating")
                    item:addItemResultArea({x = x, y = y})
                end
            end
            local firstPiecePos = self:getPositionByGrid(x, y)
            local effectLocalPos = Vector3(firstPiecePos.x + EliminateConfig.tileUnit / 2, firstPiecePos.y - EliminateConfig.tileUnit / 2, 0)
            local effectPos = self.pieceRoot:TransformPoint(effectLocalPos)
            item:addItemActivatingEffectPos(effectPos)
        end
        for i = 1, 2 do
            itemFreeze()
        end
    elseif type  == EliminateConfig.ItemType.Fog then
    elseif type  == EliminateConfig.ItemType.Shake then
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                self.piecesDic[x][y]:pieceShakeStart()
            end
        end
    elseif type  == EliminateConfig.ItemType.Grayed then
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                self.piecesDic[x][y]:pieceGrayStart()
            end
        end
    elseif type  == EliminateConfig.ItemType.Shuffle then
        local freezeList = {}
        if table.empty(self.activatingItemDic) == false then
            for _, v in pairs(self.activatingItemDic) do
                if v:getItemNature() == 2 and v:getItemType() == EliminateConfig.ItemType.Freeze then
                    table.insert(freezeList, v)
                end
            end
        end
        for _, v in pairs(freezeList) do
            v:itemTimeUp()
        end
        self.isShufflingMap = true
        self.itemShuffleCro = coroutine.start(function ()
            while self.isFilling do
                coroutine.step(self.itemShuffleCro)
            end
            self:updateWaitShuffle()
        end)
    else
        Loger.PrintError("错误-消消-道具使用时未支持的道具类型,type = ", type)
    end
end

function this:itemActivatedEndGrid(item)
    if table.empty(item) then
        return
    end
    local type = item:getItemType()
    if type  == EliminateConfig.ItemType.Craze then
        self.fillTime = EliminateConfig.EACH_COLUMN_FILL_TIME
        self.swapTime = EliminateConfig.SWAP_TIME
    elseif type  == EliminateConfig.ItemType.RandomEli then
    elseif type  == EliminateConfig.ItemType.PointEli then
    elseif type  == EliminateConfig.ItemType.ClearObstacle then
    elseif type  == EliminateConfig.ItemType.Freeze then
        local areaList = item:getItemResultArea()
        if table.empty(areaList) == false then
            for i = 1, #areaList do
                local x = areaList[i].x
                local y = areaList[i].y
                local p = self.piecesDic[x][y]
                if p:getFreezeState() then
                    p:setFreezeState(false, "itemEnd")
                end
            end
        end
        self:updateAllMatches(EliminateConfig.OperateMode.Auto)
        self:clearAutoMatches()
        self:fill()
    elseif type  == EliminateConfig.ItemType.Fog then
    elseif type  == EliminateConfig.ItemType.Shake then
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                self.piecesDic[x][y]:pieceShakeEnd()
            end
        end
    elseif type  == EliminateConfig.ItemType.Grayed then
        for x = 0, self.xDim - 1 do
            for y = 0, self.yDim - 1 do
                self.piecesDic[x][y]:pieceGrayEnd()
            end
        end
    elseif type  == EliminateConfig.ItemType.Shuffle then

    else
        Loger.PrintError("错误-消消-道具使用结束未支持的道具类型,type = ", type)
    end
    item:itemEnd()
end

function this:resetStartInfo()
    self.piecesDic = nil
    self.isGameStarted = false
    self.isFilling = false
    self.isShufflingMap = false
    self.comboCount = 0
    self.preEliTimestamp = 0
    self.preComboTimestamp = 0
    self.waitShuffleCount = 0
    self.comboCold = 0
    self.isDanceMoment = false
    self.waitPointEliCount = 0
    self.fillTime = 0
    self.swapTime = 0
end

function this:resetCoroutine()
end

--====================================访问器=============================
function this:getPieceRoot()
    return self.pieceRoot
end

function this:getFillState()
    return self.isFilling
end

function this:getComboCount()
    return self.comboCount
end

function this:GetComboProgress()
    local progress = self.comboCount / EliminateConfig.ComboEvaNeedCount.Fourth
    progress = Mathf.Clamp(progress, 0, 1)

    return progress
end

function this:setDanceMomentState(state)
    self.isDanceMoment = state
end

function this:getDanceMomentState()
    return self.isDanceMoment
end

function this:getGridMap()
    return self.piecesDic
end

function this:getGameStartState()
    return self.isGameStarted
end

function this:updateWaitShuffle()
    self:shuffleMap()
end

-- 获取地图区域的边缘(世界坐标)
function this:getMapMargin()
    local mapMarginInfo = {}
    local leftUpPos = self:getPositionByGrid(0,0)
    local rightDownPos = self:getPositionByGrid(self.xDim - 1,self.yDim - 1)
    local centerPoint = { x = math.floor(self.xDim / 2), y = math.floor(self.yDim / 2)}
    local centerPos = self:getPositionByGrid(centerPoint.x,centerPoint.y)

    local localMinX = leftUpPos.x - EliminateConfig.tileUnit / 2
    local localMaxY = leftUpPos.y + EliminateConfig.tileUnit / 2
    local localMaxX = rightDownPos.x + EliminateConfig.tileUnit / 2
    local localMinY = rightDownPos.y - EliminateConfig.tileUnit / 2
    local minX = self.pieceRoot:TransformPoint(Vector3(localMinX, 0, 0)).x
    local maxY = self.pieceRoot:TransformPoint(Vector3(0, localMaxY, 0)).y
    local maxX = self.pieceRoot:TransformPoint(Vector3(localMaxX, 0, 0)).x
    local minY = self.pieceRoot:TransformPoint(Vector3(0, localMinY, 0)).y
    local center = self.pieceRoot:TransformPoint(centerPos)
    mapMarginInfo.minX = minX
    mapMarginInfo.minY = minY
    mapMarginInfo.maxX = maxX
    mapMarginInfo.maxY = maxY
    mapMarginInfo.center = center
    return mapMarginInfo
end

--====================================事件响应===========================

function this:onPointedDownGrid(screenPos)
    if self:isCanPointed() == false then
        return
    end
    AudioManager.playSound("eliminate", "selected")
    local point = self:getPointByPosition(screenPos)
    if point == nil then
        return
    end
    local piece = self.piecesDic[point.x][point.y]
    if piece == nil then
        return
    end
    if piece:isCanMove() == false then
        --Loger.PrintError("错误-消消-无法点击, piece = ", piece:logIsCanMatch())
        return
    end
    if piece:isSecondKindSpe() and  piece:getActivedState() == false then
        if table.empty(self.swapPointList) == false then
            self:resetSelected(self.swapPointList[1])
        end
        self:activeSpecialPiece(point, piece:getType(), EliminateConfig.ActiveSpeMode.Normal)
        return
    end
    if self.waitPointEliCount > 0 then
        self.waitPointEliCount = self.waitPointEliCount - 1
        local curPointEliList = self:getWaitEliListByMatchList({{x = piece.X, y = piece.Y}})
        self:updateWaitEliQueue(curPointEliList, EliminateConfig.OperateMode.Auto)
        self:clearAutoMatches()
        self:fill()
        NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboChange, {index = 1})
        return
    end
    self:setSelectedPiece(point)
    table.insert(self.swapPointList, point)
    self:trySwap()
end

function this:onPointedDragGrid(info)
    local function isInIntervalArea(value, left, right)
        if value >= left and value < right then
            return true
        end
        return false
    end
    if self:isCanPointed() == false or #self.swapPointList ~= 1 then
        return
    end
    local point = self.swapPointList[1]
    if info.delta.x <= 50 and info.delta.y == 50 then
        return
    end
    --print(string.format("info.delta = (%s,%s)",info.delta.x, info.delta.y))
    local angle = Vector2.Angle(info.delta, Vector2.right);
    local targetPoint = {}
    if isInIntervalArea(angle, 0, 45 ) then
        targetPoint = { x = point.x + 1 , y = point.y }
    elseif isInIntervalArea(angle, 45, 135) and info.delta.y >= 0  then
        targetPoint = { x = point.x, y = point.y - 1 }
    elseif isInIntervalArea(angle, 45, 135) and info.delta.y <= 0  then
        targetPoint = { x = point.x, y = point.y + 1}
    else
        targetPoint = { x = point.x - 1 , y = point.y}
    end
    if isInIntervalArea(targetPoint.x, 0, self.xDim) == false or isInIntervalArea(targetPoint.y, 0, self.yDim) == false then
        return
    end
    local piece = self.piecesDic[targetPoint.x][targetPoint.y]
    if piece:isCanMove() == false then
        --Loger.PrintError("错误-消消-无法点击, piece = ", table.tostring(piece))
        return
    end
    table.insert(self.swapPointList, targetPoint)
    --Loger.PrintLog("消消-滑动交换, piece = ", piece:logIsCanMatch())

    self:trySwap()
end

function this:onGameReadyGrid()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(TipTimerKey)
    self:initMap()
end

function this:onGameStartGrid()
    Loger.PrintLog("游戏开始")
    self.isGameStarted = true
    self.preEliTimestamp = Time.time
    self.comboCold = EliminateConfig.COMBO_COLD_TIME
    self:randomNeedHandCount()
    GlobalTimeManager.Instance.timerController:AddTimer(TipTimerKey, -1, -1, function ()
        local curTime = Time.time
        local timeSpan = curTime - self.preEliTimestamp
        if timeSpan > EliminateConfig.TIP_TIMESPAN and self.isTiping == false and self.isGameStarted and self.isAccountClear == false then
            self:showOneMayEliTip()
        end
        if self.comboCount ~= 0 then
            local curComboTime = Time.time
            local colTtimeSpan = curComboTime - self.preComboTimestamp
            self.comboCold = EliminateConfig.COMBO_COLD_TIME - self.comboCount * 0.05
            --self.comboCold = 10
            if colTtimeSpan > self.comboCold or self.comboCold <= 0 then
                --print("该次连击距离上次已过冷却, 连击归零")
                self.comboCount = 0
                NoticeManager.Instance:Dispatch(EliminateNoticeType.ComboBreak, {index = 1})
            end
        end
    end)
end

function this:onShuffleGrid()
    self:updateWaitShuffle()
end

function this:onComboGrid()
    self.preComboTimestamp = Time.time
    self.comboCount = self.comboCount + 1
end

function this:onAccountClearGrid()
    Loger.PrintLog("结算消除")
    self:resetEliInfo()
    local accountFillCro = nil
    accountFillCro = coroutine.start(function ()
        while self.isShufflingMap  do
            coroutine.step(accountFillCro)
        end
        local needsRefill = true
        self.isAccountClear = true
        while(needsRefill)do
            --print("填充开始")
            local emptyInfo = self:getColumnEmptyInfo()
            while(self:fillStepNew(emptyInfo)) do
                coroutine.step(accountFillCro)
            end
            local curTime = Time.time
            local endTime = curTime + EliminateConfig.EACH_COLUMN_FILL_TIME
            while(curTime <= endTime) do
                curTime = curTime + Time.fixedDeltaTime
                coroutine.step(accountFillCro)
            end
            while self:checkHaveMoving() do
                --print("激活特殊块时有非静止块, 等待全图静止中")
                coroutine.step(accountFillCro)
            end
            --print("填充完毕")
            self:updateAllMatches(EliminateConfig.OperateMode.Auto)
            needsRefill = self:clearAutoMatches()
            if needsRefill == false then
                needsRefill = self:clearSpecialSuccess(EliminateConfig.PieceType.BOMB)
                if needsRefill then
                    coroutine.wait(accountFillCro, EliminateConfig.SPE_WAIT_TIME * 1000)
                end
            end
            if needsRefill == false then
                needsRefill = self:clearSpecialSuccess(EliminateConfig.PieceType.CROSS_CLEAR)
                if needsRefill then
                    coroutine.wait(accountFillCro, EliminateConfig.SPE_WAIT_TIME * 1000)
                end
            end
            if needsRefill == false then
                needsRefill = self:clearSpecialSuccess(EliminateConfig.PieceType.SAME_CLEAR)
                if needsRefill then
                    coroutine.wait(accountFillCro, EliminateConfig.SPE_WAIT_TIME * 1000)
                end
            end
            if needsRefill then
                coroutine.wait(accountFillCro, EliminateConfig.ACCOUNT_FILL_WAIT * 1000)
            end
        end
        self.isAccountClear = false
        EliminateNetModule.sendReqGameRank()
    end)
end

function this:onGameOverGrid()
    --Loger.PrintLog("游戏结束")
end

function this:onDanceMomentStartGrid()
    self.isDanceMoment = true
end

function this:onDanceMomentOverGrid()
    self.isDanceMoment = false
end

function this:onTimeUpGrid()
    --Loger.PrintLog("时间到, 游戏结束")
    self:poolAllTip()
    if table.empty(self.droppingItemDic) == false then
        for _, v in pairs(self.droppingItemDic) do
            self:poolItem(v)
        end
    end
    if table.empty(self.activatingItemDic) == false then
        for _, v in pairs(self.activatingItemDic) do
            if v:getItemNature() == 2 and v:getItemType() ~= EliminateConfig.ItemType.Shuffle then
                v:itemTimeUp()
            end
        end
    end
    self.isGameStarted = false
    local center = { x = math.floor(self.xDim / 2), y = math.floor(self.yDim / 2)}
    local function isInMap(pos, map)
        if (pos[1] < 0 or pos[1] >= map.columnMax) or (pos[2] < 0 or pos[2] >= map.columnMax)then
            return false
        end
        return true
    end
    local function lightList(list)
        if table.empty(list) then
            return
        end
        for i = 1, #list do
            if isInMap(list[i], self.piecesDic) then
                local pos = list[i]
                local lightBgItem = self.piecesBgDic[pos[1]][pos[2]]
                lightBgItem:lightFgItem()
            end
        end
    end
    local bgItem = self.piecesBgDic[center.x][center.y]
    local lightCro = nil
    lightCro = coroutine.start(function ()
        bgItem:lightFgItem()
        coroutine.wait(lightCro, 100)
        local listInfo= {}
        table.insert(listInfo, EliminateExtension:getNeighbor_Four(center.x , center.y))
        table.insert(listInfo, EliminateExtension:getNeighbor_Four_2(center.x , center.y))
        table.insert(listInfo, EliminateExtension:getNeighbor_Four_3(center.x , center.y))
        table.insert(listInfo, EliminateExtension:getNeighbor_Four_4(center.x , center.y))
        table.insert(listInfo, EliminateExtension:getNeighbor_Four_5(center.x , center.y))
        table.insert(listInfo, EliminateExtension:getNeighbor_Four_6(center.x , center.y))
        for i = 1, #listInfo do
            lightList(listInfo[i])
            coroutine.wait(lightCro, 60)
        end
        coroutine.wait(lightCro, 1000)
        NoticeManager.Instance:Dispatch(EliminateNoticeType.AccountClear)
    end)
end

function this:onItemSpawnStartGrid(itemType)
    if self.droppingItemDic == nil then
        self.droppingItemDic = {}
    end
    local item = self:getItemFromPool()
    item:setItemType(itemType)
    item:itemDrop()
    self.droppingItemDic[item:getItemIndex()] = item
end

function this:onItemSpawnOverGrid(item)
    self:poolItem(item)
end

function this:onItemClickGrid(itemIndex)
    local curItem = self.droppingItemDic[itemIndex]
    if table.empty(curItem) then
        Loger.PrintError("错误-下落的道具列表不存在该道具,itemIndex=", itemIndex)
        return
    end
    self:tryUseItem(curItem)
    curItem:itemClick()
    self:poolItem(curItem)
end

function this:tryUseItem(item)
    local maxCount = EliminateDataProxy:GetInstance():getRoomMaxPlayerCount()
    local allPlayers = EliminateDataProxy:GetInstance():getAllPlayerInfo()
    local targetList = {}
    if item:getItemNature() == 1 then
        table.insert(targetList, EliminateDataProxy:GetInstance():GetMeUid())
    else
        if maxCount == 2 then
            for _, v in pairs(allPlayers) do
                if v:getIsMe() == false then
                    table.insert(targetList, v:getUid())
                end
            end
        else
            EliminateDataProxy:GetInstance():rankAllPlayerInfo()
            for i = 1, #allPlayers - 1 do
                local v = allPlayers[i]
                if v:getIsMe() == false then
                    table.insert(targetList, v:getUid())
                end
            end
        end
    end
    NoticeManager.Instance:Dispatch(EliminateNoticeType.UseItemReq, {itemType = item:getItemType(),targetList = targetList })
end

function this:onItemActiveGrid(item)
    if self.activatingItemDic == nil then
        self.activatingItemDic = {}
    end
    local function checkIsRepeat()
        local isRepeatItem = false
        for _, v in pairs(self.activatingItemDic) do
            if v:getItemType() == item:getItemType() then
                isRepeatItem = true
                break;
            end
        end
        return isRepeatItem
    end
    local function tryAddActivatingItem(item)
        if table.empty(item) then
            return
        end
        if item:getItemTableDuration() > 0 then
            self.activatingItemDic[item:getItemIndex()] = item
        end
    end
    print(string.format("使用道具: (index:%s,type:%s,name:%s)",
            item:getItemIndex(),item:getItemType(),item:getItemName()))
    self:itemActivatingGrid(item)
    local isRepeatItem = checkIsRepeat()
    if isRepeatItem then
        if item:getCoverRule() == EliminateConfig.ItemCoverRule.Time or item:getCoverRule() == EliminateConfig.ItemCoverRule.Refresh then
            item:itemRepeatActive()
        else
            tryAddActivatingItem(item)
            item:itemRepeatActive()
        end
    else
        tryAddActivatingItem(item)
        item:itemActive()
    end
end

function this:onItemTimeUpGrid(item)
    print(string.format("道具持续时间结束: (%s,%s)", item:getItemIndex(), item:getItemName()))
    self.activatingItemDic[item:getItemIndex()] = nil
    self:itemActivatedEndGrid(item)
    self:poolItem(item)
end

function this:onExitReq()
    self.isGameStarted = false
end

function this:onExitGrid()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(TipTimerKey)
    -- 清除实体对象
    self:clearEntity()
    self:resetStartInfo()
    self:resetBaseInfo()
    self:resetCoroutine()
    self.isTiping = false
    self.tipEffectList = nil
    self.waitSpecialQueue = nil
    self.waitAccountQueue = nil
    self.waitEliAutoQueue = nil
    self.waitEliHandQueue = nil
    self.droppingItemDic = nil
    self._instance = nil
end

