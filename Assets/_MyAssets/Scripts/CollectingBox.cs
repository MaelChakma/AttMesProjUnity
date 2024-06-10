using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Box : MonoBehaviour
{
    [SerializeField]
    private LayerMask collectLayerMask;

    [SerializeField]
    private TextMeshProUGUI displayText;

    private int objCounter;

    

    private void OnTriggerEnter(Collider other)
    {
        if (collectLayerMask == (collectLayerMask | (1 << other.gameObject.layer)))
        {
            OnCollecting(other.gameObject);
        }
    }

    void OnCollecting(GameObject obj)
    {
        Destroy(obj);
        objCounter++;
        displayText.text = "" + objCounter;
    }
}
