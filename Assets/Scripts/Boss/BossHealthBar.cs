using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image lightBar;

    private float totalSegments;
    private float targetFillAmount;
    private float lerpSpeed = 5f;

    public void Setup(float total)
    {
        totalSegments = total;
        targetFillAmount = 1f;
        lightBar.fillAmount = 1f;
    }

    public void SetHealth(float currentSegments)
    {
        if (totalSegments <= 0) return;

        targetFillAmount = (float)currentSegments / totalSegments;
    }

    private void Update()
    {
        if (Mathf.Abs(lightBar.fillAmount - targetFillAmount) > 0.001f)
        {
            lightBar.fillAmount = Mathf.Lerp(lightBar.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        }
    }
}
