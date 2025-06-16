using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioClip musicClip;
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
