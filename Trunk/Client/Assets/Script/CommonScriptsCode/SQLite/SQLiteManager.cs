using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Mono.Data.Sqlite;

[LuaCallCSharp]
public class SQLiteManager : Singleton<SQLiteManager>
{
    private SQLiteHelper _sql;
    private string SQL_DATA_NAME = "ClientSQLData";
    string dbPath;

    //【创建或者打开DB文件】
    public void CreateOrOpenDb(string dbName)
    {
        SQL_DATA_NAME = dbName;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        dbPath = "data source=" + Application.streamingAssetsPath + "/" + dbName + ".db";
        //dbPath = "data source="+ dbName + ".db";
#endif

#if UNITY_ANDROID
        dbPath = "URI=file:" + Application.persistentDataPath + "/" + dbName + ".db";
#endif


#if UNITY_IOS
        dbPath = "data source=" + Application.persistentDataPath + "/" + dbName + ".db";
        //iOS："data source=" + Application.persistentDataPath + "/dataBase.db";
#endif

        Debug.Log("得到的路径为：" + dbPath);
        _sql = new SQLiteHelper(dbPath, dbName);
    }

    private SQLiteHelper sql
    {
        get
        {
            if (_sql == null)
            {
                CreateOrOpenDb(SQL_DATA_NAME);
            }
            return _sql;
        }
    }
    /// <summary>
    /// 向指定数据表中插入或修改数据(存在)
    /// 返回错误信息
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public void InsertValues(string tableName, string[] values)
    {
        //获取数据表中字段数目
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.InsertValues(tableName, values);
        CloseConnection();
    }
    /// <summary>
    /// 向指定数据表中插入或修改数据(存在)
    /// /// REPLACE当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。
    /// 当发生NOT NULL约束冲突，导致冲突的NULL值会被字段缺省值取代。若字段无缺省值，执行ABORT算法
    /// 不返回错误信息
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public void InsertOrUpdateValues(string tableName, string[] values)
    {
        //获取数据表中字段数目
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.InsertOrUpdateValues(tableName, values);
        CloseConnection();
    }

    /// <summary>
    /// 如果不存在就插入，存在就忽略的方式，用insert or ignore：
    /// IGNORE当发生约束冲突，发生冲突的行将不会被插入或改变。但命令将照常执行。在冲突行之前或之后的行将被正常的插入和改变，且不返回错误信息。
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public void InsertOrIgnoreValues(string tableName, string[] values)
    {
        //获取数据表中字段数目
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.InsertOrIgnoreValues(tableName, values);
        CloseConnection();
    }
    /// <summary>
    /// 插入字符串数据，只支持字符类型数据
    ///   SQLiteManager.Instance.InsertInto("table2", new string[] { "cececece", "289187120", "xuanyusong@gmail.com", "www.xuanyusong.com" });
    /// </summary>
    /// <param name="table_name"></param>
    /// <param name="values"></param>
    public void InsertStringInto(string table_name, string[] values)
    {
        //  if (sql == null) return;
        if (!isHaveTable(table_name))
        {
            return;
        }
        var reader = sql.InsertStringInto(table_name, values);
        CloseConnection();
    }
    public void CloseConnection()
    {
        if (_sql == null)
            return;
        _sql.CloseConnection();
        _sql = null;
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>  
    public void DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //   if (sql == null) return;
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.DeleteValuesOR(tableName, colNames, operations, colValues);
        CloseConnection();
    }


    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param> 
    public void DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //if (sql == null) return;
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.DeleteValuesAND(tableName, colNames, operations, colValues);
        CloseConnection();
    }

    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段修改成的对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public void UpdateData(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
    {
        //  if (sql == null) return;
        if (!isHaveTable(tableName))
        {
            return;
        }
        var reader = sql.UpdateValues(tableName, colNames, colValues, key, operation, value);
        CloseConnection();
    }

    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">表名.</param>
    /// <param name="items">返回的数据字段列表</param>
    /// <param name="colNames">对比的字段看列表</param>
    /// <param name="operations">SQL操作符号=,<,>等</param>
    /// <param name="colValues">查找的数据对比值.</param> 
    public List<Dictionary<string, System.Object>> ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        if (!isHaveTable(tableName))
        {
            return null;
        }
        SqliteDataReader reader = sql.ReadTable(tableName, items, colNames, operations, colValues);
        List<Dictionary<string, System.Object>> dataList = new List<Dictionary<string, System.Object>>();
        // Debug.LogFormat("获取列数=" + reader.FieldCount);   //2  获取列数
        // Debug.LogFormat("嵌套深度=" + reader.Depth);        //0  嵌套深度
        // Debug.LogFormat("是否包含行=" + reader.HasRows);      //true  是否包含行
        //Debug.LogFormat("SqlDataReader是否关闭=" + reader.IsClosed);     //false SqlDataReader是否关闭 
        //Debug.LogFormat(reader.RecordsAffected);      //-1 执行T-SQL语句所插入、修改、删除的行数
        //Debug.LogFormat(reader.VisibleFieldCount);    //2  未隐藏的字段数目(一共就两列)
        int j = 0;
        while (reader.Read())
        {//一次一行
            Dictionary<string, System.Object> lineData = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {//列数
                string key = reader.GetName(i);
                object value = reader[i];
                lineData[key] = value;

            }
            dataList.Add(lineData);
            j++;
        }
        //Debug.LogFormat("@@@@@@@@@@@@@@@@@dataList.count=" + dataList.Count);
        //for (int i = 0; i < dataList.Count; i++) {
        //    Debug.LogFormat("@@@@@@@@@@@@@@@@@dataList[" + i + "]=" + dataList[i]);
        //    Dictionary<string, System.Object> dic = (Dictionary<string, System.Object>)dataList[i];
        //    foreach (KeyValuePair<string, System.Object> pair in dic) {
        //        System.Object value = pair.Value;
        //        Debug.LogFormat("pair.key=" + pair.Key + " pair.value=" + pair.Value + " valueType=" + pair.Value.GetType());
        //    }
        //}
        reader.Close();
        CloseConnection();
        return dataList;
    }

    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public List<Dictionary<string, System.Object>> ReadFullTableData(string table_name)
    {
        //if (sql == null) return null;
        if (!isHaveTable(table_name))
        {
            return null;
        }
        var reader = sql.ReadFullTable(table_name);

        List<Dictionary<string, System.Object>> dataList = new List<Dictionary<string, System.Object>>();

        while (reader.Read())
        {
            Dictionary<string, System.Object> lineData = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string key = reader.GetName(i);
                object value = reader[i];
                lineData[key] = value;
            }
            dataList.Add(lineData);
        }
        //Debug.LogFormat("@@@@@@@@@@@@@@@@@dataList.count=" + dataList.Count);
        //for (int i = 0; i < dataList.Count; i++) {
        //    Debug.LogFormat("@@@@@@@@@@@@@@@@@dataList[" + i + "]=" + dataList[i]);
        //    Dictionary<string, System.Object> dic = (Dictionary<string, System.Object>)dataList[i];
        //    foreach (KeyValuePair<string, System.Object> pair in dic) {
        //        System.Object value = pair.Value;
        //        Debug.LogFormat("pair.key=" + pair.Key + " pair.value=" + pair.Value + " valueType=" + pair.Value.GetType());
        //    }
        //}
        reader.Close();
        CloseConnection();

        return dataList;
    }

    ///删除整个表数据
    public void DeleteFullTable(string table_name)
    {
        // if (sql == null) return;
        if (!isHaveTable(table_name))
        {
            return;
        }
        sql.DeleteFullTable(table_name);
        CloseConnection();
    }

    ///删除表
    public void DeleteTable(string table_name)
    {
        // if (sql == null) return;
        if (!isHaveTable(table_name))
        {
            return;
        }
        sql.DeleteTable(table_name);
        CloseConnection();
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public void CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        // if (sql == null) return;
        Debug.LogFormat("@@@@@@@@@@@@@@@@@CreateTable  tableName=" + tableName);
        if (isHaveTable(tableName))
        {
            Debug.Log(tableName + "表已经存在数据库当中");
        }
        else
        {
            sql.CreateTable(tableName, colNames, colTypes);

        }
        CloseConnection();
    }

    public bool isHaveTable(string tableName)
    {
        var target = sql.isHaveTable(tableName);
        CloseConnection();
        return target;
    }
}


