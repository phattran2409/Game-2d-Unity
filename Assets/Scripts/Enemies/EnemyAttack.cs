using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask playerLayer;
    
    EnemyMovement enemyMovement;    
    void tart()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        if (attackPos == null)
        {
            Debug.LogError("Attack position is not set on " + gameObject.name);
        }
    }   
    public void Attack()
    {
        // Kiểm tra va chạm với người chơi trong phạm vi tấn công
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Gọi hàm nhận sát thương trên người chơi
            player.GetComponent<Health>()?.TakeDamage(enemyMovement.damage);    
            Debug.Log("Player attacked!");
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
