require "base:enum/UIViewEnum"
require "base:mid/global/Mid_platform_personal_panel"
require "base:enum/Platform_UserCouponType"

--主界面：我的
PlatformGlobalPersonalView = BaseView:new()
local this = PlatformGlobalPersonalView
this.viewName = "PlatformGlobalPersonalView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_Personal_View, true)

--设置加载列表
this.loadOrders = {
    "base:global/platform_personal_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    --下面两行默认需要调用

    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)

    --添加UI事件监听
    self:addEvent()
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()

    --打开子界面
    ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Global_Personal_View)
end

function this:addEvent()
	--签到
	self.main_mid.sign_Button:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Sign_View)
        end
    )
	
	--好友
	self.main_mid.friend_Button:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Change_Friend_View)
        end
    )
	
	--卡包
	self.main_mid.card_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            local data = {
                page_index = 0,
                num = 100
            }
            NoticeManager.Instance:Dispatch(Platform_UserCouponType.Platform_Go_CouponMain_Page, data)
        end
    )
	
	--排行榜
	self.main_mid.rank_Button:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
			ViewManager.open(UIViewEnum.Platform_Rank_View)
        end
    )
	
	--设置
	self.main_mid.config_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            ViewManager.open(UIViewEnum.Platform_Set_MainView)
        end
    )
	
    self.main_mid.honor_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            showFloatTips("功能开发中敬请期待！")
        end
    )
    self.main_mid.pic_CellGroup:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            ViewManager.open(UIViewEnum.Platform_Photo_Display_View)
        end
    )
    
    self.main_mid.task_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            showFloatTips("功能开发中敬请期待！")
        end
    )

    self.main_mid.backpack_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            showFloatTips("功能开发中敬请期待！")
        end
    )

    self.main_mid.collect_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            showFloatTips("功能开发中敬请期待！")
        end
    )

    self.main_mid.mall_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
			showFloatTips("功能开发中敬请期待！")
            --RechargeManager.openShop(1, 0)
        end
    )
    self.main_mid.top_gold_Button:AddEventListener(UIEvent.PointerClick, self.onBtnGoldEvent)
    self.main_mid.top_diamond_Button:AddEventListener(UIEvent.PointerClick, self.onBtnDiamondEvent)
    --self.main_mid.top_yo_card_Button:AddEventListener(UIEvent.PointerClick,self.onBtnYoCardEvent)
	
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		self.main_mid.top_diamond.gameObject:SetActive(true)
	end
end
--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("ucardPersonTime")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("diamondPersonTime")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("goldPersonTime")

    --关闭子界面
    ViewManager.close(UIViewEnum.Platform_Global_View)
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.notifyRedPoint)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateSelfPhotos)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Init_Diamond_Money, this.updateDiamondAndMoney)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Diamond, this.updateDiamondAndMoney)
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Money, this.updateDiamondAndMoney)
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Cash, this.updateDiamondAndMoney)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.notifyRedPoint)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateSelfPhotos)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Init_Diamond_Money, this.updateDiamondAndMoney)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Diamond, this.updateDiamondAndMoney)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Money, this.updateDiamondAndMoney)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Cash, this.updateDiamondAndMoney)
end

--打开界面时初始化
function this:initView()
    MainModule.sendReqAlbumPicList(LoginDataProxy.playerId)

    self:updatePersonalPanel()
    this.updateDiamondAndMoney()
end

function this.notifyRedPoint()
    local applyData = PlatformFriendProxy:GetInstance():getReceiveAddFriendApplyData()
    if applyData ~= nil then
        if applyData.notFriendCount ~= nil and applyData.notFriendCount > 0 then
            printDebug("++++++++++++applyData.notFriendCount" .. applyData.notFriendCount)
            if applyData.notFriendCount > 10 then
                -- this.main_mid.friend_rp_Image.gameObject:SetActive(true)
                if applyData.notFriendCount <= 99 then
                    this.main_mid.friend_rp_Text.text = applyData.notFriendCount
                else
                    this.main_mid.friend_rp_Text.text = "99"
                end
            else
                -- this.main_mid.configredpoint_Icon:ChangeIcon(0)
                this.main_mid.friend_rp_Text.text = applyData.notFriendCount
            end
            this.main_mid.friend_rp_Image.gameObject:SetActive(true)
        else
            this.main_mid.friend_rp_Image.gameObject:SetActive(false)
        end
    end
end

--PersonalChange公用部分开始
function this.updateSelfPhotos()
    this:updatePersonalPanel()

    ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
end

this.currUserPhotosData = nil

--更新个人界面
function this:updatePersonalPanel()
    this.notifyRedPoint()

    this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()

    if this.currGlobalBaseData == nil then
        return
    end
    --printDebug("++++++++++++++++++++++++this.currGlobalBaseData:"..table.tostring(this.currGlobalBaseData))
    self.main_mid.name_Text.text = this.currGlobalBaseData.nick_name
    self.main_mid.sex_Icon:ChangeIcon(this.currGlobalBaseData.sex - 1)
    self.main_mid.ID_Text.text = "ID:" .. this.currGlobalBaseData.player_id
    self.main_mid.top_gold_Text.text = this.currGlobalBaseData.money
    self.main_mid.top_diamond_Text.text = this.currGlobalBaseData.diamond
    downloadUserHead(this.currGlobalBaseData.head_url, self.main_mid.head_Icon)

    self.main_mid.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            ViewManager.open(UIViewEnum.Personal_Change_Info_View)
        end
    )
    this:topGoldEffectTimer()
    this:updatePersonalPhotos()
end

function this:topGoldEffectTimer()
    GlobalTimeManager.Instance.timerController:AddTimer(
        "goldPersonTime",
        2500,
        -1,
        function(...)
            this.main_mid.goldEffect:Play()
        end
    )

    GlobalTimeManager.Instance.timerController:AddTimer(
        "diamondPersonTime",
        2900,
        -1,
        function(...)
            this.main_mid.diamondEffect:Play()
        end
    )
    GlobalTimeManager.Instance.timerController:AddTimer(
        "ucardPersonTime",
        3500,
        -1,
        function(...)
            this.main_mid.ucardEffect:Play()
        end
    )
end
--更新顶部金币钻石
function this.updateDiamondAndMoney()
    local currBaseData = PlatformUserProxy:GetInstance():getUserInfo()
    if currBaseData == nil then
        return
    end

	local money = this:NumberToShow(currBaseData.money)
	local diamond = this:NumberToShow(currBaseData.diamond)
	local cash = this:redbagNumberToShow(currBaseData.cash/100)
    this.main_mid.top_gold_Text.text = tostring(money)
    this.main_mid.top_diamond_Text.text = tostring(diamond)
	this.main_mid.top_packet_Text.text = tostring(cash)
end

function this:NumberToShow(number)
	if number == nil then
		printDebug("数字格式错误")
	else
        if number / 10^9 >= 1 then
            return "9.99亿"
        elseif number / 10^8 >= 1 then
			number = math.floor(number / 10^6)
			local num = table.clearZero(tonumber(string.format("%.2f", number/10^2)))
            return (num.."亿")
        elseif number / 10^6 >= 1 then
			number = math.floor(number / 10^4)
            return number.."万"
        elseif number / 10^5 >= 1 then
			number = math.floor(number / 10^3)
            local num = table.clearZero(tonumber(string.format("%.1f", number/10)))
            return (num.."万")
        elseif number / 10^4 >= 1 then
			number = math.floor(number / 10^2)
			local num = (tonumber(string.format("%.2f", number/10^2)))
            return (num.."万")
		else
			return number
		end
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

function this:updatePersonalPhotos()
    this.currUserPhotosData = PlatformUserProxy:GetInstance():getUserPhotosData()
    if this.currUserPhotosData == nil then
        return
    end
    --设置顶部位置用户自定义的五张图片
    --printDebug("++++++++++++++++++++++++this.currUserPhotosData:"..table.tostring(this.currUserPhotosData))
    local temp = this.currUserPhotosData.album_pic_info_list
    if temp == nil or #temp == 0 or temp[1] == "nil" then
        self.main_mid.add_Image.gameObject:SetActive(true)
        self.main_mid.pic_CellGroup.gameObject:SetActive(false)
        self.main_mid.add_Image:AddEventListener(
            UIEvent.PointerClick,
            function()
                local picName = string.format("%s/%s", ImageType.Photo, getUUID())
                showBottomCameraChoose(
                    picName,
                    false,
                    false,
                    function()
                        PlatformUserModule.sendReqAddAlbumPic({{url = picName, id = 0, sort_id = 1}})
                    end
                )
            end
        )
    else
        self.main_mid.add_Image.gameObject:SetActive(false)
        self.main_mid.pic_CellGroup.gameObject:SetActive(true)
        local pics = {}
        for i = 1, 4 do
            if temp[i] ~= nil and temp[i] ~= "nil" then
                table.insert(pics, temp[i])
            end
        end
        if table.empty(pics) then
            this.main_mid.add_Image.gameObject:SetActive(true)
            this.main_mid.pic_CellGroup.gameObject:SetActive(false)
            this.main_mid.add_Image:AddEventListener(
                UIEvent.PointerClick,
                function()
                    local picName = string.format("%s/%s", ImageType.Photo, getUUID())
                    showBottomCameraChoose(
                        picName,
                        false,
                        false,
                        function()
                            PlatformUserModule.sendReqAddAlbumPic({{url = picName, id = 0, sort_id = 1}})
                        end
                    )
                end
            )
        else
            self.main_mid.pic_CellGroup:SetCellData(pics, self.onSetPics, true)
        end
    end
end

--设置用户图片
function this.onSetPics(go, data, index)
    local item = this.main_mid.piccellArr[index + 1]
    downloadResizeImage(
        data.url,
        item.item_Image,
        ResizeType.MinFit,
        174,
        174,
        "",
        1,
        function(sprite)
            PlatformPhotoDisplayView.adapterImage(item.item_Image, 174, 174)
        end
    )
end

function this:onBtnDiamondEvent()
    RechargeManager.openShop(1, 0)
end

function this:onBtnGoldEvent()
    RechargeManager.openShop(2, 0)
end

function this:onBtnYoCardEvent()
    RechargeManager.openShop(3, 0)
end

--PersonalChange公用部分结束
