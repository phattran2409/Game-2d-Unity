
using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

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

    public float attackTimer = 0f;
    public float attackCooldown = 0.5f;
    public GameObject effectHealth;
    
    private PlayerLadderClimb climbScript;
    [Header("Buffer time JUMP")]
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    void Start()
    {
        GameObject startPoint = GameObject.FindWithTag("StartCheckpoint");
        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
        }
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        climbScript = GetComponent<PlayerLadderClimb>();
    }

    void Update()
    {
        if (health.Dead) return;

        grounded = IsGrounded();

        // ➤ Movement using Transform (instead of Rigidbody)
        Vector3 move = new Vector3(horizontal * speed * Time.deltaTime, 0f, 0f);
        transform.Translate(move);

        // ➤ Animation
        if (grounded)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else if (!grounded && rb.linearVelocity.y > 0.1f && climbScript.isOnLadder == false)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        // ➤ Flip
        if (!isFacingRight && horizontal > 0f) Flip();
        else if (isFacingRight && horizontal < 0f) Flip();

        // ➤ Attack cooldown timer
        attackTimer -= Time.deltaTime;

        // ➤ Update animation state
        UpdateState();


        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0 && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        // ❌ Không cần xử lý horizontal ở đây nữa
        if (health.Dead) return;
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
        anim.Play(newState.ToString());
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (health.Dead) return;
        horizontal = context.ReadValue<Vector2>().x;
    }

  
    public void Jump(InputAction.CallbackContext context)
    {
        if (health.Dead) return;


        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void KnockBack(Vector2 attackerPos)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPos).normalized;
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
        }

        if (other.CompareTag("Gems"))
        {
            Destroy(other.gameObject);
            gems += 1;
            gemText.text = ": " + gems.ToString();
            Debug.Log("Gems collected: " + gems);
        }

        if (other.CompareTag("Heart"))
        {
            Destroy(other.gameObject);
            health.IncreaseHealth(1f);
            GameObject effect = Instantiate(effectHealth, transform.position, Quaternion.identity);
            effect.transform.SetParent(transform);
            Destroy(effect, 1f);    
            Debug.Log("Health increased. Current health: " + health.currentHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = IsGrounded();
        Debug.Log("Player collided with: " + collision.gameObject.name);
    }
}

