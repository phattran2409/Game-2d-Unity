using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float attackRange;
    [SerializeField] private float damage = 1f;
    [SerializeField] private Transform attackPoint; // vị trí đánh
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] public GameObject AttackPrefab;
    [SerializeField] public Transform spawnWeapon;
    [SerializeField] private TMPro.TMP_Text warningText;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private float knockbackForce = 2f; // Lực đẩy khi đánh trúng
    [SerializeField] private AudioClip attackSound; // Âm thanh đánh


    public GameObject bowObject;
    private Animator anim;
    private PlayerController player;
    private PlayerArrowIventory playerArrowIventory;
    private Coroutine warningRoutine;
    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        bowObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }



    void Update()
    {
        //if (player.attackTimer <= 1f)
        //{
        //    bowObject.SetActive(false);
        //}
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (context.performed && player.attackTimer <= 0)
        {
            player.attackTimer = player.attackCooldown;
            Quaternion rotation = transform.rotation;
            anim.SetTrigger("attack");
            Instantiate(AttackPrefab, attackPoint.position, rotation);
            Invoke(nameof(DoDamage), 0.1f);
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "SceneState1")
        {
            Debug.LogWarning("Cannot shoot in this scene!");
            ShowSceneWarning("Cannot shoot in this scene!");
            return;
        }
        if (bowObject == null)
        {
            Debug.LogError("Bow object is not assigned!");
            return;
        }
        bowObject.SetActive(true);

        playerArrowIventory = GetComponent<PlayerArrowIventory>();
        if (context.performed && player.attackTimer <= 0f)
        {
            if (playerArrowIventory.TryUseArrow())
            {

                player.attackTimer = player.attackCooldown;
                Quaternion rotation = transform.rotation;
                anim.SetTrigger("isShoot");
                bowObject.GetComponent<Bow>()?.StartShoot();

                Debug.Log("Player attacked!");
                if (attackSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }

                Invoke(nameof(DoDamage), 0.2f);
            }
        }
    }

    void DoDamage()
    {
        //Tìm tất cả enemy trong phạm vi chém
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        Debug.Log("Hit " + hitEnemies.Length + " enemies in range");
        if (hitEnemies.Length == 0) Debug.LogError("Not found layer Enemy");

        foreach (Collider2D col in hitEnemies)
        {
            IDamageable enemy = col.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Enemy " + col.name + " took " + damage + " damage");
            }
            else
            {
                Debug.LogWarning("Collider does not implement IDamageable: " + col.name);
            }


            IKnockbackable kb = col.GetComponent<IKnockbackable>();
            if (kb != null)
            {
                kb.KnockBack((col.transform.position - attackPoint.position).normalized, knockbackForce);
            }

            // Nếu bạn muốn gọi các phương thức cụ thể của từng loại enemy, bạn có thể sử dụng: 
            //enemy.GetComponent<bear>()?.TakeDamage(damage); // Kiểm tra nếu có component bear   
            //         enemy.GetComponent<EnemyBatController>().TakeDamage(damage);    
            //      enemy.GetComponent<Enemies>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }



    private void ShowSceneWarning(string message)
    {
        if (warningText == null || messagePanel == null) return;

        if (warningRoutine != null)
            StopCoroutine(warningRoutine);

        warningRoutine = StartCoroutine(ShowWarningRoutine(message));
    }

    private IEnumerator ShowWarningRoutine(string message)
    {
        warningText.text = message;
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        messagePanel.SetActive(false);
    }

}