using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEyesData : MonoBehaviour
{
    [SerializeField]
    public MessageDisplay display;
    [SerializeField]
    public Transform leftEyeTransform;
    [SerializeField]
    public Transform rightEyeTransform;



    // Update is called once per frame
    void Update()
    {
        display.Display("IPD = " + Vector3.Distance(leftEyeTransform.position,rightEyeTransform.position));
    }
}
