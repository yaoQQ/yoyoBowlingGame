---
--- Created by Lichongzhi.
--- DateTime: 2018\3\12 0012 18:04
---
require "eliminate:module/eliminate/view/EliminateRole"
require "eliminate:module/eliminate/view/EliminateGameView"

EliminateScene = BaseScene:new()
local this = EliminateScene
this.sceneName="eliminate"
local Loger = CS.Loger

this.isEnter = false
this.roleDic = {}
this.rolePrefabList = {}

-- 注意: 由于C#场景加载时不是等场景所有对象都加载完才调用该函数, 因此要延迟几帧执行
function this:onEnter()
    if self:getIsInit() then
        return
    end
    self:endInit()

    print("=========================进入消除场景完毕===============>>>>>")

    local roleContainer = self:getContainer("role")
    table.insert(self.rolePrefabList, roleContainer.cellArr[0])
    table.insert(self.rolePrefabList, roleContainer.cellArr[1])
    --print("rolePrefabList: "..table.tostring(self.rolePrefabList))
    for _, v in pairs(self.rolePrefabList) do
        v.gameObject:SetActive(false)
    end

end

function this:onLeave()

end

--==================================访问器========================

function this:GetStageCamera()
    return self:getCamera("stageCamera")
end

function this:GetRolePrefabBySex(sex)
    if self:getIsInit() == false then
        printError("没有场景!")
        return nil
    end
    if sex == 1 then
        return self.rolePrefabList[1]
    else
        return self.rolePrefabList[2]
    end
end

--=======================================事件响应============================
function this:onEliminateEnterScene()
    local roleContainer = self:getContainer("role")
    local players = EliminateDataProxy:GetInstance():GetAllPlayer()
    for _, v in pairs(players) do
        local role = EliminateRole:new()
        role:InitRole(v, roleContainer)
        self.roleDic[v:getUid()] = role
    end
    EliminateGameView:onEnteredSceneGameView()
end

function this:onOverScene()
    for i, v in pairs(self.roleDic) do
        v:playAnimation("Victory")
    end
end

function this:onTimeUpScene()
    for i, v in pairs(self.roleDic) do
        v:onTimeUpRole()
    end
end

function this:onRankChangeScene()
   --local playerList = EliminateDataProxy:GetInstance():getAllPlayerInfo()
   -- for i = 1, #playerList do
   --     local player = playerList[i]
   --     local role = self.roleList[player:getSeat()]
   --     local rank = player:getRank()
   --     role:onSwapRole(rank, EliminateConfig.SWAP_ROLE_TIME)
   --     role:playAnimation("DANCE")
   -- end
end

function this:onScoreChangeScene()
    for i, v in pairs(self.roleDic) do
        v:onScoreChangeRole()
    end
end

function this:onComboScene(index)
    --local role = self.roleList[index]
    --role:playAnimation("DANCE")
end

function this:onComboBreakScene(index)
    for i, v in pairs(self.roleDic) do
        v:playAnimation("Dance")
    end
end

function this:OnDazzleStartScene()
    local player = nil
    for i, v in pairs(self.roleDic) do
        if v:GetRank() == 1 then
            player = v
        end
    end
    if player == nil then
        return
    end
    player:playAnimation("Dazzle")
    player:createDazzleEffect()
end

function this:OnDazzleOverScene()
    local player = nil
    for i, v in pairs(self.roleDic) do
        if v:GetRank() == 1 then
            player = v
        end
    end
    if player == nil then
        return
    end
    player:playAnimation("Dance")
    player:poolDazzleEffect()
end

function this:onExitScene()
    if self:getIsInit() == false then
        return
    end

    for i, v in pairs(self.roleDic) do
        v:onExitRole()
    end
    self.roleDic = {}
    self.rolePrefabList = {}
    self.isEnter = false
    self:leave()
end

