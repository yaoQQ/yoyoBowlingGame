require "base:AbstractPreloadOrder"

BowlingPreload = AbstractPreloadOrder:new()
local this = BowlingPreload
this.name = "BowlingPreload"

this.preloadStyle = PreloadStyle.FullLoadingBar
this.uiPreloadList =
{
}

function this:onPreloadEnd()
	CSLoger.debug(Color.Yellow, "========UI和场景预加载完成=========",self.name)
	--BowlingObjectPool:GetInstance():InitObjectPool()
end

function this:onPreloadStepEnd()
	CSLoger.debug(Color.Orange, "========预加载单步完成=========",self.name)
	--NoticeManager.Instance:Dispatch(BowlingNoticeType.LoadStep)
end