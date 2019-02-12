require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_shop_search_panel"
require "base:module/shop/data/PlatformShopSearchProxy"

--主界面：商铺
PlatformShopSearchView = BaseView:new()
local this = PlatformShopSearchView
this.viewName = "PlatformShopSearchView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Shop_Search_View, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_shop_search_panel"
}

local m_buttonSearch = {}
local m_thisPanel = {}
--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    --thisPanel = GameObject.Instantiate(this.main_mid.second_Panel.gameObject)
    self:addEvent()
end

local coOpenSubView = nil
local panelNum = 1
local hotNum = 1
local m_shopSearchData = nil
local m_shopHotSearchData = nil

--override 打开UI回调
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()

    --self.main_mid.mid_unscratch_Panel.gameObject:SetActive(true)
    self.main_mid.mid_scratch_Panel.gameObject:SetActive(false)
    self:addNotice()
    self:initView()
    self:reqGetHotText()
    --self.onUpdateShopSearchData()
end

local isOpenCity = false
function this:addEvent()
    self.main_mid.cancel_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
        end
    )
    self.main_mid.close_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
        end
    )

    self.main_mid.activity_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
            NoticeManager.Instance:Dispatch(
                NoticeType.LBS_Change_ViewState,
                PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
            )
        end
    )

    self.main_mid.clear_search_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.main_mid.search_Inputfield.text = ""
        end
    )
    self.main_mid.city_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
            NoticeManager.Instance:Dispatch(
                NoticeType.LBS_Change_ViewState,
                PlatformGlobalShopView.ShowPanelState.StateEnum.TopPanelOpen
            )
        end
    )
    self.main_mid.search_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            PlatformShopSearchProxy.addShopLatelySearchData(this.main_mid.search_Inputfield.text)
            this:sendSearchInfo()
        end
    )
    self.main_mid.clear_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            PlatformShopSearchProxy.clearShopLatelySearchData()
            this:clearLatelySearchEvent()
            m_shopSearchData = nil
        end
    )
end
--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    local text = ""
    PlatformGlobalShopView:setSearchText(text)
    this:clearHotSearchEvent()
    this:clearLatelySearchEvent()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_HotSearchListData, this.onUpdateShopSearchData)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_HotSearchListData, this.onUpdateShopSearchData)
end
--打开界面时初始化
function this:initView()
    panelNum = 1
    hotNum = 1
end

function this:reqGetHotText()
    PlatformSearchModule.sendReqGetHotSearchText()
end

--进行搜索协议
function this:sendSearchInfo()
    --printDebug("++++++++++++++++++++++++我发送搜索协议喽")
    --ShowWaiting(true, "Shop_SerchDownload")
    ViewManager.close(UIViewEnum.Platform_Shop_Search_View)
    local text = PlatformShopSearchProxy.getShopSearchText()
    PlatformGlobalShopView:setSearchText(text)

    PlatformLBSDataProxy.clearAllActivityData()
    MapManager.changeCurZoom(5)
    PlatformLBSModule.sendReqNearAllActivity(MapManager.curLng, MapManager.curLat, 50000000, text)

    ShowSearching(true, "activityClassify")

    PlatformGlobalShopView.search_key = true

    NoticeManager.Instance:Dispatch(
        NoticeType.LBS_Change_ViewState,
        PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
    )
    -- GlobalTimeManager.Instance.timerController:AddTimer(
    -- 	"SearchWaiting",2000,1,function ()
    -- 		ShowWaiting(false, "Shop_SerchDownload")
    -- 		GlobalTimeManager.Instance.timerController:RemoveTimerByKey("Shop_SerchDownload")
    -- 	end)
    --showFloatTips("敬请期待，功能协议开发中...")
end

--更新搜索记录
function this.onUpdateShopSearchData()
    this.main_mid.search_Inputfield.inputField:ActivateInputField()
    this.main_mid.search_Inputfield:OnValueChanged(
        function(obj)
            if this.main_mid.search_Inputfield.text == "" or this.main_mid.search_Inputfield.text == nil then
                this.main_mid.search_Button.gameObject:SetActive(false)
                this.main_mid.cancel_Button.gameObject:SetActive(true)
                this.main_mid.clear_search_Image.gameObject:SetActive(false)
            else
                this.main_mid.search_Button.gameObject:SetActive(true)
                this.main_mid.cancel_Button.gameObject:SetActive(false)
                this.main_mid.clear_search_Image.gameObject:SetActive(true)
            end
        end
    )

    m_shopHotSearchData = PlatformShopSearchProxy.getHotSearchData()
    m_shopSearchData = PlatformShopSearchProxy.getShopLatelySearchData()
    --printDebug("+++++++++++++++++++++++++++++++我是获取的附近搜索"..table.tostring(tab))

    --if hotTab == "" and tab == "" then
    --this.main_mid.mid_Panel.gameObject:SetActive(false)
    --else
    --this.main_mid.mid_Panel.gameObject:SetActive(true)
    --end

    if m_shopHotSearchData == nil then
        this.main_mid.hot_Panel.gameObject:SetActive(false)
        if m_shopSearchData == nil or m_shopSearchData == "" then
            this.main_mid.mid_Panel.gameObject:SetActive(false)
        else
            this.main_mid.mid_Panel.gameObject:SetActive(true)
        end
    else
        this.main_mid.hot_Panel.gameObject:SetActive(true)
        --热门搜索所传参数
        local data = {}
        data.table = m_shopHotSearchData
        this:bgSortEvent(data, this.main_mid.hot_first_Panel.transform, 10)
    end

    if m_shopSearchData == nil or m_shopSearchData == "" then
        this.main_mid.clear_Image.gameObject:SetActive(false)
        this.main_mid.lately_Panel.gameObject:SetActive(false)
    else
        this.main_mid.clear_Image.gameObject:SetActive(true)
        this.main_mid.lately_Panel.gameObject:SetActive(true)
        --最近搜索所传参数
        local msg = {}
        msg.table = m_shopSearchData
        this:bgSortEvent(msg, this.main_mid.lately_first_Panel.transform, 10)
    end
    --	printDebug("+++++++++++++++++++++++++++++++我是获取的附近搜索"..table.tostring(tab))
end

--热搜信息排列
--panel 父节点，hot_first_Panel热点，lately_first_Panel最近
--num 显示的个数
--data 所传参数1：所传数据table内容，参数2：goFun点击时跳转的方法
---@param data table
---@param num int
function this:bgSortEvent(data, panel, num)
    this.Panel = GameObject.Instantiate(this.main_mid.second_Panel.gameObject).transform
    this.Panel:SetParent(panel)
    this.Panel.localScale = Vector3(1, 1, 1)
    local allWidth = 0
    --判断table数值与所需num数值

    if #data.table < num then
        num = #data.table
    end
    for i = 1, num do
        local strLenth = string.len(data.table[i]) * 15 + 80

        if allWidth + strLenth > 970 then
            --热门搜索只显示两行
            if i == 1 then
            else
                if
                    panel == this.main_mid.hot_first_Panel.transform and hotNum > 1 or
                        panel == this.main_mid.lately_first_Panel.transform and panelNum > 3
                 then
                    return
                else
                    local panelGo = nil
                    local len1 = #m_thisPanel
                    if len1 > 0 then
                        panelGo = m_thisPanel[len1]
                        m_thisPanel = nil
                    else
                        panelGo = GameObject.Instantiate(this.main_mid.second_Panel.gameObject)
                    end
                    this.Panel = panelGo.transform
                    this.Panel:SetParent(panel)
                    this.Panel.localScale = Vector3(1, 1, 1)
                    if panel == this.main_mid.hot_first_Panel.transform then
                        hotNum = hotNum + 1
                    else
                        panelNum = panelNum + 1
                    end

                    allWidth = 0
                end
            end
        end

        local buttonGo = nil
        local len = #m_buttonSearch
        if len > 0 then
            buttonGo = m_buttonSearch[len]
            m_buttonSearch = nil
        else
            buttonGo = GameObject.Instantiate(this.main_mid.show_Image.gameObject)
        end
        this.Image = buttonGo
        this.Image.transform:SetParent(this.Panel)
        this.Image.transform.localScale = Vector3(1, 1, 1)
        this.Image.transform:GetChild(0).name = 11111

        --热门搜索点击响应方法
        local fun = function()
            PlatformShopSearchProxy.addShopLatelySearchData(data.table[i])
            this:sendSearchInfo()
        end

        this.Text = this.Image.transform:GetChild(0).transform:GetComponent(typeof(TextWidget))
        if strLenth > 970 then
            this.Text.text = string.sub(data.table[i], 1, 60) .. ".."
        else
            this.Text.text = data.table[i]
        end
        this.Image.transform:GetComponent(typeof(ImageWidget)):AddEventListener(UIEvent.PointerClick, fun)
        local tempwidth = this.Text.Txt.preferredWidth + 80
        local tempHeight = 89.5
        tempImageRect = this.Image.transform:GetComponent(typeof(RectTransform))
        tempImageRect.sizeDelta = Vector2(tempwidth, tempHeight)
        allWidth = allWidth + tempwidth + 15
        this:fitHeight()
    end
end

function this:fitHeight()
    tempImageRect = this.main_mid.hot_Panel.transform:GetComponent(typeof(RectTransform))
    local tempwidth = 1080
    local tempHeight = 100 * hotNum + 120
    tempImageRect.sizeDelta = Vector2(tempwidth, tempHeight)
    latelyImageRect = this.main_mid.lately_Panel.transform:GetComponent(typeof(RectTransform))
    local latelywidth = 1080
    local latelyHeight = 100 * panelNum + 120
    latelyImageRect.sizeDelta = Vector2(latelywidth, latelyHeight)
end

function this:clearHotSearchEvent()
    if m_shopHotSearchData == nil then
        return
    end
    for i = 1, hotNum do
        printDebug("++++++++++++++" .. hotNum)
        -- if hotNum == 1 then return end
        allChild = this.main_mid.hot_first_Panel.transform:GetChild(i - 1).gameObject
        GameObject.Destroy(allChild)
    end

    this.main_mid.hot_Panel.gameObject:SetActive(false)
    hotNum = 0
end
function this:clearLatelySearchEvent()
    if m_shopSearchData == nil or m_shopSearchData == "" then
        return
    end
    for i = 1, panelNum do
        allChild = this.main_mid.lately_first_Panel.transform:GetChild(i - 1).gameObject
        GameObject.Destroy(allChild)
    end
    this.main_mid.clear_Image.gameObject:SetActive(false)
    this.main_mid.lately_Panel.gameObject:SetActive(false)
    panelNum = 0
end

-----------------------------------------------------------------------------------
