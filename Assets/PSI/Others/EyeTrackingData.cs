using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

interface IEyeTracking
{
    abstract Type GetType();
}

class EyeTrackingSystemVector3 : IEyeTracking
{
    public new Type GetType() { return typeof(System.Numerics.Vector3); }

    public System.Numerics.Vector3 value;
    public float x;
    public float y;
    public float z;
}

class EyeTrackingUnityVector3 : IEyeTracking
{
    public new Type GetType() { return typeof(UnityEngine.Vector3); }

    public UnityEngine.Vector3 value;
    public float x;
    public float y;
    public float z;
}


class EyeTrackingInt : IEyeTracking
{
    public new Type GetType() { return typeof(int); }

    public int value;
}

