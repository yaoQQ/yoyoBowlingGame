--- 客户端单人测试
--- Created by lichongzhi.
--- DateTime: 2017/9/26 16:57
---

require "base:enum/NoticeType"
require "eliminate:module/eliminate/PersonalExtend"
require "eliminate:module/eliminate/ObjectPool"
require "eliminate:module/eliminate/EliminateExtension"
require "eliminate:module/eliminate/EliminateAudioManager"
require "eliminate:module/eliminate/view/EliminateGrid"
require "eliminate:module/eliminate/view/EliminateScene"

EliminateClientTest = {}
local this = EliminateClientTest

local ObjectPoolManager = CS.ObjectPoolManager
local NoticeManager = CS.NoticeManager

-- 寻找指定点的二环四邻接点测试
function this:getNeighborTest(x, y)
    local neighbor = EliminateExtension:getNeighbor(x, y)
    print("neighbor = "..table.tostring(neighbor))
    local  hLeft = {neighbor[1], neighbor[2]}
    local hRight ={neighbor[3], neighbor[4]}
    local vDowm = {neighbor[5], neighbor[6]}
    local vUp = {neighbor[7], neighbor[8]}
    print("hLeft = "..table.tostring(hLeft))
    print("hRight = "..table.tostring(hRight))
    print("vDowm = "..table.tostring(vDowm))
    print("vUp = "..table.tostring(vUp))

end

function this:arrayLengthTest()
    local list1 = {1,2,3,4,5,6}
    print(string.format("list1 = %s, lengh = %s", table.tostring(list1), #list1))
    local list2 = {1,2,3,6}
    print(string.format("list2 = %s, lengh = %s", table.tostring(list2), #list2))
end

function this:arrayLengthRemoveTest()
    local list1 = {1,2,3,4,5,6}
    print(string.format("pre = %s, lengh = %s", table.tostring(list1), #list1))

    local function excludeColor(colorList, color)
        for i = 1, #colorList do
            if colorList[i] == color then
                table.remove(colorList, i)
            end
        end
        return colorList
    end
    excludeColor(list1, 4)
    print(string.format("after = %s, lengh = %s", table.tostring(list1), #list1))
end

function this:tableContactTest()
    local tb1 = {"1","2"}
    local tb2 = {"3","4"}
    local tb = table.merge(tb1, tb2)
    print("tb = "..table.tostring(tb))
end

function this:tableShuffleTest()
    --local pre = {1, 2, 3, 4, 5, 6}
    local pre = {}
    for i = 1, 5 do
        pre[tostring(i)] = i
    end
    print("pre = ".. table.tostring(pre))
    local after = math.shuffle(pre)
    print("after = ".. table.tostring(after))

end

function this:tableExtendTest()
    local list1 = {1, 2, 3, 4, 5}
    local list2 = {6, 7}
    table.extend(list1, list2)
    print("list1 = "..table.tostring(list1))
end

-- 数组去重测试
function this:arrayDistinct()
    local array = {1, 1, 3, 3, 4, 5, 5}
    print("array = " ..table.tostring(array))
    local arrayHashSet = {}
    for i = 1, #array do
        local key = tostring(array[i])
        if arrayHashSet[key] == nil then
            arrayHashSet[key] = array[i]
        end
    end
    print("arrayHashSet = "..table.tostring(arrayHashSet))
    array = {}
    local l = 1
    for _, v in pairs(arrayHashSet) do
        array[l] = v
        l = l + 1
    end
    print("array = " ..table.tostring(array))
end

-- 对象数组去重测试
function this:arrayObjDistinct()
    local array = {{x = 1, y = 1},{x = 1, y = 1},{x = 2, y = 1},{x = 3, y = 1},{x = 3, y = 1} }
    print("array = " ..table.tostring(array))
    local arrayHashSet = {}
    for i = 1, #array do
        local key = string.format("%s%s", array[i].x,array[i].y)
        if arrayHashSet[key] == nil then
            arrayHashSet[key] = array[i]
        end
    end
    print("arrayHashSet = "..table.tostring(arrayHashSet))
    array = {}
    local l = 1
    for _, v in pairs(arrayHashSet) do
        array[l] = v
        l = l + 1
    end
    print("array = " ..table.tostring(array))
end

function this:createEffectTest()
    EffectManager.Instance:CreateEffect("eliminate", "fx_toush02",  function (Object)
        print("Object加载完毕~")
    end)
end

function this:shuffleItemEqualTest()
    local function isEqualShuffleItem(leftItem, rightItem)
        if self:isEqualPoint(leftItem.source, rightItem.source) and self:isEqualPoint(leftItem.target, rightItem.target)then
            return true
        elseif self:isEqualPoint(leftItem.source, rightItem.target) and self:isEqualPoint(leftItem.target, rightItem.source) then
            return true
        else
            return false
        end
    end
    local item1 = {}
    item1.source = {x = 1, y = 0}
    item1.target = {x = 2, y = 0}
    local item2 = {}
    item2.source = {x = 2, y = 0}
    item2.target = {x = 1, y = 0}

    print(isEqualShuffleItem(item1, item2))
end

function this:defaultSortTest()
    local list = {1, 3, 4, 2, 5}
    print("排序前, list = "..table.tostring(list))

    table.sort(list)
    print("排序后, list = "..table.tostring(list))
end

function this:evaluateTest()
end

function this:specialTest()
end

function this:logErrorPieceTest()
    --print("测试-logErrorPieceTest")
    EliminateGrid:GetInstance():checkIsHaveWaitEliTest()
end

function this:autoSwapTest()
    self.isAutoSwap = not self.isAutoSwap
    EliminateGrid:GetInstance():autoSwapGridTest(self.isAutoSwap)
    --if self.isAutoSwap then
    --    print("测试-开始自动交换测试")
    --else
    --    print("测试-关闭自动交换测试")
    --end
end

function this:gridBFSTest()
    local eliList = EliminateGrid:GetInstance():findEliByBFS({x = 1,y = 3})
    local map = EliminateGrid:GetInstance():getGridMap()
    print("eliListBFS = "..table.tostring(eliList))
    for i = 1, #eliList do
        local p = map[eliList[i].x][eliList[i].y]
        p:onSelectedPiece()
    end
end

function this:getCross_DanceMomentTest()
    local map = EliminateGrid:GetInstance():getGridMap()
    local x = math.random(0, map.columnMax - 1)
    local y = math.random(0, map.rowMax - 1)
    --local eliList = EliminateExtension:getCross_DanceMoment({x = x, y = y}, map)
    --print(string.format("模拟的炫舞时刻十字雷 = (%s,%s)",x, y))
    local eliList = EliminateExtension:getBombNear_DanceMoment(x,y,map)
    print(string.format("模拟的炫舞时刻炸弹雷 = (%s,%s)",x, y))
    for y = 0, map.rowMax - 1 do
        for x = 0, map.columnMax - 1 do
            local p = map[x][y]
            p:updateSprite()
        end
    end
    for i = 1, #eliList do
        local p = map[eliList[i].x][eliList[i].y]
        p:onSelectedPiece()
    end
end

function this:corStopTest()
    self.corTest = nil
    self.corTest = coroutine.start(function ()
        while true do
            print("Coring")
            coroutine.wait(self.corTest, 1000)
        end
    end)
end

function this:singleRandomTest()
    local count_1 = 0
    local testCount = 10
    local testPro = 0.3
    for i = 1, testCount do
        local r = math.single_prob(testPro)
        if r == 1 then
            count_1 = count_1 + 1
        end
    end
    print(string.format("实验次数=%s, 实验概率=%s,实际出现1的次数=%s, 实际概率=%s",testCount,testPro,count_1,count_1/testCount))
end

function this:gridDeadTest()
    local map = EliminateGrid:GetInstance():getGridMap()
    local allMayList = EliminateExtension:findMayEliminateList(map)
    if table.empty(allMayList) then
        print("死图")
    end
end

function this:mapTest()
    local map = {}
    map.rowMax = 3
    map.columnMax = 3
    for x = 0, map.columnMax - 1 do
        map[x] = {}
        for y = 0, map.rowMax - 1 do
            map[x][y] = {t = x * 10 + y}
        end
    end
    print("map = "..table.tostring(map))
    print("map11 = "..table.tostring(map[1][1]))
end

function this:isEqualPoint(left, right)
    if table.empty(left) or table.empty(right) then
        return false
    end
    if left.x == right.x and left.y == right.y then
        return true
    end
    return false
end

function this:newTableTest()
    print("新lua表格测试")
    local temp = TableEliminateItemDataBase.data[1].endEffect
    print("temp = "..(temp))
end

function this:droppedTest(uid)
    print("掉线测试")

end

function this:noEliTest()
    print("不能正确消除测试")
    local curPointEliList = EliminateGrid:GetInstance():getWaitEliListByMatchList({{x = 2, y = 0}, {x = 3, y = 0}})
    EliminateGrid:GetInstance():updateWaitEliQueue(curPointEliList, EliminateConfig.OperateMode.Auto)
    EliminateGrid:GetInstance():clearAutoMatches()
    EliminateGrid:GetInstance():fill()
end

function this:viewDestroyTest()
    print("view删除测试")
    EliminatePoolView:onDestroy()
end

function this:quickEnterTest()
    print("快速进入测试")
    NoticeManager.Instance:Dispatch(EliminateNoticeType.QuickEnterReq)
end