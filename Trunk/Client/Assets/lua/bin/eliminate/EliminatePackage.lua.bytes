require "eliminate:module/eliminate/EliminateModule"
require "eliminate:module/eliminate/EliminateNetModule"
require "eliminate:module/eliminate/view/EliminateLoadView"
require "eliminate:module/eliminate/view/EliminatePopupView"
require "eliminate:module/eliminate/view/EliminatePoolView"
require "eliminate:module/eliminate/view/EliminateGameView"
require "eliminate:module/eliminate/view/EliminateOverView"
require "eliminate:module/eliminate/view/EliminateScene"

require "eliminate:table/ItemDataBase"

require "eliminate:table/TableEliminateItemDataBase"
require "eliminate:table/TableEliminateProbabilityDataBase"
require "eliminate:table/TableEliminateTableObjectPool"
require "eliminate:EliminatePreload"         -- 切记要先require其它lua文件之后再require该文件

EliminatePackage = AbstractPackage:new()
local this = EliminatePackage

this.packName = "eliminate"


this.moduleList={
    EliminateModule,
    EliminateNetModule,
}

this.protoList =
{
    "MSG_40_Eliminate",
}

this.tableList =
{
    TableEliminateItemDataBase,
    TableEliminateProbabilityDataBase,
    TableEliminateTableObjectPool
}

this.viewList =
{
    EliminateLoadView,
    EliminatePoolView,
    EliminateGameView,
    EliminateOverView,
    EliminatePopupView,
}

return EliminatePackage
