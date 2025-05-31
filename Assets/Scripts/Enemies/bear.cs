using System;
using UnityEngine;

public class bear : MonoBehaviour
{


     [SerializeField] private float moveSpeed = 2f;
     [SerializeField] private Transform groundCheck;
     [SerializeField] private LayerMask groundLayer;
     public Rigidbody2D rb;
     public float groundCheckDistance = 0.1f;
    [Header("Chase player")]
    [SerializeField] private Transform player;

    public Health health;

    public EnemyMovement enemyMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        enemyMovement.DetectPlayer(player, health);

        if (!enemyMovement.isChasing)
        {
            rb.linearVelocity = new Vector2(moveSpeed * (enemyMovement.movingRight ? 1 : -1), rb.linearVelocity.y);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            if (!groundInfo.collider)
            {
                Flip();
            }
        }
        else
        {
            chasePlayer(player);
        }
    }

    public void chasePlayer(Transform player)
    { 
       
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

    //private void OnTriggerEnter2D(Collider2D other)
    //{ 
    //    if (other.CompareTag("Player") && enemyMovement.isChasing)
    //    {
    //        other.GetComponent<Health>()?.TakeDamage(enemyMovement.damage); // Gọi hàm TakeDamage trên đối tượng Player
    //        Debug.Log("Bear attacked player! Damage: " + enemyMovement.damage); // In ra thông báo khi tấn công thành công
    //    }
    //}
}
