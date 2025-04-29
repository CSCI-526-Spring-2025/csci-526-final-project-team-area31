using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI FtoggleText;
    public Transform[] respawnPoints;

    [System.Serializable]
    public class BatteryDoorEvent
    {
        public string sessionId;
        public string timestamp;
        public string eventType; // "battery" or "door"
        public int currentBatteryCount;
        public int doorRequirement;
        public int levelIndex;
    }

    // public GameObject playerPrefab;
    public Transform playerInstance; 
    private Transform currentRespawnPoint; 
    public static int respawnTargetLevelIndex = -1;
    public int currentLevelIndex = 0;
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        if (playerInstance == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player1");
            if (playerObject != null)
            {
                playerInstance = playerObject.transform;
            }
            else
            {
                Debug.LogError("GameManager: Player not found in scene and not assigned!");
            }
        }

        if (respawnPoints != null && respawnPoints.Length > 0)
        {
            currentRespawnPoint = respawnPoints[0];
        }
        else
        {
            Debug.LogError("GameManager: Respawn Points array is not assigned or empty!");
        }
    }

    void Start() // Runs AFTER Awake() on the new GameManager instance
    {
        // --- Check if a specific respawn was requested by the static variable ---
        if (respawnTargetLevelIndex != -1 && respawnPoints != null && respawnTargetLevelIndex >= 0 && respawnTargetLevelIndex < respawnPoints.Length)
        {
            Debug.Log($"Post-reload teleport requested to Level Index: {respawnTargetLevelIndex}");
            Transform targetPoint = respawnPoints[respawnTargetLevelIndex];
            TeleportPlayer(targetPoint); // Teleport to the requested point
            currentLevelIndex = respawnTargetLevelIndex; // Update the internal index of this NEW instance
        }
        else
        {
            // --- Normal Start: Place player at default location (usually level 0) ---
             Debug.Log("Normal scene start or invalid respawn index. Placing player at default.");
             if (respawnPoints != null && respawnPoints.Length > 0 && playerInstance != null) {
                 TeleportPlayer(respawnPoints[0]); // Default to first respawn point
                 currentLevelIndex = 0;
             } else if (playerInstance == null) {
                  Debug.LogError("GameManager (Start): Player instance is null, cannot teleport.");
             } else {
                 Debug.LogWarning("GameManager (Start): Respawn points not set up, player starts at scene position.");
             }
        }

        // --- CRITICAL: Reset the static variable ---
        // So it doesn't affect future normal scene loads
        respawnTargetLevelIndex = -1;
    }
    

    // Call this method when the player enters a new level's trigger zone
    public void SetCurrentRespawnPoint(int levelIndex)
    {
        if (respawnPoints != null && levelIndex >= 0 && levelIndex < respawnPoints.Length)
        {
            this.currentLevelIndex = levelIndex;
            
            Debug.Log("Respawn point updated to Level " + (this.currentLevelIndex));
        }
        else
        {
            Debug.LogWarning($"GameManager: Invalid level index ({levelIndex}) or respawn points not set up correctly.");
        }
    }



    public int GetCurrentLevelIndex()
    {
        return this.currentLevelIndex;
    }

    public void RespawnPlayer()
    {
         if (playerInstance == null) { Debug.LogError("Respawn failed: Player is null"); return; }
         if (respawnPoints == null || currentLevelIndex < 0 || currentLevelIndex >= respawnPoints.Length) {
             Debug.LogError($"Respawn failed: Invalid index ({currentLevelIndex}) or respawnPoints array issue.");
             // Optional fallback: respawn at point 0
             if (respawnPoints != null && respawnPoints.Length > 0) TeleportPlayer(respawnPoints[0]);
             return;
         }

        Transform targetPoint = respawnPoints[currentLevelIndex];
        Debug.Log($"Respawning player at current Level Index {currentLevelIndex}: {targetPoint.name}");
        TeleportPlayer(targetPoint);
        
    }

    private void TeleportPlayer(Transform targetPoint)
    {
        if (playerInstance == null || targetPoint == null) return;

        // Handle CharacterController teleportation safely
        CharacterController cc = playerInstance.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false; // Disable CC temporarily
            playerInstance.position = targetPoint.position;
            playerInstance.rotation = targetPoint.rotation;
            cc.enabled = true; // Re-enable CC
        }
        // Handle Rigidbody teleportation safely
        else
        {
            Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            playerInstance.position = targetPoint.position;
            playerInstance.rotation = targetPoint.rotation;
        }
        Debug.Log($"Player teleported to {targetPoint.position}");
    }

    public void LogDoorOpened(int currentBatteryCount, int requiredBatteries)
    {
        StartCoroutine(SendBatteryDoorEvent("door", currentBatteryCount, requiredBatteries));
    }

    public void LogBatteryPickup(int currentBatteryCount)
    {
        StartCoroutine(SendBatteryDoorEvent("battery", currentBatteryCount));
    }

    public IEnumerator SendBatteryDoorEvent(string eventType, int currentCount, int requiredForDoor = 0)
    {
        BatteryDoorEvent evt = new BatteryDoorEvent
        {
            sessionId = PlayerPrefs.GetString("SessionID", "unknown"),
            timestamp = System.DateTime.UtcNow.ToString("o"),
            eventType = eventType,
            currentBatteryCount = currentCount,
            doorRequirement = eventType == "door" ? requiredForDoor : 0,
            levelIndex = this.currentLevelIndex  // Add level tracking
        };

        string json = JsonUtility.ToJson(evt);
        Debug.Log("Sending Battery/Door Event: " + json);

        using (UnityWebRequest request = new UnityWebRequest("https://script.google.com/macros/s/AKfycbxRImDqK3B5Ijqo0aK-wnyPI1o5f2uYKnknk0DmxwUcEk4WRUsJ4-Dv4qQkKwgdzloYcw/exec", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "*/*");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Battery/Door event failed: " + request.error);
            }
            else
            {
                Debug.Log("Battery/Door event logged: " + request.downloadHandler.text);
            }
        }
    }
}