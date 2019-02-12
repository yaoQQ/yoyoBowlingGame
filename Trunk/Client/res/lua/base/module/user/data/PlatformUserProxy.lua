


PlatformUserProxy={}
local this=PlatformUserProxy


function PlatformUserProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformUserProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

this.mainBaseData = nil     --用户基本数据（包括用户基本信息，用户图片展示以及用户游戏信息三类）

--通过用户id来获取用户数据
function this:getSingleDataById(userId)
    if this.allUserData == nil then return end

    for i=1,#this.allUserData do
        if tostring(this.allUserData[i].singleId) == userId then
            this.mainBaseData = this.allUserData[i]
            return this.allUserData[i]
        end
    end
end

--获取用户数据
function this:getUserMainBase()
    return this.mainBaseData
end

this.allUserData = nil
--设置所有用户的数据
function this:setAllUserData(data)
    this.allUserData = data
end

--获取所有用户数据
function this:getAllUserData()
    return this.allUserData
end

this.currMsg = nil
--设置用户对话内容
function this:setUserMsg(msg)
    this.currMsg = msg
end

--获取用户对话内容
function this:getUserMsg()
    return this.currMsg
end

------------------------------用户信息------------------------------

this.userInfoData = nil
this.isUpdateUserItem = true
--设置用户自身数据信息
function this:setUserInfo(data)
    this.userInfoData = data
	
	NoticeManager.Instance:Dispatch(NoticeType.User_Update_UserInfo)
	NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
	NoticeManager.Instance:Dispatch(NoticeType.User_Init_Diamond_Money)
	NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_MyPos)
end

--获取用户自身数据信息
function this:getUserInfo()
    return this.userInfoData
end

--更新部分key的value
function this:changeUserBaseInfo(data)
	printDebug("++++++++++++++++匹配更改前的data为："..table.tostring(this.userInfoData))
    for k,v in pairs(data) do
        for m,n in pairs(this.userInfoData) do
            if k == m then
                -- print("+++++++++++++++++++++++找到匹配的key:"..k)
                this.userInfoData[k] = v
                break
            end
        end
    end

    NoticeManager.Instance:Dispatch(NoticeType.User_Update_UserBaseInfo)
end

--更新Item
function this:updateUserItem(item_info)
	if item_info.item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
		this.userInfoData.cash = item_info.item_count
	elseif item_info.item_type == ProtoEnumCommon.ItemType.ItemType_Diamond then
		this.userInfoData.diamond = item_info.item_count
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Diamond)
	elseif item_info.item_type == ProtoEnumCommon.ItemType.ItemType_Money then
		this.userInfoData.money = item_info.item_count
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Money)
	end
end

function this:noUpdateUserItem()
	this.isUpdateUserItem = false
end

function this:onUpdateUserItem()
	this.isUpdateUserItem = true
end

--更新充值结果
function this:updateRechargeResult(cash, diamond, money)
	if this.userInfoData.cash ~= cash then
		this.userInfoData.cash = cash
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
	end
	if this.userInfoData.diamond ~= diamond then
		this.userInfoData.diamond = diamond
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Diamond)
	end
	if this.userInfoData.money == money then
		this.userInfoData.money = money
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Money)
	end
end

--更新支付宝昵称
function this:updateAlipayNick(alipay_nick_name)
	this.userInfoData.alipay_nick_name = alipay_nick_name
	NoticeManager.Instance:Dispatch(NoticeType.User_Update_BindAcount)
end

------------------------------用户信息end------------------------------

this.userPhotosData = nil
--设置用户的图片数据
function this:setUserPhotosData(data)
    this.userPhotosData = data
	
	NoticeManager.Instance:Dispatch(NoticeType.User_Update_AlbumPicList)
end

--获取用户的图片数据
function this:getUserPhotosData()
    return this.userPhotosData
end

------------------------------每日次数信息------------------------------

local m_dailyData = {}
function this:setDailyData(data)
	--地图红包剩余次数
	if data.lbs_red_packet_counter ~= nil and m_dailyData.lbs_red_packet_counter ~= data.lbs_red_packet_counter then
		m_dailyData.lbs_red_packet_counter = data.lbs_red_packet_counter
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_LBSRedPacketCounter)
	end
	--在线分享红包剩余次数
	if data.online_red_packet_share_counter ~= nil and m_dailyData.online_red_packet_share_counter ~= data.online_red_packet_share_counter then
		m_dailyData.online_red_packet_share_counter = data.online_red_packet_share_counter
		printDebug("++++++++在綫++++++红包次数+++++++++++++"..table.tostring(m_dailyData))
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_OnlineRedPacketShareCounter)
	end
	--补贴剩余次数
	if data.bankruptcy_subsidy_counter ~= nil and m_dailyData.bankruptcy_subsidy_counter ~= data.bankruptcy_subsidy_counter then
		m_dailyData.bankruptcy_subsidy_counter = data.bankruptcy_subsidy_counter
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_BankruptcySubsidyCounter)
	end
end

function this:getDailyData()
    return m_dailyData
end

function this:getDailyLBSRedPacketCount()
    return m_dailyData.lbs_red_packet_counter or 0
end

function this:getOnlineRedPacketDailyCount()
    return m_dailyData.online_red_packet_share_counter or 0
end

function this:getSubsidyCountDailyCount()
    return m_dailyData.bankruptcy_subsidy_counter or 0
end

------------------------------每日次数信息end------------------------------