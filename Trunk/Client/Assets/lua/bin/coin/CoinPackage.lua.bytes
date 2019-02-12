require "coin:module/CoinModule"
require "coin:module/CoinNetModule"
require "coin:module/view/CoinLoadView"
require "coin:module/view/CoinPoolView"
require "coin:module/view/CoinGameView"
require "coin:module/view/CoinOverView"

require "coin:table/TableCoinCoinParameter"
require "coin:table/TableCoinObjectPool"
require "coin:module/CoinObjectPool"


CoinPackage = AbstractPackage:new()
local this = CoinPackage

this.packName = "coin"


this.moduleList={
    CoinModule,
    CoinNetModule,
}

this.protoList =
{

}

this.tableList =
{
    TableCoinCoinParameter,
    TableCoinObjectPool,
}

this.viewList =
{
    CoinLoadView,
    CoinPoolView,
    CoinGameView,
    CoinOverView,
}

return CoinPackage