using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimulateEyes : MonoBehaviour
{
    [SerializeField]
    private Transform leftEye;
    [SerializeField]
    private Transform rightEye;

    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float rotationSpeed = 60.0f;
    [SerializeField]
    private float iPD = 0.063f;
    [SerializeField]
    private Transform[] pointsList;

    private int pointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetEyeTransform(pointsList[pointIndex]);
    }

    private void Update()
    {
        if (pointIndex <= pointsList.Length-1)
        {
            SetEyeTransform(pointsList[pointIndex]);

            if (CheckEyeTransform(pointsList[pointIndex]))
            {
                pointIndex ++;
            }
            
            if(pointIndex == pointsList.Length)
            {
                pointIndex = 0;
            }
        }
    }

    public void SetEyeTransform(Transform targetTransform)
    {        
        leftEye.position = Vector3.MoveTowards(leftEye.position, new Vector3(targetTransform.position.x - iPD / 2, targetTransform.position.y, targetTransform.position.z), moveSpeed*Time.deltaTime);
        rightEye.position = Vector3.MoveTowards(rightEye.position, new Vector3(targetTransform.position.x + iPD / 2, targetTransform.position.y, targetTransform.position.z), moveSpeed*Time.deltaTime);
        leftEye.rotation = Quaternion.RotateTowards(leftEye.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);
        rightEye.rotation = Quaternion.RotateTowards(rightEye.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);
    }

    public bool CheckEyeTransform(Transform targetTransform)
    {
        return leftEye.position == new Vector3(targetTransform.position.x - iPD / 2, targetTransform.position.y, targetTransform.position.z) 
            && rightEye.position == new Vector3(targetTransform.position.x + iPD / 2, targetTransform.position.y, targetTransform.position.z) 
            && leftEye.rotation == targetTransform.rotation 
            && rightEye.rotation == targetTransform.rotation;
    }
}
