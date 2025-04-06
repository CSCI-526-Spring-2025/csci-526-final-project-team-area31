using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Adjust speed in Inspector
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public Health health;
    public BatteryManager1 bm;
    private Vector3 initialPosition;
    public TextMeshProUGUI notificationText;
    private bool isPromptedToReturn = false;
    private float notificationTimer = 0f;  // Timer to control the duration of the notification
    public Image damageOverlay;
    public float flashDuration = 0.2f;
    private bool isDead = false;

    // Google Apps Script Web App URL
    [HideInInspector]
    private string googleSheetURL = "https://script.google.com/macros/s/AKfycbwStmbKKdcFkriLE5gYMyARkJnaEmc9lFzvozkFeyWSwu-XrEB-VmykF5940WzY3zmdnA/exec";

    [System.Serializable]
    public class PlayerEncounterEvent
    {
        public string sessionId;
        public string timestamp;
        public string obstacleType;
        public float health;
        public Vector3 location;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        notificationText.text = "";
    }

    void Update()
    {
        if (health.health <= 0)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        // Show the notification to return when the player has 3 batteries
        //if (bm.batteryCount >= 3 && !isPromptedToReturn)
        //{
        //    notificationText.text = "Return to the starting point to win!";
        //    notificationTimer = 5f;  // Start the timer for 5 seconds
        //    isPromptedToReturn = true;  // Set flag to true so we don't repeat the message
        //}

        // Countdown for the notification
        if (notificationTimer > 0f)
        {
            notificationTimer -= Time.deltaTime;
        }
        else
        {
            notificationText.text = "";
        }

        // Check if the player is near the starting point and has collected 3 batteries
        if (Vector3.Distance(transform.position, initialPosition) < 1f && bm.batteryCount >= 3)
        {
            FinishGame();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        // Apply movement using Rigidbody2D to obey physics
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movementInput * moveSpeed, Time.fixedDeltaTime * 10);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.gameObject.CompareTag("Battery"))
        {
            Destroy(other.gameObject);
            bm.batteryCount++;
            bm.recharge_battery();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.LogBatteryPickup(bm.batteryCount);
            }
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            health.health -= 30;
            StartCoroutine(FlashRed());
            StartCoroutine(SendEncounterEvent("trap"));
            CheckPlayerDeath();
        }
        else if (other.gameObject.CompareTag("Health"))
        {
            if (health.health < 100)
            {
                Destroy(other.gameObject);
                health.health = Mathf.Min(100, health.health + 50);
                StartCoroutine(FlashGreen());
            }
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            health.health -= 30;
            StartCoroutine(FlashRed());
            StartCoroutine(SendEncounterEvent("monster"));
            CheckPlayerDeath();
        }
    }

    void FinishGame()
    {
        // Show a message, stop player movement, or load a new scene
        notificationText.text = "You collected all the batteries! Loading next level.....";
        StartCoroutine(WaitAndExecute());

       // SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("3 seconds"); 
        SceneManager.LoadScene("MazeScene_Level2");
    }

    IEnumerator FlashRed()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(flashDuration);
            damageOverlay.color = new Color(1, 0, 0, 0);
        }
    }

    IEnumerator FlashGreen()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(0, 1, 0, 0.5f);
            yield return new WaitForSeconds(flashDuration);
            damageOverlay.color = new Color(0, 1, 0, 0);
        }
    }

    void CheckPlayerDeath()
    {
        if (health.health <= 0)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator SendEncounterEvent(string type)
    {
        PlayerEncounterEvent eventData = new PlayerEncounterEvent()
        {
            sessionId = PlayerPrefs.GetString("SessionID", "unknown"),
            timestamp = System.DateTime.UtcNow.ToString("o"),
            obstacleType = type,
            health = health.health,
            location = transform.position
        };

        string jsonData = JsonUtility.ToJson(eventData);
        Debug.Log("Sending Encounter JSON: " + jsonData);

        using (UnityWebRequest request = new UnityWebRequest(googleSheetURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "*/*");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Unity error: {request.error}, Response: {request.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Encounter logged successfully: " + request.downloadHandler.text);
            }
        }
    }
}
