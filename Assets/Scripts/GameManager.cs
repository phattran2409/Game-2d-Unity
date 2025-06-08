using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        Debug.Log("End checkpoint reached!");
    }
}
