using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float launchHeight = 1.5f;
    public GameObject target; 
    private Rigidbody2D rb;
    private Animator anim;
    private bool hasExploded = false;

    [Header("Explosion Settings")]
    public float explosionRadius = 2f;
    public float damage = 1f;
    public LayerMask playerLayer; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (target != null)
        {
            LaunchParabolic(target.transform.position);
        }
        else
        {
            Debug.LogWarning("Fireball has no target!");
        }
    }

    public void LaunchParabolic(Vector2 targetPos)
    {
        Vector2 startPos = transform.position;

        float ratio = 0.85f;
        Vector2 adjustedTarget = startPos + (targetPos - startPos) * ratio;
        Vector2 adjustedToTarget = adjustedTarget - startPos;

        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float heightOffset = Mathf.Max(launchHeight, 0.1f);

        float travelTime = Mathf.Abs(adjustedToTarget.x) / 5f;

       
        if (adjustedTarget.y > startPos.y)
        {
            travelTime *= 1.7f;
        }

        float vx = adjustedToTarget.x / travelTime;

     
        float verticalGap = adjustedTarget.y - startPos.y;
        float peakY = Mathf.Max(startPos.y, adjustedTarget.y) + Mathf.Max(heightOffset, Mathf.Abs(verticalGap) * 0.5f);

        float vy = (peakY - startPos.y + 0.5f * gravity * travelTime * travelTime) / travelTime;

        rb.linearVelocity = new Vector2(vx, vy);
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        anim.SetTrigger("Explode");

      
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            Health playerHealth = hit.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage_1(damage, transform.position); 
            }
        }
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
