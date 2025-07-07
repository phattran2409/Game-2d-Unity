using UnityEngine;

public class CollectableArrow : MonoBehaviour
{

    public GameObject collectEffect;
    public PlayerArrowIventory playerArrowInventory;
    // Reference to the player's arrow inventory   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int arrowAmount;
    public int maxArrowAmout = 1;
    void Start()
    {
        arrowAmount = maxArrowAmout; // Set the amount of arrows to collect 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerArrowInventory = other.GetComponent<PlayerArrowIventory>();
            playerArrowInventory.AddArrow(arrowAmount); 
            // Hiện hiệu ứng
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
