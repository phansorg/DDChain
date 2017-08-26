using NCMB;
using System.Collections.Generic;
using ZeroFormatter;

[ZeroFormattable]
public class ScoreDataV1
{
    public enum ScoreKind
    {
        AllColor = 0,
        SingleColor = 1,
    }
    [IgnoreFormat]
    public const int SCORE_KIND_MAX = 2;

    [IgnoreFormat]
    public const string SCORE_DATA_FILE_NAME = "ScoreData.bin";

    [Index(0)]
    public virtual long PlayDateTime { get; set; }

    [Index(1)]
    public virtual int Score { get; set; }

    [Index(2)]
    public virtual int ScoreKindValue { get; set; }

    [Index(3)]
    public virtual string Id { get; set; }

    [Index(4)]
    public virtual string Name { get; set; }

    [Index(5)]
    public virtual int Row { get; set; }

    [Index(6)]
    public virtual int Col { get; set; }

    [Index(7)]
    public virtual int Color { get; set; }

    [Index(8)]
    public virtual int Link { get; set; }

    [Index(9)]
    public virtual int Direction { get; set; }

    [Index(10)]
    public virtual int Time { get; set; }

    [Index(11)]
    public virtual int Stop { get; set; }

    [Index(12)]
    public virtual int CountDisp { get; set; }

    [Index(13)]
    public virtual int Garbage { get; set; }

    [Index(14)]
    public virtual int Reserved14 { get; set; }

    [Index(15)]
    public virtual int Reserved15 { get; set; }

    [Index(16)]
    public virtual int Reserved16 { get; set; }

    [Index(17)]
    public virtual int Reserved17 { get; set; }

    [Index(18)]
    public virtual int Reserved18 { get; set; }

    [Index(19)]
    public virtual int Reserved19 { get; set; }

    public ScoreDataV1()
    {
        PlayDateTime = 0;
        Score = 0;
        ScoreKindValue = 0;
        Id = string.Empty;
        Name = string.Empty;
        Row = 8;
        Col = 6;
        Color = 5;
        Link = 4;
        Direction = 3;

        Time = 60;
        Stop = 0;
        CountDisp = 0;
        Garbage = 0;
        Reserved14 = 0;
        Reserved15 = 0;
        Reserved16 = 0;
        Reserved17 = 0;
        Reserved18 = 0;
        Reserved19 = 0;
    }

    // サーバーにハイスコアを保存 -------------------------
    public void save()
    {
        // データストアの「HighScore」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreDataV1");
        query.WhereEqualTo("ScoreKindValue", ScoreKindValue);
        query.WhereEqualTo("Id", Id);
        query.WhereEqualTo("Row", Row);
        query.WhereEqualTo("Col", Col);
        query.WhereEqualTo("Color", Color);
        query.WhereEqualTo("Link", Link);
        query.WhereEqualTo("Direction", Direction);
        query.WhereEqualTo("Time", Time);
        query.WhereEqualTo("Stop", Stop);
        query.WhereEqualTo("CountDisp", CountDisp);
        if (Garbage == 0)
        {
            query.WhereNotEqualTo("Garbage", 1);
        }
        else
        {
            query.WhereEqualTo("Garbage", Garbage);
        }

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら    
            if (e == null)
            {
                objList[0]["Name"] = Name;
                objList[0]["PlayDateTime"] = PlayDateTime;
                objList[0]["Score"] = Score;
                objList[0].SaveAsync();
            }
        });
    }

    // サーバーからハイスコアを取得  -----------------
    public void fetch()
    {
        // データストアの「HighScore」クラスから、Nameをキーにして検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("ScoreDataV1");
        query.WhereEqualTo("ScoreKindValue", ScoreKindValue);
        query.WhereEqualTo("Id", Id);
        query.WhereEqualTo("Row", Row);
        query.WhereEqualTo("Col", Col);
        query.WhereEqualTo("Color", Color);
        query.WhereEqualTo("Link", Link);
        query.WhereEqualTo("Direction", Direction);
        query.WhereEqualTo("Time", Time);
        query.WhereEqualTo("Stop", Stop);
        query.WhereEqualTo("CountDisp", CountDisp);
        if (Garbage == 0)
        {
            query.WhereNotEqualTo("Garbage", 1);
        }
        else
        {
            query.WhereEqualTo("Garbage", Garbage);
        }
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {

            //検索成功したら  
            if (e == null)
            {
                // ハイスコアが未登録だったら
                if (objList.Count == 0)
                {
                    NCMBObject obj = new NCMBObject("ScoreDataV1");
                    obj["ScoreKindValue"] = ScoreKindValue;
                    obj["Id"] = Id;
                    obj["Row"] = Row;
                    obj["Col"] = Col;
                    obj["Color"] = Color;
                    obj["Link"] = Link;
                    obj["Direction"] = Direction;
                    obj["Time"] = Time;
                    obj["Stop"] = Stop;
                    obj["CountDisp"] = CountDisp;
                    obj["Garbage"] = Garbage;

                    obj["Name"] = Name;
                    obj["PlayDateTime"] = 0;
                    obj["Score"] = 0;
                    obj.SaveAsync();
                    Score = 0;
                }
                // ハイスコアが登録済みだったら
                else
                {
                    Score = System.Convert.ToInt32(objList[0]["Score"]);
                }
            }
        });
    }
}

