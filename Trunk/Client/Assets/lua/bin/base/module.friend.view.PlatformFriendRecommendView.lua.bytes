require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_recommend_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformFriendRecommendView = BaseView:new()
local this = PlatformFriendRecommendView
this.viewName = "PlatformFriendRecommendView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Friend_Recommend_View, false)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_recommend_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    self:addNotice()
    self:initView()
end

function this:initView()
    local req = {}
    req.lng = MapManager.userLng
    req.lat = MapManager.userLat
    req.distance = 1000000 --暂定1公里
    req.maxcount = 1000 --暂定最多1000个商家
    PlatformFriendModule.onSendRecommend(req)
    this.currFriendDataIndex = 0
    this.main_mid.recommend_CellGroup:SetCellData({}, this.updateRecommendFriendCellList)
end

this.isChange = false
function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Friend_Recommend_View)
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )

    self.main_mid.scan_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            -- 调出扫描界面
            PlatformSDK.scanQRCode()
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )

    self.main_mid.search_InputField:AddEventListener(
        UIEvent.PointerClick,
        function()
            local data = {}
            data.isInstanceSearch = false
            data.myFun = function(inputTxt) --这里传入点击了确定按钮之后的逻辑
                PlatformUserModule.sendReqBaseUserInfo(tonumber(inputTxt))
            end
            ViewManager.open(UIViewEnum.Platform_Common_Search_View, data)
        end
    )

    self.main_mid.myscanpic_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(
                UIViewEnum.Platform_Common_QRCode_View,
                {
                    scanText = "yoyo&add_friend&"..LoginDataProxy.playerId,
                    titleText = "扫码加我为好友",
                    tip1Text = "[好友越多，红包越多，玩得越爽！]"
                }
            )
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )

    self.main_mid.change_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            if this.currFriendRecommendData == nil then
                return
            end
            local tempMaxIndex = math.floor(#this.currFriendRecommendData / 6)
            this.currFriendDataIndex = this.currFriendDataIndex >= tempMaxIndex and 0 or this.currFriendDataIndex + 1
            this:updateFriendRecommendList()
        end
    )
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
end
--刷新数据
function this.onRspRecommend()
    this.currFriendDataIndex = 0
    this:updateFriendRecommendList()
end
function this:onClose()
    this:removeNotice()
end

this.currFriendRecommendData = nil
local i = 1
--更新好友推荐列表
function this.updateFriendRecommendList()
    this.currFriendRecommendData = PlatformFriendProxy:GetInstance():getRecommendFriendData() or {}
    this.main_mid.nomorerecommend_Text.gameObject:SetActive(#this.currFriendRecommendData <= 0)
    local tempData = {}
    for i = 1, 6 do
        local value = this.currFriendRecommendData[(this.currFriendDataIndex * 6) + i]
        if value then
            value.index = (this.currFriendDataIndex * 6) + i
            table.insert(tempData, value)
        end
    end
    this.main_mid.recommend_CellGroup:SetCellData(tempData, this.updateRecommendFriendCellList)
end

function this.updateRecommendFriendCellList(go, data, index)
    local item = this.main_mid.newrecommendcellArr[index + 1]
    downloadUserHead(data.head_url, item.head_Image)
    item.name_Text.text = data.nick_name

    item.adress_Text.text = data.address
    if data.sex == 0 then
        item.sexbg_Icon:ChangeIcon(1)
    else
        item.sexbg_Icon:ChangeIcon(data.sex - 1)
    end
    item.press_Image.name = data.player_id

    local fun = function()
        if data == nil then
            return
        end
        PlatformFriendProxy:GetInstance():setFriendMainPageData(data)
        local data = {}
        data.isActive = true
        data.isRecommendView = true
        ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
        ViewManager.close(UIViewEnum.Platform_Common_Search_View)
    end
    item.press_Image:AddEventListener(UIEvent.PointerClick, fun)
end

this.currFriendSearchData = nil

--更新好友搜索
function this:updateFriendSearchList()
    this.currFriendSearchData = PlatformFriendProxy:GetInstance():getBaseUserInfo()

    if this.currFriendSearchData == nil then
        return
    end

    this.currFriendSearchData = this.currFriendSearchData.user_info

    if this.currFriendSearchData == nil then
        return
    end

    PlatformFriendProxy:GetInstance():setFriendMainPageData(this.currFriendSearchData)

    local data = {}
    data.isActive = true
    data.isRecommendView = true
    ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
    ViewManager.close(UIViewEnum.Platform_Common_Search_View)
end
