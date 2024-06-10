using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageDisplay : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI displayText;

    public void Display(string text)
    {
        displayText.text = text;
    }
}
