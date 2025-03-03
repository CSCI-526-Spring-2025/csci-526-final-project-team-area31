//using UnityEngine;
//using UnityEngine.Rendering.Universal; // Required for Light2D

//public class FlashlightController : MonoBehaviour
//{
//    public Transform player; // Assign player in Inspector
//    public Camera mainCamera; // Assign main camera in Inspector
//    public Vector3 offset = new Vector3(0.5f, 0.5f, 0); // Adjust flashlight position

//    void Start()
//    {
//        if (mainCamera == null)
//        {
//            mainCamera = Camera.main; // Get the main camera automatically
//        }
//    }

//    void Update()
//    {
//        if (player != null)
//        {
//            FollowPlayer();   // Keep the flashlight attached to the player
//            RotateFlashlight(); // Rotate flashlight towards the mouse cursor
//        }
//    }

//    void FollowPlayer()
//    {
//        // Attach the light to the player's position with an offset
//        transform.position = player.position + offset;
//    }

//    void RotateFlashlight()
//    {
//        // Get the mouse position in world space
//        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
//        mousePosition.z = 0; // Ensure it's in 2D space

//        // Calculate direction from light to mouse position
//        Vector3 direction = mousePosition - transform.position;

//        // Convert direction to an angle in degrees
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//        // Apply rotation to the flashlight
//        transform.rotation = Quaternion.Euler(0, 0, angle);
//    }
//}
//using UnityEngine;

//public class FlashlightController : MonoBehaviour
//{
//    public float offset = -90f; // Adjust if the cone points the wrong way

//    void Update()
//    {
//        // Get mouse position in world coordinates
//        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        mousePosition.z = 0; // Ensure Z is 0 for 2D

//        // Calculate direction from player to mouse
//        Vector3 direction = mousePosition - transform.parent.position;
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

//        // Rotate the flashlight
//        transform.rotation = Quaternion.Euler(0, 0, angle + offset);
//    }
//}

using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public float rotationOffset = -90f;
    public Vector3 positionOffset = new Vector3(0, 0, 0); // Adjust position relative to player

    void LateUpdate()
    {
        if (player == null) return; // Prevent null reference errors

        FollowPlayer();   // Keeps flashlight attached to player
        RotateFlashlight(); // Rotates flashlight independently
    }

    void FollowPlayer()
    {
        // Keep the flashlight at a fixed offset from the player's position
        transform.position = player.position + positionOffset;
    }

    void RotateFlashlight()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep in 2D space

        // Calculate direction from flashlight to mouse
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation while keeping the correct angle offset
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}
