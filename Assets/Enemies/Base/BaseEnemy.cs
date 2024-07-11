using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("Movement")]
    [Range(1,5)] [SerializeField] private int patrolDestance;
    [Range(1,5)] [SerializeField] private int speed;
    [Range(60,90)] [SerializeField] private int fieldOfView;

    [Space]

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask notMeLayer;

    private enum State
    {
        Patrol,
        Return,
        Attack,
        Dead
    }

    private Rigidbody2D myRigidBody;

    private State currentState;
    
    private Vector2 initialPosition;
    private int movementDirection;
    private GameObject player;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentState = State.Patrol;

        movementDirection = 1;

        initialPosition = transform.position;

    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrolling();
                break;
            case State.Attack:
                Attacking();
                Debug.Log("Attacking");
                break;
            case State.Dead:
                Debug.Log("Returing To Dead");
                break;
            default:
                Debug.Log("WTF!!");
                break;
        }

        CheckEdge();
    }

    private void Patrolling()
    {
        if (transform.position.x < initialPosition.x - patrolDestance)
        {
            movementDirection = 1;
            transform.localScale = new Vector2(1,1);
            //Debug.Log("move right");

        } 
        else if (transform.position.x > initialPosition.x + patrolDestance)
        {
            movementDirection = -1;
            transform.localScale = new Vector2(-1,1);
            // Debug.Log("move left");
        }

        myRigidBody.velocity = new Vector2(speed * movementDirection, myRigidBody.velocity.y);
        
    }

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            Vector2 playerDirection = (body.transform.position - transform.position).normalized;

            // float angle = Vector2.SignedAngle(transform.right, playerDirection);
            float angle = Vector2.SignedAngle(transform.localScale * Vector2.right, playerDirection);


            if (angle <= fieldOfView / 2  && angle >= -fieldOfView / 2)
            {
                if (Physics2D.Raycast(transform.position, playerDirection, 10f, notMeLayer).collider.CompareTag("Player"))
                {
                    Debug.Log("Detected");
                    currentState = State.Attack;
                    player = body.gameObject;
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            currentState = State.Patrol;

            player = null;
        }
    }

    private void Attacking()
    {
        if (player != null)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;

            myRigidBody.velocity = new Vector2(playerDirection.x * speed * 1.5f, myRigidBody.velocity.y);
        }
    }

    private void CheckEdge()
    {
        Vector2 playerFront = transform.position + new Vector3(0.2f, 0,2f);
        Debug.DrawLine(playerFront, this.transform.up, Color.red);
    }

}