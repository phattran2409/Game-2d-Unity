using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Health Settings")]
    public int totalSegments = 4;
    public int currentSegment;
    public bool isDead = false;

    [Header("UI Components")]
    public BossHealthBar healthBarUI;

    [Header("Teleportation Settings")]
    public Transform[] teleportPoints;
    private int lastTeleportIndex = -1;

    [Header("Attack Settings")]
    public float attackCooldown = 5f;
    private float attackTimer;



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
        if (isDead) return;

        // Update health bar
        UpdateHealthBar();
        // Auto attack logic
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            anim.SetTrigger("isAttacking");
            attackTimer = attackCooldown;
        }

#if UNITY_EDITOR          
        UpdateHealthBar();
#endif
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentSegment = Mathf.Max(0, currentSegment - 1);
        UpdateHealthBar();

        if (currentSegment > 0)
        {
            anim.SetTrigger("isHurt");
        }
        else
        {
            Die();
        }
    }

    public void OnDisappearAnimationComplete()
    {
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
}
