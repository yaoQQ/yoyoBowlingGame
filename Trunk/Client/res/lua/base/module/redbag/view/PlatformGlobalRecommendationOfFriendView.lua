require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_recommendation_friend_panel"

PlatformGlobalRecommendationOfFriendView = BaseView:new()
local this = PlatformGlobalRecommendationOfFriendView
this.viewName = "PlatformGlobalRecommendationOfFriendView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Global_RecommendationOfFriend_View, false)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_recommendation_friend_panel"
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
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    this:addNotice()
    this:updateRecommendationOfFriend()
end

function this:onClose()
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info)
    this:removeNotice()
end

function this:addNotice()
end

function this:removeNotice()
end
function this:addEvent()
    self.main_mid.recommendation_friend_close_Image:AddEventListener(
        UIEvent.PointerClick,
        self.recommendationFriendCloseImg
    )
    self.main_mid.add_all_Button:AddEventListener(UIEvent.PointerClick, self.addAllBtn)
    self.main_mid.in_batch_Button:AddEventListener(UIEvent.PointerClick, self.inBatchBtn)
    self.main_mid.recommendation_friend_bg_Image:AddEventListener(
        UIEvent.PointerClick,
        self.recommendationFriendCloseImg
    )
end

this.currRedbagRecommendData = {}
local m_RedbagRecommendData = {}
local i = 1
--更新红包界面
function this:updateRecommendationOfFriend()
    this.currRedbagRecommendData = PlatformFriendProxy:GetInstance():getRecommendFriendData() or {}
    this.currFriendDataIndex = 0
    this.main_mid.recommendation_friend_explain_Text.text = "推荐好友"
    local dataCount = #this.currRedbagRecommendData
    this.main_mid.nomorerecommend_Text.gameObject:SetActive(dataCount <= 0)
    this.updateRecommendFriendList()
end

function this.updateRecommendFriendList()
    local tempData = {}
    for i = 1, 5 do
        local value = this.currRedbagRecommendData[(this.currFriendDataIndex * 5) + i]
        if value then
            value.index = (this.currFriendDataIndex * 5) + i
            table.insert(tempData, value)
        end
    end
    this.main_mid.recommendationFriendCellRecycleScrollPanel:SetCellData(tempData, this.updateRecommendFriendCellList)
    m_RedbagRecommendData = tempData
    this:onChange()
end

function this.updateRecommendFriendCellList(go, data, index)
    local item = this.main_mid.recommendationFriendCellArr[index + 1]
    local addData = PlatformFriendProxy:GetInstance():getSendAddFriendApplyData()

    if PlatformFriendProxy:GetInstance():isMyAddFriendById(data.player_id) then
        item.add_end_Image.gameObject:SetActive(true)
        item.add_friend_Button.gameObject:SetActive(false)
    else
        item.add_end_Image.gameObject:SetActive(false)
        item.add_friend_Button.gameObject:SetActive(true)
    end

    downloadUserHead(data.head_url, item.head_Icon)
    item.name_Text.text = data.nick_name
    if data.address == nil or data == nil then
        return
    end
    if data.address == "" then
        item.introduce_Text.text = "所在位置:暂未定位"
    else
        item.introduce_Text.text = "所在位置:" .. data.address
    end
    item.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
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
    )
    item.add_friend_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            local selfData = PlatformUserProxy:GetInstance():getUserInfo()
            item.add_end_Image.gameObject:SetActive(true)
            item.add_friend_Button.gameObject:SetActive(false)
            local sendData = {
                op = ProtoEnumFriendModule.FriendOp.FriendOpAddFriend,
                player_id = data.player_id,
                strparam = "我是" .. selfData.nick_name
            }
            PlatformFriendModule.onReqFriendOp(sendData, data)
        end
    )
end

--点击事件
function this:inBatchBtn(args)
    if this.currRedbagRecommendData == nil then
        return
    end
    local tempMaxIndex = math.floor(#this.currRedbagRecommendData / 5)
    this.currFriendDataIndex = this.currFriendDataIndex >= tempMaxIndex and 0 or this.currFriendDataIndex + 1
    this.updateRecommendFriendList()
end

function this:recommendationFriendCloseImg()
    if this.recommendationFriendCloseImgEvent ~= nil then
        this.recommendationFriendCloseImgEvent()
    end
    this.addAllBtnEvent = nil
    this.inBatchBtnEvent = nil
    ViewManager.close(UIViewEnum.Platform_Global_RecommendationOfFriend_View)
end

function this:addAllBtn()
    local selfData = PlatformUserProxy:GetInstance():getUserInfo()
    if m_RedbagRecommendData ~= nil then
        for i = 1, #m_RedbagRecommendData do
            local sendData = {
                op = ProtoEnumFriendModule.FriendOp.FriendOpAddFriend,
                player_id = m_RedbagRecommendData[i].player_id,
                strparam = "我是" .. selfData.nick_name
            }
            this.main_mid.recommendationFriendCellArr[i].add_end_Image.gameObject:SetActive(true)
            this.main_mid.recommendationFriendCellArr[i].add_friend_Button.gameObject:SetActive(false)
            PlatformFriendModule.onReqFriendOp(sendData, m_RedbagRecommendData[i])
        end
        this.main_mid.add_all_Button.gameObject:SetActive(false)
        this.main_mid.no_all_Button.gameObject:SetActive(true)
    end
end

function this:onChange()
    local num = 0
    for i = 1, #m_RedbagRecommendData do
        if PlatformFriendProxy:GetInstance():isMyAddFriendById(m_RedbagRecommendData[i].player_id) then
            num = num + 1
        end
    end
    if num == #m_RedbagRecommendData then
        this.main_mid.add_all_Button.gameObject:SetActive(false)
        this.main_mid.no_all_Button.gameObject:SetActive(true)
        num = 0
    else
        this.main_mid.add_all_Button.gameObject:SetActive(true)
        this.main_mid.no_all_Button.gameObject:SetActive(false)
    end
    --todo这个消息刷新很频繁不合理
    --NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info)
end
