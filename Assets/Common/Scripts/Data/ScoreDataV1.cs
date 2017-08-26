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
}

