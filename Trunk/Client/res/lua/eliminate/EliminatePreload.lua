require "base:AbstractPreloadOrder"

EliminatePreload = AbstractPreloadOrder:new()
local this = EliminatePreload

this.preloadStyle = PreloadStyle.FullLoadingBar
this.uiPreloadList =
{
    EliminatePoolView,
    EliminateGameView,
    EliminateOverView,
    EliminatePopupView,

}
this.scenePreload = EliminateScene

function this:onPreloadEnd()
	CSLoger.debug(Color.Yellow, "========UI和场景预加载完成=========",self)
    ObjectPool:GetInstance():initObjectPool()
end

function this:onPreloadStepEnd()
    NoticeManager.Instance:Dispatch(EliminateNoticeType.LoadStep)
end