using System;
using UnityEngine;

public class PsiExporterPositionOrientation
    : PsiExporter<Tuple<System.Numerics.Vector3, System.Numerics.Vector3>>
{
    private UnityEngine.Vector3 PreviousPosition = Vector3.down;
    private UnityEngine.Vector3 PreviousOrientation = Vector3.down;

    void Update()
    {
        var position = gameObject.transform.position;
        var orientation = gameObject.transform.eulerAngles;
        if (CanSend() && position != PreviousPosition && PreviousOrientation != orientation)
        {
            Out.Post(new Tuple<System.Numerics.Vector3, System.Numerics.Vector3>(new System.Numerics.Vector3(position.x, position.y, position.z), new System.Numerics.Vector3(orientation.x, orientation.y, orientation.z)), GetCurrentTime());
            PreviousPosition = position;
            PreviousOrientation = orientation;
            PsiManager.AddLog("PosOr log sent at " + Time.time);
        }
    }

#if PLATFORM_ANDROID
    protected override Microsoft.Psi.Interop.Serialization.IFormatSerializer<Tuple<System.Numerics.Vector3, System.Numerics.Vector3>> GetSerializer()
    {
        return PsiFormatPositionOrientation.GetFormat();
    }
#endif
}