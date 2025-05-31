
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image; 


public class HealtBar : MonoBehaviour
{
    [SerializeField] private Health PlayerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = PlayerHealth.currentHealth / 10f;
    }
    private void Update()
    {
        

        if (currenthealthBar != null)
            currenthealthBar.fillAmount = PlayerHealth.currentHealth / 10f;
        else
            Debug.LogWarning("currenthealthBar is null!");

        
        currenthealthBar.fillAmount = PlayerHealth.currentHealth / 10f;
    }
}

