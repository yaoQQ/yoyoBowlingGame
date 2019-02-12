---
--- Created by Lichongzhi.
--- DateTime: 2018\2\7 0007 10:13
---

EliminateBaseGrid = {}
local this = EliminateBaseGrid
local RectTransformUtility = CS.UnityEngine.RectTransformUtility
local UICamera = CS.UIManager.Instance.UICamera

local Screen = CS.UnityEngine.Screen
local Loger = CS.Loger

-- 逻辑类数据
this.xDim = 0   -- 列数
this.yDim = 0   -- 行数
this.xAreaList = {}
this.yAreaList = {}
this.tipEffectList = nil
-- gameObject类引用
this.bgPrefab = nil
this.piecePrefab = nil
this.itemPrefab = nil
this.pieceBgRoot = nil
this.pieceRoot = nil
this.itemRoot = nil

this.pieceBgPoolRoot = nil      -- pieceBg对象池的父节点
this.piecePoolRoot = nil        -- piece对象池的父节点

function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

-- 初始化关卡配置
function this:initConfig()
    print("消消-BaseGrid-初始化关卡配置")
    if EliminateConfig.Row < 3 and EliminateConfig.Column < 3 then
        Loger.PrintError("错误-消消-三消游戏不允许2*2以下的地图")
        return
    end
    self.xDim = EliminateConfig.Column
    self.yDim = EliminateConfig.Row
    self.fillTime = EliminateConfig.EACH_COLUMN_FILL_TIME
    self.swapTime = EliminateConfig.SWAP_TIME
    self:initPieceArea()
end

-- 初始化块的屏幕坐标与Grid关系列表
function this:initPieceArea()
    local firstPieceX = 0 -  EliminateConfig.pxPerUnit *  ((self.xDim - 1) / 2)
    local firstPieceY = -250 +  EliminateConfig.pxPerUnit *  ((self.yDim - 1)/ 2)

    for x = 0, self.xDim - 1 do
        local left = firstPieceX + x * EliminateConfig.pxPerUnit - EliminateConfig.tileUnit / 2
        local right = firstPieceX + x * EliminateConfig.pxPerUnit + EliminateConfig.tileUnit / 2
        self.xAreaList[x] = {left, right}
    end
    for y = 0, self.yDim - 1 do
        local down = firstPieceY - y * EliminateConfig.pxPerUnit - EliminateConfig.tileUnit / 2
        local up = firstPieceY - y * EliminateConfig.pxPerUnit + EliminateConfig.tileUnit / 2
        self.yAreaList[y] = {down, up}
    end
    --print("xAreaList = "..table.tostring(self.xAreaList))
    --print("yAreaList = "..table.tostring(self.yAreaList))
end

function this:initComponentRef()
    --print("消消-Grid-初始化组件类型引用")
    local info_1 = EliminatePoolView:getInfo()
    self.bgPrefab = info_1.main_mid.pieceBgItem.gameObject
    self.piecePrefab = info_1.main_mid.pieceItem.gameObject
    self.itemPrefab = info_1.main_mid.itemItem.gameObject
    self.pieceBgPoolRoot = info_1.main_mid.pieceBg_pool_root.transform
    self.piecePoolRoot = info_1.main_mid.piece_pool_root.transform
    local info_2 = EliminateGameView:getInfo()
    self.pieceBgRoot = info_2.main_mid.pieceBg_container.transform
    self.pieceRoot = info_2.main_mid.piece_container.transform
    self.itemRoot = info_2.main_mid.item_container.transform
end

-- 初始化池
function this:initPool()
    --print("消消-Grid-初始化Pool")
    self.piecePool = Queue.new()
    self.pieceBgPool = Queue.new()
    self.itemPool = Queue.new()
    for i = 1, self.xDim * self.yDim do
        local bgItem = self:createPieceBg()
        Queue.enqueue(self.pieceBgPool, bgItem)
    end
    for i = 1, (self.xDim * self.yDim) * 2  do
        local piecesItem = self:createPiece()
        Queue.enqueue(self.piecePool, piecesItem)
    end
    for i = 1, 20  do
        Queue.enqueue(self.itemPool, self:createItem())
    end
end

function this:createPiece()
    local obj = GameObject.Instantiate(self.piecePrefab)
    obj.transform:SetParent(self.piecePoolRoot)
    obj.name = "pieceEmpty"
    local item = EliminatePiece:new()
    item:initPieceComponentData(obj)
    return item
end

function this:createPieceBg()
    local obj = GameObject.Instantiate(self.bgPrefab)
    obj.transform:SetParent(self.pieceBgPoolRoot)
    local item = EliminatePieceBg:new()
    item:initBgComponentData(obj)
    return item
end

this.itemIndex = 0
function this:createItem()
    local obj = GameObject.Instantiate(self.itemPrefab)
    local item = EliminateItem:new()
    self.itemIndex = self.itemIndex + 1
    item:initItemLogicData(self.itemIndex)
    item:initItemComponentData(obj)
    item:setItemParent(self.itemRoot)
    item:setItemPosition(EliminateConfig.POOL_POS)
    return item
end

function this:poolPiece(item)
    if item ~= nil then
        item:setPiecePosition(EliminateConfig.POOL_POS)
        Queue.enqueue(self.piecePool, item)
    end
end

function this:poolItem(item)
    if table.empty(item) then
        return
    end
    if table.empty(self.droppingItemDic) == false then
        self.droppingItemDic[item:getItemIndex()] = nil
    end
    item:itemActivated()
    Queue.enqueue(self.itemPool, item)
end

-- 初始化piece背景
function this:initPieceBg()
    --print("消消-Grid-初始化pieceBg")
    self.piecesBgDic = {}
    -- 背景
    for x = 0, self.xDim - 1 do
        self.piecesBgDic[x] = {}
        for y = 0, self.yDim - 1 do
            local bgItem = nil
            if self.pieceBgPool.count < 1 then
                local item = self:createPieceBg()
                Queue.enqueue(self.pieceBgPool, item)
            end
            bgItem = Queue.dequeue(self.pieceBgPool)
            bgItem:initBgLogicData(x, y)
            bgItem:setParent(self.pieceBgRoot)
            bgItem:setSize(EliminateConfig.pxPerUnit, EliminateConfig.pxPerUnit)
            bgItem:setPosition(self:getPositionByGrid(x, y))
            bgItem:updatePieceBgName()
            bgItem:activeFg(false)
            self.piecesBgDic[x][y] = bgItem
        end
    end
end

function this:getItemFromPool()
    if self.itemPool.count < 1 then
        Queue.enqueue(self.itemPool, self:createItem())
    end
    return  Queue.dequeue(self.itemPool)
end

function this:getItemFromActivatingOrPool(type)
    local item = nil
    if table.empty(self.activatingItemDic) == false then
        local isRepeat = false
        for _, v in pairs(self.activatingItemDic) do
            if v:getItemType() == type then
                if v:getCoverRule() == EliminateConfig.ItemCoverRule.Time or v:getCoverRule() == EliminateConfig.ItemCoverRule.Refresh then
                    isRepeat = true
                    item = v
                    break;
                end
            end
        end
        if isRepeat == false then
            item = self:getItemFromPool()
        end
    else
        item = self:getItemFromPool()
    end
    return item
end

function this:getPointByPosition(screenPos)
    --print(string.format("对应屏幕坐标系(%s, %s)", screenPos.x, screenPos.y))
    --local ratio = EliminateConfig.standardWidth / Screen.width
    --local localX, localY = screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2
    --print(string.format("对应当前屏幕分辨率的局部坐标系(%s, %s)", localX, localY))
    --print(string.format("对应适配后的屏幕分辨率的局部坐标系(%s, %s)", localX * ratio, localY * ratio))
    --local pos = Vector3(localX * ratio, localY * ratio)
    local localCursor
    local isValue, localCursor = RectTransformUtility.ScreenPointToLocalPointInRectangle(EliminateGameView:GetControlRectTransform(), screenPos, UICamera, localCursor)
    if isValue == false then
        return
    end
    --print(string.format("localCursor:(%s,%s)",localCursor.x, localCursor.y))
    local pos = localCursor
    local x = -1
    local y = -1
    for k, v in pairs(self.xAreaList) do
        if pos.x >= v[1] and pos.x <= v[2] then
            x = k
        end
    end
    for k, v in pairs(self.yAreaList) do
        if pos.y >= v[1] and pos.y <= v[2] then
            y = k
        end
    end
    if x == -1 or y == -1 then
        return nil
    end
    return {x = x, y = y}
end

-- 检查是否存在能洗图的可能
function this:checkCanShuffle()
    local colorCountDic = {}
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local p = self.piecesDic[x][y]
            local color = p:getColor()
            if colorCountDic[tostring(color)] == nil then
                colorCountDic[tostring(color)] = 1
            else
                colorCountDic[tostring(color)] = colorCountDic[tostring(color)] + 1
            end
        end
    end
    local colorList = {}
    for _, v in pairs(colorCountDic) do
        table.insert(colorList, v)
    end
    local maxColorCount = math.max(table.unpack(colorList))
    --print("maxColorCount = "..maxColorCount)
    if maxColorCount < 3 then
        return false
    end
    return true
end

-- 检测楼上的交换失败状态
function this:checkHaveSwapReturn(upSwapFailList)
    local isHaveUpSwapReturn = false
    if table.empty(upSwapFailList) then
        return isHaveUpSwapReturn
    end
    for _, v in pairs(upSwapFailList) do
        if v:getSwapFailState() then
            isHaveUpSwapReturn = true
            break
        end
    end
    return isHaveUpSwapReturn
end

-- 检测全图是否还有正在移动的, 包括正在交换的,交换失败而返回的,下落的
function this:checkHaveMoving()
    local isHave = false
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local piece = self.piecesDic[x][y]
            if piece:isCanMove() == false then
                isHave = true
                break
            end
        end
    end
    return isHave
end

-- 显示一种提示结果
function this:showOneMayEliTip()
    local allMayList = EliminateExtension:findMayEliminateList(self.piecesDic)
    if table.empty(allMayList) == false then
        local mayEliList = allMayList[1]
        --print("allMayList = "..table.tostring(allMayList))
        self.isTiping = true
        self:showMayEliTip(mayEliList)
    end
end

function this:showMayEliTip(mayEliList)
    if self.tipEffectList == nil then
        self.tipEffectList = {}
    end
    for i = 1, #mayEliList do
        local localPos = self:getPositionByGrid(mayEliList[i].x, mayEliList[i].y)
        local obj = ObjectPool:GetInstance():getObject("fx_tip")
        local pos = self.pieceRoot:TransformPoint(localPos)
        obj.transform.localPosition = pos
        table.insert(self.tipEffectList, obj)
    end
end

function this:resetBaseInfo()
    self.xDim = 0
    self.yDim = 0
    self.itemIndex = 0
    self.piecesBgDic = nil
    self.bgPrefab = nil
    self.piecePrefab = nil
    self.pieceBgRoot = nil
    self.pieceRoot = nil
    self.piecePool = nil
    self.pieceBgPool = nil

end

function this:poolAllTip()
    if table.empty(self.tipEffectList) then
        return
    end
    for i = 1, #self.tipEffectList do
        ObjectPool:GetInstance():poolObject(self.tipEffectList[i])
    end
    self.tipEffectList = {}
end

function this:clearEntity()
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            self.piecesBgDic[x][y]:onDestroy()
        end
    end
    for x = 0, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local p = self.piecesDic[x][y]
            p:onDestroy()
        end
    end
    if table.empty(self.droppingItemDic) == false then
        for _, v in pairs(self.droppingItemDic) do
            v:onDestroy()
        end
    end
    local function clearPool(pool)
        if table.empty(pool) then
            return
        end
        for i = 1, pool.count do
            local item = Queue.dequeue(pool)
            item:onDestroy()
        end
    end
    clearPool(self.piecePool)
    clearPool(self.pieceBgPool)
    clearPool(self.itemPool)
    --for i = 1, self.piecePool.count do
    --    local item = Queue.dequeue(self.piecePool)
    --    item:onDestroy()
    --end
    --for i = 1, self.pieceBgPool.count do
    --    local item = Queue.dequeue(self.pieceBgPool)
    --    item:onDestroy()
    --end
    --for i = 1, self.itemPool.count do
    --    local item = Queue.dequeue(self.itemPool)
    --    item:onDestroy()
    --end
end

--=======================================test===============================
function this:checkIsHaveWaitEliTest()
    local errorList = {}
    for x = 1, self.xDim - 1 do
        for y = 0, self.yDim - 1 do
            local piece = self.piecesDic[x][y]
            if piece:isCanMove() == false then
                table.insert(errorList, {info = piece:logIsCanMatch()})
            end
        end
    end
    if table.empty(errorList) == false then
        Loger.PrintError("消消-检测异常, 有异常块, errorList = "..table.tostring(errorList))
    else
        Loger.PrintLog("消消-检测异常, 方块正常")

    end
end

-- 自动交换测试
function this:autoSwapGridTest(isDo)
    if not isDo then
        coroutine.stop(self.autoSwapCro)
        self.autoSwapCro = nil
        return
    end
    self.autoSwapCro = nil
    self.autoSwapCro = coroutine.start(function ()
        while true do
            while self.isGameStarted == false do
                coroutine.step(self.autoSwapCro)
            end
            while self.isShufflingMap do
                coroutine.step(self.autoSwapCro)
            end
            while self.isAccountClear do
                coroutine.step(self.autoSwapCro)
            end
            coroutine.step(self.autoSwapCro)
            local allMayList = EliminateExtension:findMayEliminateList(self.piecesDic)
            --print("allMayList = "..table.tostring(allMayList))
            if table.empty(allMayList) == false then
                --local swapCount = math.random(1, #allMayList)
                local swapCount = #allMayList
                --print(string.format("可自动交换总次数 = %s,自动交换, swapCount = %s",#allMayList, swapCount))
                local curSwapCount = 0
                local listIndex = #allMayList
                while(curSwapCount < swapCount) do
                    local length = table.len(allMayList)
                    local randId = math.random(1, length)
                    --local mayEliList = allMayList[randId]
                    --local mayEliList = allMayList[curSwapCount + 1]
                    local mayEliList = allMayList[listIndex]
                    listIndex = listIndex - 1
                    --print(string.format("自动交换, mayEliList = %s", table.tostring(mayEliList)))
                    self:resetSwapInfo()
                    self.swapPointList[1] = mayEliList[1]
                    self.swapPointList[2] = mayEliList[2]
                    self:updateWaitAccountQueue()
                    self:updateSwapAccount()
                    self:resetSwapInfo()
                    --table.remove(allMayList, randId)
                    curSwapCount = curSwapCount + 1
                    coroutine.wait(self.autoSwapCro, 100)
                end
                local turnTime = math.random(1000, 2000)
                coroutine.wait(self.autoSwapCro, turnTime)
            end
        end
        coroutine.step(self.autoSwapCro)
    end)
end