

require "base:enum/NoticeType"
require "base:enum/EnumMapDirection"
require "base:util/CommonViewUtil"

--require proto
require "base:enum/proto/ProtoEnumCommon"
require "base:enum/proto/ProtoEnumLogin"
require "base:enum/proto/ProtoEnumGateway"
require "base:enum/proto/ProtoEnumPlatform"
require "base:enum/proto/ProtoEnumFriendModule"
require "base:enum/proto/ProtoEnumActive"
require "base:enum/proto/ProtoEnumCoupon"
require "base:enum/proto/ProtoEnumShop"
require "base:enum/proto/ProtoEnumUserInfo"

--require module
require "base:module/example/PlatformExampleModule"
require "base:module/login/LoginModule"
require "base:module/main/MainModule"
require "base:module/platform/PlatformModule"
require "base:module/global/PlatformGlobalModule"
require "base:module/lbs/PlatformLBSModule"
require "base:module/Shop/PlatformShopModule"
require "base:module/shop/PlatformSearchModule"
require "base:module/user/PlatformUserModule"
require "base:module/friend/PlatformFriendModule"
require "base:module/coupon/PlatformCouponModule"
require "base:module/guess/PlatformGuessBetModule"
require "base:module/redpacket/PlatformRedpacketModule"
require "base:module/message/PlatformMessageModule"
require "base:module/redbag/PlatformRedBagModule"
require "base:module/sign/PlatformSignModule"
require "base:module/rank/PlatformRankModule"
require "base:module/catchpacket/PlatformCatchPacketModule"
require "base:common/manager/GameLoginModule"

--require table
require "base:common/table/TableBaseRechargeShop"
require "base:common/table/TableBaseShopExchange"
require "base:table/TableBaseCitySwitch"
require "base:table/TableBaseGameList"
require "base:table/TableBaseOfficalMatch"
require "base:table/TableBaseSetHelpList"
require "base:table/TableBaseParameter"
require "base:table/TableBaseBankruptcySubsidy"
require "base:table/TableBaseDisableTermsData"
require "base:table/TableBaseGetmoney"
require "base:table/TableBaseSigninday"
require "base:table/TableBaseSigninmonth"

--require view

require "base:enum/UIViewEnum"
require "base:module/example/view/PlatformExampleXXXView"

require "base:module/login/view/SelectServerView"
require "base:module/login/view/LoginView"
require "base:module/main/view/BgView"
require "base:module/main/view/StatusbarView"
require "base:module/main/view/AlertWindowView"
require "base:module/main/view/FeedbackTipsView"
require "base:module/main/view/WaittingView"
require "base:module/main/view/TipsView"
require "base:module/main/view/MapView"
require "base:module/main/view/MallView"
require "base:module/main/view/TopTipsView"

require "base:module/lbs/view/PlatformLbsView"
require "base:module/lbs/view/PlatformLocalOfficialRuleView"
require "base:module/lbs/view/PlatformLoaclOfficialRankView"

--game ref
require "base:module/game/view/PlatformGlobalGameView"
require "base:module/game/view/PlatformGameUpdateView"
require "base:module/game/view/PlatformGameRuleView"

require "base:module/global/view/PlatformGlobalMessageView"
require "base:module/global/view/PlatformGlobalPersonalView"
require "base:module/global/view/PlatformGlobalView"

require "base:module/message/view/PlatformMessageSecondView"
require "base:module/message/view/PlatformMessageMainView"
require "base:module/message/view/PlatformRewardEndView"

require "base:module/redbag/view/PlatformGlobalRedBagView"
require "base:module/redbag/view/PlatformGlobalRecommendationOfFriendView"
require "base:module/redbag/view/PlatformRedBagRedPacketOpenView"
require "base:module/redbag/view/PlatformRedBagEndView"
require "base:module/redbag/view/PlatformRedBagOpenView"
require "base:module/redbag/view/PlatformRedBagShareView"
require "base:module/redbag/view/PlatformShareEndView"
require "base:module/redbag/view/PlatformRedBagPassView"

require "base:module/friend/view/PlatformFriendRecommendView"
require "base:module/friend/view/PlatformFriendMainPageView"
require "base:module/friend/view/PlatformFriendApplyView"
require "base:module/friend/view/PlatformFriendView"
require "base:module/friend/view/PlatformFriendChatView"
require "base:module/friend/view/PlatformFriendSearchView"

require "base:module/Shop/view/PlatformShopDetailInfoView"
require "base:module/Shop/view/PlatformGlobalShopMainView"
require "base:module/Shop/view/PlatformGlobalShopChatView"
require "base:module/Shop/view/PlatformGlobalShopView"
require "base:module/Shop/view/PlatformActiveRewardView"
require "base:module/Shop/view/PlatformActiveRankView"
require "base:module/Shop/view/PlatformShopSearchView"
require "base:module/Shop/view/PlatformActiveChatView"
require "base:module/Shop/view/PlatformActiveEndRewardView"

--common
require "base:module/common/view/PlatformCommonSearchView"
require "base:module/common/view/PlatformCommonQRCodeView"
require "base:module/common/view/CommonBottomSelectView"
require "base:module/common/view/EnlargePhotoView"
require "base:module/common/view/PlatformCommonAgreementView"
require "base:module/common/view/PlatformCommonTopCostView"
require "base:module/common/view/CommonUploadView"
require "base:module/common/view/PlatformCommonSubsidyView"

--卡券
require "base:module/coupon/view/PlatformCouponMainView"
require "base:module/coupon/view/PlatformCouponDetailView"
require "base:module/coupon/view/PlatformCouponShopListView"
require "base:module/coupon/view/PlatformCouponShopMapView"
require "base:module/coupon/view/PlatformCouponUseKnowView"

require "base:module/personal/view/PersonalChangeInfoView"
require "base:module/personal/view/PersonalSingleChangeView"
require "base:module/personal/view/PlatformPhotoDisplayView"

require "base:module/common/view/CommonDateSelectView"

require "base:module/guess/view/PlatformGuessChatRoomView"
require "base:module/guess/view/PlatformGuessBetRoomListView"
require "base:module/guess/view/PlatformGuessBetListView"
require "base:module/guess/view/PlatformGuessMyBetView"
require "base:module/guess/view/PlatformGuessRewardRankView"
require "base:module/guess/view/PlatformGuessRewardScoreView"
require "base:module/guess/view/PlatformGuessQuestionRankView"
require "base:module/guess/view/PlatformGuessBetView"

require "base:module/set/view/PlatformSetMainView"
require "base:module/set/view/PlatformSetAboutView"
require "base:module/set/view/PlatformSetHelpView"

--require "base:module/redbag/view/PlatformRedBagWithDrawView"
require "base:module/redbag/view/PlatformRedBagWithDrawRecordView"
require "base:module/redbag/view/PlatformRedBagWithDrawRulesView"
require "base:module/redbag/view/PlatformRedBagWithDrawPromptView"

require "base:module/lbs/view/PlatformLBSCouponDetailView"
require "base:module/lbs/view/PlatformLBSCouponOpenView"
require "base:module/lbs/view/PlatformLBSRedPacketDetailView"
require "base:module/lbs/view/PlatformLBSRedPacketOpenView"

--聊天室相关
require "base:module/redpacket/view/PlatformChatRoomRedpacketDetailView"
require "base:module/redpacket/view/PlatformChatRoomRedpacketOpenView"

--商城
require "base:module/mall/view/PlatformMallView"
require "base:module/mall/view/PlatformMallLandsView"
require "base:module/mall/view/PlatformMallTipView"
require "base:module/mall/view/PlatformMallTipConfigView"
require "base:module/mall/view/PlatformMallTipLandsView"

--PersonalChange临时
require "base:module/change/view/PlatformChangeFriendView"

-- 直接平台游戏
require "base:module/catchpacket/view/CatchPacketView"

--签到
require "base:module/sign/view/PlatformSignView"
--排行榜
require "base:module/rank/view/PlatformRankView"
require "base:module/rank/view/PlatformRankTotalView"


BasePackage = AbstractPackage:new()
local this = BasePackage

this.packName = "base"

--配置表

this.moduleList = {
    PlatformExampleModule,
    LoginModule,
    MainModule,
    PlatformModule,
    PlatformGlobalModule,
	PlatformLBSModule,
    PlatformShopModule,
    PlatformSearchModule,
    PlatformUserModule,
    PlatformFriendModule,
    PlatformCouponModule,
    PlatformGuessBetModule,
    PlatformRedpacketModule,
    PlatformMessageModule,
    PlatformRedBagModule,
	PlatformSignModule,
	PlatformRankModule,
	PlatformCatchPacketModule,
    GameLoginModule,

}

this.protoList = {
    "MSG_00_Common",
    "MSG_00_Platform",
    "MSG_10_Login",
    "MSG_11_Gateway",
    "MSG_12_Platform",
    "MSG_15_Platform",
    "MSG_17_Platform",
	
	"MSG_Active_Common",
	"MSG_Platform_Active",
	
	"MSG_Coupon_Common",
	"MSG_Platform_Coupon",
	
    "MSG_Friend_Common",
    "MSG_Platform_FriendModule",
	
	"MSG_Shop_Common",
	"MSG_Platform_Shop",
	
    "MSG_UserInfo_Common",
    "MSG_Platform_UserInfo",
    "MSG_18_Game",
}

this.tableList = {
    TableBaseCitySwitch,
    TableBaseGameList,
    TableBaseOfficalMatch,
    TableBaseSetHelpList,
    TableBaseShopExchange,
    TableBaseRechargeShop,
    TableBaseParameter,
    TableBaseBankruptcySubsidy,
    TableBaseDisableTermsData,
    TableBaseGetmoney,
	TableBaseSigninday,
	TableBaseSigninmonth,
}

this.viewList = {
    PlatformExampleXXXView,
    BgView,
    StatusbarView,
    SelectServerView,
    LoginView,
    AlertWindowView,
    FeedbackTipsView,
    WaittingView,
    SearchView,
    CommonUploadView,
    TipsView,
    MapView,
    MallView,
    TopTipsView,
    PlatformLbsView,
    PlatformLocalOfficialRuleView,
    PlatformLoaclOfficialRankView,
    PlatformGlobalGameView,
    PlatformGameUpdateView,
    PlatformGameRuleView,
    PlatformGlobalMessageView,
    PlatformGlobalPersonalView,
    PlatformGlobalView,
    PlatformMessageSecondView,
    PlatformMessageMainView,
    PlatformRewardEndView,
    PlatformGlobalRecommendationOfFriendView,
    PlatformGlobalRedBagView,
    PlatformFriendRecommendView,
    PlatformFriendMainPageView,
    PlatformFriendApplyView,
    PlatformFriendView,
    PlatformFriendChatView,
    PlatformFriendSearchView,
    PlatformShopDetailInfoView,
    PlatformGlobalShopMainView,
    PlatformGlobalShopChatView,
    PlatformGlobalShopView,
    PlatformActiveRewardView,
    PlatformActiveRankView,
    PlatformShopSearchView,
    PlatformActiveChatView,
    PlatformActiveEndRewardView,
    PlatformCommonSearchView,
    PlatformCommonQRCodeView,
    PlatformCommonAgreementView,
    CommonBottomSelectView,
    EnlargePhotoView,
    PlatformCommonTopCostView,
    PlatformCommonSubsidyView,
    --卡券
    PlatformCouponMainView,
    PlatformCouponDetailView,
    PlatformCouponShopListView,
    PlatformCouponShopMapView,
    PlatformCouponUseKnowView,
    --修改用户资料
    PersonalChangeInfoView,
    PersonalSingleChangeView,
    PlatformPhotoDisplayView,
    CommonDateSelectView,
    --设置
    PlatformSetMainView,
    PlatformSetAboutView,
    PlatformSetHelpView,
    --世界杯竞猜
    PlatformGuessChatRoomView,
    PlatformGuessBetRoomListView,
    PlatformGuessBetListView,
    PlatformGuessMyBetView,
    PlatformGuessRewardRankView,
    PlatformGuessRewardScoreView,
    PlatformGuessQuestionRankView,
    PlatformGuessBetView,
  --  PlatformRedBagWithDrawView,
    PlatformRedBagWithDrawRecordView,
    PlatformRedBagWithDrawRulesView,
    PlatformRedBagWithDrawPromptView,
    PlatformRedBagRedPacketOpenView,
    PlatformRedBagOpenView,
    PlatformRedBagEndView,
    PlatformRedBagShareView,
    PlatformShareEndView,
    PlatformRedBagPassView,
    --红包
    PlatformLBSCouponDetailView,
    PlatformLBSCouponOpenView,
    PlatformLBSRedPacketDetailView,
    PlatformLBSRedPacketOpenView,
    --聊天室相关
    PlatformChatRoomRedpacketOpenView,
    PlatformChatRoomRedpacketDetailView,
    --商城
    PlatformMallView,
    PlatformMallLandsView,
    PlatformMallTipView,
    PlatformMallTipConfigView,
    PlatformMallTipLandsView,
    --PersonalChange 临时
    PlatformChangeFriendView,
    -- 直接平台游戏
    CatchPacketView,
	--签到
	PlatformSignView,
	--排行榜
	PlatformRankView,
	PlatformRankTotalView,
	

}
