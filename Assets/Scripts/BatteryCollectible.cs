using UnityEngine;

public class BatteryCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // If the player touches the battery
        {
            BatteryManager.Instance.CollectBattery(); // Add battery to the counter
            Debug.Log("battery collected");
        }
    }
}
