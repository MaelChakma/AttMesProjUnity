using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepawnObject : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToRespawn;
    private GameObject newObject;
    private Transform parent;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Start()
    {
        parent = objectToRespawn.transform.parent;
        spawnPosition = objectToRespawn.transform.position;
        spawnRotation = objectToRespawn.transform.rotation;
        objectToRespawn.SetActive(false);
        newObject = Instantiate(objectToRespawn, spawnPosition, spawnRotation, parent);
        newObject.SetActive(true);

    }

    public void Respawn()
    {
        Destroy(newObject);
        newObject = Instantiate(objectToRespawn, spawnPosition, spawnRotation, parent);
        StartCoroutine(ActivateObject());
    }

    IEnumerator ActivateObject()
    {
        yield return new WaitForSeconds(0.1f);
        newObject.SetActive(true);
    }
}
