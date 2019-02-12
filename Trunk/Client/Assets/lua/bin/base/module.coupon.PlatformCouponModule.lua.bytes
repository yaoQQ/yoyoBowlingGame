require "base:enum/NoticeType"
require "base:enum/Platform_UserCouponType"
require "base:module/coupon/data/PlatformCouponProxy"


PlatformCouponModule = BaseModule:new()
local this = PlatformCouponModule

this.moduleName="PlatformCoupon"


--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxRspGetPlayerCouponList,this.rspPlayerCouponList)
	--this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvCoupon,this.rspPlayerGetNewCoupon)
	this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxRspDelCoupon,this.rspPlayerDeleteCoupon)
	this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxNotifyConsumeCoupon,this.GetNotifyConsumeCoupon)
	this:AddNetLister(ProtoEnumShop.MsgIdx.MsgIdxRspGetShopListInfo,this.rspGetShopListInfo)
	--this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspReceiveActiveCouponRedpacket,this.RspReceiveActiveCouponRedpacket)


end
function this.onNetMsgLister(protoID,protoBytes)
	local nfSwitch = this.netFuncList[protoID] 
	if nfSwitch then  
		nfSwitch(protoBytes) 
	else 
		this:withoutRegistNotice(protoID)
	end
end
--==================================================消息(模块交互)==================================================

function this:getRegisterNotificationList()
	if self.notificationList == nil then
		self.notificationList = {} 
		self.switch={}
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Go_CouponMain_Page,this.goCouponmainPage) 
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Req_CouponList,this.reqCouponList)   
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Fresh_CouponMain_Page,this.freshCouponmainPage)  
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Req_GetNew_Coupon,this.reqGetNewCoupon)
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Req_Del_Coupon,this.reqDelNewCoupon)
		self:AddNotifictionLister(Platform_UserCouponType.Platform_Req_Coupon_shoplist,this.reqShopList)
	end	
	 return self.notificationList
end



--==================================================响应事件==================================================
--======req响应=========
function this.rspPlayerCouponList(protoBytes)
	local rsp = this.decodeProtoBytes("coupon", "RspGetPlayerCouponList", protoBytes)
	--设置数据 
	PlatformCouponProxy.setAllCouponData(rsp)
	if PlatformCouponMainView.isOpen  then
		PlatformCouponMainView:updateAvailCoupon()
	else
		ViewManager.open(UIViewEnum.Platform_Coupon_Main_View)
	end
end

function this.rspPlayerGetNewCoupon(protoBytes)
	-- printDebug("=========================================================请求玩家获取新的卡券返回================================================")

	
end



function this.rspPlayerDeleteCoupon(protoBytes)
	printDebug("=========================================================请求玩家删除卡券返回================================================")
	local rsp = this.decodeProtoBytes("proto", "RspDelCoupon", protoBytes)
end

function this.GetNotifyConsumeCoupon(protoBytes)
	printDebug("=========================================================玩家卡券核销状态返回================================================")
	local rsp = this.decodeProtoBytes("proto", "NotifyConsumeCoupon", protoBytes)
	
	if PlatformCouponDetailView.isOpen then
		NoticeManager.Instance:Dispatch(Platform_UserCouponType.Platform_Rsp_Used_Coupon)

	end
end

function  this.rspGetShopListInfo (protoBytes)
	printDebug("=========================================================玩家卡券核可使用门店返回================================================")
	local rsp = this.decodeProtoBytes("shop","RspGetShopListInfo", protoBytes)
	PlatformCouponProxy.setCouponShopList(rsp)
	if PlatformCouponShopListView.isOpen then
		NoticeManager.Instance:Dispatch(Platform_UserCouponType.Platform_Rsp_Coupon_shoplist)
	end
end

function this.RspReceiveActiveCouponRedpacket(protoBytes)
	local rsp = this.decodeProtoBytes("platform","RspReceiveActiveCouponRedpacket", protoBytes)
	PlatformBusinessProxy:GetInstance():setOpenRedBagCouponInfo(rsp)
	PlatformBusinessProxy.isCoupon = true
	ViewManager.close(UIViewEnum.Platform_LBS_Coupon_Open_View)
end

--======notication响应=========
function this:goCouponmainPage(notice)
	printDebug("点击准备打开页面：")
	NoticeManager.Instance:Dispatch(Platform_UserCouponType.Platform_Req_CouponList,notice)
end

function this:freshCouponmainPage(notice)
	
end

function this:reqCouponList(notice)
	local data = notice:GetObj()
	printDebug("发送给服务器 卡券列表请求：")
	this.sendNetMsg(GameConfig.ServerName.MainGateway,"coupon","ReqGetPlayerCouponList",data)
end

function this:reqGetNewCoupon(notice)
	local data = notice:GetObj()
	
	printDebug("发送给服务器 领取卡券请求：")
	this.sendNetMsg(GameConfig.ServerName.MainGateway,"coupon","ReqRcvCoupon",data)
end

function this:reqDelNewCoupon(notice)
	local data = notice:GetObj()
	printDebug("发送给服务器 删除卡券请求：")
	this.sendNetMsg(GameConfig.ServerName.MainGateway,"coupon","ReqDelCoupon",data)
end

function this:reqShopList(notice)
	printDebug("发送给服务器   卡券使用门店："..tostring(notice))
	local data = notice:GetObj()
	printDebug("发送给服务器   卡券使用门店：")
	this.sendNetMsg(GameConfig.ServerName.MainGateway,"shop","ReqGetShopListInfo",data)
end
