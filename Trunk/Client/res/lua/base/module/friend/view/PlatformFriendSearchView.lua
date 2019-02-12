require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_search_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformFriendSearchView = BaseView:new()
local this = PlatformFriendSearchView
this.viewName = "PlatformFriendSearchView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Friend_Search_View, true)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_search_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end
local color = nil
function this:onShowHandler(msg)
    this.updateInstanceSearchView()
    this:updateFriendSearchList()
    this:addNotice()
end

function this:addNotice()
end

function this:removeNotice()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addEvent()
    self.main_mid.cancel_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Friend_Search_View)
        end
    )

    self.main_mid.clear_Image:AddEventListener(UIEvent.PointerClick, self.clearInputTxt)
end

local inputTxt = nil

function this.updateInstanceSearchView()
    this.main_mid.search_InputField.inputField:ActivateInputField()
    this.main_mid.search_InputField:OnValueChanged(
        function(obj)
            inputTxt = this.main_mid.search_InputField.text
            PlatformFriendProxy:GetInstance():locateFriend(inputTxt)
            this:updateFriendSearchList()
        end
    )
end

this.currSearchListData = nil
local searchListData = nil
local applyData = nil

--更新搜索列表
function this:updateFriendSearchList()
    searchListData = PlatformFriendProxy:GetInstance():getLocateFriendData()
    this.currSearchListData = {}

    if searchListData ~= nil then
        for i = 1, #searchListData do
            table.insert(this.currSearchListData, searchListData[i])
        end
        if inputTxt ~= nil then
            if inputTxt == "" then
                this.main_mid.search_Text.text = ""
            else
                this.main_mid.search_Text.text =
                    "<color=#424242FF>“" .. inputTxt .. "”</color>共" .. #searchListData .. "个好友"
            end
        end
    end
    searchListData = nil
    this.main_mid.search_CellRecycleScrollPanel:SetCellData(
        this.currSearchListData,
        this.updateFriendSearchCellList,
        true
    )
end

function this.updateFriendSearchCellList(go, data, index)
    local item = this.main_mid.searchlistcellArr[index + 1]

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

function this:clearInputTxt()
    if this.main_mid ~= nil then
        this.main_mid.search_InputField.text = ""
    end
end
