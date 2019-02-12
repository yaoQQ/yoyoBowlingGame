NoticeType = {
    --====================通用Notice====================

    Example = "Example",
    Focus_TimeOut = "Focus_TimeOut",
    --GM
    GM_Send_To_MainGateway = "GM_Send_To_MainGateway",
    --正常退出游戏
    Normal_QuitGame = "Normal_QuitGame",
    --加载
    Loading_Bar_Show = "Loading_Bar_Show",
    Loading_Bar_Hide = "Loading_Bar_Hide",
    Loading_Annulus_Show = "Loading_Annulus_Show",
    Loading_Annulus_Hide = "Loading_Annulus_Hide",
    --网络
    Socket_Error = "Socket_Error",
    --录音结束
    Record_End = "Record_End",
    --游戏更新进度
    Game_Update_Progress = "Game_Update_Progress",
    --游戏退出
    Game_Exit = "Game_Exit",
    --游戏打开竖版商城
    Game_Open_Shop = "Game_Open_Shop",
    --游戏打开横版商城
    Game_Open_Shop_Lands = "Game_Open_Shop_Lands",
    --====================业务功能相关Notice====================

    --------------------登录--------------------
    --登录
    Login_WxAuthSucceed = "Login_WxAuthSucceed",
    Login_WxAuthFail = "Login_WxAuthFail",
	Login_AlipayAuthSucceed = "Login_AlipayAuthSucceed",
	Login_AlipayAuthFail = "Login_AlipayAuthFail",
    Login_SMSAuthSucceed = "Login_SMSAuthSucceed",
    Login_RegisterSucceed = "Login_RegisterSucceed",
    Login_ResetSucceed = "Login_ResetSucceed",
    Login_LoginGatewaySucceed = "Login_LoginGatewaySucceed",
    Login_ReqConnect = "Login_ReqConnect",
    Login_Send_ReqLogin = "Login_Send_ReqLogin",
    --登出
    Logout = "Logout",
    --------------------用户--------------------
    --更新用户信息
    User_Update_UserInfo = "User_Update_UserInfo",
    --更新现金数据
    User_Update_Cash = "User_Update_Cash",
    --初始化钻石金币数据
    User_Init_Diamond_Money = "User_Init_Diamond_Money",
    --更新钻石数据
    User_Update_Diamond = "User_Update_Diamond",
    --更新金币数据
    User_Update_Money = "User_Update_Money",
    --更新用户基本信息
    User_Update_UserBaseInfo = "User_Update_UserBaseInfo",
    --更新相册图片列表
    User_Update_AlbumPicList = "User_Update_AlbumPicList",
    --更新地图红包剩余次数
    User_Update_LBSRedPacketCounter = "User_Update_LBSRedPacketCounter",
    --更新在线分享红包剩余次数
    User_Update_OnlineRedPacketShareCounter = "User_Update_OnlineRedPacketShareCounter",
    --更新补贴剩余次数
    User_Update_BankruptcySubsidyCounter = "User_Update_BankruptcySubsidyCounter",
	--更新绑定账号信息
    User_Update_BindAcount = "User_Update_BindAcount",
    --------------------商城--------------------
    --更新商城钻石数据
    Mall_Update_Diamond = "Mall_Update_Diamond",
    --更新商城金币数据
    Mall_Update_Money = "Mall_Update_Money",
    --------------------游戏--------------------
    --游戏通用
    ActivityToEnterGame = "ActivityToEnterGame", -- 活动进入游戏
    --从游戏退出(通知界面)
    Game_Exit_Notice_View = "Game_Exit_Notice_View",
    --------------------地图--------------------
    Map_Add = "Map_Add",
    Map_Sub = "Map_Sub",
    Map_Back_My_Pos = "Map_Back_My_Pos",
    Map_Change_Cur_Pos = "Map_Change_Cur_Pos",
    Map_Change_Cur_Zoom = "Map_Change_Cur_Zoom",
    Map_Change_Scale_End = "Map_Change_Scale_End",
    --------------------LBS--------------------
    --更新我的位置
    LBS_Update_MyPos = "LBS_Update_MyPos",
    --更新附近的活动列表
    LBS_Update_ActivityListData = "LBS_Update_ActivityListData",
    --更新附近的红包列表
    LBS_Update_RedPacketListData = "LBS_Update_RedPacketListData",
    --更新附近的卷包列表
    LBS_Update_CouponListData = "LBS_Update_CouponListData",
    --更新附近的竞猜列表
    LBS_Update_GuessListData = "LBS_Update_GuessListData",
    --lbs界面状态改变
    LBS_Change_ViewState = "LBS_Change_ViewState",
    --更新附近合并
    LBS_Update_CombineIcon = "LBS_Update_CombineIcon",
    --清除lbs浮标
    LBS_Clear_Icon = "LBS_Clear_Icon",
    --lbs商铺界面活动列表显示
    LBS_ShopView_ActivityList = "LBS_ShopView_ActivityList",
    --更新热门搜索
    LBS_Update_HotSearchListData = "LBS_Update_HotSearchListData",
    --------------------活动（赛事）--------------------
    --更新活动游戏状态
    Activity_Update_ActiveGameState = "Activity_Update_ActiveGameState",
    --更新活动是否选择提醒
    Activity_Update_ActiveIsStart = "Activity_Update_ActiveIsStart",
    --关注赛事回复
    Activity_ActiveStart = "Activity_ActiveStart",
    --关注赛事列表回复
    Activity_ActiveStart_List = "Activity_ActiveStart_List",
    --最近参赛列表回复
    Activity_RecentGame_List = "Activity_RecentGame_List",
    --更新所有赛事搜索回复
    LBS_Update_SearchActivityListData = "LBS_Update_SearchActivityListData",
    --更新官方赛事
    LBS_Update_OfficialListData = "LBS_Update_OfficialListData",
    --点击手机返回按键
    Device_ReturnEvent = "Device_ReturnEvent",
    --搜索消息通知
    LBS_Search_Notice = "LBS_Search_Notice",
	--------------------签到--------------------
	--初始化签到信息
	Sign_Init_SignInfo = "Sign_Init_SignInfo",
	--签到后更新签到信息
	Sign_Update_SignInfo = "Sign_Update_SignInfo",
	--领取月签到奖励后更新信息
	Sign_Update_MonthInfo = "Sign_Update_MonthInfo",
	--------------------排行榜--------------------
	--更新排行榜信息
	Rank_Update_RankInfo = "Rank_Update_RankInfo",
	--更新排行榜总榜信息
	Rank_Update_RankTotalInfo = "Rank_Update_RankTotalInfo",
	--------------------接红包--------------------
	--接红包结束
	CatchPacket_End = "CatchPacket_End",
}
