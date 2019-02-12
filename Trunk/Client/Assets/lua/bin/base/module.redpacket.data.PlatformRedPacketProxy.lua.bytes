
PlatformRedPacketProxy={}

PacketGetState =
{
    FirstGot    = 1,
    HasGot      = 2,
    Empty       = 3,
}
local params={}

local function setOPenRedpacketData(key,value)
    params[key]=value
end

local function getOPenRedpacketData(key)
   return params[key]
end

local function updatePacketData(key, rsp, state, isFromChat)
    params[key].rsp = rsp
    params[key].getState = state
    params[key].isFromChat = isFromChat
end


PlatformRedPacketProxy.SetOpenLBSPacketData = setOPenRedpacketData
PlatformRedPacketProxy.GetOpenLBSPacketData = getOPenRedpacketData
PlatformRedPacketProxy.UpdatePacketData = updatePacketData