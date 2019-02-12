require "base:module/message/data/PlatformMessageProxy"

PlatformMessageModule = BaseModule:new()
local this = PlatformMessageModule
this.moduleName = "PlatformMessage"

--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
    this.netFuncList = {}
    -- 请求获取邮件返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetMailList, this.onRspGetMail)
    -- 请求获取邮件奖励返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspMailAttach, this.onRspMailAttach)
    -- 通知用户有新邮件（在线）
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyNewMail, this.onNotifyNewMail)
    --通知用户消息
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyUserMsg, this.onNotifyUserMsg)
    --请求确认收到用户通知消息返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspConfirmUserMsg, this.onRspConfirmUserMsg)
    --请求获取用户通知消息返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetUserNotifyMsg, this.onRspGetUserNotifyMsg)
    --请求设置用户通知消息状态返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspSetUserNotifyStatus, this.onRspSetUserNotifyStatus)
    --删除邮件返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspDelMail, this.onRspDelMail)
    -- 请求领取所有邮件的物品返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetAllMailItem, this.onRspGetAllMailItem)
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
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Req_Get_Mail, this.onReqGetMail)
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Rsp_Get_Mail, this.onRspGetMailData)
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Req_Mail_Attach, this.onReqMailAttach)
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Rsp_Mail_Attach, this.onRspMailAttachData)
        self:AddNotifictionLister(NoticeType.Logout, this.onNotifyLogout)
    end
    return self.notificationList
end

------------------------------发协议------------------------------
--请求领取所有邮件的物品
function this.onReqGetAllMailItem()
    local req = {}
    req.player_id = LoginDataProxy.playerId
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetAllMailItem", req)
end
--请求获取邮件列表
function this:onReqGetMail(notice)
    local req = notice:GetObj()
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetMailList", req)
end

-- 请求获取邮件奖励
function this:onReqMailAttach(notice)
    local req = notice:GetObj()
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqMailAttach", req)
end
-- 请求删除邮件
function this.onReqDelMail(notice)
    local req = notice
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqDelMail", req)
end
--请求确认收到用户通知消息
function this.sendReqConfirmUserMsg(save_id)
    local req = {}
    req.save_id = save_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqConfirmUserMsg", req)
end

--请求获取用户通知消息
function this.sendReqGetUserNotifyMsg()
    local req = {}
    req.page_index = 0
    req.per_page_num = 100
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetUserNotifyMsg", req)
end

--请求设置用户通知消息状态
function this.sendReqSetUserNotifyStatus(status, id_list)
    local req = {}
    req.status = status
    req.id_list = id_list
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqSetUserNotifyStatus", req)
end

--------------------------------------------收到协议(服务端发出)-------------------------------
-- 请求领取所有邮件的物品返回
function this.onRspGetAllMailItem(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetAllMailItem", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
    end
end

-- 通知用户有新邮件(在线)
function this.onNotifyNewMail(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "NotifyNewMail", protoBytes)

    if rsp.mail_type == ProtoEnumCommon.MailType.MailType_Normal then
        PlatformMessageProxy.insertMailData(rsp.mail_info)
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Update_Mail_Data)
    elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Redpacket then
        PlatformNewRedBagProxy:GetInstance():insertRedPacketMailList(rsp.mail_info)
        PlatformGlobalRedBagView:updateRedBagMail()
    end
end

-- 请求获取邮件返回
function this.onRspGetMail(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetMailList", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Get_Mail, rsp)
end

-- 请求获取邮件奖励返回
function this.onRspMailAttach(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspMailAttach", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Mail_Attach, rsp)
end

-- 通知用户消息
function this.onNotifyUserMsg(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "NotifyUserMsg", protoBytes)
    --PlatformMessageProxy.setUserMsgData(rsp.notify_msg)
    this.sendReqGetUserNotifyMsg()
end

-- 请求确认收到用户通知消息返回
function this.onRspConfirmUserMsg(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspConfirmUserMsg", protoBytes)
    this.sendReqGetUserNotifyMsg()
end

-- 请求获取用户通知消息返回
function this.onRspGetUserNotifyMsg(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetUserNotifyMsg", protoBytes)
    PlatformMessageProxy.setUserMsgData(rsp.notify_msg_list)
end

-- 请求设置用户通知消息状态返回
function this.onRspSetUserNotifyStatus(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspSetUserNotifyStatus", protoBytes)
    this.sendReqGetUserNotifyMsg()
end
-- 删除邮件返回
function this.onRspDelMail(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspDelMail", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		showFloatTips("删除成功")
        if rsp.mail_type == ProtoEnumCommon.MailType.MailType_Normal then
            PlatformMessageProxy.delMailData(rsp.mail_id)
            NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Update_Mail_Data)
        elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Redpacket then
            PlatformNewRedBagProxy:GetInstance():delRedPacketMailList(rsp.mail_id)
            PlatformGlobalRedBagView:updateRedBagMail()
        end
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Del_Mail_Data)
    end
end

--------------------------------------------收到消息------------------------------
-- 请求获取邮件返回信息
function this:onRspGetMailData(notice)
    local rsp = notice:GetObj()

    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        -- 成功
        if rsp.total_count > rsp.per_page_count * (rsp.page_index + 1) then
            local tab = {}
            tab.mail_type = rsp.mail_type
            tab.page_index = rsp.page_index + 1
            tab.per_page_count = rsp.per_page_count
            NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Mail, tab)
        end
        --现只有红包邮件和普通邮件
        if rsp.mail_type == ProtoEnumCommon.MailType.MailType_Normal then
            PlatformMessageProxy.setNormalMailData(rsp.page_index, rsp.mail_info_list)
            NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Update_Mail_Data)
        elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Redpacket then
            PlatformNewRedBagProxy:GetInstance():setRedPacketMailList(rsp.page_index, rsp.mail_info_list)
            PlatformGlobalRedBagView:updateRedBagMail()
        elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Max then
        end
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFail then
        -- 失败
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailNoFound then
    -- 没有找到
    end
end

-- 请求获取邮件奖励返回
function this:onRspMailAttachData(notice)
    local rsp = notice:GetObj()
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        -- 成功
        if rsp.mail_type == ProtoEnumCommon.MailType.MailType_Normal then
            --PlatformMessageProxy.delMailinfoByID(rsp.mail_id)
        elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Redpacket then
            PlatformGlobalRedBagView:updateRedBagMail(rsp.mail_id)
        elseif rsp.mail_type == ProtoEnumCommon.MailType.MailType_Max then
        end
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFail then
        -- 失败
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailNoFound then
    -- 没有找到
    end
end

function this.onNotifyLogout()
    PlatformMessageProxy.initData()
end

-------------------------------------------------------
