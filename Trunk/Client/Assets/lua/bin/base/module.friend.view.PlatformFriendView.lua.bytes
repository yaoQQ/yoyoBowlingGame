require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PlatformFriendView = BaseView:new()
local this = PlatformFriendView
this.viewName = "PlatformFriendView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.PlatForm_Friend_View, false)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end
local color = nil
function this:onShowHandler(msg)
    printDebug("=====================PlatForm_Friend_View调用完毕======================")
    color = self.main_mid.friendcount_Button.Txt.color
    this:onUpdateFriendNum()
    this:updateFriendList(false)
    this:addNotice()

    --ShowWaiting(false,this.viewName)
end

function this:addNotice()
    -- NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op, this.onReqChatChannelOp)
    -- NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
    --NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.onUpdateFriendList)
    -- NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
    -- NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Chat_Channel_Change, this.onNotifyChatChannelChange)
end

function this:removeNotice()
    -- NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op, this.onReqChatChannelOp)
    -- NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Friend_RecommendSuccess, this.onRspRecommend)
    -- NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.onUpdateFriendList)
    -- NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op, this.onReqChatChannelOp)
    -- NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
    -- NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Chat_Channel_Change, this.onNotifyChatChannelChange)
end

--此为废弃界面
--以下为错误代码，收到在界面层对数据层设值导致数据错乱
--通知更新好友列表
function this.onUpdateFriendList(notice, rsp)
    -- body
    local req = rsp:GetObj()
    PlatformFriendProxy:GetInstance():addFriendListData(req)
    this:updateFriendList(false)
end

function this.onRspRecommend(notice, rsp)
    local req = rsp:GetObj()
    PlatformFriendProxy:GetInstance():setRecommendFriendData(req)

    printDebug("收到附近好友推荐" .. table.tostring(req))
    if PlatformFriendRecommendView.isOpen then
        PlatformFriendRecommendView:updateFriendRecommendList()
    else
        ViewManager.open(UIViewEnum.Platform_Friend_Recommend_View)
    end
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end
local isFriendView = true
function this:addEvent()
    self.main_mid.CloseImage:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.PlatForm_Friend_View)
        end
    )

    self.main_mid.add_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            printDebug("+++++++++++++++++注册了即时")
            -- ViewManager.open(UIViewEnum.Platform_Friend_Recommend_View)
            local data = {}
            data.isInstanceSearch = true
            data.myFun = function(inputTxt) --这里传入点击了确定按钮之后的逻辑
                printDebug("++++++++++++++呵呵呵呵呵收到发送消息为：" .. inputTxt)
                PlatformFriendProxy:GetInstance():locateFriend(inputTxt)
                this.main_mid.search_panel.gameObject:SetActive(true)
                this:updateFriendSearchList(true)
            end
            ViewManager.open(UIViewEnum.Platform_Common_Search_View, data)
        end
    )

    --[[好友按钮
		self.main_mid.friendcount_Button:AddEventListener(UIEvent.PointerClick,function (eventData)
		printDebug("+++++++++++++++++注册了即时")
		self.main_mid.button_Image.gameObject:SetActive(isFriendView)
		if isFriendView then
			self.main_mid.friendcount_Button.Txt.color = CSColor(1,1,1,1)
		else
			self.main_mid.friendcount_Button.Txt.color = color
		end
		isFriendView=not isFriendView
	end)
--]]
end

this.currFriendListData = nil
this.currSearchListData = nil
local tempListData = nil
local applyData = nil
--更新好友列表
function this:updateFriendList(isFromSearch)
    if not isFromSearch then
        --获取好友列表
        tempListData = PlatformFriendProxy:GetInstance():getFriendListData()
    else
        --模糊搜索
        tempListData = PlatformFriendProxy:GetInstance():getLocateFriendData()
    end

    -- if tempListData == nil then return end

    -- tempListData = tempListData.friend_info_list

    this.currFriendListData = {"1", "2"}

    if tempListData ~= nil then
        for i = 1, #tempListData do
            table.insert(this.currFriendListData, tempListData[i])
        end
        self.main_mid.friendcount_Button.Txt.text = "好友(" .. #tempListData .. ")"
    else
        self.main_mid.friendcount_Button.Txt.text = "好友(0)"
    end
    tempListData = nil
    --更新好友申请
    applyData = PlatformFriendProxy:GetInstance():getReceiveAddFriendApplyData()

    printDebug("=======================更新好友列表！！！！" .. table.tostring(this.currFriendListData))
    self.main_mid.friend_CellRecycleScrollPanel:SetCellData(this.currFriendListData, this.updateFriendCellList, true)
end
--更新搜索列表
function this:updateFriendSearchList(isFromSearch)
    if not isFromSearch then
        --获取好友列表
        tempListData = PlatformFriendProxy:GetInstance():getFriendListData()
    else
        --模糊搜索
        tempListData = PlatformFriendProxy:GetInstance():getLocateFriendData()
    end
    if tempListData ~= nil then
        for i = 1, #tempListData do
            table.insert(this.currSearchListData, tempListData[i])
        end
        self.main_mid.friendcount_Button.Txt.text = "好友(" .. #tempListData .. ")"
    else
        self.main_mid.friendcount_Button.Txt.text = "好友(0)"
    end
    tempListData = nil
    self.main_mid.search_CellRecycleScrollPanel:SetCellData(
        this.currSearchListData,
        this.updateFriendSearchCellList,
        true
    )
end

function this.updateFriendSearchCellList(go, data, index)
    local item = this.main_mid.searchlistcellArr[index + 1]
    item.config_Panel.gameObject:SetActive(false)
    item.friend_Panel.gameObject:SetActive(true)

    item.name_Text.text = data.player_base_info.nick_name

    item.honor_Text.text = data.player_base_info.address

    -- item.sexbg_Image.Img.color = data.sex == 1 and CSColor.blue or CSColor.red
    if data.player_base_info.sex == 0 then
        item.sexbg_Icon:ChangeIcon(1)
    else
        item.sexbg_Icon:ChangeIcon(data.player_base_info.sex - 1)
    end
    --item.sex_Icon:ChangeIcon(data.player_base_info.sex - 1)

    local selfData = PlatformUserProxy:GetInstance():getUserInfo()
    if selfData == nil then
        return
    end
    local distance =
        MapManager.getDistance(data.player_base_info.lng, data.player_base_info.lat, selfData.lng, selfData.lat)
    if distance <= 1000 then
        item.distance_Text.text = string.format("%0.2f", tostring(distance)) .. "m "
    else
        item.distance_Text.text = string.format("%0.2f", tostring(distance / 1000)) .. "km "
    end

    downloadUserHead(data.player_base_info.head_url, item.head_Image)

    item.newfriend_image.name = data.player_base_info.player_id

    item.newfriend_image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            FriendChatDataProxy.currChatFriendId = data.player_base_info.player_id
            printDebug("========================点击了好友，好友id为：" .. FriendChatDataProxy.currChatFriendId)

            local info = PlatformFriendProxy:GetInstance():getFriendDataById(FriendChatDataProxy.currChatFriendId)

            if info == nil then
                printDebug("好友信息为空！")
                return
            end

            PlatformFriendProxy:GetInstance():setFriendMainPageData(info)
            local data = {}
            data.isActive = true
            data.isRecommendView = false
            ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
            ViewManager.close(UIViewEnum.Platform_Common_Search_View)
        end
    )
end

function this.updateFriendCellList(go, data, index)
    local item = this.main_mid.friendlistcellArr[index + 1]

    if data == "1" then
        item.config_Panel.gameObject:SetActive(true)
        item.friend_Panel.gameObject:SetActive(false)
        item.configname_Text.text = "新的朋友"
        item.config_Icon:ChangeIcon(0)
        item.config_image:AddEventListener(
            UIEvent.PointerClick,
            function()
                ViewManager.open(UIViewEnum.Platform_Friend_Apply_View)
                ViewManager.close(UIViewEnum.Platform_Common_Search_View)
            end
        )

        if applyData ~= nil then
            if applyData.notFriendCount ~= nil and applyData.notFriendCount > 0 then
                printDebug("++++++++++++applyData.notFriendCount" .. applyData.notFriendCount)
                if applyData.notFriendCount > 10 then
                    if applyData.notFriendCount <= 99 then
                        item.configredpoint_Text.text = applyData.notFriendCount
                    else
                        item.configredpoint_Text.text = "99"
                    end
                    item.configredpoint_Icon:ChangeIcon(1)
                else
                    item.configredpoint_Text.text = applyData.notFriendCount
                    item.configredpoint_Icon:ChangeIcon(0)
                end
                item.configredpoint_Icon.gameObject:SetActive(true)
            else
                item.configredpoint_Icon.gameObject:SetActive(false)
            end
        end
    elseif data == "2" then
        item.config_Panel.gameObject:SetActive(true)
        item.friend_Panel.gameObject:SetActive(false)
        item.configname_Text.text = "添加好友"
        item.config_Icon:ChangeIcon(1)
        item.config_image:AddEventListener(
            UIEvent.PointerClick,
            function()
                local req = {}
                req.lng = MapManager.userLng
                req.lat = MapManager.userLat
                req.distance = 1000000 --暂定1公里
                req.maxcount = 1000 --暂定最多1000个商家
                PlatformFriendModule.onSendRecommend(req)
                ViewManager.close(UIViewEnum.Platform_Common_Search_View)
            end
        )
    else --if data.friend_state == "FriendStateNormal" then
        --else
        --item.config_Panel.gameObject:SetActive(false)
        --item.friend_Panel.gameObject:SetActive(false)
        --printDebug("++++++++++++++++++++未知好友状态")
        item.config_Panel.gameObject:SetActive(false)
        item.friend_Panel.gameObject:SetActive(true)

        item.name_Text.text = data.player_base_info.nick_name

        item.honor_Text.text = data.player_base_info.address

        -- item.sexbg_Image.Img.color = data.sex == 1 and CSColor.blue or CSColor.red
        if data.player_base_info.sex == 0 then
            item.sexbg_Icon:ChangeIcon(1)
        else
            item.sexbg_Icon:ChangeIcon(data.player_base_info.sex - 1)
        end
        --item.sex_Icon:ChangeIcon(data.player_base_info.sex - 1)

        local selfData = PlatformUserProxy:GetInstance():getUserInfo()
        if selfData == nil then
            return
        end
        local distance =
            MapManager.getDistance(data.player_base_info.lng, data.player_base_info.lat, selfData.lng, selfData.lat)
        if distance <= 1000 then
            item.distance_Text.text = string.format("%0.2f", tostring(distance)) .. "m "
        else
            item.distance_Text.text = string.format("%0.2f", tostring(distance / 1000)) .. "km "
        end

        downloadUserHead(data.player_base_info.head_url, item.head_Image)

        item.newfriend_image.name = data.player_base_info.player_id

        item.newfriend_image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                FriendChatDataProxy.currChatFriendId = data.player_base_info.player_id
                printDebug("========================点击了好友，好友id为：" .. FriendChatDataProxy.currChatFriendId)

                local info = PlatformFriendProxy:GetInstance():getFriendDataById(FriendChatDataProxy.currChatFriendId)

                if info == nil then
                    printDebug("好友信息为空！")
                    return
                end

                PlatformFriendProxy:GetInstance():setFriendMainPageData(info)
                local data = {}
                data.isActive = true
                data.isRecommendView = false
                ViewManager.open(UIViewEnum.Platform_Friend_Main_Page_View, data)
                ViewManager.close(UIViewEnum.Platform_Common_Search_View)
            end
        )
    end

    --好友名字
    -- item.name_Text.text = data.name

    --好友头像
    -- myHeadTexture2D = PlatformPngManagerProxy:GetInstance():getFriendHeadByFriendId(data.player_id)

    -- if myHeadTexture2D == nil and data.head_url ~= nil and data.head_url ~= "" then
    -- 	PhotoManager.downloadPhoto(data.head_url,false,function (texture2D)
    -- 		if texture2D == nil then
    -- 			printDebug("下载用户头像失败")
    -- 		else
    -- 			ImageUtil.setTexture2DImage(texture2D,item.friendhead_Image.Img)
    -- 			PlatformPngManagerProxy:GetInstance():addFriendHeadsTable(data.player_id,texture2D)
    -- 		end
    -- 	end)
    -- else
    -- 	ImageUtil.setTexture2DImage(myHeadTexture2D,item.friendhead_Image.Img)
    -- end

    --使得可以滑动显示删除

    -- item.press_Image.name = data.player_id

    -- item.press_Image:AddEventListener(UIEvent.DragEnd,function (eventData)
    -- 	currSliceImgObj = eventData.selectedObject

    -- 	if not isSlice then
    -- 		currSliceImgObj.transform:Find("delete_Image").gameObject:SetActive(true)
    -- 	else
    -- 		currSliceImgObj.transform:Find("delete_Image").gameObject:SetActive(false)
    -- 	end

    -- 	isSlice = not isSlice
    -- end)

    --点击了好友进入好友聊天界面
    -- item.press_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
    -- 	FriendChatDataProxy.currChatFriendId = tonumber(eventData.pointerPress.name)
    -- 	printDebug("========================点击了好友，好友id为："..FriendChatDataProxy.currChatFriendId)
    -- 	ViewManager.open(UIViewEnum.Platform_Friend_Chat_View)
    -- end)

    --删除好友
    -- item.delete_Image.name = data.player_id

    -- item.delete_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
    -- 	currDelBtn = eventData.pointerPress

    -- 	if currDelBtn == nil then return end

    -- 	local data = {
    -- 		op = ProtoEnumFriendModule.FriendOp.FriendOpDelFriend,

    -- 		player_id = tonumber(currDelBtn.name),
    -- 	}

    -- 	FriendChatDataProxy.currDelFriendId = tonumber(currDelBtn.name)

    -- 	PlatformFriendModule.onReqFriendOp(data)
    -- end)
end

function this:onUpdateFriendNum()
    --向服务器发送更新好友信息列表请求
    local data2 = {
        op = ProtoEnumFriendModule.FriendOp.FriendOpReqList,
        --param1 = 0,
        param2 = 100
    }
    PlatformFriendModule.onReqFriendOp(data2)
end
