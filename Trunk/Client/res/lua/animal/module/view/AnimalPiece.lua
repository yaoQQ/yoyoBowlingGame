---
--- Created by Lichongzhi.
--- DateTime: 2018/11/16 14:29
---

require "base:enum/NoticeType"

AnimalPiece = {}
local this = AnimalPiece
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

--data
this.x = 0
this.y = 0
this.pieceState = AnimalConfig.PieceState.NotOpen
this.pieceSeat = -1  -- 棋子所属的阵营,0为蓝,1为红
this.pieceId = -1  -- 棋子id, 0~7, 鼠->象
--component
this.rectTransform = nil
this.transform = nil
this.animal_icon = nil
this.faction_icon = nil
this.animal_image = nil
this.animal_name = nil

function this:InitAnimalPiece(x, y, gameObject)
    self.x = x
    self.y = y
    self.rectTransform = gameObject:GetComponent(typeof(RectTransform))
    self.transform = gameObject:GetComponent(typeof(CS.UnityEngine.Transform))
    self.animal_icon = gameObject:GetComponent(typeof(IconWidget))
    self.faction_icon = self.transform:Find("faction_icon"):GetComponent(typeof(IconWidget))
    self.animal_image = self.transform:Find("faction_icon/animal_image"):GetComponent(typeof(ImageWidget))
    self.animal_name = self.transform:Find("faction_icon/animal_name"):GetComponent(typeof(TextWidget))

    self.animal_icon:AddEventListener(UIEvent.PointerClick, function()
        self:onClickHandler(self.x, self.y)
    end)

    self:ShowNotOpenPiece()
    --self.transform.gameObject.name = string.format("%s%s",self.x, self.y)

end

function this:showNameTest()
    self.transform.gameObject.name = string.format("%s->%s%s",self.transform.gameObject.name, self.x, self.y)
end

function this:SetPieceParent(root)
    self.rectTransform:SetParent(root)
    self.rectTransform.localScale = Vector3.one
end

function this:SetPiecePosition(pos)
    self.transform.gameObject:SetActive(true)
    self.rectTransform.localPosition = Vector3(pos.x, pos.y, 0)
end

function this:GetPieceWorldPosition()
    return self.rectTransform.position
end

function this:SetPieceLast()
    self.rectTransform:SetAsLastSibling()
end

-- 更新piece索引
function this:UpdatePieceIndex(newX, newY)
    self.x = newX
    self.y = newY
    --self:showNameTest()
end



function this:DestroyPiece()
    GameObject.Destroy(self.transform.gameObject)
    self.rectTransform = nil
    self.transform = nil
    self.animal_icon = nil
    self.faction_icon = nil
    self.animal_image = nil
    self.animal_name = nil
end

--===========================================================--
function this:onClickHandler(x, y)
    if AnimalGameView:GetIsGameOver() then
        return
    end
    --print(string.format("按下了(%s,%s), state:%s", x, y, self.pieceSeat))
    if AnimalGameView:GetRoundState() == AnimalConfig.RoundState.Me then
        local curSelect = AnimalGameView:GetCurSelect()
        if curSelect == nil then
            if self.pieceState == AnimalConfig.PieceState.NotOpen then
                AnimalNetModule.ReqReverseChess(x, y)
            elseif self.pieceState == AnimalConfig.PieceState.Opened then
                if self:isMine() then
                    self.pieceState = AnimalConfig.PieceState.Selected
                    NoticeManager.Instance:Dispatch(AnimalNoticeType.Select, {x = x, y = y})
                else
                    AnimalGameView:ShowTip("无法控制对手棋子")
                end
            elseif self.pieceState == AnimalConfig.PieceState.Selected then
                printError(string.format("斗兽棋-错误, 没有正选择的棋子却点击了一个选择过的棋子", x, y))
            elseif self.pieceState == AnimalConfig.PieceState.Dead then
                -- 当前没有选择的棋子, 但点击了空棋
                print("当前没有选择的棋子, 点击了死棋")
            else
                printError("斗兽棋-错误")
            end
        else
            if (self.pieceState == AnimalConfig.PieceState.Opened and self:isMine() == false) or
                    self.pieceState == AnimalConfig.PieceState.Dead then
                if (x - curSelect.x)^2 + (y - curSelect.y)^2 <= 1 then
                    AnimalNetModule.ReqReqMoveChess({x = curSelect.x, y = curSelect.y}, {x = x, y = y})
                else
                    NoticeManager.Instance:Dispatch(AnimalNoticeType.SelectCancel)
                end
            else
                NoticeManager.Instance:Dispatch(AnimalNoticeType.SelectCancel)
            end
        end
    else
        AnimalGameView:ShowTip("对手回合中")
    end
end

function this:isMine()
    local meSeat = AnimalDataProxy:GetMeSeat()
    return self.pieceSeat == meSeat
end

-- 翻牌
function this:OnReversePiece(cardId, action)
    local spriteList = AnimalPoolView:GetNormalSprite()
    if cardId <= 7 then
        self.pieceSeat = 0
    else
        self.pieceSeat = 1
    end
    self.pieceId = cardId % 8
    self.faction_icon.gameObject:SetActive(true)
    self.animal_image.gameObject:SetActive(true)
    self.animal_name.gameObject:SetActive(true)
    self.faction_icon:ChangeIcon(self.pieceSeat)
    self.animal_image:SetPng(spriteList[self.pieceId])
    self.animal_name.text = AnimalConfig.PieceCnName[self.pieceId]
    self.pieceState = AnimalConfig.PieceState.Opened
    self.animal_image.transform.localScale = Vector3.zero
    self.animal_image.transform:DOScale(Vector3.one, 0.3):OnComplete(function ()
        action()
    end)
    AudioManager.playSound("animal", AnimalConfig.PieceEnName[self.pieceId])
end

-- 移动
function this:SetAsLastSibling()
    self.transform:SetAsLastSibling()
end

function this:ShowNotOpenPiece()
    self.transform.gameObject:SetActive(true)
    self.animal_icon:ChangeIcon(0)
    self.faction_icon.gameObject:SetActive(false)
    self.animal_image.gameObject:SetActive(false)
    self.animal_name.gameObject:SetActive(false)
    self.pieceState = AnimalConfig.PieceState.NotOpen
end

function this:ShowDeadPiece()
    self.transform.gameObject:SetActive(true)
    self.faction_icon.gameObject:SetActive(false)
    self.animal_image.gameObject:SetActive(false)
    self.animal_name.gameObject:SetActive(false)
    self.animal_icon:ChangeIcon(1)
    self.animal_image:SetPng(nil)
    self.pieceState = AnimalConfig.PieceState.Dead
    local info = {}
    info.seat = self.pieceSeat
    info.id = self.pieceId
    NoticeManager.Instance:Dispatch(AnimalNoticeType.Dead, info)
    self.pieceId = -1
    self.pieceSeat = -1
end

function this:ShowFightPiece()
    self.transform.gameObject:SetActive(false)
end

-- 被选中的表现
function this:OnSelectPiece()
    self:SetAsLastSibling()
    local spriteList = AnimalPoolView:GetSelectedSprite()
    self.animal_image:SetPng(spriteList[self.pieceId])
    self.faction_icon:ChangeIcon(self.pieceSeat + 2 )

    --self.rectTransform.localScale = Vector3.one * 1.2
    self.pieceState = AnimalConfig.PieceState.Selected
    AudioManager.playSound("animal", AnimalConfig.PieceEnName[self.pieceId])

end

-- 被取消选择的表现
function this:OnSelectCancelPiece()
    local spriteList = AnimalPoolView:GetNormalSprite()
    self.animal_image:SetPng(spriteList[self.pieceId])
    self.faction_icon:ChangeIcon(self.pieceSeat)
    --self.rectTransform.localScale = Vector3.one
    self.pieceState = AnimalConfig.PieceState.Opened

end

---==========================访问器==================================
function this:GetSeat()
    return self.pieceSeat
end

function this:GetState()
    return self.pieceState
end

function this:GetPieceId()
    return self.pieceId
end