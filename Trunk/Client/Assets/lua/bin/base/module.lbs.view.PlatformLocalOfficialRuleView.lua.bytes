require "base:enum/UIViewEnum"

--打开红包界面
PlatformLocalOfficialRuleView = BaseView:new()
local this = PlatformLocalOfficialRuleView
this.viewName = "PlatformLocalOfficialRuleView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LOCAL_OFFICIAL_RULE, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_official_reward_panel"
}

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.mask:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.enterGameBtn:AddEventListener(UIEvent.PointerClick, self.enterBtnHandler)
end

local function GetTimeStr(time)
    local m = math.floor(time / 60)
    local s = time - m * 60
    local function full(num)
        if num < 10 then
            return string.concat("0", num)
        end
        return tostring(num)
    end
    return string.concat(full(m), ":", full(s))
end
local function CountDowm(self, timer)
    local curtimer = timer - TimeManager.getServerUnixTime()
	if curtimer <= 0 then
		self.main_mid.timeCount_txt.text = "活动已结束"
		PlatformLBSModule.delaySendReqGetOfficalActivity()
	else
		self.main_mid.timeCount_txt.text = string.concat("结束倒计时: ", GetTimeStr(curtimer))
	end
    
    if curtimer + 10 <= 0 then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalOfficialRuleTimeCountDown")
    end
end
local function CalTimerCount(self, timer)
    CountDowm(self, timer)
    GlobalTimeManager.Instance.timerController:AddTimer(
        "GlobalOfficialRuleTimeCountDown",
        1000,
        -1,
        function()
            CountDowm(self, timer)
        end
    )
end

--override 打开UI回调
function this:onShowHandler(msg)
    self.activice_id = msg.id
    self.official_config = TableBaseOfficalMatch.data[msg.match_type]
    self.game_config = TableBaseGameList.data[self.official_config.gameid]

    self.canjoin = msg.game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_CanJion

    self.main_mid.titleGamename.text = self.game_config.name
    self.main_mid.gamedscri.text = self.official_config.title

    self.main_mid.enter_game_text.text = self.canjoin and "马上参赛" or "已参赛(查看排名)"
    downloadGameIcon(self.official_config.gameid, self.main_mid.gameIcon)
    CalTimerCount(self, msg.endTime)
    local data = {}
    data[1] = {}
    data[2] = {}
    data[1].title = "游戏规则"
    data[1].desc = self.official_config.desc
    data[2].title = "比赛奖励"
    data[2].desc = self.official_config.rewardDesc

    this.main_mid.rule_CellRecycleScrollPanel:SetCellData(data, self.updateRulePanel, true)
end

function this.updateRulePanel(go, data, index, dataIndex)
    local item = this.main_mid.rule_ListArr[index + 1]
    item.rewardTitle.text = data.title
    item.rewardTxt.text = data.desc
    local height = item.rewardTitle.Txt.preferredHeight * 2 + item.rewardTxt.Txt.preferredHeight + 60
    this.main_mid.rule_CellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, height)
end

function this.enterBtnHandler()
    if this.canjoin then
        GameManager.enterGame(this.official_config.gameid, EnumGameType.OfficialMatch, -1, this.activice_id)
    else
        PlatformLBSModule.sendReqGetOfficalActivityRank(this.activice_id)
        this.close()
    end
end

function this.close()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalOfficialRuleTimeCountDown")
    ViewManager.close(UIViewEnum.Platform_LOCAL_OFFICIAL_RULE)
end
