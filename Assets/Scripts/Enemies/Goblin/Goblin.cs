using UnityEngine;

public class Goblin : BaseEnemy
{

    [Space]
    [Header("Audio Clips")]
    [SerializeField] private AudioClip idleSoundClip;
    [SerializeField] private AudioClip detectedPlayerSoundClip;
    [SerializeField] private AudioClip attackingClip;

    private AudioSource myAudioSource;

    private enum State
    {
        Patrol,
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

        PlayerDamageZone.damageEnemy += TakeDamage;
    }

    private void Start()
    {
        CurrentState = State.Patrol;

        movementDirection = 1;

        initialPosition = transform.position;

        canMove = true;

    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Patrol:
                Patrolling();
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
        if (transform.position.x < initialPosition.x - patrolDestance)
        {
            movementDirection = 1;
            transform.localScale = new Vector2(1, 1);
            //Debug.Log("move right");

        }
        else if (transform.position.x > initialPosition.x + patrolDestance)
        {
            movementDirection = -1;
            transform.localScale = new Vector2(-1, 1);
            // Debug.Log("move left");
        }
        if (canMove)
        {
            myRigidBody.velocity = new Vector2(speed * movementDirection, myRigidBody.velocity.y);
        }

        if (player)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            // float angle = Vector2.SignedAngle(transform.right, playerDirection);
            float angle = Vector2.SignedAngle(transform.localScale * Vector2.right, playerDirection);

            if (angle <= fieldOfView / 2 && angle >= -fieldOfView / 2)
            {

                if (Physics2D.Raycast(transform.position, playerDirection, 10f, notMeLayer).collider.CompareTag("Player"))
                {

                    CurrentState = State.Attack;
                }
            }
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

    protected override void Chasing()
    {
        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            if (canMove)
            {
                myRigidBody.velocity = new Vector2(playerDirection.x * speed * 1.5f, myRigidBody.velocity.y);
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
                myAnimator.SetBool("isAttacking", false);
                break;
            case State.Attack:
                myAnimator.SetBool("isAttacking", true);
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
            case State.Patrol:
                myAudioSource.loop = true;
                myAudioSource.clip = idleSoundClip;
                myAudioSource.Play();
                break;
            case State.Attack:
                StartCoroutine(audioManager.PlaySoundNext(detectedPlayerSoundClip, false, attackingClip, true, myAudioSource));
                break;
            default:
                Debug.LogError("Handle Audio Error");
                break;
        }
    }
}
