using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTest
{
    public class CXTest : Singleton<CXTest>
    {
        TestClass allClass = new TestClass();

        public void init()
        {
            NoticeManager.Instance.AddNoticeLister("SHOPLIST", onDelFun);
            NoticeManager.Instance.AddNoticeLister("CLUBLIST", onDelFun);
            NoticeManager.Instance.AddNoticeLister("EXITCLUB", onDelFun);
            NoticeManager.Instance.AddNoticeLister("FINDCLUB", onDelFun);
            NoticeManager.Instance.AddNoticeLister("JOINCLUB", onDelFun);
            NoticeManager.Instance.AddNoticeLister("ENTERCLUB", onDelFun);
            NoticeManager.Instance.AddNoticeLister("EnterGame", onDelFun);
        }

        public void Send(string noticeType, BaseNotice notice = null)
        {
            Debug.LogWarning(" CXTest send type:" + noticeType);

            if (noticeType.Equals("Rep_ClubList"))
            {
                #region "Rep_ClubList"
                //allClass = new TestClass();
                //ClubInfo info1 = new ClubInfo("1001", "AAA Club", "1111111111111", 100, 100);
                //ClubInfo info2 = new ClubInfo("1002", "BBB Club", "2222222222222", 200, 100);
                //ClubInfo info3 = new ClubInfo("1003", "CCC Club", "3333333333333", 300, 100);
                //ClubInfo info4 = new ClubInfo("1004", "DDD Club", "4444444444444", 400, 100);
                //allClass.clublist.Add(info1);
                //allClass.clublist.Add(info2);
                //allClass.clublist.Add(info3);
                //allClass.clublist.Add(info4);

                //NoticeManager.Instance.Dispatch(noticeType, allClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_ExitClub"))
            {
                #region "Rep_ExitClub"
                string clubId = (notice as ObjectNotice).GetObj().ToString();
                for (int i = 0; i < allClass.Count; i++)
                {
                    if (allClass[i].id.Equals(clubId))
                    {
                        allClass.clublist.Remove(allClass.clublist[i]);
                        break;
                    }
                }
                NoticeManager.Instance.Dispatch(noticeType, allClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_FindClub"))
            {
                #region "Rep_FindClub"
                TestClass infoClass = new TestClass();
                string clubId = (notice as ObjectNotice).GetObj().ToString();
                ClubInfo info = new ClubInfo(clubId, "X Club", "123456789", 999, 100);
                infoClass.clublist.Add(info);
                infoClass.result = "SUCCESS";
                NoticeManager.Instance.Dispatch(noticeType, infoClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_JoinClub"))
            {
                #region "Rep_JoinClub"
                string clubId = (notice as ObjectNotice).GetObj().ToString();
                for (int i = 0; i < allClass.clublist.Count; i++)
                {
                    if (allClass.clublist[i].id == clubId) return;
                }

                ClubInfo info = new ClubInfo(clubId, "X Club", "123456789", 999, 100);
                allClass.clublist = addAndSortInfos(info, allClass.clublist);
                NoticeManager.Instance.Dispatch(noticeType, allClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_EnterClub"))
            {
                #region "Rep_EnterClub"
                TestClass infoClass = new TestClass();
                string clubId = (notice as ObjectNotice).GetObj().ToString();

                for (int i = 0; i < allClass.Count; i++)
                {
                    if (allClass[i].id.Equals(clubId))
                    {
                        infoClass.clublist.Add(allClass[i]);
                        NoticeManager.Instance.Dispatch(noticeType, infoClass);
                        return;
                    }
                }
                infoClass.result = "ERROR";
                NoticeManager.Instance.Dispatch(noticeType, infoClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_EnterGame"))
            {
                #region "Rep_EnterGame"
                TestClass infoClass = new TestClass();
                infoClass.initRoomList(10);
                NoticeManager.Instance.Dispatch(noticeType, infoClass);
                #endregion
            }
            else if (noticeType.Equals("Rep_ShopList"))
            {
                #region "Rep_ShopList"
                ShopInfos shopInfos = new ShopInfos();
                shopInfos.init();
                NoticeManager.Instance.Dispatch("ShopList", shopInfos);
                #endregion
            }   
        }

        void onDelFun(string noticeType, BaseNotice notice)
        {
            Debug.LogWarning(" CXTest receive type:" + noticeType);
            switch (noticeType)
            {
                case "SHOPLIST":
                    Send("Rep_ShopList", notice);
                    break;
                case "CLUBLIST":
                    Send("Rep_ClubList", notice);
                    break;
                case "FINDCLUB":
                    Send("Rep_FindClub", notice);
                    break;
                case "EXITCLUB":
                    Send("Rep_ExitClub", notice);
                    break;
                case "JOINCLUB":
                    Send("Rep_JoinClub", notice);
                    break;
                case "ENTERCLUB":
                    Send("Rep_EnterClub", notice);
                    break;
                case "EnterGame":
                    Send("Rep_EnterGame", notice);
                    break;
                default:
                    break;
            }
        }

        #region 快捷键
        public void Execute()
        {
            //if (Input.GetKeyDown("u"))
            //{
            //    Debug.LogError("按下了u");
            //    MahjongTipsInfo mjTipsInfo = new MahjongTipsInfo();
            //    mjTipsInfo.addMahjongGangInfo("emMJ_1Tiao");
            //    //mjTipsInfo.addMahjongGangInfo("emMJ_2Wan");
            //    //mjTipsInfo.addMahjongGangInfo("emMJ_7Wan");
            //    NoticeManager.Instance.Dispatch("GangTis", mjTipsInfo);
            //}
            //if (Input.GetKeyDown("l"))
            //{
            //    Debug.LogError("按下了l");
            //    MahjongTipsInfo eatTipsInfo = new MahjongTipsInfo();
            //    for (int i = 0; i < 1; i++)
            //    {
            //        eatTipsInfo.addMahjongEatInfo("emMJ_1Wan", "emMJ_2Wan", "emMJ_3Wan");
            //        eatTipsInfo.addMahjongEatInfo("emMJ_7Wan", "emMJ_8Wan", "emMJ_9Wan");
            //    }
            //    NoticeManager.Instance.Dispatch("EatTis", eatTipsInfo);
            //}

            //if (Input.GetKeyDown("o"))
            //{
            //    Debug.LogError("按下了o");
            //    ShopInfos shopInfos = new ShopInfos();
            //    shopInfos.init();
            //    NoticeManager.Instance.Dispatch("ShopList", shopInfos);
            //}
            //if (Input.GetKeyDown("y"))
            //{
            //    RecordInfos recordInfos = new RecordInfos();
            //    recordInfos.init("GST_DDZ_Classic", 15);
            //    NoticeManager.Instance.Dispatch("RecordList", recordInfos);
            //}
        }
        #endregion

        List<ClubInfo> addAndSortInfos(ClubInfo info, List<ClubInfo> allInfos)
        {
            if (allInfos == null) return null;

            List<ClubInfo> testlist = new List<ClubInfo>();
            testlist.Add(info);

            for (int i = 0; i < allInfos.Count; i++)
            {
                testlist.Add(allInfos[i]);
            }
            return testlist;
        }

    }


    #region ClubInfo
    public class TestClass
    {
        public bool testSign = true;
        public string result = "SUCCESS";

        public List<ClubInfo> clublist = new List<ClubInfo>();
        public List<RoomInfo> roomlist = new List<RoomInfo>();
        
        public int Count
        {
            get { return clublist.Count; }
        }

        public int RoomCount
        {
            get { return roomlist.Count; }
        }

        public ClubInfo this[int index]
        {
            get {
                if (index >= clublist.Count) return null;
                return clublist[index];
            }
            set { clublist[index] = value; }
        }

        public void initRoomList(int num)
        {
            for (int i=0; i<num; i++)
            {
                RoomInfo info = new RoomInfo("1100"+i,"XXXXXX",500,3000,3);
                roomlist.Add(info);
            }
        }
    }

    public class ClubInfo
    {
        public string id = "";
        public string name = "";
        public string recommend = "";
        public int score = 0;
        public int scoreExchageRate = 1;

        public ClubInfo(string id, string name, string recommend, int score, int rate)
        {
            this.id = id;
            this.name = name;
            this.recommend = recommend;
            this.score = score;
            this.scoreExchageRate = rate;
        }
    }

    public class RoomInfo
    {
        public string id = "";
        public string recommend = "";
        public int points = 0;
        public int players = 0;
        public int basescore = 0;

        public RoomInfo(string id, string recommend, int score, int basescore,int playerConut)
        {
            this.id = id;
            this.recommend = recommend;
            this.points = score;
            this.basescore = basescore;
            this.players = playerConut;
        }
    }

    #endregion

    #region MahjongTipsInfo
    public class MahjongTipsInfo
    {
        public bool testSign = true;
        public class MahjongHuInfo
        {
            public string cardType = "";
            public int exponent = 0;
            public int cardNum = 0;
        }

        public class MahjongEatInfo
        {
            public string card1 = "";
            public string card2 = "";
            public string card3 = "";
        }

        public List<MahjongHuInfo> cardInfolist = new List<MahjongHuInfo>();
        public List<MahjongEatInfo> eatInfolist = new List<MahjongEatInfo>();
        public List<string> gangInfolist = new List<string>();

        public int cardCount
        {
            get { return cardInfolist.Count; }
        }

        public int infoCount
        {
            get { return eatInfolist.Count; }
        }

        public int gangCount
        {
            get { return gangInfolist.Count; }
        }

        public void addMahjongGangInfo(string cardType)
        {
            if (!gangInfolist.Contains(cardType))
            {
                gangInfolist.Add(cardType);
            }
        }

        public void addMahjongHuInfo(string cardType, int exponent, int cardNum)
        {
            MahjongHuInfo info = new MahjongHuInfo();
            info.cardType = cardType;
            info.exponent = exponent;
            info.cardNum = cardNum;
            cardInfolist.Add(info);
        }

        public void addMahjongEatInfo(string card1, string card2, string card3)
        {
            MahjongEatInfo info = new MahjongEatInfo();
            info.card1 = card1;
            info.card2 = card2;
            info.card3 = card3;
            eatInfolist.Add(info);

        }
        public void clearInfolist()
        {
            cardInfolist.Clear();
        }

    }

    #endregion

}