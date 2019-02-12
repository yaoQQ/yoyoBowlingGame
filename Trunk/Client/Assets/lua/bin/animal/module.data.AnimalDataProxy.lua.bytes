---
--- Created by Administrator.
--- DateTime: 2018\11\15 0015 9:40
---

AnimalDataProxy = {}
local this = AnimalDataProxy

this.playerDic = nil
this.meUid = 0
this.loadViewCro = nil  -- 加载进度条是根据时间平滑移动的, 应该等进度条移动完了再执行加载完成
this.totalPreLoadCount = 0
this.curPreLoadCount = 0

function this:OnMatchEnd(match_info_list)
    self.meUid = LoginDataProxy.playerId
    self.playerDic = {}
    for _, v in pairs(match_info_list) do
        local player = AnimalPlayerData:new()
        player:InitPlayerData(v)
        self.playerDic[v.player_id] = player
    end
end

function this:GetPlayers()
    return self.playerDic
end

function this:GetMeLoadProgress()
    local me = self.playerDic[self.meUid]
    return me:GetLoadProgress()
end

function this:setMeLoadProgress(progress)
    local me = self.playerDic[self.meUid]
    return me:SetLoadProgress(progress)
end

function this:GetMeUid()
    return self.meUid
end

function this:GetPlayerBySeat(seat)
    local player = nil
    for _, v in pairs(self.playerDic) do
        if v:GetPlayerSeat() == seat then
            player = v
            break
        end
    end
    if player == nil then
        printError("斗兽棋-错误, 不存在对应seat的玩家: "..seat)
    end
    return player
end

function this:GetMeSeat()
    local me = self.playerDic[self.meUid]
    return me:GetPlayerSeat()
end

function this:GetPlayerById(id)
    return self.playerDic[id]
end

function this:GeMeData()
    return self.playerDic[self.meUid]
end

function this:GetOtherData()
    local other = nil
    for _, v in pairs(self.playerDic) do
        if v:GetPlayerId() ~= self.meUid then
            other = v
        end
    end
    return other
end

function this:OnDeadDataProxy(info)
    local player = self:GetPlayerBySeat(info.seat)
    player:OnDeadAnimal(info.id)
end

function this:OnLoadStart()
    local preCount = AnimalPreload:getPreLoadCount()
    local poolCount = AnimalObjectPool:GetInstance():GetPoolCount()
    self.totalPreLoadCount = preCount + poolCount
    CS.PreloadManager.Instance:ExecuteOrder(AnimalPreload)
end


function this:OnLoadStep()
    self.curPreLoadCount = self.curPreLoadCount + 1
    local progress = math.floor((self.curPreLoadCount / self.totalPreLoadCount) * 100)
    self:setMeLoadProgress(progress)
    if self.loadViewCro ~= nil then
        coroutine.stop(self.loadViewCro)
    end
    self.loadViewCro = coroutine.start(function ()
        while(AnimalMatchView:GetIsProSuccess() == false) do
            coroutine.step(self.loadViewCro)
        end
        AnimalNetModule.ReqLoadingProgress()
        --if self.curPreLoadCount >= self.totalPreLoadCount then
        --    NoticeManager.Instance:Dispatch(AnimalNoticeType.LoadComplete)
        --end
    end)

end

function this:OnOtherLoadStepEnd()

end

function this:OnExitDataProxy()
    self.playerDic = nil
    self.totalPreLoadCount = 0
    self.curPreLoadCount = 0
    self.meUid = 0
    if self.loadViewCro ~= nil then
        coroutine.stop(self.loadViewCro)
    end
    self.loadViewCro = nil
end