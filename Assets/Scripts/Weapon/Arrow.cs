using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public int damage = 1;

    private Rigidbody2D rb;
    public GameObject impactEffect;
    public LayerMask hitEnemy;
    public float destroyDelay = 1.0f;
    private Vector2 direction;

    //public void Update()
    //{

    //}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void FixedUpdate()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void SetDirection(Vector2 dir)
    {
        Debug.Log("Direction dir : " + dir);
        if (rb == null)
        {
            Debug.Log("Rigidbody2D is null, adding component.");
            rb = GetComponent<Rigidbody2D>();
        }
        if (rb == null)
        {
            Debug.Log("Rigid body null");
        }
        direction = dir.normalized;
        //rb.linearVelocity = direction * (speed*2);
        rb.linearVelocity = Vector2.zero;


        Vector2 launchForce = new Vector2(direction.x, 0.2f).normalized * speed;
        rb.AddForce(launchForce, ForceMode2D.Impulse);
        // Lấy hướng của vận tốc
        Debug.Log($"Arrow direction set to: {direction}, speed: {speed}");  
        Debug.Log($"Arrow Rigidbody2D velocity: {rb.linearVelocity}");
        float angle = Mathf.Atan2(launchForce.y , launchForce.x) * Mathf.Rad2Deg;
        Debug.Log("Angle :"+angle);
        transform.rotation = Quaternion.Euler (0 , 0 , angle); // Quay mũi tên theo hướng vận tốc   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        // 3. Tùy chọn: gắn tên vào đối tượng bị dính (ví dụ tường)
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
        transform.SetParent(collision.transform);

        Destroy(gameObject, 2f); // tự hủy sau 2s
    }

    private void OnEnable()
    {
        Destroy(gameObject, destroyDelay); // Tự hủy sau animation nếu prefab chỉ dùng 1 lần
    }

}
