require "base:enum/PlatformNoticeType"
require "base:module/user/data/PlatformUserProxy"
require "base:module/lbs/data/PlatformLBSDataProxy"
require "base:module/redbag/data/PlatformRedBagProxy"
require "base:module/Shop/data/PlatformBusinessProxy"
require "base:enum/NoticeType"

PlatformModule = BaseModule:new()
local this = PlatformModule

this.moduleName = "Platform"

--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
    this:registerNetMsg(ProtoEnumShop.MsgIdx.MsgIdxRspGetShopInfo)

    --收到拆红包通知
    --this:registerNetMsg(ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvActiveCashRedPacket)

    --收到拆地图红包通知
    --this:registerNetMsg(ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvMapRedPacket)

    --收到排行榜数据返回
    this:registerNetMsg(ProtoEnumActive.MsgIdx.MsgIdxRspActiveRank)
end

function this.onNetMsgLister(protoID, protoBytes)
    if ProtoEnumShop.MsgIdx.MsgIdxRspGetShopInfo == protoID then
        --收到商家基本信息通知
        this.onReceiveShopBaseInfo(protoBytes)
    elseif ProtoEnumActive.MsgIdx.MsgIdxRspRcvActiveCashRedPacket == protoID then
        --收到活动拆红包通知
        this.onReceiveRcvRedPacket(protoBytes)
    elseif ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvMapRedPacket == protoID then
        --收到地图拆红包通知
        this.onReceiveMapRedPacket(protoBytes)
    elseif ProtoEnumActive.MsgIdx.MsgIdxRspActiveRank == protoID then
        --收到排行榜数据返回
        this.onReceiveActiveRank(protoBytes)
    end
end

--==================================================消息==================================================

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_Start_RedBag_Game)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Rep_Start_RedBag_Game)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_End_RedBag_Game)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Rep_End_RedBag_Game)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_Get_RedBag_Game_Award)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Rep_Get_RedBag_Game_Award)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Rep_Get_RedBag_Game_Award_List)
        table.insert(self.notificationList, PlatformNoticeType.Platform_RedBag2Main)
        table.insert(self.notificationList, PlatformNoticeType.Platform_Main2RedBag)

        table.insert(self.notificationList, PlatformNoticeType.Platform_MultiBusi)
        table.insert(self.notificationList, PlatformNoticeType.Platform_MultiUser)

        table.insert(self.notificationList, PlatformNoticeType.Platform_GetMsg)

        table.insert(self.notificationList, PlatformNoticeType.Require_Shop_Base_Info)

        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_Rcv_RedBag_Coupon) --請求領取卡券紅包

        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_Map_Red_Bag) --請求領取紅包

        table.insert(self.notificationList, PlatformNoticeType.Platform_Req_Active_Rank) --请求活动排行榜
        table.insert(self.notificationList, PlatformNoticeType.Platform_Rsp_Active_Rank) --请求活动排行榜返回
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    -- printDebug("notice为："..notice.singleId)
    local switch = {
        [PlatformNoticeType.Platform_Req_Start_RedBag_Game] = function()
            self:onRequestStartRedBagGame(notice)
        end,
        [PlatformNoticeType.Platform_Rep_Start_RedBag_Game] = function()
            self:onReceiveStartRedBagGame(notice)
        end,
        [PlatformNoticeType.Platform_Req_End_RedBag_Game] = function()
            self:onRequestEndRedBagGame(notice)
        end,
        -- [PlatformNoticeType.Platform_Rep_End_RedBag_Game] = function()
        --     self:onReiceEndGameAward(notice)
        -- end,
        [PlatformNoticeType.Platform_Req_Get_RedBag_Game_Award] = function()
            self:onRequestGetRedBagAward(notice)
        end,
        [PlatformNoticeType.Platform_Rep_Get_RedBag_Game_Award] = function()
            self:onReceiveGetRedBagAward(notice)
        end,
        -- [PlatformNoticeType.Platform_Rep_Get_RedBag_Game_Award_List] = function()
        --     self:onReceiveGetRedBagAwardList(notice)
        -- end,
        [PlatformNoticeType.Platform_RedBag2Main] = function()
            self:onRedBag2Main(notice)
        end,
        [PlatformNoticeType.Platform_Main2RedBag] = function()
            self:onMain2RedBag(notice)
        end,
        [PlatformNoticeType.Platform_MultiBusi] = function()
            self:onMultiBusi(notice)
        end,
        [PlatformNoticeType.Platform_MultiUser] = function()
            self:onMultiUser(notice)
        end,
        [PlatformNoticeType.Platform_GetMsg] = function()
            self:onMain2UserMsg(notice)
        end,
        [PlatformNoticeType.Require_Shop_Base_Info] = function()
            self:onReqShopBaseInfo(notice)
        end,
        [PlatformNoticeType.Platform_Req_Rcv_RedBag_Coupon] = function()
            self:onReqRcvRedBagCoupon(notice)
        end,
        [PlatformNoticeType.Platform_Req_Map_Red_Bag] = function()
            self:onReqMapRedBag(notice)
        end,
        [PlatformNoticeType.Platform_Req_Active_Rank] = function()
            self:onReqActiveRank(notice)
        end,
        [PlatformNoticeType.Platform_Rsp_Active_Rank] = function()
            self:onRspActiveRank(notice)
        end
    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        --用于报错提醒
        self:withoutRegistNotice(noticeType)
    end
end

---------------------------------------------收到消息(客户端或服务端发出)---------------------------

function this:onReceiveShortRedBagConponData(notice)
end

--收到服务器红包详情页信息
-- function this:onReceiveLongRedBagData(notice)
-- 	local rep = notice:GetObj()
-- 	local info = rep.red_package_detail_info

-- 	printDebug("红包的信息为："..table.tostring(rep))
-- 	local redBag = {}
-- 	local players = {}

-- 	redBag.singleId = info.id
-- 	redBag.prize = info.price
-- 	redBag.totalPeople = info.max_person_count
-- 	redBag.leftTime = info.left_time
-- 	redBag.gameId = info.game_id

-- 	local temp = info.red_package_user_info_list

-- 	for i=1,#temp do
-- 		local player = {}
-- 		player.rank = temp[i].rank
-- 		player.score = temp[i].score
-- 		player.name = temp[i].name
-- 		player.sex = temp[i].sex
-- 		player.sign = temp[i].sign
-- 		player.iconUrl = temp[i].head_url

-- 		table.insert(players,player)
-- 	end

-- 	redBag.playerList = players

-- 	PlatformRedBagProxy:GetInstance():setSingleData(redBag)
-- 	ViewManager.open(UIViewEnum.PlatForm_Red_Bag_View)
-- end

--收到请求开始红包游戏返回
function this:onReceiveStartRedBagGame(notice)
end

--获得红包奖励
function this:onReceiveGetRedBagAward(notice)
    local rep = notice:GetObj()
    local awardData = PlatformRedBagProxy:GetInstance():getRedBagAward()

    if awardData == nil and #awardData == 0 then
        return
    end
    local removeId = nil
    for _, v in pairs(awardData) do
        if v ~= nil then
            removeId = v.id
            break
        end
    end
    PlatformRedBagProxy:GetInstance():removeRedBagAward(removeId)
    if PlatformHelpView and PlatformHelpView.isOpen then
        PlatformHelpView:getRedBagAwardSuccess()
    end
end

--红包领取界面通知main界面
function this:onRedBag2Main(notice)
end

--main界面通知红包领取界面
function this:onMain2RedBag(notice)
    PlatformHelpView:onClickRedBag(notice)
end

--显示多商户界面
function this:onMultiBusi(notice)
    local rep = notice:GetObj()
    PlatformIconProxy:GetInstance():setMultiBusiData(rep)

    if PlatformMultiBusiView and PlatformMultiBusiView.isOpen then
        PlatformMultiBusiView:onLoadMultiBusi()
    else
        ViewManager.open(UIViewEnum.platForm_Multi_Busi_View)
    end
end

--显示多用户界面
function this:onMultiUser(notice)
    local rep = notice:GetObj()
    PlatformIconProxy:GetInstance():setMultiUserData(rep)
    ViewManager.open(UIViewEnum.platForm_Multi_User_View)
end

--用户收到消息需要显示
function this:onMain2UserMsg(notice)
    local rep = notice:GetObj()
    PlatformUserProxy:GetInstance():setUserMsg(rep)
end

--收到排行榜数据返回
function this:onRspActiveRank(notice)
    local rsp = notice:GetObj()
    PlatformLBSDataProxy.setActiveRankData(rsp.rank_info_list)
    if
        rsp.rank_info_list == nil and
            PlatformGlobalShopChatView.curOpenactiviteState == PlatformGlobalShopChatView.OpenActiviteState.GameExitRank
     then
        return
    end
    ViewManager.open(UIViewEnum.Platform_Active_Rank_View,{isofficial = false})
end
--------------------------------------------发送消息(客户端发出)-------------------------------

--请求获取商店活动
function this.sendReqGetShopActivity(id, page_index, per_page_num)
    local req = {}
    req.id = id
    req.page_index = page_index
    req.per_page_num = per_page_num
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "shop", "ReqGetShopActivity", req)
end

--请求开始红包游戏
function this:onRequestStartRedBagGame(notice)
    local reqStartRedBagGame = {}
    local redBagId = PlatformRedBagProxy:GetInstance():getSingleDataId()
    reqStartRedBagGame.id = redBagId
    printDebug("点击了游戏按钮，游戏id为：" .. redBagId)
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqStartRedPackageGame", reqStartRedBagGame)
end

--请求结算红包游戏
function this:onRequestEndRedBagGame(notice)
    local req = {}
    local obj = notice:GetObj()
    req.active_id = obj.gameId
    req.score = obj.score
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqUpdateActiveGameScore", req)
end

--请求领取红包奖励
function this:onRequestGetRedBagAward(notice)
    local rep = notice:GetObj()
    local reqGetRedBagAward = {}
    printDebug("红包id为：" .. rep)
    reqGetRedBagAward.id = rep
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetRedPackageAward", reqGetRedBagAward)
end

--请求红包奖励列表
-- function this:onRequestGetRedBagAwardList(notice)
-- 	local req = {}
-- 	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetRedPackageAwardList", req)
-- end

function this:onReqShopBaseInfo(notice)
    local req = notice:GetObj()
    --	printDebug("发送给服务器的请求商店id为："..table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "shop", "ReqGetShopInfo", req)
end

function this:onReqMapRedBag(notice)
    local req = notice:GetObj()
    printDebug("发送给服务器的请求领取地图红包id为：" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqRcvMapRedPacket", req)
end

function this:onReqRcvRedBagCoupon(notice)
    local obj = notice:GetObj()
    local req = {}
    if obj.isFromChat then
        req.active_id = obj.info.active_id
        req.red_packet_id = obj.info.red_packet_id
        req.coupon_id = obj.info.coupon_id
        this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqReceiveActiveCouponRedpacket", req)
    else
        req.red_packet_id = obj.info.red_packet_id
        req.coupon_id = obj.info.coupon_id
        this.sendNetMsg(GameConfig.ServerName.MainGateway, "coupon", "ReqRcvCoupon", req)
    end
end

function this:onReqMapRedBagCoupon(notice)
    local req = notice:GetObj()
    --	printDebug("发送给服务器的请求领取卡券红包id为："..table.tostring(req))
    --this.sendNetMsg(GameConfig.ServerName.MainGateway,"platform","ReqRcvMapRedPacket",req)
end

function this:onReqActiveRank(notice)
    local req = notice:GetObj()
    --	printDebug("发送给服务器的请求排行榜数据为："..table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqActiveRank", req)
end

--------------------------------------------收到协议(服务端发出)-------------------------------

--收到商店基本信息请求通知返回
function this.onReceiveShopBaseInfo(protoBytes)
    --	printDebug("=========================================================收到商店基本信息请求通知返回================================================")
    local rsp = this.decodeProtoBytes("shop", "RspGetShopInfo", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformNoticeType.Receive_Shop_Base_Data, rsp)
end

--收到拆红包通知
function this.onReceiveRcvRedPacket(protoBytes)
    --[[printDebug("=========================================================收到拆红包通知返回================================================")
	local rsp = this.decodeProtoBytes("platform","RspRcvActiveCashRedPacket", protoBytes)   
    NoticeManager.Instance:Dispatch(PlatformNoticeType.Platform_Rsp_Rcv_Red_Bag,rsp)--]]
end

--收到地图拆红包通知
function this.onReceiveMapRedPacket(protoBytes)
    -- printDebug("=========================================================收到地图拆红包通知返回================================================")
end

--收到排行榜数据返回
function this.onReceiveActiveRank(protoBytes)
    --	printDebug("=========================================================收到排行榜数据返回================================================")
    local rsp = this.decodeProtoBytes("active", "RspActiveRank", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformNoticeType.Platform_Rsp_Active_Rank, rsp)
end
