

require "base:enum/UIViewEnum"
require "base:mid/coupon/Mid_platform_coupon_shop_list_panel"
-- require "base:enum/PlatformFriendType"
-- require "base:module/login/data/LoginDataProxy"
-- require "base:module/platform/data/Friend/FriendChatDataProxy"

PlatformCouponShopListView = BaseView:new()
local this = PlatformCouponShopListView
this.viewName = "PlatformCouponShopListView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Coupon_Shop_List_View, true)

--设置加载列表
this.loadOrders=
{
	"base:coupon/platform_coupon_shop_list_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:onShowHandler(msg)
	printDebug("=====================Platform_Coupon_Shop_List_View调用完毕======================")
	local go = self:getViewGO()
	go.transform:SetAsLastSibling()
	if msg ~=nil and msg.coupon_ids ~= nil  then
		printDebug("=====================Platform_Coupon_Shop_List_View   ===="..table.tostring(msg))
		NoticeManager.Instance:Dispatch(Platform_UserCouponType.Platform_Req_Coupon_shoplist,{shop_id =  msg.coupon_ids})
	end
	self:addNotice()
end

function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
		ViewManager.close(UIViewEnum.Platform_Coupon_Shop_List_View)
	end)
end

function this:addNotice()
	printDebug("=====================Platform_Coupon_Detail_View  addNotice================")
	NoticeManager.Instance:AddNoticeLister(Platform_UserCouponType.Platform_Rsp_Coupon_shoplist, this.updateCouponShopList)
end

function this:removeNotice()
	printDebug("=====================Platform_Coupon_Detail_View  removeNotice================")
	NoticeManager.Instance:RemoveNoticeLister(Platform_UserCouponType.Platform_Rsp_Coupon_shoplist, this.updateCouponShopList)
end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()
end

--[[function this.rspShopList ()
	printDebug("===================== listen to  rspShopList   ================")
	this.updateCouponShopList()
end
--]]
this.currCouponShopListData = nil
function this.updateCouponShopList()
	-- body
	printDebug("===================== listen to  rspShopList   ================"..table.tostring(PlatformCouponProxy.getCouponShopList())  )
	this.currCouponShopListData = PlatformCouponProxy.getCouponShopList().shop_info
	
	if this.currCouponShopListData == nil then return end

	this.main_mid.how_many_Text.text = "共"..#this.currCouponShopListData.."家门店"

	this.main_mid.shop_CellRecycleScrollPanel:SetCellData(this.currCouponShopListData,this.onUpdateCouponShopList,true)
end

function this.onUpdateCouponShopList(go,data,index)

	local item = this.main_mid.shopCellArr[index+1]

	item.shop_name_Text.text = data.name

	item.shop_add_Text.text = data.address

	item.distance_Text.text =string.format("%.2f",MapManager.getDistance(data.lng,data.lat,MapManager.userLng,MapManager.userLat)/1000).."km"

	item.press_Image.name = data.shop_id

	item.press_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
		local obj = eventData.pointerPress

		if obj == nil then
			printDebug("点击的商家为空！")
		end

		MapManager.openGaodeMapApp(MapManager.userLng,MapManager.userLat,"我的位置",data.lng,data.lat,data.name)
		--PlatformCouponProxy.setCouponUseShopsDataById(tonumber(eventData.press_Image.name))

		--ViewManager.open(UIViewEnum.Platform_Coupon_Shop_Map_View)
	end)
	
	
end