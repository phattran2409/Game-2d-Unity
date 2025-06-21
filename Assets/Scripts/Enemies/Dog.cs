using Assets.Scripts;
using UnityEngine;

public class Dog : Enemies , IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Point Move")]
    public Transform pointA;
    public Transform pointB;
    private Vector3 target;
    public float speed = 2f;
    private Transform targetPoint;

    private SpriteRenderer spriteRenderer;

    [Header("Attack")]
    public Transform attackPos;
    public Transform player;
    public GameObject attackEffectPrefab;
    public Health health;
    private float damage = 1f;
    public float attackRange = 2f;

    [Header("Anim")]
    private Animator anim;
    // Flip Attack Pos
    [SerializeField] private Vector2 attackOffsetRight = new Vector2(1.5f, 0);
    [SerializeField] private Vector2 attackOffsetLeft = new Vector2(-1.5f, 0);

    public Enemies enemies;
    private bool isDead = false; // Biến để theo dõi hướng di chuyển
    private float attackTimer = 0f;

    [Header("Health")]
    private float maxHealth = 3f; // Sức khỏe của Slimer	

    private float currentHealth; // Sức khỏe hiện tại của Slimer

    [Header("Rigid body")]
    private Rigidbody2D rb; // Rigid body của Slimer	

    private bool isKnockbacking = false; // Biến để theo dõi trạng thái knockback

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo sức khỏe hiện tại bằng sức khỏe tối đa	
        target = pointA.position;
        targetPoint = pointA; // Khởi tạo điểm mục tiêu ban đầu	
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigid body của Slimer	 
    }
    void Update()
    {
        if (isDead || isKnockbacking) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }

        flip();


        attackTimer -= Time.deltaTime;
        if (DetectPlayer() && attackTimer <= 0f)
        {
            anim.SetTrigger("IsChase");
            Attack();
            attackTimer = enemies.attackCooldown;
        }

    }

    void flip()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            if (targetPoint == pointA)
            {
                Debug.Log("Dog is at point A");
                Debug.Log("TargetGetpoint :"+targetPoint.position);
                targetPoint = pointB; // Chuyển sang điểm B	
                
                spriteRenderer.flipX = true;
                attackPos.localPosition = attackOffsetRight;
            }
            else
            {

                targetPoint = pointA;

                spriteRenderer.flipX = false;
                attackPos.localPosition = attackOffsetLeft;
            }
        }
    }
    bool DetectPlayer()
    {
        return enemies.DetectPlayer(player, transform, attackRange, isDead);
    }
    public void Attack()
    {
        health = player.GetComponent<Health>();
        Quaternion rotation = transform.rotation;

        if (health == null)
        {
            return;
        }
        health.TakeDamage_1(damage, transform.position);
        Instantiate(attackEffectPrefab, attackPos.position , rotation );
    }

    

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;// Giảm sức khỏe của Slimer	
        float xDir = transform.position.x - player.position.x;
        Vector2 knockbackDir = new Vector2(Mathf.Sign(xDir), 0);
        float knockbackDistance = 1f;
        float knockbackDuration = 0.2f;
        StartCoroutine(KnockBackThenResume(knockbackDir, knockbackDistance, knockbackDuration, target, pointA, pointB));

        if (currentHealth <= 0f)
        {
            isDead = true;
            anim.SetTrigger("IsDead"); // Gọi animation chết	
            Destroy(gameObject, 1f);
        }

    }

    public void KnockBack(Vector2 direction, float force)
    {
        if (rb == null) return;

        // reset velocity trước khi knockback
        rb.linearVelocity = Vector2.zero;
        // đẩy với impulse
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

    }

}
