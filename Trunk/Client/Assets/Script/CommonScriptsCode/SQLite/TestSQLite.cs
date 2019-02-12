using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class TestSQLite : MonoBehaviour
{

    // Use this for initialization

    private SQLiteHelper sql;

    public void OnGUI() {

        if (GUI.Button(new Rect(20f, 0, 150f, 100f), "Create")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestCreate();
        }
        if (GUI.Button(new Rect(20, 150, 150f, 100f), "Delect")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestDelet();
        }
        if (GUI.Button(new Rect(20, 350, 150f, 100f), "search")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestRead();
        }
        if (GUI.Button(new Rect(220, 150, 150f, 100f), "InserAndUpdate")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestInsertAndUpdate();
        }
        if (GUI.Button(new Rect(220, 0, 150f, 100f), "InserAndIgore")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestInsertAndIgore();
        }
        if (GUI.Button(new Rect(220, 350, 150f, 100f), "Update")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestUpdate();
        }
        if (GUI.Button(new Rect(20, 555, 150f, 100f), "clearTable")) {
            //  Debug.LogFormat("update  TestSQLite()");
            TestClear();
        }
    }

//NULL 空值
//  INTEGER 整数
//REAL 浮点数
//TEXT 字符串
//BLOB 大数据
    private void updateSql() {
        TestDelet();

        //创建数据库名称为xuanyusong.db
        //  DbAccess db = new DbAccess("data source=xuanyusong.db");
        //请注意 插入字符串是 已经要加上'宣雨松' 不然会报错
        //  db.InsertInto("momo", new string[] { "'宣雨松'", "'289187120'", "'xuanyusong@gmail.com'", "'www.xuanyusong.com'" });
        // db.CloseSqlConnection();


        //创建名为sqlite4unity的数据库
        // sql = new SQLiteHelper("data source=sqlite4unity.db");

        //创建名为table1的数据表
        // sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });

        //插入两条数据
        // sql.InsertValues("table1", new string[] { "'1'", "'张三'", "'22'", "'Zhang3@163.com'" });
        // sql.InsertValues("table1", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });
        // SQLiteManager.Instance.InsertInto("table2", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });
        //  var myTable = SQLiteManager.Instance.ReadTable("table2", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
        // for (int i = 0; i < myTable.Count; i++)
        //  {
        //    Debug.Log(myTable[i]);
        //  }
    }
    private void TestDelet() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
     //   SQLiteManager.Instance.CreateTable("table3", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "INTEGER", "TEXT", "INTEGER", "TEXT" });
        SQLiteManager.Instance.DeleteValuesOR("table3", new string[] { "ID","ID" }, new string[] {" = "," = " },  new string[] { "101101010","101101010"});
        SQLiteManager.Instance.CloseConnection();
    }
    private void TestRead() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        SQLiteManager.Instance.ReadTable("table2", new string[] { "ID", "Age","Name" }, new string[] { "Age" }, new string[] { " = " }, new string[] { "19" });
        //using (SqliteDataReader sqReader = db.SelectWhere("momo", new string[] { "name", "email" }, new string[] { "qq" }, new string[] { "=" }, new string[] { "289187120" })) {
        //  SQLiteManager.Instance.CloseConnection();

    }

    private void TestUpdate() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        //更新数据，将Name="张三"的记录中的Name改为"Zhang3"
        //  SQLiteManager.Instance.UpdateData("table2", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'张三'");
        //更新数据，将Name="张三"的记录中的Name改为"Zhang3",ID改为010
        // SQLiteManager.Instance.UpdateData("table2", new string[] { "Name", "ID" }, new string[] { "'Zhang3'" ,"010"}, "Name", "=", "'张三'");
        //更新数据，将id="111111"的记录中的id改为99999,money改为1000000000
        SQLiteManager.Instance.UpdateData("table2", new string[] { "id", "money" }, new string[] { "99999","1000000000"}, "id", "=", "111111");

    }
    private void TestCreate() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        // SQLiteManager.Instance.CreateTable("RedPacketLBSView_1231231312", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "REAL", "REAL" });
       
        //有主键的 table1表
        SQLiteManager.Instance.CreateTable("table1", new string[] { "id", "money", "lng", "lat", "discribe", "imgs" }, new string[] { "INTEGER PRIMARY KEY", "INTEGER", "REAL", "REAL", "TEXT", "TEXT" });
        //无主键的 table2表
        SQLiteManager.Instance.CreateTable("table2", new string[] { "id", "money", "lng", "lat", "discribe", "imgs" }, new string[] { "INTEGER", "INTEGER", "REAL", "REAL", "TEXT", "TEXT" });

    }
    private void TestDeleteTable() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        // SQLiteManager.Instance.CreateTable("RedPacketLBSView_1231231312", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "REAL", "REAL" });

        //删除 table1表
          SQLiteManager.Instance.DeleteTable("table1");
        //删除 table2表
          // SQLiteManager.Instance.DeleteTable("table2");

    }
    private void TestClear() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        // SQLiteManager.Instance.CreateTable("RedPacketLBSView_1231231312", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "REAL", "REAL" });

        //删除 table1表内容
           SQLiteManager.Instance.DeleteFullTable("table1");
        //删除 table2表内容
          SQLiteManager.Instance.DeleteFullTable("table2");

    }
    private void TestInsertAndUpdate() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        SQLiteManager.Instance.InsertOrUpdateValues("table1",new string[] { "111111", "1111", "10.11","100.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrUpdateValues("table1", new string[] { "22222", "22222", "10.11", "100.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrUpdateValues("table1", new string[] { "22222", "33333", "3333.11", "3333.11", "'table1'", "'//www.hao123.com//@#$@@!'" });

        //错误信息
        SQLiteManager.Instance.InsertOrUpdateValues("table1", new string[] { "22222", "44444", "44440.11", "@@4444400.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
      //  SQLiteManager.Instance.InsertOrUpdateValues("table1", new string[] { "22222", "55555", "5555.11", "5555.11", "'table1'", "'//www.hao123.com//@#$@@!'" });


        SQLiteManager.Instance.InsertOrUpdateValues("table2", new string[] { "111111", "1111", "10.11", "100.11", "'啊哈哈哈哈'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrUpdateValues("table2", new string[] { "111111", "222222", "122222", "222222", "'啊哈哈哈哈2222'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrUpdateValues("table2", new string[] { "333333", "222222", "122222", "222222", "'啊哈哈哈哈2222'", "'//www.hao123.com//@#$@@!'" });
        


        SQLiteManager.Instance.CloseConnection();
    }
    private void TestInsertAndIgore() {
        SQLiteManager.Instance.CreateOrOpenDb("test");
        SQLiteManager.Instance.InsertOrIgnoreValues("table1", new string[] { "111111", "1111", "10.11", "100.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrIgnoreValues("table1", new string[] { "22222", "22222", "10.11", "100.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrIgnoreValues("table1", new string[] { "22222", "33333", "3333.11", "3333.11", "'table1'", "'//www.hao123.com//@#$@@!'" });
        //错误信息
        SQLiteManager.Instance.InsertOrIgnoreValues("table1", new string[] { "22222", "44444", "44440.11", "@@4444400.11", "'table1'", "'//www.hao123.com//@#$@@!'" });

        SQLiteManager.Instance.InsertOrIgnoreValues("table2", new string[] { "111111", "1111", "10.11", "100.11", "'啊哈哈哈哈'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrIgnoreValues("table2", new string[] { "111111", "222222", "122222", "222222", "'啊哈哈哈哈2222'", "'//www.hao123.com//@#$@@!'" });
        SQLiteManager.Instance.InsertOrIgnoreValues("table2", new string[] { "333333", "222222", "122222", "222222", "'啊哈哈哈哈2222'", "'//www.hao123.com//@#$@@!'" });



        SQLiteManager.Instance.CloseConnection();
    }

    void Start() {

        //SQLiteManager.Instance.CreateOrOpenDb("test");

        //SQLiteManager.Instance.CreateTable("table2", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" });

        //SQLiteManager.Instance.InsertInto("table2", new string[] { "'1'", "'张三'", "'22'", "'Zhang3@163.com'" });
        //SQLiteManager.Instance.InsertInto("table2", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });


        //  var myTable = SQLiteManager.Instance.ReadTable("table2", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });

        //  SQLiteManager.Instance.CloseConnection();

        // for (int i = 0; i < myTable.Count; i++)
        //  {
        //    Debug.Log(myTable[i]);
        //  }

        //Debug.Log(myTable.Count);

        //sql = new SQLiteHelper("data source= D://SQLite//sqlite4unity.db");


        ////创建名为table1的数据表
        //sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, new string[] { "TEXT", "TEXT", "TEXT", "TEXT" });

        //插入两条数据
        //sql.InsertValues("table1", new string[] { "'1'", "'张三'", "'22'", "'Zhang3@163.com'" });
        //sql.InsertValues("table1", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });


        //读取整张表
        //SqliteDataReader reader = sql.ReadFullTable("table1");


        //while (reader.Read())
        //{
        //    Debug.Log("db是否为空2:" + reader.IsDBNull(0));
        //}

        //sql.ReadTable("table1", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });

        //while (reader.Read())
        //{
        //    for (int i = 0; i < reader.FieldCount; i++)
        //    {
        //        Debug.Log(reader.GetString(i));
        //    }
        //}

        //sql.CloseConnection();
    }
}
