---
--- Created by Lichongzhi.
--- DateTime: 2017/11/28 14:29
---

require "base:enum/NoticeType"

CoinPiece = {}
local this = CoinPiece
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--data
this.key = ""
this.faceValue = 0
--component
this.gameObject = nil
this.rectTransform = nil
this.imageWidget = nil
this.transform = nil

function this:InitCoinPiece(_key, _faceValue, gameObject)
    self.key = tostring(_key)
    self.faceValue = _faceValue
    self.gameObject = gameObject
    self.rectTransform = gameObject:GetComponent(typeof(RectTransform))
    self.transform = gameObject:GetComponent(typeof(CS.UnityEngine.Transform))
    self.imageWidget = gameObject:GetComponent(typeof(ImageWidget))
    if _faceValue == 1 then
        self.imageWidget:SetPng(CoinPoolView:GetCoin1Sprite())
        self.rectTransform.sizeDelta = Vector2(CoinConfig.CoinSize.Coin1.x, CoinConfig.CoinSize.Coin1.y)
    elseif _faceValue == 2 then
        self.imageWidget:SetPng(CoinPoolView:GetCoin2Sprite())
        self.rectTransform.sizeDelta = Vector2(CoinConfig.CoinSize.Coin2.x, CoinConfig.CoinSize.Coin2.y)
    elseif _faceValue == 5 then
        self.imageWidget:SetPng(CoinPoolView:GetCoin5Sprite())
        self.rectTransform.sizeDelta = Vector2(CoinConfig.CoinSize.Coin5.x, CoinConfig.CoinSize.Coin5.y)
    end
    self.imageWidget:AddEventListener(UIEvent.PointerDown, this.onPointerDown)
    self.imageWidget:AddEventListener(UIEvent.Drag, this.onPointerDrag)
    self.imageWidget:AddEventListener(UIEvent.PointerUp, this.onPointerUp)
    self.gameObject.name = tostring(_key)
end

function this:SetPieceParent(root)
    self.rectTransform:SetParent(root)
    self.rectTransform.localScale = Vector3.one
end

function this:PlaySpawnAnim()
    --print("播放出生动画")
    self.transform.localScale = Vector3.zero
    self.transform:DOScale(Vector3.one, 0.3)
end

function this:PlayPoolAnim()
    self.transform.localScale = Vector3.one
    self.transform:DOScale(Vector3.zero, 0.3)
end

function this:SetPiecePosition(pos)
    self.rectTransform.anchoredPosition = pos
end

function this:GetPiecePosition()
    return self.rectTransform.anchoredPosition
end

function this:GetPieceWorldPosition()
    return self.rectTransform.position
end

function this:SetPieceLast()
    self.rectTransform:SetAsLastSibling()
end

function this:DestroyPiece()
    GameObject.Destroy(self.gameObject)
    self.gameObject = nil
    self.rectTransform = nil
    self.imageWidget = nil
    self.transform = nil
end

function this:GetPieceKey()
    return self.key
end

function this.checkIsCanUpCoin(key)
    local isCanUp = false
    if CoinGameView:GetCurMoveCoin() == nil then
        isCanUp = true
    else
        local curKey = CoinGameView:GetCurMoveCoin():GetPieceKey()
        print(string.format("curKey: %s, _key: %s",curKey, key))
        if curKey == key then
            isCanUp = true
        else
            isCanUp = false
        end
    end
    print("isCanUp: "..tostring(isCanUp))

    return isCanUp
end
--===========================================================--
function this.onPointerDown(eventData)
    if Input.touchCount >= 2 then
        return
    end
    if eventData.pointerCurrentRaycast.gameObject == nil then
        return
    end
    local key = eventData.pointerCurrentRaycast.gameObject.name
    --print(string.format("按下了%s", key))
    NoticeManager.Instance:Dispatch(CoinNoticeType.PointerDown, key)

end

function this.onPointerDrag(eventData)
    if Input.touchCount >= 2 then
        return
    end
    --print(string.format("拖动了%s",eventData.pointerCurrentRaycast.gameObject.name))
    NoticeManager.Instance:Dispatch(CoinNoticeType.PointerDrag, eventData)
end

function this.onPointerUp(eventData)
    if Input.touchCount >= 2 then
        --print(string.format("当前不能抛出松开事件,key: %s", key))
        return
    end
    --print(string.format("抛出松开事件,key: %s", key))
    NoticeManager.Instance:Dispatch(CoinNoticeType.PointerUp)
end