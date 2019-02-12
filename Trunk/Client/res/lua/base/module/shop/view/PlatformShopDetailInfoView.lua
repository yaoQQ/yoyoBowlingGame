require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_shop_detailinfo_panel"
require "base:enum/PlatformFriendType"

PlatformShopDetailInfoView = BaseView:new()
local this = PlatformShopDetailInfoView
this.viewName = "PlatformShopDetailInfoView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Shop_Detail_Info_View, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_shop_detailinfo_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    --  printDebug("=====================Platform_Shop_Detail_Info_View调用完毕======================")
    this:upShopDetailInfo()
end

function this:addEvent()
    self.main_mid.closemask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Detail_Info_View)
        end
    )

    self.main_mid.close_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Shop_Detail_Info_View)
        end
    )
end

this.currShopDetailInfoData = nil

--更新商家详细信息
function this:upShopDetailInfo()
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")

    this.main_mid.name_Text.text = data.name
    downloadMerchantHead(data.headUrl, this.main_mid.head_CircleImage)

    this.currShopDetailInfoData = PlatformBusinessProxy:GetInstance():getMainBaseData()

    if this.currShopDetailInfoData == nil then
        return
    end

    downloadMerchantHead(this.currShopDetailInfoData.headurl, self.main_mid.head_CircleImage)

    self.main_mid.name_Text.text = this.currShopDetailInfoData.name

    self.main_mid.intro_Text.text = this.currShopDetailInfoData.addr

    --printDebug("=======================更新商家详细信息  shop detail")
    -- self.main_mid.apply_CellRecycleScrollPanel:SetCellData(this.currShopDetailInfoData,this.updateFriendCellList,true)
end

-- function this.updateFriendCellList(go,data,index)

-- 	local item =  this.main_mid.applycellArr[index+1]

-- 	-- downloadFromPicServer(data.head_url,item.head_Image)

-- 	item.name_Text.text = data.name

-- 	item.intro_Text.text = data.msg

-- 	if not data.isFriend then
-- 		item.add_Button.gameObject:SetActive(true)
-- 		item.added_Text.gameObject:SetActive(false)

-- 		item.add_Button.name = data.player_id
-- 		item.add_Button:AddEventListener(UIEvent.PointerClick,function (eventData)
-- 			printDebug("向服务器发送添加好友申请")
-- 			local data = {
-- 				op = ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend,
-- 				player_id = tonumber(eventData.selectedObject.name)
-- 				-- strparam = this.main_mid.sendmsg_InputField.text,
-- 			}
-- 			PlatformFriendModule.onReqFriendOp(data)
-- 		end)
-- 	else
-- 		item.add_Button.gameObject:SetActive(false)
-- 		item.added_Text.gameObject:SetActive(true)
-- 	end

-- 	item.add_Button.name = data.player_id

-- 	item.press_Image:AddEventListener(UIEvent.PointerClick,function (eventData)

-- 		printDebug("点击了")
-- 		FriendChatDataProxy.currChatFriendId = tonumber(eventData.pointerPress.name)
-- 		printDebug("========================点击了好友，好友id为："..FriendChatDataProxy.currChatFriendId)

-- 		PlatformFriendProxy:GetInstance():setFriendMainPageData(this.currFriendSearchData)

-- 		local data = {}
-- 		data.isActive = false
-- 		ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View,data)
-- 	end)

-- 	item.level_Text.text = "LV "..data.level

-- 	-- item.sex_Image.Img.color = data.sex == 1 and CSColor.blue or CSColor.red

-- 	item.sexbg_Icon:ChangeIcon(data.sex - 1)
-- 	item.sex_Icon:ChangeIcon(data.sex - 1)

-- end
