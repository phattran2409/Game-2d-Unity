using System;
using UnityEngine;

public class bear : MonoBehaviour
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
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
        };  
        if (!enemyMovement.isChasing)
        {
            Debug.Log("Bear is patrolling");   
            anim.SetTrigger("walk");  
            rb.linearVelocity = new Vector2(moveSpeed * (enemyMovement.movingRight ? 1 : -1), rb.linearVelocity.y);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
            if (!groundInfo.collider )
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

    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        anim.SetTrigger("Hurt");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;      
            anim.SetTrigger("Dead");
            Die();
        }
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
}
