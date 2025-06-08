using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float fallDelay = 0.5f;

    private Collider2D col;
    private bool isTriggered = false;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.collider.CompareTag("Player"))
        {
            isTriggered = true;
            Invoke("MakeFallThrough", fallDelay);
        }
    }

    void MakeFallThrough()
    {
        if (col != null)
        {
            col.isTrigger = true;
            rb.bodyType = RigidbodyType2D.Dynamic; // Chuyển sang kiểu động để rơi  
        }
    }   
}
