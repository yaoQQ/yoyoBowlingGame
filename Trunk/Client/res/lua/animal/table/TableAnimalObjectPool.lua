TableAnimalObjectPool = {data = {}}

TableAnimalObjectPool.m_isInit = false

function TableAnimalObjectPool.init()
	if TableAnimalObjectPool.m_isInit then
		return
	else
		TableAnimalObjectPool.data["d_fx_luandou_001"] = { keyName = "d_fx_luandou_001", projectName = "animal", path = "effect", defaultCount = 3}
		TableAnimalObjectPool.m_isInit = true
	end
end

function TableAnimalObjectPool.clear()
	TableAnimalObjectPool.m_isInit = false
	TableAnimalObjectPool.data = {}
end
