PlatformShopSearchProxy = {}
local this=PlatformShopSearchProxy

local searchText=nil
local m_shopSearchData = nil
local m_shopHotSearchData = nil

--设置热门搜索数据
function this.setHotSearchData(data)
   m_shopHotSearchData = data
   NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_HotSearchListData)
end

--获取热门搜索数据
function this.getHotSearchData()
    return m_shopHotSearchData
end


--设置最近搜索数据
function this.addShopLatelySearchData(data)
    searchText=data
    local str=PlayerPrefs.GetString("SHOP_SEARCH", "")
    local searchStr= string.concat(data,"|",str)
    PlayerPrefs.SetString("SHOP_SEARCH",searchStr)

end
--清除最近聊天数据
function this.clearShopLatelySearchData()
    PlayerPrefs.SetString("SHOP_SEARCH","")
end

--获取最近搜索数据
function this.getShopLatelySearchData()
    local str=PlayerPrefs.GetString("SHOP_SEARCH", "")   
    m_shopSearchData=string.split(str,'|')
    if m_shopSearchData ~= nil and #m_shopSearchData > 0 then
        for i=1,#m_shopSearchData-1 do
            for j = i+1, #m_shopSearchData do
                if m_shopSearchData[j] == m_shopSearchData[i]  then
                     table.remove(m_shopSearchData,j)
                end
            end
        end
    else
        m_shopSearchData = ""
    end


local searchStr = ""
for i=1,#m_shopSearchData do
    searchStr = string.concat(searchStr,"|",m_shopSearchData[i])
    PlayerPrefs.SetString("SHOP_SEARCH",searchStr)
end
    return m_shopSearchData

end

--获取搜索数据
function this.getShopSearchText()
    return searchText
end

