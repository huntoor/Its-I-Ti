using TMPro;
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
    //bool isRunning;
    bool canStand;
    bool canCrouch;
    bool canSprint;
    bool canJump;
    bool canRun;
    int jumpCount;

    public bool canDoubleAttack;
    float attackDelay;
    float doubelAttackDelay;


    PlayerInput input;

    Animator playerAnimations;
    
    AudioSource audioSource;
    [SerializeField] AudioClip walkingClip;
    [SerializeField] AudioClip jumpOneClip;
    [SerializeField] AudioClip jumpTwoClip;
    [SerializeField] AudioClip attackOneClip;
    [SerializeField] AudioClip attackTwoClip;

    public delegate void InteractPressed();
    public static InteractPressed interactPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerAnimations = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        input = new PlayerInput();
        input.Enable();

        input.onFoot.Walk.performed += OnMoveInput;
        input.onFoot.Walk.canceled += OnMoveInput;

        input.onFoot.Jump.performed += OnJumpInput;

        input.onFoot.Crouch.performed += Crouch;
        input.onFoot.Crouch.canceled += Uncrouch;

        input.onFoot.Sprint.performed += Sprint;
        input.onFoot.Sprint.canceled += Run;

        input.onFoot.Interact.performed += Interact;

        input.onFoot.Hit.performed += Attack;
    }

    void Start()
    {
        speed = 5f;
        jumpForce = 15f;
        canStand = true;
        canCrouch = true;
        canSprint = true;
        canRun = true;
        canJump = true;
        // isRunning = false;
        jumpCount = 2;
    }

    void Update()
    {
        attackDelay -= Time.deltaTime;
        doubelAttackDelay -= Time.deltaTime;


        if (doubelAttackDelay < 0)
        {
            canDoubleAttack = false;
        }
    }

    void FixedUpdate()
    {
        Movement();
        CheckIsGrounded();
        WallMechanics();

        if (!canStand)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, -Vector2.down, 1f, 1 << 7);

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
            //isRunning = true;
            transform.localScale = new Vector3(moveHorizontal * 1.1f, 1.1f, 1.1f);
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }
        else if (moveHorizontal == 0f)
        {
            //isRunning = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    void CheckIsGrounded()
    {
        RaycastHit2D ground = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 1f), 0f, Vector2.down, 0.6f, 1<<7);

        if (ground.collider != null)
        {
            playerAnimations.SetBool("isGrounded", true);
            playerAnimations.SetBool("isJumping", false);
            isGrounded = true;
            jumpCount = 2;
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
        RaycastHit2D wall = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0f, transform.localScale.x * Vector2.right, 0.2f, 1 << 7);

        if (wall.collider != null && !isGrounded)
        {
            rb.gravityScale = 0.5f;
            playerAnimations.SetBool("isWallSliding", true);
        }
        else if(wall.collider == null || isGrounded)
        {
            rb.gravityScale = 5f;
            playerAnimations.SetBool("isWallSliding", false);
        }
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            if (isGrounded)
            {
                Jump();

                audioSource.clip = jumpOneClip;
                audioSource.loop = false;
                audioSource.Play();
            }
            else if (!isGrounded && jumpCount > 1)
            {
                SecondJump();

                audioSource.clip = jumpTwoClip;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
    }

    void Jump()
    {
        playerAnimations.SetBool("isJumping", true);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount--;
    }

    void SecondJump()
    {
        playerAnimations.SetBool("isJumping", true);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.8f);
        jumpCount--;
    }

    void Uncrouch(InputAction.CallbackContext context)
    {
        RaycastHit2D Uncrouch = Physics2D.Raycast(transform.position, -Vector2.down, 1f, 1<<7);
        
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
        canJump = true;
        playerAnimations.SetBool("isCrouching", false);
        speed = 5;
        GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, 1f);
    }

    void Crouch(InputAction.CallbackContext context)
    {
        if (canCrouch)
        {
            canJump = false;
            canSprint = false;
            canRun = false;
            playerAnimations.SetBool("isCrouching", true);
            playerAnimations.SetBool("sprinting", false);
            speed = 2;
            GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, 0.6f);
        }
    }

    void Sprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            canCrouch = false;
            playerAnimations.SetBool("sprinting", true);
            speed = 7;
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

    void Attack(InputAction.CallbackContext context)
    {
        if (attackDelay < 0)
        {
            playerAnimations.SetTrigger("attackOne");

            attackDelay = 1f;

            doubelAttackDelay = 2f;

            audioSource.clip = attackOneClip;
            audioSource.loop = false;
            audioSource.Play();
        }

        if (canDoubleAttack && doubelAttackDelay > 0)
        {
            playerAnimations.SetTrigger("attackTwo");

            canDoubleAttack = false;
            
            audioSource.clip = attackOneClip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        interactPressed?.Invoke();
    }
}