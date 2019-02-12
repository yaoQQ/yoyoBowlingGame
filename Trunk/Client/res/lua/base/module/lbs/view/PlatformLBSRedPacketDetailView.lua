---
--- Created by Lichongzhi.
--- DateTime: 2018\8\14 0014 14:03
---
require "base:enum/UIViewEnum"
require "base:mid/lbs/Mid_platform_lbs_redpacket_detail_panel"

local UIExEventTool = CS.UIExEventTool
local PanelWidget = CS.PanelWidget

--打开红包界面
PlatformLBSRedPacketDetailView = BaseView:new()
local this = PlatformLBSRedPacketDetailView
this.viewName = "PlatformLBSRedPacketDetailView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LBS_RedPacket_Detail_View, true)

--设置加载列表
this.loadOrders = {
    "base:lbs/platform_lbs_redpacket_detail_panel"
}

this.clockCount = 0
this.clockCountList = {3, 5, 7, 9} -- 样式对应的倒计时数目
--this.clockCountList = {2, 2, 2, 2} -- test

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = Mid_platform_lbs_redpacket_detail_panel
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
    self.main_mid.bag_type_text.text = "商家红包"
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
            -- local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")
            -- ViewManager.open(UIViewEnum.Platform_Global_Shop_Main_View, {id = data.shop_id}) --todo改成商家id
        end
    )
end

--override 打开UI回调
function this:onShowHandler()
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")
    if table.empty(data) then
        return
    end
    self:showLBSPacketView(data)
    self:showReturnTimer(data)
end

--override 关闭UI回调
function this:onClose()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PacketDetailReturnClock")
end

function this:showLBSPacketView(data)
    print("RedPacket_Open_Data = " .. table.tostring(data))
    self.main_mid.remain_time_text.gameObject:SetActive(false)
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

    self.main_mid.bag_title_text.text = data.title
    self.main_mid.intro_text.text = data.describe
    if data.getState == PacketGetState.FirstGot or data.getState == PacketGetState.HasGot then
        self.main_mid.get_toggle.IsOn = true
        self.main_mid.money_text.text = tostring(data.rsp.money / 100) .. "元"
    elseif data.getState == PacketGetState.Empty then
        self.main_mid.get_toggle.IsOn = false
        self.main_mid.money_text.text = tostring("红包被抢光了")
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
    --self.main_mid.intro_scroll_panel.contentRT.sizeDelta = Vector2(groupW, groupH + cellSize.y)
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

function this:showReturnTimer(data)
    if data.getState == PacketGetState.FirstGot then
        self.main_mid.remain_time_text.gameObject:SetActive(true)
        self.main_mid.back_image.gameObject:SetActive(false)
        self.clockCount = this.clockCountList[data.packetStyle]
        if self.clockCount == nil then
            printError("错误, 未知的红包样式, style = " .. data.packetStyle)
            self.main_mid.remain_time_text.gameObject:SetActive(false)
            self.main_mid.back_image.gameObject:SetActive(true)
            return
        end
        self.main_mid.remain_time_text.text = tostring(self.clockCount) .. "s"
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PacketDetailReturnClock")
        GlobalTimeManager.Instance.timerController:AddTimer(
            "PacketDetailReturnClock",
            1000,
            -1,
            function()
                self.clockCount = self.clockCount - 1
                self.main_mid.remain_time_text.text = tostring(self.clockCount) .. "s"
                if self.clockCount == 0 then
                    self.main_mid.remain_time_text.gameObject:SetActive(false)
                    self.main_mid.back_image.gameObject:SetActive(true)
                end
            end
        )
    else
        self.main_mid.remain_time_text.gameObject:SetActive(false)
        self.main_mid.back_image.gameObject:SetActive(true)
    end
end

function this.close()
    ViewManager.close(UIViewEnum.Platform_LBS_RedPacket_Detail_View)
end

-- 检测rt的下边缘是否超出root的下边缘
function this.mayDown(rt, root)
    local bounds1 = UIExEventTool.GetBounds(rt, root)
    local rootBounds = UIExEventTool.GetBounds(root, root)
    if bounds1.min.y < rootBounds.min.y then
        return true
    else
        return false
    end
end

--override
--返回键关闭界面
function this:closeByEsc()
	if self.main_mid.back_image.gameObject.activeInHierarchy then
		this.close()
	end
end