require "base:common/manager/SQLiteManager"

PlatformFriendProxy = {}
local this = PlatformFriendProxy

local myId

function PlatformFriendProxy:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function PlatformFriendProxy:GetInstance()
    if self._instance == nil then
        self._instance = self:new()
        --初始化一下
        self:init()
    end

    return self._instance
end

function this:init()
end

function this:initData()
    this.friendListData = {}
    this.locateFriendData = {}
    this.receiveAddFriendApplyData = {}
    this.successAddFriendData = nil
    this.searchFriendPhoneData = nil
    this.friendChatOfflineData = {}
    this.currOfflinePageIndex = -1
    this.friendChatListData = {}
    this.friendChatData = {}
    this.sendAddFriendApplyData = {}
    this.friendMainPageData = nil
    this.recommendFriendData = {}
    this.searchBaseUserInfo = nil
end

this.friendListData = {}
this.locateFriendData = {}
this.receiveAddFriendApplyData = {}
this.successAddFriendData = nil
this.searchFriendPhoneData = nil
this.friendChatOfflineData = {}
this.currOfflinePageIndex = -1
this.friendChatListData = {}
this.friendChatData = {}
this.sendAddFriendApplyData = {}
this.friendMainPageData = nil
this.recommendFriendData = {}
this.searchBaseUserInfo = nil
--设置好友列表
function this:setFriendListData(data)
    if data then
        this.friendListData = data
    end
end

--增加好友
function this:addFriendListData(data)
    for i = 1, #this.friendListData do
        if tostring(this.friendListData[i].player_base_info.player_id) ~= tostring(data.player_base_info.player_id) then
            table.insert(this.friendListData, data)
            break
        end
    end
end

--获取好友列表
function this:getFriendListData()
    return this.friendListData
end

--通过id获取好友信息
function this:getFriendDataById(playerId)
    if this.friendListData then
        for i = 1, #this.friendListData do
            if tostring(this.friendListData[i].player_base_info.player_id) == tostring(playerId) then
                return this.friendListData[i]
            end
        end
    end
end

--通过模糊搜索（昵称）来定位好友
function this:locateFriend(nickName)
    if this.friendListData == nil or #this.friendListData == 0 then
        return
    end
    if nickName == "" then
        this.locateFriendData = {}
        return
    end

    this.locateFriendData = {}
    for i = 1, #this.friendListData do
        if string.find(this.friendListData[i].player_base_info.nick_name, nickName) ~= nil then
            table.insert(this.locateFriendData, this.friendListData[i])
        end
    end
end

--获取模糊搜索结果
function this:getLocateFriendData()
    return this.locateFriendData
end

--设置添加好友数据
function this:setReceiveAddFriendApplyData(data)
    this.receiveAddFriendApplyData = data
end

--插入添加好友申请数据
function this:insertReceiveAddFriendApplyData(data)
    if data == nil or data.player_base_info == nil then
        return
    end
    if this:isMyFriendById(data.player_base_info.player_id) then
        local msg = {
            op = ProtoEnumFriendModule.FriendOp.FriendOpAgreeAddFriend,
            player_id = data.player_base_info.player_id
        }
        PlatformFriendModule.onReqFriendOp(msg)
    elseif this:isNotInMyApplyList(data.player_base_info.player_id) then
        table.insert(this.receiveAddFriendApplyData, data)
        NoticeManager.Instance:Dispatch(PlatformFriendType.Notify_Update_Red_Point)
    end
end

function this:isNotInMyApplyList(playerId)
    for i = 1, #this.receiveAddFriendApplyData do
        if tonumber(this.receiveAddFriendApplyData[i].player_id) == tonumber(playerId) then
            return false
        end
    end
    return true
end

--获取添加好友申请数据
function this:getReceiveAddFriendApplyData()
    if this.receiveAddFriendApplyData ~= nil and #this.receiveAddFriendApplyData > 0 then
        local notFriendCount = 0

        for i = 1, #this.receiveAddFriendApplyData do
            this.receiveAddFriendApplyData[i].isFriend =
                this:isMyFriendById(this.receiveAddFriendApplyData[i].player_base_info.player_id)

            if not this.receiveAddFriendApplyData[i].isFriend then
                notFriendCount = notFriendCount + 1
            end
        end

        this.receiveAddFriendApplyData.notFriendCount = notFriendCount
    end

    -- NoticeManager.Instance:Dispatch(PlatformFriendType.Notify_Update_Red_Point)

    return this.receiveAddFriendApplyData
end

--移除好友申请数据byid
function this:removeReiceiveFriendApplyData(playerId)
    for i = 1, #this.receiveAddFriendApplyData do
        if tonumber(this.receiveAddFriendApplyData[i].player_base_info.player_id) == tonumber(playerId) then
            table.remove(this.receiveAddFriendApplyData, i)
            this.receiveAddFriendApplyData.notFriendCount = this.receiveAddFriendApplyData.notFriendCount - 1
            NoticeManager.Instance:Dispatch(PlatformFriendType.Notify_Update_Red_Point)
            return
        end
    end
end

--设置添加好友成功数据
function this:setSuccessAddFriendData(data)
    this.successAddFriendData = data
end

--获取添加好友成功数据
function this:getSuccessAddFriendData()
    return this.successAddFriendData
end

--设置搜索好友手机号返回数据
function this:setSearchFriendPhoneData(data)
    this.searchFriendPhoneData = data
end

--获取搜索好友手机号返回数据
function this:getSearchFriendPhoneData()
    return this.searchFriendPhoneData
end

--离线消息
function this.addFriendChatListData(data)
    local myTargetList = {}
    if #this.friendChatListData == 0 then
        local friendChatListItem = {}
        table.insert(friendChatListItem, data)
        local target = {}
        target.my_chat_info = friendChatListItem
        target.unVisitedCount = 1 --这个是以用户为单位的信息是否已读
        target.playerId = data.player_id
        target.msg_id = data.msg_id
        table.insert(myTargetList, target)
    else
        local isIdMatched = false
        for j = 1, #this.friendChatListData do
            if this.friendChatListData[j].playerId == data.player_id then
                table.insert(this.friendChatListData[j].my_chat_info, data)
                this.friendChatListData[j].unVisitedCount = this.friendChatListData[j].unVisitedCount + 1
                this.friendChatListData[j].msg_id = data.msg_id
                isIdMatched = true
                table.insert(myTargetList, this.friendChatListData[j])
                break
            end
        end

        if not isIdMatched then
            local friendChatListItem = {}
            table.insert(friendChatListItem, data)
            local target = {}
            target.my_chat_info = friendChatListItem
            target.unVisitedCount = 1
            target.playerId = data.player_id
            target.msg_id = data.msg_id
            table.insert(myTargetList, target)
        end
    end
    this:createMsgTableToDB(myTargetList[1].playerId, myTargetList[1].msg_id)
    this:insertMsgToDB(
        myTargetList[1].playerId,
        myTargetList[1].msg_id,
        {
            tostring(data.player_id),
            tostring(data.msg_id),
            tostring(data.chat_msg_type),
            tostring(data.msg),
            tostring(data.time),
            tostring(data.head_url)
        }
    )
    for i = 1, #this.friendChatListData do
        if myTargetList[1].playerId ~= this.friendChatListData[i].playerId then
            table.insert(myTargetList, this.friendChatListData[i])
        end
    end

    this.friendChatListData = myTargetList
end

--设置好友离线聊天数据
function this:addFriendChatOfflineData(data)
    if this.currOfflinePageIndex == data.page_index or table.empty(data.chat_info_list) then
        return
    end
    this.currOfflinePageIndex = data.page_index

    for i = 1, #data.chat_info_list do
        if data.chat_info_list[i] then
            table.insert(this.friendChatOfflineData, data.chat_info_list[i])
            this.addFriendChatListData(data.chat_info_list[i])
        end
    end
end

--获取好友离线聊天数据
function this:getFriendChatOfflineData()
    return this.friendChatOfflineData
end

--添加好友聊天通知数据
function this:addFriendChatData(data, isNotDB)
    data.isMsgVisited = isNotDB --这个是以条为单位的信息是否已读

    table.insert(this.friendChatData, data)

    local myTargetList = {}
    --这里是对服务器给的没有按用户做分类的聊天数据进行按用户分类之后，存入friendChatListData
    if #this.friendChatListData == 0 then
        local friendChatListItem = {}
        table.insert(friendChatListItem, data)
        local target = {}
        target.my_chat_info = friendChatListItem
        target.unVisitedCount = isNotDB and 0 or 1
        target.msg_id = data.msg_id
        if tonumber(data.player_id) == tonumber(LoginDataProxy.playerId) then
            target.playerId = FriendChatDataProxy.currChatFriendId
        else
            target.playerId = data.player_id
        end

        table.insert(myTargetList, target)
    else
        local isIdMatched = false
        local targetId

        if tonumber(data.player_id) == tonumber(LoginDataProxy.playerId) then
            targetId = FriendChatDataProxy.currChatFriendId
        else
            targetId = data.player_id
        end

        for j = 1, #this.friendChatListData do
            if tonumber(this.friendChatListData[j].playerId) == tonumber(targetId) then
                table.insert(this.friendChatListData[j].my_chat_info, data)
                this.friendChatListData[j].unVisitedCount =
                    isNotDB and 0 or this.friendChatListData[j].unVisitedCount + 1
                isIdMatched = true
                this.friendChatListData[j].msg_id = data.msg_id
                table.insert(myTargetList, this.friendChatListData[j])

                break
            end
        end

        if not isIdMatched then
            local friendChatListItem = {}
            table.insert(friendChatListItem, data)
            local target = {}
            target.my_chat_info = friendChatListItem
            target.unVisitedCount = isNotDB and 0 or 1
            target.playerId = targetId
            target.msg_id = data.msg_id
            table.insert(myTargetList, target)
        end
    end
    if not isNotDB then
        this:createMsgTableToDB(myTargetList[1].playerId, myTargetList[1].msg_id)
        this:insertMsgToDB(
            myTargetList[1].playerId,
            myTargetList[1].msg_id,
            {
                tostring(data.player_id),
                tostring(data.msg_id),
                tostring(data.chat_msg_type),
                tostring(data.msg),
                tostring(data.time),
                tostring(data.head_url)
            }
        )
    end

    for i = 1, #this.friendChatListData do
        if tonumber(myTargetList[1].playerId) ~= tonumber(this.friendChatListData[i].playerId) then
            table.insert(myTargetList, this.friendChatListData[i])
        end
    end

    this.friendChatListData = myTargetList

    -- printDebug("~~~~~~~~~~~~~~~收到的在线通知并且寄存在本地为：" .. table.tostring(this.friendChatListData))
end

--获取好友聊天数据
function this:getFriendChatData()
    return this.friendChatData
end

--this.friendChatListData = {}

--获取好友聊天列表数据
function this:getFriendChatListData()
    return this.friendChatListData
end

this.currChatFriendData = nil
--根据好友id设置当前聊天的好友数据
function this:setCurrChatFriendData(playerId)
    this.currChatFriendData = nil

    for i = 1, #this.friendChatListData do
        if tonumber(this.friendChatListData[i].playerId) == tonumber(playerId) then
            this.currChatFriendData = this.friendChatListData[i]
            break
        end
    end
    this:SortFriendChatDataByTime()
end

--根据消息发送时间排序
function this:SortFriendChatDataByTime()
    if this.currChatFriendData == nil then
        return
    end
    table.sort(
        this.currChatFriendData.my_chat_info,
        function(a, b)
            if a.time ~= b.time then
                return tonumber(a.time) < tonumber(b.time)
            end
            return false
        end
    )
end

--根据好友id设置好友当前聊天记录为已读
function this:setCurrChatFriendDataReaded(playerId)
    for i = 1, #this.friendChatListData do
        if tonumber(this.friendChatListData[i].playerId) == tonumber(playerId) then
            this.friendChatListData[i].unVisitedCount = 0

            local msg = this.friendChatListData[i].my_chat_info
            for j = 1, #msg do
                msg[j].isMsgVisited = true
            end
            break
        end
    end
end

--根据好友id删除好友当前聊天记录
function this:delCurrChatFriendData(playerId)
    for i = 1, #this.friendChatListData do
        if tonumber(this.friendChatListData[i].playerId) == tonumber(playerId) then
            table.remove(this.friendChatListData, i)
            break
        end
    end
    for i = 1, #this.friendListData do
        if tonumber(this.friendListData[i].player_base_info.player_id) == tonumber(playerId) then
            table.remove(this.friendListData, i)
            break
        end
    end
    this:delMsgTable(playerId)
    this:delSendAddFriendApplyData(playerId)
end

--获取当前聊天的好友数据
function this:getCurrChatFriendData(playerId)
    this.currChatFriendData = nil
    if playerId then
        for i = 1, #this.friendChatListData do
            if tonumber(this.friendChatListData[i].playerId) == tonumber(playerId) then
                this.currChatFriendData = this.friendChatListData[i]
                break
            end
        end
    end
    return this.currChatFriendData
end

------------------------------------------------SQL本地存储开始----------------------

--根据用户id从SQLite db文件获取所有聊天并保存到本地内存
function this:getHistoryMsgFromDB(playerId)
    myId = LoginDataProxy.playerId
    --用于取DB时的聊天对象id
    FriendChatDataProxy.currChatFriendId = playerId

    local tableName = "chat" .. playerId .. "_" .. myId
    local getSqlTableArray = SqliteManager.ReadFullTableData(tableName)

    if not table.empty(getSqlTableArray) then
        for k, v in pairs(getSqlTableArray) do
            this:addFriendChatData(v, true)
        end
    end
    FriendChatDataProxy.currChatFriendId = 0
end

--把聊天信息存到SQLite db文件,这里的data传一个table
local insertValue = nil
function this:insertMsgToDB(playerId, msgId, data)
    myId = LoginDataProxy.playerId
    local tableName = "chat" .. playerId .. "_" .. myId
    if SqliteManager.isHaveTable(tableName) then
        SqliteManager.InsertOrUpdateValues(tableName, data)
    end
end

--db创建聊天table,table以聊天对象的id起名字
function this:createMsgTableToDB(playerId, msgId)
    myId = LoginDataProxy.playerId
    local tableName = "chat" .. playerId .. "_" .. myId
    local isHaveSqlTabel = SqliteManager.isHaveTable(tableName)
    if not isHaveSqlTabel then
        SqliteManager.CreateTable(
            tableName,
            {"player_id", "msg_id", "chat_msg_type", "msg", "time", "head_url"},
            {
                SqlDataType.TEXT,
                SqlDataType.TEXT .. SqlDataType.KEY,
                SqlDataType.TEXT,
                SqlDataType.TEXT,
                SqlDataType.TEXT,
                SqlDataType.TEXT
            }
        )
    end
end

--删除好友时聊天记录
function this:delMsgTable(playerId)
    myId = LoginDataProxy.playerId
    local tableName = "chat" .. playerId .. "_" .. myId
    local isHaveSqlTabel = SqliteManager.isHaveTable(tableName)
    if isHaveSqlTabel then
        SqliteManager.DeleteFullTable(tableName)
    end
end

------------------------------------------------SQL本地存储结束----------------------

--设置好友主页界面数据
function this:setFriendMainPageData(data)
    this.friendMainPageData = data
end

--获取好友主页界面数据
function this:getFriendMainPageData()
    return this.friendMainPageData
end

local tempFriendListData = nil
--根据用户id判断该用户是否在我的好友列表当中
function this:isMyFriendById(playerId)
    if this.friendListData == nil then
        return false
    end

    for i = 1, #this.friendListData do
        if not table.empty(this.friendListData[i].player_base_info) then
            if tonumber(this.friendListData[i].player_base_info.player_id) == tonumber(playerId) then
                return true
            end
        end
    end
    return false
end

--设置推荐好友数据
function this:setRecommendFriendData(data)
    this.recommendFriendData = data.player_list
end

--获取推荐好友数据
function this:getRecommendFriendData()
    return this.recommendFriendData
end

--设置发送好友申请数据
function this:setSendAddFriendApplyData(data)
    if not data or this:isMyAddFriendById(data.playerId) then
        return
    end
    table.insert(this.sendAddFriendApplyData, data)
end
--移除发送好友申请数据
function this:delSendAddFriendApplyData(playerId)
    if this.sendAddFriendApplyData == nil then
        return false
    end
    for i = 1, #this.sendAddFriendApplyData do
        if tonumber(this.sendAddFriendApplyData[i].player_id) == tonumber(playerId) then
            return table.remove(this.sendAddFriendApplyData, i)
        end
    end
end

--获取发送好友申请数据
function this:getSendAddFriendApplyData()
    return this.sendAddFriendApplyData
end
--根据用户id判断该用户是否在已申请过好友列表当中
function this:isMyAddFriendById(playerId)
    if this.sendAddFriendApplyData == nil then
        return false
    end
    for i = 1, #this.sendAddFriendApplyData do
        if tonumber(this.sendAddFriendApplyData[i].player_id) == tonumber(playerId) then
            return true
        end
    end
    return false
end

--通过用户id获取好友推荐信息
function this:getRecommendDataById(playerId)
    for i = 1, #this.recommendFriendData do
        if tonumber(this.recommendFriendData[i].player_id) == tonumber(playerId) then
            return this.recommendFriendData[i]
        end
    end
end

--设置查询用户的基本信息
function this:setBaseUserInfo(data)
    this.searchBaseUserInfo = data
end

--获取查询用户的基本信息
function this:getBaseUserInfo()
    return this.searchBaseUserInfo
end
