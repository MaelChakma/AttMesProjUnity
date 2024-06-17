using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class EyeTrackingUnityVector3 : IEyeTracking
{

    public UnityEngine.Vector3 content;

    public EyeTrackingUnityVector3() { content = Vector3.down; }
    public EyeTrackingUnityVector3(UnityEngine.Vector3 v) { content = v; }
    public EyeTrackingUnityVector3(EyeTrackingVector3 other) { content.x = other.content.X; content.y = other.content.Y; content.z = other.content.Z; }

    public new Type GetType() { return typeof(EyeTrackingUnityVector3); }
    public void Write(BinaryWriter writer) { writer.Write(content.x); writer.Write(content.y); writer.Write(content.z); }
    public IEyeTracking Read(BinaryReader reader) { return new EyeTrackingUnityVector3(Vector3.down); }
    public EyeTrackingVector3 ToEyeTrackingVector3() { return new EyeTrackingVector3(content.x, content.y, content.z); }
}

public class PsiExporterEyeTracking : PsiExporter<Dictionary<string, IEyeTracking>>
{

    [Space, SerializeField]
    private GameObject PlayerHead;
    [SerializeField]
    private GameObject LeftEye;
    [SerializeField]
    private GameObject RightEye;
    [SerializeField]
    private bool ActivateDebug = false;
    [SerializeField]
    private GameObject EyeTrackingVisualization;
    [SerializeField]
    private LayerMask LayersToHit;

    private Dictionary<string, IEyeTracking> PreviousEyeTracking = new Dictionary<string, IEyeTracking>();

    override public void Start()
    {
        base.Start();

        if(PlayerHead == null) { PlayerHead = gameObject; }

        EyeTrackingTemplate template = new EyeTrackingTemplate();
        foreach(var kvp in template.content)
        {
            if(kvp.Value.GetType() == typeof(EyeTrackingVector3)){ PreviousEyeTracking.Add(kvp.Key, new EyeTrackingUnityVector3()); }
            else { PreviousEyeTracking.Add(kvp.Key, kvp.Value); }
        }

        if (EyeTrackingVisualization != null) {EyeTrackingVisualization.SetActive(ActivateDebug);}

    }

    void Update()
    {
        //Creating dictionary and recovering data
        Dictionary<string, IEyeTracking> eyeTracking = new Dictionary<string, IEyeTracking>();
        
        var playerId = 0;
        var leftEyePosition = LeftEye.transform.position;
        var leftEyeRotation = LeftEye.transform.eulerAngles;
        var leftGaze = LeftEye.transform.rotation*Vector3.forward;
        var rightEyePosition = RightEye.transform.position;
        var rightEyeRotation = RightEye.transform.eulerAngles;
        var rightGaze = RightEye.transform.rotation*Vector3.forward;
        var headDirection = PlayerHead.transform.forward;
        var centerEyePosition = (leftEyePosition + rightEyePosition) / 2;
        var averageGaze = (leftGaze + rightGaze).normalized;
        RaycastHit hit;
        bool isGazingAtSomething = Physics.Raycast(centerEyePosition, averageGaze, out hit, 100f, LayersToHit);
        Vector3 firstIntersectionPoint = Vector3.zero;
        string gazedObjectName = "null";
        bool hasEyeTrackingTags = false;
        List<string> eyeTrackingTagsList = new List<string>();

        if (isGazingAtSomething)
        {
            firstIntersectionPoint = hit.point;
            if (ActivateDebug) { Debug.DrawRay(centerEyePosition, averageGaze * 10, Color.magenta); DebugDisplay(firstIntersectionPoint);}
            gazedObjectName = hit.transform.name;
            hasEyeTrackingTags = hit.transform.TryGetComponent<EyeTrackingTags>(out EyeTrackingTags t);
            if (hasEyeTrackingTags) {eyeTrackingTagsList = t.AllTagsListStrings(); }

        }
        
        if (ActivateDebug) { Debug.DrawRay(centerEyePosition, averageGaze * 10, Color.magenta); }
        
        //Adding data to dictionary
        eyeTracking.Add("playerId", new EyeTrackingInt(playerId));
        eyeTracking.Add("leftEyePosition", new EyeTrackingVector3(leftEyePosition.x, leftEyePosition.y, leftEyePosition.z));
        eyeTracking.Add("leftEyeRotation", new EyeTrackingVector3(leftEyeRotation.x, leftEyeRotation.y, leftEyeRotation.z));
        eyeTracking.Add("leftGaze", new EyeTrackingVector3(leftGaze.x, leftGaze.y, leftGaze.z));
        eyeTracking.Add("rightEyePosition", new EyeTrackingVector3(rightEyePosition.x, rightEyePosition.y, rightEyePosition.z));
        eyeTracking.Add("rightEyeRotation", new EyeTrackingVector3(rightEyeRotation.x, rightEyeRotation.y, rightEyeRotation.z));
        eyeTracking.Add("rightGaze", new EyeTrackingVector3(rightGaze.x, rightGaze.y, rightGaze.z));
        eyeTracking.Add("headDirection", new EyeTrackingVector3(headDirection.x, headDirection.y, headDirection.z));
        eyeTracking.Add("centerEyePosition", new EyeTrackingVector3(centerEyePosition.x, centerEyePosition.y, centerEyePosition.z));
        eyeTracking.Add("averageGaze", new EyeTrackingVector3(averageGaze.x, averageGaze.y, averageGaze.z));
        eyeTracking.Add("isGazingAtSomething", new EyeTrackingBool(isGazingAtSomething));
        eyeTracking.Add("firstIntersectionPoint", new EyeTrackingVector3(firstIntersectionPoint.x, firstIntersectionPoint.y, firstIntersectionPoint.z));
        eyeTracking.Add("gazedObjectName", new EyeTrackingString(gazedObjectName));
        eyeTracking.Add("hasEyeTrackingTags", new EyeTrackingBool(hasEyeTrackingTags));
        eyeTracking.Add("eyeTrackingTagsList", new EyeTrackingStringList(eyeTrackingTagsList));

        //Send data if different
        if (CanSend() && !IsSameData(eyeTracking))
        {
            Out.Post(eyeTracking, GetCurrentTime());
            PsiManager.AddLog("EyeTracking log sent at " + Time.time);
            UpdatePreviousData(eyeTracking);
        }
        
    }

    private void DebugDisplay(Vector3 position)
    {
        if (EyeTrackingVisualization != null) { EyeTrackingVisualization.transform.position = position; }
    }

    private bool IsSameData(Dictionary<string,IEyeTracking> eyeTracking)
    {
        bool isSameData = true;
        foreach (var kvp in PreviousEyeTracking)
        {
            if(kvp.Value.GetType() == typeof(EyeTrackingInt) && eyeTracking[kvp.Key].GetType() == typeof(EyeTrackingInt))
            {
                isSameData = ((EyeTrackingInt)kvp.Value).Compare((EyeTrackingInt)eyeTracking[kvp.Key]);
            } 
            else if (kvp.Value.GetType() == typeof(EyeTrackingBool) && eyeTracking[kvp.Key].GetType() == typeof(EyeTrackingBool))
            {
                isSameData = ((EyeTrackingBool)kvp.Value).Compare((EyeTrackingBool)eyeTracking[kvp.Key]);
            }
            else if (kvp.Value.GetType() == typeof(EyeTrackingString) && eyeTracking[kvp.Key].GetType() == typeof(EyeTrackingString))
            {
                isSameData = ((EyeTrackingString)kvp.Value).Compare((EyeTrackingString)eyeTracking[kvp.Key]);
            }
            else if (kvp.Value.GetType() == typeof(EyeTrackingStringList) && eyeTracking[kvp.Key].GetType() == typeof(EyeTrackingStringList))
            {
                isSameData = ((EyeTrackingStringList)kvp.Value).Compare((EyeTrackingStringList)eyeTracking[kvp.Key]);
            }
            else if (kvp.Value.GetType() == typeof(EyeTrackingUnityVector3) && eyeTracking[kvp.Key].GetType() == typeof(EyeTrackingVector3))
            {
                isSameData = (((EyeTrackingUnityVector3)kvp.Value).ToEyeTrackingVector3()).Compare((EyeTrackingVector3)eyeTracking[kvp.Key]);
            }
            if (!isSameData) { break; }
        }
        return isSameData;
    }

    private void UpdatePreviousData(Dictionary<string, IEyeTracking> eyeTracking)
    {
        foreach (var key in eyeTracking.Keys)
        {
            if(PreviousEyeTracking[key].GetType() == typeof(EyeTrackingUnityVector3))
            {
                PreviousEyeTracking[key] = new EyeTrackingUnityVector3((EyeTrackingVector3)eyeTracking[key]);
            } else
            {
                PreviousEyeTracking[key] = eyeTracking[key];
            }
        }
    }


#if PLATFORM_ANDROID
    protected override Microsoft.Psi.Interop.Serialization.IFormatSerializer<Dictionary<string, IEyeTracking>> GetSerializer()
    {
        return PsiFormatEyeTracking.GetFormat();
    }
#endif
}

