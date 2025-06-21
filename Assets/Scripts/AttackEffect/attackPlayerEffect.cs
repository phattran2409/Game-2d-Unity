using UnityEngine;

public class attackPlayerEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float destroyAfter = 0.5f;

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    public void DestroyAfterAnim()
    {
        Destroy(gameObject);
    }
}
