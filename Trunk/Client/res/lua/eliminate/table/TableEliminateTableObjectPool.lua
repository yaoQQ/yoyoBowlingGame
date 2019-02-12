﻿TableEliminateTableObjectPool = {data = {}}

TableEliminateTableObjectPool.m_isInit = false

function TableEliminateTableObjectPool.init()
	if TableEliminateTableObjectPool.m_isInit then
		return
	else
		TableEliminateTableObjectPool.data["fx_piece_eli"] = { keyName = "fx_piece_eli", projectName = "eliminate", path = "effect", defaultCount = 15}
		TableEliminateTableObjectPool.data["fx_tip"] = { keyName = "fx_tip", projectName = "eliminate", path = "effect", defaultCount = 5}
		TableEliminateTableObjectPool.data["fx_map_blue"] = { keyName = "fx_map_blue", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_map_green"] = { keyName = "fx_map_green", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_map_orange"] = { keyName = "fx_map_orange", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_map_purple"] = { keyName = "fx_map_purple", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_map_yellow"] = { keyName = "fx_map_yellow", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_dance_moment"] = { keyName = "fx_dance_moment", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_1006_01"] = { keyName = "fx_1006_01", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_1007_01"] = { keyName = "fx_1007_01", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_1008_01"] = { keyName = "fx_1008_01", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_stage_green"] = { keyName = "fx_stage_green", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_stage_blue"] = { keyName = "fx_stage_blue", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_stage_purple"] = { keyName = "fx_stage_purple", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_stage_orange"] = { keyName = "fx_stage_orange", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_stage_firework"] = { keyName = "fx_stage_firework", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_bomb_special"] = { keyName = "fx_bomb_special", projectName = "eliminate", path = "effect", defaultCount = 3}
		TableEliminateTableObjectPool.data["fx_cross_special"] = { keyName = "fx_cross_special", projectName = "eliminate", path = "effect", defaultCount = 3}
		TableEliminateTableObjectPool.data["fx_cross_eli_h"] = { keyName = "fx_cross_eli_h", projectName = "eliminate", path = "effect", defaultCount = 3}
		TableEliminateTableObjectPool.data["fx_cross_eli_v"] = { keyName = "fx_cross_eli_v", projectName = "eliminate", path = "effect", defaultCount = 3}
		TableEliminateTableObjectPool.data["fx_stage_salvo"] = { keyName = "fx_stage_salvo", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_a_item_craze"] = { keyName = "fx_a_item_craze", projectName = "eliminate", path = "effect", defaultCount = 1}
		TableEliminateTableObjectPool.data["fx_a_item_ice_freeze"] = { keyName = "fx_a_item_ice_freeze", projectName = "eliminate", path = "effect", defaultCount = 2}
		TableEliminateTableObjectPool.data["fx_a_item_ice_break"] = { keyName = "fx_a_item_ice_break", projectName = "eliminate", path = "effect", defaultCount = 2}
		TableEliminateTableObjectPool.data["fx_a_item_fog"] = { keyName = "fx_a_item_fog", projectName = "eliminate", path = "effect", defaultCount = 2}
		TableEliminateTableObjectPool.m_isInit = true
	end
end

function TableEliminateTableObjectPool.clear()
	TableEliminateTableObjectPool.m_isInit = false
	TableEliminateTableObjectPool.data = {}
end