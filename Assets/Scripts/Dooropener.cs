using UnityEngine;
public class DoorOpener : MonoBehaviour {
    public int battery_limit = 1;
    BatteryManager1 batteryManager1;
    void Update(){
        batteryManager1 = GameObject.FindObjectOfType<BatteryManager1>();
        if (batteryManager1.batteryCount >= battery_limit){
            Destroy(gameObject);
        }
    }
}