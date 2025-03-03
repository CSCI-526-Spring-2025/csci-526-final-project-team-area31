using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightFollow : MonoBehaviour
{
    public Transform player; // Assign the player object
    public Vector3 offset = new Vector3(0, 0, 0); // Adjust flashlight position

    void Update()
    {
        if (player != null)
        {
            // Make the flashlight follow the player
            transform.position = player.position + offset;
            transform.rotation = player.rotation;
        }

        // Toggle flashlight ON/OFF when pressing 'F'
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<Light2D>().enabled = !GetComponent<Light2D>().enabled;
        }
    }
}
