﻿AliyunOSSManager = CS.AliyunOSSManager
PhotoManager = {}

function PhotoManager.decodePhotoUrl(url)
	local result1 = string.split(url,'/')
	if result1 == nil then return end

	local result2 = string.split(result1[#result1],'.')
	if result2 == nil then return end

	local result3 = string.split(result2[1],'_')
	if result3 == nil then return end

	local result4 = string.split(result3[1],'@')
	if result4 == nil then return end
	
	return result4[1]
end

--上传一张带目录的图到阿里云服务器
--finishCallback返回参数为bool,true为上传成功, false为上传失败
---@param photoName string
---@param bytes CS.byte[]
---@param imageType string
---@param finishCallback  Action<bool>
function PhotoManager.uploadImage(photoName, bytes, imageType, finishCallback)
	AliyunOSSManager.Instance:UploadImage(photoName..".jpg", bytes, imageType, finishCallback)
end

--从阿里云服务器下载带目录的图
--finishCallback返回参数为Texture2D,返回参数为nil代表下载失败
---@param photoName string
---@param imageType string
---@param finishCallback  Action<Texture2D, string>
function PhotoManager.downloadImage(photoName, finishCallback)
	AliyunOSSManager.Instance:DownloadImage(photoName..".jpg", LoginDataProxy.playerId, finishCallback)
end

--从阿里云服务器下载带目录的缩略图
--finishCallback返回参数为Texture2D,返回参数为nil代表下载失败
---@param photoName string
---@param resizeType string
---@param width int
---@param height int
---@param color string(colorCode)
---@param finishCallback  Action<Texture2D, string>
function PhotoManager.downloadResizeImage(photoName, resizeType, width, height, color, finishCallback)
	local process = ""
	if resizeType == ResizeType.LengthFit then
		process = string.format("image/resize,m_lfit,h_%s,w_%s", height, width)
	elseif resizeType == ResizeType.Fill then
		process = string.format("image/resize,m_fill,h_%s,w_%s", height, width)
	elseif resizeType == ResizeType.Pad then
		if color == nil or color == "" then
			process = string.format("image/resize,m_pad,h_%s,w_%s", height, width)
		else
			process = string.format("image/resize,m_pad,h_%s,w_%s,color_%s", height, width, color)
		end
	elseif resizeType == ResizeType.MinFit then
		process = string.format("image/resize,m_mfit,h_%s,w_%s", height, width)
	elseif resizeType == ResizeType.SingleSideWidth then
		process = string.format("image/resize,w_%s", width)
	elseif resizeType == ResizeType.SingleSideHeight then
		process = string.format("image/resize,h_%s", height)
	else
		process = string.format("image/resize,m_fixed,h_%s,w_%s", height, width)
	end
	AliyunOSSManager.Instance:DownloadResizeImage(photoName..".jpg", resizeType, process, LoginDataProxy.playerId, finishCallback)
end

--下载网络照片（非阿里云）
--finishCallback返回参数为Texture2D,返回参数为nil代表下载失败
function PhotoManager.downloadNetPhoto(url, finishCallback)
	HttpPostManager.Instance:DownloadPhoto(url, true, finishCallback)
end