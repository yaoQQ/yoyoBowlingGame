require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_apply_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformFriendApplyView = BaseView:new()
local this = PlatformFriendApplyView
this.viewName = "PlatformFriendApplyView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Friend_Apply_View, true)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_apply_panel"
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
    printDebug("=====================Platform_Friend_Apply_View调用完毕======================")
    this:updateFriendApplyList()
    this:addNotice()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Friend_Apply_View)
        end
    )
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateFriendApplyList)
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.updateFriendApplyList)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateFriendApplyList)
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.updateFriendApplyList)
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

this.currFriendApplyListData = nil

--更新好友申请列表
function this:updateFriendApplyList()
    this.currFriendApplyListData = PlatformFriendProxy:GetInstance():getReceiveAddFriendApplyData()
    for i = 1, #this.currFriendApplyListData do
        for j = #this.currFriendApplyListData, i + 1, -1 do
            if
                this.currFriendApplyListData[j - 1].player_base_info.player_id ==
                    this.currFriendApplyListData[j].player_base_info.player_id
             then
                table.remove(this.currFriendApplyListData, j - 1)
            end
        end
    end
    if this.currFriendApplyListData == nil or #this.currFriendApplyListData == 0 then
        this.main_mid.noapply_Text.gameObject:SetActive(true)
        this.main_mid.apply_CellRecycleScrollPanel.gameObject:SetActive(false)
        return
    end
    this.main_mid.apply_CellRecycleScrollPanel.gameObject:SetActive(true)
    this.main_mid.noapply_Text.gameObject:SetActive(false)
    this.main_mid.apply_CellRecycleScrollPanel.transform:GetChild(0).transform:GetComponent(typeof(RectTransform)).sizeDelta =
        Vector2(1080, #this.currFriendApplyListData * 185)
    this.main_mid.apply_CellRecycleScrollPanel:SetCellData(
        this.currFriendApplyListData,
        this.updateFriendCellList,
        true
    )
end

this.currOpFriendId = nil
this.currOpFriendCell = nil
function this.updateFriendCellList(go, data, index)
    local item = this.main_mid.applycellArr[index + 1]
    local selfData = PlatformUserProxy:GetInstance():getUserInfo()

    downloadUserHead(data.player_base_info.head_url, item.head_Image)

    item.name_Text.text = data.player_base_info.nick_name

    item.intro_Text.text = data.apply_info.msg

    item.go.name = index + 1

    if not data.isFriend then
        item.add_Button.gameObject:SetActive(true)
        item.added_Text.gameObject:SetActive(false)

        item.add_Button.name = data.player_base_info.player_id
        item.add_Button:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                local data = {
                    op = ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend,
                    player_id = tonumber(eventData.selectedObject.name)
                }
                this.currOpFriendId = tonumber(eventData.selectedObject.name)
                PlatformFriendModule.onReqFriendOp(data)
                item.add_Button.gameObject:SetActive(true)
                item.added_Text.gameObject:SetActive(false)
                PlatformFriendProxy:GetInstance():removeReiceiveFriendApplyData(data.player_id)
            end
        )
    else
        item.add_Button.gameObject:SetActive(false)
        item.added_Text.gameObject:SetActive(true)
    end

    item.add_Button.name = data.player_base_info.player_id
    item.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            FriendChatDataProxy.currChatFriendId = data.player_base_info.player_id
            PlatformFriendProxy:GetInstance():setFriendMainPageData(data)
            local data = {}
            data.isActive = false
            ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
        end
    )
    if data.player_base_info.sex == 0 then
        item.sexbg_Icon:ChangeIcon(1)
    else
        item.sexbg_Icon:ChangeIcon(data.player_base_info.sex - 1)
    end
end
