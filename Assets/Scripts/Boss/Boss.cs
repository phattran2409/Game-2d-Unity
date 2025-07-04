using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Boss : MonoBehaviour, IDamageable
{
    [Header("Cached Components")]
    private bool hasAttacked = false;
    private bool isKnockbacking = false;

    public bool DetectPlayer(Transform player, Transform enemy, float attackRange, bool isDead)
    {


        float distance = Vector2.Distance(enemy.position, player.position);
        if (distance < attackRange && !hasAttacked && isDead == false)
        {     
            return true;

        }
        return false;
    }

   

    public void Die(GameObject gameObject, float time)
    {
        Destroy(gameObject, time);
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
    public IEnumerator KnockBackThenResume(Vector2 direction, float distance, float duration, Vector3 target, Transform pointA, Transform pointB)
    {
        Vector3 originalTarget = target; 
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

        target = GetNearestPatrolPoint(pointA, pointB).position;
        isKnockbacking = false;
    }
    public Transform GetNearestPatrolPoint(Transform pointA, Transform pointB)
    {
        float distA = Vector2.Distance(transform.position, pointA.position);
        float distB = Vector2.Distance(transform.position, pointB.position);

        return distA < distB ? pointA : pointB;
    }
}
