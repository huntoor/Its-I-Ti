using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Player : MonoBehaviour
{
    private float _speed = 3.5f;
    Rigidbody2D rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private HealthPack healthPack;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(rb.velocity.x, 10f) , ForceMode2D.Impulse);
        }

        //CalculateMovement();

        if (Input.GetKeyDown(KeyCode.T)) {

            TakeDamage(20);
        
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag) {

            case "BronzeCoin":
                Destroy(other.gameObject);
                gameManager.totalScore++;
                break;

            case "SilverCoin":
                Destroy(other.gameObject);
                gameManager.totalScore+=2;
                break;


            case "GoldCoin":
                Destroy(other.gameObject);
                gameManager.totalScore+=3;
                break;

            case "HealthPack":
                Destroy(other.gameObject);
                
                if ((currentHealth < maxHealth) && currentHealth > 0)
                    healthPack.increaseHealth();
                break;





        }
        
        
    }


    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 _direction = new Vector3(horizontalInput, VerticalInput);

        transform.Translate(_direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x <= -11)
        {

            transform.position = new Vector3(11, transform.position.y, 0);

        }

        else if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }

    }


    void TakeDamage(int damage) { 
    
        currentHealth-=damage;
        healthBar.setHealth(currentHealth);

    
    }
}
