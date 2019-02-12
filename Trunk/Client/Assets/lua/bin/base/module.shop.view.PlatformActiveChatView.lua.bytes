require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_active_chatroom_panel"
require "base:enum/PlatformFriendType"

PlatformActiveChatView = BaseView:new()
local this = PlatformActiveChatView
this.viewName = "PlatformActiveChatView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Active_Chat_View, true)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_active_chatroom_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

this.ActivityData = nil
function this:onShowHandler()
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
    this:InitData()
    this.onFlushChatPanel()
end

function this:InitData()
    this.currActivityData = PlatformLBSDataProxy.getActivitySingleData()
    this.currAct2ShopData = PlatformLBSDataProxy.getRedBagShopDataByShopId(this.currActivityData.shop_id)
    this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
    this.chatPanelScorllPanelRect = this.main_mid.chatCellRecycleScrollPanel.rectTransform
    --向服务器发送进入房间请求
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op,
        {
            chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
            channel_id = this.currActivityData.active_id,
            op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Join
        }
    )
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    if table.empty(this.currActivityData) == false then
        --向服务器发送退出聊天室请求
        NoticeManager.Instance:Dispatch(
            PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op,
            {
                chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
                channel_id = this.currActivityData.active_id,
                op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Leave
            }
        )
    end
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
end

function this:addEvent()
    this.main_mid.chat_back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_Chat_View)
        end
    )
    self.main_mid.chat_face_Button:AddEventListener(UIEvent.PointerClick, self.chatFaceOpenBtnEvent)
    self.main_mid.chat_photo_Button:AddEventListener(UIEvent.PointerClick, self.chatImputOpenBtnEvent)

    self.main_mid.chatImput_InputField:OnEndEdit(self.sendChatMsg)

    self.main_mid.bottom_picture_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.onBtnPicture(false)
        end
    )

    self.main_mid.bottom_camera_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.onBtnPicture(true)
        end
    )
    -- self.main_mid.mask_Image:AddEventListener(UIEvent.PointerClick, self.onBtnMaskEvent)
end

--打开弹出下框
function this:chatImputOpenBtnEvent(eventData)
    --临时屏蔽下拉框
    if true then
        return this.main_mid.chatImput_InputField.inputField:ActivateInputField()
    end
    this.DownPopShowPage(not this.main_mid.bottom.gameObject.activeSelf)
    if not this.main_mid.bottom.gameObject.activeSelf then
        this.main_mid.bottom.gameObject:SetActive(true)
        this.main_mid.face.gameObject:SetActive(false)
        this.main_mid.chat_face_Icon:ChangeIcon(0)
    else
        this.main_mid.bottom.gameObject:SetActive(false)
        this.main_mid.chatImput_InputField.inputField:ActivateInputField()
    end
end

--打开表情弹出下框
function this:chatFaceOpenBtnEvent(eventData)
    this.faceData = PlatformPicManagerProxy:GetInstance():getFaceKeyValue()
    if this.faceData ~= nil then
        this.main_mid.face_CellGroup:SetCellData(this.faceData, this.updateFaceIcon)
    end
    this.DownPopShowPage(not this.main_mid.face.gameObject.activeSelf)
    if not this.main_mid.face.gameObject.activeSelf then
        this.main_mid.bottom.gameObject:SetActive(false)
        this.main_mid.face.gameObject:SetActive(true)
        this.main_mid.chat_face_Icon:ChangeIcon(1)
    else
        this.main_mid.face.gameObject:SetActive(false)
        this.main_mid.chat_face_Icon:ChangeIcon(0)
        this.main_mid.chatImput_InputField.inputField:ActivateInputField()
    end
end

function this.DownPopShowPage(isPlay)
    local mySequence1 = DOTween.Sequence()
    local move1 = nil
    local tempLocalPosY = this.main_mid.chat.transform.localPosition.y
    if isPlay then
        this.chatPanelScorllPanelRect.offsetMax = Vector2(this.chatPanelScorllPanelRect.offsetMax.x, -644)
        move1 = this.main_mid.chat.transform:DOLocalMove(Vector3(0, 400, 0), 0.2)
    else
        this.chatPanelScorllPanelRect.offsetMax = Vector2(this.chatPanelScorllPanelRect.offsetMax.x, -244)
        move1 = this.main_mid.chat.transform:DOLocalMove(Vector3(0, 0, 0), 0.2)
    end
    this.main_mid.chatCellRecycleScrollPanel:SetToContentBottom()
    mySequence1:Append(move1)
end

--发送图片
function this.onBtnPicture(isPhotograph)
    PlatformSDK.takePhonePhoto(
        isPhotograph,
        function(bytes)
            --上传照片
            CommonUploadView:activeUpdateTip(true, "上传中")
            this.pngName = string.format("%s/%s", ImageType.Face, getShortUUID())
            uploadImage(
                this.pngName,
                bytes,
                ImageType.Face,
                function(isSucceed)
                    if isSucceed then
                        CommonUploadView:activeUpdateTip(false, "上传图片成功")
                        local chatMsg = {
                            chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture,
                            msg = this.pngName,
                            time = os.time()
                        }
                        NoticeManager.Instance:Dispatch(
                            PlatformGlobalNoticeType.Platform_Req_Send_Chat,
                            {
                                chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
                                channel_id = this.currActivityData.active_id,
                                chat_info = chatMsg
                            }
                        )
                    else
                        CommonUploadView:activeUpdateTip(false, "上传图片失败")
                    end
                end
            )
        end,
        true,
        1024,
        1024
    )
end

function this.sendChatMsg()
    --发送文字
    if this.main_mid.chatImput_InputField.text ~= "" then
        local chatMsg = {
            player_id = LoginDataProxy.playerId,
            -- msg_id = getUUID(),
            chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Text,
            msg = this.main_mid.chatImput_InputField.text,
            time = os.time()
        }
        if DisableTermsManager.Instance:IsMatch(chatMsg.msg) then
            Alert.showAlertMsg(nil, "您输入的文字包含违规内容，请修改后再尝试", "确定")
        else
            NoticeManager.Instance:Dispatch(
                PlatformGlobalNoticeType.Platform_Req_Send_Chat,
                {
                    chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
                    channel_id = this.currActivityData.active_id,
                    chat_info = chatMsg
                }
            )
            this.main_mid.chatImput_InputField.text = ""
        end
    end
end
--广播聊天消息
function this.onNotifySendChat(notice, rsp)
    local req = rsp:GetObj()
    PlatformGlobalProxy:GetInstance():addChatNotify(tostring(req.channel_id), req)

    this.onFlushChatPanel()
end
this.currShopNotifyData = nil
this.ChatCellDataByTypeFunction = {}

this.headUrlByPlayerId = {}
--更新聊天室界面
function this.onFlushChatPanel()
    this.currShopNotifyData =
        PlatformGlobalProxy:GetInstance():getChatNotify(tostring(this.currActivityData.active_id)) or {}
    this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()

    this.currAct2ShopData = PlatformLBSDataProxy.getRedBagShopDataByShopId(this.currActivityData.shop_id)
    printDebug("+++++++++++++++++++++++lalalallalalla+++++++++this.currShopNotifyData = "..table.tostring(this.currShopNotifyData))
    if this.currActivityData == nil or this.currAct2ShopData == nil then
        return
    end
    --保证头像是最新的
    for i = 1, #this.currShopNotifyData do
        local info = this.currShopNotifyData[i].chat_info.player_base_info
        if info.player_id ~= this.currGlobalBaseData.player_id then
            this.headUrlByPlayerId[info.player_id] = info.head_url
        end
    end
    this.main_mid.chatCellRecycleScrollPanel:SetCellData(this.currShopNotifyData, this.updateChat, true)
    this.main_mid.chatCellRecycleScrollPanel:SetToContentBottom()
end

this.chatCellOtherPictureNameTab = {}
this.chatCellHeadPicTab = {}

local baseHeight = 88
local limitWidth = 816
--#region 更新聊天相关
---文本消息
function this.TextUpdateChatMsgFunction(item, userInfo, msgInfo, data, index, isSelf, dataIndex, timeImageOffsetHeight)
    local tempImageRect, tempTextRect, preferredWidth, preferredHeight

    if isSelf then
        item.self_chat_bg_Image.gameObject:SetActive(true)
        item.self_chat_Text.text = msgInfo.msg
        item.self_Image.gameObject:SetActive(false)
        tempImageRect = item.self_chat_bg_Image.rectTransform
        tempTextRect = item.self_chat_Text.rectTransform
        preferredWidth = item.self_chat_Text.Txt.preferredWidth
        preferredHeight = math.ceil(preferredWidth / (limitWidth - 60)) * 51 --由TEXT宽算出实际高度
        item.self_name_Text.text = userInfo.nick_name
    else
        item.other_chat_bg_Image.gameObject:SetActive(true)
        item.other_self_chat_Text.text = msgInfo.msg
        item.other_Image.gameObject:SetActive(false)

        tempImageRect = item.other_chat_bg_Image.rectTransform
        tempTextRect = item.self_chat_Text.rectTransform
        preferredWidth = item.other_self_chat_Text.Txt.preferredWidth
        preferredHeight = math.ceil(preferredWidth / (limitWidth - 60)) * 52 --由TEXT宽算出实际高度
    end

    preferredWidth = (preferredWidth + 60) > limitWidth and limitWidth or (preferredWidth + 60)
    preferredHeight = (preferredHeight + 60) < (36 + 10 + 60) and (36 + 10 + 60) or (preferredHeight + 60)
    preferredHeight = math.ceil(preferredHeight)
    preferredWidth = math.ceil(preferredWidth)
    tempImageRect.sizeDelta = Vector2(preferredWidth, preferredHeight)
    tempTextRect.sizeDelta = Vector2(preferredWidth - 60, preferredHeight - 60)
    local moveChatImage = (preferredHeight - baseHeight) > 0 and (preferredHeight - baseHeight) / 2 or 0
    tempImageRect.transform.localPosition = Vector3(tempImageRect.transform.localPosition.x, -moveChatImage, 0)
    this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(
        dataIndex + 1,
        preferredHeight + timeImageOffsetHeight + 130
    )
end

---图片消息
function this.PictureUpdateChatMsgFuction(item, userInfo, msgInfo, data, index, isSelf, dataIndex, timeImageOffsetHeight)
    this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, 300)
    if isSelf then
        item.self_chat_bg_Image.gameObject:SetActive(false)
        item.self_Image.gameObject:SetActive(true)
        if data.chat_info.msg ~= this.chatCellOtherPictureNameTab[index] then
            downloadImage(msgInfo.msg, item.self_Image)
            this.chatCellOtherPictureNameTab[index] = data.chat_info.msg
        end
        item.self_Image.gameObject:SetActive(true)
    else
        item.other_chat_bg_Image.gameObject:SetActive(false)
        item.other_Image.gameObject:SetActive(true)
        if msgInfo.msg ~= this.chatCellOtherPictureNameTab[index] then
            downloadImage(msgInfo.msg, item.other_Image)
            this.chatCellOtherPictureNameTab[index] = msgInfo.msg
        end
    end
end
---现金红包消息
function this.CashRedpacketUpdateChatMsgFuction(
    item,
    userInfo,
    msgInfo,
    data,
    index,
    isSelf,
    dataIndex,
    timeImageOffsetHeight)
    this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, 300)
    if not isSelf then
        item.other_chat_bg_Image.gameObject:SetActive(false)
        item.other_Image.gameObject:SetActive(false)
        local rsp = ProtobufManager.decode("common", "MsgActiveCashRedPacketSt", msgInfo.msg)
        item.other_pray_Text.text = rsp.redpacket_name
        item.redbag.gameObject:SetActive(true)
        local msg = {}
        msg.activeId = rsp.active_id
        msg.redpacketId = rsp.red_packet_id
        msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
        msg.isFromChat = true
        msg.isCoupon = false
        msg.headUrl = msgInfo.player_base_info.head_url
        msg.name = userInfo.nick_name
        msg.title = rsp.redpacket_name
        msg.describe = rsp.discribe
        msg.describeImageList = rsp.imgs
        msg.packetStyle = rsp.red_packer_style
        item.regbag_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                PlatformRedpacketModule.sendReqActiveCashRedPacketState(msg.redpacketType, msg.activeId, msg.redpacketId)
                --PlatformRedpacketModule.sendReqRcvActiveCashRedPacket(msg.redpacketType, msg.activeId, msg.redpacketId)
                printDebug("+++++++++++++++设置红包数据msg = "..table.tostring(msg))
                PlatformRedPacketProxy.SetOpenLBSPacketData("ChatRoom_RedPacket_Open_Data", msg)
            end
        )
    end
end
---卡卷红包消息
function this.CouponUpdateChatMsgFuction(item, userInfo, msgInfo, data, index, isSelf, dataIndex, timeImageOffsetHeight)
    this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, 300)
    if not isSelf then
        item.other_chat_bg_Image.gameObject:SetActive(false)
        item.other_Image.gameObject:SetActive(false)
        item.redbag.gameObject:SetActive(true)
        local rsp = ProtobufManager.decode("common", "MsgActiveCouponRedPacketSt", msgInfo.msg)
        item.other_pray_Text.text = rsp.redpacket_name
        local msg = {}
        msg.activeId = rsp.active_id
        msg.redpacketId = rsp.red_packet_id
        msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
        msg.isFromChat = true
        msg.coupon_id = rsp.coupon_id
        msg.isCoupon = true
        msg.headUrl = userInfo.head_url
        msg.name = userInfo.nick_name
        msg.title = rsp.redpacket_name
        msg.describe = rsp.discribe
        item.regbag_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                PlatformRedpacketModule.sendReqReceiveActiveCouponRedpacket(
                    msg.redpacketType,
                    msg.activeId,
                    msg.redpacketId,
                    msg.coupon_id
                )
                PlatformRedPacketProxy.SetOpenLBSPacketData("ChatRoom_RedPacket_Open_Data", msg)
            end
        )
    end
end

--#region end 更新聊天相关

local ChatMsgType = {}

ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Text] = this.TextUpdateChatMsgFunction ---文本消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture] = this.PictureUpdateChatMsgFuction ---图片消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Redpacket] = this.CashRedpacketUpdateChatMsgFuction ---现金红包消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Coupon] = this.CouponUpdateChatMsgFuction ---卡卷红包消息

--	如果是用户本人发出的消息
function this.UpdateSelfPlayer(item, userInfo, msgInfo, data, index, dataIndex, timeImageOffsetHeight)
    downloadUserHead(this.currGlobalBaseData.head_url, item.self_head_Image)
    local str = userInfo.nick_name
    item.self_name_Text.text = str
    return ChatMsgType[msgInfo.chat_msg_type](
        item,
        userInfo,
        msgInfo,
        data,
        index,
        true,
        dataIndex,
        timeImageOffsetHeight
    )
end

--	如果是其它人发出的消息
function this.UpdateOtherPlayer(item, userInfo, msgInfo, data, index, dataIndex, timeImageOffsetHeight)
    local headUrl = this.headUrlByPlayerId[userInfo.player_id] or userInfo.head_url
    downloadUserHead(headUrl, item.other_head_Image)
    local strNameType = msgInfo.player_id == this.currAct2ShopData.player_id and "一一一   " or ""
    item.name_type.gameObject:SetActive(msgInfo.player_id == this.currAct2ShopData.player_id)
    local str = string.concat(strNameType, userInfo.nick_name)
    item.other_name_Text.text = str
    return ChatMsgType[msgInfo.chat_msg_type](
        item,
        userInfo,
        msgInfo,
        data,
        index,
        false,
        dataIndex,
        timeImageOffsetHeight
    )
end

--更新聊天内容
function this.updateChat(go, data, index, dataIndex)
    local item = nil
    item = this.main_mid.chatCellArr[index + 1]
    local userInfo = data.user_base_info
    local msgInfo = data.chat_info
    item.time_Text.text = os.date("%H:%M", msgInfo.time)
    local timeImageOffsetHeight = 40 --有时间时，增加40的高度
    --两分钟合并
    printDebug("+++++++++++++++聊天内容data ="..table.tostring(data))
    if this.currShopNotifyData[dataIndex] then
        local span = msgInfo.time - this.currShopNotifyData[dataIndex].chat_info.time
        if span <= 120 then
            item.time_bg.gameObject:SetActive(false)
            timeImageOffsetHeight = 0
        else
            item.time_bg.gameObject:SetActive(true)
        end
    else
        item.time_bg.gameObject:SetActive(true)
    end
    local b = msgInfo.player_id == LoginDataProxy.playerId
    item.self.gameObject:SetActive(b)
    item.other.gameObject:SetActive(not b)
    item.redbag.gameObject:SetActive(false)
    if b then --test
        return this.UpdateSelfPlayer(item, userInfo, msgInfo, data, index, dataIndex, timeImageOffsetHeight)
    else
        return this.UpdateOtherPlayer(item, userInfo, msgInfo, data, index, dataIndex, timeImageOffsetHeight)
    end
end
--空白区域撤回底框
function this:onBtnMaskEvent()
    this.DownPopShowPage(not this.main_mid.face.gameObject.activeSelf)
end
