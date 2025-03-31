using System;
using TMPro;
using UnityEngine;

public class BatteryDisplay : MonoBehaviour
{
    private TextMeshPro batteryText;

    private void Awake()
    {
        batteryText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        BatteryManager.instance.BatteryChanged += SetText;
    }
    
    private void SetText()
    {
        batteryText.text = (int)BatteryManager.instance.batteryPercentage + "%";
    }
}
