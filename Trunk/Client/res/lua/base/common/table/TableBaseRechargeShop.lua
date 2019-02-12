TableBaseRechargeShop = {data = {}}

TableBaseRechargeShop.m_isInit = false

function TableBaseRechargeShop.init()
	if TableBaseRechargeShop.m_isInit then
		return
	else
		TableBaseRechargeShop.data[1] = { id = 1, cash = 6, type = 2, item = 600}
		TableBaseRechargeShop.data[2] = { id = 2, cash = 18, type = 2, item = 1800}
		TableBaseRechargeShop.data[3] = { id = 3, cash = 30, type = 2, item = 3000}
		TableBaseRechargeShop.data[4] = { id = 4, cash = 68, type = 2, item = 6800}
		TableBaseRechargeShop.data[5] = { id = 5, cash = 328, type = 2, item = 32800}
		TableBaseRechargeShop.data[6] = { id = 6, cash = 648, type = 2, item = 64800}
		TableBaseRechargeShop.m_isInit = true
	end
end

function TableBaseRechargeShop.clear()
	TableBaseRechargeShop.m_isInit = false
	TableBaseRechargeShop.data = {}
end
