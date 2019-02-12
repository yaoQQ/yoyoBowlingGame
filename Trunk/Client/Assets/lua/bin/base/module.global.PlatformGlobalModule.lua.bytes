require "base:enum/PlatformGlobalEnum/PlatformGlobalNoticeType"
require "base:enum/NoticeType"
require "base:module/global/data/PlatformGlobalProxy"

PlatformGlobalModule = BaseModule:new()
local this = PlatformGlobalModule

this.moduleName = "PlatformGlobal"

--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
    this.netFuncList = {}
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetHotGame, this.onRspGetHotGame)
    --请求获取好友红包信息返回
    this:AddNetLister(
        ProtoEnumPlatform.MsgIdx.MsgIdxRspGetFriendOnlineRedPacketInfo,
        this.onRspGetFriendOnlineRedPacketInfo
    )

    --请求自己的在线红包信息返回
    this:AddNetLister(
        ProtoEnumPlatform.MsgIdx.MsgIdxRspGetMyselfOnlineRedPacketInfo,
        this.onRspGetMyselfOnlineRedPacketInfo
    )
    --请求领取在线红包返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspReceiveOnLineRedPacket, this.onRspReceiveOnLineRedPacket)
    -- 通知红包被偷
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyRobbedOnlineRedPacket, this.onNotifyRobbedOnlineRedPacket)
    -- 请求金币补助返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetBankruptcySubsidy, this.onRspGetBankruptcySubsidy)
end
function this.onNetMsgLister(protoID, protoBytes)
    local nfSwitch = this.netFuncList[protoID]
    if nfSwitch then
        nfSwitch(protoBytes)
    else
        this:withoutRegistNotice(protoID)
    end
end

--==================================================消息==================================================

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        self.switch = {}

        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Rsp_User_Info, this.onRspUpdateUserInfo)
        self:AddNotifictionLister(
            PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info,
            this.onReqGetFriendOnlineRedPacketInfo
        )
        -- self:AddNotifictionLister(
        -- PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info,
        -- this.onRspUpdateGetFriendOnlineRedPacketInfo
        -- )

        self:AddNotifictionLister(
            PlatformGlobalNoticeType.Platform_Req_Get_Myself_Online_RedPacket_Info,
            this.onReqGetMyselfOnlineRedPacketInfo
        )

        --self:AddNotifictionLister(
           -- PlatformGlobalNoticeType.Platform_Req_Receive_OnLine_RedPacket,
           -- this.onReqReceiveOnLineRedPacket
        --)

        self:AddNotifictionLister(
            PlatformGlobalNoticeType.Platform_Rsp_Notify_Robbed_Online_RedPacket,
            this.onRspNotifyRobbedOnlineRedPacket
        )

        --进入游戏通知
        self:AddNotifictionLister(NoticeType.ActivityToEnterGame, this.onActivityToEnterGame)

        self:AddNotifictionLister(PlatformNoticeType.Platform_Req_RedBag_List, this.onReqRedBagList)

        --请求提现
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Rsp_Get_Money, this.onReceiveGetMoney)
		--android返回键提示
		self:AddNotifictionLister(NoticeType.Device_ReturnEvent,this.showDiviceTip)

    end
    return self.notificationList
end

---------------------------------------------收到消息(客户端或服务端发出)---------------------------

--收到用户个人信息
function this:onRspUpdateUserInfo(notice)
    -- require "base:module/platform/view/Global/PlatformGlobalView"
    -- local rep = notice:GetObj()
    -- printDebug("=================收到用户个人信息："..table.tostring(rep))
    -- PlatformGlobalProxy:GetInstance():setGlobalBaseData(rep)
    -- if PlatformGlobalView.isOpen then
    -- 	PlatformGlobalView:setMainBaseInfo()
    -- else
    -- 	ViewManager.open(UIViewEnum.Platform_Global_View)
    -- end
end

--收到附近可偷红包数据
function this:onRspUpdateGetFriendOnlineRedPacketInfo(notice)
    --	require "base:module/platform/view/RedBag/PlatformGlobalRedBagView"
    --    require "base:module/platform/data/RedBag/PlatformNewRedBagProxy"
    --	local rep = notice:GetObj().data
    --	printDebug("=================收到附近可偷红包数据："..table.tostring(rep))
    --	PlatformNewRedBagProxy:GetInstance():setStealRedBagData(rep.online_red_packet_list)
    --	--ViewManager.open(UIViewEnum.Platform_Global_RedBag_View)
    --    PlatformGlobalRedBagView.updateStealRedBagPanel()
end

--返回键提示
function this.showDiviceTip(notice,strData)
	
	local objStr = strData:GetObj()
	Alert.showVerifyMsg(nil,tostring(objStr),"取消",nil, "确定", function()
                                NoticeManager.Instance:Dispatch(NoticeType.Normal_QuitGame)
                            end)
end
--收到进入游戏场景通知
function this:onEnterGameScene(notice)
    require "base:module/global/view/PlatformGlobalView"
    if PlatformGlobalView ~= nil then
        PlatformGlobalView:enterGameHide()
    end
end

function this:onActivityToEnterGame(notice)
    printDebug("事件-从聊天进入游戏")
    local obj = notice:GetObj()
    GameManager.enterGame(obj.gameId, EnumGameType.NormalMatch, obj.shopId, obj.roomId)
end

-- 被偷红包信息
function this:onRspNotifyRobbedOnlineRedPacket(notice)
    local info = notice:GetObj()
    printDebug("被偷红包信息 = " .. table.tostring(info.user_list))

    require "base:module/redbag/data/PlatformNewRedBagProxy"
    PlatformNewRedBagProxy:GetInstance():setRobbedOnlineRedPacketData(info.user_list)
    printDebug("被偷红包了")
end

---- 显示推荐好友
--function this:onRecommendationOfFriend(notice)
--    printDebug("显示推荐好友A")
--    PlatformGlobalRecommendationOfFriendView:updateRecommendationOfFriend(notice)
--end

function this:onReceiveGetMoney(notice)
    local rsp = notice:GetObj()
    --PlatformGlobalProxy:GetInstance():setGetMoneyResultData(rsp.result)
    PlatformUserModule.sendGetMoneyCD()
    if ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_Success == rsp.result then
        local baseData = PlatformUserProxy:GetInstance():getUserInfo()
        printDebug("baseData" .. baseData.cash)
        Alert.showAlertMsg("提现成功", "请到支付宝账号查收!", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_Fail == rsp.result then
        Alert.showAlertMsg(nil, "提现失败", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_InvalidAccount == rsp.result then
        Alert.showAlertMsg(nil, "支付宝账号不存在", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_NoEnough == rsp.result then
        Alert.showAlertMsg(nil, "余额不足", "确定")
    end
end
--------------------------------------------发送消息(客户端发出)-------------------------------
--发送请求修改用户基本信息
-- function this:onRequestChangeUserInfo(notice)
-- 	local data = notice:GetObj()
-- 	printDebug("发送给服务器的基本用户信息为："..table.tostring(data))
-- 	this.sendNetMsg(GameConfig.ServerName.MainGateway,"platform","ReqChangeUserBaseInfo",data)
-- end

-- 请求获取好友红包信息
function this:onReqGetFriendOnlineRedPacketInfo(notice)
    local req = {}

    printDebug("请求获取好友红包信息:" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetFriendOnlineRedPacketInfo", req)
    --ShowWaiting(true,"GetFriendOnlineRedPacketInfo")
end

-- 请求获取自己的红包信息
function this:onReqGetMyselfOnlineRedPacketInfo(notice)
    local req = {}

    printDebug("请求获取自己的红包信息:" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetMyselfOnlineRedPacketInfo", req)
end

-- 请求领取在线红包
function this.sendReqReceiveOnLineRedPacket()
    local req = {}

    printDebug("请求领取在线红包:" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqReceiveOnLineRedPacket", req)
end

-- 请求领取红包列表
function this:onReqRedBagList(notice)
    local req = notice:GetObj()

    printDebug("请求领取红包列表:" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetActiveCashRedPacketRcvHistory", req)
end

--请求金币补助
function this.sendReqGetBankruptcySubsidy(req)
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetBankruptcySubsidy", req)
end


--------------------------------------------收到协议(服务端发出)-------------------------------

-- 收到热门游戏回复
function this.onRspGetHotGame(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetHotGame", protoBytes)
    -- printDebug("收到附近活动回复, rsp = "..table.tostring(rsp))
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Find_Hot_Game, rsp)
end

-- 收到请求开始匹配游戏返回
function this.onRspRoomMatchBegin(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspRoomMatchBegin", protoBytes)
    printDebug("请求开始匹配游戏返回 = " .. table.tostring(rsp))
end

--玩家的详细信息
-- function this.onRspUserInfo(protoBytes)
-- 	printDebug("=========================================================玩家的详细信息================================================")
-- 	 local rsp = this.decodeProtoBytes("platform","RspUserInfo", protoBytes)
--      NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_User_Info,rsp)
-- end

--请求获取好友红包信息返回
function this.onRspGetFriendOnlineRedPacketInfo(protoBytes)
    printDebug(
        "=========================================================请求获取好友红包信息返回================================================"
    )
    local rsp = this.decodeProtoBytes("platform", "RspGetFriendOnlineRedPacketInfo", protoBytes)
    printDebug("=================收到附近可偷红包数据："..table.tostring(rsp))
    PlatformNewRedBagProxy:GetInstance():setStealRedBagData(rsp.online_red_packet_list)
    PlatformNewRedBagProxy:GetInstance():setStealRedBagNum(rsp.left_can_steal_num)
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info)
    --ShowWaiting(false,"GetFriendOnlineRedPacketInfo")
end

--请求自己的在线红包信息返回
function this.onRspGetMyselfOnlineRedPacketInfo(protoBytes)
    printDebug(
        "=========================================================请求自己的在线红包信息返回================================================"
    )
    local rsp = this.decodeProtoBytes("platform", "RspGetMyselfOnlineRedPacketInfo", protoBytes)
    printDebug("=================收到自己的红包数据："..table.tostring(rsp))
	if rsp == nil then return end
    PlatformNewRedBagProxy:GetInstance():setMyselfRedBagData(rsp.online_red_packet)
    --红包刷新响应
    NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info)
end

--请求领取在线红包返回
function this.onRspReceiveOnLineRedPacket(protoBytes)
    printDebug(
        "=========================================================请求领取在线红包返回================================================"
    )
    local rsp = this.decodeProtoBytes("platform", "RspReceiveOnLineRedPacket", protoBytes)
        printDebug("请求领取在线红包返回:"..table.tostring(rsp))
        local baseData = PlatformUserProxy:GetInstance():getUserInfo()
        printDebug("请求线红包返回  updateRedBagPanel 提现￥ ："..tostring(baseData.cash/100))	
        if rsp.result == ProtoEnumPlatform.RcvOnlineRPResult.RcvOnlineRPResult_Success then
            printDebug("成功")
            rsp.isFriend = false
            PlatformRedBagProxy:GetInstance():setRedBagEndData(rsp)
            PlatformRedBagRedPacketOpenView:openSuccessHandler()
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Myself_Online_RedPacket_Info)
        elseif rsp.result == ProtoEnumPlatform.RcvOnlineRPResult.RcvOnlineRPResult_Wait then
            printDebug("等待")
        elseif rsp.result == ProtoEnumPlatform.RcvOnlineRPResult.RcvOnlineRPResult_OtherError then
            printDebug("其它错误")
            Alert.showAlertMsg(nil,"领取红包失败","确定")
        end
end

-- 通知红包被偷
function this.onNotifyRobbedOnlineRedPacket(protoBytes)
    printDebug(
        "=========================================================请求领取在线红包返回================================================"
    )
    local rsp = this.decodeProtoBytes("platform", "NotifyRobbedOnlineRedPacket", protoBytes)
    printDebug("通知红包被偷:" .. table.tostring(rsp))

    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Notify_Robbed_Online_RedPacket, rsp)
end

-- 请求红包领取列表返回
function this.onRspGetRedBagHistory(protoBytes)
    --[[printDebug("=========================================================请求红包领取列表返回================================================")
	local rsp = this.decodeProtoBytes("platform","RspGetActiveCashRedPacketRcvHistory", protoBytes)
    printDebug("请求红包领取列表返回:"..table.tostring(rsp))

    NoticeManager.Instance:Dispatch(PlatformNoticeType.Receive_RedBag_List,rsp)  --]]
end

---- 请求附近好友推荐返回
--function this.onRspFriendRecommend(protoBytes)
--	printDebug("=========================================================请求领取在线红包返回================================================")
--	local rsp = this.decodeProtoBytes("platform","RspFriendRecommend", protoBytes)
--    printDebug("请求附近好友推荐返回:"..table.tostring(rsp))

----    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Mail_Attach,rsp)
--end

-- 通知金币补助返回
function this.onRspGetBankruptcySubsidy(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetBankruptcySubsidy", protoBytes)
    --ViewManager.close(UIViewEnum.Platform_Common_Subsidy_View)
    --NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Get_Bankruptcy_Subsidy)
    printDebug("wahahahhahaha")
	--this.main_mid.subsidy_Panel.gameObject:SetActive(false)
	--this.main_mid.end_Panel.gameObject:SetActive(true)
	local data = {}
	data.dest_item = TableBaseBankruptcySubsidy.data[1].award_money
	ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
        
end



