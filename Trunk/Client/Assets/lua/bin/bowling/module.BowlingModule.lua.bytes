require "bowling:module/view/BowlingScene"

BowlingModule = BaseModule:new()
local this = BowlingModule

this.moduleName = "BowlingModule"

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        -- 平台
        table.insert(self.notificationList, CommonNoticeType.Game_Enter)
        table.insert(self.notificationList, CommonNoticeType.Game_Exit)
        -- 业务
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    if GameManager.curGameId ~= EnumGameID.Bowling then
        return
    end
    local switch = {
        -- 平台
        [CommonNoticeType.Game_Enter] = function()
            self:onBowling_EnterScene(notice)
        end,
        [CommonNoticeType.Game_Exit] = function()
            self:onBowling_Game_Exit(notice)
        end,
        -- 业务
    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        self:withoutRegistNotice(noticeType)--用于报错提醒
    end
end

--============================================ 事件 ==========================================

function this:onBowling_EnterScene(notice)
    local obj = notice:GetObj()
    if obj.game_id ~= EnumGameID.Bowling then
        return
    end
    
	--BowlingScene.initBowlingScene()
	if BowlingGameManager.isShowLoading then
		ViewManager.open(UIViewEnum.BowlingLoadingView)
	else
		BowlingScene.initBowlingScene()
	end
end

function this:onBowling_BackToPlatform(notice)
		BowlingScene.dispose()
    GameManager.exitGame(EnumGameID.Bowling)
end

function this:onBowling_Game_Exit(notice)
    CSLoger.debug(Color.Yellow, "========退出保龄球游戏事件响应=========")
    self:onBowling_BackToPlatform(notice)
end
