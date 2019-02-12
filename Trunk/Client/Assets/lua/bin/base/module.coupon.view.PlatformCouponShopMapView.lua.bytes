require "base:enum/UIViewEnum"
require "base:mid/coupon/Mid_platform_coupon_shop_map_panel"
-- require "base:enum/PlatformFriendType"
-- require "base:module/login/data/LoginDataProxy"
-- require "base:module/platform/data/Friend/FriendChatDataProxy"

PlatformCouponShopMapView = BaseView:new()
local this = PlatformCouponShopMapView
this.viewName = "PlatformCouponShopMapView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Coupon_Shop_Map_View, true)

--设置加载列表
this.loadOrders = {
    "base:coupon/platform_coupon_shop_map_panel",
    "base:global/platform_global_shop_float_panel" --商家浮标
}

this.businessGoList = {} --商户浮标
this.busiData = nil --单个商户数据table

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    local switch = {
        [self.loadOrders[1]] = function()
            self.main_mid = {}
            self:BindMonoTable(gameObject, self.main_mid)
            UITools.SetParentAndAlign(gameObject, self.container)
        end,
        [self.loadOrders[2]] = function()
            self.shopSingleObj = gameObject
            UITools.SetParentAndAlign(gameObject, self.container)
            self.shopSingleObj.transform.localPosition = Vector3(10000, 10000, 0)
        end
    }
    local fSwitch = switch[uiName]

    if fSwitch then
        fSwitch()
    else --key not found
        printDebug(uiName .. " not found !")
    end
    self:addEvent()
end

function this:onShowHandler(msg)
    printDebug("=====================Platform_Coupon_Shop_Map_View调用完毕======================")
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()

    this:updateCouponShopMap()
end

--override 关闭UI回调
function this:onClose()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("ShopMapView")
end

function this:addEvent()
    printDebug("shop map  记得测试 实时位置信息~")
    --GlobalTimeManager.Instance.timerController:AddTimer("ShopMapView", -1, -1, function()
    --	this:updateIconPos(false)
    --end)

    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.close(UIViewEnum.Platform_Coupon_Shop_Map_View)
            ViewManager.close(UIViewEnum.MapView)
        end
    )
end

function this:updateIconPos(needSetAtd)
    local data = PlatformCouponProxy.getCouponUseShopsData()

    if data == nil then
        return
    end

    --单商户浮标位置偏移
    --理论上只会同时有一个商店
    for i = 1, #self.shopGoList do
        if data.pos ~= nil then
            data.pos:updateLnglat(data.lng, data.lat)
        else
            data.pos = MapPos:new(data.lng, data.lat)
        end

        local loc1, loc2 = self.busiData[i].pos:getScreenPosFromCenter()
        self.shopGoList[i].go.transform.localPosition = Vector3(loc1, loc2, 0)
        self.shopGoList[i].go.transform.localScale = data.pos:getScale()
        if needSetAtd then
            self.shopGoList[i].go.name = tostring(loc2)
        end
    end
end

this.currCouponShopMapData = nil
function this:updateCouponShopMap()
    this.currCouponShopMapData = PlatformCouponProxy.getCouponUseShopsData()

    if this.currCouponShopMapData == nil then
        return
    end

    self.main_mid.shop_name_Text.text = this.currCouponShopMapData.name

    self.main_mid.shop_add_Text.text = this.currCouponShopMapData.address

    --还没有电话字段
    -- self.main_mid.shop_tel_Text.text =

    self.main_mid.navi_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            self.main_mid.navi_panel.gameObject:SetActive(true)
        end
    )
    self.main_mid.navi_back:AddEventListener(
        UIEvent.PointerClick,
        function()
            self.main_mid.navi_panel.gameObject:SetActive(false)
        end
    )
    self.main_mid.self_map_btn:AddEventListener(
        UIEvent.PointerClick,
        function()
            showFloatTips("功能还在开发中，敬请期待！")
        end
    )
    self.main_mid.navi_panel:AddEventListener(
        UIEvent.PointerClick,
        function()
            self.main_mid.navi_panel.gameObject:SetActive(false)
        end
    )

    self.main_mid.gade_map_btn:AddEventListener(
        UIEvent.PointerClick,
        function()
            showFloatTips("功能还在开发中，敬请期待！")
        end
    )
    self.main_mid.navi_panel.gameObject:SetActive(false)

    --打开地图
    --商户 浮标
    if MapView.IsOpen then
        MapManager.changeCurLngLat(this.currCouponShopMapData.lng, this.currCouponShopMapData.lat)
    else
        MapManager.openMapViewByLngLat(this.currCouponShopMapData.lng, this.currCouponShopMapData.lat)
    end

    this.onLoadShopSingle()
end

function this.onLoadShopSingle()
    require "base:mid/global/Mid_platform_global_shop_float_panel"
    require "base:module/Shop/data/PlatformCouponProxy"
    if self.busiSingleObj == nil then
        return
    end

    local data = PlatformCouponProxy.getCouponUseShopsData()

    if data == nil then
        return
    end

    for i = 1, #self.shopGoList do
        if self.shopGoList[i] ~= nil then
            self.shopGoList[i].go:SetActive(false)
        end
    end

    local busiGo = nil
    local reload = false

    --如果列表里面有该物体，则循环使用，无则new一个
    if self.businessGoList[i] ~= nil then
        busiGo = self.businessGoList[i]
    else
        reload = true
        busiGo = Mid_platform_global_shop_float_panel:new(GameObject.Instantiate(self.busiSingleObj))
    end

    busiGo.go:SetActive(true)

    --商家位置
    busiGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)

    --商家位置 -- MsgShopAttribute
    data.pos = MapPos:new(data.lng, data.lat)
    local loc1, loc2 = data.pos:getScreenPosFromCenter()
    busiGo.go.transform.localPosition = Vector3(loc1, loc2, 0)
    busiGo.go.transform.localScale = Vector3.one
    busiGo.go.name = tostring(loc2)

    --新生成的物体名字以data的名字命名只是暂时的，以后会用商户的独立Id命名
    busiGo.shop_Icon.gameObject.name = data.shop_id

    --按钮事件注册

    if reload then
        table.insert(self.businessGoList, busiGo)
    end
    self.busiData = data
end
