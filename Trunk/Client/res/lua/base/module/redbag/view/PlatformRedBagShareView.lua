require "base:enum/UIViewEnum"
require "base:module/redbag/data/PlatformRedBagProxy"

PlatformRedBagShareView = BaseView:new()
local this = PlatformRedBagShareView
this.viewName = "PlatformRedBagShareView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_RedBag_Share_View, false)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_redbag_share_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end
local m_type = 0
function this:onShowHandler(msg)
    self:initView()
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    m_type = msg
    --if msg == nil or msg == "" then
        --m_type = 1
    --end
    this:updataSharePanel()
  
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    this.main_mid.fail_Panel.gameObject:SetActive(false)
end

function this:addNotice()
end

function this:removeNotice()
end

function this:initView()
    this.main_mid.fail_Panel.gameObject:SetActive(false)
end
local m_endData = {}
function this:addEvent()
    --朋友圈分享
    this.main_mid.left_button:AddEventListener(
        UIEvent.PointerClick,
        function()
            local tarhetRect = this.main_mid.area_Image.rectTransform
            CS.ScreenSnapManager.Instance:StartScreenSnap(
                tarhetRect,
                function(...)
                    local succeedCallback = function()
                        showFloatTips("朋友圈分享成功！")
                        this:close()
                        this:openOnlineRedPacketOpenView()
                    end
                    local cancelCallback = function()
                        showFloatTips("朋友圈分享取消！")
                        this:close()
                    end
                    local failCallback = function()
                        showFloatTips("朋友圈分享失败！")
                        this:close()
                        this.main_mid.fail_Panel.gameObject:SetActive(true)
                    end
                    wxShareImage(
                        1,
                        CS.ScreenSnapManager.Instance:GetSnapPath(),
                        succeedCallback,
                        cancelCallback,
                        failCallback
                    )
                end
            )
        end
    )
    this.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_Share_View)
        end
    )
    this.main_mid.cancel_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_Share_View)
            --测试用 分享成功翻倍用 （封包注释）
            --this:openOnlineRedPacketOpenView()
        end
    )
    -- this.main_mid.close_Image:AddEventListener(
    --   UIEvent.PointerClick,
    --  function()
    --   this.main_mid.fail_Panel.gameObject:SetActive(false)
    -- end
    -- )

    --微信好友分享
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            local tarhetRect = this.main_mid.area_Image.rectTransform
            CS.ScreenSnapManager.Instance:StartScreenSnap(
                tarhetRect,
                function(...)
                    local succeedCallback = function()
                        showFloatTips("好友分享成功！")
                        this:close()
                        --发送相关分享所得翻倍奖励申请协议
                        this:openOnlineRedPacketOpenView()
                        


                    end
                    local cancelCallback = function()
                        showFloatTips("好友分享取消！")
                        this:close()
                    end
                    local failCallback = function()
                        showFloatTips("好友分享失败！")
                        this:close()
                        this.main_mid.fail_Panel.gameObject:SetActive(true)
                    end

                    wxShareImage(
                        0,
                        CS.ScreenSnapManager.Instance:GetSnapPath(),
                        succeedCallback,
                        cancelCallback,
                        failCallback
                    )
                end
            )
        end
    )
end

function this:updataSharePanel()

    --获取分享界面自己的头像
    local baseData = PlatformUserProxy:GetInstance():getUserInfo()
    printDebug("baseData" .. table.tostring(baseData))
    downloadUserHead(baseData.head_url, this.main_mid.head_Image)

    
    --分享当前app红包历史所有金额
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
	if data.history_total_moneys ~= nil then
        this.main_mid.packet_Text.text = tostring(this:redbagNumberToShow(data.history_total_moneys/100))
	end	
    

    --分享当前app红包金额
    --local baseData = PlatformUserProxy:GetInstance():getUserInfo()
	--if baseData ~= nil then
       -- this.main_mid.packet_Text.text = tostring(this:redbagNumberToShow(baseData.cash/100))
	--end	
    

    --分享此次所抢红包金额
    --local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
    --m_endData = data.reward_list
    --printDebug("+++++++++++++getRedBagEndData()" .. table.tostring(data))
    --for i = 1, #m_endData do
        --if m_endData[i].item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
            --this.main_mid.packet_Text.text = ((tonumber(m_endData[i].item_count)) / 100)
        --end
    --end


end

function this:close()
    if PlatformRedBagShareView.isOpen then
        ViewManager.close(UIViewEnum.Platform_RedBag_Share_View)
    end
end



function this:openOnlineRedPacketOpenView()

    openRedPacketCount = PlatformUserProxy:GetInstance():getOnlineRedPacketDailyCount()
    if openRedPacketCount > 0 then
        PlatformRedBagModule.sendReqOnlineRedPacketShareRewards(m_type)
    end
end

function this:redbagNumberToShow(number)
	if number == nil then
		printDebug("数字格式错误")
	else
		if number / 10^3 >= 1 then
			return math.floor(number)
		elseif number / 10^2 >= 1 then
			return tonumber(string.format("%.1f", number))
		else
			return number
		end
	end
end
