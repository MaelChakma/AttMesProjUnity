using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

/*
 * This scripts displays console messages on an interface in the TestScene.
 * I followed this tutorial to write this script : https://youtu.be/Pi4SHO0IEQY
 */


public class DebugLogDisplay : MonoBehaviour
{

    Dictionary<string, string> debugLogs = new Dictionary<string, string>();

    public TextMeshProUGUI display;


    private void Update()
    {
    }


    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }


    void HandleLog(string logString, string StackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitstring = logString.Split(char.Parse(":"));
            string debugKey = splitstring[0];
            string debugValue = splitstring.Length > 1 ? splitstring[1] : "";

            if(debugLogs.ContainsKey(debugKey))
            {
                debugLogs[debugKey] = debugValue;
            } else {
                debugLogs.Add(debugKey, debugValue);
            }
        }

        string displayText = "";
        foreach(KeyValuePair<string, string> log in debugLogs)
        {
            if (log.Value == "")
            {
                displayText += log.Key + "\n";
            }
            else
            {
                displayText += log.Key + log.Value + "\n";
            }
        }
        display.text = displayText;

    }
}
