using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GetEyeData : MonoBehaviour
{
    private Quaternion eyeTrackRotQuat;
    private Vector3 eyeTrackRotEuler;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        eyeTrackRotQuat = transform.rotation;
        eyeTrackRotEuler = eyeTrackRotQuat.eulerAngles;
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,transform.position + eyeTrackRotQuat*Vector3.forward);

    }
}
