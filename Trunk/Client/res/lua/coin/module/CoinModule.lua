require "base:enum/NoticeType"
require "coin:enum/CoinNoticeType"
require "coin:module/view/CoinLoadView"
require "coin:CoinPreload"

CoinModule = BaseModule:new()
local this = CoinModule
this.moduleName = "CoinModule"

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        -- 平台
        table.insert(self.notificationList, CommonNoticeType.Game_Enter)
        table.insert(self.notificationList, CommonNoticeType.Game_Exit)
        -- 业务
        for _, v in pairs(CoinNoticeType) do
            table.insert(self.notificationList, v)
        end
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    if GameManager.curGameId ~= EnumGameID.Coin then
        return
    end
    local switch = {
        -- 平台
        [CommonNoticeType.Game_Enter] = function()
            self:onCoin_EnterScene(notice)
        end,
        [CommonNoticeType.Game_Exit] = function()
            self:onCoin_Game_Exit(notice)
        end,

        -- 业务
        [CoinNoticeType.LoadStart] = function()
            self:onCoin_LoadStart(notice)
        end,
        [CoinNoticeType.LoadStepEnd] = function()
            self:onCoin_LoadStepEnd(notice)
        end,
        [CoinNoticeType.LoadComplete] = function()
            self:onCoin_LoadComplete(notice)
        end,

        [CoinNoticeType.PointerDown] = function()
            self:onCoin_PointerDown(notice)
        end,
        [CoinNoticeType.PointerDrag] = function()
            self:onCoin_PointerDrag(notice)
        end,
        [CoinNoticeType.PointerUp] = function()
            self:onCoin_PointerUp(notice)
        end,
        [CoinNoticeType.BackToPlatform] = function()
            self:onCoin_BackToPlatform(notice)
        end,
    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        self:withoutRegistNotice(noticeType)--用于报错提醒
    end
end

--============================================ 事件 ==========================================

function this:onCoin_EnterScene(notice)
    local obj = notice:GetObj()
    print("抢金币-收到进入场景通知, obj = "..table.tostring(obj))
    if obj.game_id ~= EnumGameID.Coin then
        return
    end

    CoinNetModule.InitLoginInfo(obj)
    CoinNetModule.sendReqJoinMatch(obj.game_type, obj.active_id, obj.player_id)
end

function this:onCoin_LoadStart(notice)
    CoinLoadView:OpenCoinLoadView(function ()
        CS.PreloadManager.Instance:ExecuteOrder(CoinPreload)
    end)
end

function this:onCoin_LoadStepEnd(notice)
    CoinLoadView:OnLoadingLoadView()
end

this.loadViewCro = nil  -- 加载进度条是根据时间平滑移动的, 应该等进度条移动完了再执行加载完成
function this:onCoin_LoadComplete(notice)
    if self.loadViewCro ~= nil then
        coroutine.stop(self.loadViewCro)
    end
    self.loadViewCro = coroutine.start(function ()
        while(not CoinLoadView:GetIsProSuccess()) do
            coroutine.step(self.loadViewCro)
        end
        ViewManager.close(UIViewEnum.Coin_LoadView)
        ViewManager.open(UIViewEnum.Coin_GameView)
    end)

end

function this:onCoin_PointerDown(notice)
    local obj = notice:GetObj()
    CoinGameView:OnPieceDown(obj)
end

function this:onCoin_PointerDrag(notice)
    local obj = notice:GetObj()
    CoinGameView:OnPieceDrag(obj)
end

function this:onCoin_PointerUp(notice)
    CoinGameView:OnPieceUp()
end

function this:onCoin_BackToPlatform(notice)
    CoinLoadView:OnExitLoadView()
    CoinGameView:OnExitGameView()
    CoinObjectPool:GetInstance():OnDestroyObjectPool()
    CoinNetModule:OnExitGame()
    GameManager.exitGame(EnumGameID.Coin)
end

function this:onCoin_Game_Exit(notice)
    CSLoger.debug(Color.Yellow, "========抢金币-离线事件响应=========")
    self:onCoin_BackToPlatform(notice)
end

