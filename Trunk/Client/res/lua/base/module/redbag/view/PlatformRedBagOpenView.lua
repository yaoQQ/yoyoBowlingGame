
require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_open_panel"
require "base:module/redbag/data/PlatformNewRedBagProxy"

--主界面：红包
PlatformRedBagOpenView = BaseView:new()
local this = PlatformRedBagOpenView
this.viewName = "PlatformRedBagOpenView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.Platform_RedBag_Open_View, false)

--设置加载列表
this.loadOrders=
{
	"base:redbag/platform_redbag_open_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	--下面两行默认需要调用
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	--设置UI中间代码
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	
	--添加UI事件监听
	self:addEvent()
end
this.openData = {}
--override 打开UI回调
function this:onShowHandler()
    this.openData = PlatformRedBagProxy:GetInstance():getOpenRedBagData("Open_Friend_Data")
    if table.empty(this.openData) then
        return
    end
    self:addNotice()
    self.showPacketView(this.openData)
    self:initAnimEvents()
    self:init()
    

end

function this:init()
    local anim = self.main_mid.open_panel:GetAnimClip("qianghongbao_002")
    if anim ~= nil then
        local length = math.ceil(anim.length * 1000)
        GlobalTimeManager.Instance.timerController:AddTimer("FriendOpenBtn", length, 1, function ()
            print("按钮可用")
            this.isCanPoint = true
        end)
    else
        this.isCanPoint = true
    end
    this.main_mid.noClick_Image.gameObject:SetActive(false)
end

--override 关闭UI回调
function this:onClose()
	
    
    this.main_mid.open_panel:ResetEvents()
    self.main_mid.open_btn_animator:ResetEvents()

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("ActiveOpenBtn")
    self.main_mid.open_panel:Play("qianghongbao_001");
    ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
end

--领取现金
function this:addNotice()
	--NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onUpdateUserInfo)
end

function this:removeNotice()
	--NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onUpdateUserInfo)
end
function this:addEvent()
    self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick,  self.close)
    self.main_mid.openredbag_btn:AddEventListener(UIEvent.PointerClick, self.openBtnHandler)
end

this.isCanPoint = false


function this.openBtnHandler()
    if this.isCanPoint == false then
        print("按钮还不可用~~~")
        return
    end
    this.isCanPoint = false
    this.main_mid.noClick_Image.gameObject:SetActive(true)
	PlatformRedBagModule.sendReqStealOnlineRedPacket(this.openData.id)
end

this.isLockClose = false
-- 收到成功回复之后播放效果(包括动画和特效), 同时锁定关闭本界面许可
function this:openSuccessHandler()
    this.isLockClose = true
    printDebug("播放了哦")
    self.main_mid.open_btn_animator:Play("open_btn_open");
    this:initAnimEvents()
end


function this:initAnimEvents()
    --self.main_mid.open_panel:Play("open_panel");
   -- self.main_mid.open_panel:AddEndAnimationEvent("packet_elastic", function (str)
        --self.main_mid.open_btn_animator:Play("open_btn_elastic");
    --end)
    self.main_mid.open_panel:Play("qianghongbao_002");
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
    printDebug("我是播放这里的动画哦"..table.tostring(data))
    self.main_mid.open_btn_animator:AddEndAnimationEvent("open_btn_open", function (str)
            if data.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_Success then
                self.main_mid.open_panel:Play("qianghongbao_004");
            else
                self.main_mid.open_panel:Play("qianghongbao_003");
            end
    end)
        self.main_mid.open_panel:AddEndAnimationEvent("qianghongbao_004", function (str)
            this.isLockClose = false
            this:shareEvent()
            PlatformRedBagOpenView.close(false)
        end)
        self.main_mid.open_panel:AddEndAnimationEvent("qianghongbao_003", function (str)
            this.isLockClose = false
            this:pasePacket()
            PlatformRedBagOpenView.close(false)
        end)
    
        
        
        
       

        
  
    
end

--override 关闭UI回调
function this:onClose()
    self.main_mid.open_panel:ResetEvents()
    self.main_mid.open_btn_animator:ResetEvents()

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("FriendOpenBtn")
    this.isCanPoint = false
    this:removeNotice()
end

function this.showPacketView(data)
    local num = PlatformNewRedBagProxy:GetInstance():getStealRedBagNum()
    printDebug("我是好友的头像哦data"..table.tostring(data))
    this.main_mid.name_text.text = data.name
    downloadUserHead(data.header, this.main_mid.head_circleimage)
    --this.main_mid.remain_text.text = "剩余偷取次数:"..num
end

function this.close(isForce)
    if isForce then
        ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    else
        if this.isLockClose then
            return
        end
    end
    ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    this.main_mid.open_panel:ResetEvents()
end

function this:openOnlineRedPacketOpenView()
    openRedPacketCount = PlatformUserProxy:GetInstance():getOnlineRedPacketDailyCount()
    --if openRedPacketCount <= 0 then
       -- Alert.showAlertMsg(nil,"今日分享红包的次数已到达上限!\n(每日00：00点时重置领取次数)"
       -- ,"确定")
    --else
        ViewManager.open(UIViewEnum.Platform_RedBag_Open_View)
   -- end
end
local isShare = false
--是否开启分享界面
local m_endData = {}
function this:shareEvent()
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
    printDebug("获取的消息"..table.tostring(data))
    m_endData = data.reward_list
    if m_endData == nil or m_endData == "" then
        this:pasePacket()
    else
        for i = 1, #m_endData do
            if m_endData[i].item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
                isShare = true
                break
            end
        end
        if isShare then 
            --红包刷新响应
            NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
            ViewManager.open(UIViewEnum.Platform_RedBag_End_View, 1)
        else
            --红包刷新响应
            NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
            ViewManager.open(UIViewEnum.Platform_Share_End_View , 1)
        end
        PlatformRedBagRedPacketOpenView.close(false)
        this.main_mid.open_panel:ResetEvents()
    end
end

function this:pasePacket()
    ViewManager.open(UIViewEnum.Platform_RedBag_Pass_View)
    this.main_mid.open_panel:ResetEvents()
end
