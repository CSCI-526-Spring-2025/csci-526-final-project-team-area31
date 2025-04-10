using UnityEngine;
using TMPro;

public class DropOffScript : MonoBehaviour
{
    public int batteryneed;
    private BatteryManager1 batteryManager1;

    public GameObject batteryUI;
    public TextMeshProUGUI batteryStatusText; // Reference to your text element
    public DoorOpener doorOpener; // Link this manually in the Inspector!
    public int levelIndex;
    void Start()
    {
        batteryManager1 = GameObject.FindFirstObjectByType<BatteryManager1>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            Debug.Log("Drop offed battery");
            if (batteryManager1.batteryCount >= batteryneed)
            {
                if (doorOpener != null)
                {
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.LogDoorOpened(batteryManager1.batteryCount, batteryneed);
                    }
                    batteryManager1.batteryCount -= batteryneed;
                    doorOpener.OpenDoor();
                    Destroy(gameObject);

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.SetCurrentRespawnPoint(levelIndex);
                    }
                    else
                    {
                        Debug.LogError("LevelTrigger: GameManager instance not found!");
                    }
                }
            }
            else
            {
                // Not enough batteries — show UI with how many more are needed
                int batteriesRemaining = batteryneed - batteryManager1.batteryCount;
                batteryStatusText.text = "You need " + batteriesRemaining + " more batteries to recharge this door!";
                batteryUI.SetActive(true);
                Debug.Log("Not enough batteries. Showing UI.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            batteryUI.SetActive(false);
        }
    
    }
}
