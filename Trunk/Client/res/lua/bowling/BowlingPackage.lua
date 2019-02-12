require "bowling:module/BowlingModule"
require "bowling:module/mvp/MVPGameModule"
require "bowling:BowlingPreload"
---±£¡‰«Ú”Œœ∑ 
require "bowling:module/view/BowlingScoreBoardView"
require "bowling:module/view/BowlingPunishTimeView"
require "bowling:module/view/BowlingGuideView"
require "bowling:module/view/BowlingStartCountView"
require "bowling:module/view/BowlingResultView"
require "bowling:module/view/BowlingQuitGameView"
require "bowling:module/view/BowlingLoadingView"

require "bowling:module/mvp/MVPGameModule"

BowlingPackage = AbstractPackage:new()
local this = BowlingPackage

this.packName = "bowling"


this.moduleList={
    BowlingModule,
	MVPGameModule,
}

this.protoList =
{
}

this.tableList =
{
}

this.viewList =
{
	BowlingScoreBoardView,
	BowlingPunishTimeView,
	BowlingGuideView,
	BowlingStartCountView,
	BowlingResultView,
	BowlingQuitGameView,
	BowlingLoadingView,
}

return BowlingPackage
