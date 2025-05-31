using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float startingHealth = 4f;
    public float currentHealth;
    private Animator anim;
    public bool Dead = false;
    
    private EnemyMovement enemyMovement;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();        
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            //Player hurt
             anim.SetTrigger("Hurt");
        }
        else if (currentHealth <= 0)
        {
            anim.SetTrigger("Die");
            Die();
            // Player is dead
         
        }
    }



    public void Die()
    {
    
        Dead = true;
        currentHealth = 0f;
        
        // Nếu là enemy:
        if (enemyMovement != null)
        {
            enemyMovement.isChasing = false;
        }
    }

    private void Update()
    {
       
    }
}
