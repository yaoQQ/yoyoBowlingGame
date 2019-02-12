
require "base:enum/UIViewEnum"
require "base:mid/personal/Mid_platform_photo_display_panel"
require "base:enum/PlatformFriendType"

local UIExEventTool = CS.UIExEventTool
local RectTransformUtility = CS.UnityEngine.RectTransformUtility
local Color = CS.UnityEngine.Color

PlatformPhotoDisplayView = BaseView:new()
local this = PlatformPhotoDisplayView
this.viewName = "PlatformPhotoDisplayView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Photo_Display_View, true)

--设置加载列表
this.loadOrders=
{
	"base:personal/platform_photo_display_panel",
}

-- 当前选择的相片
this.curSelectedPhoto = nil
-- 各个相框边界
this.photoFrameBoundDic = nil
-- 删除栏的边界
this.deleteBound = nil
--- 相片的协议数据列表(List<common.MsgAlbumPicInfo>)
this.photoMsgDataList = nil

this.photoViewList = nil
this.DELETE_POS = nil
this.photoGroupGo = nil
--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid = Mid_platform_photo_display_panel
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:initPhotoFrameBound()
	self:onAfterDeleteHandler()
	self:addEvent()
	--self.main_mid.platform_photo_display_panel = self.main_mid.go:GetComponent(typeof(CS.PanelWidget))
end

function this:onShowHandler(msg)
	this:updatePhotoDisplay()
	this:addNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_AlbumPicList, this.onChangeSuccess)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_AlbumPicList, this.onChangeSuccess)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this.onChangeSuccess()
	this:updatePhotoDisplay()
	this:onAfterDeleteHandler()
end

function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Photo_Display_View)
	end)
	for i = 1, #this.photoViewList do
		local image = this.photoViewList[i].photo_image
		image:AddExEventListener(UIEvent.PointerShortClick, this.onPhotoClickHandler)
		image:AddEventListener(UIEvent.DragBegin, this.onPhotoBeginDragHandler)
		image:AddEventListener(UIEvent.Drag, this.onPhotoDragHandler)
		image:AddEventListener(UIEvent.PointerUp, this.onPhotoUpHandler)
		image:AddEventListener(UIEvent.PointerLongClick, this.onPhotoLongClickHandler)
	end
	for i = 1, #this.main_mid.photoFrameItemArr do
		this.main_mid.photoFrameItemArr[i].photo_frame_image:AddEventListener(UIEvent.PointerClick, this.onFrameClickHandler)
	end

	--test
	--GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformPhotoDisplayViewInput")
	--GlobalTimeManager.Instance.timerController:AddTimer("PlatformPhotoDisplayViewInput", -1, -1, function ()
	--	if Input.GetKeyDown(KeyCode.D) then
	--		PlatformPhotoDisplayView:testDeleteAllPhoto()
	--	end
	--end)
end

-- 初始化相框边界
function this:initPhotoFrameBound()
	this.photoFrameBoundDic = {}
	this.photoViewList = {}
	this.deleteBound = {}
	for i = 1, #this.main_mid.photoFrameItemArr do
		local item = {}
		item.key = i
		item.value = this.main_mid.photoFrameItemArr[i].go.transform:GetComponent(typeof(CS.UnityEngine.RectTransform))
		this.photoFrameBoundDic[tostring(i)] = item
	end
	--删除栏的边界
	this.deleteBound = this.main_mid.delete_image.rectTransform
	--printDebug("photoFrameBoundDic = "..table.tostring(this.photoFrameBoundDic))
	for i = 1, #this.main_mid.photoItemArr do
		local item = this.main_mid.photoItemArr[i]
		item.rectTransform = this.main_mid.photoItemArr[i].go:GetComponent(typeof(CS.UnityEngine.RectTransform))
		item.photo_image.gameObject.name = tonumber(i)
		item.photoRectTransform = item.photo_image.gameObject:GetComponent(typeof(CS.UnityEngine.RectTransform))
		table.insert(this.photoViewList, item)
	end
	this.DELETE_POS = this.main_mid.delete_image.gameObject.transform.localPosition
	--printDebug("photoViewList = "..table.tostring(this.photoViewList))
	this.photoGroupGo = this.main_mid.go.transform:Find("PhotoGroup").gameObject
end

function this:trySelectedPhoto(index, view)
	if this.curSelectedPhoto == nil then
		this.curSelectedPhoto = {}
		this.curSelectedPhoto.photo = view
		this.curSelectedPhoto.index = index
		this.curSelectedPhoto.data = this.photoMsgDataList[index]
		this.curSelectedPhoto.photoRectTransform = view.photoRectTransform
		this.curSelectedPhoto.rectTransform = view.go.transform:GetComponent(typeof(RectTransform))
		this.curSelectedPhoto.rectTransform.sizeDelta = Vector2(300, 300)
		this.adapterImage(view.photo_image, 300, 300)
		this.curSelectedPhoto.isDelete = false
		this.photoMsgDataList[index] = nil
		return true
	else
		printError("选择相片错误, 已经选择过了!")
		return false
	end
end

function this.moveSelectedPhotoPosition(pos)
	if this.curSelectedPhoto == nil then
		return
	end
	this.curSelectedPhoto.rectTransform.anchoredPosition = pos
end


function this:getIndexFromPointer(eventData)
	local index = -1;
	for k, v in pairs(this.photoFrameBoundDic) do
		if (RectTransformUtility.RectangleContainsScreenPoint(v.value, eventData.position, eventData.pressEventCamera)) then
			index = v.key
			break;
		end
	end
	return index
end

function this:onSelectPhotoHandler()
	self.main_mid.delete_image.gameObject:SetActive(true)
	self.main_mid.delete_image.rectTransform:DOLocalMoveY(this.DELETE_POS.y - self.main_mid.delete_image.rectTransform.sizeDelta.y, 0.1):From()
	self.main_mid.delete_image.Img.color =  Color.white
	self.main_mid.delete_confirm_icon:ChangeIcon(0)
	self.main_mid.delete_confirm_text.text = "拖动到此处删除"
	self.main_mid.delete_confirm_text.color = Color.red
end

function this:onDeleteBoundHandler(isIn)
	if isIn then
		self.main_mid.delete_confirm_icon:ChangeIcon(1)
		self.main_mid.delete_image.Img.color = Color.red
		self.main_mid.delete_confirm_text.text = "松开手即可删除"
		self.main_mid.delete_confirm_text.color = Color.white
	else
		self.main_mid.delete_image.Img.color = Color.white
		self.main_mid.delete_confirm_icon:ChangeIcon(0)
		self.main_mid.delete_confirm_text.text = "拖动到此处删除"
		self.main_mid.delete_confirm_text.color = Color.red
	end
end

function this:onAfterDeleteHandler()
	self.main_mid.delete_image.gameObject:SetActive(false)
end

function this.onPhotoLongClickHandler(eventData)
	local index = tonumber(eventData.pointerCurrentRaycast.gameObject.name)
	if(type(index) ~= "number") then
		return
	end
	if this:trySelectedPhoto(index, this.photoViewList[index]) then
		--printDebug("选择照片成功, 当前选择 ="..table.tostring(this.curSelectedPhoto))
		this:onSelectPhotoHandler()
		this.curSelectedPhoto.rectTransform:SetAsLastSibling()
		local localPos = UIExEventTool.ScreenToTargetLocal(eventData.pointerCurrentRaycast.gameObject, this.photoGroupGo, eventData.position, eventData.pressEventCamera)
		if(localPos ~= Vector2(99999, 99999)) then
			this.moveSelectedPhotoPosition(localPos)
		end
	end
end

function this.onPhotoUpHandler(eventData)
	if this.curSelectedPhoto ~= nil then
		if this.curSelectedPhoto.isDelete then
			local deleteList = {}
			table.insert(deleteList, this.curSelectedPhoto.data.id)
			printDebug("请求删除相片, data = "..table.tostring(this.curSelectedPhoto.data))
			PlatformUserModule.sendReqDelAlbumPic(deleteList)
			this.curSelectedPhoto = nil
		else
			local originRt = this.photoFrameBoundDic[tostring(this.curSelectedPhoto.index)].value
			--printDebug(string.format("松开, 返回索引=%s",this.curSelectedPhoto.index))
			this.photoMsgDataList[this.curSelectedPhoto.index] = this.curSelectedPhoto.data
			this.curSelectedPhoto.rectTransform.sizeDelta = originRt.sizeDelta
			this.adapterImage(this.curSelectedPhoto.photo.photo_image, originRt.sizeDelta.x, originRt.sizeDelta.y)
			this.moveSelectedPhotoPosition(originRt.anchoredPosition)
			--printDebug("松开, 现在照片数据 = "..table.tostring(this.photoMsgDataList))
			local sortList = {}
			for i = 1, #this.photoMsgDataList do
				local item = {}
				item.id = this.photoMsgDataList[i].id
				item.sort_id = i
				table.insert(sortList, item)
			end
			--printDebug("sortList = "..table.tostring(sortList))
			PlatformUserModule.sendReqMoidfyAlbumPic(sortList)
			this.curSelectedPhoto = nil
		end
	end
	this:onAfterDeleteHandler()
end

local m_PointerStartLocalCursor = Vector2.zero
local m_ContentStartPosition = Vector2.zero
function this.onPhotoBeginDragHandler(eventData)
	if this.curSelectedPhoto == nil then
		return
	end
	m_PointerStartLocalCursor = Vector2.zero;
	local value
	value, m_PointerStartLocalCursor = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.main_mid.PhotoGroup.rectTransform, eventData.position, eventData.pressEventCamera, m_PointerStartLocalCursor);
	m_ContentStartPosition = this.curSelectedPhoto.rectTransform.anchoredPosition

end

function this.onPhotoDragHandler(eventData)
	-- 移动视图
	local function switchView(from, to)
		local viewTargetRt = this.photoFrameBoundDic[tostring(to)].value
		local time = 0.3
		local targetPos = viewTargetRt.anchoredPosition
		this.photoViewList[from].go.transform:DOLocalMove(Vector3(targetPos.x, targetPos.y, 0), time)
		this.photoViewList[from].rectTransform.sizeDelta = viewTargetRt.sizeDelta
		this.adapterImage(this.photoViewList[from].photo_image, viewTargetRt.sizeDelta.x, viewTargetRt.sizeDelta.y)
		this.photoViewList[from].photo_image.gameObject.name = tostring(to)
		this.photoViewList[to].photo_image.gameObject.name = tostring(from)
		local temp = this.photoViewList[from]
		this.photoViewList[from] = this.photoViewList[to]
		this.photoViewList[to] = temp
	end
	this.checkInDeleteBound(eventData)
	if this.curSelectedPhoto~= nil then
		--local curPos = this.curSelectedPhoto.rectTransform.anchoredPosition
		--local targetPos = curPos + eventData.delta
		local localCursor
		local value, localCursor = RectTransformUtility.ScreenPointToLocalPointInRectangle(this.main_mid.PhotoGroup.rectTransform, eventData.position, eventData.pressEventCamera, localCursor)
		if value == false then
			return
		end
		local pointerDelta = localCursor - m_PointerStartLocalCursor;
		local position = m_ContentStartPosition + pointerDelta

		this.moveSelectedPhotoPosition(position)
		local targetIndex = this:getIndexFromPointer(eventData)
		local originIndex = this.curSelectedPhoto.index
		--printDebug("targetIndex = "..targetIndex)
		if targetIndex == -1 then
			return
		end
		if targetIndex == originIndex then
			return
		end
		if table.empty(this.photoMsgDataList[targetIndex])then
			--printDebug("移到一个空白相框")
			return
		end
		--printDebug(string.format("更换位置(%s->%s)", originIndex, targetIndex))
		if(originIndex < targetIndex) then
			-- 从低位移到高位
			for i = originIndex + 1 , targetIndex do
				-- 更新数据
				if(this.photoMsgDataList[i - 1] == nil) then
					this.photoMsgDataList[i - 1] = this.photoMsgDataList[i]
					this.photoMsgDataList[i] = nil
				end
				-- 更新视图
				--printDebug(string.format("移动视图:(%s->%s)", i, i - 1))
				switchView(i , i - 1)
			end
		else
			for i = originIndex - 1, targetIndex, -1 do
				if(this.photoMsgDataList[i + 1] == nil) then
					this.photoMsgDataList[i + 1] = this.photoMsgDataList[i]
					this.photoMsgDataList[i] = nil
				end
				--printDebug(string.format("移动视图:(%s->%s)", i, i + 1))
				switchView(i , i + 1)
			end
		end
		this.photoMsgDataList[targetIndex] = nil
		this.curSelectedPhoto.index = targetIndex
	end

end

function this.checkInDeleteBound(eventData)
	if this.curSelectedPhoto == nil or this.deleteBound == nil then
		return
	end
	local isIn = false
	local bottomPoint = Vector2(eventData.position.x, eventData.position.y - this.curSelectedPhoto.rectTransform.sizeDelta.y / 2)
	local centerPoint = Vector2(eventData.position.x, eventData.position.y)
	local topPoint = Vector2(eventData.position.x, eventData.position.y + this.curSelectedPhoto.rectTransform.sizeDelta.y / 2)
	local isBottomIn = RectTransformUtility.RectangleContainsScreenPoint(this.deleteBound, bottomPoint, eventData.pressEventCamera)
	local isCenterIn = RectTransformUtility.RectangleContainsScreenPoint(this.deleteBound, centerPoint, eventData.pressEventCamera)
	local isTopIn = RectTransformUtility.RectangleContainsScreenPoint(this.deleteBound, topPoint, eventData.pressEventCamera)
	if isBottomIn or isCenterIn or isTopIn  then
		isIn = true
	else
		isIn = false
	end
	if not this.curSelectedPhoto.isDelete and isIn then
		--printDebug("移入 删除栏")
		this.curSelectedPhoto.isDelete = true
	elseif this.curSelectedPhoto.isDelete and not isIn then
		--printDebug("移出 删除栏")
		this.curSelectedPhoto.isDelete = false
	end
	this:onDeleteBoundHandler(this.curSelectedPhoto.isDelete)
end

function this.onPhotoClickHandler(eventData)
	if eventData.pointerCurrentRaycast.gameObject == nil then
		--printError("错误")
		return
	end
	local index = tonumber(eventData.pointerCurrentRaycast.gameObject.name)
	if(type(index) ~= "number") then
		return
	end
	--printDebug("短按, index = "..index)
	--printDebug("短按的照片数据 = "..table.tostring(this.photoMsgDataList[index]))
	ViewManager.open(UIViewEnum.Enlarge_Photo_View, nil, function ()
		EnlargePhotoView:updatePhotoView(index - 1)
	end)
end

function this.onFrameClickHandler(eventData)
	local lastSortId = this:getLastSortId()
	local sortId = lastSortId + 1
	local picName = string.format("%s/%s", ImageType.Photo, getUUID())
	showBottomCameraChoose(picName, false, false, function ()
		PlatformUserModule.sendReqAddAlbumPic({{url = picName, id = 0, sort_id = sortId}})
	end)
end

function this:getLastSortId()
	if table.empty(this.photoMsgDataList) then
		return 0
	end
	return this.photoMsgDataList[#this.photoMsgDataList].sort_id
end

function this:testDeleteAllPhoto()
	print("测试用, 删除所有照片")
	local deleteList = {}
	for i, v in pairs(this.photoMsgDataList) do
		table.insert(deleteList, v.id)
	end
	PlatformUserModule.sendReqDelAlbumPic(deleteList)
end

--更新照片显示
function this:updatePhotoDisplay()
	this.photoMsgDataList = {}
	this.photoMsgDataList = PlatformUserProxy:GetInstance():getUserPhotosData().album_pic_info_list
	if this.photoMsgDataList == nil then
		for i = 1, #this.photoViewList do
			local photoView = this.photoViewList[i]
			local frameView = this.photoFrameBoundDic[tostring(i)].value
			photoView.photo_image.gameObject:SetActive(false)
			photoView.rectTransform.anchoredPosition = frameView.anchoredPosition
		end
		return
	end
	table.sort(this.photoMsgDataList, function (a, b)
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
	printDebug("相片协议结构逻辑数据 = "..table.tostring(this.photoMsgDataList))
	self:updatePhotoList()
end

function this:updatePhotoList()
	for i = 1, #this.photoViewList do
		local photoData = this.photoMsgDataList[i]
		local photoView = this.photoViewList[i]
		local frameView = this.photoFrameBoundDic[tostring(i)].value
		photoView.rectTransform.anchoredPosition = frameView.anchoredPosition
		photoView.rectTransform.sizeDelta = frameView.sizeDelta
		if table.empty(photoData) then
			photoView.photo_image.gameObject:SetActive(false)
		elseif photoData.url == "" then
			Loger.PrintError("错误, 照片存在但名字为空串,photoData: "..table.tostring(photoData))
		else
			photoView.photo_image.gameObject:SetActive(true)
			downloadResizeImage(photoData.url, photoView.photo_image, ResizeType.MinFit, 330, 330, "", 2,  function (sprite)
				this.adapterImage(photoView.photo_image, frameView.sizeDelta.x, frameView.sizeDelta.y)
			end)
		end
	end
end

-- 照片适配, 模式: 短边适配, 长边按比例
---@param image CS.ImageWidget
function this.adapterImage(image, w, h)
	if image == nil or image.Img == nil or image.Img.sprite == nil then
		printError("错误, 照片适配参数错误")
		return
	end
	local short = math.min(image.Img.sprite.rect.width, image.Img.sprite.rect.height)
	local targetW = 0
	local targetH = 0
	if short == image.Img.sprite.rect.width then
		local r = w / short
		targetW = w
		targetH = image.Img.sprite.rect.height * r
	else
		local r = h / short
		targetW = image.Img.sprite.rect.width * r
		targetH = h
	end
	image.rectTransform.sizeDelta = Vector2(targetW, targetH)
end