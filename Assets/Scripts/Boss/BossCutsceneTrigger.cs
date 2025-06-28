using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class BossCutsceneTrigger : MonoBehaviour
{
    public PlayableDirector cutscene;
    public GameObject player;
    public CinemachineCamera bossCam;
    public CinemachineCamera mainCam;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerController>().enabled = false;
          
            bossCam.Priority = 11;
            mainCam.Priority = 0;
            cutscene.Play();
        }
    }
}
