using UnityEngine;
using UnityEngine.EventSystems;

public class Goblin : BaseEnemy
{

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip detectedPlayerClip;
    [SerializeField] private AudioClip attackingClip;

    [Space]
    [SerializeField] private GameObject attackZone;

    private AudioSource myAudioSource;
    private float attackDelay;

    private enum State
    {
        Hidden,
        Idle,
        Attack,
        Dead
    }

    private State currentState;

    private State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;

            HandleAnimation();
            HandleAudio();
        }
    }


    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        myAnimator = GetComponent<Animator>();

        myAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        PlayerDamageZone.damageEnemy += TakeDamage;
    }

    void OnDisable()
    {
        PlayerDamageZone.damageEnemy -= TakeDamage;
    }

    private void Start()
    {
        CurrentState = State.Hidden;

        movementDirection = 1;

        initialPosition = transform.position;

        canMove = true;

        attackDelay = 2f;
    }

    private void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Hidden:
                Patrolling();
                break;
            case State.Idle:
                ReturnToPosition();
                break;
            case State.Attack:
                Chasing();
                break;
            case State.Dead:
                Debug.Log("Deadge (ง •̀_•́)ง");
                break;
            default:
                Debug.Log("WTF!!");
                break;
        }

        canMove = CheckEdge();
    }

    protected override void Patrolling()
    {
        myRigidBody.gravityScale = 0;

        GetComponent<SpriteRenderer>().sortingOrder = -10;
    }

    public override void OnPlayerDetected(Collider2D playerCol)
    {
        CurrentState = State.Attack;

        myAudioSource.clip = detectedPlayerClip;
        myAudioSource.loop = false;
        myAudioSource.Play();

        player = playerCol.gameObject;
    }

    public override void OnPlayerEscape(Collider2D playerCol)
    {
        CurrentState = State.Idle;

        player = null;
    }

    protected override void Chasing()
    {
        myRigidBody.gravityScale = 1;

        GetComponent<SpriteRenderer>().sortingOrder = 1;

        if (!(myAudioSource.clip == attackingClip && myAudioSource.isPlaying))
        {
            myAudioSource.clip = attackingClip;
            myAudioSource.loop = true;
            myAudioSource.Play();
        }

        myAnimator.SetBool("IsRunning", true);

        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            if (canMove)
            {
                myRigidBody.velocity = new Vector2(playerDirection.x * speed, myRigidBody.velocity.y);
            }
        }

        if (attackDelay < 0)
        {
            attackZone.SetActive(true);
        }
    }

    public void Attack(Collider2D body)
    {
        myAnimator.SetTrigger("Attack");

        attackZone.SetActive(false);

        attackDelay = 2f;
    }

    private void ReturnToPosition()
    {
        myAudioSource.Stop();

        Vector2 returnDirection = (initialPosition - transform.position).normalized;

        myRigidBody.velocity = returnDirection * speed;

        myAnimator.SetBool("IsRunning", false);

        Debug.Log(transform.position.y);
        Debug.Log(initialPosition.y);

        if (transform.position.y >= initialPosition.y - 0.3f)
        {
            myRigidBody.velocity = Vector2.zero;

            CurrentState = State.Hidden;
        }
    }

    protected override void Dead()
    {
        PlayerDamageZone.damageEnemy -= TakeDamage;

        Destroy(gameObject);
    }

    protected override void HandleAnimation()
    {
        switch (CurrentState)
        {
            case State.Hidden:
                break;
            case State.Idle:
                break;
            case State.Attack:
                break;
            default:
                Debug.LogError("Handle Animatio Error");
                break;
        }
    }

    protected override void HandleAudio()
    {
        switch (CurrentState)
        {
            case State.Hidden:
                break;
            case State.Idle:
                break;
            case State.Attack:
                // StartCoroutine(audioManager.PlaySoundNext(detectedPlayerSoundClip, false, attackingClip, true, myAudioSource));
                break;
            default:
                Debug.LogError("Handle Audio Error");
                break;
        }
    }
}
