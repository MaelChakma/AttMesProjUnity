using Autohand;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckConstruction : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wantedParts;

    private Dictionary<GameObject, int> heldParts = new Dictionary<GameObject, int>();
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        foreach (GameObject part in wantedParts)
        {
            heldParts.Add(part, 0);
        }

    }

    public void AddPart(PlacePoint placePoint)
    {
        foreach (GameObject key in heldParts.Keys)
        {
            if (placePoint.placedObject.gameObject == key)
            {
                heldParts[key] = 1;
                break;
            }
        }
    }

    public void RemovePart(PlacePoint placePoint)
    {
        foreach (GameObject key in heldParts.Keys)
        {
            if (placePoint.placedObject.gameObject == key)
            {
                heldParts[key] = 0;
                break;
            }
        }
    }

    public void CheckParts()
    {
        bool isValidated = true;
        foreach (int i in heldParts.Values)
        {
            if(i == 0) { isValidated = false; break; }
        }
        if (isValidated) { Celebrate(); }
    }

    private void Celebrate()
    {
        particles.Play();
    }
}
