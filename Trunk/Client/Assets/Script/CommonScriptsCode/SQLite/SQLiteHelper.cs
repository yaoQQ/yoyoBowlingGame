using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;

public class SQLiteHelper
{
    /// <summary>
    /// 数据库连接定义
    /// </summary>
    private SqliteConnection dbConnection;

    /// <summary>
    /// SQL命令定义
    /// </summary>
    private SqliteCommand dbCommand;

    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader dataReader;

    private string _dataBaseName;

    /// <summary>
    /// 构造函数    
    /// </summary>
    /// <param name="connectionString">数据库连接字符串</param>
    public SQLiteHelper(string connectionString,string dataBaseName) {
        try {
            OpenDB(connectionString);
            _dataBaseName = dataBaseName;
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
    public void OpenDB(string connectionString) {
        try {
            //构造数据库连接
            dbConnection = new SqliteConnection(connectionString);
            //打开数据库
            dbConnection.Open();

            Debug.Log("Connected to " + connectionString + "success");
        }
        catch (Exception e) {
            string temp1 = e.ToString();
            Debug.Log(temp1);
        }

    }
    /// <summary>
    /// 执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString) {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        try {
            dataReader = dbCommand.ExecuteReader();
        }
        catch (Exception e) {
            Debug.LogError("错误 Sql语句==" + queryString);
            Debug.LogError(" e==" + e);
           
        }

        return dataReader;
    }
    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes) {
        if (colNames.Length != colTypes.Length) {
            throw new SqliteException("columns.Length != colType.Length");
        }
        string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++) {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        Debug.Log("创建表  queryString=" + queryString);
        return ExecuteQuery(queryString);
    }

    ///删除表所有内容
    public SqliteDataReader DeleteFullTable(string tableName) {
        string queryString = "DELETE FROM " + tableName;//修改语法 删除表内全部内容 不影响 表的存在
        return ExecuteQuery(queryString);
    }
    ///删除整个表
    public SqliteDataReader DeleteTable(string tableName) {
        string queryString = "DROP TABLE "+ _dataBaseName+"." + tableName;//修改语法 删除表
      // string queryString = "DROP TABLE " + tableName;//修改语法 删除表
        return ExecuteQuery(queryString);
        //DROP TABLE database_name.table_name;
        //DELETE FROM TableName
        //DROP TABLE database_name.table_name;
    }


    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseConnection() {
        Debug.Log("++++++++++++++++++++++++++++++++关闭DB！");
        //销毁Command
        if (dbCommand != null) {
            dbCommand.Cancel();
        }
        dbCommand = null;

        //销毁Reader
        if (dataReader != null) {
            dataReader.Close();
        }
        dataReader = null;

        //销毁Connection
        if (dbConnection != null) {
            dbConnection.Close();
        }
        dbConnection = null;
    }

    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName) {
        string queryString = "SELECT * FROM " + tableName;
        dataReader = ExecuteQuery(queryString);
        return dataReader;
    }
    /// <summary>
    /// 向指定数据表中插入数据
    /// 插入数据失败返回错误信息
      /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertValues(string tableName, string[] values) {
        //获取数据表中字段数目
        SqliteDataReader reader = ReadFullTable(tableName);
        if (reader == null) {
            Debug.LogError("InsertValues reader is NULL!! tableName=" + tableName);
            return null;
        }
        int fieldCount = reader.FieldCount;
        //  当插入的数据长度不等于字段数目时引发异常
        if (values.Length != fieldCount) {
            throw new SqliteException("values.Length!=fieldCount");
        }

        if (string.IsNullOrEmpty(values[0])) {
            values[0] = "''";
        }
        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            if (string.IsNullOrEmpty(values[i])) {
                values[i] = "''";
            }
            queryString += ", " + values[i];
        }
        queryString += " )";
        Debug.LogFormat("InsertValues  queryString=" + queryString);
        return ExecuteQuery(queryString);
    }
    /// <summary>
    /// 向指定数据表中插入数据
    /// 如果不存在就插入，存在就忽略的方式，用insert or ignore：
    /// REPLACE当发生UNIQUE约束冲突，先存在的，导致冲突的行在更改或插入发生冲突的行之前被删除。这样，更改和插入总是被执行。命令照常执行且不返回错误信息。当发生NOT NULL约束冲突，导致冲突的NULL值会被字段缺省值取代。若字段无缺省值，执行ABORT算法
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertOrUpdateValues(string tableName, string[] values) {
        //获取数据表中字段数目
        SqliteDataReader reader = ReadFullTable(tableName);
        if (reader == null) {
            Debug.LogError("InsertValues reader is NULL!! tableName=" + tableName);
            return null;
        }
        int fieldCount = reader.FieldCount;
        //  当插入的数据长度不等于字段数目时引发异常
        if (values.Length != fieldCount) {
            throw new SqliteException("values.Length!=fieldCount");
        }

        if (string.IsNullOrEmpty(values[0])) {
            values[0] = "''";
        }
        string queryString = "INSERT OR REPLACE INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            if (string.IsNullOrEmpty(values[i])) {
                values[i] = "''";
            }
            queryString += ", " + values[i];
        }
        queryString += " )";
        Debug.LogFormat("InsertValues  queryString=" + queryString);
        return ExecuteQuery(queryString);
    }
    /// <summary>
    /// 向指定数据表中插入数据
    /// 如果不存在就插入，存在就忽略的方式，用insert or ignore：
    /// IGNORE当发生约束冲突，发生冲突的行将不会被插入或改变。但命令将照常执行。在冲突行之前或之后的行将被正常的插入和改变，且不返回错误信息。
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertOrIgnoreValues(string tableName, string[] values) {
        //获取数据表中字段数目
        SqliteDataReader reader = ReadFullTable(tableName);
        if (reader == null) {
            Debug.LogError("InsertValues reader is NULL!! tableName=" + tableName);
            return null;
        }
        int fieldCount = reader.FieldCount;
        //  当插入的数据长度不等于字段数目时引发异常
        if (values.Length != fieldCount) {
            throw new SqliteException("values.Length!=fieldCount");
        }

        if (string.IsNullOrEmpty(values[0])) {
            values[0] = "''";
        }
        string queryString = "INSERT OR IGNORE INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            if (string.IsNullOrEmpty(values[i])) {
                values[i] = "''";
            }
            queryString += ", " + values[i];
        }
        queryString += " )";
        Debug.LogFormat("InsertValues  queryString=" + queryString);
        return ExecuteQuery(queryString);
    }
    /// <summary>
    /// 向指定数据表中插入字符数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertStringInto(string tableName, string[] values) {

        string queryString = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";// yq@@@@问题查错                                                                           // string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++) {
            //   Debug.Log("++++++++++++++++c#捕捉到的插入数据为 type：" + values[i].GetType() + "  value[" + i + "]=" + values[i]);

            queryString += ", " + "'" + values[i] + "'";
        }
        queryString += " )";
        Debug.Log("++++++++++++++++C#捕捉到的插入数据为：" + queryString);
        return ExecuteQuery(queryString);

    }
   
    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value) {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length) {
            throw new Exception("colNames.Length!=colValues.Length");
        }

        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for (int i = 1; i < colValues.Length; i++) {
            queryString += ", " + colNames[i] + "=" + colValues[i];
        }
        queryString += " WHERE " + key + operation + value;
        return ExecuteQuery(queryString);
    }
    //test
    public SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue) {

        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];

        for (int i = 1; i < colsvalues.Length; ++i) {

            query += ", " + cols[i] + " =" + colsvalues[i];
        }

        query += " WHERE " + selectkey + " = " + selectvalue + " ";

        return ExecuteQuery(query);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues) {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length) {
            throw new Exception("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }
        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++) {
            queryString += " OR " + colNames[i] + operations[0] + colValues[i];//修改SQL 语法
        }
        Debug.LogFormat("DeleteValuesOR  queryString=" + queryString);
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues) {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length) {
            throw new Exception("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++) {
            queryString += " AND " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    


    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.查找的表</param>
    /// <param name="items">返回数据的字段.</param>
    /// <param name="colNames">Col names.查找对比的字段</param>
    /// <param name="operations">Operations.操作符</param>
    /// <param name="colValues">Col values.查找的对比值</param>
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues) {
        //if (!isTableLegal(tableName)){
        //    return null;
        //}
        if (colNames.Length != operations.Length || operations.Length != colValues.Length) {
            throw new SqliteException("colNames.Length != operation.Length != colValues.Length");
            return null;
        }
        string queryString = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++) {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++) {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }


    //操作表是否合法
    private bool isTableLegal(string tableName, string[] values = null) {
        SqliteDataReader reader = ReadFullTable(tableName);
        if (reader == null) {
            Debug.LogError("获取表失败!! tableName=" + tableName);
            return false;
        }

        if (values != null && values.Length > 0) {
            int fieldCount = reader.FieldCount;
            //  当插入的数据长度不等于字段数目时引发异常
            if (values.Length != fieldCount) {
                throw new SqliteException("values.Length!=fieldCount tableName=" + tableName);
            }
        }
        return true;
    }
    public bool isHaveTable(string tableName) {
        if (string.IsNullOrEmpty(tableName)) {
            return false;
        }
        string sqlStr = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '" + tableName+"'";
        try {
            SqliteDataReader obj = ExecuteQuery(sqlStr);
          //  Debug.LogFormat("tableName==" + tableName);
           // Debug.LogFormat(" obj.[0]==" + obj[0]);
            if ((System.Int64)obj[0] == 0) {
              //  Debug.LogFormat("false obj==" + obj);
                //table - Student does not exist.
                return false;
            }
            else {
                //table - Student does exist.
               // Debug.LogFormat("true obj.[0]==" + obj[0]);
                return true;
            }
        }
        catch(Exception e){
          
            Debug.LogFormat(" e==" + e);
            return false;
        }
      
       
    }
}

  