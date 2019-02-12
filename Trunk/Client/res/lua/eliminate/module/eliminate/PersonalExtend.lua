---
--- Created by lichongzhi.
--- DateTime: 2017/10/24 10:18
--- 扩展工具类

table.find = function(this, value)
    for k,v in pairs(this) do
        if v == value then return k end
    end
end

-- 用于辅助调试的打印, 尤其是消息打印
table.tostring = function(data, _indent)
    if data==nil then
        printDebug("输入的table为空")
        return
    end
    local visited = {}
    local function dump(data, prefix)
        local str = tostring(data)
        if table.find(visited, data) ~= nil then return str end
        table.insert(visited, data)

        local prefix_next = prefix .. "  "
        str = str .. "\n" .. prefix .. "{"
        for k,v in pairs(data) do
            if type(k) == "number" then
                str = str .. "\n" .. prefix_next .. "[" .. tostring(k) .. "] = "
            else
                str = str .. "\n" .. prefix_next .. tostring(k) .. " = "
            end
            if type(v) == "table" then
                str = str .. dump(v, prefix_next)
            elseif type(v) == "string" then
                str = str .. '"' .. v .. '"'
            else
                str = str .. tostring(v)
            end
        end
        str = str .. "\n" .. prefix .. "}"
        return str
    end
    return dump(data, _indent or "")
end

table.merge = function(base, delta)
    if type(delta) ~= "table" then return end
    for k,v in pairs(delta) do
        base[k] = v
    end
end

table.extend = function(base, delta)
    if type(delta) ~= "table" then return end
    for i,v in ipairs(delta) do
        table.insert(base, v)
    end
end

table.len = function(tbl)
    if type(tbl) ~= "table" then return 0 end
    local n = 0
    for k,v in pairs(tbl) do n = n + 1 end
    return n
end

table.empty = function(tbl)
    if tbl == nil or next(tbl)==nil then return true end
    assert(type(tbl) == "table")
    -- if #tbl > 0 then return false end
    -- for k,v in pairs(tbl) do return false end
    return false
end

-- http://snippets.luacode.org/?p=snippets/Deep_copy_of_a_Lua_Table_2
table.clone = function(t)
    if type(t) ~= 'table' then return t end
    local mt = getmetatable(t)
    local res = {}
    for k,v in pairs(t) do
        if type(v) == 'table' then
            v = table.clone(v)
        end
        res[k] = v
    end
    setmetatable(res, mt)
    return res
end

-- http://snippets.luacode.org/?p=snippets/Table_Slice_116
table.slice = function(values,i1,i2)
    local res = {}
    local n = #values
    i1 = i1 or 1
    i2 = i2 or n
    if i2 < 0 then
        i2 = n + i2 + 1
    elseif i2 > n then
        i2 = n
    end
    if i1 < 1 or i1 > n then
        return {}
    end
    local k = 1
    for i = i1,i2 do
        res[k] = values[i]
        k = k + 1
    end
    return res
end

table.reverse = function(tab)
    local size = #tab
    local newTable = {}
    for i,v in ipairs(tab) do
        newTable[size+1-i] = v
    end
    return newTable
end

-- http://www.cplusplus.com/reference/algorithm/random_shuffle/
-- http://stackoverflow.com/questions/17119804/lua-array-shuffle-not-working
math.shuffle = function(array)
    local counter = #array
    while counter > 1 do
        local index = math.random(counter)
        array[index], array[counter] = array[counter], array[index]
        counter = counter - 1
    end
    return array
end

--以p概率产生1
math.single_prob = function(p)
    if math.random() < p then
        return 1
    else
        return 0
    end
end
