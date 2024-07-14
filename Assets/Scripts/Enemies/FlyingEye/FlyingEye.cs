using UnityEngine;

public class FlyingEye : BaseEnemy
{
    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip idleSoundClip;
    [SerializeField] private AudioClip attackingClip;

    [Space]
    [Header("Patrol Zone Points")]
    [SerializeField] private Transform[] PatrolZone;

    private Transform moveToward;
    private AudioSource myAudioSource;

    private float attackTimer;

    private enum State
    {
        Patrol,
        Chasing,
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

            HandleAudio();
            HandleAnimation();
        }
    }

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        myAnimator = GetComponent<Animator>();

        myAudioSource = GetComponent<AudioSource>();

        PlayerDamageZone.damageEnemy += TakeDamage;
    }

    private void Start()
    {
        CurrentState = State.Patrol;

        movementDirection = 1;

        initialPosition = transform.position;

        canMove = true;

        moveToward = PatrolZone[0];

        attackTimer = 1f;
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Patrol:
                Patrolling();
                //Debug.Log("Patrolling");
                return;
            case State.Chasing:
                Chasing();
                // Debug.Log("Chasing");
                return;
            case State.Attack:
                Attack();
                attackTimer -= Time.deltaTime;
                // Debug.Log("Attack");
                return;
            case State.Dead:
                Debug.Log("Deadge ( ͠° ͟ʖ ͡° )");
                break;
            default:
                Debug.LogError("Wierd Current State in FlyingEye.cs");
                break;
        }
    }

    public override void OnPlayerDetected(Collider2D playerCol)
    {
        player = playerCol.gameObject;
    }

    public override void OnPlayerEscape(Collider2D playerCol)
    {
        CurrentState = State.Patrol;

        player = null;
    }

    protected override void Patrolling()
    {
        Vector2 directionToMoveTo = (moveToward.position - transform.position).normalized;

        float distance = Vector2.Distance(moveToward.position, transform.position);

        myRigidBody.velocity = directionToMoveTo * speed;

        Flip();

        if (distance <= 0.5f)
        {
            int randomDirectionInt = Random.Range(0, PatrolZone.Length);

            moveToward = PatrolZone[randomDirectionInt];
        }

        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            if (Physics2D.Raycast(transform.position, playerDirection, 10f, notMeLayer).collider.CompareTag("Player"))
            {
                CurrentState = State.Chasing;
            }
        }
    }

    private void Flip()
    {
        if (transform.localScale.x > 0)
        {
            if (myRigidBody.velocity.x < 0)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
        }
        else
        {
            if (myRigidBody.velocity.x > 0)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
        }
    }

    protected override void Chasing()
    {
        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            myRigidBody.velocity = playerDirection * speed * 1.5f;

            Flip();

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, playerDirection, 10f, playerLayer);

            if (raycastHit2D.distance <= 0.5f)
            {
                CurrentState = State.Attack;
            }
        }
    }

    private void Attack()
    {
        if (CurrentState == State.Attack && !(myAudioSource.clip == attackingClip))
        {
            myAnimator.SetTrigger("Attack");

            myAudioSource.clip = attackingClip;
            myAudioSource.loop = false;
            myAudioSource.Play();
        }
        else
        {
            if (!myAudioSource.isPlaying && attackTimer < 0)
            {
                CurrentState = State.Chasing;

                attackTimer = 1f;
            }
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
            case State.Patrol:
                break;
            case State.Chasing:
                break;
            case State.Attack:
                break;
            case State.Dead:
                myAnimator.SetTrigger("Dead");
                break;
            default:
                Debug.LogError("Something Went Wrong in Flying Eye Animation");
                break;
        }
    }

    protected override void HandleAudio()
    {
        switch (CurrentState)
        {
            case State.Patrol:
                myAudioSource.clip = idleSoundClip;
                myAudioSource.loop = true;
                myAudioSource.Play();
                // Debug.Log(myAudioSource.isPlaying);
                break;
            case State.Chasing:
                myAudioSource.clip = idleSoundClip;
                myAudioSource.loop = true;
                myAudioSource.Play();
                Debug.Log(myAudioSource.isPlaying);
                break;
            case State.Attack:
                break;
            default:
                Debug.LogError("Wierd State in FlyingEye.cs Audio Handling");
                break;
        }
    }
}
