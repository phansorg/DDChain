﻿using System.Text;
using ZeroFormatter;
using ZeroFormatter.Formatters;
using ZeroFormatter.Segments;

public class UserDataV1Formatter<TTypeResolver> : Formatter<TTypeResolver, UserDataV1>
    where TTypeResolver : ITypeResolver, new()
{
    public override int? GetLength()
    {
        return null;
    }

    public override int Serialize(ref byte[] bytes, int offset, UserDataV1 value)
    {
        int startOffset = offset;

        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Id);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Password);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Name);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.PracticeSave);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Scene);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Reserved05);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Reserved06);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Reserved07);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Reserved08);
        offset += Formatter<TTypeResolver, string>.Default.Serialize(ref bytes, offset, value.Reserved09);
        
        return offset - startOffset;
    }

    public override UserDataV1 Deserialize(ref byte[] bytes, int offset, DirtyTracker tracker, out int byteSize)
    {
        Encoding enc = Encoding.GetEncoding("UTF-8");

        UserDataV1 value = new UserDataV1();

        value.Id = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Id);
        value.Password = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Password);
        value.Name = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Name);
        value.PracticeSave = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.PracticeSave);
        value.Scene = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Scene);
        value.Reserved05 = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Reserved05);
        value.Reserved06 = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Reserved06);
        value.Reserved07 = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Reserved07);
        value.Reserved08 = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Reserved08);
        value.Reserved09 = Formatter<TTypeResolver, string>.Default.Deserialize(ref bytes, offset, tracker, out byteSize);
        offset += 4 + enc.GetByteCount(value.Reserved09);

        return value;
    }
}
