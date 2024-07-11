using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    private float moveHorizontal;
    private float moveVertical;
    bool isGrounded;
    bool canStand;
    bool canCrouch;
    bool canSprint;
    bool canRun;
    int jumpCount;

    PlayerInput input;

    Animator playerAnimations;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        playerAnimations = GetComponent<Animator>();

        input = new PlayerInput();
        input.Enable();

        input.onFoot.Walk.performed += OnMoveInput;
        input.onFoot.Walk.canceled += OnMoveInput;

        input.onFoot.Jump.performed += OnJumpInput;

        input.onFoot.Crouch.performed += Crouch;
        input.onFoot.Crouch.canceled += Uncrouch;

        input.onFoot.Sprint.performed += Sprint;
        input.onFoot.Sprint.canceled += Run;

        input.onFoot.Hit.performed += OnHit;
        input.onFoot.Hit.canceled += OnHit;
    }
    void Start()
    {
        speed = 5f;
        jumpForce = 16f;
        canStand = true;
        canCrouch = true;
        canSprint = true;
        canRun = true;
        jumpCount = 2;
    }

    void FixedUpdate()
    {
        Movement();
        CheckIsGrounded();
        WallMechanics();

        if (!canStand)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -Vector2.down, 0.8f, 1 << 7);

            if (raycastHit2D.collider == null)
            {
                canStand = true;
                StandAfterCrouch();
            }
        }

        playerAnimations.SetFloat("speed", Mathf.Abs(moveHorizontal));
        playerAnimations.SetFloat("yVelocity", rb.velocity.y);
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        moveHorizontal  = context.ReadValue<float>();
    }

    void Movement()
    {
        if (moveHorizontal != 0f)
        {
            transform.localScale = new Vector3(moveHorizontal * 1.1f, 1.1f, 1.1f);
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }

    }

    void CheckIsGrounded()
    {
        RaycastHit2D ground = Physics2D.BoxCast(transform.position, new Vector2(0.6f, 1.2f), 0f, Vector2.down, 0.7f, 1<<7);

        if (ground.collider != null)
        {
            playerAnimations.SetBool("isGrounded", true);
            playerAnimations.SetBool("secondJump", false);
            playerAnimations.SetBool("isJumping", false);
            isGrounded = true;
            rb.gravityScale = 5;
        }
        else
        {
            playerAnimations.SetBool("isGrounded", false);
            isGrounded = false;
            canSprint = false;
        }
    }

    void WallMechanics()
    {
        RaycastHit2D wall = Physics2D.BoxCast(transform.position, new Vector2(0.6f, 1.2f), 0f, transform.localScale.x * Vector2.right, 0.2f, 1 << 7);
        Debug.DrawRay(transform.position, 0.3f * transform.localScale.x * Vector2.right, Color.red);

        if (wall.collider != null && !isGrounded && moveHorizontal == 0f)
        {
            rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(0, -1f);
        }
        else if(wall.collider != null && moveHorizontal != 0f)
        {
            rb.gravityScale = 0.5f;
        }
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        playerAnimations.SetBool("isJumping", true);
        if (isGrounded)
        {
            Jump();
            jumpCount = 2;
        }
        else if (!isGrounded && jumpCount > 1)
        {
            secondJump();
            jumpCount--;
        }
    }

    void Jump()
    {
        playerAnimations.SetBool("secondJump", true);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void secondJump()
    {
        playerAnimations.SetBool("secondJump", false);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.9f);
    }

    void Uncrouch(InputAction.CallbackContext context)
    {
        RaycastHit2D Uncrouch = Physics2D.Raycast(transform.position, -Vector2.down, 0.8f, 1<<7);
        
        if (Uncrouch.collider == null)
        {
            StandAfterCrouch();
            canStand = true;
        }
        else if (Uncrouch.collider != null)
        {
            canStand = false;
        }
    }

    void StandAfterCrouch()
    {
        canSprint = true;
        canRun = true;
        canCrouch = true;
        playerAnimations.SetBool("isCrouching", false);
        speed = 5;
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.6f, 1.2f);
    }

    void Crouch(InputAction.CallbackContext context)
    {
        if (canCrouch)
        {
            canSprint = false;
            canRun = false;
            playerAnimations.SetBool("isCrouching", true);
            playerAnimations.SetBool("sprinting", false);
            speed = 2;
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.6f, 0.8f);
        }
    }

    void Sprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            canCrouch = false;
            playerAnimations.SetBool("sprinting", true);
            speed = 10;
        }
    }

    void Run(InputAction.CallbackContext context)
    {
        if (canRun)
        {
            canSprint = true;
            canCrouch = false;
            playerAnimations.SetBool("sprinting", false);
            speed = 5;
        }
    }

    void OnHit(InputAction.CallbackContext context)
    {
        Debug.Log("Hit");
    }

}
