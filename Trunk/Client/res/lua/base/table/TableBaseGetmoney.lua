﻿TableBaseGetmoney = {data = {}}

TableBaseGetmoney.m_isInit = false

function TableBaseGetmoney.init()
	if TableBaseGetmoney.m_isInit then
		return
	else
		TableBaseGetmoney.data[1] = { id = 1, money = 100, moneylimit = 100, friendlimit = 0, cd = 86400, tips = ""}
		TableBaseGetmoney.data[2] = { id = 2, money = 1000, moneylimit = 2000, friendlimit = 0, cd = 86400, tips = "10元提现需要余额大于20元"}
		TableBaseGetmoney.m_isInit = true
	end
end

function TableBaseGetmoney.clear()
	TableBaseGetmoney.m_isInit = false
	TableBaseGetmoney.data = {}
end