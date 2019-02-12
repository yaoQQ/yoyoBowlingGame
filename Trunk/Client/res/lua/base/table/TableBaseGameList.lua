TableBaseGameList = {data = {}}

TableBaseGameList.m_isInit = false

function TableBaseGameList.init()
	if TableBaseGameList.m_isInit then
		return
	else
		TableBaseGameList.data[2] = { id = 2, sort_id = 1, name = "麻将胡了", info = "广东推倒胡玩法，4局比赛，按最终得分进行排名", count = 2, rule1 = "游戏局数", intro1 = "进行4局比赛", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "全天开赛,快乐搓麻", GameRule = "经典的广东麻将玩法，打牌使用万筒条、东南西北、中发白。玩家可以吃、碰、杠（推到胡可以听），可点炮或自摸胡牌，有玩家点炮时，允许多名玩家同时胡牌。同时也可以选择买不买马。", GamePicture = {"2-GamePicture1","2-GamePicture2","2-GamePicture3","2-GamePicture4","2-GamePicture5"}, name_e = "mahjonghul", banner_name = "mahjong_b1", players = 4, isStableOpen = 0}
		TableBaseGameList.data[5] = { id = 5, sort_id = 2, name = "弹一弹", info = "15回合弹一弹，按最终得分进行排名", count = 2, rule1 = "游戏限制", intro1 = "15次发射小球机会", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "全新手感,全新道具", GameRule = "玩家可以选择小球发射角度来对小球进行发射，进而通过碰撞消除下方不断升起的障碍物，不同数字的障碍物代表着需要多少次撞击才能够被消除。小球每次撞击障碍物都会获得分数，障碍物进入小球发射的区域则游戏失败。", GamePicture = {"5-GamePicture1","5-GamePicture2","5-GamePicture3"}, name_e = "marbles", banner_name = "marbles_b1", players = 1, isStableOpen = 1}
		TableBaseGameList.data[7] = { id = 7, sort_id = 4, name = "抢钢镚", info = "60秒游戏时间，按最终得分进行排名", count = 2, rule1 = "游戏限制", intro1 = "60秒的游戏时间", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "描述暂无", GameRule = "描述暂无", GamePicture = {"GamePicture"}, name_e = "coin", banner_name = "coin_b1", players = 1, isStableOpen = 0}
		TableBaseGameList.data[1] = { id = 1, sort_id = 3, name = "炫舞消除", info = "经典消除竞技，每局1分钟，按最终得分进行排名", count = 1, rule1 = "游戏时间", intro1 = "1分钟", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "炫酷热舞,经典消除", GameRule = "移动小魔怪，同色小魔怪在行或列的方向上组成3个及以上即可消除。", GamePicture = {"GamePicture"}, name_e = "eliminate", banner_name = "eliminate_b1", players = 1, isStableOpen = 1}
		TableBaseGameList.data[8] = { id = 8, sort_id = 5, name = "斗兽棋", info = "暗棋模式，1局比赛，按最终得分进行排名", count = 2, rule1 = "游戏限制", intro1 = "60秒的游戏时间", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "描述暂无", GameRule = "描述暂无", GamePicture = {"GamePicture"}, name_e = "animal", banner_name = "animal_b1", players = 2, isStableOpen = 1}
		TableBaseGameList.data[3] = { id = 3, sort_id = 6, name = "3D保龄球", info = "1局10回合，按最终得分进行排名", count = 1, rule1 = "游戏限制", intro1 = "1局10回合", rule2 = "排名规则", intro2 = "以最后得分进行排名", GameSketch = "描述暂无", GameRule = "描述暂无", GamePicture = {"GamePicture"}, name_e = "bowling", banner_name = "animal_b1", players = 2, isStableOpen = 0}
		TableBaseGameList.m_isInit = true
	end
end

function TableBaseGameList.clear()
	TableBaseGameList.m_isInit = false
	TableBaseGameList.data = {}
end
