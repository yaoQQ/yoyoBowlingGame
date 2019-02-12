local pb = require "pb"

ProtobufManager = {}
local this = ProtobufManager

--编码
function this.encode(spaceName, msgName, msg)
	local protoName = this.getSpacePath(spaceName, msgName)

	return assert(pb.encode(protoName, msg))
end

--解码
function this.decode(spaceName, msgName, pbBytes)
	local protoName = this.getSpacePath(spaceName, msgName)
	--printDebug("在解码的协议   路径  "..protoName)
	--printDebug("在解码的二进制    "..#pbBytes)
	return assert(pb.decode(protoName, pbBytes))
end

function this.getSpacePath(spaceName, msgName)
	return "protocol."..spaceName.."."..msgName
end


this.pbCount = 0
this.loadedPbCount = 0
this.initPbCallback = nil

function this:initPackPb(packName, registList, callback)
	this.pbCount = #registList
	this.loadedPbCount = 0
	this.initPbCallback = callback
	for i=1,#registList do
		local url ="pb/"..packName.."/"..registList[i]..".pb"
		this:loadPbFile(packName, url)
	end
end

function this:loadPbFile(packageName, pathValue)
	LoadManager.Instance:AddOrderPB(packageName, pathValue, this.onLoadPBend);
end

function this.onLoadPBend(pbUrl, pbBytes)
	pb.load(pbBytes)
	this.loadedPbCount = this.loadedPbCount + 1
	if this.loadedPbCount >= this.pbCount then
		this.pbCount = 0
		this.loadedPbCount = 0
		if this.initPbCallback ~= nil then
			this.initPbCallback()
			this.initPbCallback = nil
		end
	end
end


function this.send(serverName, spaceName, msgName, msg)
	--序列化
	local msg_encode = this.encode(spaceName, msgName, msg)
	local protoID = this.getProtoEnumValue(spaceName, msgName)
	NetworkManager.Instance:SendMessage(serverName, protoID, msg_encode)
end

function this.getProtoEnumValue(spaceName, msgName)
	return pb.enum("protocol."..spaceName..".MsgIdx", "MsgIdx"..msgName)
end


function this.sendHttpUrl(url, param, rspCallBack)
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		printDebug("发送Http协议："..url.."?"..param)
	end
	HttpPostManager.Instance:SendHttp(url, param, function (jsonStr)
		if rspCallBack ~= nil then
			if jsonStr == nil then
				local rsp = {}
				rsp.result = -1	--网络连接失败
				rspCallBack(rsp)
			else
				local rsp = JsonUtil.decode(jsonStr)
				if rsp ~= nil then
					if IS_UNITY_EDITOR or IS_TEST_SERVER then
						printDebug("发送Http协议："..table.tostring(rsp))
					end
				end
				rspCallBack(rsp)
			end
		end
	end)
end

function this.sendHttpRecharge(msgName, param, rspCallBack)
	local url
	if GameConfig.loginIP == GameConfig.loginIP3 and GameConfig.loginPort == GameConfig.loginPort3 then
		url = "http://"..GameConfig.billingIP..":8900/api/recharge/"..msgName
	else
		url = "http://"..GameConfig.loginIP..":8900/api/recharge/"..msgName
	end
	this.sendHttpUrl(url, param, rspCallBack)
end

function this.sendHttpProtobuf(msgName, msg, rspCallBack)
	--序列化
	local msg_encode = assert(pb.encode(msgName, msg))
	local url = "http://192.168.1.168:8081/zjyouyou/protobuf/"..msgName..".do"
	HttpPostManager.Instance:SendHttpProtobuf(url, msg_encode, function (protoBytes)
		if rspCallBack ~= nil then
			if protoBytes == nil then
				local rsp = {}
				rsp.result = -1	--网络连接失败
				rspCallBack(rsp)
			else
				local reqMsgName = string.gsub(msgName, "Req", "Rsp")
				local rsp = pb.decode(reqMsgName, protoBytes)
				rspCallBack(rsp)
			end
		end
	end)
end