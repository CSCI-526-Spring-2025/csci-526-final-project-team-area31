using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal; // To access Light2D

public class MonsterPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;  // Standard patrol route
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 2f;
    public float baseDetectionRange = 1f;   // Very short detection range
    public float maxDetectionRange = 4f;    // Shorter max detection range

    private int currentPointIndex = 0;
    private Transform player;
    private Light2D flashlight;
    private bool isChasing = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GameObject container = GameObject.Find("PatrolPoints");
            if (container != null)
            {
                patrolPoints = container.GetComponentsInChildren<Transform>()
                                        .Where(t => t != container.transform)
                                        .ToArray();
            }
        }

        player = GameObject.FindGameObjectWithTag("Player1")?.transform;
        if (player != null)
        {
            flashlight = player.GetComponentInChildren<Light2D>();
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player1")?.transform;
            if (player == null) return;
        }

        if (flashlight == null && player != null)
        {
            flashlight = player.GetComponentInChildren<Light2D>();
        }

        float dynamicDetectionRange = baseDetectionRange;
        if (flashlight != null && flashlight.enabled)
        {
            float intensity = flashlight.intensity;
            dynamicDetectionRange = Mathf.Lerp(baseDetectionRange, maxDetectionRange, intensity / 5f);
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= dynamicDetectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > dynamicDetectionRange)
        {
            isChasing = false;  // Player escaped if they get far enough away
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPointIndex];
        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * chaseSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDetectionRange);  // Show max possible detection range
    }
}