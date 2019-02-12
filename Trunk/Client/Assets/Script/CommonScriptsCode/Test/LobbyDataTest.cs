using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTest
{
    public class LobbyDataTest : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

    }

    public class ShopInfos
    {
        public bool testSign = true;
        public List<ShopItemInfo> itemsList = new List<ShopItemInfo>();

        public int Count
        {
            get { return itemsList.Count; }
        }

        public void init()
        {
            ShopItemInfo info1 = new ShopItemInfo("101",100,99);
            itemsList.Add(info1);
            ShopItemInfo info2 = new ShopItemInfo("102", 250, 20);
            itemsList.Add(info2);
            ShopItemInfo info3 = new ShopItemInfo("103", 399, 10);
            itemsList.Add(info3);
            ShopItemInfo info4 = new ShopItemInfo("104", 599, 5);
            itemsList.Add(info4);
            ShopItemInfo info5 = new ShopItemInfo("105", 999, 1);
            itemsList.Add(info5);
        }

    }

    public class ShopItemInfo
    {
        public string itemId = "";
        public int price = 0;
        public int num = 0;

        public ShopItemInfo(string uId, int price, int num)
        {
            this.itemId = uId;
            this.price = price;
            this.num = num;
        }
    }

    public class ItemsInfo
    {
        public string itemId = "";
        public int num = 0;

        public ItemsInfo(string uId, int num)
        {
            this.itemId = uId;
            this.num = num;
        }
    }

    public class RecordInfos
    {
        public bool testSign = true;
        public string sub_type = "";
        public List<RecordItemInfo> recordInfos = new List<RecordItemInfo>();

        public int Count{
            get { return recordInfos.Count; }
        }

        public void init(string type,int count)
        {
            this.sub_type = type;
            for (int i = 0; i < count; i++)
            {
                RecordItemInfo recordItem = new RecordItemInfo();
                recordItem.init((i+10000).ToString(), DateTime.Now.ToString());
                recordInfos.Add(recordItem);
            }
        }
    }

    public class RecordItemInfo
    {
        public string roomId = "";
        public string time = "";

        public class ScoreInfo
        {
            public string _uId;
            public string _name;
            public int _score;

            public ScoreInfo(string id, string name, int score)
            {
                _uId = id;
                _name = name;
                _score = score;
            }
        }

        public List<ScoreInfo> ScoreList = new List<ScoreInfo>();

        public int Count
        {
            get { return ScoreList.Count; }
        }

        public void init(string roomId, string time)
        {
            this.roomId = roomId;
            this.time = time;
            ScoreInfo info1 = new ScoreInfo("11001","小明",500);
            ScoreInfo info2 = new ScoreInfo("11002","小华",500);
            ScoreInfo info3 = new ScoreInfo("11003","小李",100);
            ScoreInfo info4 = new ScoreInfo("11004","小陈",1200);
            ScoreList.Add(info1);
            ScoreList.Add(info2);
            ScoreList.Add(info3);
            ScoreList.Add(info4);
        }
    }
}