using UnityEngine;
using System.Collections;

public class DarkModeTrigger : MonoBehaviour
{
    public GameObject darkModeTutorial;
    public FlashlightBattery battery;


    void Start() {
        battery = GameObject.FindObjectOfType<FlashlightBattery>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            battery.batteryLife = 50;
            Debug.Log("Darkzone trigger ");
            darkModeTutorial.SetActive(true); 
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
           darkModeTutorial.SetActive(false); 
        }
    }
}
