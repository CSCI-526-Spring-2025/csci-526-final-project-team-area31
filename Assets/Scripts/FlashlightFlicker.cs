using UnityEngine;

public class FlashlightFlicker : MonoBehaviour
{
    public float baseIntensity = 1.0f;    // The default light intensity.
    public float flickerIntensity = 0.5f; // The amount by which the light flickers.
    public float flickerSpeed = 10.0f;    // The speed of flicker variation.

    private Light flashlight;

    void Start()
    {
        flashlight = GetComponent<Light>();
        if (flashlight != null)
        {
            flashlight.intensity = baseIntensity;
        }
    }

    void Update()
    {
        if (flashlight != null)
        {
            // Generate a smooth random value using Perlin noise.
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
            flashlight.intensity = baseIntensity + (noise - 0.5f) * flickerIntensity;
        }
    }
}
