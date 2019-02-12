require "base:AbstractPreloadOrder"
require "coin:module/view/CoinPoolView"
require "coin:module/view/CoinGameView"
require "coin:module/view/CoinOverView"
CoinPreload = AbstractPreloadOrder:new()
local this = CoinPreload

this.preloadStyle = PreloadStyle.FullLoadingBar
this.uiPreloadList =
{
	CoinPoolView,
	CoinGameView,
	CoinOverView,
}

function this:onPreloadEnd()
	CSLoger.debug(Color.Yellow, "========UI和场景预加载完成=========",self)
	CoinObjectPool:GetInstance():InitObjectPool()
end

function this:onPreloadStepEnd()
	NoticeManager.Instance:Dispatch(CoinNoticeType.LoadStepEnd)
end