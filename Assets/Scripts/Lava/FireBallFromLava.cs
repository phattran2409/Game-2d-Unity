using UnityEngine;

public class FireBallFromLava : MonoBehaviour
{

    [SerializeField] private float damage;

    [SerializeField] private float speed;

    [SerializeField] private float distance;

    [SerializeField] private float startDelay;

    //private bool movingTop;
    //private float topEdge;
    //private float bottomEdge;  
    //private void Awake()
    //{
    //    // Set the initial position of the fireball;
    //    topEdge = transform.position.y + distance;
    //    bottomEdge = transform.position.y - 3f;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (movingTop)
    //    {
    //        if (transform.position.y < topEdge)
    //        {
    //            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
    //        }
    //        else
    //        {
    //            movingTop = false; // Change direction if the top edge is reached 
    //        }
    //    }
    //    else
    //    {
    //        if (transform.position.y > bottomEdge)
    //        {
    //            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
    //        }
    //        else
    //        {
    //            movingTop = true; // Change direction if the bottom edge is reached 
    //        }
    //    }

    //}


    // New Script 

    private Vector3 startPos;
    private bool goingUp = true;

    void Start()
    {
        startPos = transform.position;
        InvokeRepeating(nameof(SwitchDirection), startDelay, distance / speed);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 target = startPos + (goingUp ? Vector3.up : Vector3.down) * distance;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    void SwitchDirection()
    {
        goingUp = !goingUp;
        startPos = transform.position;
    }
}
