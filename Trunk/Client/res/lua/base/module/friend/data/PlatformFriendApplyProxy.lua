require "base:common/manager/SQLiteManager"

PlatformFriendApplyProxy = {}
local this = PlatformFriendApplyProxy

local myId = nil


--下面逻辑暂时无用

------------------------------------------------SQL本地存储开始----------------------
--存入表
--把聊天信息存到SQLite db文件,这里的data传一个table
function this:insertMsgToDB(playerId, data)
    myId = LoginDataProxy.playerId
    if SqliteManager.isHaveTable("apply_" .. myId, data) then
        SqliteManager.InsertOrUpdateValues("apply_" .. myId, data)
    end
end

--db创建聊天table,table以聊天对象的id起名字
function this:createMsgTableToDB(playerId)
    local tableName = "apply_" .. myId
    local isHaveSqlTabel = SqliteManager.isHaveTable(tableName)
    if not isHaveSqlTabel then
        SqliteManager.CreateTable(
            tableName,
            {"data"},
            {
                SqlDataType.TEXT .. SqlDataType.KEY
            }
        )
    end
end
--获取表
--根据用户id从SQLite db文件获取所有聊天并保存到本地内存
function this:getHistoryMsgFromDB(playerId)
    myId = LoginDataProxy.playerId
    local tableName = "apply_" .. myId
    local getSqlTableArray = SqliteManager.ReadFullTableData(tableName)

    if not table.empty(getSqlTableArray) then
        for k, v in pairs(getSqlTableArray) do
            printDebug("获取数据" .. table.tostring(getSqlTableArray))
        end
    end
end

local insertValue = nil

--删除SQLite db文件对应好友id的聊天信息
function this.delFriendTableById(playerId)
    myId = LoginDataProxy.playerId
    local tableName = "apply_" .. myId
    SqliteManager.DeleteFullTable(tableName)
end

------------------------------------------------SQL本地存储结束----------------------
