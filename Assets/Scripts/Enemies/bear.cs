﻿
using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class bear : MonoBehaviour, IDamageable
{


    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public int maxHealth = 3;
    public int currentHealth;
    public Rigidbody2D rb;
    public float groundCheckDistance = 0.1f;
    private Animator anim;
    [Header("Chase player")]
    [SerializeField] private Transform player;

    public Health health;

    public EnemyMovement enemyMovement;

    private EnemyAttack enemyAttack;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // HEADER 

 
    private void Awake()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovement.DetectPlayer(player, health);
        if (isDead || isKnockbacking)
        {
            //rb.linearVelocity = Vector2.zero;
            return;
        };
        if (!enemyMovement.isChasing)
        {
            Patrol();
        }
        else
        {
            chasePlayer(player);
        }
    }

    public void Patrol ()
    {
        anim.SetTrigger("walk");
        rb.linearVelocity = new Vector2(moveSpeed * (enemyMovement.movingRight ? 1 : -1), rb.linearVelocity.y);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if (!groundInfo.collider)
        {
            Flip();
        }

    }
    public void chasePlayer(Transform player)
    {
        // Nếu đã chết thì không thực hiện hành động nào khác   
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * enemyMovement.chaseSpeed, rb.linearVelocity.y);

        // Flip sprite nếu cần
        if ((direction > 0 && !enemyMovement.movingRight) || (direction < 0 && enemyMovement.movingRight))
        {

            Flip();
        }
    }

    void Flip()
    {
        enemyMovement.movingRight = !enemyMovement.movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }


    public void Die()
    {

        if (isDead)
        {
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D is null!");
            }
            rb.linearVelocity = Vector2.zero;
            Destroy(gameObject, 1f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return; // Nếu đã chết thì không xử lý va chạm nữa  
        
        if (other.CompareTag("Player"))
        {
            if (enemyAttack != null)
            {
                enemyAttack.Attack();
            }
            else
            {
                Debug.LogError("enemyAttack is null!");

            }
        }
    }

    public void TakeDamage(float damage)
    {
         currentHealth -= Mathf.FloorToInt(damage);
        KnockbackHorizontal(player);
        anim.SetTrigger("Hurt"); // Gọi animation bị thương   
        if (currentHealth <= 0f)
        {
            isDead = true;
            anim.SetTrigger("Dead"); // Gọi animation chết	
            Destroy(gameObject, 1f);
        }

    }

    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.3f;

    private bool isKnockbacking = false;

    public void KnockbackHorizontal(Transform attacker)
    {
        if (isKnockbacking || isDead) return;

        isKnockbacking = true;  

        // Xác định hướng knockback
        int direction = transform.position.x > attacker.position.x ? 1 : -1;
     
        // Dừng mọi chuyển động trước khi knockback
        rb.linearVelocity = Vector2.zero;

        // Gán velocity theo chiều ngang
        rb.linearVelocity = new Vector2(direction * knockbackForce, rb.linearVelocity.y);

        // Bắt đầu coroutine reset sau thời gian knockback
        StartCoroutine(ResetAfterKnockback());
    }


    private IEnumerator ResetAfterKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);

        // Dừng lại (nếu muốn), hoặc để tiếp tục patrol/chase
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        isKnockbacking = false;
    }
}