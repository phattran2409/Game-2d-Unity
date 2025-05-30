using UnityEngine;

public class DestroyEffect : MonoBehaviour
{

    public float destroyDelay = 3f; // Thời gian trước khi xoá hiệu ứng
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject , destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
