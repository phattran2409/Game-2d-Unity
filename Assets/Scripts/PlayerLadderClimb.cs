using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLadderClimb : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing;
    private bool isOnLadder;

    private Rigidbody2D rb;
    private Animator animator;

  

    private InputSystem_Actions inputActions; 
    private Vector2 moveInput;

    private void Awake()
    {
        inputActions = new InputSystem_Actions(); 
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        float vertical = moveInput.y;

        if (isOnLadder && Mathf.Abs(vertical) > 0.1f)
        {
            isClimbing = true;
        }
        else if (Mathf.Abs(vertical) == 0)
        {
            isClimbing = false;
        }

        animator.SetBool("isClimbing", isClimbing);
        animator.SetFloat("ClimbSpeed", vertical);
    }

    void FixedUpdate()
    {
        float vertical = moveInput.y;

        if (isClimbing)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);
         
        }
        else if (isOnLadder)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimbing = false;
            animator.SetBool("isClimbing", false);
      
        }
    }
}
