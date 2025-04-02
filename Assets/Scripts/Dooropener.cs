using UnityEngine;
using System.Collections;

public class DoorOpener : MonoBehaviour {
    public int battery_limit;
    public GameObject nextLevelUI;
    public GameObject nextLevelGroundUI;
    private BatteryManager1 batteryManager1;
    private bool doorOpened = false;
    public CameraShake cameraShake;

    void Start() {
        batteryManager1 = GameObject.FindObjectOfType<BatteryManager1>();
        nextLevelUI.SetActive(false);
    }

    void Update() {
        // Optional: Keep this if you want the door to open automatically
        if (!doorOpened && batteryManager1.batteryCount >= battery_limit) {
            OpenDoor();
        }
    }

    public void OpenDoor() {
        if (doorOpened) return;

        doorOpened = true;
        //nextLevelGroundUI.SetActive(true);
        StartCoroutine(ShowNextLevel());
        if (cameraShake != null) {
            StartCoroutine(cameraShake.Shake(1f, 0.2f));
        }
    }

    IEnumerator ShowNextLevel() {
        nextLevelUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        nextLevelUI.SetActive(false);
        Destroy(gameObject); // Destroys the door
    }
}
