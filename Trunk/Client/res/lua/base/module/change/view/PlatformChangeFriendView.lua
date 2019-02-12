---Change From PlatformFriendView

require "base:enum/UIViewEnum"
require "base:mid/change/Mid_platform_change_friend_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformChangeFriendView = BaseView:new()
local this = PlatformChangeFriendView
this.viewName = "PlatformChangeFriendView"

--设置面板特性
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Change_Friend_View, true)

--设置加载列表
this.loadOrders = {
    "base:change/platform_change_friend_panel"
}

--初始化预制体，给main_mid赋值

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end
local color = nil
function this:onShowHandler(msg)
    color = self.main_mid.friendcount_Button.Txt.color
    self:initView()
    this:addNotice()
    --向服务器发送更新好友信息列表请求
    local data = {
        op = ProtoEnumFriendModule.FriendOp.FriendOpReqList,
        param1 = 0,
        param2 = 100
    }
    PlatformFriendModule.onReqFriendOp(data)
    --打开子界面
    ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Change_Friend_View)
end
local isStart = false
function this:initView()
    if not isStart then
        isStart = true
        this.main_mid.friend_CellRecycleScrollPanel:SetCellData({}, this.updateFriendCellList, true)
        this.main_mid.friend_Text.text = 0
    end
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateFriendList)
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.onUpdateFriendApplyPoint)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateFriendList)
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.onUpdateFriendApplyPoint)
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    ViewManager.close(UIViewEnum.Platform_Common_Search_View)
end
local inputText = nil
function this:addEvent()
    self.main_mid.add_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Friend_Search_View)
        end
    )
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.close(UIViewEnum.Platform_Change_Friend_View)
            ViewManager.open(UIViewEnum.Platform_Global_Personal_View)
        end
    )
end

function this:onUpdateFriendApplyPoint()
    local applyData = PlatformFriendProxy:GetInstance():getReceiveAddFriendApplyData()
    this.main_mid.apply_config_Icon:ChangeIcon(0)
    this.main_mid.apply_config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Friend_Apply_View)
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )

    if applyData ~= nil then
        if applyData.notFriendCount ~= nil and applyData.notFriendCount > 0 then
            if applyData.notFriendCount > 10 then
                if applyData.notFriendCount <= 99 then
                    this.main_mid.apply_configredpoint_Text.text = applyData.notFriendCount
                else
                    this.main_mid.apply_configredpoint_Text.text = "99"
                end
                this.main_mid.apply_configredpoint_Icon:ChangeIcon(1)
            else
                this.main_mid.apply_configredpoint_Text.text = applyData.notFriendCount
                this.main_mid.apply_configredpoint_Icon:ChangeIcon(0)
            end
            this.main_mid.apply_configredpoint_Icon.gameObject:SetActive(true)
        else
            this.main_mid.apply_configredpoint_Icon.gameObject:SetActive(false)
        end
    end
end

this.currSearchListData = nil
local searchListData = nil
--更新好友列表
function this.updateFriendList()
    local friendData = PlatformFriendProxy:GetInstance():getFriendListData()

    this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()

    if this.currGlobalBaseData == nil then
        return
    end
    downloadUserHead(this.currGlobalBaseData.head_url, this.main_mid.head_Icon)
    this.main_mid.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            ViewManager.open(UIViewEnum.Personal_Change_Info_View)
        end
    )
    this.main_mid.friend_Text.text = "好友(" .. #friendData .. ")"

    --更新好友申请
    this:onUpdateFriendApplyPoint()

    --更新添加好友
    this.main_mid.add_config_Icon:ChangeIcon(1)
    this.main_mid.add_config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Friend_Recommend_View)
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )
    this.main_mid.friend_CellRecycleScrollPanel:SetCellData(friendData, this.updateFriendCellList, true)
end

function this.updateFriendCellList(go, data, index)
    local item = this.main_mid.friendlistcellArr[index + 1]
    if table.empty(data.player_base_info) then
        return
    end
    item.name_Text.text = data.player_base_info.nick_name

    item.honor_Text.text = data.player_base_info.address
    if data.player_base_info.sex == 0 then
        item.sexbg_Icon:ChangeIcon(1)
    else
        item.sexbg_Icon:ChangeIcon(data.player_base_info.sex - 1)
    end

    local selfData = PlatformUserProxy:GetInstance():getUserInfo()
    if selfData == nil then
        return
    end
    local distance =
        MapManager.getDistance(data.player_base_info.lng, data.player_base_info.lat, selfData.lng, selfData.lat)
    if distance <= 1000 then
        item.distance_Text.text = string.format("%0.2f", tostring(distance)) .. "m "
    else
        item.distance_Text.text = string.format("%0.2f", tostring(distance / 1000)) .. "km "
    end

    downloadUserHead(data.player_base_info.head_url, item.head_Image)

    item.newfriend_image.name = data.player_base_info.player_id

    item.newfriend_image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            FriendChatDataProxy.currChatFriendId = data.player_base_info.player_id

            local info = PlatformFriendProxy:GetInstance():getFriendDataById(FriendChatDataProxy.currChatFriendId)

            if info == nil then
                return
            end

            PlatformFriendProxy:GetInstance():setFriendMainPageData(info)
            local data = {}
            data.isActive = true
            data.isRecommendView = false
            ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )
end
