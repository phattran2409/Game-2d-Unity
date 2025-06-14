using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject completePanel;
    private GameObject instancePrefab;
    public bool reachedEndCheckpoint = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReachEndCheckpoint()
    {
        reachedEndCheckpoint = true;
        Time.timeScale = 0f; 
        instancePrefab = Instantiate(completePanel, FindObjectOfType<Canvas>().transform);
        Debug.Log("End checkpoint reached!");
    }
}
