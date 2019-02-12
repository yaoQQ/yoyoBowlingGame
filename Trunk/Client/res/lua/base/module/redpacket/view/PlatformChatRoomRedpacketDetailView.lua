require "base:enum/UIViewEnum"

local UIExEventTool = CS.UIExEventTool
local PanelWidget = CS.PanelWidget

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false
local isCoupon = false

--打开红包界面
PlatformChatRoomRedpacketDetailView = BaseView:new()
local this = PlatformChatRoomRedpacketDetailView
this.viewName = "PlatformChatRoomRedpacketDetailView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View, false)

--设置加载列表
this.loadOrders = {
    "base:redpacket/platform_chat_room_redpacket_detail_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
    self.main_mid.root_panel = self.main_mid.go:GetComponent(typeof(PanelWidget))
end

function this:checkCanScroll()
    if this.mayDown(self.main_mid.cellgroup.rectTransform, self.main_mid.root_panel.rectTransform) then
        self.main_mid.intro_scroll_panel.scrollRect.vertical = true
    else
        self.main_mid.intro_scroll_panel.scrollRect.vertical = false
    end
end

function this:addEvent()
    self.main_mid.back_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.close()
        end
    )

    self.main_mid.shop_main_page_btn:AddEventListener(
        UIEvent.PointerClick,
        function()
            showFloatTips("功能开发中敬请期待！")
        end
    )
end

--override 打开UI回调
function this:onShowHandler()
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("ChatRoom_RedPacket_Open_Data")
    if table.empty(data) then
        return
    end
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    activeId = data.activeId
    redpacketId = data.redpacketId
    redpacketType = data.redpacketType
    isCoupon = data.isCoupon
    self.main_mid.bag_type_text.text = isCoupon and "商家券包" or "商家红包"
    if not data.isCoupon then
        PlatformRedpacketModule.sendReqGetActiveCashRedPacketRcvHistory(redpacketType, activeId, redpacketId)
    end
    self:showLBSPacketView(data)
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformNoticeType.Receive_RedBag_List, this.hanlerRedPackeHistroy)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformNoticeType.Receive_RedBag_List, this.hanlerRedPackeHistroy)
end

function this.hanlerRedPackeHistroy(key, result)
    local rsp = result:GetObj()
    this.main_mid.money_text.text = tostring(rsp.avg_moneys / 100) .. "元" --用于规避后端领过传0的bug
    this.main_mid.bag_num_text.text =
        string.concat("还有", rsp.left_num, "/", rsp.total_num, "，每个", rsp.avg_moneys / 100, "元")
    this.main_mid.bag_list_scroll:SetCellData(
        rsp.rcv_user_list,
        function(go, data, index)
            this.updateRedbegList(go, data, index)
        end,
        true
    )
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:showLBSPacketView(data)
    self.main_mid.back_image.gameObject:SetActive(true)
    downloadMerchantHead(data.headUrl, self.main_mid.shop_head_image)
    self.main_mid.shop_name_text.text = data.name
    local x = 0
    local y = self.main_mid.shop_name_text.rectTransform.anchoredPosition.y
    self.main_mid.official_image.gameObject:SetActive(data.is_official)
    if data.is_official then
        x = x - 50
    end
    local namePos = Vector2(x, y)
    self.main_mid.shop_name_text.rectTransform.anchoredPosition = namePos

    self.main_mid.bag_num_text.text = ""
    self.main_mid.money_text.text = ""
    if data.getState == PacketGetState.FirstGot then
        self.main_mid.get_toggle.IsOn = true
    elseif data.getState == PacketGetState.HasGot then
        self.main_mid.get_toggle.IsOn = true
    elseif data.getState == PacketGetState.Empty then
        self.main_mid.get_toggle.IsOn = false
        self.main_mid.money_text.text = tostring("红包被抢光了")
    end
    local targetList = data.isCoupon and data.rsp.couponrcv_list or {}
    if data.isCoupon then
        local coupondata = data.rsp.coupon_red_packet
        this.main_mid.money_text.text = coupondata.coupon_name
        this.main_mid.bag_num_text.text = string.concat("还有", coupondata.remain_count, "/", coupondata.coupon_count)
    end
    this.main_mid.bag_list_scroll:SetCellData(
        targetList,
        function(go, data, index)
            this.updateRedbegList(go, data, index)
        end,
        true
    )
end
function this.updateRedbegList(go, data, index)
    local item = nil
    item = this.main_mid.redbaglistcellArr[index + 1]
    item.is_self_image.gameObject:SetActive(data.player_id == LoginDataProxy.playerId)

    local time = isCoupon and data.rcv_time or data.time
    local headUrl = isCoupon and data.player_header_url or data.head_url
    local name = isCoupon and data.player_nickname or data.nick_name

    item.time_text.text = os.date("%H:%M", time)
    item.money_text.text = isCoupon and "优惠券x1" or string.concat(data.num / 100, "元")
    downloadUserHead(headUrl, item.head_image)
    item.name_text.text = name
    if data.player_id == LoginDataProxy.playerId then
        item.name_text.text = string.concat("<color=#815b37>", item.name_text.text, "</color>")
        item.money_text.text = string.concat("<color=#815b37>", item.money_text.text, "</color>")
        item.time_text.text = string.concat("<color=#815b37>", item.time_text.text, "</color>")
    end
end
function this.close()
    ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
end
