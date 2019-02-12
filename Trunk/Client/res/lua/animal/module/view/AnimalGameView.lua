---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---
require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "animal:module/view/AnimalPiece"
require "animal:mid/Mid_animal_game_panel"

AnimalGameView = BaseView:new()
local this = AnimalGameView
this.viewName = "AnimalGameView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Animal_GameView, true)

--设置加载列表
this.loadOrders=
{
    "animal:animal_game_panel"
}

local RoundTimer = "AnimalRoundTimer"
local TipTimer = "AnimalTipTimer"

this.grid = nil
this.roundState = AnimalConfig.RoundState.Other
this.curSelect = nil
this.isGameOver = false
this.score = 0

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_animal_game_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.map_image:AddEventListener(UIEvent.PointerClick, function ()
        NoticeManager.Instance:Dispatch(AnimalNoticeType.SelectCancel)
    end)
    self:hide()
    self:addEvent()
end

function this:addEvent()
    self.main_mid.back_btn:AddEventListener(UIEvent.PointerClick, this.onBtnExit)
    self.main_mid.capitulate_btn:AddEventListener(UIEvent.PointerClick, this.onBtnExit)
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写")
    self.main_mid.round_icon.gameObject:SetActive(false)
    self.main_mid.me_round_panel.gameObject:SetActive(false)
    self.main_mid.other_round_panel.gameObject:SetActive(false)
    self.main_mid.tip_image.gameObject:SetActive(false)
    self.main_mid.direction_group.gameObject:SetActive(false)
end

function this:initMap()
    self.grid = {}
    for x = 0, AnimalConfig.Row - 1 do
        self.grid[x] = {}
        for y = 0, AnimalConfig.Column - 1 do
            local pos = AnimalUtility.GetPosByGrid(x, y)
            --print(string.format("pos: (%s,%s)", pos.x, pos.y))
            local piece = AnimalPiece:new()
            local go = GameObject.Instantiate(AnimalPoolView:GetAnimalPrefab())
            piece:InitAnimalPiece(x, y, go)
            piece:SetPieceParent(self.main_mid.map_image.rectTransform)
            piece:SetPiecePosition(pos)
            self.grid[x][y] = piece
        end
    end
    self.isGameOver = false
    self.score = 0
    AudioManager.playBGM("animal", "bgm")
end

function this:GetRoundState()
    return self.roundState
end

function this:GetCurSelect()
    return self.curSelect
end

--=========================================
function this:OnLoadComplete()
    ViewManager.open(UIViewEnum.Animal_GameView, nil, function ()
        self:initMap()
    end)
end

function this:OnSelect(x, y)
    local piece = self.grid[x][y]
    if piece == nil then
        printError("斗兽棋-错误-选择的块不存在")
        return
    end
    print(string.format("选择了: (%s,%s)",x, y))
    self:activeDirection(x, y, true)
    self.curSelect = piece
    piece:OnSelectPiece()
end

function this:activeDirection(x, y, state)
    self.main_mid.direction_group.gameObject:SetActive(state)
    if state then
        self.main_mid.direction_group.transform:SetAsLastSibling()
        local pos = AnimalUtility.GetPosByGrid(x,y)
        self.main_mid.direction_group.transform.localPosition = Vector3(pos.x, pos.y, 0)
        local neighbor = AnimalUtility.GetNeighbor(x,y)
        for i = 1, #neighbor do
            local data = neighbor[i]
            --print("data: "..table.tostring(data))
            local item = self.main_mid.directionItemArr[i]
            if (data.x >= 0 and data.x < AnimalConfig.Row) and (data.y >= 0 and data.y < AnimalConfig.Column) then
                item.go:SetActive(true)
                local p = self.grid[data.x][data.y]
                if p:GetState() == AnimalConfig.PieceState.Dead then
                    item.direction_icon:ChangeIcon(1)
                elseif p:GetState() == AnimalConfig.PieceState.NotOpen then
                    item.go:SetActive(false)
                elseif p:GetState() == AnimalConfig.PieceState.Opened then
                    if p:GetSeat() == self.grid[x][y]:GetSeat() then
                        item.go:SetActive(false)
                    else
                        item.go:SetActive(true)
                        if (self.grid[x][y]:GetPieceId() == 0 and p:GetPieceId() == 7) or (self.grid[x][y]:GetPieceId() >= p:GetPieceId()) then
                            item.direction_icon:ChangeIcon(1)
                        else
                            item.direction_icon:ChangeIcon(0)
                        end
                    end
                else
                    item.direction_icon:ChangeIcon(0)
                end
            else
                item.go:SetActive(false)
            end
        end
    else
        self.main_mid.direction_group.transform.localPosition = Vector3(10000, 10000, 0)
    end
end

function this:OnSelectCancel()
    if self.curSelect == nil then
        return
    end
    --print(string.format("取消选择: (%s,%s)",self.curSelect.x, self.curSelect.y))
    self.curSelect:OnSelectCancelPiece()
    self.curSelect = nil
    self:activeDirection(0, 0, false)
end

function this:OnScoreChange(score)
    AnimalUtility.CountScroll(self.score, score, self.main_mid.score_text, 0.5)
    self.score = score
end

---@param msg NotifyRoundTurn
function this:OnRoundTurn(msg)
    local player = AnimalDataProxy:GetPlayerById(msg.player_id)
    if player == nil then
        return
    end
    local seat = player:GetPlayerSeat()
    local spriteIndex = 0
    if msg.player_id == AnimalDataProxy:GetMeUid() then
        if seat == 0 then
            spriteIndex = 0
        else
            spriteIndex = 2
        end
        self.roundState = AnimalConfig.RoundState.Me
    else
        if seat == 0 then
            spriteIndex = 1
        else
            spriteIndex = 3
        end
        self.roundState = AnimalConfig.RoundState.Other
    end
    if msg.total_round == 1 then
        self.main_mid.round_icon.gameObject:SetActive(true)
        self.main_mid.round_icon:ChangeIcon(spriteIndex)
        self.main_mid.round_icon.transform.localScale = Vector3.zero
        self.main_mid.round_icon.transform:DOScale(Vector3.one, 0.5):OnComplete(function()
            self.main_mid.round_icon.gameObject:SetActive(false)
        end)
    else
        self.main_mid.round_icon.gameObject:SetActive(false)
    end

    self:showRoundView(msg.player_id, msg.round_time)
    if msg.no_eat_round >=15 and self.roundState == AnimalConfig.RoundState.Me then
        self:ShowTip(string.format("20回合未发生吃子将和局, 还剩%s回合",  (20 - msg.no_eat_round)))
    end
end

-- 根据玩家数据显示
function this:showRoundView(player_id, round_time)
    local me = AnimalDataProxy:GeMeData()
    local it = AnimalDataProxy:GetOtherData()
    -- me_round
    self.main_mid.me_round_me_name_text.text = me:GetPlayerName()
    self.main_mid.me_round_other_name_text.text = it:GetPlayerName()
    self.main_mid.me_round_me_seat_icon:ChangeIcon(me:GetPlayerSeat())
    self.main_mid.me_round_other_seat_icon:ChangeIcon(it:GetPlayerSeat())
    downloadUserHead(me:GetPlayerIcon() ,self.main_mid.me_round_me_head_image)
    downloadUserHead(it:GetPlayerIcon() ,self.main_mid.me_round_other_head_image)
    self.main_mid.me_round_icon:ChangeIcon(me:GetPlayerSeat())
    -- survive
    for i = 0, 7 do
        local data = me:GetSurviveById(i)
        local item = self.main_mid.meSurviveItemArr[8 - i]
        local lastItem = self.main_mid.meSurviveItemArr[#self.main_mid.meSurviveItemArr]
        if data == nil then
            item.animal_icon:ChangeIcon(1)
            if i == 7 then
                lastItem.animal_icon:ChangeIcon(1)
            end
        else
            item.animal_icon:ChangeIcon(0)
            if i == 7 then
                lastItem.animal_icon:ChangeIcon(0)
            end
        end
    end
    -- it_round
    self.main_mid.other_round_me_name_text.text = me:GetPlayerName()
    self.main_mid.other_round_other_name_text.text = it:GetPlayerName()
    self.main_mid.other_round_me_seat_icon:ChangeIcon(me:GetPlayerSeat())
    self.main_mid.other_round_other_seat_icon:ChangeIcon(it:GetPlayerSeat())
    downloadUserHead(me:GetPlayerIcon() ,self.main_mid.other_round_me_head_image)
    downloadUserHead(it:GetPlayerIcon() ,self.main_mid.other_round_other_head_image)
    self.main_mid.other_round_icon:ChangeIcon(it:GetPlayerSeat())
    -- survive
    for i = 0, 7 do
        local data = it:GetSurviveById(i)
        local item = self.main_mid.otherSurviveItemArr[8 - i]
        local lastItem = self.main_mid.otherSurviveItemArr[#self.main_mid.otherSurviveItemArr]
        if data == nil then
            item.animal_icon:ChangeIcon(1)
            if i == 7 then
                lastItem.animal_icon:ChangeIcon(1)
            end
        else
            item.animal_icon:ChangeIcon(0)
            if i == 7 then
                lastItem.animal_icon:ChangeIcon(0)
            end
        end
    end

    -- RoundTimer
    local roundTimerCount = round_time
    local function roundTimerFun()
        if roundTimerCount < 0 then
            return
        end
        roundTimerCount = roundTimerCount - 1
        if roundTimerCount >= 0 then
            if player_id == me:GetPlayerId() then
                self.main_mid.me_round_timer_text.text = tostring(roundTimerCount)
            else
                self.main_mid.other_round_timer_text.text = tostring(roundTimerCount)
            end
        else
            print("斗兽棋-单回合的30秒时间到")
            self.isGameOver = true
        end
    end
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(RoundTimer)
    local animTime = 0.4
    if player_id == me:GetPlayerId() then
        self.main_mid.me_round_panel.gameObject:SetActive(true)
        self.main_mid.other_round_panel.gameObject:SetActive(false)
        self.main_mid.me_round_timer_text.text = tostring(roundTimerCount)
        GlobalTimeManager.Instance.timerController:AddTimer(RoundTimer, 1000, -1, roundTimerFun)
        self.main_mid.me_round_me_seat_icon.transform:DOLocalMoveX(-1000, animTime):From();
        self.main_mid.me_round_other_seat_icon.transform:DOLocalMoveX(800, animTime):From();
    else
        self.main_mid.me_round_panel.gameObject:SetActive(false)
        self.main_mid.other_round_panel.gameObject:SetActive(true)
        self.main_mid.other_round_timer_text.text = tostring(roundTimerCount)
        GlobalTimeManager.Instance.timerController:AddTimer(RoundTimer, 1000, -1, roundTimerFun)
        self.main_mid.other_round_me_seat_icon.transform:DOLocalMoveX(-800, animTime):From();
        self.main_mid.other_round_other_seat_icon.transform:DOLocalMoveX(1000, animTime):From();
    end
end

---@param msg RspReverseChess
function this:OnReverse(msg)
    AudioManager.playSound("animal", "reverse")
    local x,y = msg.position.x, msg.position.y
    local piece = self.grid[x][y]
    piece:OnReversePiece(msg.chess_id, function()
        AnimalNetModule.sendNotifyOperationFinish(msg.player_id)
    end)
end

-- 移动仲裁结果
---@param msg NotifyMoveChess
function this:OnMoveResult(msg)
    if msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Fail then
        printError("斗兽棋-移动错误")
        return
    end
    self:OnSelectCancel()
    self.roundState = AnimalConfig.RoundState.Wait
    local moveCro = nil
    moveCro = coroutine.start(function ()
        local origin = { x = msg.old_position.x, y = msg.old_position.y}
        local target = { x = msg.new_position.x, y = msg.new_position.y}
        local p1 = self.grid[msg.old_position.x][msg.old_position.y]
        local p2 = self.grid[msg.new_position.x][msg.new_position.y]
        local pos1 = AnimalUtility.GetPosByGrid(origin.x, origin.y)
        local pos2 = AnimalUtility.GetPosByGrid(target.x, target.y)
        local vfxPos = p2:GetPieceWorldPosition()
        local time = AnimalConfig.MOVE_TIME
        if msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Success or
                msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Eat
        then
            self.grid[origin.x][origin.y] = p2
            self.grid[target.x][target.y] = p1
            p1:UpdatePieceIndex(target.x, target.y)
            p2:UpdatePieceIndex(origin.x, origin.y)
        end
        p1:SetAsLastSibling()
        for t = 0, time, Time.deltaTime do
            p1:SetPiecePosition(Vector2.Lerp(pos1, pos2, t / time))
            coroutine.step(moveCro)
        end
        if msg.result ~= ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Success then
            p1:ShowFightPiece()
            p2:ShowFightPiece()
            AudioManager.playSound("animal", "fight")
            local obj = AnimalObjectPool:GetInstance():GetObject("d_fx_luandou_001")
            obj.transform.localPosition = vfxPos
            coroutine.wait(moveCro, 1000)
            AnimalObjectPool:GetInstance():PoolObject(obj)
            AnimalNetModule.sendNotifyOperationFinish(msg.player_id)
        else
            AnimalNetModule.sendNotifyOperationFinish(msg.player_id)
        end

        if msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Success then
            p1:SetPiecePosition(pos2)
            p2:SetPiecePosition(pos1)
        elseif msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_Eat then
            p2:ShowDeadPiece()
            p1:SetPiecePosition(pos2)
            p2:SetPiecePosition(pos1)
        elseif msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_BeEaten then
            p1:ShowDeadPiece()
            p1:SetPiecePosition(pos1)
            p2:SetPiecePosition(pos2)
        elseif msg.result == ProtoEnumAnimalChess.MoveChess_Result.MoveChess_PerishTogether then
            p1:ShowDeadPiece()
            p2:ShowDeadPiece()
            p1:SetPiecePosition(pos1)
            p2:SetPiecePosition(pos2)
        end
        --print("移动完毕")

    end)
end

function this:ShowTip(str)
    self.main_mid.tip_image.gameObject:SetActive(true)
    self.main_mid.tip_text.text = str
    self.main_mid.tip_image.rectTransform.sizeDelta = Vector2(self.main_mid.tip_text.rectTransform.sizeDelta.x + 80, 100)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(TipTimer)
    GlobalTimeManager.Instance.timerController:AddTimer(TipTimer, 1500, 1, function ()
        self.main_mid.tip_image.gameObject:SetActive(false)
    end)
end

function this:GetIsGameOver()
    return self.isGameOver
end

function this:OnOver()
    --print("游戏结束, 移除计时器: RoundTimer")
    AudioManager.stopBGM()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(RoundTimer)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(TipTimer)
    self.isGameOver = true
end

function this:OnExitGameView()
    self:OnOver()
    if self:getIsLoaded() == false then
        return
    end
    if self.grid ~= nil then
        for x = 0, AnimalConfig.Row - 1 do
            for y = 0, AnimalConfig.Column - 1 do
                local piece =  self.grid[x][y]
                piece:DestroyPiece()
            end
        end
    end
    self.roundState = AnimalConfig.RoundState.Other
    self.curSelect = nil
    self.isGameOver = false
end

function this.onBtnExit(eventData)
    AnimalPopupView:showPopupView("是否认输?",  function ()
        this:OnOver()
        AnimalNetModule.ReqSurrender()
    end, nil)
end