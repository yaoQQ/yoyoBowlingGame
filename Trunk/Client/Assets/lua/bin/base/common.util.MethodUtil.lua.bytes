table.find = function(this, value)
    for k, v in pairs(this) do
        if v == value then
            return k
        end
    end
end
--字典转换为table
table.DicToTable = function(CSharpDic)
	local dic = {}
	--local index =1
	local iter = CSharpDic:GetEnumerator()	
	while iter:MoveNext() do
		local v = iter.Current.Value
		dic[iter.Current.Key] = v
		--index = index + 1
	end
	return dic
end

--克隆table
table.clone = function(t)
    if type(t) ~= "table" then
        return t
    end
    local mt = getmetatable(t)
    local res = {}
    for k, v in pairs(t) do
        if type(v) == "table" then
            v = table.clone(v)
        end
        res[k] = v
    end
    setmetatable(res, mt)
    return res
end
--求count
table.nums = function(t)
    if t == nil then
        return 0
    end
    local count = 0
    for k, v in pairs(t) do
        if v ~= nil then
            count = count + 1
        end
    end
    return count
end

--小数点后0的去除
table.clearZero = function(num)
    if num <= 0 then
        return 0
    else
        local t1, t2 = math.modf(num)
        ---小数如果为0，则去掉
        if t2 > 0 then
            return num
        else
            return t1
        end
    end
end

moveDebug1 = false
moveDebug2 = false

-- 用于辅助调试的打印, 尤其是消息打印
table.tostring = function(data, _indent)
    if data == nil then
        printDebug("Table为空")
        return
    end
    local visited = {}
    local function dump(data, prefix)
        local str = tostring(data)
        if table.find(visited, data) ~= nil then
            return str
        end
        table.insert(visited, data)

        local prefix_next = prefix .. "  "
        str = str .. "\n" .. prefix .. "{"
        for k, v in pairs(data) do
            if type(k) == "number" then
                str = str .. "\n" .. prefix_next .. "[" .. tostring(k) .. "] = "
            else
                str = str .. "\n" .. prefix_next .. tostring(k) .. " = "
            end
            if type(v) == "table" then
                str = str .. dump(v, prefix_next)
            elseif type(v) == "string" then
                str = str .. '"' .. v .. '"'
            else
                str = str .. tostring(v)
            end
        end
        str = str .. "\n" .. prefix .. "}"
        return str
    end
    return dump(data, _indent or "")
end

string.split =
    function(s, p)
    local rt = {}
    string.gsub(
        s,
        "[^" .. p .. "]+",
        function(w)
            table.insert(rt, w)
        end
    )
    return rt
end

---字符串拼接 取代..性能更优
string.concat = function(...)
    return table.concat({...})
end

table.empty = function(tbl)
    if tbl == nil then
        return true
    end
    assert(type(tbl) == "table")
    if #tbl > 0 then
        return false
    end
    for k, v in pairs(tbl) do
        return false
    end
    return true
end

table.extend = function(base, delta)
    if type(delta) ~= "table" then
        return
    end
    for i, v in ipairs(delta) do
        table.insert(base, v)
    end
end

--以p概率产生1
math.single_prob = function(p)
    if math.random() < p then
        return 1
    else
        return 0
    end
end

printDebug = function(str)
	if moveDebug1 then
		return
	end
    if IS_UNITY_EDITOR or IS_TEST_SERVER then
        print(str)
    end
end

printWarning = function(str)
    Loger.PrintWarning(str)
end

printError = function(str)
    Loger.PrintError(debug.traceback(str))
end

--显示中文日期，格式：**年**月**日
showChineseDataTime = function(unixTime)
    local date = TimeManager.getDateTimeByUnixTime(unixTime)
    return date.Year .. "年" .. date.Month .. "月" .. date.Day .. "日"
end

--获取uuid
getUUID =
    function()
    local seed = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"}
    local tb = {}
    for i = 1, 32 do
        table.insert(tb, seed[math.random(1, 16)])
    end
    local sid = table.concat(tb)
    return string.format(
        "%s-%s-%s-%s-%s",
        string.sub(sid, 1, 8),
        string.sub(sid, 9, 12),
        string.sub(sid, 13, 16),
        string.sub(sid, 17, 20),
        string.sub(sid, 21, 32)
    )
end

--获取uuid
getShortUUID = function()
    local seed = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"}
    local tb = {}
    for i = 1, 14 do
        table.insert(tb, seed[math.random(1, 16)])
    end
    local sid = table.concat(tb)
    return string.format("%s-%s-%s", string.sub(sid, 1, 8), string.sub(sid, 9, 12), string.sub(sid, 13, 14))
end

showBottomAndCamera =
    function(picName, successFun)
    -- body
    ShowBottomSelectWindPhoto(
        function()
            PlatformSDK.takePhonePhoto(
                true,
                function(bytes)
                    ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
                    CommonUploadView:activeUpdateTip(true, "上传中")
                    uploadImage(
                        picName,
                        bytes,ImageType.UserHead,
                        function(isSucceed)
                            if isSucceed then
                                CommonUploadView:activeUpdateTip(false, "上传图片成功")
                                if successFun ~= nil then
                                    successFun()
                                end
                            else
                                CommonUploadView:activeUpdateTip(false, "上传图片失败")
                            end
                        end
                    )
                end,
                true,
                256,
                256
            )
        end,
        function()
            PlatformSDK.takePhonePhoto(
                false,
                function(bytes)
                    ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
                    CommonUploadView:activeUpdateTip(true, "上传中")
                    uploadImage(
                        picName,
                        bytes,ImageType.UserHead,
                        function(isSucceed)
                            if isSucceed then
                                CommonUploadView:activeUpdateTip(false, "上传图片成功")
                                if successFun ~= nil then
                                    successFun()
                                end
                            else
                                CommonUploadView:activeUpdateTip(false, "上传图片失败")
                            end
                        end
                    )
                end,
                true,
                256,
                256
            )
        end
    )
end

showBottomCameraChoose =
    function(picName, isCutCamera, isCutAlbum, callback)
    ShowBottomSelectWindPhoto(
        function()
            PlatformSDK.takePhonePhoto(
                true,
                function(bytes)
                    ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
                    CommonUploadView:activeUpdateTip(true, "上传中")
                    uploadImage(
                        picName,
                        bytes,ImageType.Photo,
                        function(isSucceed)
                            if isSucceed then
								CommonUploadView:activeUpdateTip(false, "上传图片成功")
                                if callback ~= nil then
                                    callback()
                                end
                            else
                                CommonUploadView:activeUpdateTip(false, "上传图片失败")
                            end
                        end
                    )
                end,
                isCutCamera,
                1024,
                1024
            )
        end,
        function()
            PlatformSDK.takePhonePhoto(
                false,
                function(bytes)
                    ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
                    CommonUploadView:activeUpdateTip(true, "上传中")
                    uploadImage(
                        picName,
                        bytes,ImageType.Photo,
                        function(isSucceed)
                            if isSucceed then
								CommonUploadView:activeUpdateTip(false, "上传图片成功")
                                if callback ~= nil then
                                    callback()
                                end
                            else
                                CommonUploadView:activeUpdateTip(false, "上传图片失败")
                            end
                        end
                    )
                end,
                isCutAlbum,
                1024,
                1024
            )
        end
    )
end

--从非图片服务器下载图片
downloadFromOtherServer =
    function(picName, picLocateImg, notUpdate)
    if picName == nil or picName == "" then
        return
    end
    local myTexture2D = PlatformPicManagerProxy:GetInstance():getGuessPic(picName)
    if myTexture2D == nil then
        -- printDebug("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^本地没有图片，图片名字为："..picName)
        PhotoManager.downloadNetPhoto(
            picName,
            function(texture2D)
                if texture2D == nil then
                    --printDebug("下载图片失败")
                    picLocateImg:SetPng(nil) --如果没有图片，则设置图片的默认图片
                else
                    --printDebug("++++++++++下载图片成功!")
                    ImageUtil.setTexture2DImage(texture2D, picLocateImg.Img)
                    PlatformPicManagerProxy:GetInstance():addGuessPic(picName, texture2D)
                end
                myTexture2D = texture2D
            end
        )
    else
        -- printDebug("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^本地有图片，图片名字为："..picName)
        if not notUpdate then
            ImageUtil.setTexture2DImage(myTexture2D, picLocateImg.Img)
        end
    end
    if not picLocateImg.Img.enabled then
        picLocateImg.Img.enabled = true
    end
    return myTexture2D
end

--检查密码合法性（8-16位必须包含字母和数字）
checkPassword = function(password)
    local len = #password
    local isContainLetter = false
    local isContainNumber = false
    local b
    for i = 1, len do
        b = string.byte(password, i)
        if b >= 48 and b <= 57 then
            isContainNumber = true
        elseif b >= 65 and b <= 90 then
            isContainLetter = true
        elseif b >= 97 and b <= 122 then
            isContainLetter = true
        else
            Alert.showAlertMsg("密码格式错误", "密码只能由字母和数字组成", "好的")
            -- AlertLoginWindowView:showLoginAlertMsg("密码格式错误","密码只能由字母和数字组成", "好的",nil)
            return false
        end
    end
    if isContainLetter and isContainNumber then
        if len < 8 or len > 16 then
            Alert.showAlertMsg("密码格式错误", "密码长度必须为8~16位", "好的")
            --AlertLoginWindowView:showLoginAlertMsg("密码格式错误","密码长度必须为8~16位", "好的",nil)
            return false
        else
            return true
        end
    else
        Alert.showAlertMsg("密码格式错误", "请输入正确的密码", "好的")
        --AlertLoginWindowView:showLoginAlertMsg("密码格式错误","密码必须包含字母和数字", "好的",nil)
        return false
    end
end

physicsRaycast = function(ray)
    local isHit, raycastHit = UtilMethod.PhysicsRaycast(ray)
    return isHit, raycastHit
end

--[[ShareType
{
    Text = 0,
    Image = 1,
    Music = 2,
    Video = 3,
    Webpage = 4
}--]]
--微信分享
--mode	分享模式 0：分享到微信好友 1：分享到微信朋友圈
--url	路径或链接
--title	分享标题
--description	分享描述
--bmpUrl	分享缩略图路径
--succeedCallback	成功回调
--cancelCallback	取消回调
--failCallback	失败回调

--微信分享文字
wxShareText = function(mode, description, succeedCallback, cancelCallback, failCallback)
    if CSPlatformSDK.IsWXAppInstalled() then
        printDebug("已安装微信客户端")
        CSPlatformSDK.WxShare(0, mode, "", "", description, "", succeedCallback, cancelCallback, failCallback)
    else
        printDebug("未安装微信客户端")
        showFloatTips("请先安装微信")
    end
end
--微信分享图片
wxShareImage = function(mode, url, succeedCallback, cancelCallback, failCallback)
    if CSPlatformSDK.IsWXAppInstalled() then
        printDebug("已安装微信客户端")
        CSPlatformSDK.WxShare(1, mode, url, "", "", "", succeedCallback, cancelCallback, failCallback)
    else
        printDebug("未安装微信客户端")
        showFloatTips("请先安装微信")
    end
end
--微信分享网页
wxShareWebpage = function(mode, url, title, description, bmpUrl, succeedCallback, cancelCallback, failCallback)
    if CSPlatformSDK.IsWXAppInstalled() then
        printDebug("已安装微信客户端")
        CSPlatformSDK.WxShare(4, mode, url, title, description, bmpUrl, succeedCallback, cancelCallback, failCallback)
    else
        printDebug("未安装微信客户端")
        showFloatTips("请先安装微信")
    end
end

--开平方根比较消耗性能，只用平方值比较距离，且不考虑空间状态
distanseNotSqrt = function(x1, y1, x2, y2)
    return (x1 - x2) ^ 2 + (y1 - y2) ^ 2
end
--[[
string.formatNum = function(number, bit)
	if number = nil then
		print("数字格式错误")
	else
		if number / 10^8 >1 then
			number = math.floor(number / 10^6)
			return(string.format("%."..bit.."f", number/10^bit).."亿")
		elseif number / 10^5 > 1 then
			number = math.floor(number / 10^2)
			return(string.format("%."..bit.."f", number/10^bit).."万")
		else
			return number
		end
    end
end
--]]
