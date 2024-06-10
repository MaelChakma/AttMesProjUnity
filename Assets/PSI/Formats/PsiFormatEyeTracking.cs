using Microsoft.Psi.Interop.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PsiFormatEyeTracking
{
    public static Format<Dictionary<string, System.Numerics.Vector3>> GetFormat()
    {
        return new Format<Dictionary<string, System.Numerics.Vector3>>(WriteEyeTracking, ReadEyeTracking);
    }

    public static void WriteEyeTracking(Dictionary<string, System.Numerics.Vector3> eyeTracking, BinaryWriter writer)
    {
        writer.Write(eyeTracking.Count);
        foreach (var item in eyeTracking)
        {
            writer.Write(item.Key);
            writer.Write((double)item.Value.X);
            writer.Write((double)item.Value.Y);
            writer.Write((double)item.Value.Z);
        }
    }

    public static Dictionary<string, System.Numerics.Vector3> ReadEyeTracking(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        Dictionary<string, System.Numerics.Vector3> dictionary = new Dictionary<string, System.Numerics.Vector3>(count);
        for (int i = 0; i < count; i++)
        {
            var key = reader.ReadString();
            var value = new System.Numerics.Vector3((float)reader.ReadDouble(), (float)reader.ReadDouble(), (float)reader.ReadDouble());
            dictionary.Add(key, value);
        }
        return dictionary;
    }
}
