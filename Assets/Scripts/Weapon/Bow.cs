using UnityEngine;

public class Bow : MonoBehaviour
{
    private Animator anim;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float destroyDelay = 1.0f;
    public PlayerController player;

    void Start()
    {
        anim = GetComponent<Animator>();
       
    }   
    public void StartShoot()
    {   
        anim = GetComponent<Animator>();
        anim.SetTrigger("Shoot"); // animation cung bắn\q
        
    
    }   

    // Gọi từ Animation Event tại frame "nhả cung"
    public void FireArrow() 
    {
        //GameObject bowObj = Instantiate(bowPrefab, spawnPoint.position, Quaternion.identity);
        //bowObj.transform.localScale = new Vector3(player.transform.localScale.x, 1f, 1f);
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        Vector2 dir = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Debug.Log("Direction of arrow: " + dir);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            Debug.Log("Arrow script found on arrow prefab!");   
            arrowScript.SetDirection(dir);
        }
        else
        {
            Debug.LogError("Arrow prefab missing Arrow script!");
        }
    }

     public void HideBow() => gameObject.SetActive(false);   

    //private void OnEnable()
    //{
    //    Destroy(gameObject, destroyDelay); // Tự hủy sau animation nếu prefab chỉ dùng 1 lần
    //}

}
