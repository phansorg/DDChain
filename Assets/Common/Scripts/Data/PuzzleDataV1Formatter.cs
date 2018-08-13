using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Segments;

public class PuzzleDataV1Formatter<TTypeResolver> : Formatter<TTypeResolver, PuzzleDataV1>
    where TTypeResolver : ITypeResolver, new()
{
    public override int? GetLength()
    {
        return null;
    }

    public override int Serialize(ref byte[] bytes, int offset, PuzzleDataV1 value)
    {
        int startOffset = offset;

        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Row);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Col);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Color);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Link);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Direction);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Time);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Stop);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.CountDisp);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Garbage);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.WriteCount);

        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Practice);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved11);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved12);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved13);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved14);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved15);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved16);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved17);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved18);
        offset += Formatter<TTypeResolver, int>.Default.Serialize(ref bytes, offset, value.Reserved19);
        
        return offset - startOffset;
    }

    public override PuzzleDataV1 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
    {
        PuzzleDataV1 value = new PuzzleDataV1();

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
        value.WriteCount = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;

        value.Practice = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved11 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved12 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved13 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved14 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved15 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved16 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved17 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved18 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;
        value.Reserved19 = Formatter<TTypeResolver, int>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4;

        return value;
    }
}
