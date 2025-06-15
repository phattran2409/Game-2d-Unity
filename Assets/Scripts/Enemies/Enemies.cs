using Assets.Scripts;
using UnityEngine;

public class Enemies : MonoBehaviour , IDamageable 
{
     [Header("Cached Components")]
    //public bear bearScript;
    //public EnemyBatController batScript; 
    // handle attacked of enemy 
	private bool hasAttacked = false;

	[Header("Attack Cooldown")]
	public float attackCooldown = 2f;
    public bool DetectPlayer(Transform player  , Transform enemy , float attackRange , bool isDead)
	{

		
		float distance = Vector2.Distance(enemy.position, player.position);
		if (distance < attackRange && !hasAttacked && isDead == false)
		{
			
			Invoke(nameof(ResetAttack) , 2f);
			return true; 
			
		}
        return false; 
	}   

    void ResetAttack()
	{
		hasAttacked = false; // Reset trạng thái đã tấn công
	}    

	public void Die(GameObject gameObject , float time)
	{
		Destroy(gameObject, time);
	}

	public void TakeDamage(float damage)
	{
		throw new System.NotImplementedException();
	}

	 
}
