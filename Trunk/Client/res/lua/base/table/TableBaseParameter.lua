﻿TableBaseParameter = {data = {}}

TableBaseParameter.m_isInit = false

function TableBaseParameter.init()
	if TableBaseParameter.m_isInit then
		return
	else
		TableBaseParameter.data[1] = { id = 1, parameter = 1, description = "赛前题库，比赛开始前答题"}
		TableBaseParameter.data[2] = { id = 2, parameter = 5, description = "赛中题库，答题时间5分钟"}
		TableBaseParameter.data[3] = { id = 3, parameter = 1, description = "中场题库，下半场开始前答题"}
		TableBaseParameter.data[4] = { id = 4, parameter = 5, description = "世界杯竞猜赛前五道题"}
		TableBaseParameter.data[5] = { id = 5, parameter = 3, description = "世界杯中场休息出三道题"}
		TableBaseParameter.data[6] = { id = 6, parameter = 10, description = "赛中出题的时间间隔"}
		TableBaseParameter.data[7] = { id = 7, parameter = 5000, description = "活动时长超时费（超过两个小时后每小时收费（分））"}
		TableBaseParameter.data[8] = { id = 8, parameter = 0, description = "游戏内广告图收费（每张）（分）"}
		TableBaseParameter.data[9] = { id = 9, parameter = 5000, description = "活动宣传图收费（每张）（分）"}
		TableBaseParameter.data[10] = { id = 10, parameter = 200, description = "推广人数收费（每人）（分）"}
		TableBaseParameter.data[11] = { id = 11, parameter = 0, description = "赛前公告收费（每条）（分）"}
		TableBaseParameter.data[12] = { id = 12, parameter = 10000, description = "奖励金额下限（分）"}
		TableBaseParameter.data[13] = { id = 13, parameter = 4, description = "炫舞消除快速匹配的进入人数"}
		TableBaseParameter.data[14] = { id = 14, parameter = 1000, description = "每个房间初始赠送积分"}
		TableBaseParameter.data[15] = { id = 15, parameter = 1, description = "投注最小积分"}
		TableBaseParameter.data[16] = { id = 16, parameter = 200, description = "投注最大积分"}
		TableBaseParameter.data[17] = { id = 17, parameter = 100, description = "创建优惠券每张收费100（分）"}
		TableBaseParameter.data[18] = { id = 18, parameter = 5, description = "领取自己在线红包次数"}
		TableBaseParameter.data[19] = { id = 19, parameter = 50, description = "抢好友红包次数"}
		TableBaseParameter.data[20] = { id = 20, parameter = 5000, description = "玩家初始金币数量"}
		TableBaseParameter.data[21] = { id = 21, parameter = 100, description = "玩家初始钻石数量"}
		TableBaseParameter.data[22] = { id = 22, parameter = 1500, description = "商圈红包的可领取范围（米）"}
		TableBaseParameter.data[23] = { id = 23, parameter = 6, description = "用户版商圈内显示的红包数量（后端使用）"}
		TableBaseParameter.data[24] = { id = 24, parameter = 4, description = "用户版商圈外显示的红包数量（后端使用）"}
		TableBaseParameter.m_isInit = true
	end
end

function TableBaseParameter.clear()
	TableBaseParameter.m_isInit = false
	TableBaseParameter.data = {}
end