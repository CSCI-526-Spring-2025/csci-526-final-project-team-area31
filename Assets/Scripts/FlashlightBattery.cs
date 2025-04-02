//using UnityEngine;

//public class FlashlightBattery
//{

//}
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Light2D flashlight;  // Reference to the 2D Light
    public float maxBattery = 100f;  // Maximum battery life
    public float batteryLife;  // Current battery life
    public float drainRate = 0.1f;  // Battery drain per second
    public float offRechargeRate = 0.3f;
    public float minIntensity = 0.1f;  // Minimum light intensity
    public float maxIntensity = 0.5f;  // Max brightness
    private bool isInBatteryRoom = true;
    private bool isFlashlightOn = true;

    //BatteryBar
    public Image BatteryBar;
    public float Width, Height;

    [SerializeField]
    private RectTransform battery_;

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
        if (isFlashlightOn && batteryLife > 0)
        {
            batteryLife -= drainRate * Time.deltaTime;
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
            flashlight.intensity = intensity;
            UpdateBatteryUI();
        }

        else if (!isFlashlightOn ){
            batteryLife = Mathf.Clamp(batteryLife + offRechargeRate , 0, maxBattery);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
            flashlight.intensity = intensity;
            UpdateBatteryUI();
        }

        //if (batteryLife <= 0)
        //{
        //    flashlight.intensity = 0.3f;
        //    flashlight.pointLightOuterRadius =2f;
        //    //flashlight.enabled = false;  // Auto turn off when battery is empty
        //}
        //else{
            
        //    flashlight.pointLightOuterRadius =120f;
        //}

        if (batteryLife <= 0)
        {
            flashlight.intensity = 0.3f;  // Dimmed intensity
            flashlight.pointLightOuterRadius = 2f; // Small radius when battery is empty
            flashlight.pointLightInnerAngle = 80f;
            flashlight.pointLightOuterAngle = 120f;
        }
        else
        {
            //flashlight.intensity = 0.5f; // Regular intensity
            flashlight.pointLightOuterRadius = 24.11164f; // Default radius
            flashlight.pointLightInnerAngle = 80f;
            flashlight.pointLightOuterAngle = 120f;
        }

    }

    // Function to recharge the battery
    public void RechargeBattery(float amount)
    {
        if (batteryLife <= 0){
            batteryLife = 0;
        }
        batteryLife = Mathf.Clamp(batteryLife + amount, 0, maxBattery);
        flashlight.enabled = true; // Turn on again if recharged
        flashlight.pointLightOuterRadius = 4.0f;
        UpdateBatteryUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Current in the tutorial Room");
        if (other.CompareTag("Tutorial")) 
        {
            Debug.Log("Current in the tutorial Room");
            isInBatteryRoom = true;
            //flashlight.intensity = 1.0f;
            //batteryLife = maxBattery; // Reset to full battery instantly
            UpdateBatteryUI();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tutorial"))
        {
            isInBatteryRoom = false;
        }
    }
    void UpdateBatteryUI()
    {
        float newWidth = (batteryLife / maxBattery) * Width;

        battery_.sizeDelta = new Vector2(newWidth, Height);
    }

}
