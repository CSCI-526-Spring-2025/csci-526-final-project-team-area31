using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform door;  // Assign in inspector

    void Update()
    {
        if (door != null)
        {
            Vector2 direction = door.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}