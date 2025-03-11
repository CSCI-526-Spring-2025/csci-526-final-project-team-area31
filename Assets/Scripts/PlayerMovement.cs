using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
            rb.linearVelocity = Vector2.zero; // Stop the player immediately
            return;
        }
        // Get movement input from Arrow Keys or WASD
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
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            health.health -= 30;
            StartCoroutine(FlashRed());
            CheckPlayerDeath();
        }
        else if (other.gameObject.CompareTag("Health")){
            if(health.health<100){
                Destroy(other.gameObject);
                health.health =  ((health.health+50) < 100) ? health.health+50 : 100;
                StartCoroutine(FlashGreen());
            }
        }
        else if (other.gameObject.CompareTag("Monster"))
        {
            health.health -= 30;
            StartCoroutine(FlashRed());
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
            damageOverlay.color = new Color(1, 0, 0, 0.5f); // Red overlay (50% opacity)
            yield return new WaitForSeconds(flashDuration);
            damageOverlay.color = new Color(1, 0, 0, 0); // Fade back to transparent
        }
    }

    IEnumerator FlashGreen()
    {
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(0, 1, 0, 0.5f); // Green overlay (50% opacity)
            yield return new WaitForSeconds(flashDuration);
            damageOverlay.color = new Color(0, 1, 0, 0); // Fade back to transparent
        }
    }

    void CheckPlayerDeath()
    {
        if (health.health <= 0)
        {
            isDead = true;
            rb.linearVelocity = Vector2.zero; // Stop movement immediately
            //notificationText.text = "YOU DIED!"; // Show death message
        }
    }

}
