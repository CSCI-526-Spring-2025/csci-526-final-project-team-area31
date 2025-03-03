using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public static BatteryManager Instance;

    private int batteryCount = 0;
    public int batteriesNeededToWin = 3;
    private Vector3 playerStartPosition;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player
        if (player != null)
        {
            playerStartPosition = player.transform.position; // Store the starting position
        }
    }

    public void CollectBattery()
    {
        batteryCount++;
        Debug.Log("Batteries Collected: " + batteryCount);
    }

    public void ReturnToStart()
    {
        if (player != null)
        {
            player.transform.position = playerStartPosition; // Reset player's position
            Debug.Log("Player returned to start position.");
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        if (batteryCount >= batteriesNeededToWin)
        {
            Debug.Log("You collected enough batteries! You win!");
            // Add UI message or level transition here
        }
        else
        {
            Debug.Log("Not enough batteries. Keep collecting!");
        }
    }
}
