

require "base:enum/UIViewEnum"
require "base:mid/game/Mid_platform_game_rule_panel"
require "base:module/game/data/PlatformGameProxy"
require "base:table/TableBaseGameList"

local UIExEventTool = CS.UIExEventTool
--主界面：游戏
PlatformGameRuleView = BaseView:new()
local this = PlatformGameRuleView
this.viewName = "PlatformGameRuleView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Game_Rule_View, false)

--设置加载列表
this.loadOrders=
{
	"base:game/platform_game_rule_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid = Mid_platform_game_rule_panel
	self:BindMonoTable(gameObject, self.main_mid)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick, this.closeGameRuleView)
	self.main_mid.close_rule_image:AddEventListener(UIEvent.PointerClick, this.closeGameRuleView)
end

--override 打开UI回调
function this:onShowHandler(msg)
	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
	self:addNotice()	
	self:showGameRuleView(msg.gameId)

end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()
end


function this:addNotice()

end

function this:removeNotice()

end

function this:showGameRuleView(gameId)
	local content = TableBaseGameList.data[gameId].GameRule
	local shotList = TableBaseGameList.data[gameId].GamePicture
	self.main_mid.rule_content_text.text = content
	if table.empty(shotList)then
		self.main_mid.game_shot_panel.gameObject:SetActive(false)
	else
		self.main_mid.game_shot_panel.gameObject:SetActive(true)
		self.main_mid.game_shot_scroll_panel:SetCellData(shotList, function (go, data, index)
			local item = this.main_mid.shotItemArr[index + 1]
			--downloadGameIcon(data, item.game_shot_image)
			local picName = string.format("%s/%s", ImageType.GameIcon, data)
			item.game_shot_image.Img.color = UIExEventTool.HexToColor("#E3E3E3FF")
			downloadResizeImage(picName, item.game_shot_image, ResizeType.MinFit, 282, 502,"", 1,  function (sprite)
				item.game_shot_image.Img.color = UIExEventTool.HexToColor("#FFFFFFFF")
			end)
		end)
	end
	self.main_mid.enter_game_btn:AddEventListener(UIEvent.PointerClick, function ()
		print("点击了游戏规则面板的进入游戏")
		self:closeGameRuleView()
		GameManager.enterGame(gameId, EnumGameType.Hall, -1, -1)
	end)
end

function this:closeGameRuleView()
	ViewManager.close(UIViewEnum.Platform_Game_Rule_View)
end
