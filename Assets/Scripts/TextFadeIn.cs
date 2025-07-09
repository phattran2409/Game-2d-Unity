using TMPro;
using UnityEngine;
using System.Collections;

public class TextFadeIn : MonoBehaviour
{
    public float fadeDuration = 2f;

    private TextMeshProUGUI tmpText;
    private Color originalColor;

    private void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        originalColor = tmpText.color;
        tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Start transparent
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            tmpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
    }
}
