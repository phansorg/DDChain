using System.Collections.Generic;
using ZeroFormatter;

[ZeroFormattable]
public class ReplayDataV1
{
    [IgnoreFormat]
    public const string REPLAY_DATA_FILE_NAME = "ReplayData.bin";

    [Index(0)]
    public virtual int Version { get; set; }

    [Index(1)]
    public virtual string Id { get; set; }

    [Index(2)]
    public virtual long PlayDateTime { get; set; }

    [Index(3)]
    public virtual int ScoreKindValue { get; set; }

    [Index(4)]
    public virtual int Seed { get; set; }

    [Index(5)]
    public virtual int FrameCount { get; set; }

    [Index(6)]
    public virtual int InputCount { get; set; }

    [Index(7)]
    public virtual int[] InputFrame { get; set; }

    [Index(8)]
    public virtual byte[] InputType { get; set; }

    [Index(9)]
    public virtual byte[] InputData1 { get; set; }

    [Index(10)]
    public virtual byte[] InputData2 { get; set; }

    [Index(11)]
    public virtual int Row { get; set; }

    [Index(12)]
    public virtual int Col { get; set; }

    [Index(13)]
    public virtual int Color { get; set; }

    [Index(14)]
    public virtual int Link { get; set; }

    [Index(15)]
    public virtual int Direction { get; set; }

    [Index(16)]
    public virtual int Time { get; set; }

    [Index(17)]
    public virtual int Stop { get; set; }

    [Index(18)]
    public virtual int CountDisp { get; set; }

    [Index(19)]
    public virtual int Garbage { get; set; }

    [Index(20)]
    public virtual string Name { get; set; }

    [Index(21)]
    public virtual int ScoreCategoryValue { get; set; }

    [Index(22)]
    public virtual int Reserved22 { get; set; }

    [Index(23)]
    public virtual int Reserved23 { get; set; }

    [Index(24)]
    public virtual int Reserved24 { get; set; }

    [Index(25)]
    public virtual int Reserved25 { get; set; }

    [Index(26)]
    public virtual int Reserved26 { get; set; }

    [Index(27)]
    public virtual int Reserved27 { get; set; }

    [Index(28)]
    public virtual int Reserved28 { get; set; }

    [Index(29)]
    public virtual int Reserved29 { get; set; }

    public ReplayDataV1()
    {
        Version = 0;
        Id = string.Empty;
        PlayDateTime = 0;
        ScoreKindValue = 0;
        Seed = 0;
        FrameCount = 0;
        InputCount = 0;
        InputFrame = null;
        InputType = null;
        InputData1 = null;

        InputData2 = null;
        Row = 8;
        Col = 6;
        Color = 5;
        Link = 4;
        Direction = 3;
        Time = 60;
        Stop = 0;
        CountDisp = 0;
        Garbage = 0;

        Name = string.Empty;
        ScoreCategoryValue = 0;
        Reserved22 = 0;
        Reserved23 = 0;
        Reserved24 = 0;
        Reserved25 = 0;
        Reserved26 = 0;
        Reserved27 = 0;
        Reserved28 = 0;
        Reserved29 = 0;
    }
}

