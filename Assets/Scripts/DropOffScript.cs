using UnityEngine;

public class DropOffScript : MonoBehaviour
{
    public int batteryneed;
    private BatteryManager1 batteryManager1;
    
    public GameObject batteryUI;
    public DoorOpener doorOpener; // Link this manually in the Inspector!

    void Start() {
        batteryManager1 = GameObject.FindObjectOfType<BatteryManager1>();
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
                    batteryManager1.batteryCount = 0;
                    doorOpener.OpenDoor();
                    Destroy(gameObject);
                }
            }
            else
            {
                // Not enough batteries — show UI
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
