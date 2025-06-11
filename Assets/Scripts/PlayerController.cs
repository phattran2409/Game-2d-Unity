using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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
    }

    void Update()
    {
        // Move
        if (health.Dead == false)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);

            // Ground check
             grounded = IsGrounded();

            // Animation control
            //anim.SetBool("isJumping", !grounded); 
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

            // Flip
            if (!isFacingRight && horizontal > 0f) Flip();
            else if (isFacingRight && horizontal < 0f) Flip();


            UpdateState();
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
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // reduce jump height
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
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

        float knockDirectionX = MathF.Sign(knockbackDirection.x);// Ensure knockback force is set correctly  
        Vector2 force = new Vector2(knockDirectionX ,  knockBackForce) * knockBackUpWard;
        
        rb.linearVelocity  = Vector2.zero; // Reset velocity before applying knockback  
        rb.AddForce(force, ForceMode2D.Impulse); // Apply knockback force   
    }

    //public void Die()
    //{
    //    if (isDead) return;

    //    isDead = true;
    //    rb.linearVelocity = Vector2.zero; // Stop movement
    //    anim.SetTrigger("Die"); // Play die animation
    //    coll.enabled = false; // Disable collider to prevent further collisions
    //    Debug.Log("Player died");
    //}   
    // Collision detection for cherries

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = IsGrounded();    
        Debug.Log("Player collided with: " + collision.gameObject.name);
    }
}



