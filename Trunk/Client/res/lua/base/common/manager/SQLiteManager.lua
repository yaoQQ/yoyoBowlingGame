SqliteManager={}
local this=SqliteManager


SqlDataType =
{
	INTEGER = "INTEGER",--整数
    REAL = "REAL",--浮点数
	TEXT = "TEXT",--字符串
	BLOB = "BLOB",--大数据
	BOOLEAN="BOOLEAN",-- boolean 只支持1，0
	
	KEY = " PRIMARY KEY",-- 是否为主键 如: SqlDataType.INTEGER..SqlDataType.KEY 表示当前字段为主键
}


--创建DB数据库
function SqliteManager.CreateOrOpenDb(DBName)
	 SQLiteManager.Instance:CreateOrOpenDb(DBName) 
end

--- 是否存在表
function SqliteManager.isHaveTable(tableName)
	if SQLiteManager.Instance:isHaveTable(tableName) then
		return true
	end
	return false
end

--[[ /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>--]]
	--(string tableName, string[] colNames, string[] colTypes)
function SqliteManager.CreateTable(tableName,colNames,colTypes)
	 -- SQLiteManager.Instance.CreateTable("table2", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" });
	--SqliteManager.CreateTable(this.tableName, { "title", "money", "lng", "lat","discribe","imgs" }, { SqlDataType.TEXT, SqlDataType.INTEGER ,SqlDataType.REAL,SqlDataType.REAL,SqlDataType.TEXT,SqlDataType.TEXT})
	SQLiteManager.Instance:CreateTable(tableName,colNames,colTypes)
end

  --[[  根据条件查询返回数据
	/// <param name="tableName">表名.</param>
    /// <param name="items">返回的数据字段列表</param>
    /// <param name="colNames">对比的字段看列表</param>
    /// <param name="operations">SQL操作符号=,<,>等</param>
    /// <param name="colValues">查找的数据对比值.</param>--]]
--string tableName, string[] items, string[] colNames, string[] operations, string[] colValues
function SqliteManager.ReadTable(tableName,items,colNames,operations,colValues) 
	--SqliteManager.ReadTable("table2",{ "ID", "Age","Name" }, { "Age" }, { " = " }, { "19" })
	local sqlDataList = SQLiteManager.Instance:ReadTable(tableName, items, colNames, operations, this.switchSQLType(colValues))
	if sqlDataList==nil then
		return nil
	end
	if sqlDataList.Count<=0 then
		return nil
	end
	--printDebug("lua @@@@@@@@@@@@@@@@@@@sqlDataList.Count="..tostring(sqlDataList.Count))
	local dataAarry={}--所有数据集合
	for i = 0, sqlDataList.Count - 1 do
		local dic= sqlDataList[i]
		local tableData = table.DicToTable(dic)--一条完整数据 
		dataAarry[i+1] = tableData
	end
	printDebug("lua @@@@@@@@@@@@@@@@@@@dataAarry="..table.tostring(dataAarry))
	return dataAarry
end

  --[[  查询返回整个表的数据
	/// <param name="tableName">表名.</param>
	--]]
--string tableName
function SqliteManager.ReadFullTableData(tableName) 
  local sqlDataList = SQLiteManager.Instance:ReadFullTableData(tableName)
	local dataAarry={}--所有数据集合
	if sqlDataList==nil then
		return nil
	end
	if sqlDataList.Count<=0 then
		return nil
	end
--	printDebug("lua &&&&&&&&&&&sqlDataList.Count="..tostring(sqlDataList.Count))
	local dataAarry={}--所有数据集合
	for i = 0, sqlDataList.Count-1 do
		local dic= sqlDataList[i]
		local tableData = table.DicToTable(dic)--一条完整数据 
		dataAarry[i+1] = tableData
	end
--	printDebug("lua @@@@@@@@@@@@@@@@@@@dataAarry="..table.tostring(dataAarry))

	return dataAarry
end

	--[[   /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>  --]]
	----（string tableName, string[] colNames, string[] operations, string[] colValues）
function SqliteManager.DeleteValuesOR(tableName,colNames,operations,colValues) 
	--   SQLiteManager.Instance.DeleteValuesOR("table3", new string[] { "ID","ID" }, new string[] {" = "," = " },  new string[] { "101101010","101101010"});
	SQLiteManager.Instance:DeleteValuesOR(tableName, colNames, operations, this.switchSQLType(colValues))
end
function SqliteManager.DeleteValuesAND(tableName,colNames,operations,colValues) 
	SQLiteManager.Instance:DeleteValuesAND(tableName, colNames, operations, this.switchSQLType(colValues))
end
	
--[[ /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">修改的字段</param>
    /// <param name="colValues">修改的字段对应的数据(一字段对应一数据)</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>--]]
	--(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
function  SqliteManager.UpdateData(tableName,colNames,colValues,key,operation,value)
	--	//更新数据，将Name="张三"的记录中的Name改为"Zhang3"
    --    //  SQLiteManager.Instance.UpdateData("table2", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'张三'");
    --    //更新数据，将Name="张三"的记录中的Name改为"Zhang3",ID改为010
    --    SQLiteManager.Instance.UpdateData("table2", new string[] { "Name", "ID" }, new string[] { "'Zhang3'" ,"010"}, "Name", "=", "'张三'");
	--SqliteManager.CreateTable("RedPacketLBSView_100042", { "title", "money", "lng", "lat","discribe","imgs" }, { SqlDataType.TEXT, SqlDataType.INTEGER ,SqlDataType.REAL,SqlDataType.REAL,SqlDataType.TEXT,SqlDataType.TEXT})
	--SQLiteManager.Instance.UpdateData("RedPacketLBSView_100042", new string[] { "title", "money" }, new string[] { "'wwwww撒啊'","111111111111"}, "title", "=", "'哈哈啊哈'");
	
	SQLiteManager.Instance:UpdateData(tableName, colNames, this.switchSQLType(colValues),key,operation,  this.switchSQLType(value))
	-- SQLiteManager.Instance:UpdateData(tableName, col_Names, col_Values, "save_id", "=", save_id)
	--SqliteManager.UpdateData("RedPacketLBSView_100042", { "title", "money" },{"wwwww撒啊",111111111111}, "title", "=", "哈哈啊哈")
	
end

--[[/// <summary>
    /// 向指定数据表中插入字符串数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>--]]
	--(string tableName, string[] values)
function  SqliteManager.InsertStringInto(tableName,values)
	--   SQLiteManager.Instance.InsertStringInto("table2", new string[] { "cececece", "289187120", "19", "www.xuanyusong.com" });
	printDebug("***************************values="..table.tostring(values))
	local switchSqlStr =this.switchSQLType(values)
	printDebug("************************switchSqlStr="..table.tostring(switchSqlStr))
	SQLiteManager.Instance:InsertStringInto(tableName,switchSqlStr)
end

    --[[/// <summary>
    /// 插入数据
    /// 字符串传入格式："'cececece'"，支持其它类型整形："289187120"
    ///  SQLiteManager.Instance.InsertInto("table2", new string[] { "'cececece'", "289187120", "'xuanyusong@gmail.com'", "'www.xuanyusong.com'" });
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>--]]
--(string tableName, string[] values)
function  SqliteManager.InsertValues(tableName,values)
	--SqliteManager.InsertInto(this.tableName,{this.lbsData.title, this.lbsData.money ,this.lbsData.lat,this.lbsData.lng,this.lbsData.discribe ,this.lbsData.imgs[1]})
	printDebug("***************************values="..table.tostring(values))
	local switchSqlStr =this.switchSQLType(values)
	printDebug("************************switchSqlStr="..table.tostring(switchSqlStr))
	SQLiteManager.Instance:InsertValues(tableName,switchSqlStr)
end

--[[ /// <summary>
    /// 向指定数据表中插入或修改数据(存在)
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="values">插入的数值</param>--]]
	--(string tableName, string[] fieldValues, string[] values)
function  SqliteManager.InsertOrUpdateValues(tableName,values)
      SQLiteManager.Instance:InsertOrUpdateValues(tableName,this.switchSQLType(values))
end

--[[    /// <summary>
    /// 如果不存在就插入，存在就忽略的方式，
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="values">插入的数值</param>--]]
function  SqliteManager.InsertOrIgnoreValues(tableName,values)
      SQLiteManager.Instance:InsertOrIgnoreValues(tableName,this.switchSQLType(values))
end

--[[  /// <summary>
    /// 删除表里面的所有内容
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="values">插入的数值</param>--]]
function  SqliteManager.DeleteFullTable(tableName)
      SQLiteManager.Instance:DeleteFullTable(tableName)
end

--[[  /// <summary>
    /// 删除表
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="values">插入的数值</param>--]]
--function  SqliteManager.DeleteTable(tableName)
  --    SQLiteManager.Instance:DeleteTable(tableName)
--end

-- 转化lua数据为sql支持数据
-- 字符格式需多添加''
---看需求扩展支持其他数据格式
function this.switchSQLType(values)
	if values==nil then
		return ""
	end
	if type(values) =="string" then
		return "'"..values.."'"
	end
	local data={}
	for i=1,#values do
		if type(values[i]) =="string" then
			data[i] = "'"..tostring(values[i]).."'"
		elseif values[i]==nil or values[i]=="" then
			data[i] =""
		else
			data[i] = tostring(values[i])
		end
		
	--	if type(this.values[i]) == "boolean" then
			
	--	end
	end
	printDebug("data"..table.tostring(data))
	printDebug("values"..table.tostring(values))
	return data
end
