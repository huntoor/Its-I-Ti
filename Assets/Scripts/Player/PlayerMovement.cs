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
    int jumpCount;

    PlayerInput input;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        input = new PlayerInput();
        input.Enable();

        input.onFoot.Walk.performed += OnMoveInput;
        input.onFoot.Walk.canceled += OnMoveInput;

        input.onFoot.Jump.performed += OnJumpInput;

        input.onFoot.Crouch.performed += Crouch;
        input.onFoot.Crouch.canceled += Uncrouch;

        input.onFoot.Sprint.performed += Sprint;
        input.onFoot.Sprint.canceled += NormalSpeed;

        input.onFoot.Hit.performed += OnHit;
        input.onFoot.Hit.canceled += OnHit;
    }
    void Start()
    {
        speed = 5f;
        jumpForce = 16f;
        canStand = true;
        jumpCount = 2;
    }

    void FixedUpdate()
    {
        Movement();
        CheckIsGrounded();

        if (!canStand)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -Vector2.down, 0.8f, 1 << 7);

            if (raycastHit2D.collider == null)
            {
                StandAfterCrouch();
                canStand = true;
            }
        }
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
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(0.6f, 1.2f), 0f, Vector2.down, 0.7f, 1<<7);

        if (hit.collider != null)
        {
            isGrounded = true;
            rb.gravityScale = 5;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
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
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void secondJump()
    {
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
        speed = 5;
        GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, 0f);
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.6f, 1.2f);
    }

    void Crouch(InputAction.CallbackContext context)
    {
        speed = 2;
        GetComponent<CapsuleCollider2D>().offset = new Vector2(0f, -0.2f);
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.6f, 0.8f);
    }

    void Sprint(InputAction.CallbackContext context)
    {
        speed = 10;
    }

    void NormalSpeed(InputAction.CallbackContext context)
    {
        speed = 5;
    }

    void OnHit(InputAction.CallbackContext context)
    {
        Debug.Log("Hit");
    }

}
