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
        nextLevelUI.SetActive(false); // Ensure it's inactive at the start
    }

    void Update() {
        if (!doorOpened && batteryManager1.batteryCount >= battery_limit) {
            doorOpened = true;
            nextLevelGroundUI.SetActive(true);
            StartCoroutine(ShowNextLevel());
            StartCoroutine(cameraShake.Shake(1f, 0.2f));
        }
    }

    IEnumerator ShowNextLevel() {
        nextLevelUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        nextLevelUI.SetActive(false);
        
        Destroy(gameObject); // Destroy AFTER coroutine finishes
    }

}
