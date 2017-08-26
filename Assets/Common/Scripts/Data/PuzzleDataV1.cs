using ZeroFormatter;

[ZeroFormattable]
public class PuzzleDataV1
{
    [IgnoreFormat]
    public const string PUZZLE_DATA_FILE_NAME = "PuzzleData.bin";

    [Index(0)]
    public virtual int Row { get; set; }

    [Index(1)]
    public virtual int Col { get; set; }

    [Index(2)]
    public virtual int Color { get; set; }
    [IgnoreFormat]
    public const int COLOR_MAX = 5;

    [Index(3)]
    public virtual int Link { get; set; }

    [Index(4)]
    public virtual int Direction { get; set; }

    [Index(5)]
    public virtual int Time { get; set; }

    [Index(6)]
    public virtual int Stop { get; set; }

    [Index(7)]
    public virtual int CountDisp { get; set; }

    [Index(8)]
    public virtual int Garbage { get; set; }

    [Index(9)]
    public virtual int WriteCount { get; set; }

    [Index(10)]
    public virtual int Reserved10 { get; set; }

    [Index(11)]
    public virtual int Reserved11 { get; set; }

    [Index(12)]
    public virtual int Reserved12 { get; set; }

    [Index(13)]
    public virtual int Reserved13 { get; set; }

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

    public PuzzleDataV1()
    {
        Row = 8;
        Col = 6;
        Color = 5;
        Link = 4;
        Direction = 3;
        Time = 60;
        Stop = 0;
        CountDisp = 0;
        Garbage = 0;
        WriteCount = 0;

        Reserved10 = 0;
        Reserved11 = 0;
        Reserved12 = 0;
        Reserved13 = 0;
        Reserved14 = 0;
        Reserved15 = 0;
        Reserved16 = 0;
        Reserved17 = 0;
        Reserved18 = 0;
        Reserved19 = 0;
    }
}

