// using UnityEngine;

// public class DropOffScript : MonoBehaviour
// {
//     public int batteryneed;
//     private BatteryManager1 batteryManager1;
//     private DoorOpener doorOpener;

//     void Start() {
//         batteryManager1 = GameObject.FindObjectOfType<BatteryManager1>();
//         doorOpener = GameObject.FindObjectOfType<DoorOpener>(); // Assumes only one door
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player1"))
//         {
//             Debug.Log("Drop offed battery");
//             if (batteryManager1.batteryCount >= batteryneed)
//             {
//                 if (doorOpener != null)
//                 {
//                     doorOpener.OpenDoor();
//                 }
//             }
//         }
//     }
// }
