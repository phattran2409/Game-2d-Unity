using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Enemies : MonoBehaviour , IDamageable 
{
     [Header("Cached Components")]
    //public bear bearScript;
    //public EnemyBatController batScript; 
    // handle attacked of enemy 
	private bool hasAttacked = false;
    //public Transform pointA;
    //public Transform pointB;	
    private bool isKnockbacking = false;

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
    public IEnumerator KnockBackThenResume(Vector2 direction, float distance, float duration , Vector3 target , Transform pointA , Transform pointB)
    {
        Vector3 originalTarget = target; // lưu lại hướng ban đầu
        isKnockbacking = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)(direction * distance);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // Sau khi knockback xong, reset lại target move
        target = GetNearestPatrolPoint(pointA, pointB).position;
        isKnockbacking = false;
    }
    public Transform GetNearestPatrolPoint(Transform pointA ,Transform pointB)
    {
        float distA = Vector2.Distance(transform.position, pointA.position);
        float distB = Vector2.Distance(transform.position, pointB.position);

        return distA < distB ? pointA : pointB;
    }
}
