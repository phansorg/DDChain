using System.Text;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Segments;

public class ReplayDataV1Formatter<TTypeResolver> : Formatter<TTypeResolver, ReplayDataV1>
    where TTypeResolver : ITypeResolver, new()
{
    public override int? GetLength()
    {
        return null;
    }

    public override int Serialize(ref byte[] bytes, int offset, ReplayDataV1 value)
    {
        int startOffset = offset;

        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Version);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Id);
        offset += Formatter<TTypeResolver, long>.Default.Serialize(ref bytes, offset, value.PlayDateTime);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.ScoreKindValue);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Seed);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.FrameCount);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.InputCount);
        offset += Formatter<TTypeResolver, int[]>.Default.Serialize(ref bytes, offset, value.InputFrame);
        offset += Formatter<TTypeResolver, byte[]>.Default.Serialize(ref bytes, offset, value.InputType);
        offset += Formatter<TTypeResolver, byte[]>.Default.Serialize(ref bytes, offset, value.InputData1);

        offset += Formatter<TTypeResolver, byte[]>.Default.Serialize(ref bytes, offset, value.InputData2);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Row);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Col);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Color);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Link);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Direction);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Time);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Stop);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.CountDisp);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Garbage);

        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Name);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.ScoreCategoryValue);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved22);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved23);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved24);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved25);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved26);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved27);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved28);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved29);

        return offset - startOffset;
    }

    public override ReplayDataV1 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
    {
        Encoding enc = Encoding.GetEncoding("UTF-8");

        ReplayDataV1 value = new ReplayDataV1();

        value.Version = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Id = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Id);
        value.PlayDateTime = Formatter<TTypeResolver, long>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 8;
        value.ScoreKindValue = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Seed = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.FrameCount = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.InputCount = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.InputFrame = Formatter<TTypeResolver, int[]>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        if (value.InputFrame != null)
        {
            offset += value.InputFrame.Length * 4;
        }
        value.InputType = Formatter<TTypeResolver, byte[]>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        if (value.InputType != null)
        {
            offset += value.InputType.Length;
        }
        value.InputData1 = Formatter<TTypeResolver, byte[]>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        if (value.InputData1 != null)
        {
            offset += value.InputData1.Length;
        }

        value.InputData2 = Formatter<TTypeResolver, byte[]>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        if (value.InputData2 != null)
        {
            offset += value.InputData2.Length;
        }
        value.Row = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Col = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Color = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Link = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Direction = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Time = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Stop = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.CountDisp = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Garbage = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;

        value.Name = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Name);
        value.ScoreCategoryValue = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved22 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved23 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved24 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved25 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved26 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved27 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved28 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved29 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;

        return value;
    }
}
