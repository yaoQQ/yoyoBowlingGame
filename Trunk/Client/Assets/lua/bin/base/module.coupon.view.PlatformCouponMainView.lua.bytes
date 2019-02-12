require "base:enum/UIViewEnum"
require "base:mid/coupon/Mid_platform_coupon_main_panel"
-- require "base:enum/PlatformFriendType"
-- require "base:module/login/data/LoginDataProxy"
-- require "base:module/platform/data/Friend/FriendChatDataProxy"

PlatformCouponMainView = BaseView:new()
local this = PlatformCouponMainView
this.viewName = "PlatformCouponMainView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Coupon_Main_View, true)

--设置加载列表
this.loadOrders = {
    "base:coupon/platform_coupon_main_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_platform_coupon_main_panel
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
    self:initComponent()
end

function this:initComponent()
    this.curSortPanelState = this.sortPanelState.Close
    this.curSortType = this.sortTye.NewCoupon
    this.main_mid.sortDetailPanel.gameObject:SetActive(false)
end

function this:onShowHandler(msg)
    printDebug("=====================Platform_Coupon_Main_View调用完毕======================")
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    this:updateAvailCoupon()
end

function this:addNotice()
end

function this:removeNotice()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end
this.sortPanelState = {
    --关闭
    Close = 1,
    --打开
    Open = 2
}

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.close(UIViewEnum.Platform_Coupon_Main_View)
        end
    )

    self.main_mid.available_Text:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this:updateAvailCoupon()
        end
    )

    self.main_mid.used_Text:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this:updateUsedCoupon()
        end
    )

    self.main_mid.expired_Text:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this:updateExpiredCoupon()
        end
    )
    self.main_mid.sortTypeChoose:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            this.curSortPanelState =
                this.curSortPanelState == this.sortPanelState.Close and this.sortPanelState.Open or
                this.sortPanelState.Close

            this:sortPanelStateShow(this.curSortPanelState)
        end
    )
    self.main_mid.newSort_text:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            self.main_mid.chooseIcon.transform.localPosition =
                Vector3(
                self.main_mid.chooseIcon.transform.localPosition.x,
                self.main_mid.newSort_text.transform.localPosition.y,
                0
            )
            this:sortCouponByType(this.sortTye.NewCoupon)
        end
    )
    self.main_mid.expriedSort_text:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            self.main_mid.chooseIcon.transform.localPosition =
                Vector3(
                self.main_mid.chooseIcon.transform.localPosition.x,
                self.main_mid.expriedSort_text.transform.localPosition.y,
                0
            )
            this:sortCouponByType(this.sortTye.ExpiredSort)
        end
    )
end

function this:sortPanelStateShow(state)
    this.main_mid.sortDetailPanel.gameObject:SetActive(state == this.sortPanelState.Open)
    if state == this.sortPanelState.Open then
        this.main_mid.sort_image:ChangeIcon(1)
        this.main_mid.sort_text.text = "<color=#9a9a9a>" .. this.sortTypeText .. "</color>"
    else
        this.main_mid.sort_image:ChangeIcon(0)
        this.main_mid.sort_text.text = "<color=#0dace2>" .. this.sortTypeText .. "</color>"
    end
end

this.sortTye = {
    --最新排序
    NewCoupon = 1,
    --快过期排序
    ExpiredSort = 2
}
this.sortTypeText = ""

function this:sortCouponByType(_type)
    this.curSortType = _type
    if _type == this.sortTye.NewCoupon then
        this.sortTypeText = "最新"
        table.sort(
            this.currAvailCouponData,
            function(a, b)
                if a.coupon.start_time ~= b.coupon.start_time then
                    return a.coupon.start_time > b.coupon.start_time
                end
                return false
            end
        )
    else
        this.sortTypeText = "快过期"
        table.sort(
            this.currAvailCouponData,
            function(a, b)
                if a.coupon.end_time ~= b.coupon.end_time then
                    return a.coupon.end_time < b.coupon.end_time
                end
                return false
            end
        )
    end
    this:sortPanelStateShow(this.curSortPanelState)
    print("更新可使用的优惠券列表, currAvailCouponData: "..table.tostring(this.currAvailCouponData))
    self.main_mid.coupon_CellRecycleScrollPanel:SetCellData(this.currAvailCouponData, this.onUpdateCouponPanel, true)
end

this.currAvailCouponData = nil
function this:updateAvailCoupon()
    --temp
    self.main_mid.sortTypeChoose.gameObject:SetActive(false)
    --endTemp
    this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax =
        Vector2(this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax.x, -534)
    this.currAvailCouponData = PlatformCouponProxy.getAvailCouponListData()
    if this.currAvailCouponData == nil then
        return
    end

    self.main_mid.avail_Image.gameObject:SetActive(true)
    self.main_mid.used_Image.gameObject:SetActive(false)
    self.main_mid.expired_Image.gameObject:SetActive(false)
    this:sortCouponByType(this.curSortType)
end

this.currUsedCouponData = nil
function this:updateUsedCoupon()
    this.currUsedCouponData = PlatformCouponProxy.getUsedCouponListData()
    this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax =
        Vector2(this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax.x, -443)
    if this.currUsedCouponData == nil then
        return
    end
    this:sortPanelStateShow(this.sortPanelState.Close)
    self.main_mid.avail_Image.gameObject:SetActive(false)
    self.main_mid.used_Image.gameObject:SetActive(true)
    self.main_mid.expired_Image.gameObject:SetActive(false)

    self.main_mid.coupon_CellRecycleScrollPanel:SetCellData(this.currUsedCouponData, this.onUpdateCouponPanel, true)
end

this.currExpiredCouponData = nil
function this:updateExpiredCoupon()
    this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax =
        Vector2(this.main_mid.coupon_CellRecycleScrollPanel.rectTransform.offsetMax.x, -443)
    this.currExpiredCouponData = PlatformCouponProxy.getOverDueCouponListData()
    if this.currExpiredCouponData == nil then
        return
    end
    this:sortPanelStateShow(this.sortPanelState.Close)
    self.main_mid.avail_Image.gameObject:SetActive(false)
    self.main_mid.used_Image.gameObject:SetActive(false)
    self.main_mid.expired_Image.gameObject:SetActive(true)

    self.main_mid.coupon_CellRecycleScrollPanel:SetCellData(this.currExpiredCouponData, this.onUpdateCouponPanel, true)
end

function this.onUpdateCouponPanel(go, data, index)
    local item = this.main_mid.couponCellArr[index + 1]
    local isCanUse = data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable
    item.coupon_cant_use_image.gameObject:SetActive(not isCanUse)
    if data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable then
        item.expired_label_Icon.gameObject:SetActive(false)
        if data.isVisited then
            item.new_label_Image.gameObject:SetActive(false)
        else
            item.new_label_Image.gameObject:SetActive(true)
        end
    elseif data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Used then
        item.expired_label_Icon.gameObject:SetActive(true)
        item.new_label_Image.gameObject:SetActive(false)
        item.expired_label_Icon:ChangeIcon(0)
    elseif data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Overdue then
        item.expired_label_Icon.gameObject:SetActive(true)
        item.new_label_Image.gameObject:SetActive(false)
        item.expired_label_Icon:ChangeIcon(1)
    end
    item.shop_name_text.text = data.shop_list[1].name
    item.coupon_name_text.text = data.coupon.name
    item.coupon_bg_icon:ChangeIcon(data.coupon.style)
    if data.coupon.style > 0 then
        item.coupon_diy_image.gameObject:SetActive(false)
        item.coupon_bg_icon.gameObject:SetActive(true)
    else
        item.coupon_diy_image.gameObject:SetActive(true)
        downloadImage(data.coupon.icon, item.coupon_diy_image)
    end
    --print("data = "..table.tostring(data))
    downloadMerchantHead(data.shop_list[1].headurl, item.coupon_image)

    item.coupon_bg_icon:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            --print("点击了优惠券")
            PlatformCouponProxy.setSelectedCouponDataByCouponId(tostring(data.coupon_code))
            ViewManager.open(UIViewEnum.Platform_Coupon_Detail_View)
        end
    )
end
