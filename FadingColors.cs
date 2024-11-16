using UnityEngine;

public class FadingColors : MonoBehaviour
{
    [Tooltip("Option to deactivate the parent object.")]
    public bool deactivateParent = true;

    // Base decrease rates
    private float alphaDecreaseRate = 0.0003f;
    private float colorDecreaseRate = 4f * 0.0003f;

    private Material material;
    private float originalAlpha;

    void Start()
    {
        // Get the material of the object this script is attached to
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        else
        {
            Debug.LogError("No renderer attached to the object!");
            enabled = false; // Disable the script if there's no renderer
        }

        // Set the initial alpha value from the material color
        Color color = material.color;
        originalAlpha = color.a;

        // Randomly wait between 1 and 10 seconds before starting to affect alpha and color
        float randomWaitTime = Random.Range(0.5f, 1f);
        StartCoroutine(WaitBeforeFade(randomWaitTime));
    }

    private System.Collections.IEnumerator WaitBeforeFade(float waitTime)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTime);

        // Start decreasing alpha and converging color to white
        StartCoroutine(FadeToWhite());
    }

    private System.Collections.IEnumerator FadeToWhite()
    {
        Color color = material.color;

        while (color.a > 0)
        {
            // Adjust the rate based on the current alpha value relative to the original alpha
            if (color.a > (2f / 3f) * originalAlpha)
            {
                // Alpha is greater than 2/3 of the original, use 3x the base rate
                color.a -= alphaDecreaseRate * 3f;
            }
            else if (color.a > (1f / 3f) * originalAlpha)
            {
                // Alpha is between 1/3 and 2/3 of the original, use 2x the base rate
                color.a -= alphaDecreaseRate * 1.3f;
            }
            else
            {
                // Alpha is below 1/3 of the original, use the base rate
                color.a -= alphaDecreaseRate;
            }

            // Clamp the alpha to prevent it from going negative
            color.a = Mathf.Clamp01(color.a);

            // Move the color towards white (with the same rate adjustment)
            color.r += (1 - color.r) * colorDecreaseRate;
            color.g += (1 - color.g) * colorDecreaseRate;
            color.b += (1 - color.b) * colorDecreaseRate;

            // Apply the new color to the material
            material.color = color;

            // Wait for 10 milliseconds
            yield return new WaitForSeconds(0.01f);
        }

        // Deactivate the parent object if specified
        if (deactivateParent && transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false); // Disable the parent
        }
    }
}
