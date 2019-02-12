require "base:enum/UIViewEnum"
require "base:mid/coupon/Mid_platform_coupon_detail_panel"

PlatformCouponDetailView = BaseView:new()
local this = PlatformCouponDetailView
this.viewName = "PlatformCouponDetailView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Coupon_Detail_View, true)

--设置加载列表
this.loadOrders = {
    "base:coupon/platform_coupon_detail_panel"
}

this.ActiveIndex =
{
    QRCode      = 0,    -- 二维码
    CheckCardPS = 2,    -- 查看卡密
    CheckCardNO = 4,    -- 查看卡号
    UseNotice   = 6,    -- 使用须知
    ApplyShop   = 8,    -- 适用门店

}
--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_platform_coupon_detail_panel
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    printDebug("=====================Platform_Coupon_Detail_View调用完毕======================")
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    this:updateCouponDetail()
end

function this:addNotice()
    printDebug("=====================Platform_Coupon_Detail_View  addNotice================")
    NoticeManager.Instance:AddNoticeLister(Platform_UserCouponType.Platform_Rsp_Used_Coupon, this.whenConsumeCoupon)
end

function this:removeNotice()
    printDebug("=====================Platform_Coupon_Detail_View  removeNotice================")
    NoticeManager.Instance:RemoveNoticeLister(Platform_UserCouponType.Platform_Rsp_Used_Coupon, this.whenConsumeCoupon)
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.close(UIViewEnum.Platform_Coupon_Detail_View)
        end
    )
    self.main_mid.more_btn:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            showTopTips("功能开发中")
        end
    )
    self.main_mid.qr_panel:AddEventListener(UIEvent.PointerClick, function(...)
        self:activeQRPanel(false, "")
    end)

end

function this:updateCouponDetail()
    --temp
        self.main_mid.more_btn.gameObject:SetActive(false)
    --endTemp
    self:activeQRPanel(false, "")
    local data = PlatformCouponProxy.getSelectedCouponData()
    if data == nil then
        printDebug("当前所选卡券详细信息为空！")
        return
    end

    downloadMerchantHead(data.shop_list[1].headurl, self.main_mid.coupon_image)
    self.main_mid.shop_name_text.text = data.shop_list[1].name
    self.main_mid.coupon_name_text.text = data.coupon.name
    self.main_mid.coupon_bg_icon:ChangeIcon(data.coupon.style)
    if data.coupon.style > 0 then
        self.main_mid.coupon_diy_image.gameObject:SetActive(false)
    else
        self.main_mid.coupon_diy_image.gameObject:SetActive(true)
        downloadImage(data.coupon.icon, self.main_mid.coupon_diy_image)
    end
    local isCanUse = data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable
    if data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable then
        self.main_mid.use_icon:ChangeIcon(0)
        self.main_mid.user_text.text = "立即使用"
        self.main_mid.user_text.Txt.color = UIExEventTool.HexToColor("#0DACE2FF")
    elseif data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Used then
        self.main_mid.use_icon:ChangeIcon(1)
        self.main_mid.user_text.text = "已使用"
        self.main_mid.user_text.Txt.color = UIExEventTool.HexToColor("#9A9A9AFF")
    else
        self.main_mid.use_icon:ChangeIcon(1)
        self.main_mid.user_text.text = "已过期"
        self.main_mid.user_text.Txt.color = UIExEventTool.HexToColor("#9A9A9AFF")
    end
    self.main_mid.use_icon:AddEventListener(UIEvent.PointerClick,function(...)
        if isCanUse == false then
            return
        end
        self:activeQRPanel(true, data.coupon_code)

    end)
    for i = 1, #self.main_mid.actionItemArr do
        local item = self.main_mid.actionItemArr[i]
        if data.state == ProtoEnumCommon.UserCouponState.UserCouponState_Usable then
            item.action_text.Txt.color = UIExEventTool.HexToColor("#0DACE2FF")
        else
            item.action_text.Txt.color = UIExEventTool.HexToColor("#424242FF")
        end
        if i == 1 then
            if isCanUse then
                item.action_icon:ChangeIcon(this.ActiveIndex.QRCode)
            else
                item.action_icon:ChangeIcon(this.ActiveIndex.QRCode + 1)
            end

            item.action_text.text = "二维码"
            item.action_icon:AddEventListener(UIEvent.PointerClick,function(...)
                if isCanUse == false then
                    return
                end
                self:activeQRPanel(true, data.coupon_code)
            end)
        elseif i == 2 then
            if isCanUse then
                item.action_icon:ChangeIcon(this.ActiveIndex.UseNotice)
            else
                item.action_icon:ChangeIcon(this.ActiveIndex.UseNotice + 1)
            end
            item.action_text.text = "使用须知"
            item.action_icon:AddEventListener(UIEvent.PointerClick,function(...)
                if isCanUse == false then
                    return
                end
                ViewManager.open(UIViewEnum.Platform_Coupon_User_Know_View)
            end)
        elseif i == 3 then
            if isCanUse then
                item.action_icon:ChangeIcon(this.ActiveIndex.ApplyShop)
            else
                item.action_icon:ChangeIcon(this.ActiveIndex.ApplyShop + 1)
            end
            item.action_text.text = "适用门店"
            item.action_icon:AddEventListener(UIEvent.PointerClick,function(...)
                if isCanUse == false then
                    return
                end
                showTopTips("功能开发中")
                --ViewManager.open( UIViewEnum.Platform_Coupon_Shop_List_View, {coupon_ids = data.coupon.shop_id})
            end)
        end

    end
    self.main_mid.coupon_date_text.text =
    "有效时间 <color=#9a9a9a>" ..
            os.date("%Y/%m/%d %H:%M", data.coupon.start_time) ..
            " 至 " .. os.date("%Y/%m/%d %H:%M", data.coupon.end_time) .. "</color>"
    
    self.main_mid.intro_info_text.text = data.coupon.detail

end

function this:activeQRPanel(state, code)
    self.main_mid.qr_panel.gameObject:SetActive(state)
    self.main_mid.qr_bg.gameObject:SetActive(state)
    if state then
        self.main_mid.qr_coupon_code_text.text = code
        local qrIndex = "yoyo&verify_coupon&"..code
        local m_Texture2D = UtilMethod.GetQrCode(qrIndex)
        ImageUtil.setTexture2DImage(m_Texture2D, self.main_mid.qr_image.Img)
    end
end

--核销时调用
function this.whenConsumeCoupon()
    printDebug("核销优惠券  star !! ")
    self:activeQRPanel(false, "")
    PlatformCouponProxy.whenCouponUse()
    this:updateCouponDetail()
end
