--排行榜界面
PlatformRankView = BaseView:new()
local this = PlatformRankView
this.viewName = "PlatformRankView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Rank_View, true)

--设置加载列表
this.loadOrders = {
    "base:rank/platform_rank_panel"
}

local CASH_ID = 1001
local m_isShowPanelMore = false
local m_curId = 0
local m_rankType
local m_isShowPanelChange = false

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
	
    --添加UI事件监听
    self:addEvent()
end

function this:addEvent()
    self.main_mid.btn_close:AddEventListener(UIEvent.PointerClick, this.onBtnClose)
	self.main_mid.btn_more:AddEventListener(UIEvent.PointerClick, this.onBtnMore)
	self.main_mid.image_more_bg:AddEventListener(UIEvent.PointerClick, this.onBtnMore)
	self.main_mid.btn_total:AddEventListener(UIEvent.PointerClick, this.onBtnTotal)
	self.main_mid.btn_change_rank_type_game:AddEventListener(UIEvent.PointerClick, this.onBtnChangeRankTypeGame)
	self.main_mid.image_change_bg:AddEventListener(UIEvent.PointerClick, this.onBtnChangeRankTypeGame)
	self.main_mid.btn_join:AddEventListener(UIEvent.PointerClick, this.onBtnJoin)
	self.main_mid.btn_champion:AddEventListener(UIEvent.PointerClick, this.onBtnChampion)
end

function this.onBtnClose()
    ViewManager.close(UIViewEnum.Platform_Rank_View)
end

function this.onBtnMore()
	if m_isShowPanelMore then
		m_isShowPanelMore = false
		this.main_mid.panel_more.gameObject:SetActive(false)
	else
		m_isShowPanelMore = true
		this.main_mid.panel_more.gameObject:SetActive(true)
	end
end

function this.onBtnTotal()
	ViewManager.open(UIViewEnum.Platform_RankTotal_View, m_curId)
end

function this.onBtnChangeRankTypeGame()
	if m_isShowPanelChange then
		m_isShowPanelChange = false
		this.main_mid.panel_change_rank_type_game.gameObject:SetActive(false)
	else
		m_isShowPanelChange = true
		this.main_mid.panel_change_rank_type_game.gameObject:SetActive(true)
	end
end

function this.onBtnJoin()
	this.changeRankType(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame)
end

function this.onBtnChampion()
	this.changeRankType(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion)
end

function this.changeRankType(rankType)
	m_isShowPanelChange = false
	this.main_mid.panel_change_rank_type_game.gameObject:SetActive(false)
	
	if m_rankType == rankType or m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash then
		return
	end
	
    this.main_mid.panel_main.gameObject:SetActive(false)
	
	m_rankType = rankType
	--请求排行
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame then
		this.main_mid.text_rank_type_game.Txt.text = "参赛次数"
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame, m_curId)
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion then
		this.main_mid.text_rank_type_game.Txt.text = "冠军次数"
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion, m_curId)
	end
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Rank_Update_RankInfo, this.onUpdateRankInfo)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Rank_Update_RankInfo, this.onUpdateRankInfo)
end

local function getTimeStr(year, month)
	local day = 30
	if month == 1 or month == 3 or month == 5 or month == 7 or month == 8 or month == 10 or month == 12 then
		day = 31
	elseif month == 2 then
		if math.floor(day / 4) * 4 == day then
			day = 29
		else
			day = 28
		end
	end
	return month..".1-"..month.."."..day
end

--打开界面时初始化
function this:initView()
	m_isShowPanelMore = false
	this.main_mid.panel_more.gameObject:SetActive(false)
	m_isShowPanelChange = false
	this.main_mid.panel_change_rank_type_game.gameObject:SetActive(false)
    this.main_mid.panel_main.gameObject:SetActive(false)
	
	m_curId = CASH_ID
	m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash
	this.main_mid.Image_bg_redpack.gameObject:SetActive(true)
	this.main_mid.Image_bg_game.gameObject:SetActive(false)
	this.main_mid.text_name.Txt.text = "红包榜"
	local dateTime = TimeManager.getServerDateTime()
	this.main_mid.text_time.Txt.text = "时间："..getTimeStr(dateTime.Year, dateTime.Month)
	
	this.main_mid.text_rank_type_redpack.gameObject:SetActive(true)
	this.main_mid.btn_change_rank_type_game.gameObject:SetActive(false)
	
	this:initMorePanel()
	
	--请求红包排行
	m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash
	PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash, 0)
end

function this:initMorePanel()
	local gameList = {}
	local dataList = {}
	local index = 1
	
	for _,v in pairs(TableBaseGameList.data) do
		if IS_TEST_SERVER then
			table.insert(gameList, v)
		else
			if v.isStableOpen == 1 then
				table.insert(gameList, v)
			end
		end
	end
	
	local fun = function(a, b)
		return a.sort_id < b.sort_id
	end
	table.sort(gameList, fun)
	
	dataList[index] = {id = 1001, name = "红包"}
	index = index + 1
	
	for _,v in pairs(gameList) do
		dataList[index] = {id = v.id, name = v.name}
		index = index + 1
	end
	
	this.main_mid.ScrollPanel_icon:SetCellData(dataList, this.onUpdateMoreIconItems, false)
end

--更新单个数据
function this.onUpdateMoreIconItems(go, data, index)
	local item = this.main_mid.iconCellArr[index + 1]
	
	--游戏图标
	downloadGameIcon(data.id, item.image_icon)
	
	item.text_name.Txt.text = data.name
		
	--按钮注册
	item.press:AddEventListener(
		UIEvent.PointerClick,
		function(eventData)
			this.changeRankId(data.id)
		end
	)
end

function this.changeRankId(id)
	m_isShowPanelMore = false
	this.main_mid.panel_more.gameObject:SetActive(false)
	
	if m_curId == id then
		return
	end
	
    this.main_mid.panel_main.gameObject:SetActive(false)
	
	m_curId = id
	--请求排行
	if id == CASH_ID then
		this.main_mid.text_name.Txt.text = "红包榜"
		this.main_mid.text_rank_type_redpack.gameObject:SetActive(true)
		this.main_mid.btn_change_rank_type_game.gameObject:SetActive(false)
		m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash
		this.main_mid.Image_bg_redpack.gameObject:SetActive(true)
		this.main_mid.Image_bg_game.gameObject:SetActive(false)
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash, 0)
	elseif id < CASH_ID then
		this.main_mid.text_name.Txt.text = TableBaseGameList.data[id].name
		this.main_mid.text_rank_type_redpack.gameObject:SetActive(false)
		this.main_mid.btn_change_rank_type_game.gameObject:SetActive(true)
		m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame
		this.main_mid.text_rank_type_game.Txt.text = "参赛次数"
		this.main_mid.Image_bg_redpack.gameObject:SetActive(false)
		this.main_mid.Image_bg_game.gameObject:SetActive(true)
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame, id)
	end
end

function this.onUpdateRankInfo(notice, data)
	--rsp:RspPlayerRank
	local rsp = data:GetObj()
	
	this.main_mid.panel_main.gameObject:SetActive(true)
	
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(userInfo.head_url, this.main_mid.image_head_mine)
	this.main_mid.text_name_mine.text = userInfo.nick_name
	if rsp.myself_rank.rank == 0 then
		this.main_mid.text_num_mine.text = "未上榜"
	else
		this.main_mid.text_num_mine.text = "第"..rsp.myself_rank.rank.."名"
	end
	
	m_rankType = rsp.rank_type
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash then
		this.main_mid.text_desc_2_mine.gameObject:SetActive(false)
		this.main_mid.text_desc_mine.text = "本月红包金额："..string.format("%0.2f", rsp.myself_rank.param / 100).."元"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame then
		this.main_mid.text_desc_2_mine.gameObject:SetActive(true)
		this.main_mid.text_desc_2_mine.text = "本月参赛次数："..string.format("%s", rsp.myself_rank.param).."次"
		this.main_mid.text_desc_mine.text = "本月冠军次数："..string.format("%s", rsp.myself_rank.param2).."次"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion then
		this.main_mid.text_desc_2_mine.gameObject:SetActive(true)
		this.main_mid.text_desc_2_mine.text = "本月冠军次数："..string.format("%s", rsp.myself_rank.param).."次"
		this.main_mid.text_desc_mine.text = "本月参赛次数："..string.format("%s", rsp.myself_rank.param2).."次"
	end
	
	this.main_mid.CellRecycleScrollPanel:SetCellData(rsp.rank_info, this.onUpdateRankItems, false)
end

--更新单个数据
function this.onUpdateRankItems(go, data, index)
	local item = this.main_mid.rankCellArr[index + 1]
	
	if data.rank == 1 then
		item.rank_text.gameObject:SetActive(false)
		item.icon_rank.gameObject:SetActive(true)
		item.icon_rank:ChangeIcon(0)
	elseif data.rank == 2 then
		item.rank_text.gameObject:SetActive(false)
		item.icon_rank.gameObject:SetActive(true)
		item.icon_rank:ChangeIcon(1)
	elseif data.rank == 3 then
		item.rank_text.gameObject:SetActive(false)
		item.icon_rank.gameObject:SetActive(true)
		item.icon_rank:ChangeIcon(2)
	else
		item.icon_rank.gameObject:SetActive(false)
		item.rank_text.gameObject:SetActive(true)
		item.rank_text.text = tostring(data.rank)
	end
	
	downloadUserHead(data.player_base_info.head_url, item.image_head)
	
	item.text_name.text = data.player_base_info.nick_name
	
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash then
		item.text_desc_2.gameObject:SetActive(false)
		item.text_desc.text = "本月红包金额"
		item.text_num.text = string.format("%0.2f", data.param / 100).."元"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame then
		item.text_desc_2.gameObject:SetActive(true)
		item.text_desc_2.text = "本月冠军次数："..string.format("%s", data.param2).."次"
		item.text_desc.text = "本月参赛次数"
		item.text_num.text = string.format("%s", data.param)
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion then
		item.text_desc_2.gameObject:SetActive(true)
		item.text_desc_2.text = "本月参赛次数："..string.format("%s", data.param2).."次"
		item.text_desc.text = "本月冠军次数"
		item.text_num.text = string.format("%s", data.param)
	end
end