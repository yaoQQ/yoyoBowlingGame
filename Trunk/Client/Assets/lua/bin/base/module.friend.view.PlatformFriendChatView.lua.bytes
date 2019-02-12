require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_chat_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformFriendChatView = BaseView:new()
local this = PlatformFriendChatView
this.viewName = "PlatformFriendChatView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Friend_Chat_View, true)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_chat_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    --printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    --printDebug("=====================PlatForm_Friend_Chat_View调用完毕======================")
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
    this:updateFriendChatMsg()

    self:addNotice()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
    --printDebug("+++++++++++++++++注册了语音监听！")
    NoticeManager.Instance:AddNoticeLister(NoticeType.Record_End, this.onRecordEnd)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Record_End, this.onRecordEnd)
end

this.pngName = nil
this.faceData = nil
this.voiceFillAmount = nil
this.voiceFlag = false
--是否为取消发送状态
this.voiceCanel = false
--是否发送
this.voiceSend = true
local startTime = 0
function this:addEvent()
    -- this.main_mid.chatImput_InputField:SetIconList(this.faceData)

    self.main_mid.friendchat_back_Image:AddEventListener(UIEvent.PointerClick, self.onBtnBackPress)

    self.main_mid.chatImput_InputField:OnEndEdit(self.onBtnSendPress)

    self.main_mid.chat_face_Button:AddEventListener(UIEvent.PointerClick, self.chatFaceOpenBtnEvent)

    self.main_mid.chat_photo_Button:AddEventListener(UIEvent.PointerClick, self.chatImputOpenBtnEvent)

    self.main_mid.friend_config_Image:AddEventListener(UIEvent.PointerClick, self.chatConfigBtnEvent)

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

    self.main_mid.change_chat_type_Icon:AddEventListener(UIEvent.PointerClick, self.onBtnChangeIcon)

    self.main_mid.chat_voice_Button:AddEventListener(UIEvent.PointerDown, self.onBtnVoiceDown)

    self.main_mid.chat_voice_Button:AddEventListener(UIEvent.PointerUp, self.onBtnVoiceUp)
    self.main_mid.cancel_Button:AddEventListener(UIEvent.PointerEnter, self.onBtnCancel)
    self.main_mid.cancel_Button:AddEventListener(UIEvent.PointerExit, self.onBtnCancelExit)
    -- self.main_mid.mask_Image:AddEventListener(UIEvent.PointerClick, self.onBtnMaskEvent)

    this.chatPanelScorllPanelRect = this.main_mid.chatCellRecycleScrollPanel.rectTransform
end
--发送语音协议
function this.onRecordEnd(arg1, arg2)
    -- body
    if not this.voiceSend and this.voiceCanel then
        return
    end
    local req = arg2:GetObj()
    local msgData = {}
    msgData.url = req
    msgData.time = math.ceil(startTime)
    if startTime < 1 then
        return
    end
    local msgSend = BaseModule.encodeProtoBytes("common", "MsgChatAudioInfo", msgData)
    local chatMsg = {
        chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Audio,
        msg = msgSend,
        time = os.time()
    }

    FriendChatDataProxy.currChatMsg = chatMsg.msg
    FriendChatDataProxy.currChatTime = os.time()
    FriendChatDataProxy.currChatType = ProtoEnumCommon.ChatMsgType.ChatMsgType_Audio

    PlatformFriendModule.onReqSendFriendMsg({player_id = FriendChatDataProxy.currChatFriendId, chat_info = chatMsg})
end
--更新表情
function this.updateFaceIcon(go, data, index)
    local item = this.main_mid.faceGroupCellArr[index + 1]

    item.press_Image.name = data.name
    downloadUserHead(data.sprite, item.press_Image)

    -- item.press_Image:SetPng(data.sprite
    item.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            local obj = eventData.pointerPress
            --printDebug("点击了表情，表情的名字为：" .. obj.name)
            this.main_mid.chatImput_InputField.text = this.main_mid.chatImput_InputField.text .. obj.name
        end
    )
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

--点击了退出聊天界面按钮
function this.onBtnBackPress(eventData)
    ViewManager.close(UIViewEnum.Platform_Friend_Chat_View)
end

--点击了发送按钮
function this.onBtnSendPress(eventData)
    if this.main_mid.chatImput_InputField.text == "" then
        return
    end

    local msg = {}

    msg.msg = this.main_mid.chatImput_InputField.text
    msg.chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Text
    FriendChatDataProxy.currChatMsg = msg.msg
    FriendChatDataProxy.currChatTime = os.time()
    FriendChatDataProxy.currChatType = ProtoEnumCommon.ChatMsgType.ChatMsgType_Text

    local req = {}
    req.player_id = FriendChatDataProxy.currChatFriendId
    req.chat_info = msg

    if DisableTermsManager.Instance:IsMatch(msg.msg) then
        Alert.showAlertMsg(nil, "您输入的文字包含违规内容，请修改后再尝试", "确定")
    else
        PlatformFriendModule.onReqSendFriendMsg(req)
        this.main_mid.chatImput_InputField.text = ""
    end
end

this.currFriendChatMsg = nil
this.headUrlByPlayerId = {}
--更新在线的和离线的聊天消息
function this:updateFriendChatMsg()
    this.currFriendChatMsg =
        PlatformFriendProxy:GetInstance():getCurrChatFriendData(FriendChatDataProxy.currChatFriendId)

    this.curChatPlayerData = PlatformFriendProxy:GetInstance():getFriendDataById(FriendChatDataProxy.currChatFriendId)
    if this.curChatPlayerData == nil then
        return ViewManager.close(UIViewEnum.Platform_Friend_Chat_View)
    end
    self.main_mid.friendname_Text.text = this.curChatPlayerData.player_base_info.nick_name

    if this.currFriendChatMsg == nil then
        return self.main_mid.chatCellRecycleScrollPanel:SetCellData({}, self.updateFriendChatPanel, true)
    end
    local req = {}
    req.playerId = this.currFriendChatMsg.playerId
    PlatformFriendModule.onCancelOnOrOffRedPoint(req)
    --保证头像是最新的
    for i = 1, #this.currFriendChatMsg.my_chat_info do
        local info = this.currFriendChatMsg.my_chat_info[i]
        if tonumber(info.player_id) ~= tonumber(this.currGlobalBaseData.player_id) then
            this.headUrlByPlayerId[info.player_id] = info.head_url
        end
    end
    self.main_mid.chatCellRecycleScrollPanel:SetCellData(
        this.currFriendChatMsg.my_chat_info,
        self.updateFriendChatPanel,
        true
    )
    self.main_mid.chatCellRecycleScrollPanel:SetToContentBottom()
end

this.chatCellOtherPictureNameTab = {}
this.currVoice = nil
this.isCurrVoicePress = false
this.voiceTable = {}
local baseHeight = 88
local limitWidth = 816
--收到消息
function this.updateFriendChatPanel(go, data, index, dataIndex)
    local item = this.main_mid.chatCellArr[index + 1]
    local tempImageRect, tempTextRect, tempwidth, tempHeight
    item.time_Text.text = os.date("%H:%M", data.time)
    local timeImageOffsetHeight = 40 --有时间时，增加40的高度
    --两分钟合并
    if this.currFriendChatMsg.my_chat_info[dataIndex] then
        local span = data.time - this.currFriendChatMsg.my_chat_info[dataIndex].time
        if span <= 120 then
            item.time_bg.gameObject:SetActive(false)
            timeImageOffsetHeight = 0
        else
            item.time_bg.gameObject:SetActive(true)
        end
    else
        item.time_bg.gameObject:SetActive(true)
    end
    item.redbag.gameObject:SetActive(false)
    if tonumber(data.player_id) == tonumber(LoginDataProxy.playerId) then --	如果是用户本人发出的消息
        item.self.gameObject:SetActive(true)
        item.other.gameObject:SetActive(false)
        local str = data.nick_name
        item.self_name_Text.text = str
        downloadUserHead(this.currGlobalBaseData.head_url, item.self_head_Image)
        if data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Text then
            local tempImageRect, tempTextRect, preferredWidth, preferredHeight

            item.self_chat_bg_Image.gameObject:SetActive(true)
            item.self_chat_Text.text = data.msg
            item.self_Image.gameObject:SetActive(false)
            tempImageRect = item.self_chat_bg_Image.rectTransform
            tempTextRect = item.self_chat_Text.rectTransform
            preferredWidth = item.self_chat_Text.Txt.preferredWidth
            preferredHeight = math.ceil(preferredWidth / (limitWidth - 60)) * 52 --由TEXT宽算出实际高度
            item.self_name_Text.text = data.nick_name

            preferredWidth = (preferredWidth + 60) > limitWidth and limitWidth or (preferredWidth + 60)
            preferredHeight = (preferredHeight + 60) < (106) and (106) or (preferredHeight + 60)
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
        elseif data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture then
            this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, 300)
            item.self_chat_bg_Image.gameObject:SetActive(false)

            if data.msg ~= this.chatCellOtherPictureNameTab[index] then
                downloadUserHead(data.msg, item.self_Image)
                this.chatCellOtherPictureNameTab[index] = data.msg
            end
            item.self_Image.gameObject:SetActive(true)
            item.self_voice_Image.gameObject:SetActive(false)
        elseif data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Audio then
            item.self_chat_bg_Image.gameObject:SetActive(false)
            item.self_Image.gameObject:SetActive(false)
            item.self_voice_Image.gameObject:SetActive(true)
            local dataGet = BaseModule.decodeProtoBytes("common", "MsgChatAudioInfo", data.msg)
            tempImageRect = item.self_voice_Image.transform:GetComponent(typeof(RectTransform))
            tempWidth = item.self_voice_Image.Img.preferredWidth + 20 * dataGet.time
            tempHeight = item.self_voice_Image.Img.preferredHeight
            tempImageRect.sizeDelta = Vector2(tempWidth > 690 and 690 or tempWidth, tempHeight)
            this.voiceTable[tostring(index + 1)] = dataGet.url
            item.self_voice_Image.name = tostring(index + 1)
            item.self_roarer_Text.text = dataGet.time

            item.self_voice_Image:AddEventListener(
                UIEvent.PointerClick,
                function(eventData)
                    local obj = eventData.pointerPress

                    if obj == nil then
                        return
                    end
                    AudioManager.stopPlayRecord()
                    if this.isCurrVoicePress then
                        this.isCurrVoicePress = not this.isCurrVoicePress
                        return
                    end

                    this.isCurrVoicePress = not this.isCurrVoicePress

                    local msg = this.voiceTable[obj.name]

                    local dataTable = string.split(msg, "|")

                    AudioManager.startPlayRecordByFilePath(dataTable[4])
                end
            )
        end
    else
        item.self.gameObject:SetActive(false)
        item.other.gameObject:SetActive(true)
        local headUrl = this.headUrlByPlayerId[data.player_id] or data.head_url
        downloadUserHead(headUrl, item.other_head_Image)
        item.other_name_Text.text = data.nick_name
        if data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Text then
            item.other_chat_bg_Image.gameObject:SetActive(true)
            item.other_self_chat_Text.text = data.msg
            item.other_Image.gameObject:SetActive(false)

            tempImageRect = item.other_chat_bg_Image.rectTransform
            tempTextRect = item.other_self_chat_Text.rectTransform
            preferredWidth = item.other_self_chat_Text.Txt.preferredWidth
            preferredHeight = math.ceil(preferredWidth / (limitWidth - 60)) * 52 --由TEXT宽算出实际高度
            item.self_name_Text.text = data.nick_name

            preferredWidth = (preferredWidth + 60) > limitWidth and limitWidth or (preferredWidth + 60)
            preferredHeight = (preferredHeight + 60) < (106) and (106) or (preferredHeight + 60)
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
        elseif data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture then
            this.main_mid.chatCellRecycleScrollPanel:SetCellHeightOffSetByIndex(dataIndex + 1, 300)
            item.other_chat_bg_Image.gameObject:SetActive(false)
            if data.msg ~= this.chatCellOtherPictureNameTab[index] then
                downloadUserHead(data.msg, item.other_Image)
                this.chatCellOtherPictureNameTab[index] = data.msg
            end

            item.other_Image.gameObject:SetActive(true)
            item.other_voice_Image.gameObject:SetActive(false)
        elseif data.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Audio then
            item.other_chat_bg_Image.gameObject:SetActive(false)
            item.other_Image.gameObject:SetActive(false)
            item.other_voice_Image.gameObject:SetActive(true)

            -- --printDebug("+++++++++++++++++收到语音聊天信息为2："..data.msg)
            --   local dataGet = BaseModule.decodeProtoBytes("common", "MsgChatAudioInfo", data.msg)
            --printDebug("+++++++++++++++++++++我是接收的语音" .. table.tostring(dataGet))
            -- tempImageRect = item.other_voice_Image.transform:GetComponent(typeof(RectTransform))
            -- tempHeight = item.other_voice_Image.Img.preferredHeight
            -- tempImageRect.sizeDelta = Vector2(tempWidth > 690 and 690 or tempWidth, tempHeight)
            --  this.voiceTable[tostring(index + 1)] = dataGet.url
            -- item.other_voice_Image.name = tostring(index + 1)
            -- item.other_roarer_Text.text = dataGet.time

            -- local fun = function (arg)
            -- 	-- body
            -- 	local dataTable = string.split(data.msg,'|')
            -- 	-- local mItem = item
            -- 	--printDebug("++++++++++++++语音返回啊："..data.msg)
            -- end

            item.other_voice_Image:AddEventListener(
                UIEvent.PointerClick,
                function(eventData)
                    local obj = eventData.pointerPress

                    if obj == nil then
                        return
                    end

                    AudioManager.stopPlayRecord()
                    if this.isCurrVoicePress then
                        this.isCurrVoicePress = not this.isCurrVoicePress
                        return
                    end

                    this.isCurrVoicePress = not this.isCurrVoicePress

                    local msg = this.voiceTable[obj.name]

                    local dataTable = string.split(msg, "|")

                    AudioManager.startPlayRecordByFilePath(dataTable[4])
                end
            )
        end
    end

    item.time_Text.text = os.date("%H:%M", data.time)
end

function this.onBtnPicture(isPhotograph)
    --图片功能查看没做好暂时屏蔽
    PlatformSDK.takePhonePhoto(
        isPhotograph,
        function(bytes)
            --上传照片
            this.pngName = getShortUUID()
            uploadImage(
                this.pngName,
                bytes,
                ImageType.Face,
                function(isSucceed)
                    if isSucceed then
                        showFloatTips("上传照片成功")

                        local chatMsg = {
                            chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture,
                            msg = this.pngName,
                            time = os.time()
                        }

                        FriendChatDataProxy.currChatMsg = this.pngName
                        FriendChatDataProxy.currChatTime = os.time()
                        FriendChatDataProxy.currChatType = ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture
                        PlatformFriendModule.onReqSendFriendMsg(
                            {player_id = FriendChatDataProxy.currChatFriendId, chat_info = chatMsg}
                        )
                    else
                        showFloatTips("上传照片失败")
                    end
                end
            )
        end,
        true,
        1024,
        1024
    )
end

-- 按下语音按钮
function this:onBtnVoiceDown()
    startTime = 0
    local i = 0
    local a = 1
    AudioManager.startRecord(getUUID())

    GlobalTimeManager.Instance.timerController:AddTimer(
        "friendChatVoiceTimeLimit",
        60000,
        -1,
        function(...)
            AudioManager.stopRecord()
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("friendChatVoice")
        end
    )

    this.main_mid.cancel_Button.gameObject:SetActive(true)
    this.main_mid.show_bg_Image.gameObject:SetActive(true)
    this.main_mid.show_Text.text = "手指上滑，取消发送！"
    this.main_mid.show_Icon:ChangeIcon(0)

    GlobalTimeManager.Instance.timerController:AddTimer(
        "friendChatVoice",
        180,
        -1,
        function(...)
            if this.voiceSend and not this.voiceCanel then
                this.main_mid.show_Icon:ChangeIcon(i)
                i = i + a
                if i <= 0 then
                    a = 1
                elseif i >= 3 then
                    a = -1
                end
            end
        end
    )

    GlobalTimeManager.Instance.timerController:AddTimer(
        "friendChatTime",
        100,
        -1,
        function(...)
            startTime = startTime + 0.1
        end
    )
end

-- 松开语音按钮
function this:onBtnVoiceUp()
    AudioManager.stopRecord()
    if this.voiceCanel then
        this.voiceSend = false
        this.voiceCanel = false
    elseif startTime < 1 then
        this.main_mid.show_bg_Image.gameObject:SetActive(true)
        this.main_mid.show_Text.text = "录制时间太短！"
        this.main_mid.show_Icon:ChangeIcon(5)
        this.voiceCanel = false
        this.voiceSend = false
    end

    GlobalTimeManager.Instance.timerController:AddTimer(
        "waitingTime",
        500,
        -1,
        function(...)
            this.main_mid.cancel_Button.gameObject:SetActive(false)
            this.main_mid.show_bg_Image.gameObject:SetActive(false)
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("friendChatVoice")
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("friendChatVoiceTimeLimit")
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("friendChatTime")
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("waitingTime")
            this.voiceSend = true
            this.voiceFlag = false
            this.voiceCanel = false
        end
    )
end

function this:onBtnChangeIcon()
    if this.main_mid.change_chat_type_Icon.initIndex == 0 then --打字
        this.main_mid.change_chat_type_Icon:ChangeIcon(1)
        this.main_mid.chatImput_InputField.gameObject:SetActive(false)
        this.main_mid.chat_voice_Button.gameObject:SetActive(true)
    else --语音
        this.main_mid.change_chat_type_Icon:ChangeIcon(0)
        this.main_mid.chatImput_InputField.gameObject:SetActive(true)
        this.main_mid.chat_voice_Button.gameObject:SetActive(false)
    end
end

function this:onBtnCancel()
    this.voiceCanel = true
    this.voiceSend = false
    this.main_mid.show_bg_Image.gameObject:SetActive(true)
    this.main_mid.show_Text.text = "松开手指，取消发送！"
    this.main_mid.show_Icon:ChangeIcon(4)
end

function this:onBtnCancelExit()
    this.main_mid.show_Icon:ChangeIcon(0)
    this.main_mid.show_Text.text = "手指上滑，取消发送！"
    this.voiceSend = true
end

function this:chatConfigBtnEvent()
    showFloatTips("功能开发中敬请期待！")
end
--空白区域撤回底框
function this:onBtnMaskEvent()
    -- this.DownPopShowPage(not this.main_mid.face.gameObject.activeSelf)
end
