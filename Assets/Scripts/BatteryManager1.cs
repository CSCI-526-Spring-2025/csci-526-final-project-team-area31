using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryManager1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int batteryCount;
    public TextMeshProUGUI batteryText;
    public float rechargeAmount = 80f;
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

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Battery"))
    //     {
    //         //Destroy(other.gameObject);
    //         Debug.Log("user hit battery");
    //         batteryCount++;
    //         FlashlightBattery flashlight = other.GetComponentInChildren<FlashlightBattery>();
    //         if (flashlight != null)
    //         {
    //             Debug.Log("FlashlightBattery script found! Recharging...");
    //             flashlight.RechargeBattery(rechargeAmount);
    //             Debug.Log("Battery recharged by " + rechargeAmount + ". Current battery: " + flashlight.batteryLife);
    //             //Destroy(gameObject);  // Remove the battery item after pickup
    //         }
    //         else
    //         {
    //             Debug.LogError("FlashlightBattery script NOT found on player!");
    //         }
    //     }
    // }
    public void recharge_battery(){
         FlashlightBattery flashlight = GameObject.FindObjectOfType<FlashlightBattery>();
        if (flashlight != null)
        {
            Debug.Log("FlashlightBattery script found! Recharging...");
            flashlight.RechargeBattery(rechargeAmount);
            Debug.Log("Battery recharged by " + rechargeAmount + ". Current battery: " + flashlight.batteryLife);
            //Destroy(gameObject);  // Remove the battery item after pickup
        }
        else
        {
            Debug.LogError("FlashlightBattery script NOT found on player!");
        }
    }
}
