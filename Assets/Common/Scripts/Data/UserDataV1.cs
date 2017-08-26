using ZeroFormatter;

[ZeroFormattable]
public class UserDataV1
{
    [IgnoreFormat]
    public const string USER_DATA_FILE_NAME = "UserData.bin";

    [Index(0)]
    public virtual string Id { get; set; }
    [IgnoreFormat]
    public const int ID_LENGTH = 10;

    [Index(1)]
    public virtual string Password { get; set; }
    [IgnoreFormat]
    public const int PASSWORD_LENGTH = 10;

    [Index(2)]
    public virtual string Name { get; set; }
    [IgnoreFormat]
    public const int NAME_LENGTH = 10;

    [Index(3)]
    public virtual string Reserved03 { get; set; }

    [Index(4)]
    public virtual string Reserved04 { get; set; }

    [Index(5)]
    public virtual string Reserved05 { get; set; }

    [Index(6)]
    public virtual string Reserved06 { get; set; }

    [Index(7)]
    public virtual string Reserved07 { get; set; }

    [Index(8)]
    public virtual string Reserved08 { get; set; }

    [Index(9)]
    public virtual string Reserved09 { get; set; }

    public UserDataV1()
    {
        Id = string.Empty;
        Password = string.Empty;
        Name = string.Empty;
        Reserved03 = string.Empty;
        Reserved04 = string.Empty;
        Reserved05 = string.Empty;
        Reserved06 = string.Empty;
        Reserved07 = string.Empty;
        Reserved08 = string.Empty;
        Reserved09 = string.Empty;
    }

}
