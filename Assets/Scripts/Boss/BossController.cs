using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BossController : Boss, IDamageable
{
    public bool isInCutscene = false;


    [Header("Health Settings")]
    public float totalSegments = 4;
    public float currentSegment;
    public bool isDead = false;
    private bool isInvulnerable = false; 
    public float invulnerableTime = 3f;

    [Header("UI Components")]
    public BossHealthBar healthBarUI;

    [Header("Teleportation Settings")]
    public Transform[] teleportPoints;
    private int lastTeleportIndex = -1;

    [Header("Attack Settings")]
    public float attackCooldown = 5f;
    private float attackTimer;


    [Header("Fireball Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;

    [Header("Player Reference")]
    public Transform player;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentSegment = totalSegments;
        attackTimer = attackCooldown;
    
        if (healthBarUI != null)
        {
            healthBarUI.Setup(totalSegments);
        }
    }

    private void Update()
    {
        if (isDead || isInCutscene) return;

        // Update health bar
        UpdateHealthBar();
        // Auto attack logic
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            anim.SetTrigger("isAttacking");
            ShootFireball();
            attackTimer = attackCooldown;
        }


    }

  

    public void OnDisappearAnimationComplete()
    {
        //if (isDead || isInCutscene) return;
        if (teleportPoints != null && teleportPoints.Length > 1)
        {
            
           TeleportToNewPoint();
        }

        anim.SetTrigger("hasAppeared");
    }


    private void Die()
    {
        isDead = true;
        //anim.SetBool("isDead", true);
        anim.SetTrigger("Dead");

        Destroy(gameObject, 2f);
    }

    private void UpdateHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentSegment);
        }
    }

    private void TeleportToNewPoint()
    {
        if (isDead || isInCutscene) return;
        if (teleportPoints != null && teleportPoints.Length > 1)
        {
            int index;


            do
            {
                index = Random.Range(0, teleportPoints.Length);
            } while (index == lastTeleportIndex);

            lastTeleportIndex = index;
            transform.position = teleportPoints[index].position;
        }
        else if (teleportPoints != null && teleportPoints.Length == 1)
        {
            transform.position = teleportPoints[0].position;
          
        }
        else
        {
            Debug.LogWarning("No teleport points assigned.");
        }

        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable || isInCutscene) return;
        currentSegment -= damage; 
        anim.SetTrigger("isHurt");
        UpdateHealthBar();
        if (currentSegment <= 0f)
        {
            Die(); 
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
    }

    void ShootFireball()
    {
        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fb.GetComponent<Fireball>().target = player.gameObject;
    }
}
