using UnityEngine;
using System.Linq;  // Needed for auto-detection trick

public class MonsterPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;  // You can drag & drop these directly now!
    public float speed = 2f;

    private int currentPointIndex = 0;

    void Start()
    {
        // Optional: Automatically detect patrol points if they are all children of "PatrolPoints"
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            GameObject container = GameObject.Find("PatrolPoints");
            if (container != null)
            {
                patrolPoints = container.GetComponentsInChildren<Transform>()
                                        .Where(t => t != container.transform) // Exclude the parent object itself
                                        .ToArray();
            }
        }
    }

    void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // Optional: Draw patrol path in Scene View for debugging
    void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Length < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
        }
        Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
    }
}
