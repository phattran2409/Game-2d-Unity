using System;
using Assets.Scripts;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyBatController : Enemies , IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float frequency = 2f;  // Tần số sóng (lượn nhanh hay chậm)
    public float amplitude = 0.5f; // Biên độ sóng (cao/thấp bao nhiêu)
    private Vector3 target; 

    private Vector2 direction;
    private float moveDistance;
    private float elapsedTime = 0f;
    private Vector2 startPosition;
    private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb; 
	private bool isDead = false; // Biến để theo dõi hướng di chuyển  
									   // Thêm Rigidbody2D để xử lý knockback
									   // Detect player 
	public Transform player;
    public GameObject attackEffectPrefab;
    public float attackRange = 2f;

    //private bool hasAttacked = false;
    public Transform attackPos;
    
    private Health health;  
    private float damage = 1f;

    private float  attackerTimer = 0f; 

    private Animator anim;
    [Header("Health Bat")]
    public float healthBat = 3f; // Sức khỏe của Bat 
    private float curentHealth =0;
    public Enemies enemies; 
    void Start()
    {   
        curentHealth = healthBat; // Khởi tạo sức khỏe hiện tại bằng sức khỏe tối đa    
        startPosition = pointA.position;
        direction = (pointB.position - pointA.position).normalized;
        moveDistance = Vector2.Distance(pointA.position, pointB.position);
        spriteRenderer = GetComponent<SpriteRenderer>();     
	    health = player.GetComponent<Health>(); // Lấy component Health từ Player
	    anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D để xử lý knockback 
        target = pointA.position; // Khởi tạo điểm mục tiêu ban đầu
    }

    void Update()
    {
        if (isDead) return; // Nếu Bat đã chết thì không thực hiện hành động nào khác   

		elapsedTime += Time.deltaTime * speed;

        // Tính vị trí trên đường thẳng
        float progress = Mathf.PingPong(elapsedTime, moveDistance) / moveDistance;
        Vector2 linearPos = Vector2.Lerp(pointA.position, pointB.position, progress);

        // Tính dao động hình sin theo chiều vuông góc với hướng bay
        Vector2 perp = new Vector2(-direction.y, direction.x); // vector vuông góc
        float sineOffset = Mathf.Sin(elapsedTime * frequency) * amplitude;

        // Vị trí cuối cùng = vị trí đường thẳng + dao động sin
        Vector2 finalPos = linearPos + perp * sineOffset;

        float deltaX = finalPos.x - transform.position.x;
        if (deltaX > 0.01f)
            spriteRenderer.flipX = false; // bay sang phải
        else if (deltaX < -0.01f)
            spriteRenderer.flipX = true;  // bay sang trái

        //float distance = Vector2.Distance(transform.position, player.position);
        //if (distance < attackRange && !hasAttacked && health.Dead == false)
        //{
        //    Attack();   
        //    hasAttacked = true;
        //    Invoke(nameof(ResetAttack), 2f); // Thời gian hồi
        //}
        attackerTimer -= Time.deltaTime;    
        if (DetectPlayer() && attackerTimer <= 0)
        {
            Attack();
            //hasAttacked = true; // Đánh dấu đã tấn công
            attackerTimer = enemies.attackCooldown; // Đặt lại thời gian tấn công
        }

        transform.position = finalPos;
    }

   public void Attack()
    {
        Vector2 dir = (player.position - attackPos.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        health = player.GetComponent<Health>();
        if (health == null)
        {
            return;
        }   
        health.TakeDamage(damage ); // Gọi hàm TakeDamage từ Health component    
        Instantiate(attackEffectPrefab, attackPos.position, rotation);
    }
     bool DetectPlayer()
    {
        return enemies.DetectPlayer(player, transform, attackRange, isDead); 

    }
    public void TakeDamage(float damage)
    { 
        curentHealth -= damage; // Giảm sức khỏe hiện tại    
        anim.SetTrigger("isHurt");
        //   Vector2 knockbackDir = (transform.position - player.position).normalized;
        //float knockbackForce = 10f;
        //rb.linearVelocity = knockbackDir * knockbackForce;
        Vector2 knockbackDir = (transform.position - player.position).normalized;
        float knockbackDistance = 1f;
        float knockbackDuration = 0.2f;
        StartCoroutine(KnockBackThenResume(knockbackDir, knockbackDistance , knockbackDuration ,target , pointA , pointB ));
		if (curentHealth <= 0f)
		{
			anim.SetTrigger("IsDead");
            Die(); // Gọi hàm Die nếu sức khỏe <= 0    
        }   
    }   

    void Die()
    {
        // Xử lý khi Bat chết
        // Ví dụ: phá hủy đối tượng Bat
	    isDead = true; // Đánh dấu Bat đã chết  
		rb.linearVelocity = Vector2.zero; // Dừng chuyển động   
		Destroy(gameObject , 1f);
    }   
  

}
