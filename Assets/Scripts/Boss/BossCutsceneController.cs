using UnityEngine;
using Unity.Cinemachine;

public class BossCutsceneController : MonoBehaviour
{
    public GameObject player;
    public GameObject bossHealthBar;
    public AudioSource bossMusic;
    public BossController bossController;
    public BossArrowIndicator bossArrowIndicator;



    public CinemachineCamera mainCam;
    public CinemachineCamera bossCam;
    public CinemachineImpulseSource impulseSource;

    void Start()
    {
        bossController.isInCutscene = true;
        bossArrowIndicator.isInCutscene = true;
        bossHealthBar.SetActive(false);
    }
    public void StartCameraShake()
    {
        impulseSource.GenerateImpulse();

    }

    public void PlayBossTheme()
    {
        if (!bossMusic.isPlaying)
            bossMusic.Play();
    }

    public void ShowBossUI()
    {
        bossHealthBar.SetActive(true);
    }


    public void EndCutscene()
    {
        bossCam.Priority = 0;
        mainCam.Priority = 10;
        player.GetComponent<PlayerController>().enabled = true;
        bossController.isInCutscene = false;
        bossArrowIndicator.isInCutscene = false;
    }
}
