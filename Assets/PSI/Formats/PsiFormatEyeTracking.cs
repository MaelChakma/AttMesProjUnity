using Microsoft.Psi.Interop.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class PsiFormatEyeTracking
{
    public static Format<Dictionary<string, IEyeTracking>> GetFormat()
    {
        return new Format<Dictionary<string, IEyeTracking>>(WriteEyeTracking, ReadEyeTracking);
    }

    public static void WriteEyeTracking(Dictionary<string, IEyeTracking> eyeTracking, BinaryWriter writer)
    {
        writer.Write(eyeTracking.Count);
        foreach(var item in eyeTracking)
        {
            writer.Write(item.Key);
            item.Value.Write(writer);
        }
    }

    public static Dictionary<string, IEyeTracking> ReadEyeTracking(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        Dictionary<string, IEyeTracking> dictionary = new Dictionary<string, IEyeTracking>(count);
        EyeTrackingTemplate template = new EyeTrackingTemplate();
        foreach (var item in template.content)
        {
            dictionary.Add(reader.ReadString(), item.Value.Read(reader));
        }
        return dictionary;
    }
}
