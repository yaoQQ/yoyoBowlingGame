TableCoinCoinParameter = {data = {}}

TableCoinCoinParameter.m_isInit = false

function TableCoinCoinParameter.init()
	if TableCoinCoinParameter.m_isInit then
		return
	else
		TableCoinCoinParameter.data[1] = { id = 1, weight = 50, subject = {2,20}, denomination5 = 5, number5 = {3,3}, denomination2 = 2, number2 = {4,4}, denomination1 = 1, number1 = {5,5}}
		TableCoinCoinParameter.data[2] = { id = 2, weight = 50, subject = {20,20}, denomination5 = 5, number5 = {2,4}, denomination2 = 2, number2 = {3,5}, denomination1 = 1, number1 = {4,6}}
		TableCoinCoinParameter.m_isInit = true
	end
end

function TableCoinCoinParameter.clear()
	TableCoinCoinParameter.m_isInit = false
	TableCoinCoinParameter.data = {}
end
