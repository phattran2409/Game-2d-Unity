using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    
    public bool movingRight = true;
    public bool isChasing = false;
    public int damage = 1;

    [Header("Chase Settings")]
    [SerializeField] public float chaseSpeed = 2f;
    [SerializeField] public float detectRange = 5f;
    //[SerializeField] private Transform player;


    void Awake()
    {
    }   
    void Start()
    {
            }

    // Update is called once per frame
    void Update()
    {
      
    }
    

   //public void Die()
   // {
   //     // Xử lý khi enemy chết
   //     // Ví dụ: phá hủy đối tượng enemy

   //     if (currentHealth <= 0)
   //     {
   //         anim.SetTrigger("Dead");    
   //     }
   //     Destroy(gameObject);
   // }   

   public void DetectPlayer(Transform player , Health playerHealth)
   {

        if (playerHealth != null && playerHealth.Dead)
        {
            isChasing = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <  detectRange;
   }


 


}
