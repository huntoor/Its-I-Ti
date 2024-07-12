using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Movement")]
    [Range(1, 5)][SerializeField] protected int patrolDestance;
    [Range(1, 5)][SerializeField] protected int speed;
    [Range(60, 90)][SerializeField] protected int fieldOfView;

    [Space]

    [Header("Layers")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask notMeLayer;

    [Space]

    [Header("Audio Manager")]
    [SerializeField] protected AudioManager audioManager;

    protected Rigidbody2D myRigidBody;
    protected Animator myAnimator;

    protected Vector2 initialPosition;
    protected GameObject player;
    protected int movementDirection;
    protected bool canMove;

    protected virtual bool CheckEdge()
    {
        Vector2 enemyFront = transform.position + new Vector3(transform.localScale.x * 0.5f, 0f);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(enemyFront, Vector2.down, 1f, groundLayer);

        return raycastHit2D.collider != null;
    }

    protected abstract void Patrolling();

    protected abstract void Chasing();

    protected abstract void HandleAnimation();

    protected abstract void HandleAudio();

    public abstract void OnPlayerDetected(Collider2D playerCol);

    public abstract void OnPlayerEscape(Collider2D playerCol);

    //protected abstract void OnTriggerExit2D(Collider2D body);

    //protected abstract void OnTriggerEnter2D(Collider2D body);
}