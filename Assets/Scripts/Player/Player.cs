using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    int playerHealth;
    //int playerScore;
    int playerLives;

    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private AudioClip hurtClip;


    void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();

        DamageZone.damagePlayer += TakeDamage;
    }

    void Start()
    {
        playerHealth = 10;
        //playerScore = 0;
        playerLives = 3;
    }

    void FixedUpdate()
    {
        
    }

    void TakeDamage(int damage)
    {
        playerHealth -= damage;

        myAudioSource.clip = hurtClip;
        myAudioSource.loop = false;
        myAudioSource.Play();

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            GameOver();
        }
        else
        {
            PlayerRespawn();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }

    void PlayerRespawn()
    {
        transform.position = GameManager.instance.LastSavePosition;

        playerHealth = 10;
    }
}
