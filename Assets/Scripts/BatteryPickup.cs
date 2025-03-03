using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float rechargeAmount = 30f; // Amount of battery restored

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Battery pickup triggered by: " + other.gameObject.name);

        FlashlightBattery flashlight = other.GetComponentInChildren<FlashlightBattery>();
        if (flashlight != null)
        {
            Debug.Log("FlashlightBattery script found! Recharging...");
            flashlight.RechargeBattery(rechargeAmount);
            Debug.Log("Battery recharged by " + rechargeAmount + ". Current battery: " + flashlight.batteryLife);
            Destroy(gameObject);  // Remove the battery item after pickup
        }
        else
        {
            Debug.LogError("FlashlightBattery script NOT found on player!");
        }
    }
}
