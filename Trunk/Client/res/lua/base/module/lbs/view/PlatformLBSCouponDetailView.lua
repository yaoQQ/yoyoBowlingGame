---
--- Created by Lichongzhi.
--- DateTime: 2018\8\14 0014 14:03
---

require "base:enum/UIViewEnum"
require "base:mid/lbs/Mid_platform_lbs_coupon_detail_panel"
local UIExEventTool = CS.UIExEventTool
local PanelWidget = CS.PanelWidget

--打开红包界面
PlatformLBSCouponDetailView = BaseView:new()
local this = PlatformLBSCouponDetailView
this.viewName = "PlatformLBSCouponDetailView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LBS_Coupon_Detail_View, true)

--设置加载列表
this.loadOrders = {
    "base:lbs/platform_lbs_coupon_detail_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = Mid_platform_lbs_coupon_detail_panel
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
    self.main_mid.bag_type_text.text = "商家优惠券"
    self.main_mid.root_panel = self.main_mid.go:GetComponent(typeof(PanelWidget))
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
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("Coupon_Open_Data")
    if table.empty(data) then
        return
    end
    self:showLBSPacketView(data)
end

function this:showLBSPacketView(data)
    print("Coupon_Open_Data = " .. table.tostring(data))
    self.main_mid.back_image.gameObject:SetActive(true)
    data.packetStyle = data.packetStyle or 1
    self.main_mid.coupon_bg_icon:ChangeIcon(data.packetStyle)
    self.main_mid.shop_name_text.text = data.name
    self.main_mid.title_text.text = data.title
    self.main_mid.coupon_shop_name_text.text = data.name
    self.main_mid.coupon_name_text.text = data.couponName
    self.main_mid.intro_text.text = data.describe
    downloadMerchantHead(data.headUrl, self.main_mid.shop_head_image)
    downloadMerchantHead(data.headUrl, self.main_mid.coupon_image)

    if data.packetStyle > 0 then
        self.main_mid.coupon_diy_image.gameObject:SetActive(false)
    else
        self.main_mid.coupon_diy_image.gameObject:SetActive(true)
        downloadImage(data.iconUrl, self.main_mid.coupon_diy_image)
    end

    if data.describeImageList == nil then
        data.describeImageList = {}
    end
    self.main_mid.intro_scroll_panel.contentRT.anchoredPosition = Vector2.zero
    UIExEventTool.AdapterTextRectTransform(self.main_mid.intro_text.Txt, self.main_mid.intro_text.rectTransform)
    local bottomY =
    self.main_mid.intro_text.rectTransform.anchoredPosition.y - self.main_mid.intro_text.rectTransform.sizeDelta.y
    local groupPos = Vector2(0, bottomY - 30)
    self.main_mid.cellgroup.rectTransform.anchoredPosition = groupPos
    local max = #data.describeImageList
    for i = 1, #self.main_mid.publicityItemArr do
        local view = self.main_mid.publicityItemArr[i]
        if i <= max then
            view.go:SetActive(true)
            view.publicity_image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
            downloadResizeImage(data.describeImageList[i], view.publicity_image, ResizeType.MinFit, 286, 200,"", 1,  function (sprite)
                view.publicity_image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
                PlatformPhotoDisplayView.adapterImage(view.publicity_image, 286, 200)
            end)
        else
            view.go:SetActive(false)
        end
    end
    local cellSize = Vector2(286, 200)
    local space = Vector2(10, 10)
    local columnCount = 3
    local rowCount = math.ceil(max / columnCount)
    local groupW = cellSize.x * columnCount + space.x * (columnCount - 1)
    local groupH = cellSize.y * rowCount + space.y * (rowCount - 1)
    local childRt = self.main_mid.cellgroup.rectTransform
    local rootRt = self.main_mid.root_panel.rectTransform
    childRt.sizeDelta = Vector2(groupW, groupH)
    self.main_mid.intro_scroll_panel.scrollRect.vertical = UIExEventTool.IsDown(childRt, rootRt)

    -- 处理有且只有单张图
    self.main_mid.single_image.gameObject:SetActive(max == 1)
    self.main_mid.cellgroup.gameObject:SetActive(not (max == 1))
    local childRt2 = self.main_mid.single_image.rectTransform
    childRt2.anchoredPosition = groupPos
    local singleW = math.floor(groupW)
    local singleH = math.floor(cellSize.y * 3 + space.y * (3 - 1))
    self.main_mid.single_image.rectTransform.sizeDelta = Vector2(singleW, singleH)
    if max == 1 then
        self.main_mid.single_publicity_image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
        downloadResizeImage(data.describeImageList[1], self.main_mid.single_publicity_image, ResizeType.MinFit, singleW, singleH,"", 1,  function (sprite)
            self.main_mid.single_publicity_image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
            PlatformPhotoDisplayView.adapterImage(self.main_mid.single_publicity_image, singleW, singleH)
        end)
        self.main_mid.intro_scroll_panel.scrollRect.vertical = UIExEventTool.IsDown(childRt2, rootRt)
    end

end

function this.close()
    ViewManager.close(UIViewEnum.Platform_LBS_Coupon_Detail_View)
end
