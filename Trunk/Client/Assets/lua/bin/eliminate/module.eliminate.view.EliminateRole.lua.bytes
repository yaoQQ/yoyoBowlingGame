---
--- Created by Lichongzhi.
--- DateTime: 2018\3\13 0013 14:12
---
local Loger = CS.Loger
local Time = CS.UnityEngine.Time
local Transform = CS.UnityEngine.Transform
local UICamera = CS.UIManager.Instance.UICamera


EliminateRole = {}
local this = EliminateRole
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.animator = nil
this.transform = nil
this.rank = 0
this.moveCro = nil

this.STAGE_WIDTH = 0
this.name_text = nil
this.score_text = nil
this.playerData = nil   -- 玩家数据(EliminatePlayerInfo)

function this:InitRole(data, container)
    self.playerData = data
    local prefab = EliminateScene:GetRolePrefabBySex(data.sex)
    local cell = GameObject.Instantiate(prefab)
    cell.gameObject:SetActive(true)
    self.animator = cell.SceneAnimator
    self.transform = cell.transform
    self.transform:SetParent(container.transform)
    local firstPosX = this.STAGE_WIDTH / 2
    local deltaX = this.STAGE_WIDTH / (EliminateConfig.ROLE_COUNT)
    local posX = -firstPosX + (data:getSeat()) * deltaX
    self.transform.localPosition = Vector3(posX, 0, -1)
    self.transform.gameObject.name = tostring(data:getName())
    local item = EliminateGameView:GetDynamicUIByUid(data:getUid())

    item.name_text.DynamicUI:InitDynamicUI(self.transform, UICamera, Vector3(0,1,0), Vector2(1080,1920), false)
    item.score_text.DynamicUI:InitDynamicUI(self.transform, UICamera, Vector3(0,0.7,0), Vector2(1080,1920), false)
    item.arrow_image.DynamicUI:InitDynamicUI(self.transform, UICamera, Vector3(0,0.3,0), Vector2(1080,1920), false)
    self.name_text = item.name_text
    self.score_text = item.score_text
    item.go:SetActive(true)
    item.name_text.gameObject:SetActive(false)
    item.score_text.gameObject:SetActive(false)
    item.arrow_image.gameObject:SetActive(false)
    self:updateRoleUI()
    self:playAnimation("Wait")
    self:createDanceEffect()
end

function this:getRoleTransform()
    return self.transform
end

function this:playAnimation(animName)
    self.animator:PlayAnim(animName)
end

function this:onSwapRole(newRank, time)
    if self.rank == newRank then
        return
    end
    self.rank = newRank
    local startPos = self.transform.localPosition
    local targetPos = self:getLocalPosByRank(newRank)
    if self.moveCro~= nil then
        self.moveCro = nil
    end
    self.moveCro = coroutine.start(function ()
        for t = 0, time, Time.fixedDeltaTime do
            if self.transform ~= nil then
                self.transform.localPosition = Vector3.Slerp(startPos, targetPos, t / time);
                coroutine.step(self.moveCro)
            end
        end
        self.transform.localPosition = targetPos
    end)
end

function this:updateRoleUI()
    self.name_text.text = tostring(self.playerData:getName())
    self.score_text.text = tostring(self.playerData:getScore())
end

function this:onScoreChangeRole()
    self:playAnimation("Dance")
    self:updateRoleUI()
end

function this:onDanceStartRole()
    self:playAnimation("WORSHIP")
    local targetRot = self:getLocalRotateByRank(self.rank)
    self.transform.localEulerAngles = targetRot
end

function this:onDanceOverRole()
    self:playAnimation("Dance")
    local targetRot = self:getLocalRotateByRank(1)
    self.transform.localEulerAngles = targetRot
end

function this:onTimeUpRole()
    self:playAnimation("Wait")
    local targetRot = self:getLocalRotateByRank(1)
    self.transform.localEulerAngles = targetRot
end

function this:getLocalRotateByRank(rank)
    local rotate = Vector3.zero
    if rank == 1 then
        rotate.y = 180
    elseif rank == 2 then
        rotate.y = 90
    elseif rank == 3 then
        rotate.y = -90
    elseif rank == 4 then
        rotate.y = 180
    end
    return rotate
end

function this:getLocalPosByRank(rank)
    local pos = Vector3.zero
    if rank == 1 then
        pos.z = -1
    elseif rank == 2 then
        pos.x = -1
    elseif rank == 3 then
        pos.x = 1
    elseif rank == 4 then
        pos.z = 1
    end
    return pos
end

function this:GetRank()
    return self.playerData:getRank()
end

local dazzleEffectList = {}
function this:createDazzleEffect()
    local headVFX = ObjectPool:GetInstance():getObject("x_fx_xuanwu_001")
    local handVFX = nil
    if self.playerData.sex == 1 then
        handVFX = ObjectPool:GetInstance():getObject("x_fx_tuowei_boy")
    else
        handVFX = ObjectPool:GetInstance():getObject("x_fx_tuowei_girl")
    end
    local headParent = self.transform
    local handParent = self.transform:Find("Bip002/Bip002 Pelvis/Bip002 Spine/Bip002 Spine1/Bip002 Neck/Bip002 L Clavicle/Bip002 L UpperArm/Bip002 L Forearm/Bip002 L Hand/Bip002 L Finger0")
    headVFX.transform:SetParent(headParent)
    headVFX.transform.localPosition = Vector3.zero
    handVFX.transform:SetParent(handParent)
    handVFX.transform.localPosition = Vector3.zero
    table.insert(dazzleEffectList, headVFX)
    table.insert(dazzleEffectList, handVFX)
end

function this:poolDazzleEffect()
    for i, v in pairs(dazzleEffectList) do
        ObjectPool:GetInstance():poolObject(v)
    end
    dazzleEffectList = {}
end

local danceEffectList = {}
function this:createDanceEffect()
    local footParent = self.transform
    local footVFX = ObjectPool:GetInstance():getObject("x_fx_light_girl")
    footVFX.transform:SetParent(footParent)
    footVFX.transform.localPosition = Vector3.zero
    table.insert(danceEffectList, headVFX)
end

function this:poolDanceEffect()
    for i, v in pairs(danceEffectList) do
        ObjectPool:GetInstance():poolObject(v)
    end
    danceEffectList = {}
end

function this:onExitRole()
    self:poolDanceEffect()
    self:poolDazzleEffect()
    coroutine.stop(self.moveCro)
    self.moveCro = nil
    GameObject.Destroy(self.transform.gameObject)
end