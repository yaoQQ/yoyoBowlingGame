require "base:AbstractPreloadOrder"
require "base:BasePackage"
require "base:enum/UIViewEnum"
require "base:module/common/view/CommonUploadView"

BasePreloadOrder=AbstractPreloadOrder:new()
local this=BasePreloadOrder

this.preloadStyle=PreloadStyle.FullLoadingBar

this.uiPreloadList =
{
	CommonUploadView,
}

function this:onPreloadEnd()
	CSLoger.debug(Color.Yellow,"=========预加载完成===========")
end

function this:onPreloadStepEnd()

end