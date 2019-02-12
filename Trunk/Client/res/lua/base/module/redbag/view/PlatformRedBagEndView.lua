require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_end_panel"
require "base:module/redbag/data/PlatformRedBagProxy"

PlatformRedBagEndView = BaseView:new()
local this = PlatformRedBagEndView
this.viewName = "PlatformRedBagEndView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_RedBag_End_View, false)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_redbag_end_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
	
	--iOS不分享
	if IS_IOS then
		self.main_mid.dis_Text.gameObject:SetActive(false)
		self.main_mid.right_Button.gameObject:SetActive(false)
	end
end
local m_type = 0
function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    this:updataEndPanel()
    this:onRedPacketCountHandler()
    m_type = msg
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onRedPacketCountHandler)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onRedPacketCountHandler)
end

local m_endData = {}
function this:addEvent()
    this.main_mid.left_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_End_View)
        end
    )
    this.main_mid.mask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_End_View)
        end
    )
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_RedBag_Share_View,m_type)
            ViewManager.close(UIViewEnum.Platform_RedBag_End_View)
            NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)   
        end
    )
end

function this:updataEndPanel()
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
    if data.isFriend  then 
        local friendData = PlatformRedBagProxy:GetInstance():getOpenRedBagData("Open_Friend_Data")
        this.main_mid.title_Text.text = "恭喜你成功抢到<color=#F43232FF>"..friendData.name.."</color>的红包:"
    else
        this.main_mid.title_Text.text = "恭喜你获得<color=#F43232FF>在线红包</color>奖励:"
    end
    m_endData = data.reward_list
    printDebug("getRedBagEndData()"..table.tostring(m_endData))
    this.main_mid.dis_Text.text = "每天前3次炫耀，可获翻倍奖励哦!"
    this.main_mid.redbag_CellGroup:SetCellData(m_endData,this.updataEndRedBagCellList)
end

function this.updataEndRedBagCellList(go,data,index)
    local item =  this.main_mid.redbagCellArr[index+1]
    printDebug("++++++++++++++更新奖励ing"..table.tostring(data))
    if data.item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
        item.left_gold_Icon:ChangeIcon(0)
        item.left_gold_Text.text =((tonumber(data.item_count))/100).."元"
    elseif data.item_type == ProtoEnumCommon.ItemType.ItemType_Diamond then
        item.left_gold_Icon:ChangeIcon(2)
        item.left_gold_Text.text ="x"..tonumber(data.item_count)
    elseif data.item_type == ProtoEnumCommon.ItemType.ItemType_Money then
        item.left_gold_Icon:ChangeIcon(1)
        item.left_gold_Text.text ="x"..tonumber(data.item_count)
    end

    
end

function this.onRedPacketCountHandler()
    NoticeManager.Instance:Dispatch(NoticeType.Mall_Update_Money)
    local openRedPacketCount = PlatformUserProxy:GetInstance():getOnlineRedPacketDailyCount()
   -- printDebug("++++++++++++++++++++在线红包啊openRedPacketCount"..openRedPacketCount)
    
    if openRedPacketCount > 0 then
        this.main_mid.dis_Text.text = "每天前3次炫耀，可获翻倍奖励哦!"
        this.main_mid.number_Text.text = string.format("翻倍奖励剩余: %s次", openRedPacketCount)
    else
        this.main_mid.dis_Text.text = ""
        this.main_mid.number_Text.text = ""
    end
end


