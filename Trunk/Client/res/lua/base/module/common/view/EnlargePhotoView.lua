

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_enlarge_photo_panel"

local UICamera = CS.UIManager.Instance.UICamera
local Mathf = CS.UnityEngine.Mathf

EnlargePhotoView = BaseView:new()
local this = EnlargePhotoView
this.viewName = "EnlargePhotoView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Enlarge_Photo_View, true)

--设置加载列表
this.loadOrders=
{
	"base:common/enlarge_photo_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:onShowHandler(msg)
	GlobalTimeManager.Instance.timerController:AddTimer("EnlargePhotoInput", -1, -1, this.onInput)

end

function this:addEvent()
	self.main_mid.back_image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Enlarge_Photo_View)
	end)
	self.main_mid.Banner:AddExEventListener(UIEvent.PointerDoubleClick, this.onDoubleClickHandler)
	self.main_mid.Banner:AddPinchEventListener(UIEvent.PinchIn, this.onPinchInHandler)
	self.main_mid.Banner:AddPinchEventListener(UIEvent.PinchOut, this.onPinchOutHandler)
	self.main_mid.Banner.onPointerUpEvent = this.onPointerUpEventHandler
	self.main_mid.down_image.gameObject:SetActive(false)
	--self.main_mid.down_image:AddEventListener(UIEvent.PointerClick,function ()
	--	showFloatTips("功能开发中敬请期待！")
	--end)
end

function this:onClose()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("EnlargePhotoInput")
end

this.lowSpriteDic = {}
this.highSpriteDic = {}
function this:updatePhotoView(startIndex)
	local dataList = PlatformUserProxy:GetInstance():getUserPhotosData().album_pic_info_list
	if dataList == nil then
		return
	end
	for i = 1, #dataList do
		--local typeKey = string.lower(ResizeType.MinFit)
		--local key = string.format("%s_%s_%s", dataList[i].url, typeKey, 2)
		local key = dataList[i].url
		local mySprite = PlatformPicManagerProxy:GetInstance():getSprite(key)
		self.lowSpriteDic[dataList[i].url] = mySprite
	end
	self.main_mid.Banner:SetBannerData(dataList, startIndex, function (go, data, index)
		local viewList = self.main_mid.Banner.viewList
		self:showPhotoView(dataList[index + 1 - 1], viewList[0], false)
		self:showPhotoView(dataList[index + 1], viewList[1], true)
		self:showPhotoView(dataList[index + 1 + 1], viewList[2], false)
		self.main_mid.progress_text.text = string.format("%s/%s", index + 1, #dataList)
	end)
end

function this.adaptiveImage(rt, sprite)
	local ratio = 1080 / sprite.rect.width
	rt.sizeDelta = Vector2(1080, sprite.rect.height * ratio)
end

function this:showPhotoView(data, view, tryDownHigh)
	local image = view:GetComponent(typeof(ImageWidget))
	if data == nil or image == nil then
		return
	end
	image:ActiveLoadImage(false)
	--print("尝试显示图片, url = "..data.url)
	local function downloadHigh(rectTransform, image)
		if not tryDownHigh then
			if(image.loadingImg ~= nil)then
				image.loadingImg.gameObject:SetActive(false)
			end
			return
		end
		--print("尝试下载高清图片, url = "..data.url)
		downloadResizeImage(data.url, image, ResizeType.SingleSideWidth, 1080, 1920,"", 3, function (sprite)
			this.adaptiveImage(rectTransform, sprite)
			self.highSpriteDic[data.url] = sprite
		end)
	end
	local rt = view
	local highSprite = self.highSpriteDic[data.url]
	if highSprite == nil then
		local lowSprite = self.lowSpriteDic[data.url]
		image:SetPng(lowSprite)
		if lowSprite == nil then
			--downloadHigh(rt, image)
		else
			this.adaptiveImage(rt, lowSprite)
			--downloadHigh(rt, image)
		end
	else
		image:SetPng(highSprite)
		if(image.loadingImg ~= nil)then
			image.loadingImg.gameObject:SetActive(false)
		end
		this.adaptiveImage(rt, highSprite)
	end
end

function this:deletePhoto(name)
	local function destroySprite(sprite)
		if sprite == nil then
			return
		end
		CS.UnityEngine.Object.Destroy(sprite.texture)
		CS.UnityEngine.Object.Destroy(sprite)

	end
	--print("删除相册里的相片, name = "..name)
	--print("删除相册里的相片, lowSpriteDic = "..table.tostring(self.lowSpriteDic))
	--print("删除相册里的相片, highSpriteDic = "..table.tostring(self.highSpriteDic))
	local lowSprite = self.lowSpriteDic[name]
	local highSprite = self.highSpriteDic[name]
	destroySprite(lowSprite)
	destroySprite(highSprite)
	self.lowSpriteDic[name] = nil
	self.highSpriteDic[name] = nil
end

function this.onDoubleClickHandler(eventData)
	local m_Content = this.main_mid.Banner.content
	local viewRect = this.main_mid.Banner.viewRect
	if m_Content.localScale == Vector3.one then
		-- 缩放规则
		local scale = 1
		if m_Content.rect.width >= 1080 then
			if m_Content.rect.height / m_Content.rect.width > 1 then
				scale = 2
			else
				scale = viewRect.rect.height / m_Content.rect.height
			end
		else
			scale = 2;
		end
		this.main_mid.Banner:ContentAnimaTo(scale, eventData.position, eventData.pressEventCamera)
	else
		this.main_mid.Banner:ContentAnimaReset()
	end
end

function this.onPinchInHandler(eventData)
	this:photoShrink(Vector2(1080/2, 1920/2))
end

function this.onPinchOutHandler(eventData)
	this:photoEnlarge(Vector2(1080/2, 1920/2))
end

function this.onPointerUpEventHandler()
	local m_Content = this.main_mid.Banner.content
	if m_Content.localScale.x < 1 or m_Content.localScale.y < 1 then
		m_Content:DOScale(Vector3.one, 0.2)
	end
end

function this:photoEnlarge(position)
	local m_Content = this.main_mid.Banner.content
	local curScale = m_Content.localScale.x
	local targetScale = curScale * 1.1
	targetScale = Mathf.Clamp(targetScale, 1.0, 5.0)
	this.main_mid.Banner:ContentAnimaTo(targetScale, position, UICamera)
end

function this:photoShrink(position)
	local m_Content = this.main_mid.Banner.content
	local curScale = m_Content.localScale.x
	local targetScale = curScale / 1.1
	targetScale = Mathf.Clamp(targetScale, 0.7, 5.0)
	this.main_mid.Banner:ContentAnimaTo(targetScale, position, UICamera)
end

function this.onInput()
	if IS_UNITY_EDITOR then
		--滚轮缩放
		local wheelInput = Input.GetAxis("Mouse ScrollWheel")
		if wheelInput > 0 then
			local position = Vector2(Input.mousePosition.x, Input.mousePosition.y)
			this:photoEnlarge(position)
		elseif wheelInput < 0 then
			local position = Vector2(Input.mousePosition.x, Input.mousePosition.y)
			this:photoShrink(position)
		end
	end
end