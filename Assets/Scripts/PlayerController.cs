using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator anim;
    public Collider2D coll;
    public GameObject collectEffect;

    [Header("Force Knock")]
    public float knockBackForce = 10f;
    public float knockBackUpWard = 2f;

    private float horizontal;
    private bool isFacingRight = true;
    private bool isJumping;

    [Header("UI")]
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI gemText;

    public int cherries = 0;
    public int gems = 0;
    private enum PlayerState { idle, run, jumping, land }
    private PlayerState currentState;
    private bool grounded = false;
    private Health health;

    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip stepLoopSound;
    public AudioClip collectFruitSound; 
    public AudioClip collectGemSound;
    private AudioSource stepAudioSource;  
    private AudioSource sfxAudioSource;   

    void Start()
    {
        GameObject startPoint = GameObject.FindWithTag("StartCheckpoint");
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
        }

        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        health = GetComponent<Health>();

        // Setup 2 audio sources
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            stepAudioSource = sources[0];
            sfxAudioSource = sources[1];
        }
        else
        {
            stepAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }

        stepAudioSource.loop = true;
    }

    void Update()
    {
        if (health.Dead) return;

        grounded = IsGrounded();

        // Handle animation
        Debug.Log("Grounded: " + grounded);
        if (grounded)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else if (!grounded && rb.linearVelocity.y > 0.1f)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        // Flip character
        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();

        UpdateState();

        // Handle step sound
        if (grounded && Mathf.Abs(horizontal) > 0.1f)
        {
            if (!stepAudioSource.isPlaying || stepAudioSource.clip != stepLoopSound)
            {
                stepAudioSource.clip = stepLoopSound;
                stepAudioSource.Play();
            }
        }
        else
        {
            if (stepAudioSource.isPlaying && stepAudioSource.clip == stepLoopSound)
            {
                stepAudioSource.Stop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (health.Dead) return;

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private void UpdateState()
    {
        bool grounded = IsGrounded();
        float yVelocity = rb.linearVelocity.y;

        if (!grounded && yVelocity < -0.1f)
        {
            ChangeState(PlayerState.land);
        }
        else if (grounded)
        {
            ChangeState(PlayerState.idle);
        }
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        anim.Play(newState.ToString()); // Play clip theo tên state (Idle, Run, Jump...)
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (health.Dead) return;
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (health.Dead) return;

        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (jumpSound != null && sfxAudioSource != null)
            {
                Debug.Log("Jump sound played");
                sfxAudioSource.PlayOneShot(jumpSound);
            }
            else
            {
                Debug.LogWarning("JumpSound or sfxAudioSource is NULL");
            }
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // reduce jump height
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void KnockBack(Vector2 acctackerPos)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)acctackerPos).normalized;

        float knockDirectionX = MathF.Sign(knockbackDirection.x);
        Vector2 force = new Vector2(knockDirectionX, knockBackForce) * knockBackUpWard;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with: " + other.gameObject.name);

        if (other.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
            cherries += 1;
            cherryText.text = ": " + cherries.ToString();
            Debug.Log("Cherries collected: " + cherries);

            if (collectFruitSound != null && sfxAudioSource != null)
            {
                sfxAudioSource.PlayOneShot(collectFruitSound);
            }
        }

        if (other.CompareTag("Gems"))
        {
            Destroy(other.gameObject);
            gems += 1;
            gemText.text = ": " + gems.ToString();
            Debug.Log("Gems collected: " + gems);

            if (collectGemSound != null && sfxAudioSource != null)
            {
                sfxAudioSource.PlayOneShot(collectGemSound);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = IsGrounded();
        Debug.Log("Player collided with: " + collision.gameObject.name);
    }

    public AudioSource SfxAudioSource => sfxAudioSource;
}
