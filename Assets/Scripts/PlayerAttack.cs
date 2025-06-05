using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float attackRange;
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform attackPoint; // vị trí đánh
    [SerializeField] private LayerMask enemyLayer;

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("attack");
            Invoke(nameof(DoDamage), 0.2f);
        }
    }


    void DoDamage()
    {
        //Tìm tất cả enemy trong phạm vi chém
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        Debug.Log("Hit " + hitEnemies.Length + " enemies in range");    
        if (hitEnemies.Length == 0) Debug.LogError("Not found layer Enemy"); 
        
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<bear>()?.TakeDamage(damage);   
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
