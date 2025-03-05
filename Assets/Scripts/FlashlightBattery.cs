//using UnityEngine;

//public class FlashlightBattery
//{

//}
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightBattery : MonoBehaviour
{
    public Light2D flashlight;  // Reference to the 2D Light
    public float maxBattery = 100f;  // Maximum battery life
    public float batteryLife;  // Current battery life
    public float drainRate = 0.5f;  // Battery drain per second
    public float minIntensity = 0.1f;  // Minimum light intensity
    public float maxIntensity = 0.5f;  // Max brightness
    private bool isInBatteryRoom = false;
    private bool isFlashlightOn = true;

    void Start()
    {
        batteryLife = maxBattery;  // Start with full battery
    }

    void Update()
    {
        // Toggle Flashlight with "F" Key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
        }

        // Drain Battery if Flashlight is ON
        if (isFlashlightOn && batteryLife > 0 && !isInBatteryRoom)
        {
            batteryLife -= drainRate * Time.deltaTime;
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
            flashlight.intensity = intensity;
        }
        else if (batteryLife <= 0)
        {
            flashlight.intensity = 0.3f;
            flashlight.pointLightOuterRadius =2f;
            //flashlight.enabled = false;  // Auto turn off when battery is empty
        }
    }

    // Function to recharge the battery
    public void RechargeBattery(float amount)
    {
        batteryLife = Mathf.Clamp(batteryLife + amount, 0, maxBattery);
        flashlight.enabled = true; // Turn on again if recharged
        flashlight.pointLightOuterRadius = 4.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
          Debug.Log("Current in the tutorial Room");
        if (other.CompareTag("Tutorial")) 
        {
            Debug.Log("Current in the tutorial Room");
            isInBatteryRoom = true;
            flashlight.intensity = 1.0f;
            batteryLife = maxBattery; // Reset to full battery instantly
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tutorial"))
        {
            isInBatteryRoom = false;
        }
    }

}
