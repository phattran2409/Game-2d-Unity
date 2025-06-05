using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask playerLayer;
    
    public  EnemyMovement enemyMovement;
    private Animator anim;  
    public bool isAttacking = false;   
    void Awake()
    {
       anim = GetComponent<Animator>(); 
    }

    void Start()
    {
        //enemyMovement = GetComponent<EnemyMovement>();
        if (attackPos == null)
        {
            Debug.LogError("Attack position is not set on " + gameObject.name);
        }
    }

    void Update()
    {
        // Kiểm tra nếu đang tấn công và animation đã kết thúc
        if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            isAttacking = false; // Đặt lại trạng thái tấn công
        }
    }   


    public void Attack()
    {
        if (attackPos == null)
        {
            Debug.LogError("attackPos is null!");
            return;
        }

        if (enemyMovement == null)
        {
            Debug.LogError("enemyMovement is null!");
            return;
        }

        // Kiểm tra va chạm với người chơi trong phạm vi tấn công
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Gọi hàm nhận sát thương trên người chơi
            player.GetComponent<Health>()?.TakeDamage(enemyMovement.damage);    
            anim.SetTrigger("Attack"); // Gọi animation tấn công    
            isAttacking = true; // Đánh dấu là đang tấn công    
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
