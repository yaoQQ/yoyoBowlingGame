TableEliminateProbabilityDataBase = {data = {}}

TableEliminateProbabilityDataBase.m_isInit = false

function TableEliminateProbabilityDataBase.init()
	if TableEliminateProbabilityDataBase.m_isInit then
		return
	else
		TableEliminateProbabilityDataBase.data[1] = { ID = 1, score = 2000, probability = 0.6}
		TableEliminateProbabilityDataBase.data[2] = { ID = 2, score = 4000, probability = 0.6}
		TableEliminateProbabilityDataBase.data[3] = { ID = 3, score = 8000, probability = 0.6}
		TableEliminateProbabilityDataBase.data[4] = { ID = 4, score = 12000, probability = 0.5}
		TableEliminateProbabilityDataBase.data[5] = { ID = 5, score = 18000, probability = 0.5}
		TableEliminateProbabilityDataBase.data[6] = { ID = 6, score = 25000, probability = 0.5}
		TableEliminateProbabilityDataBase.data[7] = { ID = 7, score = 35000, probability = 0.4}
		TableEliminateProbabilityDataBase.data[8] = { ID = 8, score = 45000, probability = 0.4}
		TableEliminateProbabilityDataBase.data[9] = { ID = 9, score = 55000, probability = 0.4}
		TableEliminateProbabilityDataBase.data[10] = { ID = 10, score = 65000, probability = 0.4}
		TableEliminateProbabilityDataBase.data[11] = { ID = 11, score = 75000, probability = 0.2}
		TableEliminateProbabilityDataBase.m_isInit = true
	end
end

function TableEliminateProbabilityDataBase.clear()
	TableEliminateProbabilityDataBase.m_isInit = false
	TableEliminateProbabilityDataBase.data = {}
end
