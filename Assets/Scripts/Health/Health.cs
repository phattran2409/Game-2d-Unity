using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float startingHealth = 4f;
    public float currentHealth;
    private Animator anim;
    private EnemyMovement enemyMovement;

    public bool Dead = false;
    public GameObject gameOverPanelPrefab;
    private GameObject instance;

    private PlayerController playerController;  
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();        
        enemyMovement = GetComponent<EnemyMovement>();
    }
    
    public void IncreaseHealth(float amount)
    {

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        if (currentHealth > 0 && anim != null)
        {
            anim.SetTrigger("Heal");
        }
    }

    public void TakeDamage(float damage )
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (playerController != null)
            {
                playerController.KnockBack(transform.position); // Apply knockback to the player
            }
            //Player hurt
            anim.SetTrigger("Hurt");
        }
        else if (currentHealth <= 0)
        {            // Player is dead
                Die();
        }
    }
   public void TakeDamage_1(float damage , Vector2 positionGetDamaged )
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (playerController != null)
            {
                playerController.KnockBack(positionGetDamaged); // Apply knockback to the player
            }
            //Player hurt
            anim.SetTrigger("Hurt");
        }
        else if (currentHealth <= 0)
        {            // Player is dead
                Die();
        }
    }


    public void Die()
    {
    
        Dead = true;
        currentHealth = 0f; // Ensure health is set to zero on death    
                            // Handle death logic here, e.g., disable the enemy, play death animation, etc.
                            anim.SetTrigger("Die");
                            if (gameOverPanelPrefab == null)
                            {
                                 Debug.LogError("GameObject is null in Die method.");   
        }
        if (gameOverPanelPrefab != null)
        {
            instance = Instantiate(gameOverPanelPrefab, FindObjectOfType<Canvas>().transform);
        }

        if (gameObject == null)
        {
            Debug.LogError("GameObject is null in Die()");
            return;
        }
         
	    
    }

    private void Update()
    {
       
    }
}
