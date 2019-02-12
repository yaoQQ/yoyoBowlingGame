TableEliminateItemDataBase = {data = {}}

TableEliminateItemDataBase.m_isInit = false

function TableEliminateItemDataBase.init()
	if TableEliminateItemDataBase.m_isInit then
		return
	else
		TableEliminateItemDataBase.data[1] = { ID = 1, name = "狂热状态", duration = 3, startEffect = "~狂热~", clickEffect = "0", activatingEffect = "0", endEffect = "fx_a_item_craze", itemNature = "0", activeDec = "1", coverRule = 1, effectDec = "0"}
		TableEliminateItemDataBase.data[2] = { ID = 2, name = "随机消除", duration = 0, startEffect = "~随机消除~", clickEffect = "0", activatingEffect = "0", endEffect = "0", itemNature = "0", activeDec = "1", coverRule = 1, effectDec = "随机消除一种方块"}
		TableEliminateItemDataBase.data[3] = { ID = 3, name = "点击消除", duration = -1, startEffect = "~获得2个棒槌~", clickEffect = "0", activatingEffect = "0", endEffect = "", itemNature = "0", activeDec = "2", coverRule = 1, effectDec = "0"}
		TableEliminateItemDataBase.data[4] = { ID = 4, name = "消除妨碍", duration = 0, startEffect = "~邪魔退散~", clickEffect = "0", activatingEffect = "0", endEffect = "0", itemNature = "0", activeDec = "1", coverRule = 1, effectDec = "0"}
		TableEliminateItemDataBase.data[5] = { ID = 5, name = "冰冻", duration = 3, startEffect = "被冰冻了", clickEffect = "0", activatingEffect = "0", endEffect = "fx_a_item_ice_freeze", itemNature = "fx_a_item_ice_break", activeDec = "3", coverRule = 2, effectDec = "0"}
		TableEliminateItemDataBase.data[6] = { ID = 6, name = "云雾", duration = 6, startEffect = "迷雾出现了", clickEffect = "0", activatingEffect = "0", endEffect = "fx_a_item_fog", itemNature = "0", activeDec = "3", coverRule = 2, effectDec = "0"}
		TableEliminateItemDataBase.data[7] = { ID = 7, name = "震动", duration = 5, startEffect = "地震了", clickEffect = "0", activatingEffect = "0", endEffect = "0", itemNature = "0", activeDec = "4", coverRule = 2, effectDec = "0"}
		TableEliminateItemDataBase.data[8] = { ID = 8, name = "变灰", duration = 5, startEffect = "眼睛花了", clickEffect = "0", activatingEffect = "0", endEffect = "0", itemNature = "0", activeDec = "4", coverRule = 2, effectDec = "0"}
		TableEliminateItemDataBase.data[9] = { ID = 9, name = "洗牌", duration = 0, startEffect = "咕咕咕", clickEffect = "0", activatingEffect = "0", endEffect = "0", itemNature = "0", activeDec = "1", coverRule = 2, effectDec = "0"}
		TableEliminateItemDataBase.m_isInit = true
	end
end

function TableEliminateItemDataBase.clear()
	TableEliminateItemDataBase.m_isInit = false
	TableEliminateItemDataBase.data = {}
end
