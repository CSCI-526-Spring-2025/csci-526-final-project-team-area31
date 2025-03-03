using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MonsterLightReflector : MonoBehaviour
{
    public Light2D reflectionLight;  // Light on the monster that will amplify
    public Transform player;
    public float maxDistance = 5f;
    public float maxIntensity = 1.5f;

    private FlashlightBattery flashlightBattery;

    void Start()
    {
        if (player != null)
        {
            flashlightBattery = player.GetComponentInChildren<FlashlightBattery>();
        }

        if (reflectionLight != null)
        {
            reflectionLight.intensity = 0f;  // Start with no reflection
        }
    }

    void Update()
    {
        if (player == null || reflectionLight == null || flashlightBattery == null)
            return;

        if (!flashlightBattery.flashlight.enabled)  // No reflection if flashlight off
        {
            reflectionLight.intensity = 0f;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > maxDistance)
        {
            reflectionLight.intensity = 0f;  // Out of range = no reflection
            return;
        }

        // Closer = stronger reflection
        float intensityFactor = 1f - (distance / maxDistance);
        reflectionLight.intensity = intensityFactor * maxIntensity;
    }
}
