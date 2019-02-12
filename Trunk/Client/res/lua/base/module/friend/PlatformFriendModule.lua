require "base:enum/NoticeType"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/PlatformFriendProxy"

PlatformFriendModule = BaseModule:new()
local this = PlatformFriendModule

this.moduleName = "PlatformFriend"

--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
    this.netFuncList = {}
    --加好友、删除好友
    --请求好友列表返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspFriendList, this.onRspFriendList)
    --请求好友操作返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspFriendOp, this.onRspFriendOp)
    --通知申请添加好友成功
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxNotifyAgreeAddFriend, this.onNotifyAgreeAddFriend)
    --通知有人加你为好友
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxNotifyAddFriendApply, this.onNotifyAddFriendApply)
    --通过手机号搜索好友操作返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspFindUserByTelNo, this.onRspSearchPhone)
    --乙通知添加好友加入好友列表
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxNotifyAddFriend, this.onNotifyAddFriendReceiver)

    --聊天、离线聊天
    --请求发送好友聊天消息返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspSendFriendChat, this.onRspSendFriendMsg)
    --通知好友聊天消息
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxNotifyFriendChat, this.onNotifyFriendChat)
    --请求获取好友离线聊天信息返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspOfflineFriendChat, this.onRspOfflineChat)
    --请求确认收到好友聊天返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspConfirmFriendChat, this.onRspConfirmChat)

    --请求附近好友推荐返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspFriendRecommend, this.onReceiveRecommend)

    --请求好友申请列表返回
    this:AddNetLister(ProtoEnumFriendModule.MsgIdx.MsgIdxRspFriendApplyList, this.onRspFriendApplyList)
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
        self:AddNotifictionLister(NoticeType.Logout, this.onNotifyLogout)
    end
    return self.notificationList
end

function this.onNotifyLogout()
    PlatformFriendProxy:GetInstance():initData()
end

--------------------------------------------协议回复---------------------------
--请求获取好友申请列表返回
function this.onRspFriendApplyList(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspFriendApplyList", notice)
    if not table.empty(rsp.player_base_info_list) then
        local applyList = {}
        for k, v in pairs(rsp.player_base_info_list) do
            if rsp.apply_info_list[k] then
                local data = {}
                if PlatformFriendProxy:GetInstance():isMyFriendById(v.player_id) then
                    local msg = {
                        op = ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend,
                        player_id = v.player_id
                    }
                    PlatformFriendModule.onReqFriendOp(msg)
                elseif PlatformFriendProxy:GetInstance():isNotInMyApplyList(v.player_id) then
                    data.player_base_info = v
                    data.apply_info = rsp.apply_info_list[k]
                    table.insert(applyList, data)
                end
            end
        end
        PlatformFriendProxy:GetInstance():setReceiveAddFriendApplyData(applyList)
    end
end

--请求好友列表返回
function this.onRspFriendList(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspFriendList", notice)
    PlatformFriendProxy:GetInstance():setFriendListData(rsp.friend_info_list)
    NoticeManager.Instance:Dispatch(PlatformFriendType.Receive_Update_Friend_List, rsp)

    --登录时获取的好友列表返回跳出,只会调用一次
    if MainModule.getIsLoginReqFriendList() then
        --getdb登录时一次性把好友的聊天数据获取好缓存到内存
        if not table.empty(rsp.friend_info_list) then
            for _, v in pairs(rsp.friend_info_list) do
                if not table.empty(v.player_base_info) then
                    PlatformFriendProxy:GetInstance():getHistoryMsgFromDB(v.player_base_info.player_id)
                end
            end
        end
        return MainModule.setIsLoginReqFriendList(false)
    end
end

--请求好友操作返回
function this.onRspFriendOp(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspFriendOp", notice)
    local result = rsp.result
    if ProtoEnumFriendModule.FriendOpResult.FriendOpResultSuccess == result then
        --如果该项操作为删除好友，则清空该好友对应的聊天记录
        if rsp.op == ProtoEnumFriendModule.FriendOp.FriendOpDelFriend then
            PlatformFriendProxy:GetInstance():removeReiceiveFriendApplyData(FriendChatDataProxy.currDelFriendId)
            PlatformFriendProxy:GetInstance():delCurrChatFriendData(FriendChatDataProxy.currDelFriendId)
            ViewManager.close(UIViewEnum.Platform_Friend_Main_Page_View)

            --更新主页红点
            PlatformGlobalView:onUpdateChatOnlineMsgCount()

            --更新列表红点
            if PlatformGlobalMessageView:getIsOpen() then
                PlatformGlobalMessageView:updateMessagePanel()
            end
        elseif rsp.op == ProtoEnumFriendModule.FriendOp.FriendOpRejectApply then
            PlatformFriendProxy:GetInstance():removeReiceiveFriendApplyData(rsp.player_id)
            ViewManager.close(UIViewEnum.Platform_Friend_Main_Page_View)
        elseif rsp.op == ProtoEnumFriendModule.FriendOp.FriendOpAddFriend then
            showFloatTips("已发送好友申请!")
            if PlatformFriendMainPageView ~= nil and PlatformFriendMainPageView:getIsOpen() then
                PlatformFriendMainPageView:setAlreadySent()
            end
        elseif rsp.op == ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend then
            local data = {
                op = ProtoEnumFriendModule.FriendOp.FriendOpReqList,
                param1 = 0,
                param2 = 100
            }
            PlatformFriendModule.onReqFriendOp(data)
        end
    elseif ProtoEnumFriendModule.FriendOpResult.FriendOpResultFail == result then
        showFloatTips("操作失败！")
    elseif ProtoEnumFriendModule.FriendOpResult.FriendOpResultAccountNotExist == result then
        showFloatTips("账号不存在！")
    elseif ProtoEnumFriendModule.FriendOpResult.FriendOpResultAlreadyExisted == result then
        showFloatTips("好友已经存在列表中！")
    elseif ProtoEnumFriendModule.FriendOpResult.FriendOpResultFriendNotExist == result then
        showFloatTips("好友不存在！")
    elseif ProtoEnumFriendModule.FriendOpResult.FriendOpResultErrAddSelf == result then
        showFloatTips("不能添加自己为好友！")
    end
    --todo 待优化那个莫名其妙的监听
    --NoticeManager.Instance:Dispatch(PlatformFriendType.Platform_Friend_Rsp_Friend_Op, rsp)
end

--通知申请添加好友成功
function this.onNotifyAgreeAddFriend(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "NotifyAgreeAddFriend", notice)
end

--通知有人加你为好友
function this.onNotifyAddFriendApply(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "NotifyAddFriendApply", notice)
    PlatformFriendProxy:GetInstance():insertReceiveAddFriendApplyData(rsp)
end

--通过手机号搜索好友操作返回
function this.onRspSearchPhone(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspFindUserByTelNo", notice)
    PlatformFriendProxy:GetInstance():setSearchFriendPhoneData(rsp)
    if PlatformSearchFriendView:getIsOpen() then
        PlatformSearchFriendView:updateSearchFriendPhoneResult()
    else
        ViewManager.open(UIViewEnum.PlatForm_Search_Friend_View)
    end
end

--已收到添加好友成功，把新的好友加入好友列表
function this.onNotifyAddFriendReceiver(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "NotifyAddFriend", notice)
    PlatformFriendProxy:GetInstance():addFriendListData(rsp)
    NoticeManager.Instance:Dispatch(PlatformFriendType.Receive_Update_Friend_List, rsp)
end

--请求发送好友聊天消息返回
function this.onRspSendFriendMsg(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspSendFriendChat", notice)

    if rsp.result == ProtoEnumFriendModule.SendFriendChatResult.SendFriendChatResultSuccess then
        local data = {}
        local currPersonalData = PlatformUserProxy:GetInstance():getUserInfo()
        data.player_id = LoginDataProxy.playerId
        data.msg_id = rsp.msg_id --FriendChatDataProxy.currChatMsgId
        data.msg = FriendChatDataProxy.currChatMsg
        data.time = FriendChatDataProxy.currChatTime
        data.chat_msg_type = FriendChatDataProxy.currChatType
        data.head_url = currPersonalData.head_url
        PlatformFriendProxy:GetInstance():addFriendChatData(data)
        --更新主页红点
        PlatformGlobalView:onUpdateChatOnlineMsgCount()

        --更新列表红点
        if PlatformFriendChatView:getIsOpen() then
            PlatformFriendChatView:updateFriendChatMsg()
        elseif PlatformGlobalMessageView:getIsOpen() then
            PlatformGlobalMessageView:updateMessagePanel()
        end
    elseif rsp.result == ProtoEnumFriendModule.SendFriendChatResult.SendFriendChatResultFailed then
        showFloatTips("发送消息失败！")
    elseif rsp.result == ProtoEnumFriendModule.SendFriendChatResult.SendFriendChatResultNotFriend then
        showFloatTips("发送对象不在你的好友里面")
    elseif rsp.result == ProtoEnumFriendModule.SendFriendChatResult.SendFriendChatResultBeDeleted then
        showFloatTips("您和对方不是好友关系，暂时不能进行聊天")
    elseif rsp.result == ProtoEnumFriendModule.SendFriendChatResult.SendFriendChatResultMsgOverLimit then
        showFloatTips("发送的消息长度超过了1024个字节")
    end
end

--请求确认收到好友聊天返回
function this.onRspConfirmChat(notice)
    printDebug(
        "=========================================================请求确认收到好友聊天消息返回成功================================================"
    )
    local rsp = this.decodeProtoBytes("friendmodule", "RspConfirmFriendChat", notice)

    --printDebug("++++++++++++++++++++++++收到确认收到好友聊天返回的table为：" .. table.tostring(rsp))
    --PlatformFriendProxy:GetInstance():addFriendChatData(rsp)
end

--通知好友聊天消息
function this.onNotifyFriendChat(notice)
    local data = this.decodeProtoBytes("friendmodule", "NotifyFriendChat", notice)
    local rsp = data.chat_info
    rsp.head_url = rsp.player_base_info.head_url
    PlatformFriendProxy:GetInstance():addFriendChatData(rsp)

    if PlatformFriendChatView:getIsOpen() then
        PlatformFriendChatView:updateFriendChatMsg()
    end

    --更新主页红点
    PlatformGlobalView:onUpdateChatOnlineMsgCount()

    --更新列表红点
    if PlatformGlobalMessageView:getIsOpen() then
        PlatformGlobalMessageView:updateMessagePanel()
    end

    local req = {}
    req.msg_id_list = {}
    table.insert(req.msg_id_list, rsp.msg_id)
    this.onReqConfirmChat(req)
end

--请求获取好友离线聊天信息返回
function this.onRspOfflineChat(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspOfflineFriendChat", notice)
    PlatformFriendProxy:GetInstance():addFriendChatOfflineData(rsp)
    --更新主页红点
    PlatformGlobalView:onUpdateChatOnlineMsgCount()
    --更新列表红点
    if PlatformGlobalMessageView:getIsOpen() then
        PlatformGlobalMessageView:updateMessagePanel()
    end

    local req = {}
    req.msg_id_list = {}
    if not table.empty(rsp.chat_info_list) then
        for i = 1, #rsp.chat_info_list do
            table.insert(req.msg_id_list, rsp.chat_info_list[i].msg_id)
        end
    end
    if #req.msg_id_list == 0 then
        return
    end
    this.onReqConfirmOfflineChat(req)
end

--主页点击了聊天按钮后调用该通知
function this.onNotifyOnOrOffMsg(notice)
    ViewManager.open(UIViewEnum.Platform_Friend_Chat_List_View)
end

--取消红点
function this.onCancelOnOrOffRedPoint(notice)
    local req = notice
    PlatformFriendProxy:GetInstance():setCurrChatFriendDataReaded(req.playerId)

    --更新主页红点
    PlatformGlobalView:onUpdateChatOnlineMsgCount()

    --更新列表红点
    if PlatformGlobalMessageView:getIsOpen() then
        PlatformGlobalMessageView:updateMessagePanel()
    end
end

--收到附近好友推荐
function this.onReceiveRecommend(notice)
    local rsp = this.decodeProtoBytes("friendmodule", "RspFriendRecommend", notice)
    PlatformFriendProxy:GetInstance():setRecommendFriendData(rsp)
    NoticeManager.Instance:Dispatch(PlatformFriendType.Friend_RecommendSuccess)
end

-- 显示推荐好友
function this.onRecommendationOfFriend(notice)
    PlatformGlobalRecommendationOfFriendView:updateRecommendationOfFriend(notice)
end

-- 关闭推荐好友
function this.onCloseRecommendationOfFriend(notice)
    PlatformGlobalRecommendationOfFriendView:recommendationFriendCloseImg()
end

--------------------------------------------发送消息(客户端发出)-------------------------------
--请求好友操作
function this.onReqFriendOp(notice, addFriendData)
    local data = notice
    if data.op == ProtoEnumFriendModule.FriendOp.FriendOpAddFriend then
        if PlatformFriendProxy:GetInstance():isMyAddFriendById(data.player_id) then
            return
        end
        PlatformFriendProxy:GetInstance():setSendAddFriendApplyData(addFriendData)
    end
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqFriendOp", data)
end

--通过手机号搜索好友操作
function this.onReqSearchPhone(notice)
    local data = notice
    printDebug("发送给服务器的通过手机号搜索好友操作为：" .. table.tostring(data))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqFindUserByTelNo", data)
end

--请求发送好友聊天消息
function this.onReqSendFriendMsg(notice)
    local data = notice
    printDebug("发送给服务器的好友聊天消息为：" .. table.tostring(data))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqSendFriendChat", data)
end

--请求确认收到好友聊天消息
function this.onReqConfirmChat(notice)
    local data = notice
    printDebug("发送给服务器的请求确认收到好友聊天消息为：" .. table.tostring(data))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqConfirmFriendChat", data)
end

--请求获取好友离线聊天信息
function this.onReqOfflineChat(notice)
    local data = notice
    printDebug("发送给服务器的请求获取好友离线聊天信息为：" .. table.tostring(data))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqOfflineFriendChat", data)
end

--请求确认收到好友离线聊天消息
function this.onReqConfirmOfflineChat(notice)
    local data = notice
    printDebug("发送给服务器的请求确认收到好友离线聊天消息为：" .. table.tostring(data))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqConfirmFriendChat", data)
end

--请求附近好友推荐
function this.onSendRecommend(notice)
    local data = notice
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "friendmodule", "ReqFriendRecommend", data)
end