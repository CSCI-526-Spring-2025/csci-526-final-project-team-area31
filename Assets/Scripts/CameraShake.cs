using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CameraMovement cameraMovement;

    void Awake()
    {
        cameraMovement = GetComponent<CameraMovement>();
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            if (cameraMovement != null)
                cameraMovement.shakeOffset = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (cameraMovement != null)
            cameraMovement.shakeOffset = Vector3.zero;
    }
}
