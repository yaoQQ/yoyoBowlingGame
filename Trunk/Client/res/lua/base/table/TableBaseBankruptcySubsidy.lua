TableBaseBankruptcySubsidy = {data = {}}

TableBaseBankruptcySubsidy.m_isInit = false

function TableBaseBankruptcySubsidy.init()
	if TableBaseBankruptcySubsidy.m_isInit then
		return
	else
		TableBaseBankruptcySubsidy.data[1] = { id = 1, less_condition = 1000, award_money = 3000, award_count = 3, refresh_time = 0}
		TableBaseBankruptcySubsidy.m_isInit = true
	end
end

function TableBaseBankruptcySubsidy.clear()
	TableBaseBankruptcySubsidy.m_isInit = false
	TableBaseBankruptcySubsidy.data = {}
end
