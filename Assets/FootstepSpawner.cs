using UnityEngine;

public class FootstepSpawner : MonoBehaviour
{
    public GameObject footstepPrefab;
    public float stepInterval = 0.3f; // Time between steps
    private float stepTimer = 0f;
    private Vector2 lastPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsMoving())
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                SpawnFootstep();
                stepTimer = 0f;
            }
        }
    }

    bool IsMoving()
    {
        return Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
    }

    void SpawnFootstep()
    {
        Vector2 movementDirection = rb.linearVelocity.normalized;

        if (movementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            Instantiate(footstepPrefab, transform.position, Quaternion.Euler(0, 0, angle - 90f));
        }
    }
}
