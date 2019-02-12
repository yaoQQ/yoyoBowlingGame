require "base:enum/UIViewEnum"

--打开红包界面
PlatformLoaclOfficialRankView = BaseView:new()
local this = PlatformLoaclOfficialRankView
this.viewName = "PlatformLoaclOfficialRankView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LOCAL_OFFICIAL_RANK, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_official_rank_panel"
}

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.mask:AddEventListener(UIEvent.PointerClick, self.close)
end

--override 打开UI回调
function this:onShowHandler(msg)
    if table.empty(msg.offical_activity_rank) then
        return
    end
    for _, info in pairs(msg.offical_activity_rank) do
        if info.player_id == LoginDataProxy.playerId then
            downloadUserHead(info.header_url, self.main_mid.user_head_image)
            self.main_mid.name.text = info.nick_name
            self.main_mid.cur_rank.text = string.concat("第", info.rank, "名")
            local official_config = TableBaseOfficalMatch.data[msg.match_type or 1]
            local game_config = TableBaseGameList.data[official_config.gameid]
            self.main_mid.game_title.text = string.concat(game_config.name, "-", official_config.title)
        end
    end
end

function this.close()
    ViewManager.close(UIViewEnum.Platform_LOCAL_OFFICIAL_RANK)
end
