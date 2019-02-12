PlatformMessageProxy = {}
local this = PlatformMessageProxy

local m_mainData = {}
local m_normalMailListData = {}
local m_normalMailTitleListData = {}
local m_userMsgData = {}
local m_matchListData = {}
local m_redPacketListData = {}
local m_eventListData = {}
local m_userListData = {}
local pageIndex = 0

function this.initData()
    m_mainData = {}
    m_normalMailListData = {}
    m_normalMailTitleListData = {}
    m_userMsgData = {}
    m_matchListData = {}
    m_redPacketListData = {}
    m_eventListData = {}
    m_userListData = {}
    pageIndex = 0
end

--设置邮件数据
function this.setNormalMailData(index, data)
    m_normalMailListData = data
    if index == 0 then
        m_normalMailListData = {}
    end
    if data then
        for i = 1, #data do
            if data[i].award_flag == nil then
                data[i].award_flag = false
            end
            table.insert(m_normalMailListData, data[i])
        end
        this:SortNormalMailDataByTime()
    end
    PlatformGlobalView:onUpdateChatOnlineMsgCount()
    pageIndex = index
end
--插入新邮件
function this.insertMailData(data)
    if data then
        for _, v in pairs(m_normalMailListData) do
            if tonumber(data.id) == tonumber(v.id) then
                return
            end
        end
        if data.award_flag == nil then
            data.award_flag = false
        end
        table.insert(m_normalMailListData, data)
    end
    this:SortNormalMailDataByTime()
    PlatformGlobalView:onUpdateChatOnlineMsgCount()
end
--删除邮件
function this.delMailData(mail_id)
    if mail_id then
        for index, v in pairs(m_normalMailListData) do
            if tonumber(mail_id) == tonumber(v.id) then
                table.remove(m_normalMailListData, index)
                break
            end
        end
    end
    this:SortNormalMailDataByTime()
    PlatformGlobalView:onUpdateChatOnlineMsgCount()
end

--获取邮件数据
function this.getNormalMailData()
    this:SortNormalMailDataByTime()
    return m_normalMailListData
end

--获取邮件数量排除未领取邮件
function this.getMailCount()
    local count = #m_normalMailListData
    for k, v in pairs(m_normalMailListData) do
        if v.award_flag then
            count = count - 1
        end
    end
    count = count < 0 and 0 or count
    return count
end

function this.getNormalTitleMailData()
    if #m_normalMailListData == 0 then
        m_normalMailTitleListData = {}
    else
        m_normalMailTitleListData = m_normalMailListData[1]
    end
    return m_normalMailTitleListData
end

--设置用户消息
function this.setUserMsgData(data)
    if data == nil or data == "" then
        data = {}
    end
    m_matchListData = {}
    m_eventListData = {}
    m_redPacketListData = {}
    m_userMsgData = data
    for i = 1, #data do
        if data[i].msg_type == ProtoEnumCommon.UserMsgType.UserMsgType_Match then
            -- 活动消息
            table.insert(m_matchListData, data[i])
        elseif data[i].msg_type == ProtoEnumCommon.UserMsgType.UserMsgType_Event then
            -- 系统公告
            table.insert(m_eventListData, data[i])
        elseif data[i].msg_type == ProtoEnumCommon.UserMsgType.UserMsgType_RedPacket then
            -- 红包消息
            table.insert(m_redPacketListData, data[i])
        else
            printError("未知消息类型")
        end
    end
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Confirm_UserMsg)
end

--获取用户消息
function this.getUserMsgData()
    --this:SortNormalMailDataByTime()
    return m_userMsgData
end

--获取用户活动消息
function this.getMatchListData()
    this:SortUserListDataByTime(m_matchListData)
    return m_matchListData
end

--获取用户活动消息数量
function this.getMatchListNum()
    local count = #m_matchListData
    for k, v in pairs(m_matchListData) do
        if v.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
            count = count - 1
        end
    end
    count = count < 0 and 0 or count
    return count
end

--获取用户事件消息
function this.getEventListData()
    this:SortUserListDataByTime(m_eventListData)
    return m_eventListData
end

--获取用户事件消息数量
function this.getEventListNum()
    local count = #m_eventListData
    for k, v in pairs(m_eventListData) do
        if v.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
            count = count - 1
        end
    end
    count = count < 0 and 0 or count
    return count
end

--获取用户红包消息
function this.getRedPacketListData()
    this:SortUserListDataByTime(m_redPacketListData)
    return m_redPacketListData
end

--获取用户红包信息数量
function this.getRedPacketListNum()
    local count = #m_redPacketListData
    for k, v in pairs(m_redPacketListData) do
        if v.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
            count = count - 1
        end
    end
    count = count < 0 and 0 or count
    return count
end

--好友消息数量
function this.getcurrFriendChatNum()
    local frinedListData = PlatformFriendProxy:GetInstance():getFriendChatListData()
    local friendcount = 0
    if not table.empty(frinedListData) then
        for i = 1, #frinedListData do
            local curChatPlayerData = PlatformFriendProxy:GetInstance():getFriendDataById(frinedListData[i].playerId)
            if curChatPlayerData ~= nil and curChatPlayerData ~= "" then
                friendcount = friendcount + frinedListData[i].unVisitedCount
            end
        end
    end
    return friendcount
end

function this.getRedPointCount()
    local count = 0
    local mailcount = this.getMailCount()
    local redPacketcount = 0 --this.getRedPacketListNum()
    local userMatchcount = this.getMatchListNum()
    local userEventcount = this.getEventListNum()
    local friendMsgCount = this.getcurrFriendChatNum()
    count = mailcount + redPacketcount + userMatchcount + userEventcount + friendMsgCount
    return count
end

--设置详情界面奖励数据
function this.setMessageMainData(data)
    m_mainData = data
end

--获取详情界面奖励数据
function this.getMessageMainData()
    return m_mainData
end

--领取相应奖励
function this.clearAllMail()
    m_normalMailTitleListData = {}
end

--根据id领取相应奖励
function this.delMailinfoByID(id)
    for i = 1, #m_normalMailListData do
        if tonumber(m_normalMailListData[i].id) == tonumber(id) then
            table.remove(m_normalMailListData, i)
            break
        end
    end
    PlatformGlobalView:onUpdateChatOnlineMsgCount()
end

function this:SortNormalMailDataByTime()
    if m_normalMailListData == nil then
        return
    end
    table.sort(
        m_normalMailListData,
        function(a, b)
            if a.award_flag ~= b.award_flag then
                return not a.award_flag
            end
            if a.create_time ~= b.create_time then
                return tonumber(a.create_time) > tonumber(b.create_time)
            end
            return false
        end
    )
end

function this:SortUserListDataByTime(list)
    if list == nil then
        return
    end

    table.sort(
        list,
        function(a, b)
            if a.time ~= b.time then
                return tonumber(a.time) > tonumber(b.time)
            end
            return false
        end
    )
end
