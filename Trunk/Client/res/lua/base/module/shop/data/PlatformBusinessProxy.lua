


PlatformBusinessProxy={}
local this=PlatformBusinessProxy


function PlatformBusinessProxy:new(o)  
    o = o or {}  
    setmetatable(o,self)  
    self.__index = self  
    return o  
end  


function PlatformBusinessProxy:GetInstance()  
    if self._instance == nil then  
        self._instance = self:new()  
        --初始化一下
        self:init()
    end  
  
    return self._instance  
end 

function this:init()
end

------------------------------商家主页------------------------------

this.mainBaseData = nil     --商户主页基本数据（包括商户的基本信息，商户的图片展示以及商户的游戏信息三类）

--获得商户主页数据
function this:getMainBaseData()
    return this.mainBaseData
end

function this:setSingleShopData(data)
    this.mainBaseData = data
end

------------------------------商店活动列表------------------------------

this.currShopGamesInfo = nil
--设置商家活动信息(多)
function this:setShopGamesInfo(data)
    this.currShopGamesInfo = data
end

--获取商家活动信息
function this:getShopGamesInfo()
    return this.currShopGamesInfo
end

------------------------------商店活动列表end------------------------------

this.currSingleShopGameData = nil
--通过活动id来设置活动数据
function this:setShopGameDataById(gameId)
    if this.currShopGamesInfo == nil then return end
printDebug("+++++++*****************--------------")
    for i=1,#this.currShopGamesInfo do
        if tostring(this.currShopGamesInfo[i].active_id) == gameId then
            this.currSingleShopGameData = this.currShopGamesInfo[i]
            printDebug("+++++++*****************--------------"..table.tostring(this.currSingleShopGameData))
            return
        end
    end
end

function this:setSingleShopGameData(data)
    this.currSingleShopGameData = data
end

function this:getSingleShopGameData()
    return this.currSingleShopGameData
end
------------------------------商家红包---------------------

this.shopRedBagData = nil
--设置商家红包信息
function this:setShopRedBagInfo(data)
    this.shopRedBagData = data
end

--获取商家红包信息
function this:getShopRedBagInfo()
    return this.shopRedBagData
end

this.shopGainRedBagData = nil
--设置领取红包信息
function this:setGainRedBagInfo(data)
    this.shopGainRedBagData = data
end

function this:getGainRedBagInfo()
    return this.shopGainRedBagData
end

this.shopOpenRedBagData = nil
--设置打开红包信息
function this:setOpenRedBagInfo(data)
    this.shopOpenRedBagData = data
end

--获取打开红包信息
function this:getOpenRedBagInfo()
    return this.shopOpenRedBagData
end


this.shopOpenRedBagCouponData = nil
--设置打开卡券红包信息
function this:setOpenRedBagCouponInfo(data)
    this.shopOpenRedBagCouponData = data
end

--获取打开卡券红包信息
function this:getOpenRedBagCouponInfo()
    return this.shopOpenRedBagCouponData
end

PlatformBusinessProxy.isCoupon = false

----------------------------------商家商城-----------------
this.shopMarketData = nil
--设置商城数据
function this:setShopMarketData(data)
    this.shopMarketData = data
end

--获取商城数据
function this:getShopMarketData()
    return this.shopMarketData
end

--------------------------------商家最新活动-------------
this.shopShortNewsData = nil
--设置商家最新活动简介
function this:setShopShortNewsData(data)
    this.shopShortNewsData = data
end

--获取商家最新活动简介
function this:getShopShortNewsData( ... )
    return this.shopShortNewsData
end

this.shopLongNewsData = nil
--设置商家具体最新活动
function this:setShopLongNewsData(data)
    this.shopLongNewsData = data
end

--获取商家具体最新活动
function this:getShopLongNewsData()
    return this.shopLongNewsData
end

this.shopNewsReplyData = nil
--设置商家活动留言
function this:setShopNewsReplyData(data)
    this.shopNewsReplyData = data
end
--获取商家活动留言
function this:getShopNewsReplyData()
    return this.shopNewsReplyData
end

---------------------------------排行榜------------------
this.shopRedBagRankData = nil
--设置红包排行榜数据
function this:setShopRedBagRankData(data)
    this.shopRedBagRankData = {}
    for i=1,#data do
        if data[i].rankType == 1 then
            table.insert(this.shopRedBagRankData,data[i])
        end
    end
end

--获取红包排行榜数据
function this:getShopRedBagRankData()
    return this.shopRedBagRankData
end

this.shopFocusRankData = nil
--设置排行榜数据
function this:setShopFocusRankData(data)
    this.shopFocusRankData = {}
    for i=1,#data do
        if data[i].rankType == 2 then
            table.insert(this.shopFocusRankData,data[i])
        end
    end
end

--获取排行榜数据
function this:getShopFocusRankData()
    return this.shopFocusRankData
end

this.myRedBagRankData = nil
--设置我的红包榜排名
function this:setMyRedBagRankData(data)
    this.myRedBagRankData = nil
    for i=1,#data do
        if data[i]~= nil and data[i].rankType ==1 then
            this.myRedBagRankData = data[i]
            break
        end
    end
end

--获取我的红包榜排名
function this:getMyRedBagRankData()
    return this.myRedBagRankData
end

this.myFocusRankData = nil
--设置我的关注度榜排名
function this:setMyFocusRankData(data)
    this.myFocusRankData = nil
    for i=1,#data do
        if data[i]~= nil and data[i].rankType ==1 then
            this.myFocusRankData = data[i]
            break
        end
    end
end

--获取我的关注度榜排名
function this:getMyFocusRankData()
    return this.myFocusRankData
end

---------------------------商家点评------------------

this.shopIntroData = nil

--设置商家介绍数据
function this:setShopIntroData(data)
    this.shopIntroData = data
end

--获取商家介绍数据
function this:getShopIntroData()
    return this.shopIntroData
end

this.shopIntroLongCommentData = nil

--设置商家用户评价详细数据
function this:setShopIntroLongCommentData(data)
    this.shopIntroLongCommentData = data
end

--获取商家用户评价详细数据
function this:getShopIntroLongCommentData()
    return this.shopIntroLongCommentData
end

---------------------竞猜信息------------------
----竞猜商户--------------------------
-------商户聊天室-------------------
-----------商户信息----------------
-----------商户（答题，竞猜，聊天，排行榜等）数据------------
-----------个人数据(答题，红包，竞猜，聊天等)----------





this.shopGuessIntroData = nil


--设置竞猜商户数据
function this:setShopGuessData(data)
	this.shopGuessIntroData = data
end

--获取竞猜商户数据
function this:getShopGuessData()
	return this.shopGuessIntroData
end

------------针对单个商户的导航------
----------------需要清除地图上其他图标的显示-----

function this:cleanAllData()
	this.shopGuessIntroData = nil
	this.shopIntroLongCommentData = nil
	this.shopIntroData = nil
	this.myFocusRankData = nil
	this.myRedBagRankData = nil
end

