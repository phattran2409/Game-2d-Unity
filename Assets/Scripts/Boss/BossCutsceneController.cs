using UnityEngine;
using Unity.Cinemachine;
using System;

public class BossCutsceneController : MonoBehaviour
{
    public GameObject player;
    public GameObject bossHealthBarPrefab;
    private GameObject healthBarInstance;
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
        //bossHealthBar.SetActive(false);
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
        Debug.Log("ShowBossUI called");

        if (bossHealthBarPrefab == null)
        {
            Debug.LogError("Prefab is null!");
            return;
        }

        GameObject canvasGO = GameObject.Find("Boss");
        if (canvasGO == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        healthBarInstance = Instantiate(bossHealthBarPrefab, canvasGO.transform);
        Debug.Log("Health bar instantiated!");

        BossHealthBar bar = healthBarInstance.GetComponentInChildren<BossHealthBar>();
        if (bar != null)
        {
            Debug.Log("BossHealthBar found, passing to bossController");
            bossController.SetHealthBar(bar);
        }
        else
        {
            Debug.LogError("BossHealthBar component not found!");
        }
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
