using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Ranger : BaseEnemy
{
    [Space]
    [Header("Arrow Object")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowInitialPosition;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip detectPlayerClip;
    [SerializeField] private AudioClip shootClip;

    private AudioSource myAudioSource;

    private float attackTimer;

    enum State
    {
        Patrol,
        Attack,
        Dead,
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

        attackTimer = 0f;

    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case State.Patrol:
                Patrolling();
                // Debug.Log("Patrolling");
                break;
            case State.Attack:
                Attack();

                attackTimer -= Time.deltaTime;
                // Debug.Log("Attacking");
                break;
            case State.Dead:
                Debug.Log("Deadge ヽ༼ ಠ益ಠ ༽ﾉ");
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
            //transform.localScale = new Vector2(1, 1);
            //Debug.Log("move right");

        }
        else if (transform.position.x > initialPosition.x + patrolDestance)
        {
            movementDirection = -1;
            //transform.localScale = new Vector2(-1, 1);
            // Debug.Log("move left");
        }

        if (Physics2D.Raycast(transform.position, transform.localScale * Vector2.right, 0.7f, groundLayer))
        {
            movementDirection *= -1;
        }

        if (canMove)
        {
            Flip();
        
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
                    myAudioSource.clip = detectPlayerClip;
                    myAudioSource.loop = false;
                    myAudioSource.Play();

                    CurrentState = State.Attack;
                }
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

    private void Attack()
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        if (CurrentState == State.Attack && attackTimer < 0)
        {
            myAnimator.SetTrigger("Shoot");

            if (transform.localScale.x > 0)
            {
                if (playerDirection.x < 0)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
            }
            else
            {
                if (playerDirection.x > 0)
                {
                    transform.localScale *= new Vector2(-1, 1);
                }
            }

            attackTimer = 1.5f;
        }
    }

    public void ShotArrow()
    {
        myAudioSource.clip = shootClip;
        myAudioSource.loop = false;
        myAudioSource.Play();

        Instantiate(arrow, arrowInitialPosition.position, Quaternion.identity);
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

    protected override void Chasing() { }

    // protected override void TakeDamage(int damage)
    // {
    //     Debug.Log("Damage Enemy by " + damage);
    //     enemyHealth -= damage;

    //     if (enemyHealth <= 0)
    //     {
    //         Dead();
    //     }
    // }

    protected override void Dead()
    {
        PlayerDamageZone.damageEnemy -= TakeDamage;
        
        Destroy(gameObject);
    }

    protected override void HandleAnimation()
    {
        //        throw new System.NotImplementedException();
    }

    protected override void HandleAudio()
    {
        if (CurrentState == State.Patrol)
        {
            myAnimator.SetBool("IsPatrolling", true);
            myAudioSource.clip = walkingClip;
            myAudioSource.loop = true;
            myAudioSource.Play();
        }
        else
        {
            myAnimator.SetBool("IsPatrolling", false);
        }
    }
}
