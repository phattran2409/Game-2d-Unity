using UnityEngine;

public class Spike : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject trapPrefab;       
    private bool hasSpawned = false;
    private float Damage = 1f;
    private Health playerHealth;    
    private PlayerController playerController; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<Health>();
            //playerController = other.GetComponent<PlayerController>();  
            
            playerHealth.TakeDamage_1(Damage , transform.position); // Gọi phương thức TakeDamage trên đối tượng Health
            //playerController.KnockBack(transform.position);
            Debug.Log("Player has entered the trap area and took damage." + playerHealth.currentHealth);
            
        }
    }
}
