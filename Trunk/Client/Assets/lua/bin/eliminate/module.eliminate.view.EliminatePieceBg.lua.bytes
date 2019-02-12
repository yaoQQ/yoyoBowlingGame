---
--- Created by Lichongzhi.
--- DateTime: 2017/11/28 14:29
---

require "base:enum/NoticeType"
require "eliminate:module/eliminate/PersonalExtend"
require "eliminate:module/eliminate/EliminateConfig"

local Loger = CS.Loger
local RectTransform = CS.UnityEngine.RectTransform
local Vector2 = CS.UnityEngine.Vector2
local Vector3 = CS.UnityEngine.Vector3
local Image = CS.UnityEngine.UI.Image
local GameObject = CS.UnityEngine.GameObject

EliminatePieceBg = {}
local this = EliminatePieceBg
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.X = 0
this.Y = 0
-- 组件类数据
this.rectTranfrom = nil
this.gameObject = nil
this.bgFgImgae = nil
this.fgRectTranfrom = nil

function this:initBgLogicData(_x, _y)
    self.X = _x
    self.Y = _y
end

function this:initBgComponentData(gameObject)
    self.gameObject = gameObject
    self.rectTranfrom = gameObject:GetComponent(typeof(RectTransform))
    self.bgFgImgae = gameObject.transform:Find("pieceBg_fg_image"):GetComponent(typeof(Image));
    self.fgRectTranfrom = gameObject.transform:Find("pieceBg_fg_image"):GetComponent(typeof(RectTransform))
end

function this:updatePieceBgName()
    self.gameObject.name = string.format("bg_%s%s", self.X, self.Y)
end

function this:setSize(w, h)
    self.rectTranfrom.sizeDelta = Vector2(w, h)
    self.fgRectTranfrom.sizeDelta = Vector2(w, h)
end

function this:setPosition(vector3)
    self.rectTranfrom.localPosition = vector3
end

function this:setParent(root)
    self.rectTranfrom:SetParent(root)
    self.rectTranfrom.localScale = Vector3.one
end

function this:getRectTranform()
    return self.rectTranfrom
end

function this:activeFg(state)
    self.bgFgImgae.gameObject:SetActive(state)
end

function this:lightFgItem()
    self.bgFgImgae.gameObject:SetActive(true)
    self.bgFgImgae:CrossFadeAlpha(1, 0, true)
    self.bgFgImgae:CrossFadeAlpha(0, 0.4, true)

end

function this:onDestroy()
    GameObject.Destroy(self.gameObject)
end

function this:onPool()

end



