TableBaseSigninday = {data = {}}

TableBaseSigninday.m_isInit = false

function TableBaseSigninday.init()
	if TableBaseSigninday.m_isInit then
		return
	else
		TableBaseSigninday.data[1] = { id = 1, day = 1, money = 3, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[2] = { id = 2, day = 2, money = 4, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[3] = { id = 3, day = 3, money = 5, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[4] = { id = 4, day = 4, money = 6, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[5] = { id = 5, day = 5, money = 7, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[6] = { id = 6, day = 6, money = 8, money1 = 15, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.data[7] = { id = 7, day = 7, money = 30, money1 = 100, power1 = 100, diamond = 0, diamond1 = 0, power2 = 0, gold = 0, gold1 = 0, power3 = 0}
		TableBaseSigninday.m_isInit = true
	end
end

function TableBaseSigninday.clear()
	TableBaseSigninday.m_isInit = false
	TableBaseSigninday.data = {}
end
