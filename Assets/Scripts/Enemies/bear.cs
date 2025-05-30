using System;
using UnityEngine;

public class bear : MonoBehaviour
{

     public EnemyMovement enemyMovement;

     [SerializeField] private float moveSpeed = 2f;
     [SerializeField] private Transform groundCheck;
     [SerializeField] private LayerMask groundLayer;
     public float groundCheckDistance = 0.1f;
    [Header("Chase player")]
    [SerializeField] private Transform player;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyMovement.rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
        enemyMovement.DetectPlayer(player);
        if (!enemyMovement.isChasing)
        {
            enemyMovement.rb.linearVelocity = new Vector2(moveSpeed * (enemyMovement.movingRight ? 1 : -1), enemyMovement.rb.linearVelocity.y);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            if (!groundInfo.collider)
            {
                Flip();
            }
        }
        else
        {
            Debug.Log("Chasing player: " + true);    
            chasePlayer(player);
        }
    }

    public void chasePlayer(Transform player)
    { 
       
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        enemyMovement.rb.linearVelocity = new Vector2(direction * enemyMovement.chaseSpeed, enemyMovement.rb.linearVelocity.y);

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

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Player") && enemyMovement.isChasing)
        {
            other.GetComponent<Health>()?.TakeDamage(enemyMovement.damage); // Gọi hàm TakeDamage trên đối tượng Player
            Debug.Log("Bear attacked player! Damage: " + enemyMovement.damage); // In ra thông báo khi tấn công thành công
        }
    }
}
