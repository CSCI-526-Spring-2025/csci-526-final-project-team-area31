using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class FlashlightBattery : MonoBehaviour
{
    public Transform player;
    public Light2D flashlight;
    public float maxBattery = 100f;
    public float batteryLife;
    public float drainRate = 0.05f;
    public float offRechargeRate = 0.3f;
    public float minIntensity = 0.1f;
    public float maxIntensity = 0.5f;
    private bool isInBatteryRoom = true;
    private bool isFlashlightOn = true;

    // BatteryBar UI elements
    public Image BatteryBar;
    public float Width, Height;
    [SerializeField] private RectTransform battery_;

    // Google Apps Script Web App URL
    [HideInInspector]
    private string googleSheetURL = "https://script.google.com/macros/s/AKfycbwSEnDVbSTMi10rC2Wj1ssnSqT9UrJSnVzvZkbYfATrzro4HmYwfPZxLcE5o93NPMQd/exec";
    
    [System.Serializable]
    public class FlashlightEvent
    {
        public string sessionId;
        public string timestamp;
        public bool flashlightOn;
        public Vector3 location;
    }


    void Start()
    {
        batteryLife = maxBattery;
    }

    void Update()
    {
        // Toggle Flashlight with "F" Key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
            StartCoroutine(SendFlashlightToggleEvent(isFlashlightOn));
        }

        if (isFlashlightOn && batteryLife > 0)
        {
            batteryLife -= drainRate * Time.deltaTime;
            flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
            UpdateBatteryUI();
        }
        else if (!isFlashlightOn )
        {
            batteryLife = Mathf.Clamp(batteryLife + offRechargeRate, 0, maxBattery);
            flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
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
            flashlight.intensity = 0.3f;
            flashlight.pointLightOuterRadius = 2f;
            flashlight.pointLightInnerAngle = 80f;
            flashlight.pointLightOuterAngle = 120f;
        }
        else
        {
            flashlight.pointLightOuterRadius = 24.11164f;
            flashlight.pointLightInnerAngle = 80f;
            flashlight.pointLightOuterAngle = 120f;
        }

    }

    // Function to recharge the battery
    public void RechargeBattery()
    {
        if (batteryLife <= 0){
            batteryLife = 0;
        }
        batteryLife = Mathf.Clamp(batteryLife + 80f, 0, maxBattery);
        flashlight.intensity = Mathf.Lerp(minIntensity, maxIntensity, batteryLife / maxBattery);
        flashlight.enabled = true;
        flashlight.pointLightOuterRadius = 4.0f;
        UpdateBatteryUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Current in the tutorial Room");
        if (other.CompareTag("Tutorial"))
        {
            Debug.Log("Currently in the tutorial Room");
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

    IEnumerator SendFlashlightToggleEvent(bool isFlashlightOn)
    {
        FlashlightEvent eventData = new FlashlightEvent()
        {
            timestamp = System.DateTime.UtcNow.ToString("o"),
            flashlightOn = isFlashlightOn,
            sessionId = PlayerPrefs.GetString("SessionID", "unknown"),
            location = player != null ? player.position : Vector3.zero
        };

        string jsonData = JsonUtility.ToJson(eventData);
        Debug.Log("Sending JSON: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(googleSheetURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();

            // These headers MUST match curl exactly
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "*/*"); // Exactly as curl

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Unity error: {request.error}, Response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }

}
