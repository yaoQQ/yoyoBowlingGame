
require "base:enum/UIViewEnum"
require "base:mid/personal/Mid_personal_change_info_panel"
require "base:enum/PlatformFriendType"
local UIExEventTool = CS.UIExEventTool

PersonalChangeInfoView = BaseView:new()
local this = PersonalChangeInfoView
this.viewName = "PersonalChangeInfoView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Personal_Change_Info_View, true)

--设置加载列表
this.loadOrders=
{
	"base:personal/personal_change_info_panel",
}

--初始化预制体，给main_mid赋值
local isParent = false
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid = Mid_personal_change_info_panel
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:onShowHandler(msg)
	printDebug("=====================Personal_Change_Info_View调用完毕======================")
	self:initView()
	if msg then
		isParent = msg.isParent
	end
	this:updateChangeList()
	this:addNotice()
end
function this:initView()
	MainModule.sendReqAlbumPicList(LoginDataProxy.playerId)
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
	--Personal Change 临时
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateSelfPhotos)
	--Personal Change
end


function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
	--Personal Change 临时
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_AlbumPicList, this.updateSelfPhotos)
	--Personal Change
end


--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end
this.currPersonalData = nil
this.tempData = nil
local m_isSuccess = false

function this.onChangeSuccess()
	this:updateChangeList()
	ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
end


function this:addEvent()

	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		if isParent then
			ViewManager.open(UIViewEnum.Platform_Mall_View)
		end
		ViewManager.close(UIViewEnum.Personal_Change_Info_View)
	end)

	self.main_mid.set_Image:AddEventListener(UIEvent.PointerClick,function ( ... )
		ViewManager.open(UIViewEnum.Platform_Set_MainView)
	end)

	--self.main_mid.pic_CellGroup:AddEventListener(UIEvent.PointerClick,function ( ... )
	--	ViewManager.open(UIViewEnum.Platform_Photo_Display_View)
	--end)
end



--更新好友申请列表
function this:updateChangeList()
	this.currPersonalData = PlatformUserProxy:GetInstance():getUserInfo()

	-- printDebug("=======================更新用户资料列表1！！！！"..table.tostring(this.currPersonalData))
	if this.currPersonalData == nil then return end

	-- printDebug("=======================更新用户资料列表2！！！！"..table.tostring(this.currPersonalData))

	this.tempData = {1,2,3,4,5,6}

	self.main_mid.change_CellGroup:SetCellData(this.tempData,self.updateMyChangeList,false)
end

-- this.currOpFriendId = nil
-- this.currOpFriendCell = nil
this.configData=
{
   chooseType = {"重新定位","消除位置"},    
}

function this.updateMyChangeList(go,data,index)

	local item =  this.main_mid.changeCellArr[index+1]

	if data == 1 then

		item.type_Text.text = "头像"
		item.head_CircleImage.gameObject:SetActive(true)
		item.desc_Text.gameObject:SetActive(false)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		--显示用户的头像
		downloadUserHead(this.currPersonalData.head_url,item.head_CircleImage)

		item.press_Image:AddEventListener(UIEvent.PointerClick,function ()
			local picName = string.format("%s/%s", ImageType.UserHead, getUUID())
			showBottomAndCamera(picName,function ()
				-- body
				PlatformUserModule.sendReqChangeUserBaseInfo({head_url = picName})
			end)
		end) 

	elseif data == 2 then
		item.type_Text.text = "昵称"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(true)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		--显示用户的昵称
		item.desc_Text.text =this.currPersonalData.nick_name

		item.press_Image:AddEventListener(UIEvent.PointerClick,function ()
			--printDebug("++++++++++++++++++点击了修改用户昵称按钮")
			ViewManager.open(UIViewEnum.Single_Change_Info_View,{changeType = 1})
		end)

	--[[elseif data == 3 then

		item.type_Text.text = "荣誉展示"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(false)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(true)

		--显示用户的荣誉
--]]
	elseif data ==3 then

		item.type_Text.text = "用户ID"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(true)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		item.change_Image.gameObject:SetActive(false)

		--显示用户的id
		item.desc_Text.text = "ID:"..this.currPersonalData.player_id

	elseif data ==4 then

		item.type_Text.text = "性别"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(true)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		item.press_Image:AddEventListener(UIEvent.PointerClick,function ()
			ViewManager.open(UIViewEnum.Single_Change_Info_View,{changeType = 2})
		end)

		--item.sex_bg_Icon:ChangeIcon(this.currPersonalData.sex -1)
		--item.sex_Icon:ChangeIcon(this.currPersonalData.sex - 1)

		if this.currPersonalData.sex == 1 then
			item.desc_Text.text = "男"
		else
			item.desc_Text.text = "女"
		end

	elseif data ==5 then

		item.type_Text.text = "生日"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(true)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		printDebug("----------this.currPersonalData.birthday:"..this.currPersonalData.birthday)
		--显示用户的生日
		--item.desc_Text.text = showChineseDataTime(this.currPersonalData.birthday)
		item.desc_Text.text = os.date("%Y-%m-%d",this.currPersonalData.birthday)
		item.press_Image:AddEventListener(UIEvent.PointerClick,function ()	
			local year = tonumber(os.date("%Y",this.currPersonalData.birthday))
			local month = tonumber(os.date("%m",this.currPersonalData.birthday))
			local day = tonumber(os.date("%d",this.currPersonalData.birthday))
			local maxYear = tonumber(os.date("%Y",os.time()))

			--printDebug("++++++++++++++++++++++年月日为："..year.."/"..month.."/"..day)
--maxYear-120
			local sendMsg = {maxYear = maxYear,minYear =1970 ,defaultYear =year ,defaultMonth = month,defaultDay = day,confirmFunc = function (info)
				
				local data = {}
				if info ~= nil then
					data.birthday = info
				end
				
				PlatformUserModule.sendReqChangeUserBaseInfo(data)
			end}

			ViewManager.open(UIViewEnum.Common_Date_Select_View, sendMsg,function ( ... )
				-- body
				CommonDateSelectView:showDateView(sendMsg)
			end)
		end)

	elseif data ==6 then

		item.type_Text.text = "地区"
		item.head_CircleImage.gameObject:SetActive(false)
		item.desc_Text.gameObject:SetActive(this.currPersonalData.show_address)
		item.sex_Panel.gameObject:SetActive(false)
		item.hornor_Panel.gameObject:SetActive(false)

		--显示用户的地区
		if this.currPersonalData.address ~= nil then
			item.desc_Text.text = this.currPersonalData.address
		end
		item.press_Image:AddEventListener(UIEvent.PointerClick,function ()
		
			local callback = function(index)
				local func = function()
					if index == 1 then --this.main_mid.award_type_Text.text == "排名奖励" then
						local data = {}
						data.address = MapManager.userCountry..MapManager.userProvince..MapManager.userCity
						-- data.address = "广东中山"
						data.show_address = true
						
						PlatformUserModule.sendReqChangeUserBaseInfo(data)
					elseif index == 2 then
						local data = {}
						data.show_address = false
						data.address = MapManager.userCountry..MapManager.userProvince..MapManager.userCity

						PlatformUserModule.sendReqChangeUserBaseInfo(data)
					end        
				end
				return func 
			end 
			local methodGroup = {}
			for i=1,#this.configData.chooseType do
				table.insert(methodGroup, {title_Text=this.configData.chooseType[i], funEvent=callback(i)})
			end  
			
			CommonBottomSelectView:showSelectTipsDownView(methodGroup)

		end)
	end
end

--From GlobalPersonal
function this.updateSelfPhotos()
	this:updatePersonalPhotos()
	ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
end

this.currUserPhotosData = nil

function this:updatePersonalPhotos()
	this.currUserPhotosData = PlatformUserProxy:GetInstance():getUserPhotosData()
	if this.currUserPhotosData == nil then return end
	--设置顶部位置用户自定义的五张图片
	local photoMsgDataList = this.currUserPhotosData.album_pic_info_list

	if table.empty(photoMsgDataList) then
		self.main_mid.add_Image.gameObject:SetActive(true)
		self.main_mid.pic_CellGroup.gameObject:SetActive(false)
		self.main_mid.add_Image:AddEventListener(UIEvent.PointerClick,function ()
			local picName = string.format("%s/%s", ImageType.Photo, getUUID())
			showBottomCameraChoose(picName, false, false, function ()
				PlatformUserModule.sendReqAddAlbumPic({{url = picName, id = 0, sort_id = 1}})
			end)
		end)
	else
		printDebug("photoMsgDataList = "..table.tostring(photoMsgDataList))
		self.main_mid.add_Image.gameObject:SetActive(false)
		self.main_mid.pic_CellGroup.gameObject:SetActive(true)
		table.sort(photoMsgDataList, function (a, b)
			local left = a.sort_id
			local right = b.sort_id
			if left == nil or right  == nil then
				return false
			end
			if left == right then
				return false
			end
			return left < right
		end)
		local showPhotos = {}
		local photoCount = #photoMsgDataList
		if photoCount <= 4 then
			showPhotos = photoMsgDataList
		else
			for i = 1, 4 do
				table.insert(showPhotos, photoMsgDataList[i])
			end
		end
		self.main_mid.pic_CellGroup:SetCellData(showPhotos, self.onSetPics, true)
		for i = 1, #self.main_mid.piccellArr do
			local item = self.main_mid.piccellArr[i]
			if i == #showPhotos + 1 then
				--print("显示该块的更多图, i = "..i)
				item.go:SetActive(true)
				item.item_Image.gameObject:SetActive(false)
				item.more_Image.gameObject:SetActive(true)
				item.more_Image:AddEventListener(UIEvent.PointerClick,function ()
					ViewManager.open(UIViewEnum.Platform_Photo_Display_View)
				end)
			end
		end
	end
end


--设置用户图片
function this.onSetPics(go, data, index)
	local item = this.main_mid.piccellArr[index+1]
	item.go:SetActive(true)
	item.item_Image.gameObject:SetActive(true)
	item.more_Image.gameObject:SetActive(false)
	item.item_Image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
	downloadResizeImage(data.url, item.item_Image, ResizeType.MinFit, 174, 174,"", 1,  function (sprite)
		item.item_Image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
		PlatformPhotoDisplayView.adapterImage(item.item_Image, 174, 174)
	end)
	item.item_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.open(UIViewEnum.Platform_Photo_Display_View)
	end)
end

--From GlobalPersonal End
function this:changeNameEnd()
	showFloatTips("昵称修改成功")
end