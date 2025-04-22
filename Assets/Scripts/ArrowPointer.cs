using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (gameObject.activeSelf && target != null)
        {
            Vector2 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void ShowArrow(Transform newTarget)
    {
        target = newTarget;
        gameObject.SetActive(true); 
    }

    public void HideArrow()
    {
        target = null;
        gameObject.SetActive(false); 
    }
}
