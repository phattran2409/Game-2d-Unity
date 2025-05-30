using UnityEngine;

public class Enemy_sideway : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float distance;

    private bool movingLeft;
    private float leftEdge; 
    private float rightEdge;    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Set the initial position of the enemy;
        leftEdge = transform.position.x - distance;
        rightEdge = transform.position.x + distance;    
    }  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Damage :"+damage);
        Debug.Log("Collision with: " + collision.gameObject.name);  
        if (collision.CompareTag("Player"))
        { 
            collision.GetComponent<Health>().TakeDamage(damage);
            
        }
    }   
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);    
            }
            else
            {
                movingLeft = false; // Change direction if the left edge is reached 
            }
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = true; // Change direction if the right edge is reached 
            }
        }

    }
}
