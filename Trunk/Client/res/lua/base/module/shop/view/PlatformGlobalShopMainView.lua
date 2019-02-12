require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_global_shop_panel"
require "base:enum/PlatformFriendType"

--商店主页界面
PlatformGlobalShopMainView = BaseView:new()
local this = PlatformGlobalShopMainView
this.viewName = "PlatformGlobalShopMainView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Global_Shop_Main_View, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_global_shop_panel"
}

--商店id
this.shopId = 0

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Global_Shop_Main_View)
        end
    )

    self.main_mid.more_Text:AddEventListener(
        UIEvent.PointerClick,
        function()
            showTopTips("功能开发中")
            --设置商家信息
            -- ViewManager.close(UIViewEnum.Platform_Global_Shop_Main_View)
            -- ViewManager.open(UIViewEnum.Platform_Shop_Detail_Info_View)
        end
    )
end

--override 打开UI回调
function this:onShowHandler(msg)
    self:resetView()

    self:addNotice()
    if msg ~= nil and msg.id ~= nil then
        this.shopId = msg.id
        --printDebug("=====================Platform_Global_Shop_Main_View  msg ======="..table.tostring(msg))
        NoticeManager.Instance:Dispatch(PlatformNoticeType.Require_Shop_Base_Info, {id = msg.id})

        --打开界面时请求获取商店活动
        PlatformModule.sendReqGetShopActivity(this.shopId, 0, 4)
        self:upGlobalShopMainData()
    end
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformNoticeType.Receive_Shop_Base_Data, this.onShowView)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformNoticeType.Receive_Shop_Base_Data, this.onShowView)
end

function this.onShowView(notcie, rsp)
    local req = rsp:GetObj()
    PlatformBusinessProxy:GetInstance():setSingleShopData(req.shop_base_info)
    this.upGlobalShopMainData()
end

this.currShopGlobalMainData = nil

--更新商家详细信息
function this.upGlobalShopMainData()
    --因为协议都有问题，所以商家主页弄个临时数据顶着
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")

    this.main_mid.name_Text.text = data.name
    downloadMerchantHead(data.headUrl, this.main_mid.head_Image)

    this.main_mid.baselabel_CellRecycleScrollPanel:SetCellData({"粵菜", "湘菜"}, this.updateShopBaseLabel, true)
    this.main_mid.label_CellRecycleScrollPanel:SetCellData({"停車場", "wifi"}, this.updateShopLabel, true)

    this.currShopGlobalMainData = PlatformBusinessProxy:GetInstance():getMainBaseData()

    if this.currShopGlobalMainData == nil then
        return
    end

    this.main_mid.name_Text.text = this.currShopGlobalMainData.name

    this.main_mid.addr_Text.text = this.currShopGlobalMainData.addr

    --红包数额
    -- self.main_mid.money_Text.text = .."元"

    this.main_mid.baselabel_CellRecycleScrollPanel:SetCellData({"粵菜", "湘菜"}, this.updateShopBaseLabel, true)
    this.main_mid.label_CellRecycleScrollPanel:SetCellData({"停車場", "wifi"}, this.updateShopLabel, true)
    if not this.currShopGlobalMainData.whowimgs then
        this.currShopGlobalMainData.whowimgs = {}
    end

    for i = 1, #this.currShopGlobalMainData.whowimgs do
        local shopImage = this.main_mid[string.concat("cellitem", i, "_shop_image")]
        shopImage.gameObject:SetActive(true)
        downloadImage(target[i], shopImage)
    end

    downloadMerchantHead(this.currShopGlobalMainData.headurl, this.main_mid.head_Image)
end

--粵菜，香菜。。
function this.updateShopBaseLabel(go, data, index)
    local item = this.main_mid.baselabelcellArr[index + 1]

    item.baselabel_Text.text = data
end

--wifi，停車場。。。
function this.updateShopLabel(go, data, index)
    local item = this.main_mid.labelcellArr[index + 1]

    item.baselabel_Text.text = data
end

--中间图片
-- function this.updateShopPic(go, data, index)
--     local item = this.main_mid.piccellArr[index + 1]
--     downloadImage(data, item.pic_Image, ImageType.Publicity)
-- end

--底部活动
function this.updateShopGames(go, data, index)
    local item = this.main_mid.gamecellArr[index + 1]

    downloadGameIcon(data.active_info.apply.game_type, item.head_Image)

    item.gameName_Text.text = TableBaseGameList.data[data.active_info.apply.game_type].name --"炫舞消除"--data.subject

    --[[local moneyNum = 0
    for i=1,#data.ative_award do
        if data.ative_award[i].loot_type == ProtoEnumCommon.LootType.LootType_Cash then
            moneyNum = moneyNum+data.ative_award[i].item_count
        end
    end

	--活动奖励
	item.reward_Text.text = math.floor(moneyNum/100).."元"--]]
    local curTime = TimeManager.getServerUnixTime()
    if data.active_info.apply.extra_time < curTime then
        item.cd_Text.text = "活动已经结束"
    else
        -- GlobalTimeManager.Instance.timerController:AddTimer(tostring(data.active_id), 30000, -1, LoginModule.sendReqHeartbeat)
        item.cd_Text.text = os.date("%Y/%m/%d %H:%M", data.active_info.apply.extra_time) .. "结束"
    end
    item.des_Text.text = data.active_info.apply.subject -- "炫舞消除"

    item.press_Image.name = data.active_id

    item.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            showTopTips("功能开发中")
            -- ViewManager.close(UIViewEnum.Platform_Global_Shop_Main_View)
            -- NoticeManager.Instance:Dispatch(PlatformNoticeType.Platform_Activity_OpenView, eventData.pointerPress.name)
        end
    )
end

--还原界面
function this:resetView()
    for i = 1, 6 do
        local shopImage = this.main_mid[string.concat("cellitem", i, "_shop_image")]
        shopImage.gameObject:SetActive(false)
    end
    this.main_mid.head_Image:SetPng(nil)

    this.main_mid.game_CellGroup:SetCellData({}, this.updateShopGames)

    this.main_mid.name_Text.text = ""

    this.main_mid.addr_Text.text = ""
end
