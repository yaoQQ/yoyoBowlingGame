require "animal:module/AnimalModule"
require "animal:module/AnimalNetModule"

require "animal:module/view/AnimalMatchView"
require "animal:module/view/AnimalPopupView"

require "animal:module/view/AnimalPoolView"
require "animal:module/view/AnimalGameView"
require "animal:module/view/AnimalOverView"

require "animal:table/TableAnimalObjectPool"
require "animal:AnimalPreload"


AnimalPackage = AbstractPackage:new()
local this = AnimalPackage

this.packName = "animal"


this.moduleList={
    AnimalModule,
    AnimalNetModule,
}

this.protoList =
{
    "MSG_402_AnimalChess",
}

this.tableList =
{
    TableAnimalObjectPool,
}

this.viewList =
{
    AnimalMatchView,
    AnimalPopupView,
    AnimalPoolView,
    AnimalGameView,
    AnimalOverView,
}

return AnimalPackage
