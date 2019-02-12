require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_share_end_panel"
require "base:module/redbag/data/PlatformRedBagProxy"

PlatformShareEndView = BaseView:new()
local this = PlatformShareEndView
this.viewName = "PlatformShareEndView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_Share_End_View, false)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_share_end_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

local m_endData = {}
--m_openType == 0为个人红包；==1为抢好友红包；==2为翻倍奖励
local m_openType = 0
function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    m_openType = msg
    this:updataEndPanel()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
end

function this:removeNotice()
end

function this:addEvent()
    this.main_mid.left_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Share_End_View)
        end
    )
    this.main_mid.mask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Share_End_View)
        end
    )
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Share_End_View)
            NoticeManager.Instance:Dispatch(NoticeType.Mall_Update_Money)
        end
    )
end

function this:updataEndPanel()
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
    --printDebug("++++++++++++++++++m_openType"..m_openType)
    if tonumber(m_openType) == 0 then 
        this.main_mid.title_Text.text = "恭喜你获得<color=#F43232FF>在线红包</color>奖励:"
    elseif tonumber(m_openType) == 1 then 
        local friendData = PlatformRedBagProxy:GetInstance():getOpenRedBagData("Open_Friend_Data")
        this.main_mid.title_Text.text = "恭喜你成功抢到<color=#F43232FF>"..friendData.name.."</color>的红包:"
    else 
        this.main_mid.title_Text.text = "恭喜获得分享翻倍奖励:"
    end
    
    m_endData = data.reward_list
    printDebug("getRedBagEndData()"..table.tostring(m_endData))
   
   
    this.main_mid.share_end_CellGroup:SetCellData(m_endData,this.updataEndShareCellList)
end

function this.updataEndShareCellList(go,data,index)
    local item =  this.main_mid.endshareCellArr[index+1]
    printDebug("++++++++++++++更新奖励ing"..table.tostring(data))
    this.main_mid.right_Button.gameObject:SetActive(true)
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
