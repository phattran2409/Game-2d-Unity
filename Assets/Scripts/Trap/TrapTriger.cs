using UnityEngine;

public class TrapTriger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject trapPrefab;         // Kéo prefab vào từ Inspector    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trapPrefab.SetActive(true); // Kích hoạt prefab trap    
        }
    }   
}
