using System.Collections;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class Slimer : MonoBehaviour, IDamageable , IKnockbackable
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
	private float  attackTimer = 0f;

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
		targetPoint  = pointA; // Khởi tạo điểm mục tiêu ban đầu	
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
		return  enemies.DetectPlayer(player, transform, attackRange, isDead);	
	}
	public void Attack()
	{
		health = player.GetComponent<Health>();
		if (health == null)
		{
			return;
		}
		health.TakeDamage_1(damage , transform.position); // Gọi hàm TakeDamage từ Health component    
		anim.SetTrigger("isAttack"); // Gọi animation Attack	
	}
	 

	public void TakeDamage(float damage)
	{	
		currentHealth -= damage;// Giảm sức khỏe của Slimer	
		if (currentHealth  <= 0f)
		{
			isDead = true;
			anim.SetTrigger("isDead"); // Gọi animation chết	
			Destroy(gameObject , 1f);
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


	private IEnumerator KnockBackThenResume(Vector2 direction, float distance, float duration)
	{
		Vector3 originalTarget = target; // lưu lại hướng ban đầu
		isKnockbacking = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + (Vector3)(direction * distance);
		float elapsed = 0f;

		while (elapsed < duration)
		{
			transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.position = endPos;

		// Sau khi knockback xong, reset lại target move
		target = GetNearestPatrolPoint().position;
		isKnockbacking = false;
	}

	private Transform GetNearestPatrolPoint()
	{
		float distA = Vector2.Distance(transform.position, pointA.position);
		float distB = Vector2.Distance(transform.position, pointB.position);

		return distA < distB ? pointA : pointB;
	}
}
