using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PsiExporterEyeTracking : PsiExporter<Dictionary<string, System.Numerics.Vector3>>
{
    public GameObject LeftEye;
    public GameObject RightEye;

    private Dictionary<string, UnityEngine.Vector3> PreviousEyeTracking = new Dictionary<string, UnityEngine.Vector3>();

    override public void Start()
    {
        base.Start();
        PreviousEyeTracking.Add("leftEyePosition", Vector3.down);
        PreviousEyeTracking.Add("rightEyePosition", Vector3.down);
    }

    void Update()
    {
        Dictionary<string, System.Numerics.Vector3> eyeTracking = new Dictionary<string, System.Numerics.Vector3>();
        var leftEyePosition = LeftEye.transform.position;
        var rightEyePosition = RightEye.transform.position;
        eyeTracking.Add("leftEyePosition", new System.Numerics.Vector3(leftEyePosition.x, leftEyePosition.y, leftEyePosition.z));
        eyeTracking.Add("rightEyePosition", new System.Numerics.Vector3(rightEyePosition.x, rightEyePosition.y, rightEyePosition.z));

        //Check if current data is different from previous data
        bool isDataDifferent = false;
        foreach (var v in PreviousEyeTracking)
        {
            isDataDifferent = isDataDifferent || new System.Numerics.Vector3(v.Value.x, v.Value.y, v.Value.z) != eyeTracking[v.Key];
        }

        //Send data if different
        if (CanSend() && isDataDifferent)
        {
            Out.Post(eyeTracking, GetCurrentTime());
            PsiManager.AddLog("EyeTracking log sent at " + Time.time);

            //Update previous data
            foreach (var key in eyeTracking.Keys)
            {
                PreviousEyeTracking[key] = new UnityEngine.Vector3(eyeTracking[key].X, eyeTracking[key].Y, eyeTracking[key].Z);
            }
        } 

    }

#if PLATFORM_ANDROID
    protected override Microsoft.Psi.Interop.Serialization.IFormatSerializer<Dictionary<string, System.Numerics.Vector3>> GetSerializer()
    {
        return PsiFormatEyeTracking.GetFormat();
    }
#endif
}

