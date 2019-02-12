require "base:enum/UIViewEnum"
require "base:mid/friend/Mid_platform_friend_main_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"
require "base:module/friend/view/PlatformFriendApplyView"
PlatformFriendMainPageView = BaseView:new()
local this = PlatformFriendMainPageView
this.viewName = "PlatformFriendMainPageView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Friend_Main_Page_View, true)

--设置加载列表
this.loadOrders = {
    "base:friend/platform_friend_main_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = Mid_platform_friend_main_panel
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

this.isActiveAddFriend = nil
this.isRecommendView = false
function this:onShowHandler(msg)
    this.isActiveAddFriend = msg.isActive

    this.isRecommendView = msg.isRecommendView

    printDebug("=====================Platform_Friend_Main_Page_View调用完毕======================")
    this.main_mid.single.gameObject:SetActive(false)
    this:updateFriendMainPageList()
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    self:addNotice()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.setAlreadyAgree)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateFriendPhotos)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.setAlreadyAgree)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateFriendPhotos)
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Friend_Main_Page_View)
        end
    )

    self.main_mid.sendmsg_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            --  调出好友聊天界面
            printDebug("调出好友聊天界面")

            FriendChatDataProxy.currChatFriendId = this.currFriendMainPageData.player_id
            printDebug("========================点击了好友，好友id为：" .. FriendChatDataProxy.currChatFriendId)
            ViewManager.open(UIViewEnum.Platform_Friend_Chat_View, {isMain = false})
        end
    )

    self.main_mid.send_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            -- 向服务器发送添加好友申请
            printDebug("向服务器发送添加好友申请")
            local data = {
                op = ProtoEnumFriendModule.FriendOp.FriendOpAddFriend,
                -- account_name = this.currFriendMainPageData.account_name,
                player_id = this.currFriendMainPageData.player_id,
                strparam = this.main_mid.msg_InputField.text
            }
            PlatformFriendModule.onReqFriendOp(data, this.currFriendMainPageData)
        end
    )

    self.main_mid.closetip_Image:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            self.main_mid.sendtip_Panel.gameObject:SetActive(false)
        end
    )
end

this.currFriendMainPageData = nil

--收到图片信息
--[[function this.updateFriendPhotos(notice,rep)
	local rsp = rep:GetObj()

	if rsp == nil then return end
	PlatformUserProxy:GetInstance():setUserPhotosData(rsp)
	this:updateFriendPhotosEnd()
end
--]]
function this.updateFriendPhotos()
    --设置朋友圈照片
    this.currFriendPhotosData = PlatformUserProxy:GetInstance():getUserPhotosData()
    --printDebug("++++++++++++++++++++++++currFriendPhotosData:"..table.tostring(this.currFriendPhotosData))

    if this.currFriendPhotosData == nil then
        return
    end
    local temp = this.currFriendPhotosData.album_pic_info_list
    if temp == nil or #temp == 0 or temp[1] == "nil" then
        this.main_mid.pyq_GridRecycleScrollPanel.gameObject:SetActive(false)
        this.main_mid.nopyq_Text.gameObject:SetActive(true)
    else
        local max = #temp
        this.main_mid.nopyq_Text.gameObject:SetActive(false)
        this.main_mid.pyq_GridRecycleScrollPanel:SetCellData(temp, this.onSetPics, true)
        printDebug("++++++++++++++++++我是啦啦啦啦啦max = "..max)
        local cellSize = Vector2(330, 330)
        local space = Vector2(10, 10)
        local singleW =  math.floor(cellSize.x * 3 + space.x * (3 - 1))
        local singleH = math.floor(cellSize.y * 3 + space.y * (3 - 1))
        -- 处理单张图
        this.main_mid.single.gameObject:SetActive(max == 1)
        this.main_mid.pyq_GridRecycleScrollPanel.gameObject:SetActive(not (max == 1))
        this.main_mid.single.rectTransform.sizeDelta = Vector2(singleW, singleH)
        if max == 1 then
            this.main_mid.single_image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
            downloadResizeImage(temp[1].url, this.main_mid.single_image, ResizeType.MinFit, singleW, singleH,"", 1,  function (sprite)
                this.main_mid.single_image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
                PlatformPhotoDisplayView.adapterImage(this.main_mid.single_image, singleW, singleH)
            end)
        end
    end
end

--更新好友主页界面
function this:updateFriendMainPageList()
    printDebug("更新好友主页界面！")
    this.currFriendMainPageData = PlatformFriendProxy:GetInstance():getFriendMainPageData()
    local selfData = PlatformUserProxy:GetInstance():getUserInfo()

    if this.currFriendMainPageData == nil then
        return
    end

    if not this.isRecommendView then
        this.currFriendMainPageData = this.currFriendMainPageData.player_base_info
    end
    MainModule.sendReqAlbumPicList(this.currFriendMainPageData.player_id)
    printDebug("=======================更新好友主页界面！！！！")

    downloadUserHead(this.currFriendMainPageData.head_url, this.main_mid.head_Image)

    self.main_mid.name_Text.text = this.currFriendMainPageData.nick_name

    self.main_mid.repu_Text.text = "声望:0"
    --..this.currFriendMainPageData.reputation

    self.main_mid.id_Text.text = "ID:" .. this.currFriendMainPageData.player_id

    -- self.main_mid.level_Text.text = "LV "..this.currFriendMainPageData.level

    local distance =
        MapManager.getDistance(
        this.currFriendMainPageData.lng,
        this.currFriendMainPageData.lat,
        selfData.lng,
        selfData.lat
    )
    if distance <= 1000 then
        self.main_mid.distance_Text.text = string.format("%0.2f", tostring(distance)) .. "m"
    else
        self.main_mid.distance_Text.text = string.format("%0.2f", tostring(distance / 1000)) .. "km"
    end
    -- self.main_mid.sex_Image.Img.color = this.currFriendMainPageData.sex == 1 and CSColor.blue or CSColor.red
    if this.currFriendMainPageData.sex == 0 then
        self.main_mid.sexbg_Icon:ChangeIcon(1)
    else
        self.main_mid.sexbg_Icon:ChangeIcon(this.currFriendMainPageData.sex - 1)
    end

    --self.main_mid.sex_Icon:ChangeIcon(this.currFriendMainPageData.sex - 1)

    self.main_mid.pyq_GridRecycleScrollPanel.gameObject:SetActive(false)
    self.main_mid.nopyq_Text.gameObject:SetActive(false)

    local isFriend = PlatformFriendProxy:GetInstance():isMyFriendById(this.currFriendMainPageData.player_id)

    --如果是朋友，则显示发送消息按钮，否则显示添加或者拒绝按钮
    printDebug("isActiveAddFriend：" .. tostring(this.isActiveAddFriend))
    if isFriend then
        printDebug("已经是朋友")
        self.main_mid.friend_Panel.gameObject:SetActive(true)
        self.main_mid.nonfriend_noactive_Panel.gameObject:SetActive(false)
        self.main_mid.nonfriend_active_Panel.gameObject:SetActive(false)
        self.main_mid.applied_Panel.gameObject:SetActive(false)

        -- self.main_mid.sendmsg_Button.name = this.currFriendMainPageData.player_id
        self.main_mid.sendmsg_Button:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                -- require "base:module/platform/data/FriendChatDataProxy"
                --  调出好友聊天界面
                -- local selectedObj = eventData.selectedObject

                --PlatformFriendProxy:GetInstance():setCurrChatFriendData(this.currFriendMainPageData.player_id)

                FriendChatDataProxy.currChatFriendId = this.currFriendMainPageData.player_id
                printDebug("========================点击了好友，好友id为：" .. FriendChatDataProxy.currChatFriendId)
                ViewManager.open(UIViewEnum.Platform_Friend_Chat_View, {isMain = false})
            end
        )

        self.main_mid.cancel_Button:AddEventListener(
            UIEvent.PointerClick,
            function()
                Alert.showVerifyMsg(
                    nil,
                    "确定删除好友？",
                    "取消",
                    nil,
                    "确定",
                    function()
                        --  向服务器发送拒绝添加好友申请

                        --  向服务器发送刪除好友申请
                        local data = {
                            op = ProtoEnumFriendModule.FriendOp.FriendOpDelFriend,
                            player_id = this.currFriendMainPageData.player_id
                            -- strparam = this.main_mid.sendmsg_InputField.text,
                        }
                        FriendChatDataProxy.currDelFriendId = tonumber(this.currFriendMainPageData.player_id)
                        PlatformFriendModule.onReqFriendOp(data)

                        --删除好友信息
                    end
                )
            end
        )
    else
        printDebug("不是朋友")
        self.main_mid.friend_Panel.gameObject:SetActive(false)
        self.main_mid.applied_Panel.gameObject:SetActive(false)

        if PlatformFriendProxy:GetInstance():isMyAddFriendById(this.currFriendMainPageData.player_id) then
            return this:setAlreadySent()
        end

        if this.isActiveAddFriend then
            printDebug("主动添加")
            self.main_mid.nonfriend_noactive_Panel.gameObject:SetActive(false)
            self.main_mid.nonfriend_active_Panel.gameObject:SetActive(true)
            this:onValueChange()
            self.main_mid.addactivefriend_Button:AddEventListener(
                UIEvent.PointerClick,
                function()
                    local data = PlatformUserProxy:GetInstance():getUserInfo()
                    if data ~= nil then
                        this.main_mid.msg_InputField.text = "我是" .. data.nick_name
                    end

                    this.main_mid.sendtip_Panel.gameObject:SetActive(true)
                end
            )
        else
            printDebug("非主动添加")
            self.main_mid.nonfriend_noactive_Panel.gameObject:SetActive(true)

            self.main_mid.nonfriend_active_Panel.gameObject:SetActive(false)

            self.main_mid.addfriend_Button:AddEventListener(
                UIEvent.PointerClick,
                function()
                    -- 向服务器发送添加好友申请
                    printDebug("向服务器发送添加好友申请")
                    local data = {
                        op = ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend,
                        -- account_name = this.currFriendMainPageData.account_name,
                        player_id = this.currFriendMainPageData.player_id
                        -- strparam = this.main_mid.sendmsg_InputField.text,
                    }
                    PlatformFriendModule.onReqFriendOp(data)
                    PlatformFriendProxy:GetInstance():removeReiceiveFriendApplyData(
                        this.currFriendMainPageData.player_id
                    )
                end
            )

            self.main_mid.decline_Button:AddEventListener(
                UIEvent.PointerClick,
                function()
                    Alert.showVerifyMsg(
                        nil,
                        "确定拒绝好友？",
                        "取消",
                        nil,
                        "确定",
                        function()
                            --  向服务器发送拒绝添加好友申请

                            local data = {
                                op = ProtoEnumFriendModule.FriendOp.FriendOpRejectApply,
                                --op = ProtoEnumFriendModule.FriendOp.FriendOpDelFriend,
                                player_id = this.currFriendMainPageData.player_id
                                -- strparam = this.main_mid.sendmsg_InputField.text,
                            }
                            FriendChatDataProxy.currDelFriendId = tonumber(this.currFriendMainPageData.player_id)
                            PlatformFriendModule.onReqFriendOp(data) 
                        end
                    )
                end
            )
        end
    end

    -- self.main_mid.apply_CellRecycleScrollPanel:SetCellData(this.currFriendMainPageData,this.updateFriendCellList,true)
end

--设置用户图片
function this.onSetPics(go, data, index)
    local isFriend = PlatformFriendProxy:GetInstance():isMyFriendById(this.currFriendMainPageData.player_id)
    if not isFriend then
        if index > 15 then
            return
        end
    end
    local item = this.main_mid.pyqCellArr[index + 1]
    printDebug("++++++++++++++++我的框框是这个号" .. index)
    item.pic_Image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
    downloadResizeImage(
        data.url,
        item.pic_Image,
        ResizeType.MinFit,
        330,
        330,
        "",
        1,
        function(sprite)
            item.pic_Image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
            PlatformPhotoDisplayView.adapterImage(item.pic_Image, 330, 330)
        end
    )
end

-- function this.updateFriendCellList(go,data,index)

-- 	local item =  this.main_mid.applycellArr[index+1]

-- 	downloadFromPicServer(data.head_url,item.head_Image)

-- 	item.name_Text.text = data.name

-- 	item.intro_Text.text = data.msg

-- 	if not data.isAdded then
-- 		item.add_Button.gameObject:SetActive(true)
-- 		item.added_Text.gameObject:SetActive(false)

-- 		item.add_Button:AddEventListener(UIEvent.PointerClick,function (eventData)
-- 			--同意添加好友申请
-- 		end)
-- 	else
-- 		item.add_Button.gameObject:SetActive(false)
-- 		item.added_Text.gameObject:SetActive(true)
-- 	end

-- 	item.add_Button.name = data.player_id

-- 	item.press_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
-- 		-- 点击了好友，需要向服务器申请好友数据ReqUserInfo，这里要传好友账号名称给服务器
-- 	end)
-- end

------------------------外部调用
--设置请求按钮为已经请求
function this:setAlreadySent()
    self.main_mid.nonfriend_noactive_Panel.gameObject:SetActive(false)
    self.main_mid.nonfriend_active_Panel.gameObject:SetActive(false)
    self.main_mid.applied_Panel.gameObject:SetActive(true)
    if this.isActiveAddFriend then
        self.main_mid.applied_Button.Txt.text = "已申请"
    else
        self.main_mid.applied_Button.Txt.text = "已通过"
    end
    self.main_mid.sendtip_Panel.gameObject:SetActive(false)
end

--暂时
function this:setAlreadyAgree()
    printDebug("======================通知对方已经同意添加你为好友！")
    this.main_mid.nonfriend_noactive_Panel.gameObject:SetActive(false)
    this.main_mid.nonfriend_active_Panel.gameObject:SetActive(false)
    this.main_mid.applied_Panel.gameObject:SetActive(false)
    this.main_mid.sendtip_Panel.gameObject:SetActive(false)
    this.main_mid.friend_Panel.gameObject:SetActive(true)

    -- this:updateFriendMainPageList()
end

function this:onValueChange()
    this.main_mid.msg_InputField.inputField:ActivateInputField()
    this.main_mid.msg_InputField:OnValueChanged(
        function(obj)
            if this.main_mid.msg_InputField.text == "" or this.main_mid.msg_InputField.text == nil then
                this.main_mid.clear_Image.gameObject:SetActive(false)
            else
                this.main_mid.clear_Image.gameObject:SetActive(true)
            end
        end
    )

    this.main_mid.clear_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.main_mid.msg_InputField.text = ""
        end
    )
end
