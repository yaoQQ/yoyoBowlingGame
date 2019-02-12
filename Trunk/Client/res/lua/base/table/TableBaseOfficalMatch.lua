TableBaseOfficalMatch = {data = {}}

TableBaseOfficalMatch.m_isInit = false

function TableBaseOfficalMatch.init()
	if TableBaseOfficalMatch.m_isInit then
		return
	else
		TableBaseOfficalMatch.data[1] = { Id = 1, gameid = 2, title = "全天红包赛", startTime = 0, shutTime = 24, loopTime = 30, startGameDesc = "每隔半小时开始一场新的竞赛", maxApplyCount = 4, inningsOfRound = 4, reward_type = 1, rewardtime = 1200, rewardDesc = "第1名 1元红包\n第2名 0.5元红包\n第3名 0.2元红包\n第4名到10名 0.05元红包\n第11名到50名 0.01元红包", titleIcon = 0, desc = "每半个小时开启一场\n比赛玩法：广东推倒胡\n赛制：4局积分赛\n结算：本场比赛活动结束且所有场内玩家完成比赛，按照最终积分进行排名获得邮件奖励。", mail_subject = "麻将红包赛活动奖励", mail_body = "　　恭喜您在“{}”获得{}分，最终排名为：第{}名，请领取您的排名奖励。", isStableOpen = 0}
		TableBaseOfficalMatch.data[2] = { Id = 2, gameid = 5, title = "全天红包赛", startTime = 0, shutTime = 24, loopTime = 30, startGameDesc = "每隔半小时开始一场新的竞赛", maxApplyCount = 4, inningsOfRound = 1, reward_type = 1, rewardtime = 0, rewardDesc = "第1名 1元红包\n第2名 0.5元红包\n第3名 0.2元红包\n第4名到10名 0.05元红包\n第11名到50名 0.01元红包", titleIcon = 1, desc = "每半个小时开启一场\n比赛玩法：弹一弹\n赛制：15回合积分赛\n结算：本场比赛活动结束，按照参赛玩家积分进行排名获得邮件奖励。", mail_subject = "弹一弹红包赛活动奖励", mail_body = "　　恭喜您在“{}”获得{}分，最终排名为：第{}名，请领取您的排名奖励。", isStableOpen = 1}
		TableBaseOfficalMatch.data[4] = { Id = 4, gameid = 1, title = "全天红包赛", startTime = 0, shutTime = 24, loopTime = 30, startGameDesc = "每隔半小时开始一场新的竞赛", maxApplyCount = 4, inningsOfRound = 1, reward_type = 1, rewardtime = 120, rewardDesc = "第1名 1元红包\n第2名 0.5元红包\n第3名 0.2元红包\n第4名到10名 0.05元红包\n第11名到50名 0.01元红包", titleIcon = 0, desc = "每半个小时开启一场\n比赛玩法：炫舞消除\n赛制：1局积分赛\n结算：本场比赛活动结束且所有场内玩家完成比赛，按照最终积分进行排名获得邮件奖励。", mail_subject = "炫舞消除红包赛活动奖励", mail_body = "　　恭喜您在“{}”获得{}分，最终排名为：第{}名，请领取您的排名奖励。", isStableOpen = 1}
		TableBaseOfficalMatch.data[5] = { Id = 5, gameid = 8, title = "全天红包赛", startTime = 0, shutTime = 24, loopTime = 30, startGameDesc = "每隔半小时开始一场新的竞赛", maxApplyCount = 4, inningsOfRound = 1, reward_type = 1, rewardtime = 600, rewardDesc = "第1名 1元红包\n第2名 0.5元红包\n第3名 0.2元红包\n第4名到10名 0.05元红包\n第11名到50名 0.01元红包", titleIcon = 0, desc = "每半个小时开启一场\n比赛玩法：斗兽棋\n赛制：1局积分赛\n结算：本场比赛活动结束且所有场内玩家完成比赛，按照最终积分进行排名获得邮件奖励。", mail_subject = "斗兽棋红包赛活动奖励", mail_body = "　　恭喜您在“{}”获得{}分，最终排名为：第{}名，请领取您的排名奖励。", isStableOpen = 1}
		TableBaseOfficalMatch.m_isInit = true
	end
end

function TableBaseOfficalMatch.clear()
	TableBaseOfficalMatch.m_isInit = false
	TableBaseOfficalMatch.data = {}
end
