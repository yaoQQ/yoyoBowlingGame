
---图片类型, 用来区分远程图片库上的目录
ImageType =
{
    Root            = "",           --根目录
    Photo           = "Photo",      --照片
    Publicity       = "Publicity",  --(商品)宣传图
    Face            = "Face",       --(聊天)表情
    Coupon          = "Coupon",     --优惠券图
    RedPacket       = "RedPacket",  --红包图
    MerchantHead    = "MerchantHead",--商家头像
    UserHead        = "UserHead",    --用户头像
    GameIcon        = "GameIcon",   --游戏图标
    Other           = "Other",      --其他
}

--- 图片的处理模式
ResizeType =
{
    LengthFit           = "LengthFit",          --等比缩放, 长边优先
    MinFit              = "MinFit",             --等比缩放, 短边优先
    Fill                = "Fill",               --固定宽高，自动裁剪
    Pad                 = "Pad",                --固定宽高，缩略填充
    SingleSideWidth     = "SingleSideWidth",    --单边缩略, 指定宽, 高按比例
    SingleSideHeight    = "SingleSideHeight",   --单边缩略, 指定高, 宽按比例
}

PlatformPicManagerProxy={}
local this=PlatformPicManagerProxy


function PlatformPicManagerProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformPicManagerProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

this.spriteDic = {}
this.spriteList = {}
function this:addSprite(name, sprite)
    local spriteItem = {}
    spriteItem.name = name
    spriteItem.sprite = sprite
    if this.spriteDic[name] ~= nil then
        for i = #this.spriteList, 1, -1 do
            if this.spriteList[i].name == name then
                table.remove(this.spriteList, i)
            end
        end
        table.insert(this.spriteList, spriteItem)
    else
        if #this.spriteList > 128 then
            local headSpriteItem = this.spriteList[1]
            --print("图元缓存数目超标, 移除首个图元, name = "..headSpriteItem.name)
            this.spriteDic[headSpriteItem.name] = nil
            table.remove(this.spriteList, 1)
            CS.UnityEngine.Object.Destroy(headSpriteItem.sprite.texture)
            CS.UnityEngine.Object.Destroy(headSpriteItem.sprite)
        end
        table.insert(this.spriteList, spriteItem)
    end
    this.spriteDic[name] = spriteItem

end

function this:getSprite(name)
    local item = this.spriteDic[name]
    if item == nil then
        return nil
    end
    for i = #this.spriteList, 1, -1 do
        if this.spriteList[i].name == name then
            table.remove(this.spriteList, i)
        end
    end
    table.insert(this.spriteList, item)
    return item.sprite
end

function this:deleteSprite(name)
    --print("删除图元, name = "..name)
    --print("删除图元, spriteDic = "..table.tostring(this.spriteDic))
    for i = #this.spriteList, 1, -1 do
        if this.spriteList[i].name == name then
            table.remove(this.spriteList, i)
        end
    end
    local item = this.spriteDic[name]
    if item ~= nil  then
        CS.UnityEngine.Object.Destroy(item.sprite.texture)
        CS.UnityEngine.Object.Destroy(item.sprite)
        this.spriteDic[name] = nil
    end

end

this.currIconSprite = nil
function this:setIconSprite(data)
    this.currIconSprite = data

    -- for i=0,this.currIconSprite.Length-1 do
    -- printDebug("+++++++++++++++++++++设置的图像iconSprite为"..this.currIconSprite[i].name)
    -- end
end

--根据Index获取iconSprite,index从0开始
function this:getIconSpriteByIndex(index)
    if this.currIconSprite ==nil then return end

    return this.currIconSprite[index]
end

this.currFaceKeyValue = nil
--设置表情的键值对
function this:setFaceKeyValue(data)
    this.currFaceKeyValue = {}

    if data == nil then return end
    for i=0,data.Length-1 do
        table.insert(this.currFaceKeyValue,data[i])
      --  printDebug("+++++++++++++++++++++设置的图像name为:"..data[i].name.."设置的图像sprite为:"..data[i].sprite.name)
    end
end

--获取表情的键值对
function this:getFaceKeyValue()
    return this.currFaceKeyValue
end

---竞猜第三方用图--
this.guessPicTab = {}
function this:addGuessPic(picName,texture2d)
    this.guessPicTab[picName] = texture2d
end

function this:getGuessPic(picName)
    return this.guessPicTab[picName]
end

--- 上传图片到远程服务器--
---@param photoName string
---@param bytes CS.byte[]
---@param finishCallback  Action<bool>
---@param imageType Lua.ImageType(string)
uploadImage = function (photoName, bytes, imageType, finishCallback)
    PhotoManager.uploadImage(photoName, bytes, imageType, finishCallback)
end

--- 从图片服务器下载用户头像---
downloadUserHead = function (picName, picLocateImg)
    downloadImageOrDefault(picName, picLocateImg, ImageType.UserHead)
end

--- 从图片服务器下载商家头像---
downloadMerchantHead = function (picName, picLocateImg)
    downloadImageOrDefault(picName, picLocateImg, ImageType.MerchantHead)
end

--- 从图片服务器下载带目录的图片---
---@param picName string
---@param picLocateImg ImageWidget
downloadImage = function (picName, picLocateImg)
    if picLocateImg == nil then
        return
    end
    if picName == nil or picName == "" then
        picLocateImg:SetPng(nil)
        return
    end
    local cacheKey = picName
    local mySprite = PlatformPicManagerProxy:GetInstance():getSprite(cacheKey)
    if mySprite == nil then
        --printDebug("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^运行缓存没有图片，图片名字为："..picName)
        picLocateImg:SetPng(nil)
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(true)
        end
        PhotoManager.downloadImage(picName,function (texture2D)
            if texture2D ~= nil then
                local sprite = ImageUtil.CreateSpriteByTexture(texture2D)
                picLocateImg:SetPng(sprite)
                --printDebug("增加本地缓存图片, name = "..key)
                PlatformPicManagerProxy:GetInstance():addSprite(cacheKey, sprite)
                if picLocateImg.loadingImg ~= nil then
                    picLocateImg.loadingImg.gameObject:SetActive(false)
                end
            end
        end)
    else
        --printDebug("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^运行缓存有图片，图片名字为："..picName)
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(false)
        end
        picLocateImg:SetPng(mySprite)
    end
    if not picLocateImg.Img.enabled then
        picLocateImg.Img.enabled = true
    end
end

--- 从图片服务器下载带目录的图片,在下载图片失败时下载默认图片---
---@param picName string
---@param picLocateImg ImageWidget
downloadImageOrDefault = function (picName, picLocateImg, imageType)
    if picLocateImg == nil then
        return
    end
    if picName == nil or picName == "" then
        downloadDefaultImage(picLocateImg, imageType)
        return
    end
    local cacheKey = picName
    local mySprite = PlatformPicManagerProxy:GetInstance():getSprite(cacheKey)
    if mySprite == nil then
        picLocateImg:SetPng(nil)
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(true)
        end
        PhotoManager.downloadImage(picName,function (texture2D)
            if texture2D ~= nil then
                local sprite = ImageUtil.CreateSpriteByTexture(texture2D)
                picLocateImg:SetPng(sprite)
                PlatformPicManagerProxy:GetInstance():addSprite(cacheKey, sprite)
                if picLocateImg.loadingImg ~= nil then
                    picLocateImg.loadingImg.gameObject:SetActive(false)
                end
            else
                --print("下载图片错误, 转而下载默认图片: "..picName)
                downloadDefaultImage(picLocateImg, imageType)
            end
        end)
    else
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(false)
        end
        picLocateImg:SetPng(mySprite)
    end
    if not picLocateImg.Img.enabled then
        picLocateImg.Img.enabled = true
    end
end

downloadDefaultImage = function (picLocateImg, imageType)
    if picLocateImg == nil then
        return
    end
    local relativeDir = imageType
    local defaultName = "default"
    local cacheKey = ""
    if relativeDir == "" then
        cacheKey = defaultName
    else
        cacheKey = string.format("%s/%s", relativeDir, defaultName)
    end
    local mySprite = PlatformPicManagerProxy:GetInstance():getSprite(cacheKey)
    if mySprite == nil then
        picLocateImg.Img.sprite = nil
        PhotoManager.downloadImage(cacheKey,function (texture2D)
            if texture2D ~= nil then
                print("下载默认图片成功")
                local sprite = ImageUtil.CreateSpriteByTexture(texture2D)
                picLocateImg:SetPng(sprite)
                PlatformPicManagerProxy:GetInstance():addSprite(cacheKey, sprite)
            else
                picLocateImg:SetPng(nil)
            end
        end)
    else
        picLocateImg:SetPng(mySprite)
    end
    if not picLocateImg.Img.enabled then
        picLocateImg.Img.enabled = true
    end
end

---@param picName string
---@param picLocateImg CS.ImageWidget
---@param resizeType string
---@param width int
---@param height int
---@param color string(colorCode)
---@param resizeLevel int
downloadResizeImage = function(picName, picLocateImg, resizeType, width, height, color, resizeLevel, callback)
    if picLocateImg == nil then
        return
    end
    if picName == nil or picName == "" then
        picLocateImg:SetPng(nil)
        return
    end
    local function showImageBySprite(img, sprite)
        img.sprite = sprite
        if callback ~= nil  then
            callback(sprite)
        end
    end
    --local cacheKey = string.format("%s_%s_%s", picName, string.lower(resizeType), resizeLevel)
    local cacheKey = picName
    --printDebug("尝试获取本地缓存图片, name = "..key)
    local mySprite = PlatformPicManagerProxy:GetInstance():getSprite(cacheKey)
    if mySprite == nil then
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(true)
        end
        picLocateImg:SetPng(nil)
        PhotoManager.downloadResizeImage(picName, resizeType, width, height, color,function (texture2D)
            if texture2D ~= nil then
                local sprite = ImageUtil.CreateSpriteByTexture(texture2D)
                showImageBySprite(picLocateImg.Img, sprite)
                --printDebug("增加本地缓存图片, name = "..key)
                PlatformPicManagerProxy:GetInstance():addSprite(cacheKey, sprite)
                if picLocateImg.loadingImg ~= nil then
                    picLocateImg.loadingImg.gameObject:SetActive(false)
                end
            else
                if picLocateImg.loadingImg ~= nil then
                    picLocateImg.loadingImg.gameObject:SetActive(false)
                end
                picLocateImg:SetPng(nil)
            end
        end)
    else
        if picLocateImg.loadingImg ~= nil then
            picLocateImg.loadingImg.gameObject:SetActive(false)
        end
        showImageBySprite(picLocateImg.Img, mySprite)
    end
    if not picLocateImg.Img.enabled then
        picLocateImg.Img.enabled = true
    end
end

--- 下载游戏图标/游戏规则图
---@param name string
---@param picLocateImg CS.ImageWidget
downloadGameIcon = function (name, picLocateImg)
    local picName = string.format("%s/%s", ImageType.GameIcon, name)
    downloadImageOrDefault(picName, picLocateImg, ImageType.GameIcon)
end