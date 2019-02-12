--排行榜总榜（名人堂）界面
PlatformRankTotalView = BaseView:new()
local this = PlatformRankTotalView
this.viewName = "PlatformRankTotalView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_RankTotal_View, false)

--设置加载列表
this.loadOrders = {
    "base:rank/platform_rank_total_panel"
}

local CASH_ID = 1001
local m_curId = 0
local m_rankType

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
	self.main_mid.btn_join:AddEventListener(UIEvent.PointerClick, this.onBtnJoin)
	self.main_mid.btn_champion:AddEventListener(UIEvent.PointerClick, this.onBtnChampion)
end

function this.onBtnClose()
    ViewManager.close(UIViewEnum.Platform_RankTotal_View)
end

function this.onBtnJoin()
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
		return
	end
    
	this.main_mid.panel_main.gameObject:SetActive(false)
	this.main_mid.image_join.gameObject:SetActive(true)
	this.main_mid.image_champion.gameObject:SetActive(false)
	m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame
	PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame, m_curId)
end

function this.onBtnChampion()
    if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
		return
	end
    
	this.main_mid.panel_main.gameObject:SetActive(false)
	this.main_mid.image_join.gameObject:SetActive(false)
	this.main_mid.image_champion.gameObject:SetActive(true)
	m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion
	PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion, m_curId)
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView(msg)
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Rank_Update_RankTotalInfo, this.onUpdateRankTotalInfo)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Rank_Update_RankTotalInfo, this.onUpdateRankTotalInfo)
end

--打开界面时初始化
function this:initView(id)
    this.main_mid.panel_main.gameObject:SetActive(false)
	
	m_curId = id
	--请求排行
	if m_curId == CASH_ID then
		this.main_mid.text_name.Txt.text = "红包"
		--self.main_mid.test_tips.transform.localPosition = Vector3(0, 450, 0)
		self.main_mid.image_page.gameObject:SetActive(false)
		m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash, 0)
	elseif m_curId < CASH_ID then
		this.main_mid.text_name.Txt.text = TableBaseGameList.data[m_curId].name
		--self.main_mid.test_tips.transform.localPosition = Vector3(0, 494, 0)
		self.main_mid.image_page.gameObject:SetActive(true)
		self.main_mid.image_join.gameObject:SetActive(true)
		self.main_mid.image_champion.gameObject:SetActive(false)
		m_rankType = ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame
		PlatformRankModule.sendReqPlayerRank(ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame, m_curId)
	end
end

function this.onUpdateRankTotalInfo(notice, data)
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
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash then
		this.main_mid.text_desc_mine.text = "累计红包金额："..string.format("%0.2f", rsp.myself_rank.param / 100).."元"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
		this.main_mid.text_desc_mine.text = "累计参赛次数："..string.format("%s", rsp.myself_rank.param).."次"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
		this.main_mid.text_desc_mine.text = "累计冠军次数："..string.format("%s", rsp.myself_rank.param).."次"
	end
	
	--第1名
	if rsp.rank_info[1] ~= nil then
		this.main_mid.item_rank_1.gameObject:SetActive(true)
		downloadUserHead(rsp.rank_info[1].player_base_info.head_url, this.main_mid.image_head_1)
		this.main_mid.text_name_1.text = rsp.rank_info[1].player_base_info.nick_name
		if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash then
			this.main_mid.text_num_1.text = string.format("%0.2f", rsp.rank_info[1].param / 100).."元"
			this.main_mid.text_desc_1.text = "累计红包金额"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
			this.main_mid.text_num_1.text = string.format("%s", rsp.rank_info[1].param)
			this.main_mid.text_desc_1.text = "累计参赛次数"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
			this.main_mid.text_num_1.text = string.format("%s", rsp.rank_info[1].param)
			this.main_mid.text_desc_1.text = "累计冠军次数"
		end
	else
		this.main_mid.item_rank_1.gameObject:SetActive(false)
	end
	--第2名
	if rsp.rank_info[2] ~= nil then
		this.main_mid.item_rank_2.gameObject:SetActive(true)
		downloadUserHead(rsp.rank_info[2].player_base_info.head_url, this.main_mid.image_head_2)
		this.main_mid.text_name_2.text = rsp.rank_info[2].player_base_info.nick_name
		if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash then
			this.main_mid.text_num_2.text = string.format("%0.2f", rsp.rank_info[2].param / 100).."元"
			this.main_mid.text_desc_2.text = "累计红包金额"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
			this.main_mid.text_num_2.text = string.format("%s", rsp.rank_info[2].param)
			this.main_mid.text_desc_2.text = "累计参赛次数"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
			this.main_mid.text_num_2.text = string.format("%s", rsp.rank_info[2].param)
			this.main_mid.text_desc_2.text = "累计冠军次数"
		end
	else
		this.main_mid.item_rank_2.gameObject:SetActive(false)
	end
	--第3名
	if rsp.rank_info[3] ~= nil then
		this.main_mid.item_rank_3.gameObject:SetActive(true)
		downloadUserHead(rsp.rank_info[3].player_base_info.head_url, this.main_mid.image_head_3)
		this.main_mid.text_name_3.text = rsp.rank_info[3].player_base_info.nick_name
		if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash then
			this.main_mid.text_num_3.text = string.format("%0.2f", rsp.rank_info[3].param / 100).."元"
			this.main_mid.text_desc_3.text = "累计红包金额"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
			this.main_mid.text_num_3.text = string.format("%s", rsp.rank_info[3].param)
			this.main_mid.text_desc_3.text = "累计参赛次数"
		elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
			this.main_mid.text_num_3.text = string.format("%s", rsp.rank_info[3].param)
			this.main_mid.text_desc_3.text = "累计冠军次数"
		end
	else
		this.main_mid.item_rank_3.gameObject:SetActive(false)
	end
	
	table.remove(rsp.rank_info, 1)
	table.remove(rsp.rank_info, 1)
	table.remove(rsp.rank_info, 1)
	
	this.main_mid.CellRecycleScrollPanel:SetCellData(rsp.rank_info, this.onUpdateRankTotalItems, false)
end

--更新单个数据
function this.onUpdateRankTotalItems(go, data, index)
	--data数据的类型为common.MsgMatchGuessQuestionRankTotalInfo
	local item = this.main_mid.cellArr[index + 1]
	
	item.text_rank.text = tostring(data.rank)
	
	downloadUserHead(data.player_base_info.head_url, item.image_head)
	
	item.text_name.text = data.player_base_info.nick_name
	
	if m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash then
		item.text_num.text = string.format("%0.2f", data.param / 100).."元"
		item.text_desc.text = "累计红包金额"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame then
		item.text_num.text = string.format("%s", data.param)
		item.text_desc.text = "累计参赛次数"
	elseif m_rankType == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
		item.text_num.text = string.format("%s", data.param)
		item.text_desc.text = "累计冠军次数"
	end
end