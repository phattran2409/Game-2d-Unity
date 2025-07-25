﻿using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float startingHealth = 4f;
    public float currentHealth;
    private Animator anim;
    private EnemyMovement enemyMovement;

    public bool Dead = false;
    public GameObject gameOverPanelPrefab;
    private GameObject instance;

    private PlayerController playerController;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip gameOverSound;
    public AudioClip HealedSound;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    
    public void IncreaseHealth(float amount)
    {

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        if (currentHealth > 0 && anim != null)
        {
            if (playerController != null && HealedSound != null && playerController.SfxAudioSource != null)
            {
                playerController.SfxAudioSource.PlayOneShot(HealedSound);
            }
            anim.SetTrigger("Heal");    
        }
    }

    public void TakeDamage(float damage)
    {
        if (Dead) return; // ✅ Ngăn gọi lại khi đã chết

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (playerController != null)
            {
                playerController.KnockBack(transform.position);

                if (hurtSound != null && playerController.SfxAudioSource != null)
                {
                    playerController.SfxAudioSource.PlayOneShot(hurtSound);
                }
            }

            anim.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    public void TakeDamage_1(float damage, Vector2 positionGetDamaged)
    {
        if (Dead) return; // ✅ Ngăn gọi lại khi đã chết

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            if (playerController != null)
            {
                playerController.KnockBack(positionGetDamaged);

                if (hurtSound != null && playerController.SfxAudioSource != null)
                {
                    playerController.SfxAudioSource.PlayOneShot(hurtSound);
                }
            }

            anim.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    public void Die()
    {
        if (Dead) return; // ✅ Phòng trường hợp bị gọi lại

        Dead = true;
        currentHealth = 0f;
        anim.SetTrigger("Die");

        GameObject musicObj = GameObject.Find("Music");
        if (musicObj != null)
        {
            AudioSource musicAudio = musicObj.GetComponent<AudioSource>();
            if (musicAudio != null)
            {
                musicAudio.Stop();
            }
        }

        if (gameOverSound != null && playerController != null && playerController.SfxAudioSource != null)
        {
            playerController.SfxAudioSource.PlayOneShot(gameOverSound);
        }

        if (gameOverPanelPrefab == null)
        {
            Debug.LogError("GameOver panel prefab is missing");
        }

        if (gameOverPanelPrefab != null)
        {
            instance = Instantiate(gameOverPanelPrefab, FindObjectOfType<Canvas>().transform);
        }
    }
}
