using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float attackRange;
    [SerializeField] private float damage = 1f;
    [SerializeField] private Transform attackPoint; // vị trí đánh
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockbackForce = 2f; // Lực đẩy khi đánh trúng
    [SerializeField] private AudioClip attackSound; // Âm thanh đánh

    private Animator anim;
    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();

        // Lấy hoặc thêm AudioSource nếu chưa có
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("attack");

            // Phát âm thanh khi đánh
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }

            Invoke(nameof(DoDamage), 0.2f);
        }
    }

    void DoDamage()
    {
        //Tìm tất cả enemy trong phạm vi chém
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        Debug.Log("Hit " + hitEnemies.Length + " enemies in range");
        if (hitEnemies.Length == 0) Debug.LogError("Not found layer Enemy");

        foreach (Collider2D col in hitEnemies)
        {
            IDamageable enemy = col.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy " + col.name + " took " + damage + " damage");
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDamageable: " + col.name);
            }

            IKnockbackable kb = col.GetComponent<IKnockbackable>();
            if (kb != null)
            {
                kb.KnockBack((col.transform.position - attackPoint.position).normalized, knockbackForce);
            }

            // Nếu bạn muốn gọi các phương thức cụ thể của từng loại enemy, bạn có thể sử dụng: 
            //enemy.GetComponent<bear>()?.TakeDamage(damage); // Kiểm tra nếu có component bear   
            //         enemy.GetComponent<EnemyBatController>().TakeDamage(damage);    
            //      enemy.GetComponent<Enemies>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
