using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject completePanel;
    private GameObject instancePrefab;
    public bool reachedEndCheckpoint = false;

    [Header("Audio")]
    public AudioClip victorySound;             
    private AudioSource sfxAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReachEndCheckpoint()
    {
        reachedEndCheckpoint = true;

        GameObject musicObj = GameObject.Find("Music");
        if (musicObj != null)
        {
            AudioSource musicAudio = musicObj.GetComponent<AudioSource>();
            if (musicAudio != null)
            {
                musicAudio.Stop();
            }
        }

        if (victorySound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(victorySound);
        }

        Time.timeScale = 0f; 
        instancePrefab = Instantiate(completePanel, FindObjectOfType<Canvas>().transform);
        Debug.Log("End checkpoint reached!");
    }
}
