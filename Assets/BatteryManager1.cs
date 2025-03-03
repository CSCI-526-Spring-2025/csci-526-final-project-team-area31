using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryManager1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int batteryCount;
    public TextMeshProUGUI batteryText;
    void Start()
    {
        batteryText.color = Color.white;
        batteryText.text = "Batteries: " + batteryCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        batteryText.text = "Batteries: " + batteryCount.ToString();
    }
}
