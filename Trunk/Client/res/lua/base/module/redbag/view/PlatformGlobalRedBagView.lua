require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_newredbag_panel"
require "base:module/redbag/data/PlatformNewRedBagProxy"

--主界面：红包
PlatformGlobalRedBagView = BaseView:new()
local this = PlatformGlobalRedBagView
this.viewName = "PlatformGlobalRedBagView"

--设置面板特性
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_RedBag_View, true)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_newredbag_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    --下面两行默认需要调用
    UITools.SetParentAndAlign(gameObject, self.container)
    --设置UI中间代码
    self.main_mid = Mid_platform_newredbag_panel
    self:BindMonoTable(gameObject, self.main_mid)
    this.main_mid.spine.skeleton.AnimationState:SetAnimation(0, "Animation_cat_001", false)
    this.main_mid.add_friend_tip_panel.gameObject:SetActive(false)
    --添加UI事件监听
    self:addEvent()
	
	--屏蔽提现按钮
	this.main_mid.withdraw_Button.gameObject:SetActive(false)
end
this.bool = true
local textData = nil 
--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()
    self:initView()
    this.onUpdateUserInfo()
    --打开子界面
    ViewManager.open(UIViewEnum.Platform_Top_Cost_View, UIViewEnum.Platform_Global_RedBag_View)
    ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Global_RedBag_View)
end

--override 关闭UI回调
function this:onClose()
    this:stopMyRedBagLeftSeconds()
    this.leftSecondsStop()
    self:removeNotice()
    --ShowWaiting(false,this.viewName)
    this.isShowWaiting = false
    --关闭子界面
    ViewManager.close(UIViewEnum.Platform_Global_View)
    ViewManager.close(UIViewEnum.Platform_Top_Cost_View)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("isShowWaitingRedBag")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("CatTime")
    this.main_mid.redbag_Icon.gameObject:SetActive(false)
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_UserInfo, this.onUpdateUserInfo)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Cash, this.onUpdateCash)
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info,
        this.updateStealRedBagPanel
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info,
        this.updateRedBagPanel
    )
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_UserInfo, this.onUpdateUserInfo)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Cash, this.onUpdateCash)
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info,
        this.updateStealRedBagPanel
    )
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info,
        this.updateRedBagPanel
    )
end

function this:addEvent()
    self.main_mid.countdown_Image:AddEventListener(UIEvent.PointerClick, self.geMyRedBag)
    self.main_mid.add_friends_Button:AddEventListener(UIEvent.PointerClick, self.addFriendsBtn)
    self.main_mid.withdraw_Button:AddEventListener(UIEvent.PointerClick, self.withdrawBtn)
    self.main_mid.go_guess_Image:AddEventListener(UIEvent.PointerClick, self.goGuessImg)
end
--------------------------------------------------------------------------

--打开界面时初始化
function this:initView()
    this.main_mid.money_text.text = ""
    this.main_mid.countdown_Text.text = ""
    this.main_mid.add_Effect:Play()
    this.main_mid.redbag_Icon.gameObject:SetActive(false)
    this.onUpdateCash()
      
    

    --5.11
    --     if not this.isReqRecommend then
    --         local tab={}
    --         tab.lng = MapManager.userLng
    --         tab.lat = MapManager.userLat
    --         tab.distance = 1000000
    --         tab.maxcount = 10
    --         NoticeManager.Instance:Dispatch(PlatformFriendType.Platform_Friend_Req_Recommend,tab)
    --         this.isReqRecommend = true
    --     end
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Req_Get_Mail,
        {mail_type = ProtoEnumCommon.MailType.MailType_Redpacket, page_index = 0, per_page_count = 30}
    )
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Myself_Online_RedPacket_Info)
    PlatformLBSModule.sendReqNearActivity(MapManager.userLng, MapManager.userLat, MapManager.getCurScreenMapPos(500))
    ----------------------------------显示等待，如果失败，三秒后重发-----------------------------
    --后端还没改好，先屏蔽
    --ShowWaiting(true,this.viewName)
    this.isShowWaiting = true
    local fun = function(args)
        --if this.isShowWaiting then
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info)
        --else
        -- GlobalTimeManager.Instance.timerController:RemoveTimerByKey("isShowWaitingRedBag")
        -- end
    end
    fun()
    GlobalTimeManager.Instance.timerController:AddTimer("isShowWaitingRedBag", 30000, -1, fun)
    ----------------------------------------------------------------------------------------------
    this.main_mid.friendlist_CellGroup.gameObject:SetActive(false)

    --this.main_mid.game_CellRecycleScrollPanel.gameObject:SetActive(false)
end

function this.onUpdateUserInfo()
    --更新顶部基本信息
    local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()

    if currGlobalBaseData == nil then
        return
    end
    downloadUserHead(currGlobalBaseData.head_url, this.main_mid.head_Icon)
    this:showRecommendationOfFriend()
end



function this.onRspRecommend()
    if PlatformGlobalRecommendationOfFriendView.isOpen then
        PlatformGlobalRecommendationOfFriendView:updateRecommendationOfFriend()
    else
        ViewManager.open(UIViewEnum.Platform_Global_RecommendationOfFriend_View)
    end
end
this.isEnoughToGet = false
--显示推荐好友
local m_isNewDay = false
 --显示推荐好友
 function this:showRecommendationOfFriend()
    local myId = LoginDataProxy.playerId
    local friendListData = PlatformFriendProxy:GetInstance():getFriendListData()
    local time = PlayerPrefs.GetString("TIME_New"..myId,"")
    printDebug ("TIME_New"..tostring(time))
    if time == nil or time == "" then
        PlayerPrefs.SetString("TIME_New"..myId,"")
        m_isNewDay = true
    else
        if os.date("%Y/%m/%d") ~= PlayerPrefs.GetString("TIME_New"..myId,"") then
            m_isNewDay = true
        end
    end

    if m_isNewDay then
        if #friendListData < 10 then
            m_isNewDay = false
            PlayerPrefs.SetString("TIME_New"..myId,os.date("%Y/%m/%d"))
            local time = PlayerPrefs.GetString("TIME_New"..myId,"")
            printDebug ("TIME_New"..tostring(time))
            this:addFriendsBtn()
        end
    end
 end

--收到自己的红包数据
function this.onRspUpdateGetMyselfOnlineRedPacketInfo(notice, rsp)
    local rep = rsp:GetObj().data

    printDebug("=================收到自己的红包数据：" .. table.tostring(rep))
    if rep == nil then
        return
    end
    PlatformNewRedBagProxy:GetInstance():setMyselfRedBagData(rep.online_red_packet)
    this:updateRedBagPanel()
end

--更新红包界面

function this.onUpdateCash()
    local baseData =  PlatformUserProxy:GetInstance():getUserInfo()
    if baseData ~= nil then
        printDebug("+++++++++++更新红包界面+++++++++++++ ：" .. table.tostring(baseData))
        this.main_mid.money_text.text = tostring(baseData.cash / 100)
        textData = tostring(baseData.cash / 100)
    -- if baseData.cash > 1000000 then
    --this.main_mid.money_text.text = tostring(baseData.cash/1000000).."万"
    --end
    end
end

function this:updateRedBagPanel()
    local spine = this.main_mid.spine
	
    local data = PlatformNewRedBagProxy:GetInstance():getMyselfRedBagData()
    if data == nil then
        return
    end
    
    -- local fun = function (args)
    if data.iscan_receive == 0 then
        --spine.skeleton.AnimationState:SetAnimation(0, "Animation_cat_004", true)
        printDebug("redbag 还不可以领取红包")
        this.leftSecondsName = "LeftSeconds"
        this.leftSeconds = data.left_seconds
        this.isCanGetMyRedBag = false
        this.isEnoughToGet = false
        this:startMyRedBagLeftSeconds()
    elseif data.iscan_receive == 1 then
        printDebug("redbag 可以领取红包")
        if this.main_mid then
            this.main_mid.countdown_Text.text = "点击即可领取红包"
            this.main_mid.redbag_Icon:ChangeIcon(0)
            this.main_mid.redbag_Icon.gameObject:SetActive(true)
        end
        this.isCanGetMyRedBag = true
        this.isEnoughToGet = false
    elseif data.iscan_receive == 2 then
        --已经达到领取上限
        this.isEnoughToGet = true
        this.isCanGetMyRedBag = false
        this.main_mid.redbag_Icon.gameObject:SetActive(false)
    end
    GlobalTimeManager.Instance.timerController:AddTimer(
        "CatTime",
        1167,
        1,
        function(...)
            spine.skeleton.AnimationState:SetAnimation(0, "Animation_cat_002", true)
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("CatTime")
        end
    )

end

--开启自己领红包的倒计时
function this:startMyRedBagLeftSeconds(args)
    this.main_mid.redbag_Icon:ChangeIcon(1)
    this.main_mid.redbag_Icon.gameObject:SetActive(true)
    local h = math.floor(this.leftSeconds / 3600)
    local m = math.floor((this.leftSeconds % 3600) / 60)
    local s = math.floor((this.leftSeconds % 3600) % 60)
    if this.leftSeconds > 0 then
        this.main_mid.countdown_Text.text =
            "<color=#cd2a27>" ..
            ((h ~= 0) and (h > 9 and h or "0" .. h) .. ":" or "00:") ..
                ((m ~= 0) and (m > 9 and m or "0" .. m) .. ":" or "00:") ..
                    ((s ~= 0) and (s > 9 and s or "0" .. s) .. "" or "00") .. "后可领取</color>"
    end
    local fun =
        function(args)
        this.leftSeconds = this.leftSeconds - 1
        if this.leftSeconds > 0 then
            local h = math.floor(this.leftSeconds / 3600)
            local m = math.floor((this.leftSeconds % 3600) / 60)
            local s = math.floor((this.leftSeconds % 3600) % 60)
            --this.main_mid.countdown_Text.text = ((h ~= 0) and h..":" or "")..((m ~= 0) and m..":" or "")..((s ~= 0) and s.."" or "").."后可以领取"
            this.main_mid.countdown_Text.text =
                "<color=#cd2a27>" ..
                ((h ~= 0) and (h > 9 and h or "0" .. h) .. ":" or "00:") ..
                    ((m ~= 0) and (m > 9 and m or "0" .. m) .. ":" or "00:") ..
                        ((s ~= 0) and (s > 9 and s or "0" .. s) .. "" or "00") .. "后可领取</color>"
        else
            this:stopMyRedBagLeftSeconds()
            NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Myself_Online_RedPacket_Info)
        end
    end
    if not this.isEnoughToGet then
        GlobalTimeManager.Instance.timerController:AddTimer(this.leftSecondsName, 1000, -1, fun)
    end
end

--关闭自己领红包的倒计时
function this:stopMyRedBagLeftSeconds(args)
    local isTrue =
        this.leftSecondsName and (GlobalTimeManager.Instance.timerController:CheckExistByKey(this.leftSecondsName)) or
        false
    if isTrue then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.leftSecondsName)
    end
end

function this.showAddFriendTip()
    this.main_mid.add_friend_tip_panel.gameObject:SetActive(true)
    local function crossElement(state)
        local alpha = 0
        if state then
            alpha = 1
        end
        local crossTime = 0.5
        this.main_mid.secretary_head_image.Img:CrossFadeAlpha(alpha, crossTime, true)
        this.main_mid.secretary_name_text.Txt:CrossFadeAlpha(alpha, crossTime, true)
        this.main_mid.add_friend_tip_bg.Img:CrossFadeAlpha(alpha, crossTime, true)
        this.main_mid.add_friend_tip_text.Txt:CrossFadeAlpha(alpha, crossTime, true)
    end
    crossElement(true)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformGlobalRedBagView2")
    GlobalTimeManager.Instance.timerController:AddTimer("PlatformGlobalRedBagView2", 4000, 1, function()
        crossElement(false)
    end)
end
--更新可偷红包界面
function this.updateStealRedBagPanel()
    
    this.stealRedBagData = PlatformNewRedBagProxy:GetInstance():getStealRedBagData()
    if this.stealRedBagData == nil then
        this.main_mid.friendlist_CellGroup.gameObject:SetActive(false)
        -- 显示增加好友提示
        --print("显示增加好友提示")
        this.showAddFriendTip()
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformGlobalRedBagView5")
        GlobalTimeManager.Instance.timerController:AddTimer("PlatformGlobalRedBagView5", 10000, -1, function()
            this.showAddFriendTip()
        end)
        return
    end
    --print("隐藏增加好友提示")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformGlobalRedBagView5")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformGlobalRedBagView2")
    this.main_mid.add_friend_tip_panel.gameObject:SetActive(false)

    --printDebug("+++++更新可偷红包界面++++++：" .. table.tostring(this.stealRedBagData))

    local fun = function(args)
        if this.main_mid then
            for i = 1, #this.main_mid.friendlistcellArr do
                this.main_mid.friendlistcellArr[i].go:SetActive(false)
            end
        end
        if not this.main_mid.friendlist_CellGroup.gameObject.activeSelf then
            this.main_mid.friendlist_CellGroup.gameObject:SetActive(true)
        end
        this.stealRedBagLeftSeconds = {}
        
        for j = 1, #this.stealRedBagData do
            if j <= 6 then
                this.updateStealRedBagCell(j)
            end
        end
       this.leftSecondsStart()
    end
    fun()
end

--更新红包界面
function this.updateStealRedBagCell(index)
    if index > #this.main_mid.friendlistcellArr then
        return
    end
    local item = this.main_mid.friendlistcellArr[index]
    local data = this.stealRedBagData[index]

    item.go:SetActive(true)

    printDebug("更新可偷红包界面：" .. table.tostring(data))
    downloadUserHead(data.header, item.head_Icon)

    item.name_Text.text = tostring(data.name)
    if data.leftseconds <= 0 then
        item.btn_get_Text.text = "抢Ta红包"
        item.btn_Icon:ChangeIcon(0)
        item.btn_Image.gameObject:SetActive(true)
    else
        item.btn_Icon:ChangeIcon(1)
        item.btn_Image.gameObject:SetActive(false)
        data.leftseconds = data.leftseconds - 1
        local h = math.floor(data.leftseconds / 3600)
        local m = math.floor((data.leftseconds % 3600) / 60)
        local s = math.floor((data.leftseconds % 3600) % 60)
        item.btn_get_Text.text =
            ((h ~= 0) and h .. "时" or "") .. ((m ~= 0) and m .. "分" or "") .. ((s ~= 0) and s .. "秒" or "")
    end
    local fun = function()
		if data.leftseconds <= 0 then
			local msg = {}
			msg.header = data.header
			msg.id = data.player_id
			msg.name = data.name
			PlatformRedBagProxy:GetInstance():setOpenRedBagData("Open_Friend_Data", msg)
			PlatformRedBagOpenView:openOnlineRedPacketOpenView()
        else
			showFloatTips("请耐心等候...")
		end
    end

    item.btn_Image:AddEventListener(UIEvent.PointerClick, fun)
    item.head_Icon:AddEventListener(UIEvent.PointerClick, fun)

    local tab = {}
    tab.btn_get_Text = item.btn_get_Text
    tab.leftSeconds = data.leftseconds
    tab.name = data.player_id
    tab.btn_Icon = item.btn_Icon
    tab.btn_Image = item.btn_Image
    table.insert(this.stealRedBagLeftSeconds, tab)
end

--好友投红包倒计时开启
function this.leftSecondsStart(args)
    printDebug("this.stealRedBagLeftSeconds" .. table.tostring(this.stealRedBagLeftSeconds))
    if this.stealRedBagLeftSecondsTimer == nil then
        this.stealRedBagLeftSecondsTimer = "stealRedBagLeftSecondsTimer"
    end
    this.leftSecondsStop()

    local fun =
        function(args)
        --printDebug("倒计时")
        for i = 1, #this.stealRedBagLeftSeconds do
            if this.stealRedBagLeftSeconds[i] then
                --this.stealRedBagLeftSeconds[i].btn_get_Text.text = ""
                if this.stealRedBagLeftSeconds[i].leftSeconds <= 0 then
                    this.stealRedBagLeftSeconds[i].btn_get_Text.text = "抢Ta红包"
                    this.stealRedBagLeftSeconds[i].btn_Icon:ChangeIcon(0)
                    this.stealRedBagLeftSeconds[i].btn_Image.gameObject:SetActive(true)
                    table.remove(this.stealRedBagLeftSeconds, i)
                else
                    this.stealRedBagLeftSeconds[i].leftSeconds = this.stealRedBagLeftSeconds[i].leftSeconds - 1
                    local h = math.floor(this.stealRedBagLeftSeconds[i].leftSeconds / 3600)
                    local m = math.floor((this.stealRedBagLeftSeconds[i].leftSeconds % 3600) / 60)
                    local s = math.floor((this.stealRedBagLeftSeconds[i].leftSeconds % 3600) % 60)
                    this.stealRedBagLeftSeconds[i].btn_get_Text.text =
                        ((h ~= 0) and h .. "时" or "") .. ((m ~= 0) and m .. "分" or "") .. ((s ~= 0) and s .. "秒" or "")
                end
            end
        end
    end
    GlobalTimeManager.Instance.timerController:AddTimer(this.stealRedBagLeftSecondsTimer, 1000, -1, fun)
end

--好友投红包倒计时停止
function this.leftSecondsStop(args)
    local isTrue =
        this.stealRedBagLeftSecondsTimer and
        GlobalTimeManager.Instance.timerController:CheckExistByKey(this.stealRedBagLeftSecondsTimer) or
        false
    if isTrue then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.stealRedBagLeftSecondsTimer)
    end
end

--更新红包界面
function this:updateRedBagMail(id)
    printDebug("更新红包界面")
    local fun = function(args)
        this.getRedBagMailData = PlatformNewRedBagProxy:GetInstance():getRedPacketMailList()
        if this.getRedBagMailData == nil or #this.getRedBagMailData <= 0 then
            this.getRedBagMail = nil
            --this.main_mid.left_Image.gameObject:SetActive(false)
            --this.main_mid.leftredbag_Image.gameObject:SetActive(false)
            return
        end
        if #this.getRedBagMailData > 0 then
            local btnFun = function(args)
                this.getRedBagMailTempData = this.getRedBagMailData[#this.getRedBagMailData]
                local tab = {}
                tab.mail_type = ProtoEnumCommon.MailType.MailType_Redpacket
                tab.mail_id = this.getRedBagMailTempData.id
                NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Mail_Attach, tab)
            end
            --if not this.main_mid.left_Image.activeSelf then
            -- this.main_mid.left_Image.gameObject:SetActive(true)
            --this.main_mid.leftredbag_Image.gameObject:SetActive(true)
            -- end
            -- this.main_mid.left_Text.text = (#this.getRedBagMailData)
            this.getRedBagMail = btnFun
        end
    end
    if id == nil then
        fun()
    elseif id == this.getRedBagMailTempData.id then
        PlatformNewRedBagProxy:GetInstance():removeRedPacketMailListLast()
        Alert.showAlertMsg(nil, this.getRedBagMailTempData.subject, "确定", fun)
    end
end

---------------------------------------------------------------------------

--点击事件
function this:geMyRedBag()
    if this.isCanGetMyRedBag then
        --NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Receive_OnLine_RedPacket)
        local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
        this.main_mid.spine.skeleton.AnimationState:SetAnimation(0, "Animation_cat_003", false)
        if currGlobalBaseData == nil then
            return
        end
        local msg = {}
        msg.headUrl = currGlobalBaseData.head_url
        msg.number = currGlobalBaseData.number
        PlatformRedBagProxy:GetInstance():setOpenRedBagData("Open_Data", msg)
        ViewManager.open(UIViewEnum.Platform_RedBag_RedPacket_Open_View)
    else
        if this.isEnoughToGet then
            Alert.showAlertMsg("领取失败", "已达到领取上限", "确定")
        end
    end
end

function this:addFriendsBtn(args)
    printDebug("加好友抢红包")
    local req = {}
    req.lng = MapManager.userLng
    req.lat = MapManager.userLat
    req.distance = 1000000 --暂定1公里
    req.maxcount = 1000 --暂定最多1000个商家
    PlatformFriendModule.onSendRecommend(req)
    --ViewManager.open(UIViewEnum.Platform_Global_RecommendationOfFriend_View)
end

function this:withdrawBtn(args)
    local baseData = PlatformUserProxy:GetInstance():getUserInfo()
    if baseData == nil then
        return
    end
    local limitWithdrawMoney = TableBaseGetmoney.data[1].money
    if baseData.cash < limitWithdrawMoney then
        showTopTips(string.format("红包余额大于%s元方可提现", math.floor(limitWithdrawMoney / 100)))
        return
    end
    PlatformUserModule.sendGetMoneyCD()
end

function this:withdrawRecordImg(args)
    --ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_Record_View)
end

function this:leftredbagImg(args)
    printDebug("红包界面右下的那只红包")
    if this.getRedBagMail then
        this.getRedBagMail()
    end
end

function this:changeTxt(args)
    printDebug("附近红包的换一批")
end

function this:goGuessImg(args)
    printDebug("进入赛事喽")
    ViewManager.open(UIViewEnum.Platform_Global_Shop_View)
end

