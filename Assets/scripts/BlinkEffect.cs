using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public Material wallMaterial;    // Assign the wall material in the Inspector
    public float blinkSpeed = 1f;    // Control how fast the walls blink
    private Color originalColor;     // Store the original emission color

    void Start()
    {
        // Store the original emission color
        if (wallMaterial.HasProperty("_EmissionColor"))
        {
            originalColor = wallMaterial.GetColor("_EmissionColor");
        }
        else
        {
            originalColor = Color.white; // Default to white if emission is not set
        }

        // Enable emission for the material
        wallMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Blink the walls by adjusting the emission intensity over time
        float emissionStrength = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        Color emissionColor = originalColor * Mathf.LinearToGammaSpace(emissionStrength);
        wallMaterial.SetColor("_EmissionColor", emissionColor);
    }
}
