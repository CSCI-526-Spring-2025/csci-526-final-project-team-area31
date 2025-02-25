using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Adjust speed in Inspector
    private Rigidbody2D rb;
    private Vector2 movementInput;
    public BatteryManager1 bm;
    private Vector3 initialPosition;
    public TextMeshProUGUI notificationText;
    private bool isPromptedToReturn = false;
    private float notificationTimer = 0f;  // Timer to control the duration of the notification

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        notificationText.text = "";
    }

    void Update()
    {
        // Get movement input from Arrow Keys or WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveY).normalized;

        // Show the notification to return when the player has 3 batteries
        if (bm.batteryCount >= 3 && !isPromptedToReturn)
        {
            notificationText.text = "Return to the starting point to win!";
            notificationTimer = 5f;  // Start the timer for 5 seconds
            isPromptedToReturn = true;  // Set flag to true so we don't repeat the message
        }

        // Countdown for the notification
        if (notificationTimer > 0f)
        {
            notificationTimer -= Time.deltaTime;  // Decrease timer each frame
        }
        else
        {
            notificationText.text = "";  // Clear the notification after 5 seconds
        }

        // Check if the player is near the starting point and has collected 3 batteries
        if (Vector3.Distance(transform.position, initialPosition) < 1f && bm.batteryCount >= 3)
        {
            FinishGame();
        }
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody2D to obey physics
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, movementInput * moveSpeed, Time.fixedDeltaTime * 10);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            Destroy(other.gameObject);
            bm.batteryCount++;
        }
    }

    void FinishGame()
    {
        // Show a message, stop player movement, or load a new scene
        notificationText.text = "You collected all the batteries! You win!";

        // Optionally, load a different scene (e.g., a "Game Over" or "You Win" scene)
        // SceneManager.LoadScene("WinScene");  // Replace with your win scene name
    }
}
