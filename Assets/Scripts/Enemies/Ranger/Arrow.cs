using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;
    
    private Rigidbody2D myRigidBody;
    private GameObject player;

    private float lifeSpan;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {
        Vector2 playerDirection = player.transform.position - transform.position;
        myRigidBody.velocity = new Vector2(playerDirection.x, playerDirection.y).normalized * arrowSpeed;

        float rotation = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, rotation);

        lifeSpan = 5f;
    }

    private void Update()
    {
        if (lifeSpan < 0)
        {
            Destroy(gameObject);
        }
        lifeSpan -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D body)
    {
        if (body.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
