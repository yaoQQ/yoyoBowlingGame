require "base:AbstractPreloadOrder"

AnimalPreload = AbstractPreloadOrder:new()
local this = AnimalPreload
this.name = "AnimalPreload"

this.preloadStyle = PreloadStyle.FullLoadingBar
this.uiPreloadList =
{
	AnimalPoolView,
	AnimalPopupView,
	AnimalGameView,
	AnimalOverView,
}

function this:onPreloadEnd()
	CSLoger.debug(Color.Yellow, "========UI和场景预加载完成=========",self.name)
	AnimalObjectPool:GetInstance():InitObjectPool()
end

function this:onPreloadStepEnd()
	CSLoger.debug(Color.Orange, "========预加载单步完成=========",self.name)
	NoticeManager.Instance:Dispatch(AnimalNoticeType.LoadStep)
end