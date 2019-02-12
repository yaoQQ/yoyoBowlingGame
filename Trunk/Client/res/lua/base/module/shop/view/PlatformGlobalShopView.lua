require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_shop_panel"
require "base:enum/PlatformGlobalEnum/PlatformGlobalNoticeType"
require "base:module/shop/data/ActivityItem"

--主界面：商铺
PlatformGlobalShopView = BaseView:new()
local this = PlatformGlobalShopView
this.viewName = "PlatformGlobalShopView"

--设置面板特性
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_Shop_View, true)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_shop_panel"
}
local GameFilterList = {} -- 赛事游戏可过滤列表

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    --UIMono方式绑定UI对象
    self.main_mid = Mid_platform_shop_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:InitCommpent()
    self:addEvent()
    self:showActivityDetail()
	
	--暂时写死部分开放的游戏
    for _, v in pairs(TableBaseGameList.data) do
		if IS_TEST_SERVER then
			table.insert(GameFilterList, v.id)
		else
			if v.isStableOpen == 1 then
				table.insert(GameFilterList, v.id)
			end
		end
    end
    table.sort(GameFilterList)
end

local coOpenSubView = nil

this.arrowCount = 0

this.search_key = false

local lerpCro = nil
--插值移动
local function startLerpMove(self, startPos, targetPos, callback, time)
    if lerpCro then
        coroutine.stop(lerpCro)
        lerpCro = nil
    end
    lerpCro =
        coroutine.start(
        function()
            for t = 0, time, Time.deltaTime do
                local nextPos = Vector2.Lerp(startPos, targetPos, t / time)
                self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, nextPos.y)
                coroutine.step(lerpCro)
            end
            if callback then
                callback()
            end
        end
    )
end

--顶部搜索城市面板展开
local function onTopPanelOpen(self)
    if self.curTopPanelState == this.ShowPanelState.StateEnum.TopPanelOpen then
        return
    end
    local euler = self.main_mid.city_arrow.transform.localEulerAngles
    self.main_mid.city_arrow.transform.localEulerAngles = Vector3(euler.x, euler.y, 90)
    self.main_mid.cityPanel.gameObject:SetActive(true)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(false)
    self.main_mid.enterActivity.gameObject:SetActive(false)
    self.curTopPanelState = this.ShowPanelState.StateEnum.TopPanelOpen
end

--顶部搜索城市面板关闭
local function onTopPanelClose(self)
    if self.curTopPanelState == this.ShowPanelState.StateEnum.TopPanelClose then
        return
    end
    local euler = self.main_mid.city_arrow.transform.localEulerAngles
    self.main_mid.city_arrow.transform.localEulerAngles = Vector3(euler.x, euler.y, -90)
    self.main_mid.cityPanel.gameObject:SetActive(false)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(false)
    self.main_mid.enterActivity.gameObject:SetActive(true)
    self.curTopPanelState = this.ShowPanelState.StateEnum.TopPanelClose
end

--底部栏关闭状态只有按钮存在
local function onBottomPanelClose(self)
    if self.curBottomPanelState == this.ShowPanelState.StateEnum.BottomPanelClose then
        return
    end
    self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, -1600)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(false)
    self.main_mid.enterActivity.gameObject:SetActive(true)
    self.curBottomPanelState = this.ShowPanelState.StateEnum.BottomPanelClose

    --todo reverse
    if this.temp_activityInfo and this.temp_showType then
        this:showActivityDetail(this.temp_activityInfo, this.temp_showType)
    end
end

--底部栏打开简略状态
local function onBottomPanelShortOpen(self)
    if self.curBottomPanelState == this.ShowPanelState.StateEnum.BottomPanelShortOpen then
        return
    end
    self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, -730)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(true)
    self.main_mid.enterActivity.gameObject:SetActive(false)
    self.curBottomPanelState = this.ShowPanelState.StateEnum.BottomPanelShortOpen
end

--底部栏打开详细状态
local function onBottomPanelDetailOpen(self)
    if self.curBottomPanelState == this.ShowPanelState.StateEnum.BottomPanelDetailOpen then
        return
    end
    self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, -400)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(true)
    self.main_mid.enterActivity.gameObject:SetActive(false)
    self.curBottomPanelState = this.ShowPanelState.StateEnum.BottomPanelDetailOpen
end

--右边活动分类面板打开状态
local function onRightPanelDetailOpen(self)
    if self.curRightPanelState == this.ShowPanelState.StateEnum.RightPanelDetailOpen or self.isPlay then
        return
    end
    self.isPlay = true
    self.main_mid.activityChildBtnList.gameObject:SetActive(true)
    self.main_mid.activityBgImage.gameObject:SetActive(false)
    self.main_mid.activityChildBtnList:PlayBack(
        "open_search_view",
        function()
            self.isPlay = false
        end
    )
    self.main_mid.arrowIcon:ChangeIcon(1)
    self.curRightPanelState = this.ShowPanelState.StateEnum.RightPanelDetailOpen
end

--右边活动分类面板关闭状态
local function onRightPanelDetailClose(self)
    if self.curRightPanelState == this.ShowPanelState.StateEnum.RightPanelDetailClose or self.isPlay then
        return
    end
    self.isPlay = true
    self.main_mid.activityChildBtnList:Play(
        "open_search_view",
        function()
            self.isPlay = false
            self.main_mid.activityChildBtnList.gameObject:SetActive(false)
            self.main_mid.activityBgImage.gameObject:SetActive(true)
        end
    )
    self.main_mid.arrowIcon:ChangeIcon(0)
    self.curRightPanelState = this.ShowPanelState.StateEnum.RightPanelDetailClose
end

--关注赛事面板打开
local function onAttentionPanelOPen(self)
    if self.curAttentionState == this.ShowPanelState.StateEnum.AttentionPanelOPen then
        return
    end
    self.main_mid.timesbg:ChangeIcon(1)
    self.main_mid.attention_game_panel.gameObject:SetActive(true)
    self.main_mid.enterActivity.gameObject:SetActive(false)
    self.curAttentionState = this.ShowPanelState.StateEnum.AttentionPanelOPen

    --关注赛事
    local activityList = PlatformLBSDataProxy.getActiveStartList()
    local activityInfo = {}
    local activityCount = 0
    if not table.empty(activityList) then
        for k, v in pairs(activityList) do
            local activeItem = ActivityItem.createByCompetition(v)
            table.insert(activityInfo, activeItem)
            activityCount = activityCount + 1
        end
    end
    this.attentionGameTimes = activityCount
    this.attentionGameActivityInfo = activityInfo

    --最近参加赛事
    local recentActivityList = PlatformLBSDataProxy.getRecentGameList()
    local recentActivityInfo = {}
    local recentActivityCount = 0
    if not table.empty(recentActivityList) then
        for k, v in pairs(recentActivityList) do
            local recentActiveItem = ActivityItem.createByCompetition(v)
            table.insert(recentActivityInfo, recentActiveItem)
            recentActivityCount = recentActivityCount + 1
        end
    end
    this.lastGameTimes = recentActivityCount
    this.lastGameActivityInfo = recentActivityInfo
    local game_panel_state =
        activityCount >= recentActivityCount and this.gamePanelState.StateEnum.AttentionGame or
        this.gamePanelState.StateEnum.LastGame
    this.currGamePanelState = game_panel_state
    this.gamePanelState[this.currGamePanelState](self)
end

--关注赛事面板关闭
local function onAttentionPanelClose(self)
    if self.curAttentionState == this.ShowPanelState.StateEnum.AttentionPanelClose then
        return
    end
    self.main_mid.timesbg:ChangeIcon(0)
    self.main_mid.enterActivity.gameObject:SetActive(true)
    self.main_mid.attention_game_panel.gameObject:SetActive(false)
    self.curAttentionState = this.ShowPanelState.StateEnum.AttentionPanelClose
end

this.ShowPanelState = {}
--不同状态表现
this.ShowPanelState.StateEnum = {
    None = 0, --无状态
    TopPanelOpen = 1, --顶部搜索城市面板展开
    TopPanelClose = 2, --顶部搜索城市面板关闭
    BottomPanelClose = 3, --底部栏关闭状态只有按钮存在
    BottomPanelShortOpen = 4, --底部栏打开简略状态
    BottomPanelDetailOpen = 5, --底部栏打开详细状态
    RightPanelDetailOpen = 6, --右边活动分类面板打开状态
    RightPanelDetailClose = 7, --右边活动分类面板关闭状态
    AttentionPanelOPen = 8, --关注赛事面板打开
    AttentionPanelClose = 9 --关注赛事面板关闭
}

this.ShowPanelState[this.ShowPanelState.StateEnum.TopPanelOpen] = onTopPanelOpen
this.ShowPanelState[this.ShowPanelState.StateEnum.TopPanelClose] = onTopPanelClose
this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelClose] = onBottomPanelClose
this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelShortOpen] = onBottomPanelShortOpen
this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelDetailOpen] = onBottomPanelDetailOpen
this.ShowPanelState[this.ShowPanelState.StateEnum.RightPanelDetailOpen] = onRightPanelDetailOpen
this.ShowPanelState[this.ShowPanelState.StateEnum.RightPanelDetailClose] = onRightPanelDetailClose
this.ShowPanelState[this.ShowPanelState.StateEnum.AttentionPanelOPen] = onAttentionPanelOPen
this.ShowPanelState[this.ShowPanelState.StateEnum.AttentionPanelClose] = onAttentionPanelClose

--全部活动类型请求
local function onAllActivity(self)
    PlatformLBSDataProxy.clearAllActivityData()
    
    PlatformLBSModule.sendReqNearAllActivity(MapManager.curLng, MapManager.curLat, MapManager.getCurScreenMapPos(500))
	--请求推荐商家赛事
	PlatformLBSModule.sendReqRecommonedBusActive()
	--请求官方赛事
    PlatformLBSModule.sendReqGetOfficalActivity()
	
    ShowSearching(true, "activityClassify")
end

--红包活动
local function onRedPacketActivity(self)
    PlatformLBSDataProxy.clearAllActivityData()
    PlatformLBSModule.sendReqNearRedBag(MapManager.curLng, MapManager.curLat)
    ShowSearching(true, "activityClassify")
end

--劵包活动类型
local function onRedPacketCuponActivity(self)
    PlatformLBSDataProxy.clearAllActivityData()
    PlatformLBSModule.sendReqNearCoupon(MapManager.curLng, MapManager.curLat)
    ShowSearching(true, "activityClassify")
end

--游戏活动类型
local function onShopGameActivityActivity(self)
    PlatformLBSDataProxy.clearAllActivityData()
	
    PlatformLBSModule.sendReqNearActivity(MapManager.curLng, MapManager.curLat, MapManager.getCurScreenMapPos(500))
	--请求推荐商家赛事
	PlatformLBSModule.sendReqRecommonedBusActive()
	--请求官方赛事
    PlatformLBSModule.sendReqGetOfficalActivity()
	
    ShowSearching(true, "activityClassify")
end

this.RightClickState = {}

--筛选类型
this.RightClickState.StateEnum = {
    AllActivity = 0, --全部活动类型
    RedPacketActivity = 1, --红包活动
    RedPacketCuponActivity = 2, --劵包活动类型
    ShopGameActivityActivity = 3, --游戏活动类型
}

this.RightClickState[this.RightClickState.StateEnum.AllActivity] = onAllActivity
this.RightClickState[this.RightClickState.StateEnum.RedPacketActivity] = onRedPacketActivity
this.RightClickState[this.RightClickState.StateEnum.RedPacketCuponActivity] = onRedPacketCuponActivity
this.RightClickState[this.RightClickState.StateEnum.ShopGameActivityActivity] = onShopGameActivityActivity

--滑动开始
local function DragBottomPanelBegin(self, initRectMaxY)
    if not self.main_mid.mid_scratch_Panel.gameObject.activeSelf then
        self.main_mid.mid_scratch_Panel.gameObject:SetActive(true)
    -- self.main_mid.enterActivity.gameObject:SetActive(false)
    end
    self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, initRectMaxY)
    self.curBottomPanelState = this.ShowPanelState.StateEnum.None
end
--滑动中
local function DragBottomPanel(self, eventData)
    self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, self.ParentRect.offsetMax.y + eventData.delta.y)
end

local function DragBottomPanelEnd(self)
    if self.ParentRect.offsetMax.y <= -1570 then
        startLerpMove(
            self,
            self.ParentRect.offsetMax,
            Vector2(self.ParentRect.offsetMax.x, -1600),
            function(...)
                this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelClose)
            end,
            0.05
        )
        return
    end
    if self.ParentRect.offsetMax.y <= -880 and self.ParentRect.offsetMax.y > -1570 then
        startLerpMove(
            self,
            self.ParentRect.offsetMax,
            Vector2(self.ParentRect.offsetMax.x, -730),
            function(...)
                this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelShortOpen)
            end,
            0.05
        )
        return
    end
    if self.ParentRect.offsetMax.y > -880 then
        startLerpMove(
            self,
            self.ParentRect.offsetMax,
            Vector2(self.ParentRect.offsetMax.x, -400),
            function(...)
                this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelDetailOpen)
            end,
            0.05
        )
        return
    end
end

--override 打开UI回调
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()

    --self.main_mid.mid_unscratch_Panel.gameObject:SetActive(true)

    self:addNotice()
    self:updateCityPanel()
    if LoginDataProxy.isGetUserInfo then
        this.onLoginSuccess()
    end

    --打开子界面
    coOpenSubView =
        coroutine.start(
        function()
            ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Global_Shop_View)
            coroutine.step(coOpenSubView)
            ViewManager.open(UIViewEnum.MapView)
            coroutine.step(coOpenSubView)
            ViewManager.open(UIViewEnum.PlatForm_Float_View)
            coroutine.step(coOpenSubView)
        end
    )
end

function this:addEvent()
    self.main_mid.search_Inputfield:AddEventListener(UIEvent.PointerClick, self.onClickButton)
    self.main_mid.city_button:AddEventListener(
        UIEvent.PointerClick,
        function()
            local b = self.curTopPanelState == this.ShowPanelState.StateEnum.TopPanelOpen
            local curState =
                b and this.ShowPanelState.StateEnum.TopPanelClose or this.ShowPanelState.StateEnum.TopPanelOpen
            this:PanelStateExclusion(curState)
        end
    )
    self.main_mid.enterActivitybg:AddEventListener(
        UIEvent.PointerClick,
        function(data)
            if data.dragging and self.curBottomPanelState == this.ShowPanelState.StateEnum.None then
                return
            end
            self.ParentRect.offsetMax = Vector2(self.ParentRect.offsetMax.x, -1600)
            self.main_mid.mid_scratch_Panel.gameObject:SetActive(true)
            self.main_mid.enterActivity.gameObject:SetActive(false)
            startLerpMove(
                self,
                self.ParentRect.offsetMax,
                Vector2(self.ParentRect.offsetMax.x, -730),
                function(...)
                    this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelShortOpen)
                end,
                0.2
            )
        end
    )
    self.main_mid.enterActivitybg:AddEventListener(
        UIEvent.DragBegin,
        function(eventData)
            DragBottomPanelBegin(self, -1600)
        end
    )
    self.main_mid.enterActivitybg:AddEventListener(
        UIEvent.Drag,
        function(eventData)
            DragBottomPanel(self, eventData)
        end
    )
    self.main_mid.enterActivitybg:AddEventListener(
        UIEvent.DragEnd,
        function(eventData)
            DragBottomPanelEnd(self)
        end
    )
    self.main_mid.shop_count_do_button:AddEventListener(
        UIEvent.DragBegin,
        function()
            local b = self.curBottomPanelState == this.ShowPanelState.StateEnum.BottomPanelDetailOpen
            local initMaxY = b and -400 or -730
            DragBottomPanelBegin(self, initMaxY)
        end
    )
    self.main_mid.shop_count_do_button:AddEventListener(
        UIEvent.Drag,
        function(eventData)
            DragBottomPanel(self, eventData)
        end
    )
    self.main_mid.shop_count_do_button:AddEventListener(
        UIEvent.DragEnd,
        function()
            DragBottomPanelEnd(self)
        end
    )
    self.main_mid.shop_count_do_button:AddEventListener(
        UIEvent.PointerClick,
        function()
            if self.curBottomPanelState == this.ShowPanelState.StateEnum.None then
                return
            end
            --local b = self.curBottomPanelState == this.ShowPanelState.StateEnum.BottomPanelDetailOpen
            local curState = this.ShowPanelState.StateEnum.BottomPanelClose
            -- b and this.ShowPanelState.StateEnum.BottomPanelClose or
            -- this.ShowPanelState.StateEnum.BottomPanelDetailOpen
            local MoveY = -1600 --b and -1600 or -400
            startLerpMove(
                self,
                self.ParentRect.offsetMax,
                Vector2(self.ParentRect.offsetMax.x, MoveY),
                function(...)
                    this:PanelStateExclusion(curState)
                end,
                0.2
            )
        end
    )
    self.main_mid.BtnBackMyPos:AddEventListener(
        UIEvent.PointerClick,
        function()
            NoticeManager.Instance:Dispatch(NoticeType.Map_Back_My_Pos)
        end
    )
    self.main_mid.ActivityBtnList:AddEventListener(
        UIEvent.PointerClick,
        function()
            local b = self.curRightPanelState == this.ShowPanelState.StateEnum.RightPanelDetailOpen
            local curState =
                b and this.ShowPanelState.StateEnum.RightPanelDetailClose or
                this.ShowPanelState.StateEnum.RightPanelDetailOpen
            this:PanelStateExclusion(curState)
        end
    )
    self.main_mid.arrowIconPress:AddEventListener(
        UIEvent.PointerClick,
        function()
            local b = self.curRightPanelState == this.ShowPanelState.StateEnum.RightPanelDetailOpen
            local curState =
                b and this.ShowPanelState.StateEnum.RightPanelDetailClose or
                this.ShowPanelState.StateEnum.RightPanelDetailOpen
            this:PanelStateExclusion(curState)
        end
    )
    for _, value in pairs(this.RightClickState.StateEnum) do
        self.main_mid[string.concat("ActivityBtnList", value)]:AddEventListener(
            UIEvent.PointerClick,
            function()
                this:activityClassifyByChooseType(value)
            end
        )
    end
    self.main_mid.search_button:AddEventListener(
        UIEvent.PointerClick,
        function()
            --this.RightClickState[self.curRightClickType](self)
            this:activityClassifyByChooseType(this.curRightClickType)
            self.main_mid.search_button.gameObject:SetActive(false)
            this.curLngPosDirtyCaching = MapManager.curLng
            this.curLatPosDirtyCaching = MapManager.curLat
        end
    )
    self.main_mid.detail_search_button:AddEventListener(
        UIEvent.PointerClick,
        function()
            --this.RightClickState[self.curRightClickType](self)
            this:activityClassifyByChooseType(this.curRightClickType)
            self.main_mid.detail_search_button.gameObject:SetActive(false)
            this.main_mid.curClassTxttitle.gameObject:SetActive(true)
            this.curLngPosDirtyCaching = MapManager.curLng
            this.curLatPosDirtyCaching = MapManager.curLat
        end
    )
    self.main_mid.pressPanel:AddEventListener(
        UIEvent.PointerClick,
        function()
            this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelClose)
        end
    )

    this.main_mid.isStartGame_press:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            if eventData == nil then
                return
            end
            local b = self.curAttentionState == this.ShowPanelState.StateEnum.AttentionPanelOPen
            local curState =
                b and this.ShowPanelState.StateEnum.AttentionPanelClose or
                this.ShowPanelState.StateEnum.AttentionPanelOPen
            this:PanelStateExclusion(curState)
        end
    )
    this.main_mid.close_attention:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            if eventData == nil then
                return
            end
            this:PanelStateExclusion(this.ShowPanelState.StateEnum.AttentionPanelClose)
        end
    )
    this.main_mid.attentionPress:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this.currGamePanelState = this.gamePanelState.StateEnum.AttentionGame
            this.gamePanelState[this.gamePanelState.StateEnum.AttentionGame](self)
        end
    )
    this.main_mid.lastGamePress:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this.currGamePanelState = this.gamePanelState.StateEnum.LastGame
            this.gamePanelState[this.gamePanelState.StateEnum.LastGame](self)
        end
    )
    self.main_mid.game_recycle_scroll:SetCellData(
        GameFilterList,
        self.onSetGameTypeInfo,
        true
    )
end

--初始化组件
function this:InitCommpent()
    self.main_mid.cityItem.gameObject:SetActive(false)
    self.main_mid.activityMembers.gameObject:SetActive(false)
    self.main_mid.detailactivityMembers.gameObject:SetActive(false)
    self.ParentRect = self.main_mid.Parent.rectTransform
    self.curRightClickType = this.RightClickState.StateEnum.AllActivity
    this.curGameTypeClickState = this.GameTypeState.None
    self.isPlay = false
    this.resetChooseIcon()
    this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelClose)
end

--override 关闭UI回调
function this:onClose()
    this.initOfficialTimer = false
    --关闭子界面
    ViewManager.close(UIViewEnum.Platform_Global_View)
    ViewManager.close(UIViewEnum.MapView)
    ViewManager.close(UIViewEnum.PlatForm_Float_View)
    ViewManager.close(UIViewEnum.SearchView)
    if PlatformShopSearchView.isOpen then
        ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
    end
    self.isPlay = false
    this.resetChooseIcon()
    this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelClose)
	
	self:removeNotice()
end

this.curLngPosDirtyCaching = 0
this.curLatPosDirtyCaching = 0

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_UserInfo, this.onLoginSuccess)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onUpdateActivityListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_RedPacketListData, this.onUpdateRedPacketListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_CouponListData, this.onUpdateCouponListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_SearchActivityListData, this.onUpdateAllListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Change_ViewState, this.onChangePanelState)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_ShopView_ActivityList, this.LBS_ShopView_ActivityList)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Activity_ActiveStart_List, this.Activity_ActiveStart_List)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Activity_RecentGame_List, this.onActivity_RecentGame_List)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Map_Change_Scale_End, this.onOpenSearchBtn)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_OfficialListData, this.onUpdateOfficialListData)
    GlobalTimeManager.Instance.timerController:AddTimer(
        "GlobalShopView",
        -1,
        -1,
        function()
            --搜索按钮暂时每帧计算 （待优化）
            if
                this.main_mid.search_button.gameObject.activeSelf == true and
                    this.main_mid.detail_search_button.gameObject.activeSelf == true
             then
                return
            end

            if this.curLatPosDirtyCaching ~= MapManager.curLat or this.curLngPosDirtyCaching ~= MapManager.curLng then
                this.curLngPosDirtyCaching = MapManager.curLng
                this.curLatPosDirtyCaching = MapManager.curLat
                this.main_mid.detail_search_button.gameObject:SetActive(true)
                this.main_mid.curClassTxttitle.gameObject:SetActive(false)
                this.main_mid.search_button.gameObject:SetActive(true)
            end
        end
    )
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_UserInfo, this.onLoginSuccess)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onUpdateActivityListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_RedPacketListData, this.onUpdateRedPacketListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_CouponListData, this.onUpdateCouponListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Change_ViewState, this.onChangePanelState)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_ShopView_ActivityList, this.LBS_ShopView_ActivityList)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Map_Change_Scale_End, this.onOpenSearchBtn)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_SearchActivityListData, this.onUpdateAllListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Activity_ActiveStart_List, this.Activity_ActiveStart_List)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_OfficialListData, this.onUpdateOfficialListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Activity_RecentGame_List, this.onActivity_RecentGame_List)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalShopView")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalOfficialTimeCountDown")
end

function this.onLoginSuccess()
    --请求关注赛事列表
    PlatformShopModule.sendReqActiveStartListNotify()
    --请求最近参赛
    PlatformShopModule.sendReqFindRecentActive()
    this:activityClassifyByChooseType(this.curRightClickType)
end

this.currShopData = nil

------我的赛事---
this.gamePanelState = {}
--不同状态表现
this.gamePanelState.StateEnum = {
    None = 0, --无状态
    AttentionGame = 1, --关注赛事
    LastGame = 2 --最近参赛
}
this.lastGameTimes = 0
this.attentionGameTimes = 0
--关注赛事
local function onLastGame(self)
    this.main_mid.attention_game_bg:ChangeIcon(1)
    this.main_mid.last_game_bg:ChangeIcon(0)
    this.main_mid.none_game.gameObject:SetActive(this.lastGameTimes == 0)
    this.main_mid.attention_game_txt.text = "<color=#9a9a9aFF>关注赛事(" .. tostring(this.attentionGameTimes) .. ")</color>"
    this.main_mid.last_game_txt.text = "最近赛事(" .. tostring(this.lastGameTimes) .. ")"
    this.main_mid.attentionGame:SetCellData(this.lastGameActivityInfo or {}, self.onSetAttentionDetailInfo, true)
    this.main_mid.none_game_txt.text = "暂无参赛记录"
end
--最近参赛
local function onAttentionGame(self)
    this.main_mid.attention_game_bg:ChangeIcon(0)
    this.main_mid.last_game_bg:ChangeIcon(1)
    this.main_mid.none_game.gameObject:SetActive(this.attentionGameTimes == 0)
    this.main_mid.attention_game_txt.text = "关注赛事(" .. tostring(this.attentionGameTimes) .. ")"
    this.main_mid.last_game_txt.text = "<color=#9a9a9aFF>最近赛事(" .. tostring(this.lastGameTimes) .. ")</color>"
    this.main_mid.attentionGame:SetCellData(this.attentionGameActivityInfo or {}, self.onSetAttentionDetailInfo, true)
    this.main_mid.none_game_txt.text = "暂无关注赛事"
end
this.gamePanelState[this.gamePanelState.StateEnum.AttentionGame] = onAttentionGame
this.gamePanelState[this.gamePanelState.StateEnum.LastGame] = onLastGame

------------------------------切换城市------------------------------

local isInitHistoryCity = false
local historyCityIdList = {}
local historyCityItemList = {}
local hotCityItemList = {}

--更新城市相关
function this:updateCityPanel()
    this:updateHistoryCity()
    this:updateHotCity()
end

function this:updateHistoryCity()
    --历史城市
    for _, v in ipairs(historyCityItemList) do
        v:SetActive(false)
    end
    if not isInitHistoryCity then
        --PlayerPrefs.SetString("HistoryCity", "")
        local str = PlayerPrefs.GetString("HistoryCity", "")
        historyCityIdList = {}
        if str ~= "" then
            local result = string.split(str, "|")
            for i = 1, #result do
                historyCityIdList[i] = tonumber(result[i])
            end
        else
        end
        isInitHistoryCity = true
    end
    this.main_mid.history_Panel.gameObject:SetActive(false)
    for i = 1, #historyCityIdList do
        local id = historyCityIdList[i]
        if not historyCityItemList[i] then
            historyCityItemList[i] = GameObject.Instantiate(self.main_mid.cityItem.gameObject)
        end
        this.main_mid.history_Panel.gameObject:SetActive(true)
        historyCityItemList[i]:SetActive(true)
        UITools.SetParentAndAlign(historyCityItemList[i], this.main_mid.historyCityGroup.gameObject)
        local transform = historyCityItemList[i].transform
        local cityname = transform:Find("Normalcitybg/Normalcityname"):GetComponent(typeof(TextWidget))
        cityname.text = TableBaseCitySwitch.data[id].city_name
        local emptyImageWidget = transform:GetComponent(typeof(EmptyImageWidget))
        emptyImageWidget:AddEventListener(
            UIEvent.PointerClick,
            function()
                self.main_mid.city_button.Txt.text = TableBaseCitySwitch.data[id].city_name

                local b = self.curTopPanelState == this.ShowPanelState.StateEnum.TopPanelOpen
                local curState =
                    b and this.ShowPanelState.StateEnum.TopPanelClose or this.ShowPanelState.StateEnum.TopPanelOpen
                this:PanelStateExclusion(curState)

                MapManager.changeCurLngLat(TableBaseCitySwitch.data[id].lng, TableBaseCitySwitch.data[id].lat)

                this:addHistoryCity(TableBaseCitySwitch.data[id].id)
            end
        )
    end
end

function this:updateHotCity()
    --国内热门城市
    for _, v in ipairs(hotCityItemList) do
        v:SetActive(false)
    end
    if not table.empty(hotCityItemList) then
        for _, v in pairs(hotCityItemList) do
            v:SetActive(false)
        end
    end
    local len = #TableBaseCitySwitch.data
    if len > 20 then
        len = 20
    end

    for i = 1, len do
        if not hotCityItemList[i] then
            hotCityItemList[i] = GameObject.Instantiate(self.main_mid.cityItem.gameObject)
        end
        hotCityItemList[i]:SetActive(true)
        UITools.SetParentAndAlign(hotCityItemList[i], this.main_mid.HotCityGroup.gameObject)
        local transform = hotCityItemList[i].transform
        local cityname = transform:Find("Normalcitybg/Normalcityname"):GetComponent(typeof(TextWidget))
        cityname.text = TableBaseCitySwitch.data[i].city_name
        local emptyImageWidget = transform:GetComponent(typeof(EmptyImageWidget))

        emptyImageWidget:AddEventListener(
            UIEvent.PointerClick,
            function()
                this:addHistoryCity(TableBaseCitySwitch.data[i].id)
                self.main_mid.city_button.Txt.text = TableBaseCitySwitch.data[i].city_name

                local b = self.curTopPanelState == this.ShowPanelState.StateEnum.TopPanelOpen
                local curState =
                    b and this.ShowPanelState.StateEnum.TopPanelClose or this.ShowPanelState.StateEnum.TopPanelOpen
                this:PanelStateExclusion(curState)

                MapManager.changeCurLngLat(TableBaseCitySwitch.data[i].lng, TableBaseCitySwitch.data[i].lat)
            end
        )
    end
    local tempImageRect = this.main_mid.hotcity_Panel.rectTransform
    local tempwidth = 1080
    local tempHeight = #hotCityItemList * 42 + 300
    tempImageRect.sizeDelta = Vector2(tempwidth, tempHeight)
end

--添加历史城市
function this:addHistoryCity(id)
    local newList = {}
    newList[1] = id
    local index = 2
    for i = 1, #historyCityIdList do
        if historyCityIdList[i] ~= id then
            newList[index] = historyCityIdList[i]
            index = index + 1
        end
        if #newList >= 3 then
            break
        end
    end
    historyCityIdList = newList
    local str = ""
    for i = 1, #historyCityIdList do
        if i > 1 then
            str = str .. "|"
        end
        str = str .. tostring(historyCityIdList[i])
    end
    PlayerPrefs.SetString("HistoryCity", str)
    this:updateHistoryCity()
end

------------------------------切换城市end------------------------------

function this:onClickButton()
    ViewManager.open(UIViewEnum.Platform_Shop_Search_View)
    this:PanelStateExclusion(this.ShowPanelState.StateEnum.TopPanelClose)
end

--设置关注赛事详情信息
function this.onSetAttentionDetailInfo(go, data, index)
    local item = this.main_mid.attentionListArr[index + 1]
	this:setDetailInfo(item, data)
end

this.GameTypeState = {
    None = 0, --无
    XiaoChu = 1, --炫舞消除
    Majiang = 2, --麻将
    Buyu = 3, --捕鱼
    TiaoYiTiao = 4, --跳一跳
    TanYiTan = 5, --弹一弹
    TuYaTiaoyu = 6 --涂鸦跳跃
}
-- 更新活动详情
function this:showActivityDetail(activityInfo, showType, isExtern)
    if not isExtern then
        this.temp_activityInfo = activityInfo
        this.temp_showType = showType
    end
    local shopListRect = self.main_mid.shop_list_CellRecycleScrollPanel.rectTransform
    if showType == ActivityItem.ActivityType.Competition then
        self.main_mid.game_recycle_scroll.gameObject:SetActive(true)
        shopListRect.offsetMax = Vector2(shopListRect.offsetMax.x, -337)
        self.main_mid.game_recycle_scroll:SetCellData(
                GameFilterList,
                self.onSetGameTypeInfo,
                true
        )
    else
        self.main_mid.game_recycle_scroll.gameObject:SetActive(false)
        shopListRect.offsetMax = Vector2(shopListRect.offsetMax.x, -135)
    end
    if table.empty(activityInfo) then
        self.main_mid.activity_count_text.text = string.format("共%s个活动", 0)
        self.main_mid.shop_count_do_button_Txt.text = string.format("共%s个活动", 0)
        self.main_mid.shop_list_CellRecycleScrollPanel:SetCellData({}, self.onSetDetailInfo, true)
        this.main_mid.no_activity_Text.gameObject:SetActive(true)
        this.main_mid.no_activity_Text.text = "没有搜到相关活动"
        return
    end

    this.main_mid.no_activity_Text.gameObject:SetActive(false)
    this.main_mid.no_activity_Text.gameObject:SetActive(false)
    local function sortFun3(a, b)
        local left = a.startTime
        local right = b.startTime
        if left == nil or right == nil then
            return false
        end
        if left == right then
            return false
        end
        return left < right
    end

    local function sortFun2(a, b)
        local left = 0
        local right = 0
        if a.type == ActivityItem.ActivityType.Competition and a.rewardType == ActivityItem.ActivityRewardType.Coupon then
            left = a.rewardCouponCount
            right = b.rewardCouponCount
        else
            left = a.rewardCount
            right = b.rewardCount
        end

        if left == nil or right == nil then
            return false
        end
        if left == right then
            return sortFun3(a, b)
        end
        return left > right
    end
    table.sort(
        activityInfo,
        function(a, b)
            local left = a.sortWeight
            local right = b.sortWeight
            if left == nil or right == nil then
                return false
            end
            if left == right then
                return sortFun2(a, b)
            end
            return left > right
        end
    )
    local activityCount = #activityInfo
    --print("排序后的活动 = "..table.tostring(activityInfo))
    if showType == ActivityItem.ActivityType.RedPacket then
        self.main_mid.activity_count_text.text = string.format("共%s个红包", activityCount)
        self.main_mid.shop_count_do_button_Txt.text = string.format("共%s个红包", activityCount)
    elseif showType == ActivityItem.ActivityType.Coupon then
        self.main_mid.activity_count_text.text = string.format("共%s个优惠券", activityCount)
        self.main_mid.shop_count_do_button_Txt.text = string.format("共%s个优惠券", activityCount)
    elseif showType == ActivityItem.ActivityType.Competition then
        self.main_mid.activity_count_text.text = string.format("共%s个赛事", activityCount)
        self.main_mid.shop_count_do_button_Txt.text = string.format("共%s个赛事", activityCount)
        self.main_mid.game_recycle_scroll.gameObject:SetActive(true)
    else
        self.main_mid.activity_count_text.text = string.format("共%s个活动", activityCount)
        self.main_mid.shop_count_do_button_Txt.text = string.format("共%s个活动", activityCount)
    end

    self.main_mid.shop_list_CellRecycleScrollPanel:SetCellData(activityInfo, self.onSetDetailInfo, true)
    self.main_mid.shop_list_CellRecycleScrollPanel.scrollRect.enabled = true
    self.main_mid.shop_list_CellRecycleScrollPanel.mask.enabled = true
end

this.curCompetitionInfo = nil
function this:setCompetitionInfo(info)
    this.curCompetitionInfo = info
end

function this.resetChooseIcon(...)
    for k, v in pairs(this.main_mid.shopactivitylistcellArr) do
        if v.choose_togle.gameObject.activeSelf then
            v.choose_togle.gameObject:SetActive(false)
        end
    end
end

function this.sortGameType()
    local activityInfo = this.curCompetitionInfo
    local selectActivityList = {}
    for i = 1, #activityInfo do
        local activity = activityInfo[i]
        if activity.gameId == this.curGameTypeClickState then
            table.insert(selectActivityList, activity)
        end
    end
    this:showActivityDetail(selectActivityList, ActivityItem.ActivityType.Competition)
end

function this.onSetGameTypeInfo(go, data, index)
    local item = this.main_mid.shopactivitylistcellArr[index + 1]
    downloadGameIcon(data, item.game_image)
    item.game_image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this.resetChooseIcon()
            if data == this.curGameTypeClickState then
                this.curGameTypeClickState = this.GameTypeState.None
                this:showActivityDetail(this.curCompetitionInfo, ActivityItem.ActivityType.Competition)
            else
                this.curGameTypeClickState = data
                item.choose_togle.gameObject:SetActive(true)
                this.sortGameType()
            end
        end
    )
end

this.ChooseTypeTxt = {
    [0] = "全部活动",
    [1] = "红包活动",
    [2] = "优惠卷活动",
    [3] = "赛事活动",
    [4] = "官方活动"
}
local clickBaseColor = CSColor(154 / 255, 154 / 255, 154 / 244, 1)
local clickNewColor = CSColor(13 / 255, 172 / 255, 226 / 244, 1)
--分类筛选功能
function this:activityClassifyByChooseType(clickType)
    this.main_mid.activityMembers.gameObject:SetActive(false)
    self.main_mid.detailactivityMembers.gameObject:SetActive(false)
    this.main_mid.redpoint_bg.gameObject:SetActive(false)
    self.curRightClickType = clickType
    this.search_key = false

    for k, v in pairs(this.RightClickState.StateEnum) do
        self.main_mid[string.concat("ActivityBtnList", v, "txt")].color = clickBaseColor
    end
    self.main_mid[string.concat("ActivityBtnList", self.curRightClickType, "txt")].color = clickNewColor
    this.main_mid.chooseIcon.transform.localPosition =
        self.main_mid[string.concat("ActivityBtnList", self.curRightClickType)].transform.localPosition
    self.main_mid.activitybg:ChangeIcon(self.curRightClickType)

    local curRightTarget = self.main_mid[string.concat("ActivityBtnList", self.curRightClickType, "bg")]
    UITools.SetParentAndAlign(self.main_mid.RefreshIcon.gameObject, curRightTarget.gameObject)
    UITools.SetParentAndAlign(self.main_mid.detailmembersParent.gameObject, curRightTarget.gameObject)
    self.main_mid.RefreshIcon.gameObject:SetActive(false)

    self.main_mid.curShortClassTxt.text = string.concat("【", this.ChooseTypeTxt[self.curRightClickType], "】")
    self.main_mid.curDetailClassTxt.text = string.concat("【", this.ChooseTypeTxt[self.curRightClickType], "】")

    this.RightClickState[self.curRightClickType](self)
end

function this.onUpdateOfficialListData()
    this.CalTimerCount(self)
    if this.curRightClickType == this.RightClickState.StateEnum.AllActivity then
        return this.onUpdateAllListData()
    end
    if this.curRightClickType == this.RightClickState.StateEnum.ShopGameActivityActivity then
        this.onUpdateActivityListData()
    end
end

function this:onUpdateAllListData()
    this.main_mid.RefreshIcon.gameObject:SetActive(true)
    local active_list = PlatformLBSDataProxy.getAllActivityData()
    local activityInfo = {}
    if not table.empty(active_list) then
        for i = 1, #active_list do
            local activeItem = ActivityItem.createByCompetition(active_list[i])
            table.insert(activityInfo, activeItem)
        end
    end
    local official_list = PlatformLBSDataProxy.getOfficialList()
    if not table.empty(official_list) then
        for i = 1, #official_list do
            local activeItem = ActivityItem.createByCompetition(official_list[i])
            table.insert(activityInfo, activeItem)
        end
    end

    local couponredPacketList = PlatformLBSDataProxy.getAllRedBagCouponData()
    if not table.empty(couponredPacketList) then
        for i = 1, #couponredPacketList do
            local activeItem = ActivityItem.createByCoupon(couponredPacketList[i], PlatformLBSDataProxy.getRedBagShopDataByShopId(couponredPacketList[i].shop_id))
            table.insert(activityInfo, activeItem)
        end
    end
    local redPacketList = PlatformLBSDataProxy.getAllRedBagData()
    if not table.empty(redPacketList) then
        for i = 1, #redPacketList do
            if redPacketList[i].has_received == 0 then
                local activeItem = ActivityItem.createByRedPacket(redPacketList[i], PlatformLBSDataProxy.getRedBagShopDataByShopId(redPacketList[i].shop_id))
                table.insert(activityInfo, activeItem)
            end
        end
    end
    this.main_mid.activityMembers.gameObject:SetActive(#activityInfo > 0 and not this.search_key)
    this.main_mid.detailactivityMembers.gameObject:SetActive(#activityInfo > 0 and not this.search_key)
    this.main_mid.activityText.text = #activityInfo
    this.main_mid.detailactivityText.text = #activityInfo
    this:showActivityDetail(activityInfo, ActivityItem.ActivityType.All)
    this.Activity_ActiveStart_List()
end
function this.onUpdateActivityListData(...)
    if this.curRightClickType ~= this.RightClickState.StateEnum.ShopGameActivityActivity then
        return
    end
    this.main_mid.RefreshIcon.gameObject:SetActive(true)
    local data = PlatformLBSDataProxy.getAllActivityData()
    local activityInfo = {}
    local official_list = PlatformLBSDataProxy.getOfficialList()
    if not table.empty(official_list) then
        for i = 1, #official_list do
            local activeItem = ActivityItem.createByCompetition(official_list[i])
            table.insert(activityInfo, activeItem)
        end
    end
    if not table.empty(data) then
        for i = 1, #data do
            local activeItem = ActivityItem.createByCompetition(data[i])
            table.insert(activityInfo, activeItem)
        end
    end

    this.main_mid.activityMembers.gameObject:SetActive(#activityInfo > 0)
    this.main_mid.detailactivityMembers.gameObject:SetActive(#activityInfo > 0)
    this.main_mid.detailactivityText.text = #activityInfo
    this.main_mid.activityText.text = #activityInfo

    this.curGameTypeClickState = this.GameTypeState.None
    this.resetChooseIcon()
    this:setCompetitionInfo(activityInfo)
    this:showActivityDetail(activityInfo, ActivityItem.ActivityType.Competition)
    this.Activity_ActiveStart_List()
end
function this.onUpdateCouponListData(...)
    if this.curRightClickType ~= this.RightClickState.StateEnum.RedPacketCuponActivity then
        return
    end
    this.main_mid.RefreshIcon.gameObject:SetActive(true)
    local redPacketList = PlatformLBSDataProxy.getAllRedBagCouponData()
    local activityInfo = {}
    if not table.empty(redPacketList) then
        for i = 1, #redPacketList do
            local activeItem = ActivityItem.createByCoupon(redPacketList[i], PlatformLBSDataProxy.getRedBagShopDataByShopId(redPacketList[i].shop_id))
            table.insert(activityInfo, activeItem)
        end
        this.main_mid.activityMembers.gameObject:SetActive(#activityInfo > 0)
        this.main_mid.detailactivityMembers.gameObject:SetActive(#activityInfo > 0)
        this.main_mid.activityText.text = #activityInfo
        this.main_mid.detailactivityText.text = #activityInfo
    end
    this:showActivityDetail(activityInfo, ActivityItem.ActivityType.Coupon)
    this.Activity_ActiveStart_List()
end
function this.onUpdateRedPacketListData(...)
    if this.curRightClickType ~= this.RightClickState.StateEnum.RedPacketActivity then
        return
    end
    this.main_mid.RefreshIcon.gameObject:SetActive(true)
    local redPacketList = PlatformLBSDataProxy.getAllRedBagData()
    local activityInfo = {}
    if not table.empty(redPacketList) then
        for i = 1, #redPacketList do
            if redPacketList[i].has_received == 0 then
                local activeItem = ActivityItem.createByRedPacket(redPacketList[i], PlatformLBSDataProxy.getRedBagShopDataByShopId(redPacketList[i].shop_id))
                table.insert(activityInfo, activeItem)
            end
        end
        this.main_mid.activityMembers.gameObject:SetActive(#activityInfo > 0)
        this.main_mid.detailactivityMembers.gameObject:SetActive(#activityInfo > 0)
        this.main_mid.activityText.text = #activityInfo
        this.main_mid.detailactivityText.text = #activityInfo
    end
    this:showActivityDetail(activityInfo, ActivityItem.ActivityType.RedPacket)
    this.Activity_ActiveStart_List()
end

local function GetTimeStr(time)
    local m = math.floor((time) / 60)
    local s = time - m * 60
    local function full(num)
        if num < 10 then
            return string.concat("0", num)
        end
        return tostring(num)
    end
    return string.concat(full(m), ":", full(s))
end
this.officialTimeTextList = {}
this.sendHeartBeatTimes = 0
this.official_timer = 0
local function CountDowm(self)
    local curtimer = this.official_timer - TimeManager.getServerUnixTime()
    if not table.empty(this.officialTimeTextList) then
        for _, targettext in pairs(this.officialTimeTextList) do
            if targettext then
				if curtimer <= 0 then
					targettext.text = "活动已结束"
					PlatformLBSModule.delaySendReqGetOfficalActivity()
				else
					targettext.text =  string.concat("结束倒计时: ", GetTimeStr(curtimer))
				end
            end
        end
    end
end
this.initOfficialTimer = false
function this.CalTimerCount(self)
    if this.initOfficialTimer then
        return
    end
    this.initOfficialTimer = true
    CountDowm(self)
    GlobalTimeManager.Instance.timerController:AddTimer(
        "GlobalOfficialTimeCountDown",
        1000,
        -1,
        function()
            CountDowm(self)
        end
    )
end

--设置活动详情信息
function this.onSetDetailInfo(go, data, index)
    local item = this.main_mid.shoplistcellArr[index + 1]
    this:setDetailInfo(item, data)
end

function this:setDetailInfo(item, data)
	local function onClickItemHandler(data)
        if data.type == ActivityItem.ActivityType.Competition then
            if data.isLocalOfficial then
                ViewManager.open(
                    UIViewEnum.Platform_LOCAL_OFFICIAL_RULE,
                    {match_type = data.match_type, id = data.id, endTime = data.endTime, game_state = data.game_state}
                )
            else
				PlatformGlobalShopChatView.showPlatformGlobalShopChatView(data.id)
            end
        else
            local msg = {}
            msg.redpacketId = data.id
            msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
            msg.isFromChat = false
            msg.headUrl = data.shopImageUrl
            msg.name = data.shopName
            msg.title = data.title
            msg.describe = data.describe
            msg.describeImageList = data.describeImageList
            msg.packetStyle = data.packetStyle
            msg.is_official = data.is_official
			
			msg.positionLimitType = data.positionLimitType
			msg.cityCode = data.cityCode
			msg.lng = data.lng
			msg.lat = data.lat
			
            local isCoupon = data.type == ActivityItem.ActivityType.Coupon
            if isCoupon then
                msg.coupon_id = data.couponId
                msg.couponName = data.rewardDescribe
                msg.iconUrl = data.iconUrl
                PlatformRedPacketProxy.SetOpenLBSPacketData("Coupon_Open_Data", msg)
				PlatformLBSCouponOpenView.openPlatformLBSCouponOpenView(true)
            else
                PlatformRedPacketProxy.SetOpenLBSPacketData("RedPacket_Open_Data", msg)
                PlatformLBSRedPacketOpenView.openLBSRedPacketOpenView()
            end
        end
    end
	
	item.pressbg:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            onClickItemHandler(data)
        end
    )
	
	--商店名
    item.shop_name_text.text = data.isLocalOfficial and "官方赛" or data.shopName
	--商店头像
	if data.isLocalOfficial then
		--官方赛的商家头像特殊处理
		downloadMerchantHead("MerchantHead/official", item.shop_head_image)
	else
		downloadMerchantHead(data.shopImageUrl, item.shop_head_image)
	end
	
    if this.officialTimeTextList[item.count_time] then
        this.officialTimeTextList[item.count_time] = nil
    end
	
	--可参赛状态
    item.join_state_icon:ChangeIcon(data.joinState)
	
    if data.type == ActivityItem.ActivityType.Competition then
		--赛事
		
		--游戏图标
		item.redbag_icon.gameObject:SetActive(false)
		item.coupon_icon.gameObject:SetActive(false)
        item.game_icon.gameObject:SetActive(true)
        downloadGameIcon(data.gameId, item.game_icon)
		
        item.get_state_icon.gameObject:SetActive(false)

		--奖励类型
		item.reward_icon.gameObject:SetActive(true)
		item.reward_icon:ChangeIcon(data.rewardType)
	
        if data.isLocalOfficial then
			--官方赛
            local official_config = TableBaseOfficalMatch.data[data.match_type]
            local game_config = TableBaseGameList.data[data.gameId]
			
            item.shop_intro_text.text = official_config.title
			
			--结束时间
            item.shop_opentime_text.text = "结束时间:  "..os.date("%m月%d日 %H:%M", data.endTime)
			
			--倒计时
			item.shop_reward_bg.gameObject:SetActive(false)
			item.time_bg.gameObject:SetActive(true)
			
            this.officialTimeTextList[item.count_time] = item.count_time
            downloadGameIcon(data.gameId, item.officialGameIcon)
            if this.official_timer ~= data.endTime then
                this.official_timer = data.endTime
            end
            this.CalTimerCount(self)
			return
        end
    else
		item.game_icon.gameObject:SetActive(false)
		if data.type == ActivityItem.ActivityType.RedPacket then
			--红包
			item.redbag_icon.gameObject:SetActive(true)
			item.coupon_icon.gameObject:SetActive(false)
		elseif data.type == ActivityItem.ActivityType.Coupon then
			--优惠券
			item.redbag_icon.gameObject:SetActive(false)
			item.coupon_icon.gameObject:SetActive(true)
		end
			
        item.get_state_icon.gameObject:SetActive(true)
		item.reward_icon.gameObject:SetActive(false)
    end
	
	--活动名
	item.shop_intro_text.text = data.title
	
	--奖励
	item.time_bg.gameObject:SetActive(false)
	item.shop_reward_bg.gameObject:SetActive(true)
    if data.rewardType == ActivityItem.ActivityRewardType.None then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Gold then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Cash then
        local text = string.format("<size=24>%s</size>%s", "￥", math.floor(data.rewardCount / 100))
        item.shop_reward_text.text = text
    elseif data.rewardType == ActivityItem.ActivityRewardType.UTicket then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Coupon then
        item.shop_reward_text.text = data.rewardDescribe
    else
        printError("错误, 未知奖励类型: " .. data.rewardType)
    end

    if data.rewardType == ActivityItem.ActivityRewardType.Coupon then
		local width = item.shop_reward_text.Txt.preferredWidth
		if width < 300 then
			width = 300
		elseif width > 500 then
			width = 500
		end
        item.shop_reward_bg.rectTransform.sizeDelta = Vector2(width, 68)
        item.shop_reward_text.rectTransform.offsetMin = Vector2(0, 0)
        item.shop_reward_text.rectTransform.offsetMax = Vector2(0, 0)
    else
        item.shop_reward_bg.rectTransform.sizeDelta = Vector2(300, 68)
        item.shop_reward_text.rectTransform.offsetMin = Vector2(80, 0)
        item.shop_reward_text.rectTransform.offsetMax = Vector2(0, 0)
    end

    item.shop_opentime_text.text =
        string.format(
        "时间: %s-%s %02s:%02s 至 %s-%s %02s:%02s",
        data.startTime.Month,
        data.startTime.Day,
        data.startTime.Hour,
        data.startTime.Minute,
        data.endTime.Month,
        data.endTime.Day,
        data.endTime.Hour,
        data.endTime.Minute
    )
end

-----------------------------------------------------------------------------------
--状态互斥
function this:PanelStateExclusion(state)
    self.main_mid.pressPanel.gameObject:SetActive(
        state == this.ShowPanelState.StateEnum.BottomPanelShortOpen or
            state == this.ShowPanelState.StateEnum.BottomPanelDetailOpen
    )
    if state >= this.ShowPanelState.StateEnum.TopPanelOpen and state <= this.ShowPanelState.StateEnum.TopPanelClose then
        this.ShowPanelState[this.ShowPanelState.StateEnum.RightPanelDetailClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.AttentionPanelClose](self)
    elseif
        state >= this.ShowPanelState.StateEnum.BottomPanelClose and
            state <= this.ShowPanelState.StateEnum.BottomPanelDetailOpen
     then
        this.ShowPanelState[this.ShowPanelState.StateEnum.RightPanelDetailClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.TopPanelClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.AttentionPanelClose](self)
    elseif
        state >= this.ShowPanelState.StateEnum.RightPanelDetailOpen and
            state <= this.ShowPanelState.StateEnum.RightPanelDetailOpen
     then
        this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.TopPanelClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.AttentionPanelClose](self)
    elseif
        state >= this.ShowPanelState.StateEnum.AttentionPanelOPen and
            state <= this.ShowPanelState.StateEnum.AttentionPanelClose
     then
        this.ShowPanelState[this.ShowPanelState.StateEnum.RightPanelDetailClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.BottomPanelClose](self)
        this.ShowPanelState[this.ShowPanelState.StateEnum.TopPanelClose](self)
    end
    this.ShowPanelState[state](self)
end

--外部接口改变界面状态
function this.onChangePanelState(key, rsp)
    local state = rsp:GetObj()
    if state == PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen then
        this.ParentRect.offsetMax = Vector2(this.ParentRect.offsetMax.x, -1600)
        this.main_mid.mid_scratch_Panel.gameObject:SetActive(true)
        this.main_mid.enterActivity.gameObject:SetActive(false)
        startLerpMove(
            this,
            this.ParentRect.offsetMax,
            Vector2(this.ParentRect.offsetMax.x, -730),
            function(...)
                this:PanelStateExclusion(this.ShowPanelState.StateEnum.BottomPanelShortOpen)
            end,
            0.2
        )
    else
        this:PanelStateExclusion(state)
    end
end
--外部接口商家活动列表切换
function this.LBS_ShopView_ActivityList(key, rsp)
    local activityInfo = rsp:GetObj()
    if activityInfo.activityType == ActivityItem.ActivityType.Competition then
        this.curGameTypeClickState = this.GameTypeState.None
        this.resetChooseIcon()
        this:setCompetitionInfo(activityInfo)
    end
    this:showActivityDetail(activityInfo, activityInfo.activityType, true)
end

function this:setSearchText(text)
    this.main_mid.search_Text.text = text
end

function this.onOpenSearchBtn()
    if not LoginDataProxy.isGetUserInfo then
        return
    end
    if this.main_mid.search_button.gameObject.activeSelf == true then
        this.main_mid.search_button.gameObject:SetActive(false)
    end
    if this.main_mid.detail_search_button.gameObject.activeSelf == true then
        this.main_mid.detail_search_button.gameObject:SetActive(false)
        this.main_mid.curClassTxttitle.gameObject:SetActive(true)
    end
    this.curLngPosDirtyCaching = MapManager.curLng
    this.curLatPosDirtyCaching = MapManager.curLat
    --放大缩小默认搜索一次
    --this.RightClickState[this.curRightClickType](this)
    this:activityClassifyByChooseType(this.curRightClickType)
end
--关注赛事列表
function this.Activity_ActiveStart_List()
    local activityList = PlatformLBSDataProxy.getActiveStartList()
    this.arrowCount = table.nums(activityList)
    this.main_mid.redpoint_bg.gameObject:SetActive(this.arrowCount > 0)
    this.main_mid.times.text = this.arrowCount
    this.main_mid.redpoint_bg.transform:GetComponent(typeof(RectTransform)).sizeDelta =
        Vector2(this.main_mid.times.Txt.preferredWidth + 23, 36)

    if this.curAttentionState == this.ShowPanelState.StateEnum.AttentionPanelOPen then
        this.curAttentionState = this.ShowPanelState.StateEnum.None
        onAttentionPanelOPen(this)
    end
end
--最近参赛列表
function this.onActivity_RecentGame_List()
    if this.curAttentionState == this.ShowPanelState.StateEnum.AttentionPanelOPen then
        this.gamePanelState[this.currGamePanelState](this)
    end
end
