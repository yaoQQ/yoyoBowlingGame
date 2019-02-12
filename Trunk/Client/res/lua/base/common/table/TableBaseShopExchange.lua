TableBaseShopExchange = {data = {}}

TableBaseShopExchange.m_isInit = false

function TableBaseShopExchange.init()
	if TableBaseShopExchange.m_isInit then
		return
	else
		TableBaseShopExchange.data[1] = { id = 1, src_type = 1, src_item = 100, dest_type = 2, dest_item = 100, dest_level = 1}
		TableBaseShopExchange.data[2] = { id = 2, src_type = 2, src_item = 88, dest_type = 3, dest_item = 8800, dest_level = 1}
		TableBaseShopExchange.data[3] = { id = 3, src_type = 1, src_item = 1000, dest_type = 2, dest_item = 1080, dest_level = 3}
		TableBaseShopExchange.data[4] = { id = 4, src_type = 2, src_item = 888, dest_type = 3, dest_item = 100000, dest_level = 3}
		TableBaseShopExchange.data[5] = { id = 5, src_type = 1, src_item = 3800, dest_type = 2, dest_item = 4280, dest_level = 5}
		TableBaseShopExchange.data[6] = { id = 6, src_type = 2, src_item = 3880, dest_type = 3, dest_item = 500000, dest_level = 5}
		TableBaseShopExchange.m_isInit = true
	end
end

function TableBaseShopExchange.clear()
	TableBaseShopExchange.m_isInit = false
	TableBaseShopExchange.data = {}
end
