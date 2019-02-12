using System.Collections.Generic;


public class ResVersions
{
    public class ComparisonRecord
    {
        public string resPath;
        public ResVersions.ResRecord resRecord;
        public ResVersions.ResRecord serverRecord;
        public float progress;

    }

    public class ResRecord
    {
        public string md5;
        public long size;
    }

    public class ResPack
    {
        public string zipPath;
        public string unZipPath;
        public long size;
        public bool loadedSign;
        public string md5;
        public Dictionary<string, ResRecord> resRecords;
    }

    public int versionCode;


    //key 是包名 例如 base包 麻将包
    public Dictionary<string, List<ResPack>> packageRecords = new Dictionary<string, List<ResPack>>();




}

