using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float startingHealth = 4f;
    public float currentHealth;
    private Animator anim;
    private bool Dead;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();        
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
            if (!Dead)
            {
                anim.SetTrigger("Die");
            }   
            // Player is dead
         
        }
    }

 

    public void Die()
    {
        Dead = true;
        currentHealth = 0f; // Ensure health is set to zero on death    
        // Handle death logic here, e.g., disable the enemy, play death animation, etc.
        anim.SetTrigger("Die");
    }

    private void Update()
    {
    }
}
